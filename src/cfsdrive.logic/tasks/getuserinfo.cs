#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : getuserinfo.cs
 * 
 * Contents	: Implementation of the logic to get user info
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
    public class GetUserInfo : BaseTaskWithUser
    {
        private UserInfoRequestParams _params;
        private UserInfoDto _userInfo;

        public GetUserInfo(LoggedUser user, Action<ITaskResult> completionCallback) :
            base("GetUserInfo", user, completionCallback)
        {
            _params = new UserInfoRequestParams();
        }

        protected override void OnRun()
        {
            RestApiService apiService = GetRestApiService();
            _userInfo = apiService.Execute(ApiUrls.Auth.GetUserInfo, _params).Result;
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _userInfo;
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
