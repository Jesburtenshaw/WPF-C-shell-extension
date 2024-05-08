#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : loggeduser.cs 
 * 
 * Contents	: Model contained the userinfo
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using cfsdrive.logic.services.rest.dto;

namespace cfsdrive.logic.models
{
    /// <summary>
    /// Main Configuration settings
    /// </summary>
    public class LoggedUser
    {
        public bool IsLogged { get; set; } = false;

        public string TenantUrl { get; set; } = string.Empty;

        public string AuthCookieName { get; set; }

        public string AuthCookieValue { get; set; }

        public UserInfoDto UserInfo { get; set; }
    }
}
