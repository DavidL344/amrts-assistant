using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

namespace amrts_map
{
    public class Project
    {
        public static string DefaultName = "ArmyMenMap1";
        public static string DefaultLocation = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\Projects");
        public static string DefaultMapLocation = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\3DO\Army Men RTS\missions\mp\8ally.x");
        public static string FileTypeName = "Map Project";
        public static string FileExtension = "amramp";
        public static string MapTypeName = "Army Men RTS Map File";
        public static string MapExtension = "x";

        public static OpenedProject New(string name = null, string projectPath = null, string mapFilePath = null)
        {
            OpenedProject newProject = new OpenedProject();
            if (name == null || projectPath == null || mapFilePath == null) return newProject;
            string projectRoot = Path.GetFullPath(Path.Combine(projectPath, name));
            if (Directory.Exists(projectRoot)) throw new IOException("The directory already exists!");
            if (!File.Exists(mapFilePath)) throw new FileNotFoundException("The map file doesn't exist!");

            newProject.Project["Name"] = name;
            newProject.Project["Path"] = String.Format(@"{0}\{1}.{2}", projectRoot, name, FileExtension);
            newProject.Project["Root"] = projectRoot;
            newProject.PathVars["Cache"] = String.Format(@"{0}\{1}\", projectRoot, "cache");
            newProject.PathVars["Edit"] = String.Format(@"{0}\{1}\", projectRoot, "edit");
            newProject.PathVars["Export"] = String.Format(@"{0}\{1}\", projectRoot, "export");

            string mapExtensionFilePath = Path.ChangeExtension(mapFilePath, ".x-e");
            newProject.Map["x"] = Path.Combine(newProject.PathVars["Cache"], Path.GetFileName(mapFilePath));
            newProject.Map["x_edit"] = Path.Combine(newProject.PathVars["Edit"], Path.GetFileNameWithoutExtension(mapFilePath) + @"_x\");
            newProject.Map["x_export"] = Path.Combine(newProject.PathVars["Export"], Path.GetFileName(mapFilePath));
            if (File.Exists(mapExtensionFilePath))
            {
                newProject.Map["x-e"] = Path.ChangeExtension(newProject.Map["x"], ".x-e");
                newProject.Map["x-e_edit"] = Path.Combine(newProject.PathVars["Edit"], Path.GetFileNameWithoutExtension(mapFilePath) + @"_x-e\");
                newProject.Map["x-e_export"] = Path.ChangeExtension(newProject.Map["x_export"], ".x-e");
            }
            else newProject.Map["x-e"] = newProject.Map["x-e_edit"] = newProject.Map["x-e_export"] = null;

            foreach (KeyValuePair<string, string> entry in newProject.PathVars) Directory.CreateDirectory(entry.Value);
            File.Copy(mapFilePath, newProject.Map["x"]);
            if (newProject.Map["x-e"] != null) File.Copy(mapExtensionFilePath, newProject.Map["x-e"]);
            UnpackMap(newProject);

            Save(newProject);
            return newProject;
        }

        public static OpenedProject Open(string projectPath)
        {
            if (!File.Exists(projectPath)) throw new FileNotFoundException("The map project doesn't exist!");

            // The object has to be created beforehand to assign non-serialized variables
            OpenedProject obj = new OpenedProject();
            obj.Reset(); // Suppresses the warning about the unnecessary assignment above

            obj = JsonConvert.DeserializeObject<OpenedProject>(File.ReadAllText(projectPath));

            // Set the values of variables that were not deserialized
            obj.Project = new Dictionary<string, string>()
            {
                { "Name", Path.GetFileNameWithoutExtension(projectPath) },
                { "Path", projectPath },
                { "Root", Path.GetDirectoryName(projectPath) }
            };

            // Get full path of the files (parts of the serialized object contain relative paths)
            obj.ChangePathType("absolute");
            return obj;
        }

        public static void Save(OpenedProject openedProject)
        {
            // Use relative paths for serialization
            openedProject.ChangePathType("relative");
            try
            {
                File.WriteAllText(openedProject.Project["Path"], JsonConvert.SerializeObject(openedProject, Formatting.Indented));
            }
            finally
            {
                // Switch back to absolute paths for the program to use
                openedProject.ChangePathType("absolute");
            }
        }

        public static void SaveAs(OpenedProject openedProject)
        {
            VistaFolderBrowserDialog projectNewLocation = Dialog.BrowseFolder("Save Project As...");
            if (projectNewLocation != null) DirectoryMethods.Copy(DirectoryMethods.GetParentDirectory(openedProject.Project["Path"]), projectNewLocation.SelectedPath);
        }

        public static void UnpackMap(OpenedProject openedProject)
        {
            if (openedProject.IsKeyValid("x") && File.Exists(openedProject.Map["x"]))
                DrPack.Bridge.Run.Extract(openedProject.Map["x"], openedProject.Map["x_edit"]);
            if (openedProject.IsKeyValid("x-e") && File.Exists(openedProject.Map["x-e"]))
                DrPack.Bridge.Run.Extract(openedProject.Map["x-e"], openedProject.Map["x-e_edit"]);
        }

        public static void PackMap(OpenedProject openedProject)
        {
            if (openedProject.IsKeyValid("x") && Directory.Exists(openedProject.Map["x_edit"]))
                DrPack.Bridge.Run.Create(openedProject.Map["x_export"], openedProject.Map["x_edit"]);
            if (openedProject.IsKeyValid("x-e") && Directory.Exists(openedProject.Map["x-e_edit"]))
                DrPack.Bridge.Run.Create(openedProject.Map["x-e_export"], openedProject.Map["x-e_edit"]);
        }

        public static void DiscardChanges(OpenedProject openedProject)
        {
            DirectoryMethods.Clean(openedProject.PathVars["Edit"]);
            UnpackMap(openedProject);
        }

        public static void CleanExport(OpenedProject openedProject)
        {
            DirectoryMethods.Clean(openedProject.PathVars["Export"]);
        }

        public static void Build(OpenedProject openedProject)
        {
            if (openedProject.IsKeyValid("x") && File.Exists(openedProject.Map["x_export"])) File.Delete(openedProject.Map["x_export"]);
            if (openedProject.IsKeyValid("x-e", true) && File.Exists(openedProject.Map["x-e_export"])) File.Delete(openedProject.Map["x-e_export"]);
            PackMap(openedProject);
        }

        public static void Close(OpenedProject openedProject)
        {
            openedProject.Reset();
        }
    }
}
