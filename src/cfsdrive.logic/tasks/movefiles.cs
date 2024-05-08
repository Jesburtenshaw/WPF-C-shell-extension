#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : movefiles.cs
 * 
 * Contents	: Implementation of the logic to move files
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
    public class MoveFiles : BaseTaskWithUser
    {
        private MoveFilesRequestParams _params;
        private int _moviedCount = 0;
        
        public MoveFiles(int[] filesIds, int destinationId, LoggedUser user, Action<ITaskResult> completionCallback) :
            base("MoveFiles", user, completionCallback)
        {
            _params = new MoveFilesRequestParams
            {
                FileIds = filesIds,
                DestinationId = destinationId
            };
        }

        protected override void OnRun()
        {
            RestApiService apiService = GetRestApiService();
            _moviedCount = apiService.Execute(ApiUrls.Files.Move, _params).Result;
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _moviedCount == _params.FileIds.Length;
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
