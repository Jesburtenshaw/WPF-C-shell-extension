#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : ilogger.cs 
 * 
 * Contents	: Declaration of ILogger interface to support logging
 * 
 * Author	:  Sergey Fasonov
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cfsdrive.logic.interfaces
{
    public interface ILogger : IService
    {
        /// <summary>
        /// Logs a formatted message with Info level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogInfo(string message);

        /// <summary>
        /// Logs a formatted message with Info level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// Logs an error - wrapper method
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogError(string message, params object[] args);

        /// <summary>
        /// Logs an error - wrapper method
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogError(Exception ex, string message, params object[] args);

        /// <summary>
        /// Logs an error - wrapper method
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        void LogError(Exception ex, string message);

        /// <summary>
        /// Logs an error - wrapper method
        /// </summary>
        /// <param name="ex"></param>
        void LogError(Exception ex);
    }
}
