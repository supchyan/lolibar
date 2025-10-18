using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LolibarApp.Source.Tools;

/// <summary>
/// Class, which provides capabilities to create context menus for lolibar.
/// </summary>
public class LolibarToast
{
    Window          ToastWnd    { get; set; }   = new();
    /// <summary>
    /// Container's text content, which will be drawn inside.
    /// </summary>
    public string?  Text        { get; set; }
    /// <summary>
    /// Font family. You need to put your font into lolibar's `Fonts` folder, then install it.
    /// After that, you can put your font name inside FontFamily property here. (Equals to `BarFontFamily` by default)
    /// </summary>
    public string?  FontFamily  { get; set; } = LolibarMod.BarFontFamily;
    /// <summary>
    /// Font weight. (Equals to `BarFontWeight` by default)
    /// </summary>
    public int      FontWeight  { get; set; } = LolibarMod.BarFontWeight;
    /// <summary>
    /// Font Size. (Equals to `BarFontSize` by default)
    /// </summary>
    public int      FontSize    { get; set; } = LolibarMod.BarFontSize;
    /// <summary>
    /// Time in milliseconds toast will be shown.
    /// </summary>
    public double   ShowTime    { get; set; } = 220;
    /// <summary>
    /// Toast text color. (Equals to `BarContainersColor` by default)
    /// </summary>
    public SolidColorBrush TextColor        { get; set; } = LolibarMod.BarContainersColor;
    /// <summary>
    /// Toast background color. (Equals to `BarColor` by default)
    /// </summary>
    public SolidColorBrush BackgroundColor  { get; set; } = LolibarMod.BarColor;
    /// <summary>
    /// Toast drop shadow color. (Equals to `BarShadowColor` by default)
    /// </summary>
    public SolidColorBrush ShadowColor { get; set; } = LolibarMod.BarShadowColor;

    public void Create()
    {
        // Display Scale setting offset. (i. e. "125% Recommended" stuff fix)
        var scaleOffset = Lolibar.ScreenSize.X / Lolibar.Inch_Screen.X;

        ToastWnd = new()
        {
            Name = LolibarHelper.GetRandomString(10),

            Opacity = 0,

            SizeToContent = SizeToContent.WidthAndHeight,

            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = VerticalAlignment.Center,

            WindowStyle     = WindowStyle.None,
            ResizeMode      = ResizeMode.NoResize,
            Visibility      = Visibility.Collapsed,

            AllowsTransparency  = true,
            Topmost             = true,
            UseLayoutRounding   = true,
            ShowInTaskbar       = false,
            IsTabStop           = false,

            Background = LolibarColor.FromHEX("#00000000"),

            FontSize    = LolibarMod.BarFontSize,
            //FontFamily  = (System.Windows.Media.FontFamily)App.Current.Resources["mononoki"],
        };
        // Register event to set window transparent for mouse events
        ToastWnd.SourceInitialized += ToastWnd_SourceInitialized;

        var TextBlockContainer = new TextBlock()
        {
            Text                = Text,
            TextWrapping        = TextWrapping.Wrap,
            FontSize            = FontSize,
            FontFamily          = new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"), $"Fonts/#{FontFamily}"),
            FontWeight          = System.Windows.FontWeight.FromOpenTypeWeight(FontWeight),
            Margin              = LolibarMod.BarContainersContentMargin,
            Padding             = new Thickness(2 * LolibarMod.BarShadowBlurRadius),
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center,
            Foreground          = TextColor,
        };

        var DropShadowEffect = new DropShadowEffect()
        {
            ShadowDepth = 0,
            Opacity     = 1,
            BlurRadius  = LolibarMod.BarShadowBlurRadius,
            Color       = ShadowColor.Color,
        };

        var BorderContainer = new Border()
        {
            Background      = BackgroundColor, // LolibarColor.FromHEX("#ffffff");
            BorderThickness = LolibarMod.BarStrokeThickness,
            BorderBrush     = LolibarMod.BarStrokeColor,
            CornerRadius    = LolibarMod.BarCornerRadius,
            Effect          = DropShadowEffect,
            Margin          = new Thickness(LolibarMod.BarShadowBlurRadius),
        };

        BorderContainer.Child   = TextBlockContainer;
        ToastWnd.Content        = BorderContainer;

        // Prevent toast window appearing in Alt+Tab UI
        Lolibar.HideFromAltTab(ToastWnd);

        ToastWnd.Show();

        // Set menu spawn position
        if (LolibarMod.BarSnapToTop)
        {
            ToastWnd.Top = LolibarMod.BarHeight;
        }
        else
        {
            ToastWnd.Top = Lolibar.Inch_Screen.Y - LolibarMod.BarHeight - ToastWnd.Height;
        }

        ToastWnd.Left = (Lolibar.Inch_Screen.X - ToastWnd.Width) / 2;

        LolibarAnimator.ContextMenu.Show(ToastWnd);

        // Start async loop to close toast window on ShowTime expire (ShowTime = 0)
        CloseOnExpire();
    }

    private void ToastWnd_SourceInitialized(object? sender, EventArgs e)
    {
        // Make toast window transparent for mouse events
        var hwnd = new WindowInteropHelper((Window)sender).Handle;
        LolibarHelper.SetWindowExTransparent(hwnd);
    }

    async void CloseOnExpire()
    {
        var timeStamp = DateTime.Now.Ticks;
        while (timeStamp + ShowTime * 10000 > DateTime.Now.Ticks)
        {
            await Task.Delay(1);
        }
        LolibarAnimator.ContextMenu.Hide(ToastWnd);
    }
}
