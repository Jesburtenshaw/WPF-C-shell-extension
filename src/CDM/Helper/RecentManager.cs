﻿using CDM.Common;
using CDM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CDM.Helper
{
    public static class RecentManager
    {
        private static string recentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Recent);

        public static ObservableCollection<FileFolderModel> RecentItemList = new ObservableCollection<FileFolderModel>();

        public static ObservableCollection<FileFolderModel> GetRecentItems()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                RecentItemList.Clear();
            });

            // Get all files in the Recent folder
            string[] recentFiles = Directory.GetFiles(recentFolder);
            foreach (string recentFile in recentFiles)
            {
                var file = ShortcutHelper.GetLnkTarget(recentFile);
                if (String.IsNullOrEmpty(file))
                {
                    continue;
                }
                if (File.Exists(file))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    FileAttributes f = File.GetAttributes(file);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        RecentItemList.Add(new FileFolderModel()
                        {

                            Name = fileInfo.Name,
                            LastModifiedDateTime = fileInfo.LastWriteTime,
                            Path = fileInfo.FullName,
                            IconSource = IconHelper.GetIcon(fileInfo.FullName),
                            OriginalPath = recentFile,
                            IsPined = PinManager.IsPined(fileInfo.FullName),
                            Type = "File"
                        });
                    });
                }
                else if (Directory.Exists(file))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(file);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        RecentItemList.Add(new FileFolderModel()
                        {
                            Name = dirInfo.Name,
                            LastModifiedDateTime = dirInfo.LastWriteTime,
                            Path = dirInfo.FullName,
                            IconSource = IconHelper.GetIcon(dirInfo.FullName),
                            OriginalPath = recentFile,
                            IsPined = PinManager.IsPined(dirInfo.FullName),
                            IsDefault = StarManager.IsDefault(dirInfo.FullName),
                            Type = "Dir"
                        });
                    });
                }
            }
            // Get all folders in the Recent folder
            // string[] recentFolders = Directory.GetDirectories(recentFolderPath);            
            return RecentItemList;
        }

        public static void Add(FileFolderModel item)
        {
            var t = RecentItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            if (null != t)
            {
                return;
            }

            if (item.Type == "File")
            {
                item.OriginalPath = Path.Combine(recentFolder, $"{Path.GetFileNameWithoutExtension(item.Path)}.lnk");
            }
            else
            {
                item.OriginalPath = Path.Combine(recentFolder, $"{DirectoryHelper.GetDirectoryName(item.Path)}.lnk");
            }
            ShortcutHelper.CreateLnk(item.OriginalPath, item.Path);

            RecentItemList.Insert(0, item);
            CollectionViewSource.GetDefaultView(RecentItemList).Refresh();
        }

        public static void Remove(FileFolderModel item)
        {
            var t = RecentItemList.FirstOrDefault(e => e.Path.Equals(item.Path));
            if (null == t)
            {
                return;
            }
            File.Delete(item.OriginalPath);
            RecentItemList.Remove(item);
            CollectionViewSource.GetDefaultView(RecentItemList).Refresh();
        }
    }
}