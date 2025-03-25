using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;


// This mod represents minimal set of the modding methods to begin.
// Recommendations to use:
//
// PreInitialize(): for properties override;
//    Initialize(): for containers initialization;
//        Update(): for containers update;
// 
// Check `ExampleUpdatablePropertiesMod` to get into properties override.
// Check `ExampleFirstContainerMod` to get into containers initialization and update.
// Check `ExampleUpdatablePropertiesMod` to get into properties override.


class ExampleEmptyMod : LolibarMod
{
    public override void PreInitialize()
    {
        // Put your Pre-Initialization here...
    }
    public override void Initialize()
    {
        // Put your Initialization here...
    }
    public override void Update()
    {
        // Put your Updates here...
    }
}