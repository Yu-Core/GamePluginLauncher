using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace GamePluginLauncher.Utils
{
    public static class VisualTreeUtils
    {
        public static T FindParent<T>(DependencyObject obj) where T : class
        {
            while (obj != null && !(obj is T))
                obj = VisualTreeHelper.GetParent(obj);
            return obj as T;
        }
    }
}
