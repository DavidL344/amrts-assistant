﻿using System;
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
        public string NameWithVersion {
            get
            {
                return String.Format("{0} v{1}", AssemblyTitle, AssemblyVersion);
            }
        }
        private string LongLine {
            get
            {
                string temp = "";
                for (int i = 0; i < 126; i++) temp += "-";
                return temp;
            }
        }

        private readonly List<string[]> licenseList = new List<string[]>
        {
            // { <Component Name>, <License URL>, <License Type> }
            // <License Type> is only applied if there's neither online nor offline license information
            new string[] { "app", "https://raw.githubusercontent.com/DavidL344/amrts-assistant/master/LICENSE", "MIT" },
            new string[] { "ModernWpfUI", "https://raw.githubusercontent.com/Kinnara/ModernWpf/master/LICENSE", "MIT" },
            new string[] { "Newtonsoft.Json", "https://raw.githubusercontent.com/JamesNK/Newtonsoft.Json/master/LICENSE.md", "MIT" },
            new string[] { "Dark Reign 2 - Pack Utility", null, "MIT" }
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
                string licenseData = await LoadLicense(license[0], license[1], license[2]);

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

        public async Task<string> LoadLicense(string localName = null, string onlineUrl = null, string alternativeLicenseInfo = null)
        {
            if (onlineUrl == null) return LoadOfflineLicense(localName, alternativeLicenseInfo).Trim();
            string licenseData = "";

            try
            {
                using (HttpClient webClient = new HttpClient())
                {
                    licenseData = await webClient.GetStringAsync(new Uri(onlineUrl));
                }
            }
            catch (Exception e)
            {
                licenseData = LoadOfflineLicense(localName, alternativeLicenseInfo);
                licenseData = String.Format("Warning: This license might not be up-to-date.\r\nReason: {0}\r\nFetching attempted from: {1}\r\n{2}\r\n\r\n{3}", e.Message, onlineUrl, LongLine, licenseData.TrimStart());
            }
            finally
            {
                licenseData = licenseData.Trim();
            }
            return licenseData;
        }

        private string LoadOfflineLicense(string localName = null, string alternativeLicenseInfo = null)
        {
            byte[] licenseBytes;
            string licenseData = "";
            switch (localName)
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
                    return String.Format("This component is available under the following license: {0} (the direct link to the license couldn't be found)", alternativeLicenseInfo);
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
