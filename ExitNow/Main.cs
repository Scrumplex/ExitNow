using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ExitNow
{
    public partial class Main : Form
    {
        public List<string> WhitelistedProcesses = new List<string>();
        private bool _autostart;

        public Main()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Program.Hotkeys.Enable(Handle, Hotkeys.Modifiers.Alt, Keys.F3);

            LoadHistory();
            InitializeMenuItems();

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                if (args[1] == "autorun")
                {
                    trayIcon.BalloonTipText = @"ExitNow is running in the background!";
                    trayIcon.ShowBalloonTip(5);
                    _autostart = true;
                }
            }

            //Whitelist
            WhitelistedProcesses.Add("explorer");
            WhitelistedProcesses.Add("taskmgr");
            WhitelistedProcesses.Add(Process.GetCurrentProcess().ProcessName);
        }

        private void OnExit(object sender, FormClosingEventArgs e)
        {
            Program.Hotkeys.UnhookAll();
            //Let's just appreciate that it works and let LINQ do it's thing.
            var contents = listViewHistory.Items.Cast<ListViewItem>().Select(item => item.Text).ToList().Aggregate("", (current, item) => current + (item + "\n"));

            File.WriteAllText(Program.BasePath + Program.HistoryFile, contents);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!_autostart) return;
            Hide();
            _autostart = false;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            var activeProcess = ProcessManager.GetActiveProcess();

            if (m.Msg == Hotkeys.WmHotkeyMsgId)
            {
                if ((int) m.WParam == 0)
                {
                    if (WhitelistedProcesses.Contains(activeProcess.ProcessName) == false)
                    {
                        KillProcess(activeProcess);
                    }
                    else
                    {
                        var processName = activeProcess.ProcessName;
                        if (
                            MessageBox.Show($@"Do you want to kill {processName}?", @"ExitNow",
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            KillProcess(activeProcess);
                        }
                    }
                }
            }
            base.WndProc(ref m);
        }

        private void KillProcess(Process p)
        {
            try
            {
                p.Kill();
                trayIcon.BalloonTipText = p.ProcessName + @" has been killed.";
                trayIcon.ShowBalloonTip(5);

                listViewHistory.Items.Add(p.ProcessName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), @"ExitNow");
            }
        }

        public void LoadHistory()
        {
            if (!File.Exists(Program.BasePath + Program.HistoryFile)) return;
            var lines = File.ReadAllLines(Program.BasePath + Program.HistoryFile);
            foreach (var line in lines)
            {
                listViewHistory.Items.Add(line);
            }
        }

        public void InitializeMenuItems()
        {
            const string subkey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
            var key = Registry.CurrentUser.OpenSubKey(subkey, true);
            if (key == null)
                return;

            var expectedValue = "\"" + Application.ExecutablePath + "\" autorun";
            var value = key.GetValue(Application.ProductName);
            if (value != null)
            {
                if (!value.Equals(expectedValue))
                {
                    key.DeleteValue(Application.ProductName);
                    key.SetValue(Application.ProductName, expectedValue);
                }
                autostartItem.Checked = true;
            }
            key.Close();
            if (Program.IsAdministrator())
            {
                restartAsAdminItem.Visible = false;
            }
        }

        //Menu Items

        private void OnInfoItemClick(object sender, EventArgs e)
        {
            new About().Show();
        }

        private void OnHideItemClick(object sender, EventArgs e)
        {
            Hide();
            trayIcon.BalloonTipText = @"ExitNow is now hidden";
            trayIcon.ShowBalloonTip(5);
        }

        private void OnExitItemClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnAutostartItemClick(object sender, EventArgs e)
        {
            const string subkey = @"Software\Microsoft\Windows\CurrentVersion\Run";
            var key = Registry.CurrentUser.OpenSubKey(subkey, true);
            if (key == null)
                return;

            if (!autostartItem.Checked)
            {
                var value = "\"" + Application.ExecutablePath + "\" autorun";
                key.SetValue(Application.ProductName, value);
                autostartItem.Checked = true;
            }
            else
            {
                key.DeleteValue(Application.ProductName);
                autostartItem.Checked = false;
            }
            key.Close();
        }

        private void OnRestartAsAdminItemClick(object sender, EventArgs e)
        {
            var info = new ProcessStartInfo(Application.ExecutablePath) {Verb = "runas"};
            Program.DeletePidFile();
            try
            {
                Process.Start(info);
            }
            catch (Exception)
            {
                Program.CreatePidFile();
            }
            Application.Exit();
        }

        private void OnTrayIconClick(object sender, EventArgs e)
        {
            Show();
        }
    }
}