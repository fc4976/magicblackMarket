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
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using PixelLab.Common;
using MemoryDiagnostics;

namespace BeijingTest2
{
    public partial class App : Application
    {
        /// <Share data among pages>
        public Myproperty Property { get; set; }
        public ObservableCollection<Commodity> AllCommodities { get; set; }
        public ObservableCollection<Commodity> SaleCommodities { get; set; }
        public ObservableCollection<Commodity> MyCommodities { get; set; }
        public IncidentList MyincidentList { get; set; }
        public MyNewsList MyNewsList {get; set;}
        public PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
        public bool IsContinueOldGame { get; set; }
        public bool IsFromIntroduce { get;set; }

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

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                //MemoryDiagnosticsHelper.Start(TimeSpan.FromMilliseconds(500), true);

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();
        }
        private void StoreCurrentState()
        {
            PhoneApplicationService.Current.State["Property"] = Property;
            PhoneApplicationService.Current.State["AllCommodities"] = AllCommodities;
            PhoneApplicationService.Current.State["SaleCommodities"] = SaleCommodities;
            PhoneApplicationService.Current.State["MyCommodities"] = MyCommodities;
            PhoneApplicationService.Current.State["MyincidentList"] = MyincidentList;
            PhoneApplicationService.Current.State["MyNews"] = MyNewsList;
        }
        public void RestoreCurrentState()
        {
            Property = PhoneApplicationService.Current.State["Property"] as Myproperty;
            AllCommodities = PhoneApplicationService.Current.State["AllCommodities"] as ObservableCollection<Commodity>;
            SaleCommodities = PhoneApplicationService.Current.State["SaleCommodities"] as ObservableCollection<Commodity>;
            MyCommodities = PhoneApplicationService.Current.State["MyCommodities"] as ObservableCollection<Commodity>;
            MyincidentList = PhoneApplicationService.Current.State["MyincidentList"] as IncidentList;
            MyNewsList = PhoneApplicationService.Current.State["MyNews"] as MyNewsList;
        }
        public void StartNewGame()
        {
            AllCommodityStore allCommodityStore = new AllCommodityStore();
            AllCommodities = allCommodityStore.CommodityStore.Commodities;
            MyCommodityStore myCommodityStore = new MyCommodityStore();
            MyCommodities = myCommodityStore.CommodityStore.Commodities;
            Property = new Myproperty();
            MyincidentList = new IncidentList();
            MyNewsList = new MyNewsList();   
        }
        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            StartNewGame();
            if (AppContextStoreRestoreClass.IsFileExist())
                IsContinueOldGame = true;
            else
                IsContinueOldGame = false;
        }
        
        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            RestoreCurrentState();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            StoreCurrentState();
            AppContextStoreRestoreClass.Store();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            AppContextStoreRestoreClass.Store();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
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
            RootFrame = new PhoneApplicationFrame();
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