using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amrts_launcher
{
    public class Vars
    {
        public static readonly string AppMainDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Army Men RTS Assistant";
        public static readonly string AppCommonDir = AppMainDir + @"\common";
        public static readonly string LauncherSettings = AppCommonDir + @"\settings.json";
        public static readonly string KnownGamePaths = AppCommonDir + @"\known_game_paths.json";
    }
}
