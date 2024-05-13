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
using System.Reflection;
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

            PinnedItemList = PinManager.GetPinnedItems();
            RecentItemList = RecentManager.GetRecentItems();
            DriveList = DriveManager.GetDrivesItem();
            DriveSelectedItem = new DriveModel();
            FoldersItemList = new ObservableCollection<FileFolderModel>();
            CurFolder = nullFolder;

            //SearchBox();

            TxtSearchGotFocusCommand = new RelayCommand(searchBoxGotFocus);
            TxtSearchLostFocusCommand = new RelayCommand(searchBoxLostFocus);
            //SearchItemCommand = new RelayCommand(searchItem);
            PinCommand = new RelayCommand(Pin);
            UnpinCommand = new RelayCommand(Unpin);
            StarCommand = new RelayCommand(Star);
            UnstarCommand = new RelayCommand(Unstar);
            OpenItemCommand = new RelayCommand(OpenItem);
            CopyItemPathCommand = new RelayCommand(CopyItemPath);
            RenameItemCommand = new RelayCommand(RenameItem);
            DeleteItemCommand = new RelayCommand(DeleteItem);
            DoRenameCommand = new RelayCommand(DoRename);
            CancelRenameCommand = new RelayCommand(CancelRename);
            RenameTextChangedCommand = new RelayCommand(RenameTextChanged);

            //DriveCommand = new RelayCommand(driveCommand);
            IsSearchBoxPlaceholderVisible = Visibility.Visible;
            IsDriveWindowVisible = Visibility.Visible;
            IsDriveFoldersVisible = Visibility.Collapsed;
        }

        #endregion

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

        private FileFolderModel curRenameItem;
        public FileFolderModel CurRenameItem
        {
            get
            {
                return curRenameItem;
            }
            set
            {
                curRenameItem = value;
                OnPropertyChanged(nameof(CurRenameItem));
            }
        }

        private FileFolderModel nullFolder = new FileFolderModel();
        private FileFolderModel curFolder;
        public FileFolderModel CurFolder
        {
            get
            {
                return curFolder;
            }
            set
            {
                curFolder = value;
                OnPropertyChanged(nameof(CurFolder));
            }
        }

        private SearchStatusModel curSearchStatus = new SearchStatusModel();
        public SearchStatusModel CurSearchStatus
        {
            get
            {
                return curSearchStatus;
            }
            set
            {
                curSearchStatus = value;
                OnPropertyChanged(nameof(CurSearchStatus));
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

        private RenameStatusModel curRenameStatus = new RenameStatusModel();
        public RenameStatusModel CurRenameStatus
        {
            get
            {
                return curRenameStatus;
            }
            set
            {
                curRenameStatus = value;
                OnPropertyChanged(nameof(CurRenameStatus));
            }
        }

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
        public RelayCommand PinCommand { get; set; }
        public RelayCommand UnpinCommand { get; set; }
        public RelayCommand StarCommand { get; set; }
        public RelayCommand UnstarCommand { get; set; }
        public RelayCommand OpenItemCommand { get; set; }
        public RelayCommand CopyItemPathCommand { get; set; }
        public RelayCommand RenameItemCommand { get; set; }
        public RelayCommand DeleteItemCommand { get; set; }
        public RelayCommand DoRenameCommand { get; set; }
        public RelayCommand CancelRenameCommand { get; set; }
        public RelayCommand RenameTextChangedCommand {  get; set; }

        #endregion

        #region :: Methods ::

        private void Pin(object obj)
        {
            FileFolderModel curItem = null;
            FileFolderModel unpinedItem = null;
            var driveItem = obj as DriveModel;
            if (null != driveItem)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(driveItem.DriveName);
                unpinedItem = new FileFolderModel
                {
                    Name = dirInfo.Name,
                    LastModifiedDateTime = dirInfo.LastWriteTime,
                    Path = dirInfo.FullName,
                    IconSource = IconHelper.GetIcon(dirInfo.FullName),
                    Type = "Dir",
                    IsPined = true
                };
            }
            else
            {
                curItem = obj as FileFolderModel;
                unpinedItem = new FileFolderModel
                {
                    Name = curItem.Name,
                    LastModifiedDateTime = curItem.LastModifiedDateTime,
                    Path = curItem.Path,
                    IconSource = curItem.IconSource,
                    Type = curItem.Type,
                    IsPined = true,
                    OriginalPath = curItem.OriginalPath
                };
            }
            try
            {
                PinManager.Pin(unpinedItem);

                FileFolderModel t = null;
                if (null == curItem || string.IsNullOrEmpty(curItem.OriginalPath))
                {
                    t = RecentItemList.FirstOrDefault(e => e.Path.Equals(unpinedItem.Path));
                    if (null != t)
                    {
                        t.IsPined = !t.IsPined;
                    }
                }
                else if (null == curItem || !string.IsNullOrEmpty(curItem.OriginalPath))
                {
                    t = FoldersItemList.FirstOrDefault(e => e.Path.Equals(unpinedItem.Path));
                    if (null != t)
                    {
                        t.IsPined = !t.IsPined;
                    }
                }
                if (null != driveItem)
                {
                    driveItem.IsPined = !driveItem.IsPined;
                }
                else
                {
                    curItem.IsPined = !curItem.IsPined;
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, $"Could not pin {unpinedItem.Name}");
            }
        }

        private void Unpin(object obj)
        {
            FileFolderModel pinedItem = null;
            FileFolderModel curItem = null;
            var driveItem = obj as DriveModel;
            if (null != driveItem)
            {
                pinedItem = PinnedItemList.FirstOrDefault(e => e.Path.Equals(driveItem.DriveName));
            }
            else
            {
                curItem = obj as FileFolderModel;
                pinedItem = PinnedItemList.FirstOrDefault(e => e.Path.Equals(curItem.Path));
            }
            if (null == pinedItem)
            {
                return;
            }
            try
            {
                PinManager.Unpin(pinedItem);

                FileFolderModel t = null;
                if (null == curItem || string.IsNullOrEmpty(curItem.OriginalPath))
                {
                    t = RecentItemList.FirstOrDefault(e => e.Path.Equals(pinedItem.Path));
                    if (null != t)
                    {
                        t.IsPined = !t.IsPined;
                    }
                }
                else if (null == curItem || !string.IsNullOrEmpty(curItem.OriginalPath))
                {
                    t = FoldersItemList.FirstOrDefault(e => e.Path.Equals(pinedItem.Path));
                    if (null != t)
                    {
                        t.IsPined = !t.IsPined;
                    }
                }
                if (null != driveItem)
                {
                    driveItem.IsPined = !driveItem.IsPined;
                }
                else
                {
                    curItem.IsPined = !curItem.IsPined;
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, $"Could not unpin {pinedItem.Name}");
            }
        }

        private void Star(object obj)
        {
            var item = obj as FileFolderModel;
            try
            {
                StarManager.SetDefault(item.Path);

                var t = RecentItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = PinnedItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = FoldersItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }

                t = RecentItemList.FirstOrDefault(e => !e.Equals(item) && e.IsDefault);
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = PinnedItemList.FirstOrDefault(e => !e.Equals(item) && e.IsDefault);
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = FoldersItemList.FirstOrDefault(e => !e.Equals(item) && e.IsDefault);
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }

                item.IsDefault = !item.IsDefault;
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, $"Could not star {item.Name}");                
            }
        }

        private void Unstar(object obj)
        {
            var item = obj as FileFolderModel;
            try
            {
                StarManager.SetDefault(item.Path);

                var t = RecentItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = PinnedItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }
                t = FoldersItemList.FirstOrDefault(e => !e.Equals(item) && e.Path.Equals(item.Path));
                if (null != t)
                {
                    t.IsDefault = !t.IsDefault;
                }

                item.IsDefault = !item.IsDefault;
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, $"Could not unstar {item.Name}");
            }
        }

        private void OpenItem(object obj)
        {
            var item = obj as FileFolderModel;
            if (null == item)
            {
                return;
            }
            try
            {
                Process.Start(item.Path);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex, $"Could not open file or folder {item.Name}");
                //throw ex;
            }
        }

        private void CopyItemPath(object obj)
        {
            var item = obj as FileFolderModel;
            if (null == item)
            {
                return;
            }
            Clipboard.SetText(item.Path);
        }

        private void RenameItem(object obj)
        {
            var item = obj as FileFolderModel;
            if (null == item)
            {
                return;
            }
            CurRenameItem = item;
            CurRenameStatus.IsDoing = true;
            curRenameStatus.Name = item.Name;
        }

        private void DeleteItem(object obj)
        {
            var item = obj as FileFolderModel;
            if (null == item)
            {
                return;
            }
            var mbr = MessageBox.Show($"Are you sure to delete file or folder {item.Name}?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (mbr != MessageBoxResult.Yes)
            {
                return;
            }
            if (File.Exists(item.Path))
            {
                File.Delete(item.Path);
            }
            else
            {
                Directory.Delete(item.Path);
            }

            var t = RecentItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            if (null != t)
            {
                RecentManager.Remove(t);
            }
            t = PinnedItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            if (null != t)
            {
                PinManager.Unpin(t);
            }
            t = FoldersItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            if (null != t)
            {
                FoldersItemList.Remove(item);
                CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
            }
        }

        private void DoRename(object obj)
        {
            if (CurRenameStatus.IsError)
            {
                return;
            }
            if (string.IsNullOrEmpty(CurRenameStatus.Name))
            {
                CancelRename(obj);
                return;
            }
            if (CurRenameStatus.Name.Equals(CurRenameItem.Name))
            {
                CancelRename(obj);
                return;
            }
            var dir = Path.GetDirectoryName(CurRenameItem.Path);
            var newPath = Path.Combine(dir, CurRenameStatus.Name);
            if (CurRenameItem.Type == "Dir")
            {
                Directory.Move(CurRenameItem.Path, newPath);
            }
            else
            {
                File.Move(CurRenameItem.Path, newPath);
            }

            var t = RecentItemList.FirstOrDefault(e => e.Path.Equals(CurRenameItem.Path));
            if (null != t)
            {
                RecentManager.Remove(t);
                t.Name = curRenameStatus.Name;
                t.Path = newPath;
                RecentManager.Add(t);
            }
            t = PinnedItemList.FirstOrDefault(e => e.Path.Equals(CurRenameItem.Path));
            if (null != t)
            {
                PinManager.Unpin(t);
                t.Name = curRenameStatus.Name;
                t.Path = newPath;
                PinManager.Pin(t);
            }
            t = FoldersItemList.FirstOrDefault(e => e.Path.Equals(CurRenameItem.Path));
            if (null != t)
            {
                t.Name = curRenameStatus.Name;
                t.Path = newPath;
            }

            CancelRename(obj);
        }

        private void CancelRename(object obj)
        {
            CurRenameItem = null;
            CurRenameStatus.IsDoing = false;
            CurRenameStatus.Name = "";
            CurRenameStatus.IsError = false;
            CurRenameStatus.Desc = "";
        }

        private void BackNavigationClick(object obj)
        {
            CurFolder = nullFolder;
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
                FoldersItemList.Clear();

                DriveModel item = sender as DriveModel;

                if (item?.DriveName != null && item.DriveName != string.Empty)
                {
                    var defaultFolderPath = StarManager.GetDefault(item.DriveName, out string driveStarFile);
                    if (!string.IsNullOrEmpty(defaultFolderPath))
                    {
                        NavigateToFolder(defaultFolderPath);
                        directoryHistory.Push(item.DriveName);
                        directoryHistory.Push(defaultFolderPath);
                    }
                    else
                    {
                        NavigateToFolder(item.DriveName);
                        directoryHistory.Push(item.DriveName);
                    }

                    CurrentDrivePath = item.DriveName;
                    IsDriveFoldersVisible = Visibility.Visible;
                    IsDriveWindowVisible = Visibility.Collapsed;
                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowErrorMessage(ex);
                //throw ex;
            }

            //TxtSearchBoxItem = string.Empty;
            //IsSearchBoxPlaceholderVisible = Visibility.Visible;
            //CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
        }

        private void searchBoxTextChanged(object sender)
        {
            CurSearchStatus.IsError = false;
            CurSearchStatus.Desc = "";
            if (!string.IsNullOrEmpty(TxtSearchBoxItem))
            {
                IsSearchBoxPlaceholderVisible = Visibility.Collapsed;
                if (TxtSearchBoxItem.Length > 256)
                {
                    CurSearchStatus.IsError = true;
                    CurSearchStatus.Desc = "Search items have a maximum limit of 256 characters.";
                    return;
                }
                if (FoldersItemList.Count > 0)
                {
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FoldersItemList);
                    view.Filter = searchItemFilter;
                }
                else
                {
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(RecentItemList);
                    view.Filter = searchItemFilter; 
                    view = (CollectionView)CollectionViewSource.GetDefaultView(PinnedItemList);
                    view.Filter = searchItemFilter;
                }
                return;
            }
            else
            {
                IsSearchBoxPlaceholderVisible = Visibility.Visible;
                CollectionViewSource.GetDefaultView(FoldersItemList).Refresh();
            }
        }

        private string forbiddenChars = "\\/:*\"<>|";
        private void RenameTextChanged(object sender)
        {
            CurRenameStatus.IsError = false;
            CurRenameStatus.Desc = "";
            if (string.IsNullOrEmpty(CurRenameStatus.Name))
            {
                CurRenameStatus.IsError = true;
                CurRenameStatus.Desc = "Please type the name.";
                return;
            }
            foreach(var ch in forbiddenChars)
            {
                if (CurRenameStatus.Name.Contains(ch))
                {
                    CurRenameStatus.IsError = true;
                    CurRenameStatus.Desc = $"Name couldn't contain any of the following chars {forbiddenChars}";
                    return;
                }
            }
            if (CurRenameStatus.Name.Length > 256)
            {
                CurRenameStatus.IsError = true;
                CurRenameStatus.Desc = "File name or folder name has a maximum limit of 256 characters.";
                return;
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
                        ExceptionHelper.ShowErrorMessage(ex, $"Could not open file {SelectedFileFolderItem.Name}");
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
                    ExceptionHelper.ShowErrorMessage(ex, $"Could not open file or folder {SelectedRecentItem.Name}");                    
                    //throw ex;
                }
            }
        }

        public void PinnedItemDoubleClick(object sender)
        {
            var pinedItem = sender as FileFolderModel;
            if (pinedItem != null && !string.IsNullOrEmpty(pinedItem.Path))
            {
                try
                {
                    Process.Start(pinedItem.Path);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.ShowErrorMessage(ex, $"Could not open file or folder {pinedItem.Name}");                    
                    //throw ex;
                }
            }
        }

        public void NavigateToFolder(string folderPath)
        {
            TxtSearchBoxItem = "";
            try
            {
                if (!string.IsNullOrEmpty(folderPath))
                {
                    List<FileFolderModel> subFolderFiles = new List<FileFolderModel>();

                    // Check if the folder exists
                    if (Directory.Exists(folderPath))
                    {
                        if (!IsRootFolder(folderPath))
                        {
                            var curDir = new DirectoryInfo(folderPath);
                            CurFolder = new FileFolderModel
                            {
                                Path = curDir.FullName,
                                Name = curDir.Name,
                                LastModifiedDateTime = curDir.LastWriteTime,
                                IconSource = IconHelper.GetIcon(curDir.FullName),
                                Type = "Dir",
                                IsPined = PinManager.IsPined(folderPath),
                                IsDefault = StarManager.IsDefault(folderPath)
                            };
                        }

                        //Get subfolders
                        string[] subDirectories = Directory.GetDirectories(folderPath);
                        foreach (string subFolder in subDirectories)
                        {
                            DirectoryInfo subFolderInfo = new DirectoryInfo(subFolder);
                            if (!subFolderInfo.Attributes.ToString().Contains(FileAttributes.Hidden.ToString()) &&
                       !subFolderInfo.Attributes.ToString().Contains(FileAttributes.NotContentIndexed.ToString()) &&
                       !subFolderInfo.Attributes.ToString().Contains(FileAttributes.ReparsePoint.ToString()))
                            {

                                subFolderFiles.Add(new FileFolderModel
                                   {
                                       Path = subFolderInfo.FullName,
                                       Name = subFolderInfo.Name,
                                       LastModifiedDateTime = subFolderInfo.LastWriteTime,
                                       IconSource = IconHelper.GetIcon(subFolderInfo.FullName),
                                       Type = "Dir",
                                       IsDefault = StarManager.IsDefault(subFolder),
                                       IsPined = PinManager.IsPined(subFolder)
                                   });
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
                                    IconSource = IconHelper.GetIcon(fileInfo.FullName),
                                    Type = "File",
                                    IsDefault = StarManager.IsDefault(file),
                                    IsPined = PinManager.IsPined(file)
                                }
                                );
                            }
                        }
                    }
                    else
                    {
                        // ExceptionHelper.ShowErrorMessage("The folder does not exist.");
                    }
                    FoldersItemList.Clear();
                    subFolderFiles.ForEach(item =>
                    {
                        FoldersItemList.Add(item);
                    });
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
                return (item as FileFolderModel).Name.IndexOf(TxtSearchBoxItem, StringComparison.OrdinalIgnoreCase) >= 0;
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

        //private void RecentItemSort(object obj)
        //{
        //    IsAscending = !IsAscending;
        //}

        #endregion
    }
}
