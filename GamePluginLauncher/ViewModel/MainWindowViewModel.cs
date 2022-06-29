﻿using GamePluginLauncher.Model;
using GamePluginLauncher.Utils;
using GamePluginLauncher.View.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using SimpleMvvm;
using SimpleMvvm.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GamePluginLauncher.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        //public ObservableCollection<GameLauncher> GameLaunchers { get; set; }
        //public MainWindowViewModel()
        //{
        //    GameLaunchers = new ObservableCollection<GameLauncher>()
        //    {
        //        new GameLauncher() { Name = "LOL" },
        //        new GameLauncher() { Name = "CF" },
        //        new GameLauncher() { Name = "DNF" },
        //        new GameLauncher() { Name = "LSCS" }
        //    };
        //}

        

        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set => UpdateValue(ref _windowState, value);
        }

        private int _appListListBoxSelectedIndex;
        public int AppListListBoxSelectedIndex
        {
            get => _appListListBoxSelectedIndex;
            set => UpdateValue(ref _appListListBoxSelectedIndex, value);
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
            }) ;
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
        }
    }
}
