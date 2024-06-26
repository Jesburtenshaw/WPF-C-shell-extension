﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CDM.Models
{
    public class FileFolderModel : INotifyPropertyChanged
    {
        public string Path { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public BitmapSource IconSource { get; set; }
        public string OriginalPath { get; set; }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string type;
        /// <summary>
        /// Dir,File
        /// </summary>
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private bool isPined;
        public bool IsPined
        {
            get
            {
                return isPined;
            }
            set
            {
                isPined = value;
                OnPropertyChanged(nameof(IsPined));
            }
        }

        private bool isDefault;
        public bool IsDefault
        {
            get
            {
                return isDefault;
            }
            set
            {
                isDefault = value;
                OnPropertyChanged(nameof(isDefault));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}