using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace ExitNow
{

    public partial class Form1 : Form
    {

        public static List<String> blacklist = new List<String>();
        public static Boolean running;


        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);

        private Int32 GetWindowProcessID(Int32 hwnd)
        {
            Int32 pid = 1;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        private Process GetActiveProcess()
        {
            Int32 hwnd = 0;
            hwnd = GetForegroundWindow();
            return Process.GetProcessById(GetWindowProcessID(hwnd));
        }
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/exitnow.pid"))
            {
                Int32 pid = Convert.ToInt32(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/exitnow.pid"));
                Process[] ps = Process.GetProcesses();

                foreach (Process p in ps)
                {
                    if (p.Id == pid)
                    {
                        MessageBox.Show("Already running with PID " + pid, "ExitNow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        running = true;
                        Application.Exit();
                        break;
                    }
                }
            }

            if (running == false)
            {
                Hotkeys HK = new Hotkeys();
                HK.unhookAll();
                HK.enable(this.Handle, Hotkeys.Modifiers.Alt, Keys.F3);

                string[] args = Environment.GetCommandLineArgs();
                if (args.Length == 2)
                {
                    if (args[1] == "autorun")
                    {
                        timer1.Start();
                        notifyIcon1.BalloonTipText = "ExitNow is now hidden";
                        notifyIcon1.ShowBalloonTip(5);
                        settingsToolStripMenuItem.Visible = true;
                    }
                }

                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/exitnow.pid", "" + Process.GetCurrentProcess().Id);

                //Blacklist
                blacklist.Add("explorer");
                blacklist.Add("taskmgr");
                blacklist.Add(Process.GetCurrentProcess().ProcessName);

                //reg                
                string subkey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
                RegistryKey key = Registry.CurrentUser.OpenSubKey(subkey, true);
                object value = null;
                try
                {
                    value = key.GetValue(Application.ProductName);
                }
                catch (Exception ex)
                {

                }
                key.Close();
                if (value == null)
                {
                    notifyIcon1.BalloonTipText = "Autostart not enabled :/";
                    notifyIcon1.ShowBalloonTip(5);
                    autostartToolStripMenuItem.Checked = false;
                }
                else
                {
                    autostartToolStripMenuItem.Checked = true;
                }
            }
        }

        private void Form1_Exit(object sender, FormClosingEventArgs e)
        {
            if (running == false)
            {
                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/exitnow.pid");
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Hotkeys.WM_HOTKEY_MSG_ID:
                    //Für spatere Hotkeys :)
                    switch ((int)m.WParam)
                    {
                        case 0:
                            if (blacklist.Contains(GetActiveProcess().ProcessName) == false)
                            {
                                try
                                {
                                    GetActiveProcess().Kill();
                                    notifyIcon1.BalloonTipText = GetActiveProcess().ProcessName + " was killed.";
                                    notifyIcon1.ShowBalloonTip(5);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString(), "ExitNow");
                                }
                            }
                            else
                            {
                                if (MessageBox.Show("Do you want to kill " + GetActiveProcess().ProcessName + "?", "ExitNow", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        GetActiveProcess().Kill();
                                        notifyIcon1.BalloonTipText = GetActiveProcess().ProcessName + " was killed.";
                                        notifyIcon1.ShowBalloonTip(5);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString(), "ExitNow");
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            settingsToolStripMenuItem.Visible = true;
            notifyIcon1.BalloonTipText = "ExitNow is now hidden";
            notifyIcon1.ShowBalloonTip(5);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            settingsToolStripMenuItem.Visible = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutform = new About();
            aboutform.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Hide();
        }

        private void autostartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (autostartToolStripMenuItem.Checked == true)
            {
                string value = "\"" + Application.ExecutablePath + "\" autorun";
                string subkey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
                RegistryKey key = Registry.CurrentUser.OpenSubKey(subkey, true);
                key.SetValue(Application.ProductName, value);
                key.Close();
            }
            else
            {
                string subkey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
                RegistryKey key = Registry.CurrentUser.OpenSubKey(subkey, true);
                key.DeleteValue(Application.ProductName);
                key.Close();
            }

        }
    }
}
