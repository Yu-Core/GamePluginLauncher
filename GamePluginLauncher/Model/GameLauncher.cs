using GamePluginLauncher.Utils;
using SimpleMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePluginLauncher.Model
{
    public class GameLauncher : NotificationObject
    {
        private string? _name;
        public string? Name
        {
            get => _name;
            set => UpdateValue(ref _name, value);
        }

        private ObservableCollection<GamePlugin>? _gamePlugins;
        public ObservableCollection<GamePlugin>? GamePlugins
        {
            get => _gamePlugins;
            set => UpdateValue(ref _gamePlugins, value);
        }
        private int _selectIndex;
        public int SelectIndex
        {
            get => _selectIndex;
            set => UpdateValue(ref _selectIndex, value);
        }

        public void AddPlugin(string name, string path)
        {
            if (GamePlugins == null)
                GamePlugins = new ObservableCollection<GamePlugin>();
            GamePlugins.Add(new GamePlugin { Name = name, Path = path });
        }

        public void AddPlugin(string path)
        {
            path = PathHelper.FormatPath(path);
            if (!System.IO.File.Exists(path))
                throw new Exception("文件不存在。");

            var suffix = PathHelper.GetSuffix(path).ToUpper();
            switch (suffix)
            {
                case "EXE":
                    AddPlugin(PathHelper.GetFileNameWithoutSuffix(path), path);
                    break;

                case "LNK":
                    var lnk = LnkReader.Lnk.OpenLnk(path);
                    AddPlugin(lnk.BasePath);
                    break;

                default:
                    throw new Exception($"不支持的扩展名：“{suffix}”。");
            }
        }
    }
}
