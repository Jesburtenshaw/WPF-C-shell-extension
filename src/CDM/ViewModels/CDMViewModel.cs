using CDM.Common;
using CDM.Helper;
using CDM.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CDM.ViewModels
{
    internal class CDMViewModel : ViewModelBase
    {
        #region :: Constructor ::
        public CDMViewModel()
        {

            DoubleClickCommand = new RelayCommand(DoubleDriveClick);
            BackWindowCommand = new RelayCommand(BackNavigationClick);
            SearchBoxTextChangedCommand = new RelayCommand(searchBoxTextChanged);
            FolderItemDoubleClickCommand = new RelayCommand(FolderItemDoubleClick);
            RecentItemDoubleClickCommand = new RelayCommand(RecentItemDoubleClick);
            PinnedItemDoubleClickCommand = new RelayCommand(PinnedItemDoubleClick);

            //RecentItemSortCommand = new RelayCommand(RecentItemSort);

            RecentItemList = new ObservableCollection<FileFolderModel>();
            PinnedItemList = new ObservableCollection<FileFolderModel>();
            FoldersItemList = new ObservableCollection<FileFolderModel>();

            GetDrivesItem();
            GetRecentItems();
            GetPinnedItems();
            //SearchBox();

            TxtSearchGotFocusCommand = new RelayCommand(searchBoxGotFocus);
            TxtSearchLostFocusCommand = new RelayCommand(searchBoxLostFocus);
            //SearchItemCommand = new RelayCommand(searchItem);

            //DriveCommand = new RelayCommand(driveCommand);
            IsSearchBoxPlaceholderVisible = Visibility.Visible;
            IsDriveWindowVisible = Visibility.Visible;
            IsDriveFoldersVisible = Visibility.Collapsed;
        }
        #endregion

        //private void RecentItemSort(object obj)
        //{
        //    IsAscending = !IsAscending;
        //}


        //private bool _isAscending = true;
        //public bool IsAscending
        //{
        //    get => _isAscending;
        //    set
        //    {
        //        _isAscending = value;
        //        OnPropertyChanged(nameof(IsAscending));
        //    }
        //}



        #region :: Properties ::
        public string TempFolderFileSource { get; set; }
        public string CurrentDrivePath { get; set; }

        static Stack<string> directoryHistory = new Stack<string>();

        private DriveModel _driveSelectedItem;
        public DriveModel DriveSelectedItem
        {
            get
            {
                return _driveSelectedItem;
            }
            set
            {
                _driveSelectedItem = value;
                OnPropertyChanged(nameof(DriveSelectedItem));
            }
        }

        private string _txtSearchBoxItem;
        public string TxtSearchBoxItem
        {
            get
            {
                return _txtSearchBoxItem;
            }
            set
            {
                _txtSearchBoxItem = value;
                OnPropertyChanged(nameof(TxtSearchBoxItem));
            }
        }

        private ObservableCollection<DriveModel> _driveList;
        public ObservableCollection<DriveModel> DriveList
        {
            get { return _driveList; }
            set
            {
                _driveList = value;
                OnPropertyChanged(nameof(DriveList));
            }
        }

        private ObservableCollection<FileFolderModel> _recentItemList;
        public ObservableCollection<FileFolderModel> RecentItemList
        {
            get { return _recentItemList; }
            set
            {
                _recentItemList = value;
                OnPropertyChanged(nameof(RecentItemList));
            }
        }

        private FileFolderModel _selectedRecentItem;
        public FileFolderModel SelectedRecentItem
        {
            get { return _selectedRecentItem; }
            set
            {
                _selectedRecentItem = value;
                OnPropertyChanged(nameof(SelectedRecentItem));
            }
        }

        private FileFolderModel _selectedPinnedItem;
        public FileFolderModel SelectedPinnedItem
        {
            get { return _selectedPinnedItem; }
            set
            {
                _selectedPinnedItem = value;

                OnPropertyChanged(nameof(SelectedPinnedItem));
            }
        }

        private ObservableCollection<FileFolderModel> _pinnedItemList;
        public ObservableCollection<FileFolderModel> PinnedItemList
        {
            get { return _pinnedItemList; }
            set
            {
                _pinnedItemList = value;
                OnPropertyChanged(nameof(PinnedItemList));
            }
        }

        private ObservableCollection<FileFolderModel> _foldersItemList;
        public ObservableCollection<FileFolderModel> FoldersItemList
        {
            get { return _foldersItemList; }
            set
            {
                _foldersItemList = value;
                OnPropertyChanged(nameof(FoldersItemList));
            }
        }

        private FileFolderModel _selectedFileFolderItem;
        public FileFolderModel SelectedFileFolderItem
        {
            get { return _selectedFileFolderItem; }
            set
            {
                _selectedFileFolderItem = value;
                OnPropertyChanged(nameof(SelectedFileFolderItem));
            }
        }

        private Visibility _isDriveWindowVisible;
        public Visibility IsDriveWindowVisible
        {
            get { return _isDriveWindowVisible; }
            set
            {
                _isDriveWindowVisible = value;
                OnPropertyChanged(nameof(IsDriveWindowVisible));
            }
        }
        private Visibility _isDriveFoldersVisible;


        public Visibility IsDriveFoldersVisible
        {
            get { return _isDriveFoldersVisible; }
            set
            {
                _isDriveFoldersVisible = value;
                OnPropertyChanged(nameof(IsDriveFoldersVisible));
            }
        }

        private Visibility _isSearchBoxPlaceholderVisible;
        public Visibility IsSearchBoxPlaceholderVisible
        {
            get { return _isSearchBoxPlaceholderVisible; }
            set
            {
                _isSearchBoxPlaceholderVisible = value;
                OnPropertyChanged(nameof(IsSearchBoxPlaceholderVisible));
            }
        }
        #endregion

        #region :: Commands ::
        public RelayCommand BackWindowCommand { get; set; }
        public RelayCommand DoubleClickCommand { get; set; }
        public RelayCommand SearchBoxTextChangedCommand { get; set; }
        public RelayCommand TxtSearchGotFocusCommand { get; set; }
        public RelayCommand TxtSearchLostFocusCommand { get; set; }
        public RelayCommand FolderItemDoubleClickCommand { get; set; }
        public RelayCommand RecentItemDoubleClickCommand { get; set; }
        public RelayCommand PinnedItemDoubleClickCommand { get; set; }
        public RelayCommand SearchItemCommand { get; set; }
        //public RelayCommand RecentItemSortCommand { get; set; }

        #endregion

        #region :: Methods ::
        public async void GetDrivesItem()
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                DriveList = new ObservableCollection<DriveModel>();
                DriveSelectedItem = new DriveModel();
                foreach (DriveInfo drive in drives)
                {
                    DriveList.Add(new DriveModel()
                    {
                        DriveName = drive.Name,
                        DriveDescription = drive.DriveFormat,
                    });
                }
                //DriveList.Add(new DriveModel()
                //{
                //    DriveName = "Test1",
                //    DriveDescription = "Tetsing",
                //});
                //DriveList.Add(new DriveModel()
                //{
                //    DriveName = "Test2",
                //    DriveDescription = "Tetsing2",
                //});
                //DriveList.Add(new DriveModel()
                //{
                //    DriveName = "Test3",
                //    DriveDescription = "Tetsing2",
                //});
                //DriveList.Add(new DriveModel()
                //{
                //    DriveName = "Test4",
                //    DriveDescription = "Tetsing2",
                //});
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        public async void GetRecentItems()
        {
            try
            {
                string recentFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
                // Get all files in the Recent folder
                string[] recentFiles = Directory.GetFiles(recentFolderPath);
                foreach (string recentFile in recentFiles)
                {
                    var file = GetLnkTarget(recentFile);
                    if (!String.IsNullOrEmpty(file) && File.Exists(file))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        FileAttributes f = File.GetAttributes(file);
                        RecentItemList.Add(new FileFolderModel()
                        {

                            Name = fileInfo.Name,
                            LastModifiedDateTime = fileInfo.LastWriteTime,
                            Path = fileInfo.FullName,
                            IconSource = IconHelper.GetIcon(fileInfo.FullName)
                        });
                    }
                }
                // Get all folders in the Recent folder
                // string[] recentFolders = Directory.GetDirectories(recentFolderPath);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        public async void GetPinnedItems()
        {
            try
            {

                // Path to the Taskbar pinned items folder
                string pinnedItemsFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar");

                // Get all files in the pinned items folder
                string[] pinnedFiles = Directory.GetFiles(pinnedItemsFolderPath);

                //// Get all folders in the pinned items folder
                //string[] pinnedFolders = Directory.GetDirectories(pinnedItemsFolderPath);

                foreach (string pinnedFile in pinnedFiles)
                {
                    var file = GetLnkTarget(pinnedFile);
                    if (!String.IsNullOrEmpty(file) != null && File.Exists(file))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        FileAttributes f = File.GetAttributes(file);
                        PinnedItemList.Add(new FileFolderModel()
                        {
                            Name = fileInfo.Name,
                            LastModifiedDateTime = fileInfo.LastWriteTime,
                            Path = fileInfo.FullName,
                            IconSource = IconHelper.GetIcon(fileInfo.FullName)
                        });

                    }

                }
                // Get all folders in the Recent folder

                //string[] recentFolders = Directory.GetDirectories(pinnedItemsFolderPath);

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        string GetLnkTarget(string lnkPath)
        {
            try
            {
                // Create a Shell object
                dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));

                // Get the folder containing the .lnk file
                var folder = shell.NameSpace(System.IO.Path.GetDirectoryName(lnkPath));

                // Get the .lnk file
                var folderItem = folder.ParseName(System.IO.Path.GetFileName(lnkPath));

                // Get the target of the .lnk file
                dynamic link = folderItem.GetLink;

                return link.Path;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        private void BackNavigationClick(object obj)
        {

            if (directoryHistory.Count == 0)
            {
                //cannot go back
                return;
            }

            //Check if current is root directory or drive
            if (directoryHistory.Count == 1 || IsRootFolder(directoryHistory.Peek()))
            {
                IsDriveWindowVisible = Visibility.Visible;
                IsDriveFoldersVisible = Visibility.Collapsed;
                return;
            }

            //Navigate to previous directory
            directoryHistory.Pop();
            NavigateToFolder(directoryHistory.Peek());

            TxtSearchBoxItem = string.Empty;
            IsSearchBoxPlaceholderVisible = Visibility.Visible;
            CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
        }

        private void DoubleDriveClick(object sender)
        {
            try
            {
                FoldersItemList = new ObservableCollection<FileFolderModel>();

                DriveModel item = sender as DriveModel;

                if (item?.DriveName != null && item.DriveName != string.Empty)
                {
                    NavigateToFolder(item.DriveName);

                    directoryHistory.Push(item.DriveName);
                    CurrentDrivePath = item.DriveName;

                    IsDriveFoldersVisible = Visibility.Visible;
                    IsDriveWindowVisible = Visibility.Collapsed;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Occured! {Environment.NewLine}{Environment.NewLine} {ex.Message}");
                //throw ex;
            }

            //TxtSearchBoxItem = string.Empty;
            //IsSearchBoxPlaceholderVisible = Visibility.Visible;
            //CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
        }
        private void searchBoxTextChanged(object sender)
        {
            if (!string.IsNullOrEmpty(TxtSearchBoxItem))
            {
                IsSearchBoxPlaceholderVisible = Visibility.Collapsed;
                CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FoldersItemList);
                view.Filter = searchItemFilter;
                return;
            }
            else
            {
                IsSearchBoxPlaceholderVisible = Visibility.Visible;
                CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
            }

        }

        public void FolderItemDoubleClick(object sender)
        {

            if (SelectedFileFolderItem != null && !string.IsNullOrEmpty(SelectedFileFolderItem.Path))
            {
                string path = SelectedFileFolderItem.Path;

                // Check if the path exist and path is folder
                if (Directory.Exists(path))
                {
                    directoryHistory.Push(path);
                    NavigateToFolder(path);

                    //TxtSearchBoxItem = string.Empty;
                    //IsSearchBoxPlaceholderVisible = Visibility.Visible;
                    //CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
                }

                // Check if the path exist and path is file
                else if (File.Exists(path))
                {
                    try
                    {
                        Process.Start(path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Could not open file {SelectedFileFolderItem.Name} {Environment.NewLine} {Environment.NewLine} {ex.Message}");
                        //throw ex;
                    }
                }
            }
        }

        public void RecentItemDoubleClick(object sender)
        {
            if (SelectedRecentItem != null && !string.IsNullOrEmpty(SelectedRecentItem.Path))
            {
                try
                {
                    Process.Start(SelectedRecentItem.Path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open file {SelectedRecentItem.Name} {Environment.NewLine} {Environment.NewLine} {ex.Message}");
                    //throw ex;
                }
            }
        }
        public void PinnedItemDoubleClick(object sender)
        {
            if (SelectedPinnedItem != null && !string.IsNullOrEmpty(SelectedPinnedItem.Path))
            {
                try
                {
                    Process.Start(SelectedPinnedItem.Path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open file {SelectedPinnedItem.Name} {Environment.NewLine} {Environment.NewLine} {ex.Message}");
                    //throw ex;
                }
            }
        }

        public void NavigateToFolder(string folderPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(folderPath))
                {
                    List<FileFolderModel> subFolderFiles = new List<FileFolderModel>();

                    // Check if the folder exists
                    if (Directory.Exists(folderPath))
                    {
                        //Get subfolders
                        string[] subDirectories = Directory.GetDirectories(folderPath);
                        foreach (string subFolder in subDirectories)
                        {
                            DirectoryInfo subFolderInfo = new DirectoryInfo(subFolder);
                            if (!subFolderInfo.Attributes.ToString().Contains(FileAttributes.Hidden.ToString()) &&
                       !subFolderInfo.Attributes.ToString().Contains(FileAttributes.NotContentIndexed.ToString()) &&
                       !subFolderInfo.Attributes.ToString().Contains(FileAttributes.ReparsePoint.ToString()))
                            {

                                subFolderFiles.Add(
                               new FileFolderModel()
                               {
                                   Path = subFolderInfo.FullName,
                                   Name = subFolderInfo.Name,
                                   LastModifiedDateTime = subFolderInfo.LastWriteTime,
                                   IconSource = IconHelper.GetIcon(subFolderInfo.FullName)
                               }
                                   );
                            }
                        }

                        //Get files
                        string[] files = Directory.GetFiles(folderPath);
                        foreach (string file in files)
                        {
                            FileInfo fileInfo = new FileInfo(file);

                            if (!fileInfo.Attributes.ToString().Contains(FileAttributes.Hidden.ToString()) &&
                        !fileInfo.Attributes.ToString().Contains(FileAttributes.NotContentIndexed.ToString()) &&
                        !fileInfo.Attributes.ToString().Contains(FileAttributes.ReparsePoint.ToString()))
                            {
                                subFolderFiles.Add(
                                new FileFolderModel()
                                {
                                    Path = fileInfo.FullName,
                                    Name = fileInfo.Name,
                                    LastModifiedDateTime = fileInfo.LastWriteTime,
                                    IconSource = IconHelper.GetIcon(fileInfo.FullName)
                                }
                                );
                            }
                        }
                    }
                    else
                    {
                        // Console.WriteLine("The folder does not exist.");
                    }
                    FoldersItemList = new ObservableCollection<FileFolderModel>(subFolderFiles);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            TxtSearchBoxItem = string.Empty;
            IsSearchBoxPlaceholderVisible = Visibility.Visible;
            CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
        }

        private bool IsRootFolder(string path)
        {
            if (path == CurrentDrivePath)
            {
                return true;
            }
            return false;
        }

        // File icon set

        //private string SetIcon(string extension)
        //{
        //    string folderFileSource = ""; // Initialize the variable to hold the icon path

        //    switch (extension.ToLower())
        //    {
        //        case ".pdf":
        //            folderFileSource = "Resources/icnpdf.png";
        //            break;
        //        case ".jpg":
        //        case ".jpeg":
        //            folderFileSource = "Resources/icnjpg.png";
        //            break;
        //        case ".png":
        //            folderFileSource = "Resources/icnpng.png";
        //            break;
        //        case ".mkv":
        //            folderFileSource = "Resources/icnmkv.png";
        //            break;
        //        case ".xaml":
        //            folderFileSource = "Resources/icnxml.png";
        //            break;
        //        case ".svg":
        //            folderFileSource = "Resources/icnsvg.png";
        //            break;
        //        case ".txt":
        //            folderFileSource = "Resources/icntxt.png";
        //            break;
        //        case ".sql":
        //            folderFileSource = "Resources/icnsql.png";
        //            break;
        //        case ".zip":
        //            folderFileSource = "Resources/icnzip.png";
        //            break;
        //        case ".sln":
        //            folderFileSource = "Resources/icnsln.png";
        //            break;
        //        case ".exe":
        //            folderFileSource = "Resources/icnexe.png";
        //            break;
        //        default:
        //            folderFileSource = "Resources/icnfolder.png";
        //            break;
        //    }
        //    return folderFileSource;
        //}

        // search list filter

        //private bool folderListFilter(object item)
        //{
        //    if (String.IsNullOrEmpty(TxtSearchBoxItem))
        //        return true;
        //    else
        //        return (item as FolderModel).FolderName.StartsWith(TxtSearchBoxItem, StringComparison.OrdinalIgnoreCase);
        //}

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show($"Error Occured! {Environment.NewLine}{Environment.NewLine} {message}");
        }

        //search functionlaity
        private void searchBoxLostFocus(object obj)
        {
            if (!string.IsNullOrEmpty(TxtSearchBoxItem))
            {
                return;
            }
            else
            {
                IsSearchBoxPlaceholderVisible = Visibility.Visible;

                CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
            }
        }

        private void searchBoxGotFocus(object obj)
        {
            IsSearchBoxPlaceholderVisible = Visibility.Collapsed;
        }

        private bool searchItemFilter(object item)
        {
            if (string.IsNullOrEmpty(TxtSearchBoxItem) || TxtSearchBoxItem == "Search")
                return true;
            else
                return (item as FileFolderModel).Name.StartsWith(TxtSearchBoxItem, StringComparison.OrdinalIgnoreCase);
        }
        #endregion
    }
}
