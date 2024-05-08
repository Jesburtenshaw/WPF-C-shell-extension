#region Copyright (c) 2018
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : utilities.cs 
 * 
 * Contents	: Implementation the common utilities
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace cfsdrive.logic.helpers
{
    /// <summary>
    /// Helper class to convert the string value to the enum's value
    /// </summary>
    public static class EnumConverter
    {
        public static T ToEnum<T>(object value, T defaultValue) where T : struct
        {
            if (value == null)
                return defaultValue;

            if (string.IsNullOrEmpty(value.ToString()))
            {
                return defaultValue;
            }

            T result;
            return Enum.TryParse<T>(value.ToString(), true, out result) ? result : defaultValue;
        }
    }

    /// <summary>
    /// Helper class to convert object to different single types
    /// </summary>
    public static class ObjectConverter
    {
        /// <summary>
        /// Converts value to float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(object value)
        {
            float fVal = float.MinValue;
            try
            {
                fVal = Convert.ToSingle(value);
            }
            catch (Exception ex)
            {
                ex.GetType();
            }
            return fVal;
        }

        /// <summary>
        /// Converts value to integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(object value)
        {
            int iVal = int.MinValue;
            try
            {
                iVal = Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                ex.GetType();
            }
            return iVal;
        }

        /// <summary>
        /// Converts value to long integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(object value)
        {
            long lVal = int.MinValue;
            try
            {
                lVal = Convert.ToInt64(value);
            }
            catch (Exception ex)
            {
                ex.GetType();
                lVal = 0;
            }
            return lVal;
        }

        /// <summary>
        /// Converts value to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(object value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch (Exception ex)
            {
                ex.GetType();
            }
            return false;
        }

        /// <summary>
        /// Converts value to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(object value)
        {
            return value?.ToString() ?? string.Empty;
        }
    }

    public static class DataRowFieldExtension
    {
        public static string ToString(DataRow row, string fieldIndex)
        {
            if (row.IsNull(fieldIndex))
                return null;
            return ObjectConverter.ToString(row[fieldIndex]);
        }

        public static long ToLong(DataRow row, string fieldIndex)
        {
            if (row.IsNull(fieldIndex))
                return long.MinValue;

            return ObjectConverter.ToLong(row[fieldIndex]);
        }

        public static DateTime ToDateTime(DataRow row, string fieldIndex)
        {
            if (row.IsNull(fieldIndex))
                return DateTime.MinValue;

            return (DateTime)row[fieldIndex];
        }

        public static bool IsNull(DataRow row, string fieldIndex)
        {
            return row.IsNull(fieldIndex);
        }
    }

    public static class DataTimeConverter
    {
        public static DateTime FromTicks(long ticks)
        {
            long beginTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

            DateTime dt = new DateTime(beginTicks + ticks * 10000, DateTimeKind.Utc);
            return dt.ToLocalTime();
        }

        public static int UnixTimeStampUtc(DateTime currentTime)
        {
            DateTime zuluTime = currentTime.ToUniversalTime();
            DateTime unixEpoch = new DateTime(1970, 1, 1);
            return (int)(zuluTime.Subtract(unixEpoch)).TotalSeconds;
        }

    }

    public static class EnumExtensions
    {
        public static bool IsOneOf(this Enum enumeration, params Enum[] enums)
        {
            return enums.Contains(enumeration);
        }
    }

    public static class FileSizeFormatter
    {
        // Load all suffixes in an array  
        static readonly string[] _suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        public static string FormatSize(long bytes)
        {
            int counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }
            return $"{number:n1}{_suffixes[counter]}";
        }
    }
}
