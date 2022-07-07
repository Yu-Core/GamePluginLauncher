using GamePluginLauncher.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace GamePluginLauncher.Model
{
    public static class StaticData
    {
        public static ObservableCollection<GameLauncher>? GameLaunchers { get; private set; }

        public const string PATH_DATA = @"Data\";
        public const string PATH_GAMELAUNCHER_JSON = PATH_DATA + "GameLauncher.json";

        private static void LoadGameLaunchers()
        {
            if (File.Exists(PATH_GAMELAUNCHER_JSON))
            {
                var json = File.ReadAllText(PATH_GAMELAUNCHER_JSON);
                GameLaunchers = JsonConvert.DeserializeObject<ObservableCollection<GameLauncher>>(json);
            }
            if (GameLaunchers == null)
            {
                GameLaunchers = new ObservableCollection<GameLauncher>();
            }
            foreach (var item in GameLaunchers)
            {
                if (item.GamePlugins == null)
                    item.GamePlugins = new ObservableCollection<GamePlugin>();
            }
        }

        private static void SaveGameLaunchers()
        {
            if (!Directory.Exists(PATH_DATA))
                Directory.CreateDirectory(PATH_DATA);
            var json = JsonConvert.SerializeObject(GameLaunchers, Formatting.Indented);
            File.WriteAllText(PATH_GAMELAUNCHER_JSON, json);
        }


        public static void InitStaticData()
        {
            LoadGameLaunchers();
        }

        public static void SaveStaticData()
        {
            SaveGameLaunchers();
        }

        public static void AddGameLauncher(string path)
        {
            if (GameLaunchers == null)
                GameLaunchers = new ObservableCollection<GameLauncher>();

            var name = PathHelper.GetFileNameWithoutSuffix(path);

            int count = StaticData.GameLaunchers.Count;
            int id = count > 0 ? StaticData.GameLaunchers[count - 1].Id + 1 : 0;

            GameLaunchers.Add(new GameLauncher
            {
                Id = id,
                Name = name,
                GamePlugins = new ObservableCollection<GamePlugin>(),
                SelectPluginId = 0
            });

            GameLaunchers[GameLaunchers.Count - 1].AddPlugin("原生启动",path);
        }
    }
}
