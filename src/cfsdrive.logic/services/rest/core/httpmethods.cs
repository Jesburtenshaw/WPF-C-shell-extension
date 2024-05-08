#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : httpmethods.cs 
 * 
 * Contents	: Declaration the available httpmethods
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


namespace cfsdrive.logic.services.rest.core
{
    public enum HttpMethods
    {
        Get = 1,
        Post = 2,
        Put = 3,
        Delete = 4,
        PostPlain = 5,
        DeleteJson = 6,
        DownloadFile = 7,
        UploadFile = 8
    }
}
