using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace GamePluginLauncher.Utils
{
    public static class WindowHelper
    {
        public static void SetForeground(Window window)
        {
            var wndPid = WinApi.GetWindowThreadProcessId(new WindowInteropHelper(window).Handle, default);
            var curPid = WinApi.GetWindowThreadProcessId(WinApi.GetForegroundWindow(), default);

            WinApi.AttachThreadInput(curPid, wndPid, true);
            window.Show();
            window.Activate();
            WinApi.AttachThreadInput(curPid, wndPid, false);
        }
    }
}
