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
            return StaticData.GameLaunchers.Where(it => it.Id == value).First();
        }

        public override int ConvertBack(GameLauncher value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
