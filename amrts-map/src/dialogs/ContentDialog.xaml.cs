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

namespace amrts_map.Dialogs
{
    /// <summary>
    /// Interaction logic for ContentDialog.xaml
    /// </summary>
    public partial class ContentDialog
    {
        public ContentDialog()
        {
            InitializeComponent();

            cb_main.IsEnabled = false;
            this.Title = "Coming Soon!";

            /*
            string[] files = DirectoryMethods.GetAllFiles(@<extracted base.x>, "*.cfg");
            foreach (string file in files)
            {
                string fileName = System.IO.Path.GetFileName(file);
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = comboBoxItem.Tag = fileName;
                cb_main.Items.Add(comboBoxItem);
            }
            */
        }
    }
}
