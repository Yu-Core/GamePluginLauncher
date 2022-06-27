using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GamePluginLauncher.Utils.Converters
{
    public class PathToExeIcon : ConverterBase<string, ImageSource>
    {
        public override ImageSource Convert(string value, object parameter, CultureInfo culture)
        {
            try
            {
                return GetIcon(value);
            }
            catch
            {
                return null;
            }
        }

        public override string ConvertBack(ImageSource value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static ImageSource GetIcon(string fileName)
        {
            System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle,
                        new Int32Rect(0, 0, icon.Width, icon.Height),
                        BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
