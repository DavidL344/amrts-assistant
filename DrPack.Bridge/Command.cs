using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrPack.Bridge
{
    internal class Command
    {
        private static ProcessStartInfo ProcessInfoTemplate
        {
            get
            {
                return new ProcessStartInfo
                {
                    FileName = Main.ResPath,
                    WorkingDirectory = Main.AppDir,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
            }
        }

        internal static string Execute(string arguments, bool verify = true, bool returnConsoleOutput = false, bool ignoreWarnings = false)
        {
            if (verify) Verify(arguments, ignoreWarnings);
            Main.Extract();

            ProcessStartInfo processStartInfo = ProcessInfoTemplate;
            processStartInfo.Arguments = arguments;

            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
            string stdout = process.StandardOutput.ReadToEnd();

            if (process.ExitCode != 0) throw new Exception(stdout);
            if (stdout.Contains("Unable")) throw new IOException(stdout);

            if (returnConsoleOutput) return stdout;
            return null;
        }

        internal static void Verify(string arguments, bool ignoreWarnings = false)
        {
            string[] args = arguments.Split(' ');
            if (args.Length < 2 || args.Length > 5) throw new ArgumentOutOfRangeException(String.Format("Invalid argument length detected!\r\nExpected 2-5, got {0}.", args.Length));
            switch (args[0])
            {
                case "c":
                    int optionalArgumentCount = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        bool argumentsProcessed = VerifyOptions(args.Skip(1).ToArray());
                        if (argumentsProcessed)
                        {
                            List<string> ShiftList = new List<string>(args);
                            ShiftList.RemoveAt(1);
                            args = ShiftList.ToArray();
                            optionalArgumentCount++;
                        }
                    }

                    string exceptionText;
                    if (optionalArgumentCount != 0)
                    {
                        exceptionText = String.Format("Invalid argument length detected!\r\nExpected 3-{0}, got {1}.", 3 + optionalArgumentCount, args.Length);
                    }
                    else
                    {
                        exceptionText = String.Format("Invalid argument length detected!\r\nExpected 3, got {0}.", args.Length);
                    }
                    if (args.Length < 3 || args.Length > 3 + optionalArgumentCount) throw new ArgumentOutOfRangeException(exceptionText);

                    if (File.Exists(args[1]) && !ignoreWarnings) throw new WarningException("The archive already exists!");
                    if (!Directory.Exists(args[2])) throw new DirectoryNotFoundException("The directory doesn't exist!");
                    if (!Directory.EnumerateFileSystemEntries(args[2]).Any()) throw new IOException("The directory is empty!");
                    break;
                case "x":
                    if (args.Length != 3) throw new ArgumentOutOfRangeException(String.Format("Invalid argument length detected!\r\nExpected 3, got {0}.", args.Length));
                    if (!File.Exists(args[1])) throw new FileNotFoundException("The archive doesn't exist!");
                    if (Directory.Exists(args[2]))
                    {
                        // Throw the exception only if the directory isn't empty
                        if (Directory.EnumerateFileSystemEntries(args[2]).Any() && !ignoreWarnings) throw new WarningException("The directory already exists!");
                    }
                    break;
                case "l":
                    if (args.Length != 2) throw new ArgumentOutOfRangeException(String.Format("Invalid argument length detected!\r\nExpected 2, got {0}.", args.Length));
                    if (!File.Exists(args[1])) throw new FileNotFoundException("The archive doesn't exist!");
                    break;
                default:
                    throw new ArgumentException("The action is invalid!");
            }
        }

        private static bool VerifyOptions(string[] args)
        {
            if (args[0].StartsWith("-"))
            {
                if (args[0] == "-r") return true;
                else if (args[0].StartsWith("-m=*."))
                {
                    string[] splitString = args[0].Split(new string[] { "-m=*." }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitString.Length != 1) throw new ArgumentException();
                    return true;
                }
                else throw new ArgumentException("The options are invalid!");
            }
            return false;
        }
    }
}
