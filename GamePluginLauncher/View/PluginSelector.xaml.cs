﻿using GamePluginLauncher.Carousel;
using GamePluginLauncher.ViewModel;
using System;
using System.Collections.Generic;
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
        public int LauncherIndex
        { 
            set=> pluginSelectorViewModel.LauncherIndex = value;
        }
        public PluginSelector()
        {
            InitializeComponent();
        }
    }
}