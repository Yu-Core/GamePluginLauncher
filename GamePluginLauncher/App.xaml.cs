using GamePluginLauncher.Model;
using GamePluginLauncher.Utils;
using GamePluginLauncher.View;
using GamePluginLauncher.View.Dialogs;
using MaterialDesignThemes.Wpf;
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
            //判断程序是否已经打开
            if (GetStartedProcess() is Process p)
            {
                WinApi.SendMessage(p.MainWindowHandle, 0x0400, default, default);
                Environment.Exit(0);
            }
            else
            {
                base.OnStartup(e);
                //初始化静态数据
                StaticData.InitStaticData();
                //判断是否有启动参数（直接打开是没有启动参数的，如果通过桌面创建的图标打开会有参数，会打开控制台程序，进而打开该程序）
                if (e.Args.Length > 0)
                {
                    try
                    {
                        var LauncherId = Convert.ToInt32(e.Args[0]);
                        var Has = StaticData.GameLaunchers.Where(x => x.Id == LauncherId).Any();
                        if(!Has)
                        {
                            MsgBoxHelper.ShowError("该管理器不存在");
                            Environment.Exit(0);
                        }
                        //根据参数，启动相应的管理器
                        var pluginSelector = new PluginSelector()
                        {
                            LauncherId = Convert.ToInt32(e.Args[0])
                        };
                        pluginSelector.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Environment.Exit(0);
                    }
                }
                else
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                }
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
            //return (from p in Process.GetProcesses() where p.ProcessName == cur.ProcessName && p.Id != cur.Id select p).FirstOrDefault();
            return Process.GetProcesses().FirstOrDefault(it => it.ProcessName == cur.ProcessName && it.Id != cur.Id);
        }
    }
}
