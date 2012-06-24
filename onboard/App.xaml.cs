using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;
using System.Globalization;

namespace Onboard
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;
        private static Client _Client = null;

        public static MainViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        public static Client Client
        {
            get
            {
                if (_Client == null)
                {
                    _Client = new Client();
                }
                return _Client;
            }
        }

        static void _Client_OnJournalUpdated(object sender, JournalEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
                ViewModel.Journal = e.JournalItems;
                ViewModel.IsUpdating = false;
            }));
        }

        static void _Client_OnBoardInfoUpdated(object sender, StateEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(new Action(delegate
             {
                 ViewModel.FromRussia = e.FromRussia;
                 ViewModel.ToRussia = e.ToRussia;
                 ViewModel.IsUpdating = false;
             }));
        }

        void Client_OnClientException(object sender, ClientExeptionEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
                Exception ex = e.Exeption;
                MessageBox.Show(e.Message);
                ViewModel.IsUpdating = false;
            }));

        }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;
            string[] cultureStrings = { "en-US", "ru-RU", "fi-FI" };
            CultureInfo c = Thread.CurrentThread.CurrentUICulture;
            if (cultureStrings.Contains(c.Name))
            {
                Onboard.Resources.Resource.Culture = c;
            }
            else
            {
                Onboard.Resources.Resource.Culture = CultureInfo.InvariantCulture;
            }
            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            Client.OnBoardInfoUpdated += new StateEventHandler(_Client_OnBoardInfoUpdated);
            Client.OnJournalUpdated += new JournalEventHandler(_Client_OnJournalUpdated);
            Client.OnJournalGetPrevious += new JournalEventHandler(Client_OnJournalGetPrevious);
            Client.OnClientException += new ClientExeptionHandler(Client_OnClientException);
           // Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");
            
            ViewModel.LoadData();
           
        }

        void Client_OnJournalGetPrevious(object sender, JournalEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(new Action(delegate
           {
               List<JournalItemInfo> old = ViewModel.Journal;
               old.AddRange(e.JournalItems);
               ViewModel.Journal = old;
               ViewModel.IsUpdating = false;
           }));
        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            ViewModel.SaveData();
        }

        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            ViewModel.SaveData();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                try
                {
                    ViewModel.SaveData();
                }
                catch { }
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                try
                {
                    ViewModel.SaveData();
                }
                catch { }
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new Microsoft.Phone.Controls.TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}