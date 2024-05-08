#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : getitemcomments.cs
 * 
 * Contents	: Implementation of the logic to get comments for selected folder
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
    public class GetItemComments : BaseTaskWithUser
    {
        private CommentDto[] _comments = null;
        private int _itemId = 0;

        public GetItemComments(int itemId, LoggedUser user, Action<ITaskResult> completionCallback) :
            base("GetItemComments", user, completionCallback)
        {
            _itemId = itemId;
        }

        protected override void OnRun()
        {
            RestApiService apiService = GetRestApiService();
            _comments = apiService.Execute(ApiUrls.Comments.Get, null, new object[] { _itemId.ToString() }).Result;
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _comments;
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
