using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amrts_launcher
{
    public class Settings
    {
        public string GameDirectory;
        public Dictionary<string, AppSettings> AppSettings = new Dictionary<string, AppSettings>();

        public Settings()
        {
            string[] directories = new string[] { Vars.AppMainDir, Vars.AppCommonDir };
            foreach (string directory in directories) if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string[] apps = new string[] { "amrts-map", "amrts-store", "DrPack.Bridge" };
            foreach (string app in apps) AppSettings.Add(app, new AppSettings());
            AppSettings["DrPack.Bridge"].Required = true;
        }

        public void Save()
        {
            File.WriteAllText(Vars.LauncherSettings, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public Settings Load()
        {
            string SettingsData = File.ReadAllText(Vars.LauncherSettings);
            return JsonConvert.DeserializeObject<Settings>(SettingsData);
        }
    }
}
