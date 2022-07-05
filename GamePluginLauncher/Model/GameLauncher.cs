
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
        private int _id;
        public int Id
        {
            get => _id;
            set => UpdateValue(ref _id, value);
        }
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
        private int _selectPluginId;
        public int SelectPluginId
        {
            get => _selectPluginId;
            set => UpdateValue(ref _selectPluginId, value);
        }

        public void AddPlugin(string name, string path)
        {
            if (GamePlugins == null)
                GamePlugins = new ObservableCollection<GamePlugin>();

            int count = GamePlugins.Count;
            int id = count > 0 ? GamePlugins[count - 1].Id + 1 : 0;

            GamePlugins.Add(new GamePlugin
            {
                Id = id,
                Name = name,
                Path = path,
                BackgroundPath = ImageHelper.GetRandomBackground()
            });
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
