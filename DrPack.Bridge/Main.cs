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
        public static void Extract(bool force = false)
        {
            string AppDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string ResPath = String.Format("{0}{1}", AppDir, @"\drpack.exe");
            if (!File.Exists(ResPath) || force) Resource.Extract(Properties.Resources.drpack, AppDir);
        }
    }
}
