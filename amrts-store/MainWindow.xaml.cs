using amrts_store.Pages;
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

namespace amrts_store
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavigateToPage(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(Pages.Settings));
            }
            else
            {
                var selectedItem = (ModernWpf.Controls.NavigationViewItem)args.SelectedItem;
                string pageName = "amrts_store.Pages." + (string)selectedItem.Tag;
                Type pageType = typeof(Home).Assembly.GetType(pageName);
                if (pageType != null) contentFrame.Navigate(pageType);
            }
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            nv_main.SelectedItem = nv_main.MenuItems[0];
            contentFrame.Navigate(typeof(Home));
        }
    }
}
