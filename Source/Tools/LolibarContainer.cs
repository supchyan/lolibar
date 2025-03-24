
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
    public string?                  Name                        { get; set; }
    /// <summary>
    /// Parent, where this container should be drawn inside.
    /// </summary>
    public StackPanel?              Parent                      { get; set; }
    /// <summary>
    /// Container's icon content, which will be drawn inside.
    /// </summary>
    public object?                  Icon                        { get; set; }
    /// <summary>
    /// Reference to the original `Text` property and cannot be reassiged.
    /// </summary>
    public string?                  RefText                     { get; private set; }
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

    // Left separator belongs to container
    System.Windows.Shapes.Rectangle SeparatorLeft               { get; set; }       = new();
    // Right separator belongs to container
    System.Windows.Shapes.Rectangle SeparatorRight              { get; set; }       = new();

    // Main container, contains whole UI layout
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
    /// Becomes true, after container has been created and placed into the parent.
    /// </summary>
    public bool                     IsCreated                   { get; private set; }
    /// <summary>
    /// Position, where container's separator should be drawn. Use `LolibarEnums.SeparatorPosition` Enum to help yourself.
    /// </summary>
    public LolibarEnums.SeparatorPosition? SeparatorPosition    { get; set; }
    /// <summary>
    /// Event invoked on the mouse LEFT key's up.
    /// </summary>
    public MouseButtonEventHandler? MouseLeftButtonUpEvent      { get; set; }
    /// <summary>
    /// Event invoked on the mouse RIGHT key's up.
    /// </summary>
    public MouseButtonEventHandler? MouseRightButtonUpEvent     { get; set; }
    /// <summary>
    /// Event invoked on the mouse WHEEL state's change (Up or Down spin).
    /// </summary>
    public MouseWheelEventHandler?  MouseWheelEvent             { get; set; }
    /// <summary>
    /// Event invoked on the mouse RIGHT key's up.
    /// </summary>
    public Func<int>?               MouseMiddleButtonUpFunc     { get; set; }

    SolidColorBrush BorderBackground()
    {
        return HasBackground ? LolibarHelper.SetColor($"#30{LolibarHelper.ARGBtoHEX(Color ?? new SolidColorBrush())[3..]}") : LolibarHelper.SetColor("#00000000");
    }

    public StackPanel GetBody()
    {
        return StackPanelContainer;
    }

    public void Create()
    {
        if (Name   == null || Name == string.Empty) throw new ArgumentNullException("name");
        if (Parent == null) return;

        bool drawLeftSeparator  = SeparatorPosition == LolibarEnums.SeparatorPosition.Left  || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;
        bool drawRightSeparator = SeparatorPosition == LolibarEnums.SeparatorPosition.Right || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;
        
        App.Current.Resources[$"{Name}BorderBackground"]    = BorderBackground();
        App.Current.Resources[$"{Name}Color"]               = Color;
        App.Current.Resources[$"{Name}Text"]                = Text;

        App.Current.Resources[$"{Name}ImageIcon"]           = LolibarIcon.ParseICO(string.Empty);
        App.Current.Resources[$"{Name}SvgIcon"]             = Geometry.Empty;

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

        BorderContainer         = new()
        {
            Name                = Name,
            Margin              = LolibarMod.BarContainerMargin,
            CornerRadius        = LolibarMod.BarContainersCornerRadius,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center
        };
        BorderContainer.SetResourceReference(Border.BackgroundProperty, $"{Name}BorderBackground");

        StackPanelContainer     = new()
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
            Stretch             = Stretch.Uniform,
            Margin              = LolibarMod.BarContainersContentMargin,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center
        };
        PathContainer.SetResourceReference(Path.DataProperty, $"{Name}SvgIcon");
        PathContainer.SetResourceReference(Path.FillProperty, $"{Name}Color");

        StackPanelContainer.Children.Add(PathContainer);

        // ico / png / etc ... icon container
        ImageContainer          = new()
        {
            Stretch             = Stretch.Uniform,
            Width               = 14,
            Height              = 14,
            Margin              = LolibarMod.BarContainersContentMargin,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center
        };
        ImageContainer.SetResourceReference(System.Windows.Controls.Image.SourceProperty, $"{Name}ImageIcon");

        StackPanelContainer.Children.Add(ImageContainer);

        UpdateIconContainersInstance();

        // Assiging the reference for the text value, which is won't be reassigned in future.
        RefText = Text;

        TextBlockContainer      = new()
        {
            Margin              = LolibarMod.BarContainersContentMargin,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center
        };
        TextBlockContainer.SetResourceReference(TextBlock.TextProperty,         $"{Name}Text" );
        TextBlockContainer.SetResourceReference(TextBlock.ForegroundProperty,   $"{Name}Color");

        StackPanelContainer.Children.Add(TextBlockContainer);

        if (Text == null)
        {
            TextBlockContainer.Visibility = Visibility.Collapsed;
        }

        if 
        (
            MouseLeftButtonUpEvent      != null ||
            MouseRightButtonUpEvent     != null ||
            MouseWheelEvent             != null ||
            MouseMiddleButtonUpFunc     != null
        )
        {
            BorderContainer.SetContainerEvents
            (
                MouseLeftButtonUpEvent,
                MouseRightButtonUpEvent,
                MouseWheelEvent,
                MouseMiddleButtonUpFunc
            );
        }

        Thickness separatorMargin = LolibarMod.BarContainerMargin;

        // Adds an optional left separator
        if (drawLeftSeparator)
        {
            StackPanel TmpStackPanel = new()
            {
                Margin              = separatorMargin,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment   = System.Windows.VerticalAlignment.Center
            };

            TmpStackPanel.Children.Add(SeparatorLeft);
            Parent.Children.Add(TmpStackPanel);
        }

        // Adds a new child
        Parent.Children.Add(BorderContainer);

        // Adds an optional right separator
        if (drawRightSeparator)
        {
            StackPanel TmpStackPanel = new()
            {
                Margin              = separatorMargin,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment   = System.Windows.VerticalAlignment.Center
            };

            TmpStackPanel.Children.Add(SeparatorRight);
            Parent.Children.Add(TmpStackPanel);
        }

        IsCreated = true;
    }

    public void Update()
    {
        if (!IsCreated) return;

        if (Text == null)
        {
            TextBlockContainer.Visibility = Visibility.Collapsed;
        }
        else
        {
            TextBlockContainer.Visibility = Visibility.Visible;
        }

        UpdateIconContainersInstance();

        App.Current.Resources[$"{Name}Text"             ] = Text;
        App.Current.Resources[$"{Name}Color"            ] = Color;
        App.Current.Resources[$"{Name}BorderBackground" ] = BorderBackground();
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
