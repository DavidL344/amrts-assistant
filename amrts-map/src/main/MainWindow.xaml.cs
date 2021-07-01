using amrts_map.Dialogs;
using ModernWpf.Controls;
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
            mi_menu_file_save.IsEnabled = mi_menu_file_save_as.IsEnabled = loaded;
            mi_menu_edit_discard_changes.IsEnabled = mi_menu_edit_run_studio.IsEnabled = loaded;
            mi_menu_project_item_new.IsEnabled = mi_menu_project_item_add.IsEnabled = loaded;
            mi_menu_project_show_explorer.IsEnabled = mi_menu_project_show_terminal.IsEnabled = loaded;
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
            string action = null;
            if (InternalMethods.IsThisOrSimilarKeyDown(Key.LeftCtrl))
            {
                // Ctrl+*
                switch (e.Key)
                {
                    case Key.N:
                        action = "file_new";
                        break;
                    case Key.O:
                        action = "file_open";
                        break;
                    case Key.S:
                        action = InternalMethods.IsThisOrSimilarKeyDown(Key.LeftShift) ? "file_save_as" : "file_save";
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
                        break;
                }

                if (InternalMethods.IsThisOrSimilarKeyDown(Key.LeftAlt))
                {
                    // Ctrl+Alt+*
                    switch (e.Key)
                    {
                        case Key.E:
                            action = "project_show_explorer";
                        break;
                        case Key.T:
                            action = "project_show_terminal";
                        break;
                    }
                }
            }

            if (InternalMethods.IsThisOrSimilarKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.A))
            {
                // *+Shift+A
                if (InternalMethods.IsThisOrSimilarKeyDown(Key.LeftCtrl)) action = "project_item_new";
                if (InternalMethods.IsThisOrSimilarKeyDown(Key.LeftAlt)) action = "project_item_add";
            }

            if (action != null) PerformAction(action);
        }

        public async void PerformAction(string action)
        {
            bool initialized;
            try
            {
                initialized = !OpenedProject.Initialized;
            }
            catch
            {
                initialized = false;
            }
            if (initialized)
            {
                switch (action.ToLower())
                {
                    case "file_save":
                    case "file_save_as":
                    case "edit_discard_changes":
                    case "edit_run_studio":
                    case "project_item_new":
                    case "project_item_add":
                    case "project_show_explorer":
                    case "project_show_terminal":
                    case "build_build_project":
                    case "build_clean_project":
                        return;
                    default:
                        break;
                }
            }

            try
            {
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
                    case "edit_discard_changes":
                        ContentDialogResult contentDialogResult = await Dialog.Show("Discard changes?", null, DialogButton.YesNo, ContentDialogButton.Secondary);
                        if (contentDialogResult == ContentDialogResult.Primary) Project.DiscardChanges(OpenedProject);
                        break;
                    case "edit_run_studio":
                    case "project_item_new":
                        await Dialog.Show("Coming Soon!");
                        break;
                    case "project_item_add":
                        Project.AddFile(OpenedProject);
                        break;
                    case "project_show_explorer":
                        InternalMethods.Run(InternalMethods.GetProcessStartInfo("explorer.exe"), OpenedProject.Project["Root"]);
                        break;
                    case "project_show_terminal":
                        InternalMethods.Run(InternalMethods.GetProcessStartInfo("cmd.exe"), null, OpenedProject.Project["Root"]);
                        break;
                    case "build_build_project":
                        Project.Build(OpenedProject);
                        break;
                    case "build_clean_project":
                        Project.CleanExport(OpenedProject);
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
            catch (Exception e)
            {
                InternalMethods.CatchException(e, false);
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

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Project.AddFile(OpenedProject, files);
        }
    }
}
