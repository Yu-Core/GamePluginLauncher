using GamePluginLauncher.Carousel;
using GamePluginLauncher.Utils.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePluginLauncher.Utils.Converters
{
    public class AnimImageToVisibility : ConverterBase<AnimImage, bool>
    {
        public override bool Convert(AnimImage value, object parameter, CultureInfo culture)
        {
            if(Math.Abs(value.Degree - 180d) < 1 )
            {
                return true;
            }
            return false;
        }

        public override AnimImage ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
