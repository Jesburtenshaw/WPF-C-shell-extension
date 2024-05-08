#region Copyright (c) 2021
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace cfsdrive.logic.helpers
{
    public static class PathHelper
    {
        /// <summary>
        /// Returns the path to executable assemly's folder
        /// </summary>
        /// <returns></returns>
        public static string GetExecutableFolder()
        {
            Uri uriCodeBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            return Path.GetDirectoryName(uriCodeBase.LocalPath);
        }

        /// <summary>
        /// Returns the user's specific temporary folder
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryFolder()
        {
            return Path.GetTempPath();
        }

        /// <summary>
        /// Returns the temporary installe full-qualified file naem
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryFile(string fileName)
        {
            return Path.Combine(GetTemporaryFolder(), fileName);
        }

        /// <summary>
        /// Returns the path of  settings file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetSettingsFile(string fileName)
        {
            return Path.Combine(GetExecutableFolder(), fileName);
        }

        /// <summary>
        /// Returns the path of common settings file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetCommonSettingsFile(string folder, string fileName)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return Path.Combine(folder, fileName);
        }

        /// <summary>
        /// Returns the path to the logger's settings file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetLoggerSettingFile(string fileName)
        {
            return Path.Combine(GetExecutableFolder(), fileName);
        }

        public static string GetSettingsFile(string folder, string fileName)
        {
            return Path.Combine(folder, fileName);
        }

        /// <summary>
        /// Returns the path to the log file
        /// </summary>
        /// <returns></returns>
        public static string GetLogFile()
        {
            string specificFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), GetExecutableFolder());

            // Check if folder exists and if not, create it
            if (!Directory.Exists(specificFolder))
                Directory.CreateDirectory(specificFolder);

            return Path.Combine(specificFolder, "addin.log");
        }

        public static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        static extern bool PathCompactPathEx([Out] StringBuilder pszOut, string szPath, int cchMax, int dwFlags);

        public static string TruncatePath(string fullFilePath, int length)
        {
            if (fullFilePath.Length <= length)
                return fullFilePath;
            StringBuilder sb = new StringBuilder(length + 1);
            PathCompactPathEx(sb, fullFilePath, length + 1, 0);
            return sb.ToString();
        }

        /// <summary>
        /// Returns the path to the assembly in executable folder
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAssemblyPath(string name)
        {
            return Path.Combine(GetExecutableFolder(), name);
        }

        private static string numberPattern = " ({0})";

        public static string NextAvailableFilename(string path)
        {
            // Short-cut if already available
            if (!File.Exists(path))
                return path;

            // If path has extension then insert the number pattern just before the extension and return next filename
            if (Path.HasExtension(path))
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path)), numberPattern));

            // Otherwise just append the pattern to the path and return next filename
            return GetNextFilename(path + numberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);
            
            if (!File.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }

        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public static bool IsDirectory(string fileName)
        {
            FileAttributes attr = File.GetAttributes(fileName);
            return attr.HasFlag(FileAttributes.Directory);
        }
    }
}
