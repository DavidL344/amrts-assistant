using Microsoft.Win32;
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
using Path = System.IO.Path;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

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

        private void ShowSampleRecentsInfo()
        {
            if (Keyboard.IsKeyDown(Key.Escape)) lb_recent.SelectedIndex = -1;
            if (lb_recent.SelectedIndex == -1) return;
            string selectedItem = (string)lb_recent.SelectedItem;
            string[] selectedItemArray = selectedItem.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int selectedIndex = lb_recent.SelectedIndex;
            MessageBox.Show(String.Format("Selected ID: {0}\r\nItem Name: {1}\r\nItem Path: {2}", selectedIndex, selectedItemArray[0], selectedItemArray[1].Trim()));

            OpenedProject recentProject = new OpenedProject();
            try
            {
                recentProject.Open(selectedItemArray[1].Trim());
                OpenMainWindow(recentProject);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Map Assistant for Army Men RTS", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lb_recent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowSampleRecentsInfo();
        }

        private void lb_recent_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter)) ShowSampleRecentsInfo();
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Name.ToString().Split(new string[] { "btn_" }, StringSplitOptions.RemoveEmptyEntries)[0];
            string[] buttonNameTypes = new string[] { "start_", "project_" };

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

        private void PerformAction(string action)
        {
            switch (action.ToLower())
            {
                case "new":
                case "new_back":
                    if (string.IsNullOrWhiteSpace(txt_project_name.Text)) txt_project_name.Text = "ArmyMenMap1";
                    if (string.IsNullOrWhiteSpace(txt_project_new_location.Text))
                        txt_project_new_location.Text = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\Projects");
                    if (string.IsNullOrWhiteSpace(txt_project_new_map_location.Text))
                        txt_project_new_map_location.Text = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\3DO\Army Men RTS\missions\mp\8ally.x");
                    SwitchUI();
                    break;
                case "new_location_browse":
                    FolderBrowserDialog projectLocation = Dialog.BrowseFolder();
                    if (projectLocation != null) txt_project_new_location.Text = projectLocation.SelectedPath;
                    break;
                case "new_map_location_browse":
                    OpenFileDialog mapLocation = Dialog.OpenFile("Select a map", "Army Men RTS Map File|*.x");
                    if (mapLocation != null) txt_project_new_map_location.Text = mapLocation.FileName;
                    break;
                case "new_create":
                    OpenedProject openedProject = new OpenedProject();
                    try
                    {
                        openedProject.New(txt_project_name.Text, txt_project_new_location.Text, txt_project_new_map_location.Text);
                        OpenMainWindow(openedProject);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Map Assistant for Army Men RTS", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case "open":
                    OpenFileDialog openProjectDialog = Dialog.OpenFile("Open a project", "Map Project|*.amramp");
                    if (openProjectDialog != null)
                    {
                        string info = String.Format("Selected file: {0}", openProjectDialog.FileName);
                        MessageBox.Show(info, "Map Assistant for Army Men RTS", MessageBoxButton.OK, MessageBoxImage.Information);

                        OpenedProject selectedProject = new OpenedProject();
                        try
                        {
                            selectedProject.Open(openProjectDialog.FileName);
                            OpenMainWindow(selectedProject);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Map Assistant for Army Men RTS", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    break;
                case "import":
                    OpenFileDialog importFileDialog = Dialog.OpenFile("Import a map", "Army Men RTS Map File|*.x");
                    if (importFileDialog != null)
                    {
                        string additionalFile = "(not detected)";
                        if (File.Exists(Path.ChangeExtension(importFileDialog.FileName, ".x-e")))
                        {
                            additionalFile = Path.ChangeExtension(importFileDialog.FileName, ".x-e");
                        }
                        string info = String.Format("Selected file: {0}\r\nAdditional file: {1}", importFileDialog.FileName, additionalFile);
                        MessageBox.Show(info, "Map Assistant for Army Men RTS", MessageBoxButton.OK, MessageBoxImage.Information);

                        OpenedProject importedProject = new OpenedProject();
                        importedProject.Open(importFileDialog.FileName);
                        importedProject.Project["Name"] = Path.GetFileName(importFileDialog.FileName);
                        importedProject.Project["Path"] = null;
                        OpenMainWindow(importedProject);
                    }
                    break;
                case "empty":
                    OpenMainWindow();
                    break;
                default:
                    break;
            }
        }

        private void SwitchUI(string type = null)
        {
            switch (type)
            {
                case null:
                    if (grid_main.Visibility == Visibility.Visible)
                    {
                        SwitchUI("project_new");
                        return;
                    }
                    SwitchUI("main");
                    break;
                case "main":
                    grid_project_new.Visibility = Visibility.Hidden;
                    grid_main.Visibility = Visibility.Visible;
                    break;
                case "project_new":
                    grid_main.Visibility = Visibility.Hidden;
                    grid_project_new.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void OpenMainWindow(OpenedProject openedProject = null)
        {
            lb_recent.SelectedIndex = -1; // Reset the selected recent file (if applicable)
            try
            {
                if (openedProject == null) openedProject = new OpenedProject();
                if (openedProject.Initialized && openedProject.Project["Path"] != null && !File.Exists(openedProject.Project["Path"]))
                {
                    MessageBox.Show("The file doesn't exist!", "Map Assistant for Army Men RTS", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MainWindow mainWindow = new MainWindow(openedProject);
                this.Hide();

                txt_project_name.Text = null;
                txt_project_new_location.Text = null;
                txt_project_new_map_location.Text = null;
                SwitchUI("main");

                mainWindow.ShowDialog();
                this.Show();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Map Assistant for Army Men RTS", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
