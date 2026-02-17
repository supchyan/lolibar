using System.Windows.Media;

namespace LolibarApp.Source.Tools;

static class LolibarColor
{
    /// <summary>
    /// Generates `SolidColorBrush` object, by getting HEX Color value. You can use RGB as well as ARGB color codes.
    /// Example: "#5500FF00" - Semi-transparent green color code.
    /// </summary>
    public static SolidColorBrush FromHEX(string color)
    {
        var brush = (SolidColorBrush?)new BrushConverter().ConvertFrom(color);

        return brush ?? new SolidColorBrush() { Color = System.Windows.Media.Color.FromRgb(0,0,0) };
    }
}
