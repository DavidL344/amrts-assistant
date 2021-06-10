using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public string NameWithVersion {
            get
            {
                return String.Format("{0} v{1}", AssemblyTitle, AssemblyVersion);
            }
        }

        public AboutBox()
        {
            InitializeComponent();
            this.Title = String.Format("About {0}", AssemblyTitle);

            List<string[]> licenseList = new List<string[]>
            {
                new string[] { "app", "https://raw.githubusercontent.com/DavidL344/amrts-assistant/master/LICENSE" },
                new string[] { "ModernWpfUI", "https://raw.githubusercontent.com/Kinnara/ModernWpf/master/LICENSE" }
            };

            foreach (string[] license in licenseList)
            {
                string licenseData = LoadLicense(license[0], license[1]);

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
        }

        public string LoadLicense(string localName = null, string onlineUrl = null)
        {
            string licenseData = "";
            try
            {
                using (System.Net.WebClient webClient = new System.Net.WebClient()) licenseData = webClient.DownloadString(new Uri(onlineUrl));
            }
            catch (Exception e)
            {
                byte[] licenseBytes;
                switch (localName)
                {
                    case null:
                    case "app":
                        licenseBytes = Properties.Resources.LICENSE;
                        break;
                    case "ModernWpfUI":
                        licenseBytes = Properties.Resources.LICENSE_ModernWpfUI;
                        break;
                    default:
                        return null;
                }
                using (MemoryStream stream = new MemoryStream(licenseBytes))
                using (StreamReader reader = new StreamReader(stream))
                {
                    licenseData = reader.ReadToEnd();
                }

                string longLine = "";
                for (int i = 0; i < 126; i++) longLine += "-";
                licenseData = String.Format("Warning: This license might not be up-to-date.\r\nReason: {0}\r\nFetching attempted from: {1}\r\n{2}\r\n\r\n{3}", e.Message, onlineUrl, longLine, licenseData.TrimStart());
            }
            finally
            {
                licenseData = licenseData.Trim();
            }
            return licenseData;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
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

        public string AssemblyVersion
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
