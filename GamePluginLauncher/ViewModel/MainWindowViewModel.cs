using GamePluginLauncher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePluginLauncher.ViewModel
{
    public class MainWindowViewModel
    {
        public List<GameLauncher> GameLaunchers{ get; set; }
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
