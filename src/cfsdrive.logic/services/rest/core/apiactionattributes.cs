#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : apiactionattributes.cs 
 * 
 * Contents	: Implementation of api action's attributes
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


namespace cfsdrive.logic.services.rest.core
{
    public class ApiActionAttributes<TParams, TResult>
    {
        public string Action { get; set; }
        public HttpMethods Method { get; }

        public ApiActionAttributes(string action, HttpMethods method)
        {
            Action = action;
            Method = method;
        }
    }
}
