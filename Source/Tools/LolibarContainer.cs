using LolibarApp.Mods;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace LolibarApp.Source.Tools
{
    public class LolibarContainer
    {
        public string Name { get; set; }
        public StackPanel? Parent { get; set; }
        public Geometry? Icon { get; set; }
        public string? Text { get; set; }
        public LolibarEnums.SeparatorPosition? SeparatorPosition { get; set; }
        public System.Windows.Input.MouseButtonEventHandler? MouseLeftButtonUpEvent { get; set; }
        public System.Windows.Input.MouseButtonEventHandler? MouseRightButtonUpEvent { get; set; }
        public Border? Border { get; private set; }

        public void Create()
        {
            if (Parent == null) return;

            bool drawLeftSeparator  = SeparatorPosition == LolibarEnums.SeparatorPosition.Left || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;
            bool drawRightSeparator = SeparatorPosition == LolibarEnums.SeparatorPosition.Right || SeparatorPosition == LolibarEnums.SeparatorPosition.Both;

            System.Windows.Shapes.Rectangle separatorLeft = new()
            {
                RadiusX = Config.BarSeparatorRadius,
                RadiusY = Config.BarSeparatorRadius,
                Width = Config.BarSeparatorWidth,
                Height = Config.BarSeparatorHeight,
                Fill = Config.BarContainersContentColor,
                Opacity = 0.3
            };
            System.Windows.Shapes.Rectangle separatorRight = new()
            {
                RadiusX = Config.BarSeparatorRadius,
                RadiusY = Config.BarSeparatorRadius,
                Width = Config.BarSeparatorWidth,
                Height = Config.BarSeparatorHeight,
                Fill = Config.BarContainersContentColor,
                Opacity = 0.3
            };

            Border border = new Border()
            {
                Name = Name,
                Margin = Config.BarContainerMargin,
                CornerRadius = Config.BarContainersCornerRadius,
                Background = Config.BarContainerColor,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            Border = border;

            StackPanel stackPanel = new StackPanel()
            {
                Name = $"{Name}StackPanel",
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                Margin = Config.BarContainerInnerMargin,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            border.Child = stackPanel;

            if (Icon != null)
            {
                App.Current.Resources[$"{Name}Icon"] = Icon;
                Path iconItem = new Path()
                {
                    Stretch = Stretch.Uniform,
                    Margin = Config.BarContainersContentMargin,
                    Fill = Config.BarContainersContentColor,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center
                };
                iconItem.SetResourceReference(Path.DataProperty, $"{Name}Icon");

                stackPanel.Children.Add(iconItem);
            }

            if (Text != null)
            {
                App.Current.Resources[$"{Name}Text"] = Text;
                TextBlock textItem = new TextBlock()
                {
                    Margin = Config.BarContainersContentMargin,
                    Foreground = Config.BarContainersContentColor,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center
                };
                textItem.SetResourceReference(TextBlock.TextProperty, $"{Name}Text");

                stackPanel.Children.Add(textItem);
            }


            if (MouseLeftButtonUpEvent != null || MouseRightButtonUpEvent != null)
            {
                border.SetContainerEvents(
                    LolibarEvents.UI_MouseEnter,
                    LolibarEvents.UI_MouseLeave,
                    MouseLeftButtonUpEvent,
                    MouseRightButtonUpEvent
                );
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
        }
        public void Update(string? newText = null, Geometry? newIcon = null)
        {
            App.Current.Resources[$"{Name}Text"] = newText;
            App.Current.Resources[$"{Name}Icon"] = newIcon;
        }
    }
}
