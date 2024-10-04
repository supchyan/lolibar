using System.Windows;
using System.Windows.Controls;
using static LolibarApp.Source.Tools.LolibarEnums;

namespace LolibarApp.Source.Tools;

public class LolibarVirtualDesktop
{
    static Border?     InitializedBorder    {  get; set; }
    public static int  OldDesktopCount      { get; private set; }
    public static int  OldDesktopIndex      { get; private set; }
    public static bool IsErrorTabGenerated  { get; private set; }

    static WinVer WindowsVersion { get; set; } = WinVer.Unknown;

    static void GetWindowsVersion()
    {
        try
        {
            int testInt = VirtualDesktop.Desktop.Count;
            WindowsVersion = WinVer.Win10;
        }
        catch
        {
            try
            {
                int testInt = VirtualDesktop11.Desktop.Count;
                WindowsVersion = WinVer.Win11;
            }
            catch
            {
                try
                {
                    int testInt = VirtualDesktop11_24H2.Desktop.Count;
                    WindowsVersion = WinVer.Win11_24H2;
                }
                catch
                {
                    WindowsVersion = WinVer.Unsupported;
                }
            }
        }
    }
    public static void InvokeWorkspaceTabsUpdate(Border border)
    {
        // Stop doing all logic below, if `error_tab` has been generated.
        if (IsErrorTabGenerated) return;

        if (InitializedBorder == null) InitializedBorder = border;

        var parent = (StackPanel)border.Child;

        /* 
        What's happening below?

        Windows OS still has no one way to handle Virtual Desktops,
        so https://github.com/MScholtes/VirtualDesktop made tools
        for different Win32 builds to handle this stuff properly.
        I've implemented it as different `namespaces`, so that code below
        tries to adapt them to current Windows OS patch.

        Contact me on my Discord server, if you know better way, thanks.
        
        (supchyan)
        */

        switch (WindowsVersion)
        {
            case WinVer.Unknown:
                GetWindowsVersion();
                InvokeWorkspaceTabsUpdate(InitializedBorder);
            break;

            case WinVer.Win10:
                if (!ShouldUpdateWorkspaces(VirtualDesktop.Desktop.Count, VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current))) return;

                UpdateWorkspaceTabs(
                    parent,
                    VirtualDesktop.Desktop.Count,
                    VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current)
                );
            break;

