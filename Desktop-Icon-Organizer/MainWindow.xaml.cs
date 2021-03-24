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
        public MainWindow()
        {
            InitializeComponent();

            List<FileType> fileTypes = new List<FileType>();
            fileTypes.Add(new FileType("Text", ".txt.doc.docx.rtf.wpd"));
            fileTypes.Add(new FileType("Image", ".jpg.jpeg.png.gif.tiff.eps.raw"));
            fileTypes.Add(new FileType("Video", ".mp4.mov.wmv.avi.flv.f4v.webm"));
            fileTypes.Add(new FileType("Compressed", ".rar.zip.7z.rpm.z"));
            fileTypes.Add(new FileType("Steam Game", ".url.gam.sav"));
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

            itmctrlExtensions.ItemsSource = selectedFileType.Extensions;
        }

        private void AllExtensions_Checked(object sender, RoutedEventArgs e)
        {
            int i;
            for (i = 0; i < itmctrlExtensions.Items.Count; i++)
            {
                UIElement uiElement = (UIElement)itmctrlExtensions.ItemContainerGenerator.ContainerFromIndex(i);
                CheckBox checkBox = uiElement as CheckBox;
                checkBox.IsChecked = true;
            }
           

        }

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
                // Do something with selected folder string
            }
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
