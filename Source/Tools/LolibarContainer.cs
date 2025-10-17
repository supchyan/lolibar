
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;

namespace LolibarApp.Source.Tools;

/// <summary>
/// Class, which provides capabilities to create basic lolibar UI layout.
/// </summary>
public class LolibarContainer
{
    /// <summary>
    /// Name of the container. Beware of dublicate names.
    /// </summary>
    string?                         Name                        { get; set; }
    /// <summary>
    /// Parent, where this container should be drawn inside.
    /// </summary>
    public StackPanel?              Parent                      { get; set; }
    /// <summary>
    /// Container's icon content, which will be drawn inside.
    /// </summary>
    public object?                  Icon                        { get; set; }
    /// <summary>
    /// Icon angle in degrees. (0 by default)
    /// </summary>
    public double                   IconAngle                   { get; set; }       = 0.0;
    /// <summary>
    /// Container's text content, which will be drawn inside.
    /// </summary>
    public string?                  Text                        { get; set; }
    /// <summary>
    /// Container content's color. (Equals `BarContainersColor` by default) 
    /// </summary>
    public SolidColorBrush?         Color                       { get; set; }       = LolibarMod.BarContainersColor;
    /// <summary>
    /// Set it to `true`, if you want to make this container have a visible background. (False as default)
    /// </summary>
    public bool                     HasBackground               { get; set; }
    /// <summary>
    /// Adjusts left margin value. (Adds to current left margin this value) (0.0 by default)
    /// </summary>
    public double                   LeftMarginOffset            { get; set; }       = 0.0;
    /// <summary>
    /// Adjusts right margin value. (Adds to current right margin this value) (0.0 by default)
    /// </summary>
    public double                   RightMarginOffset           { get; set; }       = 0.0;

    // Left separator belongs to container
    System.Windows.Shapes.Rectangle SeparatorLeft               { get; set; }       = new();
    // Right separator belongs to container
    System.Windows.Shapes.Rectangle SeparatorRight              { get; set; }       = new();
    // Root node of container
    Border                          BorderContainer             { get; set; }       = new();
    // UI body, which contains all components of the current container.
    StackPanel                      StackPanelContainer         { get; set; }       = new();
    // Text content container.
    TextBlock                       TextBlockContainer          { get; set; }       = new();
    // Svg icon container. Won't be drawn, if typeof(Icon) isn't `Geometry` or `StreamGeometry`
    Path                            PathContainer               { get; set; }       = new();
    // Ico/Jpg/Png... icon container. Won't be drawn, if typeof(Icon) isn't `Image`
    System.Windows.Controls.Image   ImageContainer              { get; set; }       = new();
    /// <summary>
    /// Becomes true, after container has been initialized.
    /// </summary>
    public bool                     IsInitialized               { get; private set; }
    /// <summary>
    /// Position, where container's separator should be drawn. Use `LolibarEnums.SeparatorPosition` Enum to help yourself.
    /// </summary>
    public LolibarEnums.SeparatorPosition? SeparatorPosition    { get; set; }
    /// <summary>
    /// Event invoked on the mouse LEFT key's up.
    /// </summary>
    public Func<MouseButtonEventArgs, int>? MouseLeftButtonUp   { get; set; }
    /// <summary>
    /// Event invoked on the mouse RIGHT key's up.
    /// </summary>
    public Func<MouseButtonEventArgs, int>? MouseRightButtonUp  { get; set; }
    /// <summary>
    /// Event invoked on the mouse MIDDLE key's up.
    /// </summary>
    public Func<MouseButtonEventArgs, int>? MouseMiddleButtonUp { get; set; }
    /// <summary>
    /// Event invoked on the mouse WHEEL state's change (Up or Down spin).
    /// </summary>
    public Func<MouseWheelEventArgs, int>? MouseWheelDelta      { get; set; }

    SolidColorBrush BorderBackground()
    {
        return HasBackground ? LolibarColor.FromHEX($"#30{LolibarHelper.ARGBtoHEX(Color ?? new SolidColorBrush())[3..]}") : LolibarColor.FromHEX("#00000000");
    }

