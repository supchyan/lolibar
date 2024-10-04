using System.Windows;
using System.Windows.Media;

namespace LolibarApp.Source.Tools;

public static partial class LolibarHelper
{
    public static int GetWindowsScaling()
    {
        return (int)(100 * Screen.PrimaryScreen?.Bounds.Width ?? 0 / SystemParameters.PrimaryScreenWidth);
    }
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
    /// Simplifies container's events initialization.
    /// </summary>
    /// <param name="element">Actual container</param>
    /// <param name="MouseButtonLeftUp">Invokes action on `MouseButtonLeftUp`.</param>
    /// <param name="MouseButtonRightUp">Invokes action on `MouseButtonRightUp`.</param>
    public static void SetContainerEvents(this UIElement element, System.Windows.Input.MouseButtonEventHandler? MouseButtonLeftUpEvent = null, System.Windows.Input.MouseButtonEventHandler? MouseButtonRightUpEvent = null, System.Windows.Input.MouseWheelEventHandler? MouseWheelEvent = null)
    {
        if (MouseButtonLeftUpEvent != null )   element.PreviewMouseLeftButtonUp    += MouseButtonLeftUpEvent;
        if (MouseButtonRightUpEvent != null)   element.PreviewMouseRightButtonUp   += MouseButtonRightUpEvent;
        if (MouseWheelEvent != null        )   element.PreviewMouseWheel           += MouseWheelEvent;
        element.MouseEnter += LolibarEvents.UI_MouseEnter;
        element.MouseLeave += LolibarEvents.UI_MouseLeave;
    }

    /// <summary>
    /// Simulates specified Key Down event.
    /// </summary>
    public static void KeyDown(Keys vKey)
    {
        LolibarExtern.keybd_event((byte)vKey, 0, 0x0000, 0);
    }
    /// <summary>
    /// Simulates specified Key Up event.
    /// </summary>
    public static void KeyUp(Keys vKey)
    {
        LolibarExtern.keybd_event((byte)vKey, 0, 0x0002, 0);
    }
    /// <summary>
    /// Simulates WIN + TAB HotKey.
    /// </summary>
    public static void OpenWindowsDesktopsUI()
    {
        LolibarHelper.KeyDown(Keys.LWin);
        LolibarHelper.KeyDown(Keys.Tab);
        LolibarHelper.KeyUp(Keys.Tab);
        LolibarHelper.KeyUp(Keys.LWin);
    }
}
