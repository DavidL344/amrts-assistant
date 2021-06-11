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

        // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        public static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs = true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        public static string GetPath(string pathToConvert, string secondPathToConvert, bool useRelativePath = false)
        {
            if (useRelativePath)
            {
                return InternalMethods.GetRelativePath(InternalMethods.GetParentDirectory(pathToConvert), secondPathToConvert);
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
