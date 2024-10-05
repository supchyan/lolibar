﻿using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace LolibarApp.Source.Tools;

/// <summary>
/// Class, which provides capabilities to create UI containers.
/// </summary>
public class LolibarContainer
{
    public string       Name            { get; set; }
    public StackPanel?  Parent          { get; set; }
    public Geometry?    Icon            { get; set; }
    public string?      Text            { get; set; }
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
    public System.Windows.Input.MouseWheelEventHandler? MouseWheelEvent             { get; set; }

    SolidColorBrush BorderBackground()
    {
        return HasBackground ? LolibarHelper.SetColor($"#30{LolibarHelper.ARGBtoHEX(LolibarMod.BarContainersContentColor)[3..]}") : LolibarHelper.SetColor("#00000000");
    }

    public void Create()
    {
        if (Name   == null || Name == string.Empty) throw new ArgumentNullException("name");
        if (Parent == null) return;

        bool drawLeftSeparator  = SeparatorPosition == LolibarEnums.SeparatorPosition.Left  || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;
        bool drawRightSeparator = SeparatorPosition == LolibarEnums.SeparatorPosition.Right || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;

        System.Windows.Shapes.Rectangle separatorLeft = new()
        {
            RadiusX = LolibarMod.BarSeparatorRadius,
            RadiusY = LolibarMod.BarSeparatorRadius,
            Width   = LolibarMod.BarSeparatorWidth,
            Height  = LolibarMod.BarSeparatorHeight,
            Fill    = LolibarMod.BarContainersContentColor,
            Opacity = 0.3
        };
        System.Windows.Shapes.Rectangle separatorRight = new()
        {
            RadiusX = LolibarMod.BarSeparatorRadius,
            RadiusY = LolibarMod.BarSeparatorRadius,
            Width   = LolibarMod.BarSeparatorWidth,
            Height  = LolibarMod.BarSeparatorHeight,
            Fill    = LolibarMod.BarContainersContentColor,
            Opacity = 0.3
        };

        App.Current.Resources[$"{Name}BorderBackground"] = BorderBackground();
        Border border = new()
        {
            Name                = Name,
            Margin              = LolibarMod.BarContainerMargin,
            CornerRadius        = LolibarMod.BarContainersCornerRadius,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = System.Windows.VerticalAlignment.Center
        };
        border.SetResourceReference(Border.BackgroundProperty, $"{Name}BorderBackground");

        BorderComponent = border;

        StackPanel stackPanel = new()
        {
            Name                = $"{Name}StackPanel",
            Orientation         = System.Windows.Controls.Orientation.Horizontal,
            Margin              = LolibarMod.BarContainerInnerMargin,
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
                Margin              = LolibarMod.BarContainersContentMargin,
                Fill                = LolibarMod.BarContainersContentColor,
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
                Margin              = LolibarMod.BarContainersContentMargin,
                Foreground          = LolibarMod.BarContainersContentColor,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment   = System.Windows.VerticalAlignment.Center
            };
            textItem.SetResourceReference(TextBlock.TextProperty, $"{Name}Text");

            stackPanel.Children.Add(textItem);
        }


        if (MouseLeftButtonUpEvent != null || MouseRightButtonUpEvent != null || MouseWheelEvent != null)
        {
            border.SetContainerEvents(
                MouseLeftButtonUpEvent,
                MouseRightButtonUpEvent,
                MouseWheelEvent
            );
        }

        Thickness separatorMargin = LolibarMod.BarContainerMargin;

        // Adds an optional left separator
        if (drawLeftSeparator)
        {
            StackPanel TmpStackPanel = new()
            {
                Margin = separatorMargin,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            TmpStackPanel.Children.Add(separatorLeft);
            Parent.Children.Add(TmpStackPanel);
        }

        // Adds a new child
        Parent.Children.Add(border);

        // Adds an optional right separator
        if (drawRightSeparator)
        {
            StackPanel TmpStackPanel = new()
            {
                Margin = separatorMargin,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            TmpStackPanel.Children.Add(separatorRight);
            Parent.Children.Add(TmpStackPanel);
        }

        IsCreated = true;
    }

    public void Update()
    {
        if (!IsCreated) return;

        App.Current.Resources[$"{Name}Text"] = Text;
        App.Current.Resources[$"{Name}Icon"] = Icon;
        App.Current.Resources[$"{Name}BorderBackground"] = BorderBackground();
    }
}
