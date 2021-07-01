using ModernWpf.Controls;
using System.Windows;

namespace amrts_map.Dialogs
{
    public partial class ClassicDialog : ContentDialog
    {
        public string DialogText
        {
            get => tb_text.Text;
            set => tb_text.Text = value;
        }

        public string CheckboxText
        {
            get => (string)chk_box.Content;
            set => chk_box.Content = value;
        }

        public bool CheckboxValue
        {
            get => (bool)chk_box.IsChecked;
        }

        public ClassicDialog(bool showTitle = true, bool showCheckbox = false)
        {
            InitializeComponent();

            if (!showTitle)
            {
                tb_text.Margin = new Thickness(0, 0, 0, 20);
                tb_text.FontSize = 18;
                tb_text.HorizontalAlignment = HorizontalAlignment.Center;
                tb_text.VerticalAlignment = VerticalAlignment.Center;
            }

            if (!showCheckbox) chk_box.Visibility = Visibility.Collapsed;
        }
    }
}
