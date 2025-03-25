using LolibarApp.Source.Tools;
using LolibarApp.Source;

//namespace LolibarApp.Mods;

class ExampleLaunchedApplicationsMod : LolibarMod
{
    // This mod example illustrates `LolibarProcess` capabilities.
    // If you want to simulate Windows Dockbar process handling, use this mod as a reference for your one!

    // Create some parent for applications:
    LolibarContainer ExampleAppsContainerParent = new();

    public override void PreInitialize() { }
    public override void Initialize()
    {
        // Set it up
        ExampleAppsContainerParent = new()
        {
            Name                    = "ExampleAppsContainerParent",
            Parent                  = Lolibar.BarRightContainer,
        };
        ExampleAppsContainerParent.Create();

        // Call this, if you want to clone all your pinned applications
        // into some container:
        LolibarProcess.AddPinnedAppsToContainer(ExampleAppsContainerParent.GetBody());
        
        // Now every `pinned to taskbar` application will be drawn inside lolibar.
        // He~ <3
    }
    public override void Update()
    {
        
    }
}
