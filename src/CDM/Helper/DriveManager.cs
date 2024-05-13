using CDM.Common;
using CDM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CDM.Helper
{
    public static class DriveManager
    {
        private static ObservableCollection<DriveModel> DriveList = new ObservableCollection<DriveModel>();

        public static ObservableCollection<DriveModel> GetDrivesItem()
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();//.Where(item => item.DriveType == DriveType.Network).ToArray();        
                foreach (DriveInfo drive in drives)
                {
                    DriveList.Add(new DriveModel()
                    {
                        DriveName = drive.Name,//.TrimEnd('\\'),
                        DriveDescription = drive.VolumeLabel,
                        IsPined = PinManager.IsPined(drive.Name)
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
                ExceptionHelper.ShowErrorMessage(ex);
            }
            return DriveList;
        }
    }
}