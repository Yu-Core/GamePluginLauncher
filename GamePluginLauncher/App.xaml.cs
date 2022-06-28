using GamePluginLauncher.Model;
using GamePluginLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GamePluginLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (GetStartedProcess() is Process p)
            {
                WinApi.SendMessage(p.MainWindowHandle, 0x0400, default, default);
                Environment.Exit(0);
            }
            else
            {
                base.OnStartup(e);
                StaticData.InitStaticData();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            StaticData.SaveStaticData();
        }

        private static Process GetStartedProcess()
        {
            Process cur = Process.GetCurrentProcess();
            return (from p in Process.GetProcesses() where p.ProcessName == cur.ProcessName && p.Id != cur.Id select p).FirstOrDefault();
        }
    }
}
