#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : iquerystringparams.cs 
 * 
 * Contents	: Declaration the iquerystringparams interface
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


namespace cfsdrive.logic.services.rest.core
{
    public interface IQueryStringParams
    {
        string ToQueryString();
    }
}
