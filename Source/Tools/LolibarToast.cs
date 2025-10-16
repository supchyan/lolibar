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
    Window          ToastWnd        { get; set; }   = new();
    /// <summary>
    /// Toast font size. (BarFontSize by default)
    /// </summary>
    public double   FontSize        { get; set; } = LolibarMod.BarFontSize;
    /// <summary>
    /// True when toast text is bold. (false by default)
    /// </summary>
    public bool     IsBold          { get; set; } = false;
    /// <summary>
    /// Toast width.
    /// </summary>
    public double   Width           { get; set; }   = 120;
    /// <summary>
    /// Toast height.
    /// </summary>
    public double   Height          { get; set; }   = 40;
    /// <summary>
    /// Toast content.
    /// </summary>
    public string   Text            { get; set; }   = "";
    /// <summary>
    /// Time in milliseconds toast will be shown.
    /// </summary>
    public double   ShowTime        { get; set; }   = 220;
    /// <summary>
    /// Toast background color. (BarColor by default)
    /// </summary>
    public SolidColorBrush Color    { get; set; }   = LolibarMod.BarColor;
    /// <summary>
    /// Toast text color. (BarContainersColor by default)
    /// </summary>
    public SolidColorBrush TextColor { get; set; } = LolibarMod.BarContainersColor;
    /// <summary>
    /// Toast drop shadow color. (BarShadowColor by default)
    /// </summary>
    public SolidColorBrush ShadowColor { get; set; } = LolibarMod.BarShadowColor;

    TextBlock TextBlockContainer { get; set; }

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
            FontFamily  = (System.Windows.Media.FontFamily)App.Current.Resources["mononoki"],
        };
        // Register event to set window transparent for mouse events
        ToastWnd.SourceInitialized += ToastWnd_SourceInitialized;

        TextBlockContainer = new TextBlock()
        {
            Text                = Text,
            TextWrapping        = TextWrapping.Wrap,
            FontSize            = FontSize,
            FontWeight          = IsBold ? FontWeights.Bold : FontWeights.Normal,
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
            Background      = Color, // LolibarColor.FromHEX("#ffffff");
            BorderThickness = LolibarMod.BarStrokeThickness,
            BorderBrush     = LolibarMod.BarStrokeColor,
            CornerRadius    = LolibarMod.BarCornerRadius,
            Effect          = DropShadowEffect,

            Margin = new Thickness(LolibarMod.BarShadowBlurRadius),
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
