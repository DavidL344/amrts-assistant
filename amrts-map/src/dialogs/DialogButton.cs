using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amrts_map.Dialogs
{
    public static class DialogButton
    {
        public static string[] OK => new string[] { "OK" };
        public static string[] YesNo => new string[] { "Yes", "No" };
        public static string[] OKCancel => new string[] { "OK", null, "Cancel" };
        public static string[] OKClipboard => new string[] { "OK", "Copy to clipboard" };
        public static string[] YesNoCancel => new string[] { "Yes", "No", "Cancel" };
    }
}
