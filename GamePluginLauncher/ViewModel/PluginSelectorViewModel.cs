using GamePluginLauncher.Carousel;
using GamePluginLauncher.Model;
using GamePluginLauncher.Utils;
using GamePluginLauncher.Utils.Converters;
using GamePluginLauncher.View;
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
using System.Windows.Controls;

namespace GamePluginLauncher.ViewModel
{
    public class PluginSelectorViewModel:ViewModelBase
    {
        private GameLauncher? _gameLauncher;
        public GameLauncher? GameLauncher
        {
            get => _gameLauncher;
            set => UpdateValue(ref _gameLauncher,value); 
        }

        public DelegateCommand? OpenGamePluginCommand { get; set; }
        public DelegateCommand? RemoveGamePluginCommand { get; set; }
        public DelegateCommand? EditGamePluginPathCommand { get; set; }
        public DelegateCommand? CloseWindowCommand { get; set; }
        public DelegateCommand? MinWindowCommand { get; set; }

        private void OpenGamePlugin(int id)
        {
            var plugin = GameLauncher?.GamePlugins?.FirstOrDefault(x => x.Id == id);
            if(plugin == null)
            {
                MsgBoxHelper.ShowError("该插件不存在");
            }
            var path = PathHelper.FormatPath(plugin.Path);
            var info = Executer.ShellExecute(IntPtr.Zero, "open", path, string.Empty,
                PathHelper.GetLocatedFolderPath(path), Executer.ShowCommands.SW_SHOWNORMAL);

            if ((int)info < 32)
            {
                MsgBoxHelper.ShowError($"启动时发生错误：{Executer.GetErrorStr(info)}");
            }
            else
            {
                if (StaticData.Config.IsCloseAfterPluginStarted)
                {
                    Application.Current.Shutdown();
                }
            }
        }


        protected override void Init()
        {
            base.Init();
            RemoveGamePluginCommand = new DelegateCommand<AnimImage>(RemoveGamePlugin);
            OpenGamePluginCommand = new DelegateCommand<int>(OpenGamePlugin);
            EditGamePluginPathCommand = new DelegateCommand(EditGamePluginPath);
            CloseWindowCommand = new DelegateCommand(CloseWindow);
            MinWindowCommand = new DelegateCommand(MinWindow);
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
                    var plugins = StaticData.GameLaunchers?.First(it => it.Id == GameLauncher?.Id).GamePlugins;
                    var plugin = plugins?.Where(it=>it.Id == animImage.PluginId).FirstOrDefault();
                    if (plugin != null)
                    {
                        plugins?.Remove(plugin);

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
        private void EditGamePluginPath(object obj)
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
        private void CloseWindow(object obj)
        {
            ((Window)obj).Close();
            var _mainWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            if (_mainWindow != null) _mainWindow.WindowState = WindowState.Normal;
        }
        private void MinWindow(object obj)
        {
            ((Window)obj).WindowState = WindowState.Minimized;
        }
    }
}
