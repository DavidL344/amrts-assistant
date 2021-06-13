using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrPack.Bridge
{
    public class Main
    {
        public static string AppDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        public static string ResPath = String.Format("{0}{1}", AppDir, @"\drpack.exe");

        public static void Extract(bool force = false)
        {
            if (!File.Exists(ResPath) || force) Resource.Extract(Properties.Resources.drpack, ResPath);
        }
    }
}
