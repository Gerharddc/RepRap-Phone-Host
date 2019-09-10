using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using System.IO;
using RepRap_Phone_Host.ListItems;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host.FileSystem
{
    /// <summary>
    /// This class contains the function needed to find files with
    /// a given extension and add them to a ListPicker
    /// </summary>
    static class FileFinder
    {
        /// <summary>
        /// Find files with specified parameters and add them to a ListPicker
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="folderPath"></param>
        /// <param name="extension"></param>
        static public void findFilesAndAddToListpicker(ListPicker fileList, string folderPath, string extension)
        {
            if (fileList == null)
                return;//Ignore wrongful listpickers

            var isf = IsolatedStorageFile.GetUserStoreForApplication();

            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            List<FileItems> fileItemsList = new List<FileItems>();

            /*foreach (FileInfo fileInfo in directoryInfo.GetFiles("*" + extension))
            {
                fileItemsList.Add(new FileItems() { Name = fileInfo.Name.Split('.')[0], FilePath = fileInfo.FullName });
            }*/
            foreach (string fileName in isf.GetFileNames("*" + extension))
            {
                fileItemsList.Add(new FileItems() { Name = fileName.Split('.')[0], FilePath = fileName });
            }

            fileList.ItemsSource = fileItemsList;
        }

        static public List<FileItems> findFilesAndCreateList(string folderPath, string extension)
        {
            var isf = IsolatedStorageFile.GetUserStoreForApplication();

            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            List<FileItems> fileItemsList = new List<FileItems>();

            /*foreach (FileInfo fileInfo in directoryInfo.GetFiles("*" + extension))
            {
                fileItemsList.Add(new FileItems() { Name = fileInfo.Name.Split('.')[0], FilePath = fileInfo.FullName });
            }*/
            foreach (string fileName in isf.GetFileNames("*" + extension))
            {
                fileItemsList.Add(new FileItems() { Name = fileName.Split('.')[0], FilePath = fileName });
            }

            return fileItemsList;
        }
    }
}
