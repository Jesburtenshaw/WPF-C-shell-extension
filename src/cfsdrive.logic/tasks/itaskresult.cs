#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : itaskresult.cs 
 * 
 * Contents	: Declaration the interface for task result
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using cfsdrive.logic.services.rest.dto;

namespace castleshield.securemail.core.tasks
{

    public interface ITaskResult
    {
        bool Success { get; set; }

        /// <summary>
        /// Error DTO if the error exists
        /// </summary>
        ErrorDto Error { get; set; }

        /// <summary>
        /// Tasks results object
        /// </summary>
        object Result { get; set; }
    }
}