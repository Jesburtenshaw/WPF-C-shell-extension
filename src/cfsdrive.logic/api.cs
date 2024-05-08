#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : api.cs 
 * 
 * Contents	: Main class provide functionality of API
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using cfsdrive.logic.helpers;
using cfsdrive.logic.interfaces;
using cfsdrive.logic.models;
using cfsdrive.logic.notification;
using cfsdrive.logic.services;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;

namespace cfsdrive.logic
{
    /// <summary>
    /// The class implements the main business logic of addin
    /// </summary>
    public class Api : SingletonBase<Api>
    {
        private const string SettingsConfigFile = "settings.json"; // file name of shapes configuration
        private const string RoamingFolderName = "cfsdrive";
        private readonly IServiceProvider _serviceProvider;
        private Api()
        {
            //SimpleIoc.Default.Register(InitializeConfiguration, true);
            //SimpleIoc.Default.Register(InitializeLogger, true);
            _serviceProvider = ConfigureServices();
        }
   
        private IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            // Register services
            serviceCollection.AddSingleton(InitializeConfiguration);
            serviceCollection.AddSingleton(InitializeLogger);
            // Add more services as needed

            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Returns the Addin's Configuration
        /// </summary>
        public Configuration<Settings> Configuration => Ioc.Default.GetService<Configuration<Settings>>();

        public Logger Logger => Ioc.Default.GetService<Logger>();// SimpleIoc.Default.GetInstance<Logger>();

        public LoggedUser User { get; private set; }

        // [SF] TEMPORARY SOLUTION
        public Clipboard Clipboard = null;

        /// <summary>
        /// Initializes the Api instance
        /// </summary>
        public void Init()
        {
            try
            {
                Logger.LogInfo("Initializes the Api class");

                Clipboard = new Clipboard
                {
                    ClipboardType = ClipboardType.None,
                    Items = null
                };

                WeakReferenceMessenger.Default.Register<LoginMessage>(this, (r,m) =>
                {
                    Logger.LogInfo("Received successfully LoginMessage in the API");
                    User = m.User;
                });

                WeakReferenceMessenger.Default.Register<LogoutMessage>(this, (r,m) =>
                {
                    Logger.LogInfo("Received LogoutMessage in the API");
                    User.IsLogged = false;
                });

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "The error occurred in App::Init");
            }
        }
        //public void Init()
        //{
        //    try
        //    {
        //        Logger.LogInfo("Initializes the Api class");

        //        Clipboard = new Clipboard
        //        {
        //            ClipboardType = ClipboardType.None,
        //            Items = null
        //        };

        //        Messenger.Default.Register(this, (Action<LoginMessage>)(m =>
        //        {
        //            Logger.LogInfo("Received successfully LoginMessage in the API");
        //            User = m.User;
        //        }));

        //        Messenger.Default.Register(this, (Action<LogoutMessage>)(m =>
        //        {
        //            Logger.LogInfo("Received LogoutMessage in the API");
        //            User.IsLogged = false;
        //        }));

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, "The error occured in App::Init");
        //    }
        //}

        /// <summary>
        /// Deinitializes the api
        /// </summary>
        public void Done()
        {
            Logger.LogInfo("Logic api is done");
        }

        public bool IsUserLogged
        {
            get
            {
                return User != null && User.IsLogged;
            }
        }

        /// <summary>
        /// Initializes the Configuration settings
        /// </summary>
        /// <returns></returns>
        private Configuration<Settings> InitializeConfiguration(IServiceProvider serviceProvider)
        {
            var config = new Configuration<Settings>(PathHelper.GetSettingsFile(SettingsConfigFile));
            if (config.Settings == null)
            {
                config.Settings = new Settings();
                config.Save();
            }
            return config;
        }

        private Logger InitializeLogger(IServiceProvider serviceProvider)
        {
            var logger = new Logger();
            // Assuming Logger.Initialize method accepts IConfiguration as a parameter
            logger.Initialize(PathHelper.GetLoggerSettingFile("logging.config"), serviceProvider.GetRequiredService<Configuration<Settings>>().Settings.LogEnabled, serviceProvider.GetRequiredService<Configuration<Settings>>().Settings.LogFile);
            return logger;
        }

        //private Configuration<Settings> InitializeConfiguration()
        //{
        //    Configuration<Settings> config = new Configuration<Settings>(PathHelper.GetSettingsFile(SettingsConfigFile));
        //    if (config.Settings == null)
        //    {
        //        config.Settings = new Settings();
        //        config.Save();
        //    }
        //    return config;
        //}

        //private Logger InitializeLogger()
        //{
        //    Logger logger = new Logger();
        //    logger.Initialize(PathHelper.GetLoggerSettingFile("logging.config"), Configuration.Settings.LogEnabled, Configuration.Settings.LogFile);
        //    return logger;
        //}
    }
}
