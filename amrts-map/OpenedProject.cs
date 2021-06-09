using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amrts_map
{
    public class OpenedProject
    {
        public Dictionary<string, string> Project = new Dictionary<string, string>();
        public Dictionary<string, string> Map = new Dictionary<string, string>();
        public Dictionary<string, string> PathVars = new Dictionary<string, string>();
        public bool Initialized = false;

        public OpenedProject()
        {
            // ./ - root directory (contains the project file)
            // ./cache/ - cached original map files
            // ./edit/ - extracted map files for edit
            // ./export/ - exported modified map files
            Close();
        }

        public void New(string name = null, string projectPath = null, string mapFilePath = null)
        {
            if (name == null || projectPath == null || mapFilePath == null) return;
            if (Directory.Exists(projectPath)) throw new IOException("The directory already exists!");

            Project["Name"] = name;
            Project["Path"] = projectPath;
            Map["Name"] = null;
            Map["x"] = null;
            Map["x-e"] = null;
            PathVars["Root"] = null;
            PathVars["Cache"] = null;
            PathVars["Edit"] = null;
            PathVars["Export"] = null;

            if (projectPath != null)
            {
                string projectRoot = Path.GetDirectoryName(projectPath);
                try
                {
                    Directory.CreateDirectory(projectRoot);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                PathVars["Root"] = projectRoot;
                PathVars["Cache"] = Path.Combine(projectRoot, "/cache/");
                PathVars["Edit"] = Path.Combine(projectRoot, "/edit/");
                PathVars["Export"] = Path.Combine(projectRoot, "/export/");
            }

            // The map path isn't specified when the user creates a project without importing a map
            if (mapFilePath != null)
            {
                if (!File.Exists(mapFilePath)) throw new FileNotFoundException("The map file doesn't exist!");
                string mapExtensionFilePath = Path.ChangeExtension(mapFilePath, ".x-e");
                Map["Name"] = Path.GetFileNameWithoutExtension(mapFilePath);
                Map["x"] = Path.Combine(Project["Root"], "/cache/", Path.GetFileName(mapFilePath));
                if (File.Exists(mapExtensionFilePath)) Map["x-e"] = Path.ChangeExtension(Map["x"], ".x-e");

                File.Copy(mapFilePath, Map["x"]);
                if (Map["x-e"] != null) File.Copy(mapFilePath, Map["x-e"]);
                Initialized = true;
            }
        }

        public void Open(string filePath = null)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("The map project doesn't exist!");
            switch (Path.GetExtension(filePath))
            {
                case ".amramp":
                    // TODO: read the project name and map information from the file
                    Project["Name"] = Path.GetFileNameWithoutExtension(filePath);
                    Project["Path"] = filePath;

                    // By default, the map files have the same name as the project name (might be customizable later)
                    Map["Name"] = Path.GetFileNameWithoutExtension(filePath);
                    Map["x"] = File.Exists(Path.ChangeExtension(filePath, ".x")) ? Path.ChangeExtension(filePath, ".x") : null;
                    Map["x-e"] = File.Exists(Path.ChangeExtension(filePath, ".x-e")) ? Path.ChangeExtension(filePath, ".x-e") : null;
                    break;
                case ".x":
                    Map["Name"] = Path.GetFileNameWithoutExtension(filePath);
                    Map["x"] = filePath;
                    Map["x-e"] = File.Exists(Path.ChangeExtension(filePath, ".x-e")) ? Path.ChangeExtension(filePath, ".x-e") : null;
                    break;
                default:
                    throw new InvalidDataException("The file is invalid!");
            }
            Initialized = true;
        }

        public void Close()
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
