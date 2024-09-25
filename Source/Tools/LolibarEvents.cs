using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

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

    public static void OpenUserSettingsEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = "Start-Process ms-settings:accounts",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        }.Start();
    }
    public static void OpenTaskManagerEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = "Start-Process taskmgr.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        }.Start();
    }
    public static void OpenPowerSettingsEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = "Start-Process ms-settings:batterysaver",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        }.Start();
    }
    public static void OpenTimeSettingsEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        // probable fix #9, so shouldn't be overlooked. seems, `sender` here is application itself...
        //if (e.OriginalSource == sender) System.Windows.MessageBox.Show("show");
        new Process
        {
            StartInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = "Start-Process ms-settings:dateandtime",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        }.Start();
    }

    public static void SwapRamInfoEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LolibarDefaults.ChangeRamInfo();
    }
    public static void SwapDiskInfoEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LolibarDefaults.ChangeDiskInfo();
    }
    public static void SwapNetworkInfoEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LolibarDefaults.ChangeNetworkInfo();
    }

    public static void SwapWorkspacesEvent(object sender, MouseWheelEventArgs e)
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