    public StackPanel GetBody()
    {
        return StackPanelContainer;
    }
    /// <summary>
    /// LolibarContainer root node by itself.
    /// </summary>
    public Border GetRoot()
    {
        return BorderContainer;
    }
    /// <summary>
    /// Initializes container content. Don't use it if you want to add your container in lolibar layout. Use `Create()` method instead.
    /// </summary>
    public void Initialize()
    {
        // Skip initialtization if completed before
        if (IsInitialized) return;

        Name = LolibarHelper.GetRandomString(32);

        App.Current.Resources[$"{Name}BorderBackground"] = BorderBackground();
        App.Current.Resources[$"{Name}Color"] = Color;
        App.Current.Resources[$"{Name}Text"] = Text;

        App.Current.Resources[$"{Name}ImageIcon"] = LolibarIcon.ParseICO(string.Empty);
        App.Current.Resources[$"{Name}SvgIcon"] = Geometry.Empty;

        BorderContainer = new()
        {
            Name    = Name,
            Margin  = new Thickness(
                                    LolibarMod.BarContainerMargin.Left + LeftMarginOffset,
                                    LolibarMod.BarContainerMargin.Top,
                                    LolibarMod.BarContainerMargin.Right + RightMarginOffset,
                                    LolibarMod.BarContainerMargin.Bottom
                                ),
            CornerRadius        = LolibarMod.BarContainersCornerRadius,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center
        };

        BorderContainer.SetResourceReference(Border.BackgroundProperty, $"{Name}BorderBackground");

        StackPanelContainer = new()
        {
            Name                = $"{Name}StackPanel",
            Orientation         = System.Windows.Controls.Orientation.Horizontal,
            Margin              = LolibarMod.BarContainerInnerMargin,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center
        };

        BorderContainer.Child = StackPanelContainer;

        // Svg icon container
        PathContainer           = new()
        {
            Width               = LolibarMod.BarIconSize,
            Height              = LolibarMod.BarIconSize,
            MinWidth            = 0,
            MinHeight           = 0,
            Stretch             = Stretch.Uniform,
            Margin              = LolibarMod.BarContainersContentMargin,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center,
        };
        PathContainer.SetResourceReference(Path.DataProperty, $"{Name}SvgIcon");
        PathContainer.SetResourceReference(Path.FillProperty, $"{Name}Color");
        PathContainer.SetResourceReference(Path.RenderTransformProperty, $"{Name}PathRenderTransform");

        StackPanelContainer.Children.Add(PathContainer);

        // ico / png / etc ... icon container
        ImageContainer          = new()
        {
            Stretch             = Stretch.Uniform,
            Width               = LolibarMod.BarIconSize,
            Height              = LolibarMod.BarIconSize,
            MinWidth            = 0,
            MinHeight           = 0,
            Margin              = LolibarMod.BarContainersContentMargin,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center,
        };
        ImageContainer.SetResourceReference(System.Windows.Controls.Image.SourceProperty, $"{Name}ImageIcon");
        ImageContainer.SetResourceReference(Path.RenderTransformProperty, $"{Name}ImageRenderTransform");

        StackPanelContainer.Children.Add(ImageContainer);

        UpdateIconContainersInstance();

        TextBlockContainer = new()
        {
            MinWidth            = 0,
            MinHeight           = 0,
            TextWrapping        = TextWrapping.Wrap,
            Margin              = LolibarMod.BarContainersContentMargin,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center,
            // FontWeight       = FontWeight.FromOpenTypeWeight(600) // it fits bad with mononoki font, but would be cool to add as 'BoldText = true' property ;v;
        };
        TextBlockContainer.SetResourceReference(TextBlock.TextProperty, $"{Name}Text");
        TextBlockContainer.SetResourceReference(TextBlock.ForegroundProperty, $"{Name}Color");

        StackPanelContainer.Children.Add(TextBlockContainer);

        if (Text == null)
        {
            TextBlockContainer.Visibility = Visibility.Collapsed;
        }

        if
        (
            MouseLeftButtonUp       != null ||
            MouseRightButtonUp      != null ||
            MouseMiddleButtonUp     != null ||
            MouseWheelDelta         != null
        )
        {
            BorderContainer.SetContainerEvents
            (
                MouseLeftButtonUp,
                MouseRightButtonUp,
                MouseMiddleButtonUp,
                MouseWheelDelta
            );
        }

        // Say this container is initialized
        IsInitialized = true;
    }
    /// <summary>
    /// Initializes and adds container to the Parent container.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public void Create()
    {
        if (Parent == null) return;

        bool drawLeftSeparator  = SeparatorPosition == LolibarEnums.SeparatorPosition.Left  || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;
        bool drawRightSeparator = SeparatorPosition == LolibarEnums.SeparatorPosition.Right || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;

        SeparatorLeft   = new()
        {
            RadiusX     = LolibarMod.BarSeparatorRadius,
            RadiusY     = LolibarMod.BarSeparatorRadius,
            Width       = LolibarMod.BarSeparatorWidth,
            Height      = LolibarMod.BarSeparatorHeight,
            Opacity     = 0.3
        };
        SeparatorLeft.SetResourceReference(System.Windows.Shapes.Rectangle.FillProperty, $"{Name}Color");

        SeparatorRight  = new()
        {
            RadiusX     = LolibarMod.BarSeparatorRadius,
            RadiusY     = LolibarMod.BarSeparatorRadius,
            Width       = LolibarMod.BarSeparatorWidth,
            Height      = LolibarMod.BarSeparatorHeight,
            Opacity     = 0.3
        };
        SeparatorRight.SetResourceReference(System.Windows.Shapes.Rectangle.FillProperty, $"{Name}Color");

        // Initialize rest of container
        Initialize();

        // Adds an optional left separator
        if (drawLeftSeparator)
        {
            StackPanel _ = new()
            {
                Margin              = LolibarMod.BarContainerMargin,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment   = System.Windows.VerticalAlignment.Center
            };

            _.Children.Add(SeparatorLeft);
            Parent.Children.Add(_);
        }

        // Adds a new child
        Parent.Children.Add(BorderContainer);

        // Adds an optional right separator
        if (drawRightSeparator)
        {
            StackPanel _ = new()
            {
                Margin              = LolibarMod.BarContainerMargin,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment   = System.Windows.VerticalAlignment.Center
            };

            _.Children.Add(SeparatorRight);
            Parent.Children.Add(_);
        }
    }

