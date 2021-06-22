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
using System.Windows.Shapes;
using Ookii.Dialogs.Wpf;

namespace amrts_map
{
    /// <summary>
    /// Interaction logic for WelcomeScreen.xaml
    /// </summary>
    public partial class WelcomeScreen : Window
    {
        public List<string[]> RecentlyOpenedFiles = new List<string[]>();
        public OpenedProject OpenedProject;

        public WelcomeScreen()
        {
            InitializeComponent();
            LoadRecentlyOpenedFiles();
            HandleArgs();
        }

        private void LoadRecentlyOpenedFiles(bool loadExternally = true)
        {
            lb_recent.Items.Clear();
            if (loadExternally) RecentlyOpenedFiles = RecentlyOpened.Data;
            else
            {
                // Create samples
                for (int i = 1; i <= 10; i++)
                    RecentlyOpenedFiles.Add(new string[] { String.Format("Sample Project {0}", i), Environment.ExpandEnvironmentVariables(String.Format(@"%userprofile%\Desktop\Project{0}\Project{0}.amramp", i)) });
            }
            foreach (string[] value in RecentlyOpenedFiles) lb_recent.Items.Add(String.Format("{0}\r\n{1}{2}", value[0], "          ", value[1]));
        }

        private void ClearRecentlyOpenedFiles(object sender, RoutedEventArgs e)
        {
            RecentlyOpened.Clear();
            lb_recent.Items.Clear();
        }

        private async void OpenRecentFile(bool debug = false)
        {
            if (Keyboard.IsKeyDown(Key.Escape)) lb_recent.SelectedIndex = -1;
            if (lb_recent.SelectedIndex == -1) return;
            string selectedItem = (string)lb_recent.SelectedItem;
            string[] selectedItemArray = selectedItem.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int selectedIndex = lb_recent.SelectedIndex;
            
            try
            {
                if (debug) await Dialog.Show(String.Format("Selected ID: {0}\r\nItem Name: {1}\r\nItem Path: {2}", selectedIndex, selectedItemArray[0], selectedItemArray[1].Trim()));
                OpenedProject = Project.Open(selectedItemArray[1].Trim());
                OpenMainWindow(OpenedProject);
            }
            catch (Exception e)
            {
                InternalMethods.CatchException(e, false);
            }
        }

        private void lb_recent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenRecentFile();
        }

