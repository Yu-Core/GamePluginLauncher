using GamePluginLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GamePluginLauncher.Utils.Converters
{
    public class PathToFileName : ConverterBase<string, string>
    {
        public override string Convert(string value, object parameter, CultureInfo culture)
        {
            return PathHelper.GetFileName(value);
        }

        public override string ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
