#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : assemblyextension.cs 
 * 
 * Contents	: Implementation of Assembly extension
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.IO;
using System.Reflection;

namespace cfsdrive.logic.extensions
{
    public static class AssemblyExtension
    {
        public static DateTime GetBuildDateTime(this Assembly assembly)
        {
            DateTime buildDate = new FileInfo(assembly.Location).LastWriteTime;
            return buildDate;
        }

        public static string GetDirectoryPath(this Assembly assembly)
        {
            string filePath = new Uri(assembly.CodeBase).LocalPath;
            return Path.GetDirectoryName(filePath);
        }
    }
}