        private void lb_recent_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter)) OpenRecentFile();
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Name.ToString().Split(new string[] { "btn_" }, StringSplitOptions.RemoveEmptyEntries)[0];
            string[] buttonNameTypes = new string[] { "start_", "project_", "map_" };

            foreach (string buttonNameType in buttonNameTypes)
            {
                if (buttonName.StartsWith(buttonNameType))
                {
                    buttonName = buttonName.Split(new string[] { buttonNameType }, StringSplitOptions.RemoveEmptyEntries)[0];
                    PerformAction(buttonName);
                    break;
                }
            }
        }

        private async void PerformAction(string action, bool debug = false)
        {
            try
            {
                switch (action.ToLower())
                {
                    case "new":
                    case "new_back":
                        SwitchUI("project_new");
                        break;
                    case "new_location_browse":
                        VistaFolderBrowserDialog projectLocation = Dialog.BrowseFolder();
                        if (projectLocation != null) txt_project_new_location.Text = projectLocation.SelectedPath;
                        break;
                    case "new_map_location_browse":
                        VistaOpenFileDialog mapLocation = Dialog.OpenFile("Select a map", String.Format("{0}|*.{1}", Project.MapTypeName, Project.MapExtension));
                        if (mapLocation != null) txt_project_new_map_location.Text = mapLocation.FileName;
                        break;
                    case "new_create":
                        try
                        {
                            OpenedProject = Project.New(txt_project_name.Text, txt_project_new_location.Text, txt_project_new_map_location.Text);
                            OpenMainWindow(OpenedProject);
                        }
                        catch (Exception e)
                        {
                            InternalMethods.CatchException(e, false);
                        }
                        break;
                    case "open":
                        VistaOpenFileDialog openProjectDialog = Dialog.OpenFile("Open a project", String.Format("{0}|*.{1}", Project.FileTypeName, Project.FileExtension));
                        if (openProjectDialog != null)
                        {
                            try
                            {
                                if (debug)
                                {
                                    string info = String.Format("Selected file: {0}", openProjectDialog.FileName);
                                    await Dialog.Show(info);
                                }

                                OpenedProject = Project.Open(openProjectDialog.FileName);
                                OpenMainWindow(OpenedProject);
                            }
                            catch (Exception e)
                            {
                                InternalMethods.CatchException(e, false);
                            }
                        }
                        break;
                    case "packer":
                    case "packer_back":
                        SwitchUI("map_packer");
                        break;
                    case "packer_mapfile_location_browse":
                        VistaOpenFileDialog mapLocationPack = Dialog.OpenFile("Select a map", String.Format("{0}|*.{1}", Project.MapTypeName, Project.MapExtension));
                        if (mapLocationPack != null) txt_map_packer_mapfile_location.Text = mapLocationPack.FileName;
                        break;
                    case "packer_directory_location_browse":
                        VistaFolderBrowserDialog projectLocationPack = Dialog.BrowseFolder();
                        if (projectLocationPack != null) txt_map_packer_directory_location.Text = projectLocationPack.SelectedPath;
                        break;
                    case "packer_extract":
                        DrPack.Bridge.Run.Extract(txt_map_packer_mapfile_location.Text, txt_map_packer_directory_location.Text);
                        break;
                    case "packer_create":
                        DrPack.Bridge.Run.Create(txt_map_packer_mapfile_location.Text, txt_map_packer_directory_location.Text);
                        break;
                    case "packer_save_as":
                        VistaSaveFileDialog folderBrowserDialog = Dialog.SaveFile("Pack Map As...", String.Format("{0}|*.{1}", Project.MapTypeName, Project.MapExtension));
                        if (folderBrowserDialog != null) DrPack.Bridge.Run.Create(folderBrowserDialog.FileName, txt_map_packer_directory_location.Text);
                        break;
                    case "empty":
                        OpenMainWindow();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                // If the exception is a warning, it was expected
                InternalMethods.CatchException(e, e is System.ComponentModel.WarningException);
            }
        }

        private void SwitchUI(string type = "main")
        {
            if (grid_main.Visibility != Visibility.Visible) type = "main";
            switch (type)
            {
                case "main":
                    grid_project_new.Visibility = Visibility.Hidden;
                    grid_map_packer.Visibility = Visibility.Hidden;
                    grid_main.Visibility = Visibility.Visible;
                    break;
                case "project_new":
                    if (string.IsNullOrWhiteSpace(txt_project_name.Text)) txt_project_name.Text = Project.DefaultName;
                    if (string.IsNullOrWhiteSpace(txt_project_new_location.Text))
                        txt_project_new_location.Text = Project.DefaultLocation;
                    if (string.IsNullOrWhiteSpace(txt_project_new_map_location.Text))
                        txt_project_new_map_location.Text = Project.DefaultMapLocation;

                    grid_main.Visibility = Visibility.Hidden;
                    grid_map_packer.Visibility = Visibility.Hidden;
                    grid_project_new.Visibility = Visibility.Visible;
                    break;
                case "map_packer":
                    grid_main.Visibility = Visibility.Hidden;
                    grid_project_new.Visibility = Visibility.Hidden;
                    grid_map_packer.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private async void OpenMainWindow(OpenedProject openedProject = null)
        {
            lb_recent.SelectedIndex = -1; // Reset the selected recent file (if applicable)
            try
            {
                if (openedProject == null) openedProject = new OpenedProject();
                if (openedProject.Initialized)
                {
                    if (openedProject.Project["Path"] != null && !File.Exists(openedProject.Project["Path"]))
                    {
                        await Dialog.Show("The file doesn't exist!");
                        return;
                    }
                    RecentlyOpened.Add(openedProject.Project["Name"], openedProject.Project["Path"]);
                }

                MainWindow mainWindow = new MainWindow(openedProject);
                this.Hide();

                OpenedProject = null;
                txt_project_name.Text = null;
                txt_project_new_location.Text = null;
                txt_project_new_map_location.Text = null;
                SwitchUI();
                LoadRecentlyOpenedFiles();

                mainWindow.ShowDialog();
                this.Show();
            }
            catch (Exception e)
            {
                InternalMethods.CatchException(e, false);
            }
        }

        private async void FileDropped(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length != 1)
                {
                    await Dialog.Show("Only one file is supported at a time.");
                    return;
                }

                try
                {
                    OpenedProject = Project.Open(files[0]);
                    OpenMainWindow(OpenedProject);
                }
                catch (Exception)
                {
                    try
                    {
                        throw new InvalidDataException("The map file is invalid!");
                    }
                    catch (Exception ex)
                    {
                        InternalMethods.CatchException(ex, true);
                    }
                }
            }
        }

        public void HandleArgs(string[] args = null)
        {
            if (args == null) args = App.Args;
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "-new":
                        if (InternalMethods.MultipleInstancesRunning()) btn_project_new_back.Visibility = Visibility.Hidden;
                        SwitchUI("project_new");
                        break;
                    case "-pack":
                        if (InternalMethods.MultipleInstancesRunning()) btn_map_packer_back.Visibility = Visibility.Hidden;
                        SwitchUI("map_packer");
                        break;
                    case "-window":
                        OpenMainWindow();
                        this.Close();
                        break;
                    case "-open":
                        try
                        {
                            if (args.Length == 2)
                            {
                                OpenedProject = Project.Open(args[1]);
                                OpenMainWindow(OpenedProject);
                            }
                            else
                            {
                                PerformAction("open");
                            }
                        }
                        catch (Exception e)
                        {
                            InternalMethods.CatchException(e, false);
                        }
                        finally
                        {
                            this.Close();
                        }
                        break;
                    default:
                        HandleArgs(new string[] { "-open", args[0] });
                        break;
                }
            }
        }
    }
}
