﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace amrts_map
{
    public class Project
    {
        public static string DefaultName = "ArmyMenMap1";
        public static string DefaultLocation = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\Projects");
        public static string DefaultMapLocation = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\3DO\Army Men RTS\missions\mp\8ally.x");
        public static string FileExtension = "amramp";

        public static OpenedProject New(string name = null, string projectPath = null, string mapFilePath = null)
        {
            OpenedProject newProject = new OpenedProject();
            if (name == null || projectPath == null || mapFilePath == null) return newProject;
            string projectRoot = Path.GetFullPath(Path.Combine(projectPath, name));
            if (Directory.Exists(projectRoot)) throw new IOException("The directory already exists!");
            if (!File.Exists(mapFilePath)) throw new FileNotFoundException("The map file doesn't exist!");

            newProject.Project["Name"] = name;
            newProject.Project["Path"] = String.Format(@"{0}\{1}.{2}", projectRoot, name, FileExtension);
            newProject.PathVars["Root"] = projectRoot;
            newProject.PathVars["Cache"] = String.Format(@"{0}\{1}\", projectRoot, "cache");
            newProject.PathVars["Edit"] = String.Format(@"{0}\{1}\", projectRoot, "edit");
            newProject.PathVars["Export"] = String.Format(@"{0}\{1}\", projectRoot, "export");

            string mapExtensionFilePath = Path.ChangeExtension(mapFilePath, ".x-e");
            newProject.Map["Name"] = Path.GetFileNameWithoutExtension(mapFilePath);
            newProject.Map["x"] = Path.Combine(newProject.PathVars["Cache"], Path.GetFileName(mapFilePath));
            if (File.Exists(mapExtensionFilePath)) newProject.Map["x-e"] = Path.ChangeExtension(newProject.Map["x"], ".x-e");

            try
            {
                foreach (KeyValuePair<string, string> entry in newProject.PathVars) Directory.CreateDirectory(entry.Value);
                File.Copy(mapFilePath, newProject.Map["x"]);
                if (newProject.Map["x-e"] != null) File.Copy(mapExtensionFilePath, newProject.Map["x-e"]);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            newProject.Initialized = true;
            Save(newProject);
            return newProject;
        }

        public static OpenedProject Open(string projectPath)
        {
            if (!File.Exists(projectPath)) throw new FileNotFoundException("The map project doesn't exist!");
            Stream stream = File.Open(projectPath, FileMode.Open);

            // The object has to be created beforehand to assign non-serialized variables
            OpenedProject obj = new OpenedProject();
            obj.Reset(); // Suppresses the warning about the unnecessary assignment above

            // TODO: consider switching to System.Runtime.Serialization.Formatters.Soap
            // (this will require to convert Dictionary<string, string> as it cannot be serialized by Soap)
            BinaryFormatter formatter = new BinaryFormatter();
            obj = (OpenedProject)formatter.Deserialize(stream);
            stream.Close();

            // Set the values of variables that were not deserialized
            obj.Project = new Dictionary<string, string>()
            {
                { "Name", Path.GetFileNameWithoutExtension(projectPath) },
                { "Path", projectPath }
            };

            // Get full path of the files (parts of the serialized object contain relative paths)
            obj.Map["x"] = Path.Combine(Path.GetDirectoryName(obj.Project["Path"]), obj.Map["x"]);
            if (obj.Map["x-e"] != null) obj.Map["x-e"] = Path.Combine(Path.GetDirectoryName(obj.Project["Path"]), obj.Map["x-e"]);

            obj.Initialized = true;
            return obj;
        }

        public static OpenedProject OpenMap(string mapPath)
        {
            if (!File.Exists(mapPath)) throw new FileNotFoundException("The map doesn't exist!");
            
            OpenedProject map = new OpenedProject();
            map.Project["Name"] = Path.GetFileName(Path.GetFileName(mapPath));
            map.Project["Path"] = null;
            map.Map["Name"] = map.Project["Name"];
            map.Map["x"] = mapPath;
            if (File.Exists(Path.ChangeExtension(mapPath, ".x-e"))) map.Map["x-e"] = Path.ChangeExtension(mapPath, ".x-e");
            map.Initialized = true;
            return map;
        }

        public static void Save(OpenedProject openedProject, string customPath = null)
        {
            if (customPath == null) customPath = openedProject.Project["Path"];

            // Use relative paths for serialization
            openedProject.Map["x"] = GetRelativePath(Path.GetDirectoryName(openedProject.Project["Path"] + @"\..\"), openedProject.Map["x"]);
            if (openedProject.Map["x-e"] != null) openedProject.Map["x-e"] = GetRelativePath(Path.GetDirectoryName(openedProject.Project["Path"] + @"\..\"), openedProject.Map["x-e"]);

            try
            {
                Stream stream = File.Open(customPath, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, openedProject);
                stream.Close();
            }
            finally
            {
                // Switch back to absolute paths for the program to use
                openedProject.Map["x"] = Path.Combine(Path.GetDirectoryName(openedProject.Project["Path"]), openedProject.Map["x"]);
                if (openedProject.Map["x-e"] != null) openedProject.Map["x-e"] = Path.Combine(Path.GetDirectoryName(openedProject.Project["Path"]), openedProject.Map["x-e"]);
            }
        }

        public static void Close(OpenedProject openedProject)
        {
            openedProject.Reset();
        }


        // A .NET framework alternative of Path.GetRelativePath() - https://stackoverflow.com/a/51180239
        public static string GetRelativePath(string relativeTo, string path)
        {
            var uri = new Uri(relativeTo);
            var rel = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(path)).ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (rel.Contains(Path.DirectorySeparatorChar.ToString()) == false)
            {
                rel = $".{ Path.DirectorySeparatorChar }{ rel }";
            }
            return rel;
        }
    }
}
