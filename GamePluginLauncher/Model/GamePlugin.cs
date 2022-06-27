﻿using SimpleMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePluginLauncher.Model
{
    public class GamePlugin : NotificationObject
    {
        private string? _name;
        public string? Name
        {
            get => _name;
            set => UpdateValue(ref _name, value);
        }
        private string? _path;
        public string? Path
        {
            get => _path;
            set => UpdateValue(ref _path, value);
        }
        private string? _imagePath;
        public string? ImagePath
        {
            get => _imagePath;
            set => UpdateValue(ref _imagePath, value);
        }
    }
}
