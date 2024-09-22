using System.Windows.Media;
using System.Windows;

namespace LolibarApp.Source.Tools
{
    public abstract class ModLolibar
    {
        // UI
        public static bool UseSystemTheme { get; set; }
        public static bool SnapToTop { get; set; }
        public static bool HideLeftContainers { get; set; }
        public static bool HideCenterContainers { get; set; }
        public static bool HideRightContainers { get; set; }

        public static int UpdateDelay { get; set; }

        public static double BarMargin { get; set; }
        public static double BarWidth { get; set; }
        public static double BarHeight { get; set; }
        public static double BarBorderRadius { get; set; }
        public static double BarOpacity { get; set; }
        public static double BarStroke { get; set; }
        public static double FontSize { get; set; }
        public static double IconSize { get; set; }
        public static double SeparatorWidth { get; set; }
        public static double SeparatorBorderRadius { get; set; }

        public static Thickness ElementMargin { get; set; }

        public static SolidColorBrush BarColor { get; set; }
        public static SolidColorBrush ElementColor { get; set; }

        public static string BarUserText { get; set; }
        public static string BarCurProcText { get; set; }
        public static string BarCpuText { get; set; }
        public static string BarRamText { get; set; }
        public static string BarDiskText { get; set; }
        public static string BarNetworkText { get; set; }
        public static string BarSoundText { get; set; }
        public static string BarPowerText { get; set; }
        public static string BarTimeText { get; set; }

        public static Geometry? BarSoundIcon { get; set; }
        public static Geometry? BarNetworkIcon { get; set; }
        public static Geometry? BarCpuIcon { get; set; }
        public static Geometry? BarCurProcIcon { get; set; }
        public static Geometry? BarDiskIcon { get; set; }
        public static Geometry? BarRamIcon { get; set; }
        public static Geometry? BarPowerIcon { get; set; }

        public abstract void Initialize();
        public abstract void Update();
    }
}
