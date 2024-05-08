#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : getfoldercontent.cs
 * 
 * Contents	: Implementation of the logic to get content of selected folder
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
    public class GetFolderContent : BaseTaskWithUser
    {
        private FolderContentDto _foldercontent = null;
        private FolderContentRequestParams _params = null;
        private int _folderId = 0;

        public GetFolderContent(int folderId, int itemsCount, string sortColumn, bool isAscending, LoggedUser user, Action<ITaskResult> completionCallback) :
            base("GetFolderContent", user, completionCallback)
        {
            _folderId = folderId;
            _params = new FolderContentRequestParams
            {
                Count = itemsCount,
                Skip = 0,
                SortColumn = sortColumn,
                Ascending = isAscending
            };
        }

        protected override void OnRun()
        {
            RestApiService apiService = GetRestApiService();
            _foldercontent = apiService.Execute(ApiUrls.Folder.GetContent, _params, new object[] { _folderId <= 0 ? "root" : _folderId.ToString() }).Result;
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _foldercontent;
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
