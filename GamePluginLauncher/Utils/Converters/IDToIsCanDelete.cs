using GamePluginLauncher.Carousel;
using GamePluginLauncher.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePluginLauncher.Utils.Converters
{
    public class IDToIsCanDelete : ConverterBase<int, bool>
    {
        public override bool Convert(int value, object parameter, CultureInfo culture)
        {
            if(value == 0) return false;
            return true;
        }

        public override int ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
