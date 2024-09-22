using System.Windows.Media;
using System.Windows;

namespace LolibarApp.Source.Tools
{
    public abstract class ModLolibar
    {
        // UI
        /// <summary>
        /// When Lolibar's colors should be affected by system theme. (True by default)
        /// </summary>
        public static bool UseSystemTheme { get; set; }
        /// <summary>
        /// When Lolibar should be snapped to top of the screen. (False by default)
        /// </summary>
        public static bool SnapToTop { get; set; }
        /// <summary>
        /// Hides lolibar's left side content. (False by default)
        /// </summary>
        public static bool HideLeftContainers { get; set; }
        /// <summary>
        /// Hides lolibar's center content. (False by default)
        /// </summary>
        public static bool HideCenterContainers { get; set; }
        /// <summary>
        /// Hides lolibar's right side content. (False by default)
        /// </summary>
        public static bool HideRightContainers { get; set; }

        /// <summary>
        /// Time between update loop iterations in milliseconds. (1000ms by default)
        /// </summary>
        public static int UpdateDelay { get; set; }

        /// <summary>
        /// Margin between screen edges and the Lolibar.
        /// </summary>
        public static double BarMargin { get; set; }
        /// <summary>
        /// Lolibar's width property.
        /// </summary>
        public static double BarWidth { get; set; }
        /// <summary>
        /// Lolibar's height property.
        /// </summary>
        public static double BarHeight { get; set; }
        /// <summary>
        /// Lolibar's border radius property.
        /// </summary>
        public static double BarBorderRadius { get; set; }
        /// <summary>
        /// Lolibar's opacity property. (From 0 to 1)
        /// </summary>
        public static double BarOpacity { get; set; }
        /// <summary>
        /// Lolibar's stroke size property.
        /// </summary>
        public static double BarStrokeSize { get; set; }
        /// <summary>
        /// Lolibar's font size property.
        /// </summary>
        public static double FontSize { get; set; }
        /// <summary>
        /// Lolibar's icon size property.
        /// </summary>
        public static double IconSize { get; set; }
        /// <summary>
        /// Separator - that bar between elements in lolibar's containers.
        /// </summary>
        public static double SeparatorWidth { get; set; }
        /// <summary>
        /// Separator - that bar between elements in lolibar's containers.
        /// </summary>
        public static double SeparatorBorderRadius { get; set; }
        /// <summary>
        /// Margin between elements in lolibar's containers.
        /// </summary>
        public static Thickness ElementMargin { get; set; }
        /// <summary>
        /// Lolibar's main color.
        /// </summary>
        public static SolidColorBrush BarColor { get; set; }
        /// <summary>
        /// Lolibar's elements color. (Icons / Text / etc.)
        /// </summary>
        public static SolidColorBrush ElementColor { get; set; }
        /// <summary>
        /// Content of the User Container.
        /// </summary>
        public static string BarUserText { get; set; }
        /// <summary>
        /// Content of the Current Process Container.
        /// </summary>
        public static string BarCurProcText { get; set; }
        /// <summary>
        /// Content of the CPU Container.
        /// </summary>
        public static string BarCpuText { get; set; }
        /// <summary>
        /// Content of the RAM Container.
        /// </summary>
        public static string BarRamText { get; set; }
        /// <summary>
        /// Content of the Disk Container.
        /// </summary>
        public static string BarDiskText { get; set; }
        /// <summary>
        /// Content of the Network Container.
        /// </summary>
        public static string BarNetworkText { get; set; }
        /// <summary>
        /// Content of the Sound Container.
        /// </summary>
        public static string BarSoundText { get; set; }
        /// <summary>
        /// Content of the Power Container.
        /// </summary>
        public static string BarPowerText { get; set; }
        /// <summary>
        /// Content of the Time Container.
        /// </summary>
        public static string BarTimeText { get; set; }

        /// <summary>
        /// Icon of the Sound Container.
        /// </summary>
        public static Geometry? BarSoundIcon { get; set; }
        /// <summary>
        /// Icon of the Network Container.
        /// </summary>
        public static Geometry? BarNetworkIcon { get; set; }
        /// <summary>
        /// Icon of the CPU Container.
        /// </summary>
        public static Geometry? BarCpuIcon { get; set; }
        /// <summary>
        /// Icon of the Current Process Container.
        /// </summary>
        public static Geometry? BarCurProcIcon { get; set; }
        /// <summary>
        /// Icon of the Disk Container.
        /// </summary>
        public static Geometry? BarDiskIcon { get; set; }
        /// <summary>
        /// Icon of the RAM Container.
        /// </summary>
        public static Geometry? BarRamIcon { get; set; }
        /// <summary>
        /// Icon of the Power Container.
        /// </summary>
        public static Geometry? BarPowerIcon { get; set; }

        /// <summary>
        /// Initialization method. Invokes once at lolibar's launch.
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Update method. Invokes repeatedly every "UpdateDelay".
        /// </summary>
        public abstract void Update();
    }
}
