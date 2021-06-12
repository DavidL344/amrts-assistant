﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace amrts_map
{
    public class Dialog
    {
        public static OpenFileDialog OpenFile(string title = "Open file", string filter = "All Supported Files|*.amramp;*.x")
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                CheckFileExists = true,
                CheckPathExists = true,
                DereferenceLinks = true
            };

            if (openFile.ShowDialog() == true) return openFile;
            return null;
        }

        public static SaveFileDialog SaveFile(string title = "Save file", string filter = "All Supported Files|*.amramp;*.x")
        {
            SaveFileDialog saveFile = new SaveFileDialog
            {
                Title = title,
                Filter = filter,
                CheckPathExists = true,
                DereferenceLinks = true
            };
            if (saveFile.ShowDialog() == true) return saveFile;
            return null;
        }

        public static FolderBrowserDialog BrowseFolder(string title = "Browse folder", bool newFolderButton = true)
        {
            // A possible alternative (2 MS Office References): https://stackoverflow.com/a/28449277
            // - Microsoft.Office.Core (Microsoft Office 14.0 Object Library)
            // - Microsoft.Office.Interop.Excel (Microsoft Excel 14.0 Object Library)

            // A possible alternative (1 NuGet Package): https://stackoverflow.com/a/49087741
            // - <PackageReference Include="Microsoft-WindowsAPICodePack-Shell" Version="1.1.4" />
            FolderBrowserDialog browseFolder = new FolderBrowserDialog
            {
                Description = title,
                ShowNewFolderButton = newFolderButton
            };

            if (browseFolder.ShowDialog() == DialogResult.OK) return browseFolder;
            return null;
        }

        public static void About(Window owner)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Owner = owner;
            aboutBox.ShowDialog();
        }
    }
}