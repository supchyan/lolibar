using System.Windows;
using System.Windows.Media;

namespace LolibarApp.Source.Tools;

public static partial class LolibarHelper
{
    public static double Inch_ScreenWidth  { get; set; }
    public static double Inch_ScreenHeight { get; set; }

    public static void PreUpdateInchScreenSize()
    {
        Inch_ScreenWidth    = SystemParameters.PrimaryScreenWidth;
        Inch_ScreenHeight   = SystemParameters.PrimaryScreenHeight;
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
    public static void SetContainerEvents(this UIElement element, System.Windows.Input.MouseButtonEventHandler? MouseButtonLeftUp = null, System.Windows.Input.MouseButtonEventHandler? MouseButtonRightUp = null)
    {
        if (MouseButtonLeftUp != null )   element.PreviewMouseLeftButtonUp    += MouseButtonLeftUp;
        if (MouseButtonRightUp != null)   element.PreviewMouseRightButtonUp   += MouseButtonRightUp;

        element.MouseEnter += LolibarEvents.UI_MouseEnter;
        element.MouseLeave += LolibarEvents.UI_MouseLeave;
    }
}
