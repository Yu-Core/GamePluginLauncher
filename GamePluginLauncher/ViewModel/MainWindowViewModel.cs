using GamePluginLauncher.Model;
using GamePluginLauncher.Utils;
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
        public MainWindowViewModel()
        {
            //GameLaunchers = new ObservableCollection<GameLauncher>()
            //{
            //    new GameLauncher() { Name = "LOL" },
            //    new GameLauncher() { Name = "CF" },
            //    new GameLauncher() { Name = "DNF" },
            //    new GameLauncher() { Name = "LSCS" }
            //};
        }

        

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

        public DelegateCommand NewGameLauncherCommand { get; set; }

        private void NewGameLauncher(object obj)
        {
            //InputTextDialog.ShowDialog(name =>
            //{
            //    StaticData.AddAppList(name);
            //    AppListListBoxSelectedIndex = StaticData.AppLists.Count - 1;
            //}, "新分类", "请输入新分类的名称：", "新分类");

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

        protected override void Init()
        {
            base.Init();

            var config = StaticData.Config;
            //AppListListBoxSelectedIndex = config.AppListListBoxSelectedIndex;
            WindowTopmost = config.MainWindowTopmost;
            MinimizeWindowAfterOpening = config.MinimizeMainWindowAfterOpening;
            ShowOpenErrorMsg = config.ShowOpenErrorMessage;

            NewGameLauncherCommand = new DelegateCommand(NewGameLauncher);
        }
    }
}
