using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GamePluginLauncher
{
    public class DataTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DT1 { get; set; }

        public DataTemplate DT2 { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return DT2;
            return DT1;
        }
    }
}
