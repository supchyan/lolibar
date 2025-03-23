using System.Windows.Controls;
using static LolibarApp.Source.Tools.LolibarEnums;

namespace LolibarApp.Source.Tools;

public class LolibarVirtualDesktop
{
    /// <summary>
    /// Some initialized readonly parent container for the virtual desktops navigation. (null by default)
    /// </summary>
    static StackPanel? InitializedParent            { get; set; }
    /// <summary>
    /// Some initialized readonly trigger,
    /// determines whenever desktops' tabs should be named
    /// as actual desktops they controls. (false by default)
    /// </summary>
    static bool        InitializedShowDesktopNames  { get; set; }
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
    /// <summary>
    /// Invokes drawing/updating logic for the Virtual Desktop lib.
    /// Calling this, will draw/update all current windows tabs in specified `parent`. 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="showDesktopNames"></param>
    public static void DrawWorkspacesInParent(StackPanel? parent, bool showDesktopNames)
    {
        // Stop doing all logic below, if `error_tab` has been generated
        // or parent is undefined.
        if (IsErrorTabGenerated || parent == null) return;

        InitializedParent ??= parent;
        InitializedShowDesktopNames = showDesktopNames;

        /* 
        What's happening below?

        WinOS has no std to work with virtual desktops,
        but https://github.com/MScholtes/VirtualDesktop made tools
        for different WinOS versions, which handle this stuff properly.
        I've implemented it as different `namespaces`, so the code below
        adapts to specified WinOS patch, otherwise returns `unsupported` callback.

        Contact me on my Discord server, if you know better way
        to solve that kind of problem, thanks.
        
        (supchyan)
        */

        switch (WindowsVersion)
        {
            case WinVer.Unknown:
                GetWindowsVersion();
                DrawWorkspacesInParent(InitializedParent, InitializedShowDesktopNames);
            break;

            case WinVer.Win10:
                UpdateWorkspaceTabs(
                    parent,
                    desktopCount: VirtualDesktop.Desktop.Count,
                    currentDesktopIndex: VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current),
                    showDesktopNames: InitializedShowDesktopNames
                );
            break;

            case WinVer.Win11:
                UpdateWorkspaceTabs(
                    parent,
                    desktopCount: VirtualDesktop11.Desktop.Count,
                    currentDesktopIndex: VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current),
                    showDesktopNames: InitializedShowDesktopNames
                );
            break;

            case WinVer.Win11_24H2:
                UpdateWorkspaceTabs(
                    parent,
                    desktopCount: VirtualDesktop11_24H2.Desktop.Count,
                    currentDesktopIndex: VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current),
                    showDesktopNames: InitializedShowDesktopNames
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

        new LolibarContainer()
        {
            Name = $"WorkspaceErrorTab",
            Parent = parent,
            Text = "unsupported",
            HasBackground = true

        }.Create();

