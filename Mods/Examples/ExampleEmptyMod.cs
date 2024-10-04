using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;



// This mod represents minimal set of the modding methods to begin.
// Recommendations to use:
//
// PreInitialize(): for properties override;
//    Initialize(): for containers initialization;
//        Update(): for containers update;
//
// You can use `Update()` to update properties as well,
// but keep it in mind, you can't update properties of the already initialized containers,
// so any changes in `Update()` will be only applied to containers initialized after.
// 
// What about statusbar's properties, such as Width / Height ?
// All properties overrides inside `Update()` will be applied to statusbar immediately.
// Check `ExampleUpdatablePropertiesMod` to get in there.

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