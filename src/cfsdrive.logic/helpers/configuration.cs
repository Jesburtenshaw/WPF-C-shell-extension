#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : configuration.cs 
 * 
 * Contents	: Implementation the global Configuration in json
 * 
 * Author	:  Sergey Fasonov
 * 
 */
#endregion

using System;
using System.IO;
using Newtonsoft.Json;

namespace cfsdrive.logic.helpers
{
    /// <summary>
    /// Class implements the configuration as singleton
    /// </summary>
    public class Configuration<TSomeSettings> where TSomeSettings : class
    {
        private readonly string _settingsFile;

        public Configuration(string settingsFile)
        {
            _settingsFile = settingsFile;
            Settings = Load(settingsFile);
        }

        public TSomeSettings Settings { get; set; } = null;

        /// <summary>
        /// Saves the configuraion settings
        /// </summary>
        public void Save()
        {
            using (StreamWriter file = File.CreateText(_settingsFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, Settings);
            }
        }

        #region Implementation

        /// <summary>
        /// Loads the settings
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private TSomeSettings Load(string filename)
        {
            if (!File.Exists(filename))
                return null;

            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize(file, typeof(TSomeSettings)) as TSomeSettings;
            }
        }

        #endregion Implementation
    }
}
