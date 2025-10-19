using System.Windows;
using System.Windows.Media;

namespace LolibarApp.Source.Tools;

public abstract partial class LolibarProperties
{
    /// <summary>
    /// Font family. You need to specify a proper "Font name" here. Also, you need to insall your font before use here.
    /// </summary>
    public static string    BarFontFamily   { get; set; } = "Arial";
    /// <summary>
    /// Font weight.
    /// </summary>
    public static int       BarFontWeight   { get; set; } = 400;
    /// <summary>
    /// Shadow color. Use ARGB hex to make it transparent. (#80000000 by default)
    /// </summary>
    public static SolidColorBrush BarShadowColor { get; set; } = LolibarColor.FromHEX("#80000000");
    /// <summary>
    /// Shadow blur radius (strength) (8.0 by default).
    /// </summary>
    public static double BarShadowBlurRadius { get; set; } = 8.0;
    /// <summary>
    /// Defulat margin between children in context menu. (10.0 by default)
    /// </summary>
    public static double BarContextMenuChildMargin { get; set; } = 10.0;
    /// <summary>
    /// Align lolibar with some screen point. (Center by default)
    /// </summary>
    public static LolibarEnums.BarScreenPosition BarScreenPosition { get; set; } = LolibarEnums.BarScreenPosition.Center;
    /// <summary>
    /// Screen corner invoker. Addition to bool `BarCornersInvokesDesktopsMenu`.
    /// </summary>
    public static LolibarEnums.BarTargetCorner BarTargetCorner { get; set; } = LolibarEnums.BarTargetCorner.Left;
    /// <summary>
    /// Simulates feature from Gnome Linux systems, where you can open all apps in convenient presentation,
    /// moving the cursor to the statusbar's corner. You can modify the target corner with `BarTargetCorner` property. (False by default)
    /// </summary>
    public static bool BarCornersInvokesDesktopsMenu { get; set; } = false;
    /// <summary>
    /// When Lolibar should be snapped to top of the screen. (True by default)
    /// </summary>
    public static bool BarSnapToTop { get; set; } = true;
    /// <summary>
    /// Hides vanilla Windows taskbar. (False by default)
    /// </summary>
    public static bool BarHideVanillaTaskBar { get; set; } = false;

    /// <summary>
    /// Time between update loop iterations in milliseconds. (1000ms by default)
    /// </summary>
    public static int BarUpdateDelay { get; set; } = 1000;

    /// <summary>
    /// Margin between screen edges and the Lolibar. (new Thickness(8.0) by default)
    /// </summary>
    public static Thickness BarMargin { get; set; } = new Thickness(8.0);
    /// <summary>
    /// Lolibar's width property. (-1 by default, which is fit to screen width)
    /// </summary>
    public static double BarWidth { get; set; } = -1;
    /// <summary>
    /// Lolibar's height property. (60.0 by default)
    /// </summary>
    public static double BarHeight { get; set; } = 60.0;
    /// <summary>
    /// Lolibar's font size. (12 by default)
    /// </summary>
    public static int BarFontSize { get; set; } = 12;
    /// <summary>
    /// Lolibar's icon size. (16.0 by default)
    /// </summary>
    public static double BarIconSize { get; set; } = 16.0;
    /// <summary>
    /// Separator - that bar between elements in lolibar's containers. This is its width property.
    /// </summary>
    public static double BarSeparatorWidth { get; set; } = 4.0;
    /// <summary>
    /// Separator - that bar between elements in lolibar's containers. This is its height property.
    /// </summary>
    public static double BarSeparatorHeight { get; set; } = 16.0;
    /// <summary>
    /// Separator - that bar between elements in lolibar's containers. This is its border radius property.
    /// </summary>
    public static double BarSeparatorRadius { get; set; } = BarSeparatorWidth / 2.0;
    /// <summary>
    /// Lolibar outline color. (BarContainersColor by default)
    /// </summary>
    public static SolidColorBrush BarStrokeColor { get; set; } = BarContainersColor;
    /// <summary>
    /// Lolibar's stroke size property. (new Thickness(0) by default)
    /// </summary>
    public static Thickness BarStrokeThickness { get; set; } = new Thickness(0);
    /// <summary>
    /// Margin between containers in lolibar's containers. (new Thickness(0) by default)
    /// </summary>
    public static Thickness BarContainerMargin { get; set; } = new Thickness(0);
    /// <summary>
    /// Gap between Border and its content inside. ( Padding of the text inside its box, if it's more accurate )
    /// </summary>
    public static Thickness BarContainerInnerMargin { get; set; } = new Thickness(9.0, 5.0, 9.0, 5.0);
    /// <summary>
    /// Gap between elements inside border. ( Padding between the text and the icon inside their box [ border ], if it's more accurate )
    /// </summary>
    public static Thickness BarContainersContentMargin { get; set; } = new Thickness(5.0, 0.0, 5.0, 0.0);
    /// <summary>
    /// Gap between workspaces.
    /// </summary>
    public static Thickness BarWorkspacesMargin { get; set; } = new Thickness(8.0, 0.0, 8.0, 0.0);


    /// <summary>
    /// Lolibar's main color.
    /// </summary>
    public static SolidColorBrush BarColor { get; set; } = LolibarColor.FromHEX("#38393c");
    /// <summary>
    /// Lolibar's elements color. (Icons / Text / etc.)
    /// </summary>
    public static SolidColorBrush BarContainersColor { get; set; } = LolibarColor.FromHEX("#dddddd");

    /// <summary>
    /// Lolibar's border radius property. (new CornerRadius(8.0) by default)
    /// </summary>
    public static CornerRadius BarCornerRadius { get; set; } = new CornerRadius(8.0);
    /// <summary>
    /// Cornder radius of the elements containers. (new CornerRadius(3.0) by default)
    /// </summary>
    public static CornerRadius BarContainersCornerRadius { get; set; } = new CornerRadius(3.0);
}