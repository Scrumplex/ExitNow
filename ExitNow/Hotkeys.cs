using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ExitNow
{
    public class Hotkeys
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int WM_HOTKEY_MSG_ID = 0x0312;

        private List<HookInfo> hookedKeys;

        private int freeID;

        public struct HookInfo
        {
            public int ID;

            public IntPtr hWnd;

            public HookInfo(IntPtr Handle, int id)
            {
                ID = id;
                hWnd = Handle;
            }
        }

        [Flags]
        public enum Modifiers : int
        {
            Win = 8,
            Shift = 4,
            Ctrl = 2,
            Alt = 1,
            None = 0
        }

        public HookInfo[] HookedKeys
        {
            get
            {
                return hookedKeys.ToArray();
            }
        }

        public Hotkeys()
        {
            hookedKeys = new List<HookInfo>();
            freeID = 0;
        }
        ~Hotkeys()
        {
            unhookAll();
        }

        public void unhookAll()
        {
            for (int i = 0; i < hookedKeys.Count; i++)
            {
                disable(hookedKeys[i]);
            }
        }

        public HookInfo enable(IntPtr Handle, Modifiers Mod, Keys Key)
        {
            HookInfo i = new HookInfo(Handle, freeID++);
            hookedKeys.Add(i);
            RegisterHotKey(Handle, i.ID, (int)Mod, (int)Key);
            return i;
        }

        public void disable(HookInfo i)
        {
            RemoveHook(i);
            UnregisterHotKey(i.hWnd, i.ID);
        }

        private void RemoveHook(HookInfo hInfo)
        {
            for (int i = 0; i < hookedKeys.Count; i++)
            {
                if (hookedKeys[i].hWnd == hInfo.hWnd && hookedKeys[i].ID == hInfo.ID)
                {
                    hookedKeys.RemoveAt(i--);
                }
            }
        }
    }
}
