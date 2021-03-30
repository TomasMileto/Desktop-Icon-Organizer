using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
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

        string Run(string _destFolder, FileType _fileType, List<string> _selExtns)
        {
            string pathTop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string ret;

            //Get All files matching any element of the list of selected extensions
            List<string> selectedFiles = GetAllFiles(pathTop, _selExtns);


            //Move the files which names are in selectedFiles to the folder Steam Games
            string source = null;
            string dest = null;
            int movedCount = 0;
            foreach (string file in selectedFiles)
            {
                source = System.IO.Path.Combine(pathTop, file);
                dest = System.IO.Path.Combine(_destFolder, file);

                if (!System.IO.File.Exists(dest)){
                    System.IO.File.Move(source, dest);
                    movedCount++;
                }
            }

            if (movedCount == 0)
                ret = $"No files of the type \"{_fileType.FileName}\" ({List2String(_selExtns)}) were moved (due to the lack of these or the presence of duplicate files on {System.IO.Path.GetFileName(_destFolder)}).";
            else
                ret = $"Successfully moved {movedCount} {_fileType.FileName} file(s) ({List2String(_selExtns)}) to the folder: {System.IO.Path.GetFileName(_destFolder)}";

            return ret;
        }
        List<string> GetAllFiles(string _directory, List<string> ext_list )
        {
            List<string> files = new List<string>();

            string[] fileEntries = Directory.GetFiles(_directory);

            foreach (string filePath in fileEntries)
                if (ext_list.Contains(System.IO.Path.GetExtension(filePath)))
                    files.Add(System.IO.Path.GetFileName(filePath));

            return files;
        }

        void Output (string s)
        {
            if (txtOutput.Text == "The outcome will be displayed here!")
                txtOutput.Text = s;
            else
                txtOutput.Text += s;
        }
        #region Output FUNC Overloads
        void Output (int i)
        {
            string s = i.ToString();
            if (txtOutput.Text == "The outcome will be displayed here!")
                txtOutput.Text = s;
            else
                txtOutput.Text += s;
        }
        void Output (float f)
        {
            string s = f.ToString();
            if (txtOutput.Text == "The outcome will be displayed here!")
                txtOutput.Text = s;
            else
                txtOutput.Text += s;
        }
        #endregion

        string List2String(List<string> _list)
        {
            string ret = "";
            for (int i = 0; i < _list.Count; i++){
                if( i == _list.Count -1)
                    ret += $"{_list[i]}.";
                else
                    ret += $"{_list[i]}, ";
            }
           
            return ret;
        }


        DependencyObject findElementInItemsControlItemAtIndex(ItemsControl itemsControl, int itemOfIndexToFind, string nameOfControlToFind)
        {
            if (itemOfIndexToFind >= itemsControl.Items.Count) return null;

            DependencyObject depObj = null;
            object o = itemsControl.Items[itemOfIndexToFind];
            if (o != null)
            {
                var item = itemsControl.ItemContainerGenerator.ContainerFromItem(o);
                if (item != null)
                {
                    depObj = getVisualTreeChild(item, nameOfControlToFind);
                    return depObj;
                }
            }
            return null;
        }

        DependencyObject getVisualTreeChild(DependencyObject obj, String name)
        {
            DependencyObject dependencyObject = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < childrenCount; i++)
            {
                var oChild = VisualTreeHelper.GetChild(obj, i);
                var childElement = oChild as FrameworkElement;
                if (childElement != null)
                {
                    if ( childElement is TextBlock)
                    {
                        dependencyObject = childElement.FindName(name) as DependencyObject;
                        if (dependencyObject != null)
                            return dependencyObject;
                    }

                    if (childElement.Name == name)
                    {
                        return childElement;
                    }
                }
                dependencyObject = getVisualTreeChild(oChild, name);
                if (dependencyObject != null)
                    return dependencyObject;
            }
            return dependencyObject;
        }

    }

  

}
