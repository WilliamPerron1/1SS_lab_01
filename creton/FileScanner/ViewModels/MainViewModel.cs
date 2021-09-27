using FileScanner.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Orientation = System.Windows.Controls.Orientation;

namespace FileScanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string selectedFolder;
        private ObservableCollection<StackPanel> folderItems = new ObservableCollection<StackPanel>();

        public DelegateCommand<string> OpenFolderCommand { get; private set; }
        public DelegateCommand<string> ScanFolderCommand { get; private set; }

        public ObservableCollection<StackPanel> FolderItems
        {
            get => folderItems;
            set
            {
                folderItems = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFolder
        {
            get => selectedFolder;
            set
            {
                selectedFolder = value;
                OnPropertyChanged();
                ScanFolderCommand.RaiseCanExecuteChanged();
            }
        }

        public MainViewModel()
        {
            OpenFolderCommand = new DelegateCommand<string>(OpenFolder);
            ScanFolderCommand = new DelegateCommand<string>(ScanFolder, CanExecuteScanFolder);
        }

        private bool CanExecuteScanFolder(string obj)
        {
            return !string.IsNullOrEmpty(SelectedFolder);
        }

        private void OpenFolder(string obj)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    SelectedFolder = fbd.SelectedPath;
                }
            }
        }

        private async void ScanFolder(string dir)
        {
            //await Task.Run(() =>
            //{
            await GetDirs(dir);
            //});

        }

        private async Task GetDirs(string dir)
        {
            await add_dir(dir);
            try
            {
                foreach (var item in Directory.EnumerateFiles(dir, "*"))
                {
                    await add_file(item);
                }
                foreach (var d in Directory.EnumerateDirectories(dir, "*"))
                {
                    await GetDirs(d);
                }
            }
            catch (Exception e) { }
        }

        private async Task add_dir(string dir)
        {
            Image dir_icon = new Image();
            dir_icon.Width = 20;
            dir_icon.Source = new BitmapImage(new Uri("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTPEoa2UaA4ViZ0aUMY_fn-8nJ7gR2awGBWFA&usqp=CAU"));

            System.Windows.Controls.Label u = new System.Windows.Controls.Label();
            u.Content = dir;

            StackPanel sp = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            sp.Children.Add(dir_icon);
            sp.Children.Add(u);

            FolderItems.Add(sp);
        }
        private async Task add_file(string dir)
        {
            Image file_icon = new Image();
            file_icon.Width = 20;
            file_icon.Source = new BitmapImage(new Uri("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTLmCtCpZT2olbcZ3Q0hGUA0YpMIbxeFx3QfQ&usqp=CAU"));

            System.Windows.Controls.Label Url = new System.Windows.Controls.Label();
            Url.Content = dir;

            StackPanel sp = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            sp.Children.Add(file_icon);
            sp.Children.Add(Url);

            FolderItems.Add(sp);

        }

        ///TODO : Tester avec un dossier avec beaucoup de fichier
        ///TODO : Rendre l'application asynchrone
        ///TODO : Ajouter un try/catch pour les dossiers sans permission


    }
}
