using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExitNow
{
    internal class Hotkeys
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int WmHotkeyMsgId = 0x0312;

        private readonly List<HookInfo> _hookedKeys;

        private int _freeId;

        public struct HookInfo
        {
            public int Id;

            public IntPtr Hwnd;

            public HookInfo(IntPtr hwnd, int id)
            {
                Hwnd = hwnd;
                Id = id;
            }
        }

        [Flags]
        public enum Modifiers
        {
            Win = 8,
            Shift = 4,
            Ctrl = 2,
            Alt = 1,
            None = 0
        }

        public Hotkeys()
        {
            _hookedKeys = new List<HookInfo>();
            _freeId = 0;
        }

        ~Hotkeys()
        {
            UnhookAll();
        }

        public void UnhookAll()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < _hookedKeys.Count; i++)
            {
                Disable(_hookedKeys[i]);
            }
        }

        public HookInfo Enable(IntPtr handle, Modifiers mod, Keys key)
        {
            var i = new HookInfo(handle, _freeId++);
            _hookedKeys.Add(i);
            RegisterHotKey(handle, i.Id, (int) mod, (int) key);
            return i;
        }

        public void Disable(HookInfo hInfo)
        {
            RemoveHook(hInfo);
            UnregisterHotKey(hInfo.Hwnd, hInfo.Id);
        }

        private void RemoveHook(HookInfo hInfo)
        {
            for (var i = 0; i < _hookedKeys.Count; i++)
            {
                if (_hookedKeys[i].Hwnd == hInfo.Hwnd && _hookedKeys[i].Id == hInfo.Id)
                {
                    _hookedKeys.RemoveAt(i--);
                }
            }
        }
    }
}