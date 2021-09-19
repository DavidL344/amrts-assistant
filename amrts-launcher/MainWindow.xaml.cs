using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace amrts_launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings Settings = new Settings();
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(Vars.LauncherSettings))
            {
                Settings.GameDirectory = DetectGame();
                Settings.Save();
            }
            Settings = Settings.Load();

            //MessageBox.Show($"GameDir: {Settings.GameDirectory}\r\nCheck for updates: Settings.AutoCheckForUpdates\r\nAuto-update: Settings.AutoUpdate");
        }

        public string DetectGame()
        {
            string[] knownGamePaths = new string[] {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\3DO\Army Men RTS",
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Steam\steamapps\common\Army Men RTS",
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\TopCD\Army Men\Army Men RTS"
            };
            File.WriteAllText(Vars.KnownGamePaths, JsonConvert.SerializeObject(knownGamePaths, Formatting.Indented));

            for (int i = 0; i < knownGamePaths.Length; i++)
                if (Directory.Exists(knownGamePaths[i])) return knownGamePaths[i];
            return null;
        }
    }
}
