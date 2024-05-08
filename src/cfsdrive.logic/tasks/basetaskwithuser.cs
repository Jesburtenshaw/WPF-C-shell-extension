#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : basetaskwithuser.cs
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
    public abstract class BaseTaskWithUser : BaseTask
    {
        private LoggedUser _user;

        public BaseTaskWithUser(string taskName, LoggedUser user, Action<ITaskResult> completionCallback) :
            base(taskName, completionCallback)
        {
            _user = user;
        }

        internal RestApiService GetRestApiService()
        {
            return new RestApiService(_user.TenantUrl, _user.AuthCookieName, _user.AuthCookieValue);
        }
    }
}
