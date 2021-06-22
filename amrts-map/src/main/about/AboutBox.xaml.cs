using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace amrts_map
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public string NameWithVersion
        {
            get
            {
                return String.Format("{0} v{1}", AssemblyTitle, AssemblyVersion);
            }
        }
        private string LongLine
        {
            get
            {
                string temp = "";
                for (int i = 0; i < 126; i++) temp += "-";
                return temp;
            }
        }

        private readonly List<string[]> licenseList = new List<string[]>
        {
            // { <Component Name>, <Component URL>, <License Type>, <License URL> }
            // <License Type> is only applied if there's neither online nor offline license information
            new string[] { "app", "https://github.com/DavidL344/amrts-assistant", "MIT", "https://raw.githubusercontent.com/DavidL344/amrts-assistant/master/LICENSE" },
            new string[] { "ModernWpfUI", "https://github.com/Kinnara/ModernWpf", "MIT", "https://raw.githubusercontent.com/Kinnara/ModernWpf/master/LICENSE" },
            new string[] { "Newtonsoft.Json", "https://github.com/JamesNK/Newtonsoft.Json", "MIT", "https://raw.githubusercontent.com/JamesNK/Newtonsoft.Json/master/LICENSE.md" },
            new string[] { "Dark Reign 2 - Pack Utility", "https://code.google.com/archive/p/darkreign2/", "MIT", null }
        };

        public AboutBox()
        {
            InitializeComponent();
            this.Title = String.Format("About {0}", AssemblyTitle);
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            foreach (string[] license in licenseList)
            {
                string licenseData = await LoadLicense(license[0], license[1], license[2], license[3]);

                if (licenseData == null) return;
                TabItem tabItem = new TabItem();
                tabItem.Header = license[0];

                TextBox textBox = new TextBox
                {
                    Text = licenseData,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                    TextWrapping = TextWrapping.Wrap,
                    IsReadOnly = true
                };

                Grid grid = new Grid();
                grid.Margin = new Thickness(10);
                grid.Children.Add(textBox);

                if (license[0] == "app") tabItem.Header = AssemblyTitle;
                tabItem.Content = grid;
                tc_licenseInfo.Items.Add(tabItem);
            }
            pr_status.IsActive = false;
            tc_licenseInfo.Visibility = Visibility.Visible;
        }

        public async Task<string> LoadLicense(string componentName = null, string componentUrl = null, string licenseType = null, string licenseUrl = null)
        {
            string licenseData = "";
            if (licenseUrl == null)
            {
                licenseData = LoadOfflineLicense(componentName, componentUrl, licenseType).Trim();
                licenseData = String.Format("Project URL: {0}\r\n\r\n{1}", componentUrl, licenseData);
                return licenseData;
            }

            try
            {
                using (HttpClient webClient = new HttpClient())
                {
                    licenseData = await webClient.GetStringAsync(new Uri(licenseUrl));
                }
            }
            catch (Exception e)
            {
                licenseData = LoadOfflineLicense(componentName, componentUrl, licenseType);
                licenseData = String.Format("Warning: This license might not be up-to-date.\r\nReason: {0}\r\nFetching attempted from: {1}\r\n{2}\r\n\r\n{3}", e.Message, licenseUrl, LongLine, licenseData.TrimStart());
            }
            finally
            {
                licenseData = licenseData.Trim();
                licenseData = String.Format("Project URL: {0}\r\n\r\n{1}", componentUrl, licenseData);
            }
            return licenseData;
        }

        private string LoadOfflineLicense(string componentName = null, string componentUrl = null, string licenseType = null)
        {
            byte[] licenseBytes;
            string licenseData = "";
            switch (componentName)
            {
                case null:
                case "app":
                    licenseBytes = Properties.Resources.LICENSE;
                    break;
                case "ModernWpfUI":
                    licenseBytes = Properties.Resources.LICENSE_ModernWpfUI;
                    break;
                case "Newtonsoft.Json":
                    licenseBytes = Properties.Resources.LICENSE_Newtonsoft_Json;
                    break;
                default:
                    return String.Format("This component is available under the following license: {0} (the direct link to the license couldn't be found)", licenseType);
            }
            using (MemoryStream stream = new MemoryStream(licenseBytes))
            using (StreamReader reader = new StreamReader(stream))
            {
                licenseData = reader.ReadToEnd();
            }
            return licenseData;
        }

        #region Assembly Attribute Accessors
        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
