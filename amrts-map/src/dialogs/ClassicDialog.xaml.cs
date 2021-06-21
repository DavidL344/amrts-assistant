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

        public ClassicDialog(bool showTitle = true)
        {
            InitializeComponent();

            if (!showTitle)
            {
                tb_text.Margin = new Thickness(0, 0, 0, 20);
                tb_text.FontSize = 18;
                tb_text.HorizontalAlignment = HorizontalAlignment.Center;
                tb_text.VerticalAlignment = VerticalAlignment.Center;
            }
        }
    }
}
