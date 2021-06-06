using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for WelcomeScreen.xaml
    /// </summary>
    public partial class WelcomeScreen : Window
    {
        public List<string[]> RecentlyOpenedFiles = new List<string[]>();
        public WelcomeScreen()
        {
            InitializeComponent();
            LoadRecentlyOpenedFiles();
        }

        private void LoadRecentlyOpenedFiles(bool loadExternally = false)
        {
            if (loadExternally) throw new NotImplementedException();

            // Create samples
            for (int i = 1; i <= 10; i++)
                RecentlyOpenedFiles.Add(new string[] { String.Format("Sample Project {0}", i), Environment.ExpandEnvironmentVariables(String.Format(@"%userprofile%\Desktop\Project{0}\Project{0}.amramp", i)) });
            foreach (string[] value in RecentlyOpenedFiles) lb_recent.Items.Add(String.Format("{0}\r\n{1}{2}", value[0], "          ", value[1]));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_recent.SelectedIndex == -1) return;
            string selectedItem = (string)lb_recent.SelectedItem;
            string[] selectedItemArray = selectedItem.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int selectedIndex = lb_recent.SelectedIndex;
            MessageBox.Show(String.Format("Selected ID: {0}\r\nItem Name: {1}\r\nItem Path: {2}", selectedIndex, selectedItemArray[0], selectedItemArray[1].Trim()));
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Name.ToString().Split(new string[] { "btn_" }, StringSplitOptions.RemoveEmptyEntries)[0];
            if (buttonName.StartsWith("start_"))
            {
                buttonName = buttonName.Split(new string[] { "start_" }, StringSplitOptions.RemoveEmptyEntries)[0];
                PerformAction(buttonName);
                return;
            }
        }

        private void PerformAction(string action)
        {
            switch (action.ToLower())
            {
                case "new":
                case "open":
                case "import":
                    MessageBox.Show("Coming Soon!", "Map Assistant for Army Men RTS", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "empty":
                    MainWindow mainWindow = new MainWindow();
                    this.Hide();
                    mainWindow.ShowDialog();
                    this.Show();
                    break;
                default:
                    break;
            }
        }
    }
}