            case WinVer.Win11:
                if (!ShouldUpdateWorkspaces(VirtualDesktop11.Desktop.Count, VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current))) return;

                UpdateWorkspaceTabs(
                    parent,
                    VirtualDesktop11.Desktop.Count,
                    VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current)
                );
            break;

            case WinVer.Win11_24H2:
                if (!ShouldUpdateWorkspaces(VirtualDesktop11_24H2.Desktop.Count, VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current))) return;

                UpdateWorkspaceTabs(
                    parent,
                    VirtualDesktop11_24H2.Desktop.Count,
                    VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current)
                );
            break;

            case WinVer.Unsupported:
                CreateErrorWorkspaceTab(parent);
            break;
        }
    }
    // Creates error tab, if no valid method to draw workspaces...
    static void CreateErrorWorkspaceTab(StackPanel parent)
    {
        parent.Children.RemoveRange(0, parent.Children.Count);

        Border border = new()
        {
            Margin = new Thickness(5, 5, 5, 5),
            CornerRadius = LolibarMod.BarContainersCornerRadius,
            Background = LolibarMod.BarContainersContentColor
        };
        TextBlock tabBlock = new()
        {
            Text = $"unsupported",
            Margin = LolibarMod.BarContainerInnerMargin,
            Foreground = LolibarMod.BarColor,
        };
        border.Child = tabBlock;
        parent.Children.Add(border);

        // To prevent reapeats
        IsErrorTabGenerated = true;
    }
    // Recreates workspaces tabs, which is related on Windows Virtual Desktops
    static void UpdateWorkspaceTabs(StackPanel parent, int desktopCount, int currentDesktopIndex)
    {
        // Remove all children
        parent.Children.RemoveRange(0, parent.Children.Count);

        // And create new children, lol
        var hasBackground = false;

        for (int i = 0; i < desktopCount; i++)
        {
            int index = i; // Just put it here like that

            hasBackground = i == currentDesktopIndex;

            LolibarContainer tab = new()
            {
                Name = $"WorkspaceTab{index + 1}",
                Parent = parent,
                Text = $"{index + 1}",
                HasBackground = hasBackground,
                MouseLeftButtonUpEvent = new System.Windows.Input.MouseButtonEventHandler((object sender, System.Windows.Input.MouseButtonEventArgs e) => {
                    MoveToDesktop(index);
                }),
                MouseRightButtonUpEvent = new System.Windows.Input.MouseButtonEventHandler((object sender, System.Windows.Input.MouseButtonEventArgs e) => {
                    RemoveDesktop(index);
                })
            };

            // Create a new desktop on current tab's left click
            if (i == currentDesktopIndex)
            {
                tab.MouseLeftButtonUpEvent = 
                    new System.Windows.Input.MouseButtonEventHandler((object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                    {
                        CreateDesktop();
                    });
            }

            // Add tab to a parent component
            tab.Create();
        }
    }

    static bool ShouldUpdateWorkspaces(int currentDesktopCount, int currentDesktopIndex) {
        if (OldDesktopCount != currentDesktopCount)
        {
            OldDesktopCount = currentDesktopCount;
            return true;
        }
        if (OldDesktopIndex != currentDesktopIndex)
        {
            OldDesktopIndex = currentDesktopIndex;
            return true;
        }
        return false;
    }
    static void CreateDesktop()
    {
        switch (WindowsVersion)
        {
            case WinVer.Win10:
                VirtualDesktop.Desktop.Create();
                MoveToDesktop(VirtualDesktop.Desktop.Count - 1);
            break;

            case WinVer.Win11:
                VirtualDesktop11.Desktop.Create();
                MoveToDesktop(VirtualDesktop11.Desktop.Count - 1);
            break;

            case WinVer.Win11_24H2:
                VirtualDesktop11_24H2.Desktop.Create();
                MoveToDesktop(VirtualDesktop11_24H2.Desktop.Count - 1);
            break;
        }
        InvokeWorkspaceTabsUpdate(InitializedBorder);
    }
    static void MoveToDesktop(int index)
    {
        switch (WindowsVersion)
        {
            case WinVer.Win10:
                VirtualDesktop.Desktop.FromIndex(index).MakeVisible();
            break;

            case WinVer.Win11:
                VirtualDesktop11.Desktop.FromIndex(index).MakeVisible();
            break;

            case WinVer.Win11_24H2:
                VirtualDesktop11_24H2.Desktop.FromIndex(index).MakeVisible();
            break;
        }
        InvokeWorkspaceTabsUpdate(InitializedBorder);
    }
    static void RemoveDesktop(int index)
    {
        // Prevent user from removing last standing workspace
        if (OldDesktopCount == 1) return;

        switch (WindowsVersion)
        {
            case WinVer.Win10:
                // return if this UI hasn't updated yet.
                if (index >= VirtualDesktop.Desktop.Count) return;

                VirtualDesktop.Desktop.FromIndex(index).Remove();
            break;

            case WinVer.Win11:
                // return if this UI hasn't updated yet.
                if (index >= VirtualDesktop11.Desktop.Count) return;

                VirtualDesktop11.Desktop.FromIndex(index).Remove();
            break;

            case WinVer.Win11_24H2:
                // return if this UI hasn't updated yet.
                if (index >= VirtualDesktop11_24H2.Desktop.Count) return;

                VirtualDesktop11_24H2.Desktop.FromIndex(index).Remove();
            break;
        }
        InvokeWorkspaceTabsUpdate(InitializedBorder);
    }

    public static void GoToDesktopRight()
    {
        switch (WindowsVersion)
        {
            case WinVer.Win10:
                if (VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current) == VirtualDesktop.Desktop.Count - 1) return;

                VirtualDesktop.Desktop.FromIndex(VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current) + 1).MakeVisible();
            break;

            case WinVer.Win11:
                if (VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current) == VirtualDesktop11.Desktop.Count - 1) return;

                VirtualDesktop11.Desktop.FromIndex(VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current) + 1).MakeVisible();
            break;

            case WinVer.Win11_24H2:
                if (VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current) == VirtualDesktop11_24H2.Desktop.Count - 1) return;

                VirtualDesktop11_24H2.Desktop.FromIndex(VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current) + 1).MakeVisible();
            break;
        }
        InvokeWorkspaceTabsUpdate(InitializedBorder);
    }

    public static void GoToDesktopLeft()
    {
        switch (WindowsVersion)
        {
            case WinVer.Win10:
                if (VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current) == 0) return;

                VirtualDesktop.Desktop.FromIndex(VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current) - 1).MakeVisible();
            break;

            case WinVer.Win11:
                if (VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current) == 0) return;

                VirtualDesktop11.Desktop.FromIndex(VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current) - 1).MakeVisible();
            break;

            case WinVer.Win11_24H2:
                if (VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current) == 0) return;

                VirtualDesktop11_24H2.Desktop.FromIndex(VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current) - 1).MakeVisible();
            break;
        }
        InvokeWorkspaceTabsUpdate(InitializedBorder);
    }
}
