using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace amrts_store.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings
    {
        private amrts_store.Settings AppSettings = new amrts_store.Settings();
        private string ConfigPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\settings.json";
        private bool SettingsLoaded = false;
        public Settings()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    Load();
                }
                else
                {
                    Save();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                SettingsLoaded = true;
            }
        }

        private void Save()
        {
            AppSettings.General["game_client"] = general_game_client.Text;
            AppSettings.Security["game_client_mods"] = security_game_client_mods.IsOn;
            AppSettings.Security["enable_unknown_sources"] = security_unknown_sources_enable.IsOn;
            AppSettings.Security["enable_external_installation"] = security_enable_external_installation.IsChecked;

            AppSettings.StoreList.Clear();

            // Ignore the first item as it is the default one
            bool isTheDefaultEntry = true;
            foreach (ComboBoxItem comboBoxItem in security_store_list.Items)
            {
                if (!isTheDefaultEntry)
                    AppSettings.StoreList.Add(new string[] { (string)comboBoxItem.Content, (string)comboBoxItem.Tag });
                isTheDefaultEntry = false;
            }
            AppSettings.Security["store_list_id"] = security_store_list.SelectedIndex;
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(AppSettings, Formatting.Indented));
        }

        private void Load()
        {
            AppSettings = JsonConvert.DeserializeObject<amrts_store.Settings>(File.ReadAllText(ConfigPath));
            general_game_client.Text = (string)AppSettings.General["game_client"];
            security_game_client_mods.IsOn = (bool)AppSettings.Security["game_client_mods"];
            security_unknown_sources_enable.IsOn = (bool)AppSettings.Security["enable_unknown_sources"];
            security_enable_external_installation.IsChecked = (bool)AppSettings.Security["enable_external_installation"];

            security_store_list.Items.Clear();
            AppSettings.StoreList.Insert(0, amrts_store.Settings.DefaultStore);

            bool isTheDefaultEntry = true;
            foreach (string[] store in AppSettings.StoreList)
            {
                if (!isTheDefaultEntry)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = store[0];
                    comboBoxItem.Tag = store[1];
                    security_store_list.Items.Add(comboBoxItem);
                }
                isTheDefaultEntry = false;
            }

            // Indirect Int64 to int conversion
            security_store_list.SelectedIndex = int.Parse(AppSettings.Security["store_list_id"].ToString());
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsLoaded) Save();
        }

        private void Toggled(object sender, RoutedEventArgs e)
        {
            if (SettingsLoaded) Save();
        }
    }
}
