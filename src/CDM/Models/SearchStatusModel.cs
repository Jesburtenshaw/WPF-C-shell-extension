using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CDM.Models
{
    public class SearchStatusModel : INotifyPropertyChanged
    {
        private string desc;
        public string Desc
        {
            get
            {
                return desc;
            }
            set
            {
                desc = value;
                OnPropertyChanged(nameof(Desc));
            }
        }

        private bool isError;
        public bool IsError
        {
            get
            {
                return isError;
            }
            set
            {
                isError = value;
                OnPropertyChanged(nameof(IsError));
            }
        }

        private bool canSearch;
        public bool CanSearch
        {
            get
            {
                return canSearch;
            }
            set
            {
                canSearch = value;
                OnPropertyChanged(nameof(CanSearch));
            }
        }

        private bool isDoing;
        public bool IsDoing
        {
            get
            {
                return isDoing;
            }
            set
            {
                isDoing = value;
                OnPropertyChanged(nameof(IsDoing));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
