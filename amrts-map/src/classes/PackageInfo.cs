using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amrts_map
{
    public class PackageInfo
    {
        public string Name;
        public string Description;
        public string Author;
        public int[] Version;
        public Dictionary<string, bool> Flags;
        public Dictionary<string, string> Schema
        {
            get
            {
                return new Dictionary<string, string>{
                    { "Version", "0-preview" },
                    { "AppVersion", InternalMethods.Version }
                };
            }
        }
        public List<string[]> Dependencies;

        public PackageInfo()
        {
            Reset();
        }

        public void Reset()
        {
            Name = "Untitled Package";
            Description = "A sample description for the package.";
            Author = "Unknown Author";
            Version = new int[] { 1, 0, 0, 0 };
            Flags = new Dictionary<string, bool>
            {
                { "Debug", true },
                { "RequiresCleanGame", true }
            };
            Dependencies = new List<string[]>();
        }
    }
}
