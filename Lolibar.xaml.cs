using System.Windows;
using System.Windows.Media;
using Ikst.MouseHook;
using lolibar.tools;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace lolibar
{

    public partial class Lolibar : Window
    {
        // Misc
        MouseHook MouseHandler = new();

        // For screen coordinates calculation
        Matrix transformToDevice;
        System.Windows.Size screenSize;
        static readonly double Inch_ScreenWidth  = SystemParameters.PrimaryScreenWidth;
        static readonly double Inch_ScreenHeight = SystemParameters.PrimaryScreenHeight;
        double StatusBarVisiblePosY, StatusBarHidePosY;

        // Drawing conditions
        bool IsHidden, oldIsHidden;

        // Null window to prevent lolibar's appearing inside alt+tab menu
        Window nullWin = new()
        {
            Visibility = Visibility.Hidden,
            WindowStyle = WindowStyle.ToolWindow,
            ShowInTaskbar = false,
            Width = 0, Height = 0,
            Top = Inch_ScreenWidth // to move it outside the screen 
        };

        // A trigger to prevent different app's job before... it's window actually rendered
        bool IsRendered;

        // A trigger to prevent app from being closed by any means.
        bool CanBeClosed;

        public Lolibar()
        {
            InitializeComponent();

            // Move lolibar into null window
            nullWin.Show();
            Owner = GetWindow(nullWin);

            Closed                                      += Lolibar_Closed;
            ContentRendered                             += Lolibar_ContentRendered;

            BarCurProcIdContainer.MouseEnter            += Container_MouseEnter;
            BarCurProcIdContainer.MouseLeave            += Container_MouseLeave;
            BarCurProcIdContainer.MouseLeftButtonUp     += BarCurProcContainer_MouseLeftButtonUp;

            BarCurProcNameContainer.MouseEnter          += Container_MouseEnter;
            BarCurProcNameContainer.MouseLeave          += Container_MouseLeave;
            BarCurProcNameContainer.MouseLeftButtonUp   += BarCurProcContainer_MouseLeftButtonUp;

            BarRamContainer.MouseEnter                  += Container_MouseEnter;
            BarRamContainer.MouseLeave                  += Container_MouseLeave;
            BarRamContainer.MouseLeftButtonUp           += BarRamContainer_MouseLeftButtonUp;

            BarPowerContainer.MouseEnter                += Container_MouseEnter;
            BarPowerContainer.MouseLeave                += Container_MouseLeave;
            BarPowerContainer.MouseLeftButtonUp         += BarPowerContainer_MouseLeftButtonUp;

            BarSoundContainer.MouseEnter                += Container_MouseEnter;
            BarSoundContainer.MouseLeave                += Container_MouseLeave;
            BarSoundContainer.MouseLeftButtonUp         += BarSoundContainer_MouseLeftButtonUp;

            BarTimeContainer.MouseEnter                 += Container_MouseEnter;
            BarTimeContainer.MouseLeave                 += Container_MouseLeave;
            BarTimeContainer.MouseLeftButtonUp          += BarTimeContainer_MouseLeftButtonUp;

            _Initialize();
            _Update();

            // Should be below Initialize and Update calls, because it has Resources[] dependency
            MouseHandler.MouseMove                      += MouseHandler_MouseMove;
            MouseHandler.Start();

            GenerateTrayMenu();
        }

        // Setup
        void SetDefaults()
        {
            // --- Setup properties ---

            Resources["SnapToTop"]              = false;    // If true, snaps Lolibar to top of the screen
            Resources["UpdateDelay"]            = 500;      // Delay for Update() method. Low delay affects performance!

            //

            // --- Global UI properties ---

            Resources["BarMargin"]              = 8.0;
            Resources["BarHeight"]              = 42.0;
            Resources["BarWidth"]               = Inch_ScreenWidth - 2 * (double)Resources["BarMargin"]; // Fit to screen width

            Resources["BarBorderRadius"]        = 6.0;
            Resources["BarOpacity"]             = 1.0;
            Resources["BarStroke"]              = 0.0;
            Resources["BarColor"]               = LolibarHelper.SetColor("#eeeeee");

            Resources["ElementMargin"]          = new Thickness(16.0, 0.0, 16.0, 0.0);
            Resources["ElementColor"]           = LolibarHelper.SetColor("#111111");
            Resources["IconSize"]               = 16.0;
            Resources["FontSize"]               = 12.0;

            Resources["SeparatorWidth"]         = 4.0;
            Resources["SeparatorBorderRadius"]  = (double)Resources["SeparatorWidth"] / 2.0;

            //

            // --- Containers initialization ---

            Resources["BarCurProcIdText"]       = ""; // No icon slot for this

            Resources["BarCurProcNameText"]     = ""; // No icon slot for this

            Resources["BarCpuText"]             = "";
            Resources["BarCpuIcon"]             = LolibarDefaults.CpuIcon;

            Resources["BarRamText"]             = "";
            Resources["BarRamIcon"]             = LolibarDefaults.RamIcon;

            Resources["BarGpuText"]             = "";
            Resources["BarGpuIcon"]             = LolibarDefaults.GpuIcon;

            Resources["BarDiskText"]            = "";
            Resources["BarDiskIcon"]            = LolibarDefaults.DiskIcon;

            Resources["BarNetworkText"]         = "";
            Resources["BarNetworkIcon"]         = LolibarDefaults.NetworkIcon;

            Resources["BarSoundText"]           = "";
            Resources["BarSoundIcon"]           = LolibarDefaults.SoundIcon;

            Resources["BarPowerText"]           = "";
            Resources["BarPowerIcon"]           = LolibarDefaults.PowerIcon;

            Resources["BarTimeText"]            = ""; // No icon slot for this

            //

            // --- Hiding triggers ---

            Resources["BarLeftContainerIsVisible"]   = true;
            Resources["BarCenterContainerIsVisible"] = true;
            Resources["BarRightContainerIsVisible"]  = true;

            //

        }
        void UpdateDefaults()
        {
            // Current process container

            Resources["BarCurProcIdText"]       = LolibarDefaults.CurProcIdInfo;
            Resources["BarCurProcNameText"]     = LolibarDefaults.CurProcNameInfo;

            //

            Resources["BarCpuText"]             = LolibarDefaults.CpuInfo;

            Resources["BarRamText"]             = LolibarDefaults.RamInfo();

            Resources["BarGpuText"]             = LolibarDefaults.GpuInfo;

            Resources["BarDiskText"]            = LolibarDefaults.DiskInfo;

            Resources["BarNetworkText"]         = LolibarDefaults.NetworkInfo;

            Resources["BarSoundText"]           = LolibarDefaults.SoundInfo;

            // Power container

            Resources["BarPowerText"]           = LolibarDefaults.PowerInfo;
            Resources["BarPowerIcon"]           = LolibarDefaults.PowerIcon; // Dynamically update power icon

            //

            Resources["BarTimeText"]            = LolibarDefaults.TimeInfo;
        }

        void PostInitializeContainersVisibility()
        {
            if (!(bool)Resources["BarLeftContainerIsVisible"])    BarLeftContainer.Visibility     = Visibility.Collapsed;
            if (!(bool)Resources["BarCenterContainerIsVisible"])  BarCenterContainer.Visibility   = Visibility.Collapsed;
            if (!(bool)Resources["BarRightContainerIsVisible"])   BarRightContainer.Visibility    = Visibility.Collapsed;
        }

        void PostInitializeSnapping()
        {
            if (!(bool)Resources["SnapToTop"])
            {
                StatusBarVisiblePosY = Inch_ScreenHeight - Height - (double)Resources["BarMargin"];
                StatusBarHidePosY    = Inch_ScreenHeight;
            }
            else
            {
                StatusBarVisiblePosY = (double)Resources["BarMargin"];
                StatusBarHidePosY    = -Height - (double)Resources["BarMargin"];
            }
        }

        #region App status Methods
        void CloseApplicationGently()
        {
            CanBeClosed = true;
            System.Windows.Application.Current.Shutdown();
        }
        // https://stackoverflow.com/questions/3895188/restart-application-using-c-sharp
        void RestartApplicationGently()
        {
            CanBeClosed = true;
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region Lifecycle Methods
        void __PreInitialize()
        {
            LolibarDefaults.Initialize();
            SetDefaults();
        }
        void __PostInitialize()
        {
            PostInitializeContainersVisibility();
            PostInitializeSnapping();
        }
        void _Initialize()
        {
            __PreInitialize();

            Initialize(); // From Lolibar_Config.cs

            __PostInitialize();
        }

        void __PreUpdate()
        {
            LolibarDefaults.Update();
            UpdateDefaults();
        }
        async void _Update()
        {
            while (true)
            {
                await Task.Delay((int)Resources["UpdateDelay"]);

                __PreUpdate();

                Update(); // From Lolibar_Config.cs
            }
        }
        #endregion
    }
}