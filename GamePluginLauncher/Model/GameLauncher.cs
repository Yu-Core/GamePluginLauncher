using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePluginLauncher.Model
{
    public class GameLauncher
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
        public List<GamePlugin>? GamePlugins { get; set; }
    }
}