        // To prevent reapeats
        IsErrorTabGenerated = true;
    }
    // Recreates workspaces tabs, which is related on Windows Virtual Desktops
    static void UpdateWorkspaceTabs(StackPanel? parent, int desktopCount, int currentDesktopIndex, bool showDesktopNames)
    {
        if (parent == null) return;

        // Remove all children
        parent.Children.RemoveRange(0, parent.Children.Count);

        var hasBackground = false;

        // And create new children, lol
        for (int i = 0; i < desktopCount; i++)
        {
            int index = i; // Just put it here like that

            hasBackground = i == currentDesktopIndex;

            var desktopName = "";

            switch (WindowsVersion)
            {
                case WinVer.Win10:
                    desktopName = $"{VirtualDesktop.Desktop.DesktopNameFromIndex(index)}";
                    break;

                case WinVer.Win11:
                    desktopName = $"{VirtualDesktop11.Desktop.DesktopNameFromIndex(index)}";
                    break;

                case WinVer.Win11_24H2:
                    desktopName = $"{VirtualDesktop11_24H2.Desktop.DesktopNameFromIndex(index)}";
                    break;
            }

            LolibarContainer tab = new()
            {
                Name = $"WorkspaceTab{index + 1}",
                Parent = parent,
                Text = showDesktopNames ? desktopName : $"{index + 1}",
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
    /// <summary>
    /// Manually updates initialized Virtual Dekstops in some initialized parent.
    /// </summary>
    public static void UpdateInitializedDesktops()
    {
        if (InitializedParent == null) return;

        DrawWorkspacesInParent(InitializedParent, InitializedShowDesktopNames);
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
        DrawWorkspacesInParent(InitializedParent, InitializedShowDesktopNames);
    }
    static void MoveToDesktop(int index)
    {
        switch (WindowsVersion)
        {
            case WinVer.Win10:
                // break if this UI hasn't updated yet.
                if (index >= VirtualDesktop.Desktop.Count) break;

                VirtualDesktop.Desktop.FromIndex(index).MakeVisible();
            break;

            case WinVer.Win11:
                // break if this UI hasn't updated yet.
                if (index >= VirtualDesktop11.Desktop.Count) break;

                VirtualDesktop11.Desktop.FromIndex(index).MakeVisible();
            break;

            case WinVer.Win11_24H2:
                // break if this UI hasn't updated yet.
                if (index >= VirtualDesktop11_24H2.Desktop.Count) break;

                VirtualDesktop11_24H2.Desktop.FromIndex(index).MakeVisible();
            break;
        }
        DrawWorkspacesInParent(InitializedParent, InitializedShowDesktopNames);
    }
    static void RemoveDesktop(int index)
    {
        // Prevent user from removing last standing workspace
        if (OldDesktopCount == 1) return;

        switch (WindowsVersion)
        {
            case WinVer.Win10:
                // break if this UI hasn't updated yet.
                if (index >= VirtualDesktop.Desktop.Count) break;

                VirtualDesktop.Desktop.FromIndex(index).Remove();
            break;

            case WinVer.Win11:
                // break if this UI hasn't updated yet.
                if (index >= VirtualDesktop11.Desktop.Count) break;

                VirtualDesktop11.Desktop.FromIndex(index).Remove();
            break;

            case WinVer.Win11_24H2:
                // break if this UI hasn't updated yet.
                if (index >= VirtualDesktop11_24H2.Desktop.Count) break;

                VirtualDesktop11_24H2.Desktop.FromIndex(index).Remove();
            break;
        }
        DrawWorkspacesInParent(InitializedParent, InitializedShowDesktopNames);
    }
    public static void GoToDesktopRight()
    {
        switch (WindowsVersion)
        {
            case WinVer.Win10:
                // break if this UI hasn't updated yet.
                if (VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current) == VirtualDesktop.Desktop.Count - 1) break;

                VirtualDesktop.Desktop.FromIndex(VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current) + 1).MakeVisible();
            break;

            case WinVer.Win11:
                // break if this UI hasn't updated yet.
                if (VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current) == VirtualDesktop11.Desktop.Count - 1) break;

                VirtualDesktop11.Desktop.FromIndex(VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current) + 1).MakeVisible();
            break;

            case WinVer.Win11_24H2:
                // break if this UI hasn't updated yet.
                if (VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current) == VirtualDesktop11_24H2.Desktop.Count - 1) break;

                VirtualDesktop11_24H2.Desktop.FromIndex(VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current) + 1).MakeVisible();
            break;
        }
        DrawWorkspacesInParent(InitializedParent, InitializedShowDesktopNames);
    }
    public static void GoToDesktopLeft()
    {
        switch (WindowsVersion)
        {
            case WinVer.Win10:
                // break if this UI hasn't updated yet.
                if (VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current) == 0) break;

                VirtualDesktop.Desktop.FromIndex(VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current) - 1).MakeVisible();
            break;

            case WinVer.Win11:
                // break if this UI hasn't updated yet.
                if (VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current) == 0) break;

                VirtualDesktop11.Desktop.FromIndex(VirtualDesktop11.Desktop.FromDesktop(VirtualDesktop11.Desktop.Current) - 1).MakeVisible();
            break;

            case WinVer.Win11_24H2:
                // break if this UI hasn't updated yet.
                if (VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current) == 0) break;

                VirtualDesktop11_24H2.Desktop.FromIndex(VirtualDesktop11_24H2.Desktop.FromDesktop(VirtualDesktop11_24H2.Desktop.Current) - 1).MakeVisible();
            break;
        }
        DrawWorkspacesInParent(InitializedParent, InitializedShowDesktopNames);
    }
}
