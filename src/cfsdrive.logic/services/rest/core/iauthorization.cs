#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : iauthorization.cs 
 * 
 * Contents	: Declaration of the iauthorization interface
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


namespace cfsdrive.logic.services.rest.core
{
    public interface IApiAuthorization
    {
        string Username { get; set; }

        string ApiKey { get; set; }
    }
}
