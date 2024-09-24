using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Ikst.MouseHook;
using LolibarApp.Source.Tools;
using LolibarApp.Mods;

namespace LolibarApp.Source
{

    public partial class Lolibar : Window
    {
        // Misc
        MouseHook MouseHandler  = new();
        Config config = new(); // We use Config's object to invoke Update() and Initialize() methods.
        LolibarVirtualDesktop lolibarVirtualDesktop = new();

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

            Closed          += Lolibar_Closed;
            ContentRendered += Lolibar_ContentRendered;

            // ---

            BarUserContainer.SetContainerEvents(
                LolibarEvents.UI_MouseEnter,
                LolibarEvents.UI_MouseLeave,
                LolibarEvents.BarUserContainer_MouseLeftButtonUp,
                null
            );

            BarCurProcContainer.SetContainerEvents(
                LolibarEvents.UI_MouseEnter,
                LolibarEvents.UI_MouseLeave,
                LolibarEvents.BarCurProcContainer_MouseLeftButtonUp,
                null
            );

            // ---

            BarRamContainer.SetContainerEvents(
                LolibarEvents.UI_MouseEnter,
                LolibarEvents.UI_MouseLeave,
                null,
                LolibarEvents.BarRamContainer_MouseRightButtonUp
            );

            BarDiskContainer.SetContainerEvents(
                LolibarEvents.UI_MouseEnter,
                LolibarEvents.UI_MouseLeave,
                null,
                LolibarEvents.BarDiskContainer_MouseRightButtonUp
            );

            BarNetworkContainer.SetContainerEvents(
                LolibarEvents.UI_MouseEnter,
                LolibarEvents.UI_MouseLeave,
                null,
                LolibarEvents.BarNetworkContainer_MouseRightButtonUp
            );

            // ---

            BarPowerContainer.SetContainerEvents(
                LolibarEvents.UI_MouseEnter,
                LolibarEvents.UI_MouseLeave,
                LolibarEvents.BarPowerContainer_MouseLeftButtonUp,
                null
            );

            BarSoundContainer.SetContainerEvents(
                LolibarEvents.UI_MouseEnter,
                LolibarEvents.UI_MouseLeave,
                LolibarEvents.BarSoundContainer_MouseLeftButtonUp,
                null
            );

            // ---

            BarTimeContainer.SetContainerEvents(
                LolibarEvents.UI_MouseEnter,
                LolibarEvents.UI_MouseLeave,
                LolibarEvents.BarTimeContainer_MouseLeftButtonUp,
                null
            );

            InitializeCycle();
            UpdateCycle();

            // Should be below Initialize and Update calls, because it has Resources[] dependency
            MouseHandler.MouseMove += MouseHandler_MouseMove;
            MouseHandler.Start();

            GenerateTrayMenu();
        }

        void ListenForSystemThemeUsage()
        {
            if (!Config.UseSystemTheme) return;

            Config.BarColor        = ShouldSystemUseDarkMode() ? LolibarHelper.SetColor("#232428") : LolibarHelper.SetColor("#eeeeee");
            Config.BarElementColor = ShouldSystemUseDarkMode() ? LolibarHelper.SetColor("#b5bac1") : LolibarHelper.SetColor("#2d2d2d");
        }
        void PostInitializeContainersVisibility()
        {
            if (Config.HideBarLeftContainers)       BarLeftContainer.Visibility         = Visibility.Collapsed;
            if (Config.HideBarCenterContainers)     BarCenterContainer.Visibility       = Visibility.Collapsed;
            if (Config.HideBarRightContainers)      BarRightContainer.Visibility        = Visibility.Collapsed;
            if (Config.HideBarInfoContainer)        BarInfoContainer.Visibility         = Visibility.Collapsed;
            if (Config.HideBarWorkspacesContainer)  BarWorkspacesContainer.Visibility   = Visibility.Collapsed;
        }
        void PostInitializeSnapping()
        {
            if (!Config.SnapBarToTop)
            {
                StatusBarVisiblePosY = LolibarHelper.Inch_ScreenHeight - Config.BarHeight - Config.BarMargin;
                StatusBarHidePosY    = LolibarHelper.Inch_ScreenHeight;
            }
            else
            {
                StatusBarVisiblePosY = Config.BarMargin;
                StatusBarHidePosY    = -Config.BarHeight - Config.BarMargin;
            }
        }

