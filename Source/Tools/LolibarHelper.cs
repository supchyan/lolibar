using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LolibarApp.Source.Tools;

public static class LolibarHelper
{
    static bool LeftButtonPressed   { get; set; }
    static bool RightButtonPressed  { get; set; }
    static bool MiddleButtonPressed { get; set; }
    /// <summary>
    /// Returns current System Primary Monitor "UI Scale". (i.e. that one in Display Settings: 100%-500%)
    /// </summary>
    /// <returns></returns>
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
    /// Simulates WIN + TAB keybind.
    /// </summary>
    public static void OpenWindowsDesktopsUI()
    {
        LolibarHelper.KeyDown(Keys.LWin);
        LolibarHelper.KeyDown(Keys.Tab);
        LolibarHelper.KeyUp(Keys.Tab);
        LolibarHelper.KeyUp(Keys.LWin);
    }
    /// <summary>
    /// Hides vanilla Windows Taskbar (Dockbar / Statusbar / i.e.) [DEPRECATED]
    /// </summary>
    //public static void HideWindowsTaskbar()
    //{
    //    var hwnd                = LolibarExtern.FindWindow("Shell_TrayWnd", "");
    //    var startButtonHandle   = LolibarExtern.FindWindowEx(LolibarExtern.GetDesktopWindow(), 0, "button", 0);

    //    LolibarExtern.ShowWindow(hwnd,              LolibarEnums.WindowStateEnum.Hide);
    //    LolibarExtern.ShowWindow(startButtonHandle, LolibarEnums.WindowStateEnum.Hide);
    //}
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
    /// <summary>
    /// Returns randomly generated string of specified length.
    /// </summary>
    /// <param name="length">Result string length</param>
    /// <returns></returns>
    public static string GetRandomString(int length)
    {
        string result = "";
        for (int i = 0; i < length; i++)
        {
            result += Convert.ToChar(new Random().Next(65, 91));
        }
        return result;
    }
    public static void SetWindowExTransparent(IntPtr hWnd)
    {
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        var extendedStyle = LolibarExtern.GetWindowLong(hWnd, GWL_EXSTYLE);
        _ = LolibarExtern.SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
    }
}