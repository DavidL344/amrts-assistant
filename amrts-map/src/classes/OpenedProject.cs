using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace amrts_map
{
    public class OpenedProject
    {
        [JsonIgnore] public Dictionary<string, string> Project = new Dictionary<string, string>();
        public Dictionary<string, string> Map = new Dictionary<string, string>();
        public Dictionary<string, string> PathVars = new Dictionary<string, string>();
        [JsonIgnore] public bool Initialized;

        [JsonIgnore] readonly string[] xTypes = new string[] { "x", "x-e" };
        [JsonIgnore] readonly string[] xKeys = new string[] { "", "_edit", "_export" };
        [JsonIgnore] readonly string[] pathVarKeys = new string[] { "Root", "Cache", "Edit", "Export" };

        public OpenedProject()
        {
            // ./ - root directory (contains the project file)
            // ./cache/ - cached original map files
            // ./edit/ - extracted map files for edit
            // ./export/ - exported modified map files
            Reset();
        }

        public void ChangePathType(string type)
        {
            bool useRelativePath = false;
            if (type == "relative") useRelativePath = true;

            foreach (string xType in xTypes)
            {
                if (IsMapKeyValid(xType))
                {
                    foreach (string xKey in xKeys)
                        this.Map[xType + xKey] = InternalMethods.GetPath(this.Project["Path"], this.Map[xType + xKey], useRelativePath);
                }
            }
            
            foreach (string pathVarKey in pathVarKeys)
                this.PathVars[pathVarKey] = InternalMethods.GetPath(this.Project["Path"], this.PathVars[pathVarKey], useRelativePath);
        }

        public bool IsMapKeyValid(string fileExtensionWithoutDot)
        {
            bool valid = true;
            try
            {
                foreach (string searchedKey in xKeys)
                {
                    string checkedKey = fileExtensionWithoutDot + searchedKey;
                    valid = this.Map.ContainsKey(checkedKey) && this.Map[checkedKey] != null;
                }
            }
            catch
            {
                valid = false;
            }
            finally { }
            return valid;
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
