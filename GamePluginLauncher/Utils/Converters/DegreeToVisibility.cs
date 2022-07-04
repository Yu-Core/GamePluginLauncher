using GamePluginLauncher.Carousel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePluginLauncher.Utils.Converters
{
    public class DegreeToVisibility : ConverterBase<double, bool>
    {
        public override bool Convert(double value, object parameter, CultureInfo culture)
        {
            if (value - 180d < 1)
            {
                return true;
            }
            return false;
        }

        public override double ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
