#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : uiresources.cs 
 * 
 * Contents	: Provides a base interface to get resources from ui module
 * 
 * Author	:  Sergey Fasonov
 * 
 */
#endregion

using System.IO;

namespace cfsdrive.logic.interfaces
{
    public interface IUiResourceService
    {
        /// <summary>
        /// Returns string from UI resources
        /// </summary>
        /// <returns></returns>
        string GetString(string key);

        /// <summary>
        /// Returns image (as stream from) resources
        /// </summary>
        /// <returns></returns>
        object GetImage(string key);

        Stream GetImageStream(string key);
    }
}
