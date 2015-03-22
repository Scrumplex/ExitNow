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
                history = Convert.ToInt16(config.DocumentElement.SelectSingleNode("/settings/history").InnerText);
            }
            else 
            {
                history = 0;
            }
        }

        public void ApplyConfig()
        {
            domainUpDown1.SelectedIndex = history;
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            history = domainUpDown1.SelectedIndex;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            XElement settings =
                new XElement("settings",
                    new XElement("history", history)
                );
            settings.Save(path + configfile);
            System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/exitnow.pid");
            Process.Start(Application.ExecutablePath);
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.File.Delete(path + historyfile);
        }
    }
}
