using LolibarApp.Source;
using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;

class ExampleWorkspacesMod : LolibarMod
{
    // This mod represents power of the `LolibarVirtualDesktop` library.
    // But to be straight, it's self sufficient,
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
            UseWorkspaceSwapEvents = true,
            SeparatorPosition = LolibarEnums.SeparatorPosition.Left,
        };
        WorkspacesContainer.Create();

        // We use this to fill WorkspacesContainer with Desktop Tabs.
        // Otherwise, it will be empty.
        // After invoking it for the first time,
        // `InvokeWorkspaceTabsUpdate` will update itself automatically,
        // so no need to put it into `Update()` hook!
        LolibarVirtualDesktop.InvokeWorkspaceTabsUpdate(WorkspacesContainer.BorderComponent);

        // Important thing, `LolibarVirtualDesktop` controls last provided container,
        // so if you want to dublicate your virtual desktops controls for some reason,
        // it won't work like that.
    }
    public override void Update() { }
}

