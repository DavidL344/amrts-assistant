using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amrts_store
{
    public class Settings
    {
        public Dictionary<string, object> General;
        public Dictionary<string, object> Security;
        public List<string[]> StoreList;
        public static string[] DefaultStore = new string[] { "Army Men RTS Store (GitHub)", null };

        public Settings()
        {
            Reset();
        }

        public void Reset(string type = null)
        {
            string[] types = new string[] { "general", "security", "store_list" };
            switch (type)
            {
                case "general":
                    General = new Dictionary<string, object>()
                    {
                        { "game_client", null }
                    };
                    break;
                case "security":
                    Security = new Dictionary<string, object>()
                    {
                        { "game_client_mods", false },
                        { "enable_unknown_sources", false },
                        { "enable_external_installation", false },
                        { "store_list_id", 0 }
                    };
                    break;
                case "store_list":
                    StoreList = new List<string[]>
                    {
                        DefaultStore
                    };
                    break;
                case null:
                    foreach (string typeInArray in types)
                    {
                        Reset(typeInArray);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
