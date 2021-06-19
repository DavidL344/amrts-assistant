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
        [JsonIgnore] public bool RequiresInitialization
        {
            get
            {
                Dictionary<string, string>[] dictionaries = new Dictionary<string, string>[] { Map, PathVars };
                foreach (Dictionary<string, string> dictionary in dictionaries)
                    foreach (KeyValuePair<string, string> entry in Map)
                        if (entry.Value != null) return true;
                return false;
            }
        }
        [JsonIgnore] public bool Initialized {
            get
            {
                return RequiresInitialization && this.IsValid();
            }
        }

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
                if (IsKeyValid(xType))
                {
                    foreach (string xKey in xKeys)
                        this.Map[xType + xKey] = this.Map[xType + xKey] != null ? InternalMethods.GetPath(this.Project["Path"], this.Map[xType + xKey], useRelativePath) : null;
                }
            }

            foreach (string pathVarKey in pathVarKeys)
                this.PathVars[pathVarKey] = this.PathVars[pathVarKey] != null ? InternalMethods.GetPath(this.Project["Path"], this.PathVars[pathVarKey], useRelativePath) : null;

            // The project file should always be located at the project's root
            if (useRelativePath) this.PathVars["Root"] = "";
        }

        public bool IsKeyValid(string dictionaryKey, bool strictIO = false)
        {
            bool valid = true;
            if (xTypes.Contains(dictionaryKey))
            {
                foreach (string searchedKey in xKeys)
                {
                    string checkedKey = dictionaryKey + searchedKey;
                    if (!this.Map.ContainsKey(checkedKey)) return false;

                    // Allow the dictionary key for "x-e" to be null since its presence is optional
                    if (this.Map[checkedKey] == null)
                        if (strictIO || !checkedKey.StartsWith("x-e")) return false;
                }
            }
            else if (pathVarKeys.Contains(dictionaryKey))
            {
                valid = this.PathVars.ContainsKey(dictionaryKey) && this.PathVars[dictionaryKey] != null;
                if (!valid) return false;
            }
            else valid = false;
            return valid;
        }

        public bool IsValid()
        {
            // Dynamically add the map extension to the project if it exists
            if (File.Exists(Path.ChangeExtension(Map["x"], ".x-e")))
            {
                Map["x-e"] = Path.ChangeExtension(Map["x"], ".x-e");
                Map["x-e_edit"] = Path.Combine(PathVars["Edit"], Path.GetFileNameWithoutExtension(Map["x"]) + @"_x-e\");
                Map["x-e_export"] = Path.ChangeExtension(Map["x_export"], ".x-e");
            }
            else
            {
                Map["x-e"] = null;
                Map["x-e_edit"] = null;
                Map["x-e_export"] = null;
            }

            // The dictionary keys for maps
            foreach (string value in xTypes)
                if (!IsKeyValid(value)) throw new Exception("The map definitions in the project file are missing!");

            // The dictionary keys for paths
            foreach (string value in pathVarKeys)
            {
                if (!IsKeyValid(value)) throw new Exception("The directory definitions in the project file are missing!");
                if (!Directory.Exists(this.PathVars[value]))
                {
                    switch (value)
                    {
                        case "Root":
                            throw new IOException("The project root couldn't be found!");
                        case "Cache":
                            throw new DirectoryNotFoundException("The directory containing cached map files couldn't be found!");
                        case "Edit":
                            amrts_map.Project.DiscardChanges(this);
                            break;
                        case "Export":
                            amrts_map.Project.CleanExport(this);
                            break;
                        default:
                            break;
                    }
                }
            }
            return true;
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
        }
    }
}
