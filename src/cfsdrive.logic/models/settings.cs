#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : settings.cs 
 * 
 * Contents	: Implementation the addin's Configuration
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

namespace cfsdrive.logic.models
{
    /// <summary>
    /// Main Configuration settings
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Path to the log file
        /// </summary>
        public string LogFile { get; set; } = string.Empty;

        /// <summary>
        /// Is logger enabled
        /// </summary>
        public bool LogEnabled { get; set; } = false;

        public string TenantUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string DownloadFolderName { get; set; } = "tierfive";
    }
}
