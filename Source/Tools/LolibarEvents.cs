using System.Windows;
using System.Windows.Input;
using Windows.Media.Control;

namespace LolibarApp.Source.Tools;

public class LolibarEvents
{
    public static void UI_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        // --- changes cursor type ---
        sender.GetType().GetProperty("Cursor")?.SetValue(sender, System.Windows.Input.Cursors.Hand);

        LolibarAnimator.BeginDecOpacityAnimation((UIElement)sender);
    }
    public static void UI_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        LolibarAnimator.BeginIncOpacityAnimation((UIElement)sender);
    }

    public static void SwapWorkspacesByWheelEvent(object sender, MouseWheelEventArgs e)
    {
        if (e.Delta > 0)
        {
            LolibarVirtualDesktop.GoToDesktopLeft();
        }

        if (e.Delta < 0)
        {
            LolibarVirtualDesktop.GoToDesktopRight();
        }
    }
}
