#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : copyfiles.cs
 * 
 * Contents	: Implementation of the logic to copy files
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
    public class CopyFiles : BaseTaskWithUser
    {
        private CopyFilesRequestParams _params;
        private int _copiedCount = 0;
        
        public CopyFiles(int[] filesIds, int destinationId, LoggedUser user, Action<ITaskResult> completionCallback) :
            base("CopyFiles", user, completionCallback)
        {
            _params = new CopyFilesRequestParams
            {
                FileIds = filesIds,
                DestinationId = destinationId
            };
        }

        protected override void OnRun()
        {
            RestApiService apiService = GetRestApiService();
            _copiedCount = apiService.Execute(ApiUrls.Files.Copy, _params).Result;
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _copiedCount == _params.FileIds.Length;
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
