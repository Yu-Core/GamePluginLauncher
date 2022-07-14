using GamePluginLauncher.Carousel;
using GamePluginLauncher.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GamePluginLauncher.Utils.Converters
{
    public class GameLauncherToBackground : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int selectpluginId = (int)values[0];
            int id = (int)values[1];

            GameLauncher gameLauncher = StaticData.GameLaunchers.FirstOrDefault(it=>it.Id == id);
            GamePlugin gamePlugin = gameLauncher.GamePlugins.FirstOrDefault(it=>it.Id == selectpluginId);

            return new BitmapImage(new Uri(gamePlugin.BackgroundPath));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
