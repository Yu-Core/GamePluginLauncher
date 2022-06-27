using System;
using System.Collections.Generic;
using System.Text;

namespace GamePluginLauncher.Utils
{
    public static class PathHelper
    {
        public static string FormatPath(string path)
        {
            return path.Replace('/', '\\');
        }

        public static string GetFileName(string path)
        {
            return path[(path.LastIndexOf('\\') + 1)..];
        }

        public static string GetSuffix(string path)
        {
            if (path.LastIndexOf('.') <= path.LastIndexOf('\\'))
            {
                return string.Empty;
            }
            else
            {
                return path[(path.LastIndexOf('.') + 1)..];
            }
        }

        public static string GetFileNameWithoutSuffix(string path)
        {
            return GetFileName(path)[0..^(GetSuffix(path).Length + 1)];
        }

        public static string GetLocatedFolderPath(string path)
        {
            return path[0..^GetFileName(path).Length];
        }
    }
}
