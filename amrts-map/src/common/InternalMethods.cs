using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public static string ProcessPath
        {
            get
            {
                return Path.GetFullPath(Assembly.GetExecutingAssembly().Location);
            }
        }

        public static ProcessStartInfo ThisApp
        {
            get
            {
                return new ProcessStartInfo
                {
                    FileName = ProcessPath,
                    WorkingDirectory = Path.GetDirectoryName(ProcessPath)
                };
            }
        }

        public static int ProcessInstances
        {
            get
            {
                return Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count();
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

        public static void Run(ProcessStartInfo processStartInfo = null, string arguments = null)
        {
            if (processStartInfo == null) processStartInfo = ThisApp;
            if (arguments != null) processStartInfo.Arguments = arguments;
            Process.Start(processStartInfo);
        }

        public static bool MultipleInstancesRunning()
        {
            return ProcessInstances > 1;
        }
    }
}
