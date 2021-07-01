using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrPack.Bridge
{
    internal class Resource
    {
        internal static void Extract(byte[] resource, string location)
        {
            if (File.Exists(location)) File.Delete(location);
            using (FileStream fsDst = new FileStream(location, FileMode.CreateNew, FileAccess.Write))
            {
                fsDst.Write(resource, 0, resource.Length);
            }
        }
    }
}
