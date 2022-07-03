using GamePluginLauncher.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePluginLauncher.Utils.Converters
{
    public class IDToGameLauncher : ConverterBase<int, GameLauncher>
    {
        public override GameLauncher Convert(int value, object parameter, CultureInfo culture)
        {
            var count = StaticData.GameLaunchers.Where(it => it.Id == value).Count();
            if (count == 0)
            {
                //由于第一次DataContext的绑定在id指定之前，为防止报错需要临时指定一个id，在id指定之后DataContext会重绑定
                return StaticData.GameLaunchers[StaticData.GameLaunchers.Count - 1];
            }
            return StaticData.GameLaunchers.Where(it => it.Id == value).First();
        }

        public override int ConvertBack(GameLauncher value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
