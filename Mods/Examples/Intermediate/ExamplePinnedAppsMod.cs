using LolibarApp.Source.Tools;
using LolibarApp.Source;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExamplePinnedAppsMod : LolibarMod
{
    // This mod example illustrates `LolibarProcess` capabilities.
    // It close to what windows taskbar does with pinned apps.

    // Create some parent for applications:
    LolibarContainer ExampleAppsContainerParent = new();

    public override void PreInitialize() { }
    public override void Initialize()
    {
        // Set it up
        ExampleAppsContainerParent = new()
        {
            Parent = Lolibar.BarRightContainer,
        };
        ExampleAppsContainerParent.Create();

        // Call this, if you want to clone all your pinned applications
        // into some container:
        LolibarProcess.AddPinnedAppsToContainer
        (
            parent:                 ExampleAppsContainerParent.GetBody(),           // Parent select
            appContainerTitleState: LolibarEnums.AppContainerTitleState.OnlyActive, // When you want to see apps' titles (names)
            appTitleMaxLength:      default                                         // Max title length (when is visible)
        );
        
        // Now every `pinned to taskbar` application will be drawn inside lolibar.

        // At this moment it has several restrictions.
        // First, you can handle only MainWindow of some process, 
        // so if you have multiple browser windows opened as an example, 
        // you can only operate with the active one using lolibar.

        // Second, if your pinned app .ink file has script included, it won't run. (Discord .ink as an example)
        // I know about second issue and working on it's solution, so I'll remove this message after fix.
    }
    public override void Update()
    {
        
    }
}
