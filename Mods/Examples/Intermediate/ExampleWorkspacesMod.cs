using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleWorkspacesMod : LolibarMod
{
    // This mod represents the power of `LolibarVirtualDesktop` library.
    // But to be straight, very part of mod is self sufficient,
    // so all you need is create a parent container, where you want to put your tabs,
    // and just enable tabs update hook in `Initialize()` hook. (A hook inside a hook, I can't tell.. xd)

    // I want to store all my desktops in the unique container, so let's create one:
    LolibarContainer? WorkspacesContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        // Let's define it's properties:
        WorkspacesContainer     = new()
        {
            Name                = "ExampleWorkspacesContainer",
            Parent              = Lolibar.BarRightContainer,
            SeparatorPosition   = LolibarEnums.SeparatorPosition.Left,
            MouseWheelDelta     = SwapWorkspacesByMouseWheel
        };
        WorkspacesContainer.Create();

        // We use `DrawWorkspacesInParent()` to fill WorkspacesContainer with workspaces (virtual desktops / tabs),
        // otherwise, it will be empty.
        LolibarVirtualDesktop.DrawWorkspacesInParent
        (
            parent: WorkspacesContainer.GetBody(),
            showDesktopNames: true // Check it to `true`, if you want to draw desktops' names (Desktops with no name will be named as their position index).
        );

        // `DrawWorkspacesInParent` will update itself automatically,
        // so no need to put it into `Update()` hook!

        // Important thing, `LolibarVirtualDesktop` controls last provided container,
        // so if you want to dublicate your virtual desktops controls in different containers for some reason,
        // it won't work like that.
    }
    public override void Update() { }

    // This event listens mouse wheel. Wheel delta up (delta > 0) swap desktops (workspaces) to left, otherwise (delta < 0) to right:
    int SwapWorkspacesByMouseWheel(MouseWheelEventArgs e)
    {
        if (e.Delta > 0)
        {
            LolibarVirtualDesktop.GoToDesktopLeft();
        }

        if (e.Delta < 0)
        {
            LolibarVirtualDesktop.GoToDesktopRight();
        }

        return 0;
    }
}

