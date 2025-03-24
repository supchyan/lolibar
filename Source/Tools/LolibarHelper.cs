using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LolibarApp.Source.Tools;

public static partial class LolibarHelper
{
    static bool MiddleButtonPressed { get; set; }
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
    /// Converts `ico` files to image bitmap source.
    /// </summary>
    /// <param name="icon"></param>
    /// <returns></returns>
    public static BitmapSource ToBitmapSource(this Icon icon)
    {
        return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    }
    /// <summary>
    /// Truncates string with "..." at the end. Example: `Hello wo...`
    /// </summary>
    /// <param name="length">Max string length.</param>
    /// <returns></returns>
    public static string Truncate(this string str, int length)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        return str.Length <= length ? str : $"{str[0..length]}...";
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
    /// <param name="MouseButtonLeftUpEvent">EVENT called on LEFT mouse button up.</param>
    /// <param name="MouseButtonRightUpEvent">EVENT called on RIGHT mouse button up.</param>
    /// <param name="MouseWheelEvent">EVENT called on WHEEL mouse delta changes (mouse wheel `up` or `down` scroll).</param>
    /// <param name="MouseMiddleButtonUpFunc">FUNCTION called on MIDDLE mouse button up.</param>
    public static void SetContainerEvents
    (
        this UIElement element,
        MouseButtonEventHandler?    MouseButtonLeftUpEvent      = null,
        MouseButtonEventHandler?    MouseButtonRightUpEvent     = null,
        MouseWheelEventHandler?     MouseWheelEvent             = null,
        Func<int>?                  MouseMiddleButtonUpFunc     = null
    )
    {
        if (MouseButtonLeftUpEvent      != null) element.PreviewMouseLeftButtonUp    += MouseButtonLeftUpEvent;
        if (MouseButtonRightUpEvent     != null) element.PreviewMouseRightButtonUp   += MouseButtonRightUpEvent;
        if (MouseWheelEvent             != null) element.PreviewMouseWheel           += MouseWheelEvent;

        if (MouseMiddleButtonUpFunc     != null)
        {
            element.PreviewMouseDown    += (object sender, MouseButtonEventArgs e) =>
            {
                MiddleButtonPressed = Mouse.MiddleButton == MouseButtonState.Pressed;
            };
            element.PreviewMouseUp      += (object sender, MouseButtonEventArgs e) =>
            {
                if (MiddleButtonPressed)
                {
                    MouseMiddleButtonUpFunc();
                    MiddleButtonPressed = false;
                }
            };
        }


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
