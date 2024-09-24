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
        /// When Lolibar should be snapped to top of the screen. (True by default)
        /// </summary>
        public static bool SnapBarToTop { get; set; }
        /// <summary>
        /// Hides lolibar's left side content. (False by default)
        /// </summary>
        public static bool HideBarLeftContainers { get; set; }
        /// <summary>
        /// Hides lolibar's center content. (False by default)
        /// </summary>
        public static bool HideBarCenterContainers { get; set; }
        /// <summary>
        /// Hides lolibar's right side content. (False by default)
        /// </summary>
        public static bool HideBarRightContainers { get; set; }
        /// <summary>
        /// Hides lolibar's user container. (False by default)
        /// </summary>
        public static bool HideBarUserContainer { get; set; }
        /// <summary>
        /// Hides lolibar's info container. (False by default)
        /// </summary>
        public static bool HideBarInfoContainer { get; set; }
        /// <summary>
        /// Hides lolibar's workspaces container. (False by default)
        /// </summary>
        public static bool HideBarWorkspacesContainer { get; set; }

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
        /// Lolibar's opacity property. (From 0 to 1)
        /// </summary>
        public static double BarOpacity { get; set; }
        /// <summary>
        /// Lolibar's font size property.
        /// </summary>
        public static double BarFontSize { get; set; }
        /// <summary>
        /// Lolibar's icon size property.
        /// </summary>
        public static double BarIconSize { get; set; }
        /// <summary>
        /// Lolibar's small icon size property.
        /// </summary>
        public static double BarIconSizeSmall { get; set; }
        /// <summary>
        /// Separator - that bar between elements in lolibar's containers. This is it's width property.
        /// </summary>
        public static double BarSeparatorWidth { get; set; }
        /// <summary>
        /// Separator - that bar between elements in lolibar's containers. This is it's height property.
        /// </summary>
        public static double BarSeparatorHeight { get; set; }
        /// <summary>
        /// Separator - that bar between elements in lolibar's containers. This is it's border radius property.
        /// </summary>
        public static double BarSeparatorRadius { get; set; }

        /// <summary>
        /// Lolibar's stroke size property.
        /// </summary>
        public static Thickness BarStrokeThickness { get; set; }
        /// <summary>
        /// Margin between containers in lolibar's containers.
        /// </summary>
        public static Thickness BarContainerMargin { get; set; }
        /// <summary>
        /// Gap between Border and it's content inside. ( Padding of the text inside it's box, if it's more accurate )
        /// </summary>
        public static Thickness BarContainerInnerMargin { get; set; }
        /// <summary>
        /// Gap between elements inside border. ( Padding between the text and the icon inside their box [ border ], if it's more accurate )
        /// </summary>
        public static Thickness BarContainersContentMargin { get; set; }
        /// <summary>
        /// Gap between workspaces.
        /// </summary>
        public static Thickness BarWorkspacesMargin { get; set; }
        

        /// <summary>
        /// Lolibar's containers color. ( That boxes, where sort of content draws )
        /// </summary>
        public static SolidColorBrush BarContainerColor { get; set; }
        /// <summary>
        /// Lolibar's main color.
        /// </summary>
        public static SolidColorBrush BarColor { get; set; }
        /// <summary>
        /// Lolibar's elements color. (Icons / Text / etc.)
        /// </summary>
        public static SolidColorBrush BarContainersContentColor { get; set; }

        /// <summary>
        /// Lolibar's border radius property.
        /// </summary>
        public static CornerRadius BarCornerRadius { get; set; }
        /// <summary>
        /// Cornder radius of the elements containers.
        /// </summary>
        public static CornerRadius BarContainersCornerRadius { get; set; }

        /// <summary>
        /// 'Add Workspace' text near Workspaces Container
        /// </summary>
        public static string BarAddWorkspaceText { get; set; }
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
        /// Content of the Power Container.
        /// </summary>
        public static string BarPowerText { get; set; }
        /// <summary>
        /// Content of the Time Container.
        /// </summary>
        public static string BarTimeText { get; set; }

        /// <summary>
        /// Icon of the Network Container.
        /// </summary>
        public static Geometry BarNetworkIcon { get; set; }
        /// <summary>
        /// Icon of the CPU Container.
        /// </summary>
        public static Geometry BarCpuIcon { get; set; }
        /// <summary>
        /// Icon of the Current Process Container.
        /// </summary>
        public static Geometry BarCurProcIcon { get; set; }
        /// <summary>
        /// Icon of the Disk Container.
        /// </summary>
        public static Geometry BarDiskIcon { get; set; }
        /// <summary>
        /// Icon of the RAM Container.
        /// </summary>
        public static Geometry BarRamIcon { get; set; }
        /// <summary>
        /// Icon of the Power Container.
        /// </summary>
        public static Geometry BarPowerIcon { get; set; }

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
