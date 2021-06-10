using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace amrts_map
{
    [Serializable()]
    public class OpenedProject
    {
        [NonSerialized()] public Dictionary<string, string> Project = new Dictionary<string, string>();
        public Dictionary<string, string> Map = new Dictionary<string, string>();
        public Dictionary<string, string> PathVars = new Dictionary<string, string>();
        [NonSerialized()] public bool Initialized;

        public OpenedProject()
        {
            // ./ - root directory (contains the project file)
            // ./cache/ - cached original map files
            // ./edit/ - extracted map files for edit
            // ./export/ - exported modified map files
            Reset();
        }

        public void Reset()
        {
            Project["Name"] = null;
            Project["Path"] = null;
            Map["Name"] = null;
            Map["x"] = null;
            Map["x-e"] = null;
            PathVars["Root"] = null;
            PathVars["Cache"] = null;
            PathVars["Edit"] = null;
            PathVars["Export"] = null;
            Initialized = false;
        }
    }
}
