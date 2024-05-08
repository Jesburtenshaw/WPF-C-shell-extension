#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : renameitem.cs
 * 
 * Contents	: Implementation of the logic to rename an item
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
    public class RenameItem : BaseTaskWithUser
    {
        private ItemRenameRequestParams _params;
        private int _itemId = 0;
        private bool _result = false;

        public RenameItem(int itemId, string newName, LoggedUser user, Action<ITaskResult> completionCallback) :
            base("RenameItem", user, completionCallback)
        {
            _itemId = itemId;
            _params = new ItemRenameRequestParams(newName);
        }

        protected override void OnRun()
        {
            RestApiService apiService = GetRestApiService();
            _result = apiService.Execute(ApiUrls.Files.Rename, _params, new object[] { _itemId.ToString() }).Result;
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _result;
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
