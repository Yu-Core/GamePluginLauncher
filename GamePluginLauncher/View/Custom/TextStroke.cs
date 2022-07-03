using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GamePluginLauncher.View.Custom
{
    public class TextStroke : TextBlock
    {
        //protected new void OnRender(DrawingContext drawingContext)
        //{
        //    var formattedText = new FormattedText(Text, CultureInfo.CurrentCulture,
        //        FlowDirection.LeftToRight,
        //        new Typeface
        //        (
        //            FontFamily,
        //            FontStyle,
        //            FontWeight,
        //            FontStretch
        //        ),
        //        30,
        //        Foreground, 1);

        //    var geometry = formattedText.BuildGeometry(new Point(10, 10));

        //    drawingContext.DrawGeometry
        //    (
        //        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F00002")),
        //        new Pen(new SolidColorBrush(Colors.Black), 1),
        //        geometry
        //    );

        //    base.OnRender(drawingContext);
        //}

        protected new void OnRender(DrawingContext drawingContext)
        {
            var str = "欢迎访问我博客 http://lindexi.gitee.io 里面有大量 UWP WPF 博客";

            var formattedText = new FormattedText(str, CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface
                (
                    new FontFamily("微软雅黑"),
                    FontStyles.Normal,
                    FontWeights.Normal,
                    FontStretches.Normal
                ),
                30,
                Brushes.Black, 1);

            var geometry = formattedText.BuildGeometry(new Point(10, 10));

            drawingContext.DrawGeometry
            (
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F00002")),
                new Pen(new SolidColorBrush(Colors.Black), 1),
                geometry
            );

            base.OnRender(drawingContext);
        }
    }
}
