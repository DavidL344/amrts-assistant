using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amrts_launcher
{
    public class AppSettings
    {
        public string InstallDirectory = null;
        public bool AutoCheckForUpdates = true;
        public bool AutoUpdate = true;
        [JsonIgnore] public bool Required = false;
    }
}
