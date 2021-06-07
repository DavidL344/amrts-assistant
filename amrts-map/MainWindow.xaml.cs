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
        public MainWindow(string projectPath = null)
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(this.ActionListener);

            if (projectPath == null)
            {
                mi_menu_build_build_project.IsEnabled = false;
            }
        }

        private void MenuItemClicked(object sender, RoutedEventArgs e)
        {
            string menuItemName = (sender as FrameworkElement).Name.ToString().Split(new string[] { "mi_menu_" }, StringSplitOptions.RemoveEmptyEntries)[0];
            PerformAction(menuItemName);
        }

        public void ActionListener(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                string action;
                switch (e.Key)
                {
                    case Key.N:
                        action = "file_new";
                        break;
                    case Key.O:
                        action = "file_open";
                        break;
                    case Key.S:
                        action = (Keyboard.IsKeyDown(Key.LeftShift)) ? "file_save_as" : "file_save";
                        break;
                    case Key.I:
                        action = "file_import";
                        break;
                    case Key.E:
                        action = "file_export";
                        break;
                    case Key.R:
                        action = "edit_run_studio";
                        break;
                    default:
                        return;
                }
                PerformAction(action);
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
                case "edit_run_studio":
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
