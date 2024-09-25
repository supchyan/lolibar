using LolibarApp.Mods;
using System.Windows;
using System.Windows.Controls;

namespace LolibarApp.Source.Tools;

public class LolibarVirtualDesktop
{
    int  oldDesktopCount    = 0;
    int  oldDesktopIndex    = 0;
    bool errorTabGenerated  = false;

    public void WorkspaceTabsListener(Border parent)
    {
        var spawnContainer = (StackPanel)parent.Child;
        // Stop doing all logic below, if error tab has been already generated.
        if (errorTabGenerated) return;

        /* 
        What happens below?

        Windows still has no one way to handle Virtual Desktops,
        so someone made tools for differents Win32 builds to handle this stuff properly.
        I've implemented it as different `usings`, so the code below checks
        if any method is avaliable to work with and then tries to follow it's `using` as well.
        Hope I've explained everything good to you, thanks.
        If you know better way to handle this part of the Win32, feel free to notify me
        on my Discord server ;)
        
        (supchyan)
        */
        if (IsVirtualDesktopValid())
        {
            if (!ShouldUpdateWorkspaces(VirtualDesktop.Desktop.Count, VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current))) return;
            
            UpdateWorkspaceTabs(
                spawnContainer,
                VirtualDesktop.Desktop.Count,
                VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current)
            );
        }
        else if (IsVirtualDesktop11Valid())
        {
            if (!ShouldUpdateWorkspaces(VirtualDesktop11.Desktop.Count, VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current))) return;

            UpdateWorkspaceTabs(
                spawnContainer,
                VirtualDesktop11.Desktop.Count,
                VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current)
            );

        }
        else if (IsVirtualDesktop11_24H2Valid())
        {
            if (!ShouldUpdateWorkspaces(VirtualDesktop11_24H2.Desktop.Count, VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current))) return;

            UpdateWorkspaceTabs(
                spawnContainer,
                VirtualDesktop11_24H2.Desktop.Count,
                VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current)
            );
        }
        else
        {
            CreateErrorWorkspaceTab(spawnContainer);
        }
    }
    // Creates error tab, if no valid method to draw workspaces...
    void CreateErrorWorkspaceTab(StackPanel parent)
    {
        parent.Children.RemoveRange(0, parent.Children.Count);

        Border border = new Border()
        {
            Margin = new Thickness(5, 5, 5, 5),
            CornerRadius = Config.BarContainersCornerRadius,
            Background = Config.BarContainersContentColor
        };
        TextBlock tabBlock = new TextBlock()
        {
            Text = $"unsupported",
            Margin = Config.BarContainerInnerMargin,
            Foreground = Config.BarColor,
        };
        border.Child = tabBlock;
        parent.Children.Add(border);

        // To prevent regenerating
        errorTabGenerated = true;
    }
    // Recreates workspaces tabs, which is related on Windows Virtual Desktops
    void UpdateWorkspaceTabs(StackPanel parent, int desktopCount, int currentDesktopIndex)
    {
        // Remove all children
        parent.Children.RemoveRange(0, parent.Children.Count);

        // And create new children, lol
        var background = System.Windows.Media.Brushes.Transparent;
        var foreground = Config.BarContainersContentColor;

        for (int i = 0; i < desktopCount; i++)
        {
            int index = i; // Just put it here like that
            
            if (i != currentDesktopIndex)
            {
                background = System.Windows.Media.Brushes.Transparent;
            }
            else
            {
                var hex         = LolibarHelper.ARGBtoHEX(foreground).Substring(3);
                background      = LolibarHelper.SetColor($"#30{hex}");
            }

            Border border       = new Border()
            {
                Margin          = Config.BarWorkspacesMargin,
                CornerRadius    = Config.BarContainersCornerRadius,
                Background      = background
            };
            TextBlock tabBlock  = new TextBlock()
            {
                Text            = $"{index + 1}",
                Margin          = Config.BarContainerInnerMargin,
                Foreground      = foreground,
            };
            border.Child = tabBlock;
            border.SetContainerEvents(
                LolibarEvents.UI_MouseEnter,
                LolibarEvents.UI_MouseLeave,
                new System.Windows.Input.MouseButtonEventHandler((object sender, System.Windows.Input.MouseButtonEventArgs e) => {
                    MoveToDesktop(index);
                }),
                new System.Windows.Input.MouseButtonEventHandler((object sender, System.Windows.Input.MouseButtonEventArgs e) => {
                    RemoveDesktop(index);
                })
            );

            // if user clicked to already selected workspace -> create a new one
            if (i == currentDesktopIndex)
            {
                border.PreviewMouseLeftButtonUp += 
                    new System.Windows.Input.MouseButtonEventHandler((object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                    {
                        CreateDesktop();
                    });
            }

            parent.Children.Add(border);
        }
    }

    static bool IsVirtualDesktopValid()
    {
        try
        {
            int testInt = VirtualDesktop.Desktop.Count;
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
    static bool IsVirtualDesktop11Valid()
    {
        try
        {
            int testInt = VirtualDesktop11.Desktop.Count;
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
    static bool IsVirtualDesktop11_24H2Valid()
    {
        try
        {
            int testInt = VirtualDesktop11.Desktop.Count;
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    bool ShouldUpdateWorkspaces(int currentDesktopCount, int currentDesktopIndex) {
        if (oldDesktopCount != currentDesktopCount)
        {
            oldDesktopCount = currentDesktopCount;
            return true;
        }
        if (oldDesktopIndex != currentDesktopIndex)
        {
            oldDesktopIndex = currentDesktopIndex;
            return true;
        }
        return false;
    }
    static void CreateDesktop()
    {
        if (IsVirtualDesktopValid())
        {
            VirtualDesktop.Desktop.Create();
            MoveToDesktop(VirtualDesktop.Desktop.Count - 1);
        }
        else if (IsVirtualDesktop11Valid())
        {
            VirtualDesktop11.Desktop.Create();
            MoveToDesktop(VirtualDesktop11.Desktop.Count - 1);
        }
        else if (IsVirtualDesktop11_24H2Valid())
        {
            VirtualDesktop11_24H2.Desktop.Create();
            MoveToDesktop(VirtualDesktop11_24H2.Desktop.Count - 1);
        }
    }
    static void MoveToDesktop(int index)
    {
        if (IsVirtualDesktopValid())
        {
            VirtualDesktop.Desktop.FromIndex(index).MakeVisible();
        }
        else if (IsVirtualDesktop11Valid())
        {
            VirtualDesktop11.Desktop.FromIndex(index).MakeVisible();
        }
        else if (IsVirtualDesktop11_24H2Valid())
        {
            VirtualDesktop11_24H2.Desktop.FromIndex(index).MakeVisible();
        }
    }
    void RemoveDesktop(int index)
    {
        // Prevent user from removing last standing workspace
        if (oldDesktopCount == 1) return;

        if (IsVirtualDesktopValid())
        {
            // return if this UI hasn't updated yet.
            if (index >= VirtualDesktop.Desktop.Count) return;
            VirtualDesktop.Desktop.FromIndex(index).Remove();
        }
        else if (IsVirtualDesktop11Valid())
        {
            // return if this UI hasn't updated yet.
            if (index >= VirtualDesktop11.Desktop.Count) return;
            VirtualDesktop11.Desktop.FromIndex(index).Remove();
        }
        else if (IsVirtualDesktop11_24H2Valid())
        {
            // return if this UI hasn't updated yet.
            if (index >= VirtualDesktop11_24H2.Desktop.Count) return;
            VirtualDesktop11_24H2.Desktop.FromIndex(index).Remove();
        }
    }
}
