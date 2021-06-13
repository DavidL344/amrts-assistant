using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace amrts_map
{
    public class RecentlyOpened
    {
        public static string FileXML = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\recently_opened.xml";
        private static List<string[]> newData;
        public static List<string[]> Data
        {
            get
            {
                if (!File.Exists(FileXML)) Save();
                using (FileStream stream = new FileStream(FileXML, FileMode.Open))
                {
                    XmlSerializer format = new XmlSerializer(typeof(List<string[]>));
                    return format.Deserialize(stream) as List<string[]>;
                }
            }
        }

        public static void Add(string projectName, string projectPath)
        {
            newData = Data;
            for (int i = 0; i < newData.Count; i++)
            {
                if (newData[i][0].Trim() == projectName.Trim() && newData[i][1].Trim() == projectPath.Trim()) newData.RemoveAt(i);
            }
            newData.Insert(0, new string[] { projectName, projectPath });
            if (newData.Count > 10) newData.RemoveAt(newData.Count - 1);
            Save();
        }

        public static void Clear()
        {
            newData = new List<string[]>();
            Save();
        }

        private static void Save()
        {
            using (FileStream stream = new FileStream(FileXML, FileMode.Create))
            {
                XmlSerializer format = new XmlSerializer(typeof(List<string[]>));
                format.Serialize(stream, newData);
            }
        }
    }
}
