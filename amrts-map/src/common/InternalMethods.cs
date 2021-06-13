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

        public static string GetPath(string pathToConvert, string secondPathToConvert, bool useRelativePath = false)
        {
            if (useRelativePath)
            {
                return DirectoryMethods.GetRelativePath(DirectoryMethods.GetParentDirectory(pathToConvert), secondPathToConvert);
            }
            else
            {
                return Path.Combine(Path.GetDirectoryName(pathToConvert), secondPathToConvert);
            }
        }

        public static void ExtractResource(byte[] resource, string location)
        {
            using (FileStream fsDst = new FileStream(location, FileMode.CreateNew, FileAccess.Write))
            {
                fsDst.Write(resource, 0, resource.Length);
            }
        }
    }
}