        void UpdateDefaultInfo()
        {
            // Check if system theme affects lolibar's colors
            ListenForSystemThemeUsage();

            // Left Containers
            if(!Config.HideBarLeftContainers)
            {
                Config.BarUserText = LolibarDefaults.GetUserInfo();

                Config.BarCurProcText = LolibarDefaults.GetCurProcInfo();
            }

            // Center Containers
            if (!Config.HideBarCenterContainers)
            {
                Config.BarCpuText = LolibarDefaults.GetCpuInfo();

                Config.BarRamText = LolibarDefaults.GetRamInfo();

                Config.BarDiskText = LolibarDefaults.GetDiskInfo();
                Config.BarDiskIcon = LolibarDefaults.GetDiskIcon();         // Dynamically updates disk icon

                Config.BarNetworkText = LolibarDefaults.GetNetworkInfo();
                Config.BarNetworkIcon = LolibarDefaults.GetNetworkIcon();   // Dynamically updates network icon

                Config.BarAddTabText = LolibarDefaults.GetAddTabInfo();
            }

            // Right Containers
            if (!Config.HideBarRightContainers)
            {
                Config.BarPowerText = LolibarDefaults.GetPowerInfo();
                Config.BarPowerIcon = LolibarDefaults.GetPowerIcon();       // Dynamically updates power icon

                Config.BarSoundText = LolibarDefaults.GetSoundInfo();

                Config.BarTimeText = LolibarDefaults.GetTimeInfo();
            }
        }

        void UpdateResources()
        {
            // --- Global UI properties ---

            Resources["BarMargin"] = Config.BarMargin;
            Resources["BarHeight"] = Config.BarHeight;
            Resources["BarWidth"] = Config.BarWidth;

            Resources["BarColor"] = Config.BarColor;
            Resources["BarBorderRadius"] = Config.BarBorderRadius;
            Resources["BarOpacity"] = Config.BarOpacity;
            Resources["BarStrokeSize"] = Config.BarStrokeSize;

            Resources["BarElementColor"] = Config.BarElementColor;

            Resources["BarElementMargin"] = Config.BarElementMargin;
            Resources["BarWorkspacesMargin"] = Config.BarWorkspacesMargin;
            Resources["BarWorkspacesInnerMargin"] = Config.BarWorkspacesInnerMargin;

            Resources["BarIconSize"] = Config.BarIconSize;
            Resources["BarIconSizeSmall"] = Config.BarIconSizeSmall;

            Resources["BarFontSize"] = Config.BarFontSize;

            Resources["BarSeparatorWidth"] = Config.BarSeparatorWidth;
            Resources["BarSeparatorHeight"] = Config.BarSeparatorHeight;

            Resources["BarSeparatorBorderRadius"] = Config.BarSeparatorBorderRadius;


            // --- Left Containers ---

            Resources["BarAddTabText"] = Config.BarAddTabText;

            Resources["BarUserText"] = Config.BarUserText;

            Resources["BarCurProcText"] = Config.BarCurProcText;
            Resources["BarCurProcIcon"] = Config.BarCurProcIcon;

            // --- Center Containers ---

            Resources["BarCpuText"] = Config.BarCpuText;
            Resources["BarCpuIcon"] = Config.BarCpuIcon;

            Resources["BarRamText"] = Config.BarRamText;
            Resources["BarRamIcon"] = Config.BarRamIcon;

            Resources["BarDiskText"] = Config.BarDiskText;
            Resources["BarDiskIcon"] = Config.BarDiskIcon;

            Resources["BarNetworkText"] = Config.BarNetworkText;
            Resources["BarNetworkIcon"] = Config.BarNetworkIcon;

            // --- Right Containers ---

            Resources["BarSoundText"] = Config.BarSoundText;
            Resources["BarSoundIcon"] = Config.BarSoundIcon;

            Resources["BarPowerText"] = Config.BarPowerText;
            Resources["BarPowerIcon"] = Config.BarPowerIcon;

            Resources["BarTimeText"] = Config.BarTimeText;
        }
    }
}
