using GamePluginLauncher.Carousel;
using GamePluginLauncher.Model;
using GamePluginLauncher.Utils;
using GamePluginLauncher.Utils.Converters;
using GamePluginLauncher.View.Dialogs;
using MaterialDesignThemes.Wpf;
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
        private int _launcherID;
        public int LauncherID
        {
            get => _launcherID;
            set => UpdateValue(ref _launcherID,value);
        }

        public DelegateCommand? OpenGamePluginCommand { get; set; }
        public DelegateCommand? RemoveGamePluginCommand { get; set; }

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
        }


        protected override void Init()
        {
            base.Init();
            RemoveGamePluginCommand = new DelegateCommand<AnimImage>(RemoveGamePlugin);
        }

        private async void RemoveGamePlugin(AnimImage animImage)
        {
            //创建对话框用户控件
            var dialog = new RemovePluginDialog();
            //打开对话框，接收关闭返回值
            var result = await DialogHost.Show(dialog, "PluginDialog");
            //判断是否确定
            if (Convert.ToBoolean(result))
            {
                try
                {
                    var plugins = StaticData.GameLaunchers.Where(it => it.Id == LauncherID).First().GamePlugins;
                    var plugin = plugins.Where(it=>it.Id == animImage.PluginId).FirstOrDefault();
                    if (plugin != null)
                    {
                        plugins.Remove(plugin);

                        var cmv = ParentElementHelper.GetParent<CarouselModuleView>(animImage);
                        cmv.RemoveElement(animImage.PluginId);
                        
                    }
                    else MsgBoxHelper.ShowError("删除失败, 错误原因：删除对象不存在");
                }
                catch (Exception ex)
                {
                    MsgBoxHelper.ShowError("删除失败，错误原因：" + ex.Message);
                }
            }
        }
    }
}
