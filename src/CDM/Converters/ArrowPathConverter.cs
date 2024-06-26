﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CDM.Converters
{
    public class ArrowPathConverter : IValueConverter
    {
        public ArrowPathConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAscending = (bool)value;
            return isAscending ? "M0 0 L5 10 L10 0 Z" : "M0 10 L5 0 L10 10 Z";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
