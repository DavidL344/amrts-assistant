using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amrts_map
{
    class InternalMethods
    {
        public static string Name {
            get
            {
                return AboutBox.AssemblyTitle;
            }
        }

        public static string Version
        {
            get
            {
                return AboutBox.AssemblyVersion;
            }
        }

        // A .NET framework alternative of Path.GetRelativePath() - https://stackoverflow.com/a/51180239
        public static string GetRelativePath(string relativeTo, string path)
        {
            var uri = new Uri(relativeTo);
            var rel = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(path)).ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (rel.Contains(Path.DirectorySeparatorChar.ToString()) == false)
            {
                rel = $".{ Path.DirectorySeparatorChar }{ rel }";
            }
            return rel;
        }

        public static string GetParentDirectory(string directoryPath)
        {
            return Path.GetDirectoryName(directoryPath + @"\..\");
        }
    }
}
