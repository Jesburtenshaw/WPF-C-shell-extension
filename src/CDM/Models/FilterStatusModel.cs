﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CDM.Models
{
    public class FilterStatusModel : INotifyPropertyChanged
    {        
        private bool showDrives;
        public bool ShowDrives
        {
            get
            {
                return showDrives;
            }
            set
            {
                showDrives = value;
                OnPropertyChanged(nameof(ShowDrives));
            }
        }

        private bool showTypes;
        public bool ShowTypes
        {
            get
            {
                return showTypes;
            }
            set
            {
                showTypes = value;
                OnPropertyChanged(nameof(ShowTypes));
            }
        }

        private string curDrives;
        public string CurDrives
        {
            get
            {
                if (string.IsNullOrEmpty(curDrives))
                {
                    curDrives = "Drive";
                }
                return curDrives;
            }
            set
            {
                curDrives = value;
                OnPropertyChanged(nameof(CurDrives));
            }
        }

        private string curType;
        public string CurType
        {
            get
            {
                if (string.IsNullOrEmpty(curType))
                {
                    curType = "Type";
                }
                return curType;
            }
            set
            {
                curType = value;
                OnPropertyChanged(nameof(CurType));
            }
        }

        private bool isFiltering;
        public bool IsFiltering
        {
            get
            {
                return isFiltering;
            }
            set
            {
                isFiltering = value;
                OnPropertyChanged(nameof(IsFiltering));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}