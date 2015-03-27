using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace ExitNow
{
    public partial class Settings : Form
    {

        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ExitNow/";
        private static string configfile = "config.xml";
        private static string historyfile = "history.txt";

        public static int history;
        public static int autoup;
        public static int upserver;

        public Settings()
        {
            InitializeComponent();
        }
        
        public void Settings_Load(object sender, EventArgs e)
        {
            ApplyConfig();
        }

        public static void LoadConfig()
        {
            if (System.IO.File.Exists(path + configfile))
            {
                XmlDocument config = new XmlDocument();
                config.Load(path + configfile);
                try
                {
                    history = Convert.ToInt16(config.DocumentElement.SelectSingleNode("/settings/history").InnerText);
                    autoup = Convert.ToInt16(config.DocumentElement.SelectSingleNode("/settings/update/auto").InnerText);
                    upserver = Convert.ToInt16(config.DocumentElement.SelectSingleNode("/settings/update/server").InnerText);
                }
                catch (Exception ex)
                {
                    System.IO.File.Delete(path + configfile);
                    MessageBox.Show("Config not compatible");
                    history = 0;
                    autoup = 0;
                    upserver = 0;
                }
            }
            else 
            {
                history = 0;
                autoup = 0;
                upserver = 0;
            }
        }

        public void ApplyConfig()
        {
            comboBox4.SelectedIndex = history;
            comboBox1.SelectedIndex = autoup;
            comboBox2.SelectedIndex = upserver;

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
                comboBox3.SelectedIndex = 0;
            }
            else
            {
                comboBox3.SelectedIndex = 1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            XElement settings =
                new XElement("settings",
                    new XElement("history", comboBox4.SelectedIndex),
                    new XElement("update",
                        new XElement("auto", comboBox1.SelectedIndex),
                        new XElement("server", comboBox2.SelectedIndex)
                    )
                );
            settings.Save(path + configfile);

            if (comboBox3.SelectedIndex == 1)
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

            System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/exitnow.pid");
            Process.Start(Application.ExecutablePath);
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.File.Delete(path + historyfile);
        }
    }
}
