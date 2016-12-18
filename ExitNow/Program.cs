using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;

namespace ExitNow
{
    internal static class Program
    {
        public static Hotkeys Hotkeys;

        public static string BasePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                        @"\ExitNow";

        public static readonly string PidFile = BasePath + @"\exitnow.pid";
        public static readonly string HistoryFile = @"\history.txt";

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Create Data Directory
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            //Check PID File
            if (File.Exists(PidFile))
            {
                var pid = Convert.ToInt32(File.ReadAllText(PidFile));
                var processes = Process.GetProcesses();

                if (processes.Any(p => p.Id == pid))
                {
                    MessageBox.Show($@"Already running with PID {pid}!", Application.ProductName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            //Create PID File
            CreatePidFile();

            Hotkeys = new Hotkeys();

            Application.Run(new Main());
        }

        public static void CreatePidFile()
        {
            File.WriteAllText(PidFile, Process.GetCurrentProcess().Id.ToString());
        }

        public static void DeletePidFile()
        {
            //Delete PID File
            if (File.Exists(PidFile))
            {
                File.Delete(PidFile);
            }
        }

        public static bool IsAdministrator()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}