#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : errordto.cs 
 * 
 * Contents	: Declaration of Error DTO
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


namespace cfsdrive.logic.services.rest.dto
{
    public class ErrorDto: BaseDto
    {
        public System.Net.HttpStatusCode Status { get; set; }

        public string Message { get; set; }

        public object Errors { get; set; }
    }
}