using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DrPack.Bridge
{
    public class Main
    {
        internal static string AppDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        internal static string ResPath = String.Format("{0}{1}", AppDir, @"\drpack.exe");

        /// <summary>
        /// Extract the Dark Reign 2 Pack Utility.
        /// </summary>
        /// <param name="force">Replace the exiting file</param>
        public static void Extract(bool force = false)
        {
            if (!File.Exists(ResPath) || force) Resource.Extract(Properties.Resources.drpack, ResPath);
        }
    }
}
