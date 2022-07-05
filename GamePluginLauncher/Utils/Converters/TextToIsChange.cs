using GamePluginLauncher.Carousel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GamePluginLauncher.Utils.Converters
{
    public class TextToIsChange : ConverterBase<string, bool>
    {
        public override bool Convert(string value, object parameter, CultureInfo culture)
        {
            return ((AnimImage)parameter).GamePlugin.Name.Equals(value);
        }

        public override string ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
