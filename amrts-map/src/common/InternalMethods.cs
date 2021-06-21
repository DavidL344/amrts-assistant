using amrts_map.Dialogs;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        public static void Run(ProcessStartInfo processStartInfo = null, string arguments = null, string workingDirectory = null)
        {
            if (processStartInfo == null) processStartInfo = ThisApp;
            if (arguments != null) processStartInfo.Arguments = arguments;
            if (workingDirectory != null) processStartInfo.WorkingDirectory = workingDirectory;
            Process.Start(processStartInfo);
        }

        public static ProcessStartInfo GetProcessStartInfo(string executablePath)
        {
            return new ProcessStartInfo
            {
                FileName = executablePath,
                WorkingDirectory = Path.GetDirectoryName(executablePath)
            };
        }

        public static bool MultipleInstancesRunning()
        {
            return ProcessInstances > 1;
        }

        public static bool IsThisOrSimilarKeyDown(Key key)
        {
            switch (key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                case Key.LeftAlt:
                case Key.RightAlt:
                    return Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
                case Key.LeftShift:
                case Key.RightShift:
                    return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
                default:
                    return false;
            }
        }

        public async static void CatchException(Exception e, bool expected = false)
        {
            string title = expected ? null : "An exception has occured";
            ContentDialogResult contentDialogResult = await Dialog.Show(e.Message, title, expected ? DialogButton.OK : DialogButton.OKClipboard);
            if (contentDialogResult == ContentDialogResult.Secondary) Clipboard.SetText($"An exception has occured:\r\n{e.Message}\r\n{e.StackTrace}");
        }
    }
}
