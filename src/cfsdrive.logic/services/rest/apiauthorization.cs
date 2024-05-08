#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : apiauthorization.cs 
 * 
 * Contents	: Implementation of the API Authorization
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


using cfsdrive.logic.services.rest.core;

namespace cfsdrive.logic.services.rest
{
    public class ApiAuthorization : IApiAuthorization
    {
        public string Username { get; set; }

        public string ApiKey { get; set; }
    }
}
