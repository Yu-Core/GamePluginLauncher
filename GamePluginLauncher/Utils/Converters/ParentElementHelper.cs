using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GamePluginLauncher.Utils.Converters
{
    public static class ParentElementHelper
    {
        /// <summary>
        /// Get:获取符合类型的父元素
        /// </summary>
        /// <typeparam name="T">父控件的类型</typeparam>
        /// <param name="visual">要找的是obj的父控件</param>
        /// <returns>目标父控件</returns>
        public static T GetParent<T>(DependencyObject visual)
            where T : FrameworkElement
        {
            if (visual == null) { return default; }

            var _parent = VisualTreeHelper.GetParent(visual);
            while (_parent != null)
            {
                if (_parent is T _pVisual) { return _pVisual; }

                // 在上一级父控件中没有找到指定名字的控件，就再往上一级找
                _parent = VisualTreeHelper.GetParent(_parent);
            }

            return default;
        }

        /// <summary>
        /// Get:获取符合类型的父元素
        /// </summary>
        /// <typeparam name="T">父控件的类型</typeparam>
        /// <param name="visual">要找的是obj的父控件</param>
        /// <param name="name">想找的父控件的Name属性</param>
        /// <returns>目标父控件</returns>
        public static T GetParent<T>(Visual visual, string name)
            where T : FrameworkElement
        {
            if (visual == null) { return default; }

            var _parent = VisualTreeHelper.GetParent(visual);
            while (_parent != null)
            {
                if (_parent is T _pVisual)
                {
                    if (_pVisual.Name == name)
                    {
                        return _pVisual;
                    }
                }

                // 在上一级父控件中没有找到指定名字的控件，就再往上一级找
                _parent = VisualTreeHelper.GetParent(_parent);
            }

            return default;

        }

    }
}
