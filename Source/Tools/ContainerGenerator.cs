using LolibarApp.Mods;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LolibarApp.Source.Tools
{
    public class ContainerGenerator
    {
        /// <summary>
        /// Generates container in specified parent.
        /// </summary>
        /// <param name="parent">Parent container, where `this` should be placed.</param>
        /// <param name="icon">Icon source. [ `hasIcon` have to be set to true ]</param>
        /// <param name="text">Text source. [ `hasText` have to be set to true ]</param>
        /// <param name="mouseLeftButtonUpEvent">Left button up event source. [ `focusable` have to be set to true ]</param>
        /// <param name="mouseRightButtonUpEvent">Right button up event source. [ `focusable` have to be set to true ]</param>
        public static void CreateContainer(
            StackPanel  parent,
            Geometry?   icon    = null,
            string?     text    = null,
            System.Windows.Input.MouseButtonEventHandler? mouseLeftButtonUpEvent  = null,
            System.Windows.Input.MouseButtonEventHandler? mouseRightButtonUpEvent = null )
        {
            Border border = new Border()
            {
                Margin                  = Config.BarWorkspacesMargin,
                CornerRadius            = Config.BarContainersCornerRadius,
                Background              = Config.BarContainerColor,
                HorizontalAlignment     = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment       = System.Windows.VerticalAlignment.Center
            };

            StackPanel stackPanel = new StackPanel()
            {
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                Margin      = Config.BarContainerInnerMargin,
            };

            border.Child    = stackPanel;

            if (icon != null)
            {
                Path iconItem = new Path()
                {
                    Data    = icon,
                    Stretch = Stretch.Uniform,
                    Margin  = Config.BarContainersContentMargin,
                    Fill    = Config.BarContainersContentColor
                };

                stackPanel.Children.Add(iconItem);
            }

            if (text != null)
            {
                TextBlock textItem = new TextBlock()
                {
                    Text        = text,
                    Margin      = Config.BarContainersContentMargin,
                    Foreground  = Config.BarContainersContentColor,
                };

                stackPanel.Children.Add(textItem);
            }
            

            if (mouseLeftButtonUpEvent != null || mouseRightButtonUpEvent != null)
            {
                border.SetContainerEvents(
                    LolibarEvents.UI_MouseEnter,
                    LolibarEvents.UI_MouseLeave,
                    mouseLeftButtonUpEvent,
                    mouseRightButtonUpEvent
                );
            }

            if (parent == Lolibar.barRightContainer)
            {
                // Save current children into array
                UIElement[] childrenArray = new UIElement[parent.Children.Count];
                parent.Children.CopyTo(childrenArray, 0);

                // Remove all children
                parent.Children.RemoveRange(0, parent.Children.Count);

                // Add a new child
                parent.Children.Add(border);

                // Restore removed children to keep a new element
                // at the start of the container!
                foreach(UIElement child in childrenArray)
                {
                    parent.Children.Add(child);
                }
            }
            else
            {
                // Add a new child
                parent.Children.Add(border);
            }
        }
    }
}
