using System.Drawing.Printing;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LolibarApp.Source.Tools;

public static partial class LolibarHelper
{
    static bool LeftButtonPressed { get; set; }
    static bool RightButtonPressed { get; set; }
    static bool MiddleButtonPressed { get; set; }
    public static int GetWindowsScaling()
    {
        return (int)(100 * Screen.PrimaryScreen?.Bounds.Width ?? 0 / SystemParameters.PrimaryScreenWidth);
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
    public static string Truncate(this string str, int length, bool drawDots = true)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }
        var dots = drawDots ? "..." : string.Empty;
        return str.Length <= length ? str : $"{str[0..length]}{dots}";
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
        this UIElement container,
        Func<MouseButtonEventArgs, int>? MouseLeftButtonUp      = null,
        Func<MouseButtonEventArgs, int>? MouseRightButtonUp     = null,
        Func<MouseButtonEventArgs, int>? MouseMiddleButtonUp    = null,
        Func<MouseWheelEventArgs,  int>? MouseWheelDelta        = null
    )
    {
        // Left button
        if (MouseLeftButtonUp != null)
        {
            container.PreviewMouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                LeftButtonPressed = Mouse.LeftButton == MouseButtonState.Pressed;
            };
            container.PreviewMouseUp += (object sender, MouseButtonEventArgs e) =>
            {
                if (LeftButtonPressed)
                {
                    MouseLeftButtonUp(e);
                    LeftButtonPressed = false;
                }
            };
        }
        // Right button
        if (MouseRightButtonUp != null)
        {
            container.PreviewMouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                RightButtonPressed = Mouse.RightButton == MouseButtonState.Pressed;
            };
            container.PreviewMouseUp += (object sender, MouseButtonEventArgs e) =>
            {
                if (RightButtonPressed)
                {
                    MouseRightButtonUp(e);
                    RightButtonPressed = false;
                }
            };
        }
        // Middle button
        if (MouseMiddleButtonUp != null)
        {
            container.PreviewMouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                MiddleButtonPressed = Mouse.MiddleButton == MouseButtonState.Pressed;
            };
            container.PreviewMouseUp += (object sender, MouseButtonEventArgs e) =>
            {
                if (MiddleButtonPressed)
                {
                    MouseMiddleButtonUp(e);
                    MiddleButtonPressed = false;
                }
            };
        }
        // Wheel delta
        if (MouseWheelDelta != null)
        {
            container.PreviewMouseWheel += (object sender, MouseWheelEventArgs e) =>
            {
                MouseWheelDelta(e);
            };
        }

        container.MouseEnter += LolibarEvents.UI_MouseEnter;
        container.MouseLeave += LolibarEvents.UI_MouseLeave;
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
    /// <summary>
    /// Adjusts Lolibar to the center at the top or bottom of the screen, depends on original location.
    /// </summary>
    /// <param name="BarWidth">Modded BarWidth property.</param>
    /// <param name="BarMargin">Modded BarMargin property.</param>
    /// <returns></returns>
    public static (double BarWidth, double BarLeft) OffsetLolibarToCenter(double BarWidth, double BarMargin)
    {
        return
        (
            Lolibar.Inch_Screen.X > 2 * BarMargin ? Lolibar.Inch_Screen.X - 2 * BarMargin : BarWidth,
            (Lolibar.Inch_Screen.X - BarWidth) / 2 > 0 ? (Lolibar.Inch_Screen.X - BarWidth) / 2 : 0
        );
    }
    /// <summary>
    /// Hides vanilla Windows Taskbar (Dockbar / Statusbar / i.e.)
    /// </summary>
    public static void HideWindowsTaskbar()
    {
        var hwnd                = LolibarExtern.FindWindow("Shell_TrayWnd", "");
        var startButtonHandle   = LolibarExtern.FindWindowEx(LolibarExtern.GetDesktopWindow(), 0, "button", 0);

        LolibarExtern.ShowWindow(hwnd,              LolibarEnums.WindowStateEnum.Hide);
        LolibarExtern.ShowWindow(startButtonHandle, LolibarEnums.WindowStateEnum.Hide);
    }
    /// <summary>
    /// Shows vanilla Windows Taskbar (Dockbar / Statusbar / i.e.)
    /// </summary>
    public static void ShowWindowsTaskbar()
    {
        var hwnd                = LolibarExtern.FindWindow("Shell_TrayWnd", "");
        var startButtonHandle   = LolibarExtern.FindWindowEx(LolibarExtern.GetDesktopWindow(), 0, "button", 0);

        LolibarExtern.ShowWindow(hwnd,              LolibarEnums.WindowStateEnum.ShowNormal);
        LolibarExtern.ShowWindow(startButtonHandle, LolibarEnums.WindowStateEnum.ShowNormal);
    }
}
