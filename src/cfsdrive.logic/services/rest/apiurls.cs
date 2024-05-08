#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : apiurls.cs 
 * 
 * Contents	: Declaration of rest api urls
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using cfsdrive.logic.services.rest.core;
using cfsdrive.logic.services.rest.dto;
using cfsdrive.logic.services.rest.models;
using System.Collections.Generic;

namespace cfsdrive.logic.services.rest
{
    public static class ApiUrls
    {
        public static class Auth
        {
            public static ApiActionAttributes<LoginPostRequestParams, bool> Login =
                new ApiActionAttributes<LoginPostRequestParams, bool>("api/auth/login", HttpMethods.Post);

            public static ApiActionAttributes<UserInfoRequestParams, UserInfoDto> GetUserInfo =
                new ApiActionAttributes<UserInfoRequestParams, UserInfoDto>("api/auth/userInfo", HttpMethods.Get);
        }

        public static class Folder
        {
            public static ApiActionAttributes<FolderInfoRequestParams, FolderInfoDto> GetInfo =
                new ApiActionAttributes<FolderInfoRequestParams, FolderInfoDto>("api/files/{0}", HttpMethods.Get);

            public static ApiActionAttributes<FolderContentRequestParams, FolderContentDto> GetContent =
                new ApiActionAttributes<FolderContentRequestParams, FolderContentDto>("api/files/{0}/items", HttpMethods.Get);

            public static ApiActionAttributes<FolderCreateRequestParams, int> Create =
                new ApiActionAttributes<FolderCreateRequestParams, int>("api/files/{0}/folder", HttpMethods.Post);
        }

        public static class Files
        {
            public static ApiActionAttributes<ArrayQueryJsonParams<int>, string> Delete =
                new ApiActionAttributes<ArrayQueryJsonParams<int>, string>("api/files", HttpMethods.DeleteJson);

            public static ApiActionAttributes<CopyFilesRequestParams, int> Copy =
                new ApiActionAttributes<CopyFilesRequestParams, int>("api/files/copy", HttpMethods.Post);

            public static ApiActionAttributes<MoveFilesRequestParams, int> Move =
                new ApiActionAttributes<MoveFilesRequestParams, int>("api/files/move", HttpMethods.Post);

            public static ApiActionAttributes<ItemRenameRequestParams, bool> Rename =
                new ApiActionAttributes<ItemRenameRequestParams, bool>("api/files/{0}/rename", HttpMethods.Put);

            public static ApiActionAttributes<DownloadFileRequestParams, bool> Download =
                new ApiActionAttributes<DownloadFileRequestParams, bool>("api/files/{0}/download", HttpMethods.DownloadFile);

            public static ApiActionAttributes<UploadFileRequestParams, bool> Upload =
                new ApiActionAttributes<UploadFileRequestParams, bool>("api/files/{0}/upload", HttpMethods.UploadFile);
            
        }

        public static class Comments
        {
            public static ApiActionAttributes<GetFileCommentsRequestParams, CommentDto[]> Get =
                new ApiActionAttributes<GetFileCommentsRequestParams, CommentDto[]>("api/files/{0}/comments", HttpMethods.Get);
        }
    }
}
