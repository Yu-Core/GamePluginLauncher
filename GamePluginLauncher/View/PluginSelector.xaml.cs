using GamePluginLauncher.Carousel;
using GamePluginLauncher.Model;
using GamePluginLauncher.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GamePluginLauncher.View
{
    /// <summary>
    /// PluginSelector.xaml 的交互逻辑
    /// </summary>
    public partial class PluginSelector : Window
    {
        PluginSelectorViewModel pluginSelectorViewModel => (PluginSelectorViewModel)DataContext;
        public int LauncherId
        {
            set
            {
                var gameLauncher = StaticData.GameLaunchers.FirstOrDefault(it => it.Id == value);
                if (gameLauncher != null)
                {
                    pluginSelectorViewModel.GameLauncher = gameLauncher;
                }
                else
                {
                    MessageBox.Show("该游戏启动器不存在");
                    this.Close();
                }
            }
        }
        public PluginSelector()
        {
            InitializeComponent();

        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
