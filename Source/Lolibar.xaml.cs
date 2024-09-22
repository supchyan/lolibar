using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Ikst.MouseHook;
using LolibarApp.Source.Tools;
using LolibarApp.Source.Mods;

namespace LolibarApp.Source
{

    public partial class Lolibar : Window
    {
        // Misc
        MouseHook MouseHandler  = new();
        Config config = new(); // Object, to invoke initialize and update methods

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
                Container_MouseEnter,
                Container_MouseLeave,
                BarUserContainer_MouseLeftButtonUp,
                null
            );

            BarCurProcContainer.SetContainerEvents(
                Container_MouseEnter,
                Container_MouseLeave,
                BarCurProcContainer_MouseLeftButtonUp,
                BarCurProcContainer_MouseRightButtonUp
            );

            // ---

            BarRamContainer.SetContainerEvents(
                Container_MouseEnter,
                Container_MouseLeave,
                null,
                BarRamContainer_MouseRightButtonUp
            );

            BarDiskContainer.SetContainerEvents(
                Container_MouseEnter,
                Container_MouseLeave,
                null,
                BarDiskContainer_MouseRightButtonUp
            );

            BarNetworkContainer.SetContainerEvents(
                Container_MouseEnter,
                Container_MouseLeave,
                null,
                BarNetworkContainer_MouseRightButtonUp
            );

            // ---

            BarPowerContainer.SetContainerEvents(
                Container_MouseEnter,
                Container_MouseLeave,
                BarPowerContainer_MouseLeftButtonUp,
                null
            );

            BarSoundContainer.SetContainerEvents(
                Container_MouseEnter,
                Container_MouseLeave,
                BarSoundContainer_MouseLeftButtonUp,
                null
            );

            // ---

            BarTimeContainer.SetContainerEvents(
                Container_MouseEnter,
                Container_MouseLeave,
                BarTimeContainer_MouseLeftButtonUp,
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
            Config.ElementColor    = ShouldSystemUseDarkMode() ? LolibarHelper.SetColor("#b5bac1") : LolibarHelper.SetColor("#2d2d2d");
        }

        void PostInitializeSnapping()
        {
            if (!Config.SnapToTop)
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
            // System theme affects lolibar's colors

            ListenForSystemThemeUsage();

            //

            // Left Container

            Config.BarUserText = LolibarDefaults.GetUserInfo();

            Config.BarCurProcText = LolibarDefaults.GetCurProcInfo();

            //

            // Center Container

            Config.BarCpuText = LolibarDefaults.GetCpuInfo();

            Config.BarRamText = LolibarDefaults.GetRamInfo();

            Config.BarDiskText = LolibarDefaults.GetDiskInfo();
            Config.BarDiskIcon = LolibarDefaults.GetDiskIcon();    // Dynamically update disk icon

            Config.BarNetworkText = LolibarDefaults.GetNetworkInfo();
            Config.BarNetworkIcon = LolibarDefaults.GetNetworkIcon(); // Dynamically update network icon

            Config.BarSoundText = LolibarDefaults.GetSoundInfo();

            //

            // Right Container

            Config.BarPowerText = LolibarDefaults.GetPowerInfo();
            Config.BarPowerIcon = LolibarDefaults.GetPowerIcon(); // Dynamically update power icon

            Config.BarTimeText = LolibarDefaults.GetTimeInfo();

            //
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
            Resources["BarStroke"] = Config.BarStroke;

            Resources["ElementColor"] = Config.ElementColor;
            Resources["ElementMargin"] = Config.ElementMargin;
            Resources["IconSize"] = Config.IconSize;
            Resources["FontSize"] = Config.FontSize;

            Resources["SeparatorWidth"] = Config.SeparatorWidth;
            Resources["SeparatorBorderRadius"] = Config.SeparatorBorderRadius;

            // --- Containers ---

            Resources["BarUserText"] = Config.BarUserText;

            Resources["BarCurProcText"] = Config.BarCurProcText;
            Resources["BarCurProcIcon"] = Config.BarCurProcIcon;

            // ---

            Resources["BarCpuText"] = Config.BarCpuText;
            Resources["BarCpuIcon"] = Config.BarCpuIcon;

            Resources["BarRamText"] = Config.BarRamText;
            Resources["BarRamIcon"] = Config.BarRamIcon;

            Resources["BarDiskText"] = Config.BarDiskText;
            Resources["BarDiskIcon"] = Config.BarDiskIcon;

            Resources["BarNetworkText"] = Config.BarNetworkText;
            Resources["BarNetworkIcon"] = Config.BarNetworkIcon;

            // ---

            Resources["BarSoundText"] = Config.BarSoundText;
            Resources["BarSoundIcon"] = Config.BarSoundIcon;

            Resources["BarPowerText"] = Config.BarPowerText;
            Resources["BarPowerIcon"] = Config.BarPowerIcon;

            Resources["BarTimeText"] = Config.BarTimeText;

            //
        }
    }
}
