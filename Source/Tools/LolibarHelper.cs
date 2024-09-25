using System.Windows;
using System.Windows.Media;

namespace LolibarApp.Source.Tools;

public static partial class LolibarHelper
{
    public static readonly double Inch_ScreenWidth = SystemParameters.PrimaryScreenWidth;
    public static readonly double Inch_ScreenHeight = SystemParameters.PrimaryScreenHeight;

    /// <summary>
    /// Generates `SolidColorBrush` object, by getting HEX Color value.
    /// </summary>
    public static SolidColorBrush SetColor(string color)
    {
        return (SolidColorBrush)new BrushConverter().ConvertFrom(color);
    }
    /// <summary>
    /// Converts ARGB Color to HEX one!
    /// </summary>
    public static string ARGBtoHEX(SolidColorBrush brush)
    {
        return $"#{Convert.ToHexString([brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B]).Replace("-", "")}";
    }

    /// <summary>
    /// Gently closes application.
    /// </summary>
    public static void CloseApplicationGently()
    {
        System.Windows.Application.Current.Shutdown();
    }
    // https://stackoverflow.com/questions/3895188/restart-application-using-c-sharp
    /// <summary>
    /// Gently restarts application.
    /// </summary>
    public static void RestartApplicationGently()
    {
        System.Windows.Forms.Application.Restart();
        System.Windows.Application.Current.Shutdown();
    }

    /// <summary>
    /// Simplifies initialize of the default events for `containers`.
    /// </summary>
    /// <param name="element">Actual container</param>
    /// <param name="mouseEnterEvent">Set 'null', if you don't need it</param>
    /// <param name="mouseLeaveEvent">Set 'null', if you don't need it</param>
    /// <param name="leftMouseClickEvent">Set 'null', if you don't need it</param>
    /// <param name="rightMouseClickEvent">Set 'null', if you don't need it</param>
    public static void SetContainerEvents(this UIElement element, System.Windows.Input.MouseEventHandler? mouseEnterEvent, System.Windows.Input.MouseEventHandler? mouseLeaveEvent, System.Windows.Input.MouseButtonEventHandler? leftMouseClickEvent, System.Windows.Input.MouseButtonEventHandler? rightMouseClickEvent)
    {
        if (mouseEnterEvent != null)        element.MouseEnter          += mouseEnterEvent;
        if (mouseLeaveEvent != null)        element.MouseLeave          += mouseLeaveEvent;
        if (leftMouseClickEvent != null)    element.PreviewMouseLeftButtonUp   += leftMouseClickEvent;
        if (rightMouseClickEvent != null)   element.PreviewMouseRightButtonUp += rightMouseClickEvent;
    }
}
