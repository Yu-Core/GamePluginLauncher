using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace GamePluginLauncher.Utils
{
    public static class MsgBoxHelper
    {
        public static MessageBoxResult ShowMessage(string message, string caption = null)
        {
            return MessageBox.Show(message, caption ?? "信息",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult ShowQuestion(string message, string caption = null)
        {
            return MessageBox.Show(message, caption ?? "请选择",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static MessageBoxResult ShowWarning(string message, string caption = null)
        {
            return MessageBox.Show(message, caption ?? "警告",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static MessageBoxResult ShowError(string message, string caption = null)
        {
            return MessageBox.Show(message, caption ?? "错误",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
