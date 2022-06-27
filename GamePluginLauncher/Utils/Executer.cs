using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GamePluginLauncher.Utils
{
    public static class Executer
    {
        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        public enum Errors : int
        {
            ERROR_FILE_NOT_FOUND = 2,
            ERROR_PATH_NOT_FOUND = 3,
            ERROR_BAD_FORMAT = 11,
            SE_ERR_SHARE = 26,
            SE_ERR_ASSOCINCOMPLETE = 27,
            SE_ERR_DDETIMEOUT = 28,
            SE_ERR_DDEFAIL = 29,
            SE_ERR_DDEBUSY = 30,
            SE_ERR_NOASSOC = 31
        }

        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            ShowCommands nShowCmd);

        public static string GetErrorStr(IntPtr i)
        {
            return (int)i switch
            {
                0 => "内存不足",
                2 => "文件名错误",
                3 => "路径名错误",
                5 => "拒绝访问",
                11 => "EXE 文件无效",
                26 => "发生共享错误",
                27 => "文件名不完全或无效",
                28 => "超时",
                29 => "DDE 事务失败",
                30 => "正在处理其他 DDE 事务而不能完成该 DDE 事务",
                31 => "没有相关联的应用程序",
                _ => "未知错误：" + i.ToString(),
            };
        }
    }
}
