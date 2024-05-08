#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : createfolder.cs
 * 
 * Contents	: Implementation of the logic to create a folder
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
using cfsdrive.logic.services.rest.models;

namespace cfsdrive.logic.tasks
{
    public class CreateFolder : BaseTaskWithUser
    {
        private FolderCreateRequestParams _params;
        private int _parentId = 0;
        private int _newFolderId = 0;

        public CreateFolder(int parentId, string folderName, LoggedUser user, Action<ITaskResult> completionCallback) :
            base("CreateFolder", user, completionCallback)
        {
            _parentId = parentId;
            _params = new FolderCreateRequestParams(folderName);
        }

        protected override void OnRun()
        {
            RestApiService apiService = GetRestApiService();
            _newFolderId = apiService.Execute(ApiUrls.Folder.Create, _params, new object[] { _parentId.ToString() }).Result;
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _newFolderId;
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
