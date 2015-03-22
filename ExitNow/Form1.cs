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
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ExitNow/";
        public static string pidfile = "exitnow.pid";
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(path) == false)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            if(System.IO.File.Exists(path + pidfile))
            {
                Int32 pid = Convert.ToInt32(System.IO.File.ReadAllText(path + pidfile));
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

                Settings.LoadConfig();
                LoadHistory();

                if (Settings.history == 0)
                {
                    panel1.Visible = false;

                    Size = new Size(409, 75);
                }

                Update();

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

                System.IO.File.WriteAllText(path + pidfile, "" + Process.GetCurrentProcess().Id);

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
                if(System.IO.File.Exists(path + pidfile)) {
                    System.IO.File.Delete(path + pidfile);
                }
            }
        }

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
                                    notifyIcon1.BalloonTipText = GetActiveProcess().ProcessName + " was killed.";
                                    notifyIcon1.ShowBalloonTip(5);
                                    AddToHistory(GetActiveProcess());
                                    GetActiveProcess().Kill();
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
                                        AddToHistory(GetActiveProcess());
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

        public void AddToHistory(Process p)
        {
            string name = p.ProcessName;
            string historyfile = "history.txt";

            listView1.Items.Add(name);
            if (System.IO.File.Exists(path + historyfile) == false)
            {
                System.IO.File.WriteAllText(path + historyfile, "");
            }

            if (Settings.history == 1)
            {
                string inhalt = System.IO.File.ReadAllText(path + historyfile);
                inhalt = name + Environment.NewLine + inhalt;
                System.IO.File.WriteAllText(path + historyfile, inhalt);
            }
        }

        public void LoadHistory()
        {
            string historyfile = "history.txt";
            if (System.IO.File.Exists(path + historyfile))
            {
                String[] lines = System.IO.File.ReadAllLines(path + historyfile);
                foreach (String line in lines)
                {
                    listView1.Items.Add(line);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Settings().Show();
        }

        public void Update()
        {
            Uri versionuri = new Uri("http://scrumplex.cloudza.org/ExitNow/update/version.txt");

            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                System.Net.WebClient web = new System.Net.WebClient();
                int newv = Convert.ToInt16(web.DownloadString(versionuri));
                int thisv = Convert.ToInt16(Properties.Resources.VersionID);
                if (newv > thisv)
                {
                    //UPDATE DA
                    Process.Start("http://scrumplex.cloudza.org/ExitNow/update/");
                    Application.Exit();
                }
                else if (newv < thisv)
                {
                    //DEV
                    MessageBox.Show("Developer Version");
                }
            }
        }
    }
}
