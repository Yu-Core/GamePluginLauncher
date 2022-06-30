using GamePluginLauncher.Model;
using GamePluginLauncher.Utils;
using Microsoft.Win32;
using SimpleMvvm;
using SimpleMvvm.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GamePluginLauncher.ViewModel
{
    public class PluginSelectorViewModel:ViewModelBase
    {
        public int GameLauncherIndex { get; set; }
        public DelegateCommand? OpenGamePluginCommand { get; set; }
        public DelegateCommand? EditGamePluginCommand { get; set; }
        public DelegateCommand? RemoveGamePluginCommand { get; set; }
        public DelegateCommand? ShowInExplorerCommand { get; set; }

        private void OpenGamePlugin(GamePlugin gamePlugin)
        {
            var path = PathHelper.FormatPath(gamePlugin.Path);
            var info = Executer.ShellExecute(IntPtr.Zero, "open", path, string.Empty,
                PathHelper.GetLocatedFolderPath(path), Executer.ShowCommands.SW_SHOWNORMAL);

            //if ((int)info < 32)
            //{
            //    if (ShowOpenErrorMsg)
            //        MsgBoxHelper.ShowError($"启动时发生错误：{Executer.GetErrorStr(info)}");
            //}
            //else
            //{
            //    if (MinimizeWindowAfterOpening)
            //        WindowState = WindowState.Minimized;
            //}
        }


        protected override void Init()
        {
            base.Init();


        }
    }
}
