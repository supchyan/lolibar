using LolibarApp.Mods;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace LolibarApp.Source.Tools;

public class LolibarContainer
{
    public string Name                  { get; set; }
    public StackPanel? Parent           { get; set; }
    public Geometry? Icon               { get; set; }
    public string? Text                 { get; set; }
    public bool UseWorkspaceSwapEvents  { get; set; }
    public bool IsCreated               { get; private set; }
    public LolibarEnums.SeparatorPosition? SeparatorPosition                        { get; set; }
    public System.Windows.Input.MouseButtonEventHandler? MouseLeftButtonUpEvent     { get; set; }
    public System.Windows.Input.MouseButtonEventHandler? MouseRightButtonUpEvent    { get; set; }

    // Rarely used
    public Border Border { get; private set; }

    public void Create()
    {
        if (Name   == null || Name == string.Empty) throw new ArgumentNullException("name");
        if (Parent == null) return;

        bool drawLeftSeparator  = SeparatorPosition == LolibarEnums.SeparatorPosition.Left || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;
        bool drawRightSeparator = SeparatorPosition == LolibarEnums.SeparatorPosition.Right || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;

        System.Windows.Shapes.Rectangle separatorLeft = new()
        {
            RadiusX = Config.BarSeparatorRadius,
            RadiusY = Config.BarSeparatorRadius,
            Width   = Config.BarSeparatorWidth,
            Height  = Config.BarSeparatorHeight,
            Fill    = Config.BarContainersContentColor,
            Opacity = 0.3
        };
        System.Windows.Shapes.Rectangle separatorRight = new()
        {
            RadiusX = Config.BarSeparatorRadius,
            RadiusY = Config.BarSeparatorRadius,
            Width   = Config.BarSeparatorWidth,
            Height  = Config.BarSeparatorHeight,
            Fill    = Config.BarContainersContentColor,
            Opacity = 0.3
        };

        Border border = new()
        {
            Name                = Name,
            Margin              = Config.BarContainerMargin,
            CornerRadius        = Config.BarContainersCornerRadius,
            Background          = Config.BarContainerColor,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center
        };

        Border = border;

        StackPanel stackPanel = new()
        {
            Name                = $"{Name}StackPanel",
            Orientation         = System.Windows.Controls.Orientation.Horizontal,
            Margin              = Config.BarContainerInnerMargin,
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
                Margin              = Config.BarContainersContentMargin,
                Fill                = Config.BarContainersContentColor,
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
                Margin              = Config.BarContainersContentMargin,
                Foreground          = Config.BarContainersContentColor,
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
