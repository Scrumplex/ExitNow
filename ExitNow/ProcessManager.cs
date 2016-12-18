using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ExitNow
{
    internal class ProcessManager
    {
        [DllImport("user32.dll")]
        public static extern int GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);

        public static int GetWindowProcessId(int hwnd)
        {
            int pid;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        public static Process GetActiveProcess()
        {
            var hwnd = GetForegroundWindow();
            return Process.GetProcessById(GetWindowProcessId(hwnd));
        }
    }
}
