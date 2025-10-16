using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;
using System.Diagnostics;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleUsernameMod : LolibarMod
{
    public override void PreInitialize() { }
    public override void Initialize()
    {
        var ExampleUsernameContainer    = new LolibarContainer()
        {
            Parent                      = Lolibar.BarLeftContainer,
            Text                        = LolibarStats.UserInfo, // your OS username
            MouseLeftButtonUp           = OpenUserSettings, // let's add a `leftclick` mouse event to this container
        };
        ExampleUsernameContainer.Create();
    }
    public override void Update() { }
    int OpenUserSettings(MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = "Start-Process ms-settings:accounts",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        }.Start();

        return 0;
    }
}
