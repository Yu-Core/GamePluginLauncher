using GamePluginLauncher.Model;
using GamePluginLauncher.Utils;
using GamePluginLauncher.View;
using GamePluginLauncher.View.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using SimpleMvvm;
using SimpleMvvm.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IW = IWshRuntimeLibrary;

namespace GamePluginLauncher.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        public DelegateCommand? NewGameLauncherCommand { get; set; }
        public DelegateCommand? RemoveGameLauncherCommand { get; set; }
        public DelegateCommand? EditGameLauncherCommand { get; set; }
        public DelegateCommand? RenameGameLauncherCommand { get; set; }
        public DelegateCommand? ShowInExplorerCommand { get; set; }
        public DelegateCommand? AddGamePluginCommand { get; set; }
        public DelegateCommand? OpenGameLauncherCommand { get; set; }
        public DelegateCommand? EditGameLauncherPathCommand { get; set; }
        public DelegateCommand? ViewSourceCommand { get; set; }
        public DelegateCommand? ShowAboutCommand { get; set; }
        public DelegateCommand? ThanksCommand { get; set; }
        public DelegateCommand? CloseWindowCommand { get; set; }
        public DelegateCommand? CreateDesktopShortcutsCommand { get; set; }

        private void NewGameLauncher(object obj)
        {
            try
            {
                var ofd = new OpenFileDialog
                {
                    Filter = "应用程序|*.exe"
                };
                if (ofd.ShowDialog() == true)
                {
                    StaticData.AddGameLauncher(ofd.FileName);
                }
            }
            catch (Exception e)
            {
                MsgBoxHelper.ShowError(e.Message);
            }
        }

        private async void RemoveGameLauncher(GameLauncher gameLauncher)
        {
            //创建对话框用户控件
            var dialog = new RemoveLauncherDialog();
            //打开对话框，接收关闭返回值
            var result = await DialogHost.Show(dialog);
            //判断是否确定
            if (Convert.ToBoolean(result))
            {
                StaticData.GameLaunchers.Remove(gameLauncher);
            }
        }

        private async void EditGameLauncher(GameLauncher gameLauncher)
        {
            var dialog = new EditLauncherDialog()
            {
                DataContext = gameLauncher
            };
            var result = await DialogHost.Show(dialog);
            if (Convert.ToBoolean(result))
            {
                if (string.IsNullOrWhiteSpace(dialog.LauncherName.Text))
                {
                    //MsgBoxHelper.ShowError("名称不能为空。");
                    var dialog2 = new MessageDialog("名称不能为空");
                    await DialogHost.Show(dialog2);
                    return;
                }
                if (!File.Exists(dialog.LauncherPath.Text) || PathHelper.GetSuffix(dialog.LauncherPath.Text).ToUpper() != "EXE")
                {
                    //MsgBoxHelper.ShowError("文件不存在或不支持。");
                    var dialog3 = new MessageDialog("文件不存在或不支持");
                    await DialogHost.Show(dialog3);
                    return;
                }
                gameLauncher.Name = dialog.LauncherName.Text;
                gameLauncher.GamePlugins[0].Path = dialog.LauncherPath.Text;
            }
        }
        private async void RenameGameLauncher(GameLauncher gameLauncher)
        {
            var dialog = new RenameLauncherDialog()
            {
                DataContext = gameLauncher
            };
            var result = await DialogHost.Show(dialog);
            if (Convert.ToBoolean(result))
            {
                gameLauncher.Name = dialog.LauncherName.Text;
            }
        }
        private void ShowInExplorer(GameLauncher gameLauncher)
        {
            Process.Start(new ProcessStartInfo("Explorer.exe")
            {
                Arguments = "/e,/select," + gameLauncher.GamePlugins[0].Path
            });
        }
        private void AddGamePlugin(GameLauncher gameLauncher)
        {
            try
            {
                var ofd = new OpenFileDialog
                {
                    Filter = "应用程序|*.exe"
                };
                if (ofd.ShowDialog() == true)
                {
                    gameLauncher.AddPlugin(ofd.FileName);
                }
            }
            catch (Exception e)
            {
                MsgBoxHelper.ShowError(e.Message);
            }
        }
        private void OpenGameLauncher(object LauncherId)
        {
            var pluginSelector = new PluginSelector()
            {
                LauncherId = (int)LauncherId
            };
            pluginSelector.ShowDialog();
        }
        private void EditGameLauncherPath(object obj)
        {
            try
            {
                var ofd = new OpenFileDialog
                {
                    Filter = "应用程序|*.exe"
                };
                if (ofd.ShowDialog() == true)
                {
                    ((TextBox)obj).Text = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                MsgBoxHelper.ShowError(ex.Message);
            }
        }
        private void ViewSource(object obj)
        {
            ProcessStartInfo info = new ProcessStartInfo("https://github.com/Yu-Core/GamePluginLauncher")
            {
                UseShellExecute = true
            };
            Process.Start(info)?.Dispose();
        }

        private async void ShowAbout(object obj)
        {
            var assemblyName = System.Reflection.Assembly.GetEntryAssembly()?.GetName();
            var dialog = new MessageDialog($"{assemblyName.Name} v{assemblyName.Version} \nBy Yu-Core","关于");
            await DialogHost.Show(dialog);
        }
        private async void Thanks(object obj)
        {
            var assemblyName = System.Reflection.Assembly.GetEntryAssembly()?.GetName();
            var dialog = new MessageDialog("感谢以下项目的帮助\n\nhttps://github.com/Mzying2001/AppLauncher_WPF\n\nhttps://github.com/DuelWithSelf/WPFEffects", "鸣谢");
            await DialogHost.Show(dialog);
        }
        private void CloseWindow(object obj)
        {
            ((Window)obj).Close();
        }
        public async void CreateDesktopShortcuts(GameLauncher gameLauncher)
        {
            string LnkName = $"{gameLauncher.Name}启动器";
            string GamePath = gameLauncher.GamePlugins[0].Path;
            string Arguments = $"{GamePath} {gameLauncher.Id.ToString()}";

            string shortcutPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), LnkName + ".lnk");
            // 确定是否已创建快捷方式
            if (System.IO.File.Exists(shortcutPath))
            {
                var dialog = new MessageDialog("快捷方式已存在，请勿重复创建");
                await DialogHost.Show(dialog);
                return;
            }
            //string AppName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
            // 获取当前应用程序目录地址
            string exePath = AppDomain.CurrentDomain.BaseDirectory + "LauncherAssist" + ".exe";
            IW.IWshShell shell = new IW.WshShell();
            
            //foreach (var item in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "*.lnk"))
            //{
            //    IW.WshShortcut tempShortcut = (IW.WshShortcut)shell.CreateShortcut(item);
            //    if (tempShortcut.TargetPath == exePath&&tempShortcut.Arguments == Arguments&& tempShortcut.Description == GamePath)
            //    {
            //        var dialog = new MessageDialog("快捷方式已存在，请勿重复创建");
            //        await DialogHost.Show(dialog);
            //        return;
            //    }
            //}
            IW.WshShortcut shortcut = (IW.WshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = exePath;//应用程序路径
            shortcut.Arguments = Arguments;// 参数  
            shortcut.Description = $"{GamePath}";//描述
            shortcut.WorkingDirectory = Environment.CurrentDirectory;//程序所在文件夹，在快捷方式图标点击右键可以看到此属性  
            shortcut.IconLocation = GamePath;//图标，该图标是应用程序的资源文件  
            shortcut.WindowStyle = 1;
            shortcut.Save();
            var dialog2 = new MessageDialog("桌面快捷方式已创建");
            await DialogHost.Show(dialog2);
        }
        protected override void Init()
        {
            base.Init();

            NewGameLauncherCommand = new DelegateCommand(NewGameLauncher);
            RemoveGameLauncherCommand = new DelegateCommand<GameLauncher>(RemoveGameLauncher);
            EditGameLauncherCommand = new DelegateCommand<GameLauncher>(EditGameLauncher);
            RenameGameLauncherCommand = new DelegateCommand<GameLauncher>(RenameGameLauncher);
            ShowInExplorerCommand = new DelegateCommand<GameLauncher>(ShowInExplorer);
            AddGamePluginCommand = new DelegateCommand<GameLauncher>(AddGamePlugin);
            OpenGameLauncherCommand = new DelegateCommand(OpenGameLauncher);
            EditGameLauncherPathCommand = new DelegateCommand(EditGameLauncherPath);
            ViewSourceCommand = new DelegateCommand(ViewSource);
            ShowAboutCommand = new DelegateCommand(ShowAbout);
            ThanksCommand = new DelegateCommand(Thanks);
            CloseWindowCommand = new DelegateCommand(CloseWindow);
            CreateDesktopShortcutsCommand = new DelegateCommand<GameLauncher>(CreateDesktopShortcuts);
        }
    }
}
