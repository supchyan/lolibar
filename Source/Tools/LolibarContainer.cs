using LolibarApp.Modding;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace LolibarApp.Source.Tools;

/// <summary>
/// Class, which provides capabilities to create UI containers.
/// </summary>
public class LolibarContainer
{
    public string Name                  { get; set; }
    public StackPanel? Parent           { get; set; }
    public Geometry? Icon               { get; set; }
    public string? Text                 { get; set; }
    /// <summary>
    /// Set it to `true`, if you want to make this container handle Virtual Desktop Events. (False as default)
    /// </summary>
    public bool UseWorkspaceSwapEvents  { get; set; }
    /// <summary>
    /// Becomes true, after container has been created and placed into the parent.
    /// </summary>
    public bool IsCreated               { get; private set; }
    /// <summary>
    /// Set it to `true`, if you want to make this container have a visible background. (False as default)
    /// </summary>
    public bool HasBackground           { get; set; }
    /// <summary>
    /// Can be used as reference to the `Border` component inside a container.
    /// You can use it to refer to a parent and create other containers inside it. || Example: var parent = (StackPanel)Border.Child;
    /// </summary>
    public Border BorderComponent       { get; private set; }
    public LolibarEnums.SeparatorPosition? SeparatorPosition                        { get; set; }
    public System.Windows.Input.MouseButtonEventHandler? MouseLeftButtonUpEvent     { get; set; }
    public System.Windows.Input.MouseButtonEventHandler? MouseRightButtonUpEvent    { get; set; }

    

    public void Create()
    {
        if (Name   == null || Name == string.Empty) throw new ArgumentNullException("name");
        if (Parent == null) return;

        bool drawLeftSeparator  = SeparatorPosition == LolibarEnums.SeparatorPosition.Left  || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;
        bool drawRightSeparator = SeparatorPosition == LolibarEnums.SeparatorPosition.Right || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;

        System.Windows.Shapes.Rectangle separatorLeft = new()
        {
            RadiusX = ModClass.BarSeparatorRadius,
            RadiusY = ModClass.BarSeparatorRadius,
            Width   = ModClass.BarSeparatorWidth,
            Height  = ModClass.BarSeparatorHeight,
            Fill    = ModClass.BarContainersContentColor,
            Opacity = 0.3
        };
        System.Windows.Shapes.Rectangle separatorRight = new()
        {
            RadiusX = ModClass.BarSeparatorRadius,
            RadiusY = ModClass.BarSeparatorRadius,
            Width   = ModClass.BarSeparatorWidth,
            Height  = ModClass.BarSeparatorHeight,
            Fill    = ModClass.BarContainersContentColor,
            Opacity = 0.3
        };

        Border border = new()
        {
            Name                = Name,
            Margin              = ModClass.BarContainerMargin,
            CornerRadius        = ModClass.BarContainersCornerRadius,
            Background          = HasBackground ? LolibarHelper.SetColor($"#30{LolibarHelper.ARGBtoHEX(ModClass.BarContainersContentColor)[3..]}") : LolibarHelper.SetColor("#00000000"),
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center
        };

        BorderComponent = border;

        StackPanel stackPanel = new()
        {
            Name                = $"{Name}StackPanel",
            Orientation         = System.Windows.Controls.Orientation.Horizontal,
            Margin              = ModClass.BarContainerInnerMargin,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center
        };

        border.Child = stackPanel;

        if (Icon != null)
        {
            App.Current.Resources[$"{Name}Icon"] = Icon;
            Path iconItem = new()
            {
                Stretch             = Stretch.Uniform,
                Margin              = ModClass.BarContainersContentMargin,
                Fill                = ModClass.BarContainersContentColor,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment   = System.Windows.VerticalAlignment.Center
            };
            iconItem.SetResourceReference(Path.DataProperty, $"{Name}Icon");

            stackPanel.Children.Add(iconItem);
        }

        if (Text != null)
        {
            App.Current.Resources[$"{Name}Text"] = Text;
            TextBlock textItem = new()
            {
                Margin              = ModClass.BarContainersContentMargin,
                Foreground          = ModClass.BarContainersContentColor,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment   = System.Windows.VerticalAlignment.Center
            };
            textItem.SetResourceReference(TextBlock.TextProperty, $"{Name}Text");

            stackPanel.Children.Add(textItem);
        }


        if (MouseLeftButtonUpEvent != null || MouseRightButtonUpEvent != null)
        {
            border.SetContainerEvents(
                MouseLeftButtonUpEvent,
                MouseRightButtonUpEvent
            );
        }
        
        // Applies workspace scrolling feature to specified container
        if (UseWorkspaceSwapEvents)
        {
            border.PreviewMouseWheel += LolibarEvents.SwapWorkspacesEvent;
        }

        if (Parent == Lolibar.BarRightContainer)
        {
            // Saves current children into array
            UIElement[] childrenArray = new UIElement[Parent.Children.Count];
            Parent.Children.CopyTo(childrenArray, 0);

            // Removes all children
            Parent.Children.RemoveRange(0, Parent.Children.Count);

            // Adds an optional separator
            if (drawLeftSeparator)
            {
                Parent.Children.Add(separatorLeft);
            }

            // Adds a new child
            Parent.Children.Add(border);

            // Adds an optional separator
            if (drawRightSeparator)
            {
                Parent.Children.Add(separatorRight);
            }

            // Restores removed children to keep a new element
            // at the start of the container!
            foreach (UIElement child in childrenArray)
            {
                Parent.Children.Add(child);
            }
        }
        else
        {
            if (drawLeftSeparator)
            {
                Parent.Children.Add(separatorLeft);
            }

            // Adds a new child
            Parent.Children.Add(border);

            // Adds an optional separator
            if (drawRightSeparator)
            {
                Parent.Children.Add(separatorRight);
            }
        }

        IsCreated = true;
    }
    public void Update()
    {
        if (!IsCreated) return;

        App.Current.Resources[$"{Name}Text"] = Text;
        App.Current.Resources[$"{Name}Icon"] = Icon;
    }
}
