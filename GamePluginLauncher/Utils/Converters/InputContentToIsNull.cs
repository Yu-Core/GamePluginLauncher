using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePluginLauncher.Utils.Converters
{
    public class InputContentToIsNull : ConverterBase<string, bool>
    {
        public override bool Convert(string value, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public override string ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
