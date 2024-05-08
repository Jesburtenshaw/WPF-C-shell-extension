#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : logger.cs 
 * 
 * Contents	: Implementation of the common logger service
 * 
 * Author	:  Sergey Fasonov
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using cfsdrive.logic.extensions;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace cfsdrive.logic.services
{
    public sealed class Logger : cfsdrive.logic.interfaces.ILogger
    {
        private readonly ILog _logger; // logger instance

        public Logger()
        {
            _logger = LogManager.GetLogger(typeof(Logger));
        }

        /// <summary>
        /// Initialize the logger based on parameters
        /// </summary>
        /// <param name="logConfigFilePath"></param>
        /// <param name="isTurned"></param>
        /// <param name="logFilePath"></param>
        public void Initialize(string logConfigFilePath, bool isTurned, string logFilePath)
        {
            FileInfo fi = new FileInfo(logConfigFilePath);
            if (fi.Exists)
                log4net.Config.XmlConfigurator.Configure(fi);

            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Threshold = isTurned ? Level.All : Level.Off;
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);

            if (isTurned)
            {
                if (string.IsNullOrEmpty(logFilePath))
                {
                    logFilePath = Path.Combine(Assembly.GetExecutingAssembly().GetDirectoryPath(), "addin.log");
                }

                foreach (var fileAppender in LogManager.GetRepository().GetAppenders().OfType<FileAppender>())
                {
                    // apply transformation to the filename
                    fileAppender.File = logFilePath;
                    // notify the logging subsystem of the configuration change
                    fileAppender.ActivateOptions();
                }
            }

            LogInfo("Initialize the logger");
        }

        /// <summary>
        /// Logs a formatted message with Info level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void LogInfo(string message)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(message);
        }

        /// <summary>
        /// Logs a formatted message with Info level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void LogInfo(string message, params object[] args)
        {
            System.Diagnostics.Trace.TraceInformation(message, args);
            if (_logger.IsInfoEnabled)
                _logger.InfoFormat(message, args);
        }

        /// <summary>
        /// Logs an error - wrapper method
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void LogError(string message, params object[] args)
        {
            if (_logger.IsErrorEnabled)
                _logger.ErrorFormat(message, args);
        }

        /// <summary>
        /// Logs an error - wrapper method
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void LogError(Exception ex, string message, params object[] args)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(args == null || args.Length <= 0 ? message : string.Format(message, args), ex);
        }

        /// <summary>
        /// Logs an error - wrapper method
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public void LogError(Exception ex, string message)
        {
            LogError(ex, message, null);
        }

        /// <summary>
        /// Logs an error - wrapper method
        /// </summary>
        /// <param name="ex"></param>
        public void LogError(Exception ex)
        {
            LogError(ex, ex?.Message);
        }

        public List<string> GetLogFiles()
        {
            return LogManager.GetRepository().GetAppenders().OfType<FileAppender>().Select(log => log.File).ToList();
        }
    }
}
