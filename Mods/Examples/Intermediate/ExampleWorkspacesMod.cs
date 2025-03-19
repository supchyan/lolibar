using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleWorkspacesMod : LolibarMod
{
    // This mod represents power of the `LolibarVirtualDesktop` library.
    // But to be straight, heavy part of mod is self sufficient,
    // so all you need is create parent container, where you want to put your tabs,
    // and just invoke tabs update once in `Initialize()` hook.
    //
    // In the end, lolibar will handle windows virtual desktops on time.

    LolibarContainer? WorkspacesContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        // I want to store my tabs in the unique container, so let's create one:
        WorkspacesContainer = new()
        {
            Name = "ExampleWorkspacesContainer",
            Parent = Lolibar.BarRightContainer,
            SeparatorPosition = LolibarEnums.SeparatorPosition.Left,
            MouseWheelEvent = SwapWorkspacesByMouseWheelEvent // swap workspaces, scrolling above this container
        };
        WorkspacesContainer.Create();

        // We use `InvokeWorkspaceTabsUpdate()` to fill WorkspacesContainer with workspaces.
        // Otherwise, it will be empty.
        // After invoking it for the first time,
        // `InvokeWorkspaceTabsUpdate` will update itself automatically,
        // so no need to put it into `Update()` hook!
        LolibarVirtualDesktop.InvokeWorkspaceTabsUpdate(
            // Where we should append workspaces. Their parent container:
            parent:             WorkspacesContainer.SpaceInside,
            // Append workspaces* (*desktops) with its names, otherwise names will be replaced by indexes.
            // Ah and, if desktop has no name, it will be named its index too.
            showDesktopNames:   true
        );

        // Important thing, `LolibarVirtualDesktop` controls last provided container,
        // so if you want to dublicate your virtual desktops controls for some reason,
        // it won't work like that.
    }
    public override void Update() { }

    // This event listens mouse wheel. Wheel up swap desktops (workspaces) to left, otherwise to right:
    void SwapWorkspacesByMouseWheelEvent(object sender, MouseWheelEventArgs e)
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

