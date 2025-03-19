using LolibarApp.Source.Tools;
using System.Windows;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleStaticPropertiesMod : LolibarMod
{
    // So, what about modifying lolibar's theme?
    // It's simple, but you have to understand 2 important things:
    //
    // 1. DON'T: modify preperties in `Initialize()` hook!
    // 2.    DO: modify preperties in `Update()` or `PreInitialize()` hook:
    public override void PreInitialize()
    {
        BarUpdateDelay      = 250;
        BarHeight           = 36;
        BarColor            = LolibarHelper.SetColor("#2a3247");
        BarContainersColor  = LolibarHelper.SetColor("#6f85bd");
        BarMargin           = 10;
        BarCornerRadius     = new CornerRadius(16);
    }
    public override void Initialize() { } // It have to be used for containers initialization only!
    public override void Update() { }

}