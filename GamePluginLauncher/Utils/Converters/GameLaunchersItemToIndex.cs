using GamePluginLauncher.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GamePluginLauncher.Utils.Converters
{
    public class GameLaunchersItemToIndex : IMultiValueConverter
    {
        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            
           return ((ObservableCollection<GameLauncher>)(values[1])).IndexOf((GameLauncher)(values[0]));
           

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