    public void Update()
    {
        if (!IsInitialized) return;

        if (Text == null)
        {
            TextBlockContainer.Visibility = Visibility.Collapsed;
        }
        else
        {
            TextBlockContainer.Visibility = Visibility.Visible;
        }

        UpdateIconContainersInstance();

        App.Current.Resources[$"{Name}Text"            ]    = Text;
        App.Current.Resources[$"{Name}Color"           ]    = Color;
        App.Current.Resources[$"{Name}BorderBackground"]    = BorderBackground();
        App.Current.Resources[$"{Name}PathRenderTransform"] = new RotateTransform(IconAngle, 0.5 * LolibarMod.BarIconSize, 0.5 * LolibarMod.BarIconSize);
        App.Current.Resources[$"{Name}IcoRenderTransform" ] = new RotateTransform(IconAngle, 0.5 * LolibarMod.BarIconSize, 0.5 * LolibarMod.BarIconSize);
    }
    void UpdateIconContainersInstance()
    {
        if (Icon == null)
        {
            PathContainer .Visibility = Visibility.Collapsed;
            ImageContainer.Visibility = Visibility.Collapsed;
        }
        else
        {
            // Shouldn't be here tbh, BUT somehow i can't remove it.
            // Applications' icons, called by GetAssociatedIcon() is in `Icon` type,
            // so attempt to update them drops this condition:
            if (Icon.GetType() == typeof(Icon))
            {
                App.Current.Resources[$"{Name}ImageIcon"]   = ((Icon)Icon).ToBitmapSource();
                ImageContainer.Visibility                   = Visibility.Visible;
                PathContainer.Visibility                    = Visibility.Collapsed;
            }

            if (Icon.GetType() == typeof(BitmapSource))
            {
                App.Current.Resources[$"{Name}ImageIcon"]   = (BitmapSource)Icon;
                ImageContainer.Visibility                   = Visibility.Visible;
                PathContainer.Visibility                    = Visibility.Collapsed;
            }
            if (Icon.GetType() == typeof(StreamGeometry) || Icon.GetType() == typeof(Geometry))
            {
                App.Current.Resources[$"{Name}SvgIcon"]     = (Geometry)Icon;
                ImageContainer.Visibility                   = Visibility.Collapsed;
                PathContainer.Visibility                    = Visibility.Visible;
            }
        }
    }
}
