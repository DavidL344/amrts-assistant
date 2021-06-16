using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace amrts_map
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string ProjectPath;
        private OpenedProject OpenedProject;
        private TabControl mainTabControl;

        public MainWindow(OpenedProject openedProject)
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(this.ActionListener);
            this.OpenedProject = openedProject;
            ProjectLoaded(OpenedProject.Initialized);
        }

        private void MenuItemClicked(object sender, RoutedEventArgs e)
        {
            string menuItemName = (sender as FrameworkElement).Name.ToString().Split(new string[] { "mi_menu_" }, StringSplitOptions.RemoveEmptyEntries)[0];
            PerformAction(menuItemName);
        }

        private void ProjectLoaded(bool loaded)
        {
            mi_menu_file_save.IsEnabled = mi_menu_file_save_as.IsEnabled = mi_menu_file_export.IsEnabled = loaded;
            mi_menu_edit_discard_changes.IsEnabled = mi_menu_edit_run_studio.IsEnabled = loaded;
            mi_menu_build_build_project.IsEnabled = mi_menu_build_clean_project.IsEnabled = loaded;
            tc_main.Visibility = loaded ? Visibility.Visible : Visibility.Hidden;

            this.Title = InternalMethods.Name;
            if (OpenedProject.Project["Name"] != null)
            {
                this.Title = String.Format("{0} | {1}", OpenedProject.Project["Name"], this.Title);
                ti_projectName.Header = OpenedProject.Project["Name"];
            }
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
                    case Key.Delete:
                        action = "edit_discard_changes";
                        break;
                    case Key.R:
                        action = "edit_run_studio";
                        break;
                    case Key.B:
                        action = "build_build_project";
                        break;
                    default:
                        return;
                }
                PerformAction(action);
            }
        }

        public void PerformAction(string action)
        {
            if (!OpenedProject.Initialized)
            {
                switch (action.ToLower())
                {
                    case "file_save":
                    case "file_save_as":
                    case "file_export":
                    case "edit_discard_changes":
                    case "edit_run_studio":
                    case "build_build_project":
                    case "build_clean_project":
                        return;
                    default:
                        break;
                }
            }

            switch (action.ToLower())
            {
                case "file_new":
                    InternalMethods.Run(null, "-new");
                    break;
                case "file_open":
                    InternalMethods.Run(null, "-open");
                    break;
                case "file_save":
                    Project.Save(OpenedProject);
                    break;
                case "file_save_as":
                    Project.SaveAs(OpenedProject);
                    break;
                case "file_import":
                case "file_export":
                    MessageBox.Show("Coming Soon!", InternalMethods.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "edit_discard_changes":
                    MessageBoxResult messageBoxResult = MessageBox.Show("Discard changes?", InternalMethods.Name, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    if (messageBoxResult == MessageBoxResult.Yes) Project.DiscardChanges(OpenedProject);
                    break;
                case "edit_run_studio":
                    MessageBox.Show("Coming Soon!", InternalMethods.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "build_build_project":
                    Project.Build(OpenedProject);
                    break;
                case "build_clean_project":
                    Project.Clean(OpenedProject);
                    break;
                case "help_about":
                    Dialog.About(this);
                    break;
                case "file_close":
                    Project.Close(OpenedProject);
                    this.Close();
                    break;
                case "file_exit":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        private void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            mainTabControl = (sender as TabControl);

            // Set sample tabs
            string[] sampleTabNames = new string[] { "game.cfg", "objects.cfg", "terrain.blk", "terrain.tga", "types.cfg", "engine__loading__back.tga" };
            foreach (string sampleTabName in sampleTabNames)
            {
                TabItem tabItem = new TabItem();
                tabItem.Header = sampleTabName;
                mainTabControl.Items.Add(tabItem);
            }
        }
    }
}
