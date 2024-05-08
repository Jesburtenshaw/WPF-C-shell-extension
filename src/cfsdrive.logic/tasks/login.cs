#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : login.cs
 * 
 * Contents	: Implementation of the logic to login user
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
    public class Login : BaseTask
    {
        private LoginPostRequestParams _params;
        private LoggedUser _user;
        private string _tenantUrl;

        public Login(string tenantUrl, string username, string password, Action<ITaskResult> completionCallback) :
            base("Login", completionCallback)
        {
            _tenantUrl = tenantUrl;
            _params = new LoginPostRequestParams
            {
                Username = username,
                Password = password
            };
        }

        protected override void OnRun()
        {
            RestApiService apiService = new RestApiService(_tenantUrl);
            bool result = apiService.Execute(ApiUrls.Auth.Login, _params).Result;

            _user = new LoggedUser
            {
                IsLogged = !result,
                TenantUrl = _tenantUrl
            };

            if (_user.IsLogged)
            {
                Uri uri = new Uri(_tenantUrl);
                IEnumerable<Cookie> responseCookies = apiService.Cookies.GetCookies(uri).Cast<Cookie>();
                foreach (Cookie cookie in responseCookies)
                {
                    _user.AuthCookieName = cookie.Name;
                    _user.AuthCookieValue = cookie.Value;
                }
            }
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _user;
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
