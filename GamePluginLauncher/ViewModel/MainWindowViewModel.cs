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
        public List<GameLauncher> GameLaunchers { get; set; }
        public MainWindowViewModel()
        {
            GameLaunchers = new List<GameLauncher>()
            {
                new GameLauncher() { Name = "LOL" },
                new GameLauncher() { Name = "CF" },
                new GameLauncher() { Name = "DNF" },
                new GameLauncher() { Name = "LSCS" },
                null
            };
        }
    }
}
