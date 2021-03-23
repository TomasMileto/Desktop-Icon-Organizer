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
        public MainWindow()
        {
            InitializeComponent();

            List<FileType> fileTypes = new List<FileType>();
            fileTypes.Add(new FileType("Text", ".txt.doc.docx.rtf.wpd"));
            fileTypes.Add(new FileType("Image"));
            fileTypes.Add(new FileType("Video"));
            fileTypes.Add(new FileType("Compressed"));
            fileTypes.Add(new FileType("Steam Game"));
            fileTypes.Add(new FileType("Shortcut"));
            cmbbox_FileTypes.ItemsSource = fileTypes;
            //cmbbox_FileTypes.SelectedIndex = 0;

            foreach(string ext in fileTypes[0].Extensions)
            {
                Console.WriteLine(ext);
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
