﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CDM.Common;
using CDM.Helper;
using CDM.ViewModels;
using Microsoft.Win32;

namespace CDM.UserControls
{
    /// <summary>
    /// Interaction logic for CDMUserControl.xaml
    /// </summary>
    public partial class CDMUserControl : UserControl
    {
        public delegate void dlgtTest();
        public event dlgtTest EventTest;

        public CDMUserControl()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            InitializeComponent();
            this.DataContext = new CDMViewModel();
            SetInitialTheme();

            // Subscribe to system theme changes
            Microsoft.Win32.SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            //IsSystemInDarkMode();

            ListOfDriveItems.MouseRightButtonDown += ListOfDriveItems_MouseRightButtonDown;
        }

        private void ListOfDriveItems_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;
            HandleException(exception);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception);
            e.SetObserved();
        }

        private void HandleException(Exception ex)
        {
            ExceptionHelper.ShowErrorMessage(ex);
        }

        private void SetInitialTheme()
        {
            // Check the system theme initially
            bool isDarkTheme = IsSystemInDarkMode();
            //UpdateUIForTheme(isDarkTheme);
        }

        private bool IsSystemInDarkMode()
        {
            try
            {
                const string RegistryKeyPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
                const string RegistryValueName = "AppsUseLightTheme";

                //int res = (int)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", -1);

                int lightThemeValue = (int)Registry.GetValue(RegistryKeyPath, RegistryValueName, defaultValue: 1);
                // 0 - dark
                // 1 - Light
                if (lightThemeValue == 0)
                {
                    var darkStyle = new Uri("pack://application:,,,/CDM;component/Themes/DarkStyle.xaml", UriKind.RelativeOrAbsolute);
                    bool isUpdated = ResourceDictionaryManager.UpdateDictionary("Themes", darkStyle, this);
                    //ResourceDictionaryManager.UpdateResourceColor("NavibarBackgroundColor", (SolidColorBrush)(App.Current.Resources["NavibarBackgroundDarkColor"]));
                }
                else
                {
                    var lightStyle = new Uri("pack://application:,,,/CDM;component/Themes/LightStyle.xaml", UriKind.RelativeOrAbsolute);
                    bool isUpdated = ResourceDictionaryManager.UpdateDictionary("Themes", lightStyle, this);
                    // ResourceDictionaryManager.UpdateResourceColor("NavibarBackgroundColor", (SolidColorBrush)(App.Current.Resources["NavibarBackgroundLightColor"]));
                }

                return lightThemeValue == 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                // System theme might have changed, recheck and update UI
                bool isDarkTheme = IsSystemInDarkMode();
                //UpdateUIForTheme(isDarkTheme);
            }
        }
    }
}
