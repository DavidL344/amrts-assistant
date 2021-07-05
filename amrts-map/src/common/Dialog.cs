using amrts_map.Dialogs;
using ModernWpf.Controls;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace amrts_map
{
    public class Dialog
    {
        public static VistaOpenFileDialog OpenFile(string title = "Open file", string filter = "All Supported Files|*.amramp;*.x")
        {
            VistaOpenFileDialog openFile = new VistaOpenFileDialog
            {
                Title = title,
                Filter = filter,
                CheckFileExists = true,
                CheckPathExists = true,
                DereferenceLinks = true
            };

            if ((bool)openFile.ShowDialog()) return openFile;
            return null;
        }

        public static VistaSaveFileDialog SaveFile(string title = "Save file", string filter = "All Supported Files|*.amramp;*.x")
        {
            VistaSaveFileDialog saveFile = new VistaSaveFileDialog
            {
                Title = title,
                Filter = filter,
                CheckPathExists = true,
                DereferenceLinks = true
            };
            if ((bool)saveFile.ShowDialog()) return saveFile;
            return null;
        }

        public static VistaFolderBrowserDialog BrowseFolder(string title = "Browse folder", bool newFolderButton = true)
        {
            VistaFolderBrowserDialog browseFolder = new VistaFolderBrowserDialog
            {
                Description = title,
                ShowNewFolderButton = newFolderButton,
                UseDescriptionForTitle = true
            };

            if ((bool)browseFolder.ShowDialog()) return browseFolder;
            return null;
        }

        public static void About(Window owner)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Owner = owner;
            aboutBox.ShowDialog();
        }

        public static async Task<ContentDialogResult> Show(string dialogText, string caption = null, string[] buttonText = null, ContentDialogButton defaultButton = ContentDialogButton.Primary)
        {
            int buttonTextArrayLength = 3;
            bool showTitle = caption != null;
            if (buttonText == null) buttonText = DialogButton.OK;

            if (buttonText.Length != buttonTextArrayLength)
            {
                List<string> buttonTextList = buttonText.OfType<string>().ToList();
                for (int i = buttonTextList.Count; i <= buttonTextArrayLength; i++)
                    buttonTextList.Add(null);
                buttonText = buttonTextList.ToArray();
            }

            for (int i = 0; i < buttonText.Length; i++)
                if (string.IsNullOrEmpty(buttonText[i])) buttonText[i] = "";

            ClassicDialog dialog = new ClassicDialog(showTitle)
            {
                Title = caption ?? "",
                DialogText = dialogText,
                PrimaryButtonText = buttonText[0],
                SecondaryButtonText = buttonText[1],
                CloseButtonText = buttonText[2],
                DefaultButton = defaultButton,
            };

            // ContentDialogResult.Primary, ContentDialogResult.Secondary, ContentDialogResult.None
            return await dialog.ShowAsync();
        }

        public static async Task<object[]> ShowWithCheckbox(string dialogText, string caption = null, string[] buttonText = null, ContentDialogButton defaultButton = ContentDialogButton.Primary, string checkboxText = null)
        {
            int buttonTextArrayLength = 3;
            bool showTitle = caption != null;
            if (buttonText == null) buttonText = DialogButton.OK;

            if (buttonText.Length != buttonTextArrayLength)
            {
                List<string> buttonTextList = buttonText.OfType<string>().ToList();
                for (int i = buttonTextList.Count; i <= buttonTextArrayLength; i++)
                    buttonTextList.Add(null);
                buttonText = buttonTextList.ToArray();
            }

            for (int i = 0; i < buttonText.Length; i++)
                if (string.IsNullOrEmpty(buttonText[i])) buttonText[i] = "";

            ClassicDialog dialog = new ClassicDialog(showTitle, true)
            {
                Title = caption ?? "",
                DialogText = dialogText,
                CheckboxText = checkboxText ?? "",
                PrimaryButtonText = buttonText[0],
                SecondaryButtonText = buttonText[1],
                CloseButtonText = buttonText[2],
                DefaultButton = defaultButton,
            };

            ContentDialogResult returnValue = await dialog.ShowAsync();
            return new object[] { returnValue, dialog.CheckboxValue };
        }

        public static async Task<Dialogs.ContentDialog> ShowContentDialog(System.Windows.Controls.Grid grid = null)
        {
            Dialogs.ContentDialog contentDialog = new Dialogs.ContentDialog();
            await contentDialog.ShowAsync(ContentDialogPlacement.Popup);
            return contentDialog;
        }
    }
}
