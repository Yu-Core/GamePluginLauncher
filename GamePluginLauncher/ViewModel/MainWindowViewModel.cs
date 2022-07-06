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

namespace GamePluginLauncher.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set => UpdateValue(ref _windowState, value);
        }

        private bool _windowTopmost;
        public bool WindowTopmost
        {
            get => _windowTopmost;
            set => UpdateValue(ref _windowTopmost, value);
        }

        private bool _minimizeWindowAfterOpening;
        public bool MinimizeWindowAfterOpening
        {
            get => _minimizeWindowAfterOpening;
            set => UpdateValue(ref _minimizeWindowAfterOpening, value);
        }

        private bool _showOpenErrorMsg;
        public bool ShowOpenErrorMsg
        {
            get => _showOpenErrorMsg;
            set => UpdateValue(ref _showOpenErrorMsg, value);
        }

        public DelegateCommand? NewGameLauncherCommand { get; set; }
        public DelegateCommand? RemoveGameLauncherCommand { get; set; }
        public DelegateCommand? EditGameLauncherCommand { get; set; }
        public DelegateCommand? RenameGameLauncherCommand { get; set; }
        public DelegateCommand? ShowInExplorerCommand { get; set; }
        public DelegateCommand? AddGamePluginCommand { get; set; }
        public DelegateCommand? OpenGameLauncherCommand { get; set; }
        public DelegateCommand? EditGameLauncherPathCommand { get; set; }

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
                    var dialog2 = new ErrorMessageDialog() { DataContext = "名称不能为空" };
                    await DialogHost.Show(dialog2);
                    return;
                }
                if (!File.Exists(dialog.LauncherPath.Text) || PathHelper.GetSuffix(dialog.LauncherPath.Text).ToUpper() != "EXE")
                {
                    //MsgBoxHelper.ShowError("文件不存在或不支持。");
                    var dialog3 = new ErrorMessageDialog() { DataContext = "文件不存在或不支持" };
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
        private void OpenGameLauncher(object id)
        {
            var pluginSelector = new PluginSelector()
            {
                LauncherID = (int)id
            };
            pluginSelector.ShowDialog();
            //测试使用
            //var dialog = new ErrorMessageDialog()
            //{
            //    DataContext = $"进入了第{index}个启动器"
            //};
            //await DialogHost.Show(dialog);
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
        protected override void Init()
        {
            base.Init();

            var config = StaticData.Config;
            //AppListListBoxSelectedIndex = config.AppListListBoxSelectedIndex;
            WindowTopmost = config.MainWindowTopmost;
            MinimizeWindowAfterOpening = config.MinimizeMainWindowAfterOpening;
            ShowOpenErrorMsg = config.ShowOpenErrorMessage;

            NewGameLauncherCommand = new DelegateCommand(NewGameLauncher);
            RemoveGameLauncherCommand = new DelegateCommand<GameLauncher>(RemoveGameLauncher);
            EditGameLauncherCommand = new DelegateCommand<GameLauncher>(EditGameLauncher);
            RenameGameLauncherCommand = new DelegateCommand<GameLauncher>(RenameGameLauncher);
            ShowInExplorerCommand = new DelegateCommand<GameLauncher>(ShowInExplorer);
            AddGamePluginCommand = new DelegateCommand<GameLauncher>(AddGamePlugin);
            OpenGameLauncherCommand = new DelegateCommand(OpenGameLauncher);
            EditGameLauncherPathCommand = new DelegateCommand(EditGameLauncherPath);
        }
    }
}
