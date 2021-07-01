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
        /// <summary>
        /// Creates a Dark Reign 2 archive.
        /// </summary>
        /// <param name="file">A path of the archive to be created</param>
        /// <param name="location">A directory containing the files to be packed</param>
        /// <param name="recursive">Include subdirectories</param>
        /// <param name="mask">Include only the following file extension</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception">Thrown when DrPack throws an error</exception>
        /// <exception cref="IOException"></exception>
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

        /// <summary>
        /// Unpacks a Dark Reign 2 archive.
        /// </summary>
        /// <param name="file">A path of the archive to be extracted</param>
        /// <param name="location">A directory for the files to be extracted into</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception">Thrown when DrPack throws an error</exception>
        /// <exception cref="IOException"></exception>
        public static void Extract(string file, string location)
        {
            if (string.IsNullOrWhiteSpace(file)) throw new ArgumentException("The archive path is not defined!");
            if (string.IsNullOrWhiteSpace(location)) throw new ArgumentException("The directory location is not defined!");
            Command.Execute(String.Format("x {0} {1}", file, location));
            if (!Directory.Exists(location)) throw new IOException("The directory couldn't be created!");
            if (!Directory.EnumerateFileSystemEntries(location).Any()) throw new IOException("The archive wasn't extracted!");
        }

        /// <summary>
        /// Reads the contents of a Dark Reign 2 archive.
        /// </summary>
        /// <param name="file">A path of the archive to be read from</param>
        /// <returns>A list of the packed files.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception">Thrown when DrPack throws an error</exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        public static string List(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) throw new ArgumentException("The archive path is not defined!");
            if (!File.Exists(file)) throw new FileNotFoundException("The archive doesn't exist!");
            return Command.Execute(String.Format("l {0}", file), true, true);
        }
    }
}
