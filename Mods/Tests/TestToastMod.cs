using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;


// TESTING FEATURES, DON'T TRY TO IMPLEMENT IT IN YOUR MOD
//namespace LolibarApp.Mods;

class TestToastMod : LolibarMod
{
    LolibarContainer testtoastContainer = new();
    public override void PreInitialize() { }
    public override void Initialize()
    {
        testtoastContainer    = new()
        {
            Parent                  = Lolibar.BarRightContainer,
            Text                    = "toast",
            HasBackground           = true,
            RightMarginOffset       = 10.0,
            MouseLeftButtonUp       = ShowToast
        };
        testtoastContainer.Create();
    }
    public override void Update() { }
    int ShowToast(MouseButtonEventArgs e)
    {
        LolibarToast test = new()
        {
            Text = "TEST text!!! 🐳",
            ShowTime = 1000
        };
        test.Create();

        return 0;
    }
}