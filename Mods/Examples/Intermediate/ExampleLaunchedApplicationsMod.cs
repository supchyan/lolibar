using LolibarApp.Source.Tools;
using LolibarApp.Source;

namespace LolibarApp.Mods;

class ExampleLaunchedApplicationsMod : LolibarMod
{
    LolibarContainer ApplicationsContainerParent = new();

    public override void PreInitialize() { }
    public override void Initialize()
    {
        ApplicationsContainerParent = new()
        {
            Name                    = "ApplicationsContainerParent",
            Parent                  = Lolibar.BarRightContainer,
        };
        ApplicationsContainerParent.Create();

        // Call this, if you want to clone all your pinned applications
        // into some container:
        LolibarProcess.AddPinnedAppsToContainer(ApplicationsContainerParent.GetBody());
    }
    public override void Update()
    {
        
    }
}
