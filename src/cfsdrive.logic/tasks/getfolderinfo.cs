#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : getbreadcrumbs.cs
 * 
 * Contents	: Implementation of the logic to get breadcrumbs for selected folder
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using castleshield.securemail.core.tasks;
using cfsdrive.logic.models;
using cfsdrive.logic.services.rest;
using cfsdrive.logic.services.rest.dto;
using cfsdrive.logic.services.rest.models;

namespace cfsdrive.logic.tasks
{
    public class GetFolderInfo : BaseTaskWithUser
    {
        private FolderInfoDto _folderInfo = null;
        private int _folderId = 0;

        public GetFolderInfo(int folderId, LoggedUser user, Action<ITaskResult> completionCallback) :
            base("GetFolderInfo", user, completionCallback)
        {
            _folderId = folderId;
        }

        protected override void OnRun()
        {
            RestApiService apiService = GetRestApiService();
            _folderInfo = apiService.Execute(ApiUrls.Folder.GetInfo, null, new object[] { _folderId <= 0 ? "root" : _folderId.ToString() }).Result;
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _folderInfo;
                }

                _completionCallback?.Invoke(result);
            }
            catch (Exception ex)
            {
                Api.Instance.Logger.LogError(ex, $"The exception occured in the ExecutionFinished of task {Name}");
            }

            Api.Instance.Logger.LogInfo($"Task {Name} runs the continuous task.");
        }
    }
}
