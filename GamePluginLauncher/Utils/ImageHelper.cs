using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePluginLauncher.Utils
{
    public static class ImageHelper
    {
        public static string GetRandomBackground()
        {
            List<string> tFileItems = new List<string>();
            string sPath = AppDomain.CurrentDomain.BaseDirectory + "Image";
            DirectoryInfo dir = new DirectoryInfo(sPath);
            FileInfo[] fis = dir.GetFiles();
            if (fis.Length > 0)
            {
                for (int j = 0; j < fis.Length; j++)
                    tFileItems.Add(fis[j].FullName);
            }

            Random r = new Random();
            int i = r.Next(tFileItems.Count);

            return tFileItems[i];
        }
    }
}
