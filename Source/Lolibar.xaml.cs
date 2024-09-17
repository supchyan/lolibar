using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ikst.MouseHook;
using LolibarApp.Source.Tools;

namespace LolibarApp.Source
{

    public partial class Lolibar : Window
    {
        // Misc
        MouseHook MouseHandler = new();

        // For screen coordinates calculation
        Matrix transformToDevice;
        System.Windows.Size screenSize;
        public static double StatusBarVisiblePosY   { get; private set; }
        public static double StatusBarHidePosY      { get; private set; }

        // Drawing conditions
        bool IsHidden, oldIsHidden;

        // Null window to prevent lolibar's appearing inside alt+tab menu
        Window nullWin = new()
        {
            Visibility = Visibility.Hidden,
            WindowStyle = WindowStyle.ToolWindow,
            ShowInTaskbar = false,
            Width = 0, Height = 0,
            Top = LolibarHelper.Inch_ScreenWidth // to move it outside the screen 
        };

        // A trigger to prevent different app's job before... it's window actually rendered
        bool IsRendered;

        // System theme check
        [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
        public static extern bool ShouldSystemUseDarkMode();

        public Lolibar()
        {
            InitializeComponent();

            // Move lolibar into null window
            nullWin.Show();
            Owner = GetWindow(nullWin);

            Closed                                      += Lolibar_Closed;
            ContentRendered                             += Lolibar_ContentRendered;

            BarUserContainer.MouseEnter                 += Container_MouseEnter;
            BarUserContainer.MouseLeave                 += Container_MouseLeave;
            BarUserContainer.MouseLeftButtonUp          += BarUserContainer_MouseLeftButtonUp;

            BarCurProcContainer.MouseEnter              += Container_MouseEnter;
            BarCurProcContainer.MouseLeave              += Container_MouseLeave;
            BarCurProcContainer.MouseLeftButtonUp       += BarCurProcContainer_MouseLeftButtonUp;
            BarCurProcContainer.MouseRightButtonUp      += BarCurProcContainer_MouseRightButtonUp;

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

            Resources["UseSystemTheme"]         = true;     // If true, statusbar's default colors will be affected by system's current theme
            Resources["SnapToTop"]              = false;    // If true, snaps Lolibar to top of the screen
            Resources["UpdateDelay"]            = 500;      // Delay for Update() method. Low delay affects performance!

            //

            // --- Global UI properties ---

            Resources["BarMargin"]              = 8.0;
            Resources["BarHeight"]              = 42.0;
            Resources["BarWidth"]               = LolibarHelper.Inch_ScreenWidth - 2 * (double)Resources["BarMargin"]; // Fit to screen width as default

            Resources["BarColor"]               = LolibarHelper.SetColor("#2d2d2d");
            Resources["BarBorderRadius"]        = 6.0;
            Resources["BarOpacity"]             = 1.0;
            Resources["BarStroke"]              = 0.0;

            Resources["ElementColor"]           = LolibarHelper.SetColor("#eeeeee");
            Resources["ElementMargin"]          = new Thickness(16.0, 0.0, 16.0, 0.0);
            Resources["IconSize"]               = 16.0;
            Resources["FontSize"]               = 12.0;

            Resources["SeparatorWidth"]         = 4.0;
            Resources["SeparatorBorderRadius"]  = (double)Resources["SeparatorWidth"] / 2.0;

            //

            // --- Containers initialization ---

            Resources["BarCurProcText"]         = "";
            Resources["BarCurProcIcon"]         = LolibarDefaults.CurProcIcon;

            Resources["BarUserText"]            = "";

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
            // System theme affects lolibar's colors

            ListenForSystemThemeUsage();

            //

            // Left Container

            Resources["BarUserText"]            = LolibarDefaults.UserInfo;

            Resources["BarCurProcText"]         = LolibarDefaults.GetCurProcInfo();

            //

            // Center Container

            Resources["BarCpuText"]             = LolibarDefaults.CpuInfo;

            Resources["BarRamText"]             = LolibarDefaults.GetRamInfo();

            Resources["BarGpuText"]             = LolibarDefaults.GpuInfo;

            Resources["BarDiskText"]            = LolibarDefaults.DiskInfo;

            Resources["BarNetworkText"]         = LolibarDefaults.NetworkInfo;

            Resources["BarSoundText"]           = LolibarDefaults.SoundInfo;

            //

            // Right Container

            Resources["BarPowerText"]           = LolibarDefaults.PowerInfo;
            Resources["BarPowerIcon"]           = LolibarDefaults.PowerIcon; // Dynamically update power icon

            Resources["BarTimeText"]            = LolibarDefaults.TimeInfo;
        
            //
        }

        void ListenForSystemThemeUsage()
        {
            if ((bool)Resources["UseSystemTheme"])
            {
                Resources["BarColor"] = ShouldSystemUseDarkMode() ? LolibarHelper.SetColor("#2d2d2d") : LolibarHelper.SetColor("#eeeeee");
                Resources["ElementColor"] = ShouldSystemUseDarkMode() ? LolibarHelper.SetColor("#eeeeee") : LolibarHelper.SetColor("#2d2d2d");

            }
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
                StatusBarVisiblePosY = LolibarHelper.Inch_ScreenHeight - Height - (double)Resources["BarMargin"];
                StatusBarHidePosY    = LolibarHelper.Inch_ScreenHeight;
            }
            else
            {
                StatusBarVisiblePosY = (double)Resources["BarMargin"];
                StatusBarHidePosY    = -Height - (double)Resources["BarMargin"];
            }
        }

        #region Statusbar State handling
        
        #endregion
    }
}