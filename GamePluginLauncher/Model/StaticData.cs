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

        public static Config? Config { get; private set; }

        public const string PATH_DATA = @"Data\";
        public const string PATH_APPLIST_JSON = PATH_DATA + "GameLauncher.json";
        public const string PATH_CONFIG_JSON = PATH_DATA + "Config.json";

        private static void LoadGameLaunchers()
        {
            if (File.Exists(PATH_APPLIST_JSON))
            {
                var json = File.ReadAllText(PATH_APPLIST_JSON);
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
            File.WriteAllText(PATH_APPLIST_JSON, json);
        }

        private static void LoadConfig()
        {
            if (File.Exists(PATH_CONFIG_JSON))
            {
                var json = File.ReadAllText(PATH_CONFIG_JSON);
                Config = JsonConvert.DeserializeObject<Config>(json);
            }
            if (Config == null)
            {
                Config = new Config();
            }
        }

        private static void SaveConfig()
        {
            if (!Directory.Exists(PATH_DATA))
                Directory.CreateDirectory(PATH_DATA);
            var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(PATH_CONFIG_JSON, json);
        }

        public static void InitStaticData()
        {
            LoadGameLaunchers();
            LoadConfig();
        }

        public static void SaveStaticData()
        {
            SaveGameLaunchers();
            SaveConfig();
        }

        public static void AddGameLauncher(string name)
        {
            if (GameLaunchers == null)
                GameLaunchers = new ObservableCollection<GameLauncher>();
            GameLaunchers.Add(new GameLauncher
            {
                Name = name,
                GamePlugins = new ObservableCollection<GamePlugin>()
            });
        }
    }
}
