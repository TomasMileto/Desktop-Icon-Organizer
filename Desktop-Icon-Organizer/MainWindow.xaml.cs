using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop_Icon_Organizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileType selectedFileType;
        string folderPath;

        public MainWindow()
        {
            InitializeComponent();

            List<FileType> fileTypes = new List<FileType>();
            fileTypes.Add(new FileType("Text", ".txt.doc.docx.rtf.wpd"));
            fileTypes.Add(new FileType("Image", ".jpg.jpeg.png.gif.tiff.eps.raw"));
            fileTypes.Add(new FileType("Video", ".mp4.mov.wmv.avi.flv.f4v.webm"));
            fileTypes.Add(new FileType("Compressed", ".rar.zip.7z.rpm.z"));
            fileTypes.Add(new FileType("Steam Game", ".url.gam.sav"));
            fileTypes.Add(new FileType("Photoshop", ".psd.psb"));
            fileTypes.Add(new FileType("Shortcut", ".link"));
            cmbbox_FileTypes.ItemsSource = fileTypes;
            //cmbbox_FileTypes.SelectedIndex = 0;

            //selectedFileType = fileTypes[0];
            //if(selectedFileType != null)
            //    itemsExtensions.ItemsSource = selectedFileType.Extensions;

        }

        private void cmboxFileTypes_SlctChanged(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            this.selectedFileType = comboBox.SelectedItem as FileType;

            AllExtensions.IsChecked = false;

            itmctrlExtensions.ItemsSource = selectedFileType.Extensions;

        }

        #region CheckBoxes
        private void AllExtensions_Checked(object sender, RoutedEventArgs e)
        {
            int i;
            
            for (i = 0; i < itmctrlExtensions.Items.Count; i++)
            {
                DependencyObject dObject = findElementInItemsControlItemAtIndex(itmctrlExtensions, i, "extensionCheckbox");
                CheckBox checkBox = dObject as CheckBox;
                checkBox.IsChecked = true;
            }
        }
        private void AllExtensions_Unchecked(object sender, RoutedEventArgs e)
        {
            int i;

            for (i = 0; i < itmctrlExtensions.Items.Count; i++)
            {
                DependencyObject dObject = findElementInItemsControlItemAtIndex(itmctrlExtensions, i, "extensionCheckbox");
                CheckBox checkBox = dObject as CheckBox;
                checkBox.IsChecked = false;
            }
        }

        private void Extension_Checked(object sender, RoutedEventArgs e)
        {
            int i;
            bool allChecked = true;

            for (i = 0; i < itmctrlExtensions.Items.Count; i++)
            {
                DependencyObject dObject = findElementInItemsControlItemAtIndex(itmctrlExtensions, i, "extensionCheckbox");
                CheckBox checkBox = dObject as CheckBox;

                if (checkBox.IsChecked == false) allChecked = false;
            }

            if (allChecked)
                AllExtensions.IsChecked = true;
            else
                AllExtensions.IsChecked = null;
        }

        private void Extension_Unchecked(object sender, RoutedEventArgs e)
        {
            int i;
            bool allChecked = false;

            for (i = 0; i < itmctrlExtensions.Items.Count; i++)
            {
                DependencyObject dObject = findElementInItemsControlItemAtIndex(itmctrlExtensions, i, "extensionCheckbox");
                CheckBox checkBox = dObject as CheckBox;

                if (checkBox.IsChecked == true) allChecked = true;
            }

            if (!allChecked)
                AllExtensions.IsChecked = false;
            else
                AllExtensions.IsChecked = null;
        }
        #endregion

        private void btnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Destination Folder Dialog";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = dlg.FileName;
                txtSelectedFolder.Text = folder;
                folderPath = folder;
            }
        }

        private void RUN_Click(object sender, RoutedEventArgs e)
        {
            string output = "";
            bool errorFlag = false;

            if(selectedFileType == null){
                output += "No file type was selected.\n";
                errorFlag = true;
            }else 
                if(AllExtensions.IsChecked == false){
                    output += "No file extension was checked.\n";
                    errorFlag = true;
                 }
            if (folderPath == null){
                output += "No destination folder was selected.\n";
                errorFlag = true;
            }

            if (errorFlag){
                txtOutput.Foreground = Brushes.Red; return;
            }
            //If no error was raised
            List<string> selectedExtensions = new List<string>();

            for (int i = 0; i < itmctrlExtensions.Items.Count; i++)
            {
                DependencyObject dObject = findElementInItemsControlItemAtIndex(itmctrlExtensions, i, "extensionCheckbox");
                CheckBox checkBox = dObject as CheckBox;

                if (checkBox.IsChecked == true) 
                    selectedExtensions.Add(selectedFileType.Extensions[i]);
            }

            txtOutput.Foreground = Brushes.Black;
            txtOutput.Text = Run(folderPath, selectedFileType, selectedExtensions);

        }
    }

    public class FileType
    {
        public string FileName { get; set; }
        public List<string> Extensions { get; set; }

        public FileType(string _name)
        {
            this.FileName = _name;
        }
        public FileType(string _name, string extList)
        {
            this.FileName = _name;

            List<string> _extensions = new List<string>();
            int i=0, j;
            for(j=i+1; j<extList.Length; j++)
            {
                if(extList[j] == '.' )      //if j is pointing at a '.' 
                {
                    _extensions.Add(extList.Substring(i, j - i));
                    i = j;
                }
                if(j == extList.Length - 1)                         //or at the last char of the string
                {
                    _extensions.Add(extList.Substring(i, j + 1 - i));
                }
            }

            this.Extensions = new List<string>();
            this.Extensions = _extensions;
        }

    }
        

}
