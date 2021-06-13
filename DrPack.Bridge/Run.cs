using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrPack.Bridge
{
    public class Run
    {
        public static void Create(string file, string location, bool recursive = true, string mask = null)
        {
            if (string.IsNullOrWhiteSpace(file)) throw new ArgumentException("The archive path is not defined!");
            if (string.IsNullOrWhiteSpace(location)) throw new ArgumentException("The directory location is not defined!");
            string arguments = String.Format("c");
            if (recursive) arguments += " -r";
            if (mask != null) arguments += String.Format(" -m=*.{0}", mask);
            arguments += String.Format(" {0} {1}", file, location);
            Command.Execute(arguments);
            if (!File.Exists(file)) throw new IOException("The archive couldn't be created!");
        }

        public static void Extract(string file, string location)
        {
            if (string.IsNullOrWhiteSpace(file)) throw new ArgumentException("The archive path is not defined!");
            if (string.IsNullOrWhiteSpace(location)) throw new ArgumentException("The directory location is not defined!");
            Command.Execute(String.Format("x {0} {1}", file, location));
            if (!Directory.Exists(location)) throw new IOException("The directory couldn't be created!");
            if (!Directory.EnumerateFileSystemEntries(location).Any()) throw new IOException("The archive wasn't extracted!");
        }

        public static string List(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) throw new ArgumentException("The archive path is not defined!");
            return Command.Execute(String.Format("l {0}", file), true, true);
        }
    }
}
