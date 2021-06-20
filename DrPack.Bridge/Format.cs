using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrPack.Bridge
{
    public class Format
    {
        public static Dictionary<string, string> Formats = new Dictionary<string, string>()
        {
            { "amrts", ".tga" },
            { "dr2", ".pic" }
        };

        public static string Get(string filePath)
        {
            string fileContent = File.ReadAllText(filePath, Encoding.ASCII);
            bool isAmrts = fileContent.Contains(Formats["amrts"]);
            bool isDr2 = fileContent.Contains(Formats["dr2"]);

            if ((isAmrts && isDr2) || (!isAmrts && !isDr2)) return null;
            if (isAmrts) return "amrts";
            if (isDr2) return "dr2";
            return null;
        }

        public static void ConvertFile(string filePath, string typeTo, bool convertIfInvalid = false)
        {
            string fileContent = File.ReadAllText(filePath, Encoding.ASCII);
            string typeFrom = Get(filePath);

            if (!convertIfInvalid && typeFrom == null) throw new InvalidDataException("The archive is corrupted!");
            string[] imageConversionFormat = new string[] { Formats[typeFrom], Formats[typeTo] };
            
            fileContent.Replace(imageConversionFormat[0], imageConversionFormat[1]);
            File.WriteAllText(filePath, fileContent, Encoding.ASCII);
        }
    }
}
