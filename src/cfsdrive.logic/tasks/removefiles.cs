#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : removefiles.cs
 * 
 * Contents	: Implementation of the logic to remove files
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
    public class RemoveFiles : BaseTaskWithUser
    {
        private int[] _filesIds;

        public RemoveFiles(int[] filesIds, LoggedUser user, Action<ITaskResult> completionCallback) :
            base("RemoveFiles", user, completionCallback)
        {
            _filesIds = filesIds;
        }

        protected override void OnRun()
        {
            RestApiService apiService = GetRestApiService();
            string result = apiService.Execute(ApiUrls.Files.Delete, new ArrayQueryJsonParams<int>(_filesIds)).Result;
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = true;
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
