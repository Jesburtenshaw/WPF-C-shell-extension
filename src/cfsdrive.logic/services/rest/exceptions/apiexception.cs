#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : apiexception.cs 
 * 
 * Contents	: Implementation of rest api exception
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using cfsdrive.logic.exceptions;
using cfsdrive.logic.services.rest.dto;

namespace cfsdrive.logic.services.rest.exceptions
{
    public class ApiException : CoreException
    {
        public ApiException() :
            base("Api error")
        {
        }

        public ApiException(string message) :
            base(message)
        {
        }

        public ApiException(ErrorDto error) :
            base($"Api error")
        {
            Error = error;
        }

        public ErrorDto Error { get; private set; }
    }
}
