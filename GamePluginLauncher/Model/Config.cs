using System;
using System.Collections.Generic;
using System.Text;

namespace GamePluginLauncher.Model
{
    public class Config
    {
        public WindowInfo? MainWindowInfo { get; set; }

        public int AppListListBoxSelectedIndex { get; set; }

        public bool MainWindowTopmost { get; set; }

        public bool MinimizeMainWindowAfterOpening { get; set; } = false;

        public bool ShowOpenErrorMessage { get; set; } = true;
    }
}
