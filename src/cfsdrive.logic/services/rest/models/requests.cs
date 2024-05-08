#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : requests.cs 
 * 
 * Contents	: Implementation of the requests query string parameters
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace cfsdrive.logic.services.rest.models
{
    /// <summary>
    /// Login json parameters
    /// </summary>
    public class LoginPostRequestParams : QueryJsonParams
    {
        [JsonProperty("login")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; } = null;
    }

    /// <summary>
    /// User info query parameters
    /// </summary>
    public class UserInfoRequestParams : QueryStringParams
    {

    }

    /// <summary>
    /// Folderinfo query parameters
    /// </summary>
    public class FolderInfoRequestParams : QueryStringParams
    {

    }

    /// <summary>
    /// Folder content query parameters
    /// </summary>
    public class FolderContentRequestParams : QueryStringParams
    {
        [DisplayName("take")]
        public int Count { get; set; }

        [DisplayName("skip")]
        public int Skip { get; set; }

        [DisplayName("sortCol")]
        public string SortColumn { get; set; }
        
        [DisplayName("isAsc")]
        public bool Ascending { get; set; }
    }

    /// <summary>
    /// Folder content query parameters
    /// </summary>
    public class GetFileCommentsRequestParams : QueryStringParams
    {

    }

    public class CopyFilesRequestParams : QueryJsonParams
    {
        [JsonProperty("fileIds")]
        public int[] FileIds { get; set; }

        [JsonProperty("destinationId")]
        public int DestinationId { get; set; }
    }

    public class MoveFilesRequestParams : QueryJsonParams
    {
        [JsonProperty("fileIds")]
        public int[] FileIds { get; set; }

        [JsonProperty("destinationId")]
        public int DestinationId { get; set; }
    }

    public class FolderCreateRequestParams : SimpleStringQueryJsonParams
    {
        public FolderCreateRequestParams(string value) :
            base(value)
        {

        }
    }

    public class ItemRenameRequestParams : SimpleStringQueryJsonParams
    {
        public ItemRenameRequestParams(string value) :
            base(value)
        {

        }
    }

    public class DownloadFileRequestParams : QueryStringParams
    {

    }

    public class UploadFileRequestParams : QueryStringParams
    {

    }
}
