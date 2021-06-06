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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace amrts_map
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(this.ActionListener);
        }

        private void MenuItemClicked(object sender, RoutedEventArgs e)
        {
            string menuItemName = (sender as FrameworkElement).Name.ToString().Split(new string[] { "btn_menu_" }, StringSplitOptions.RemoveEmptyEntries)[0];
            PerformAction(menuItemName);
        }

        public void ActionListener(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.N:
                        PerformAction("file_new");
                        break;
                    case Key.O:
                        PerformAction("file_open");
                        break;
                    case Key.S:
                        string action = (Keyboard.IsKeyDown(Key.LeftShift)) ? "file_save_as" : "file_save";
                        PerformAction(action);
                        break;
                    case Key.I:
                        PerformAction("file_import");
                        break;
                    case Key.E:
                        PerformAction("file_export");
                        break;
                    default:
                        break;
                }
            }
        }

        public void PerformAction(string action)
        {
            switch (action.ToLower())
            {
                case "file_new":
                case "file_open":
                case "file_save":
                case "file_save_as":
                case "file_import":
                case "file_export":
                case "edit_discard_changes":
                case "build_build_project":
                case "build_clean_project":
                    MessageBox.Show("Coming Soon!", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "file_close":
                    this.Close();
                    break;
                case "file_exit":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}
