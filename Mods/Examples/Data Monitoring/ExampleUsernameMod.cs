using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;

class ExampleUsernameMod : LolibarMod
{
    public override void PreInitialize() { }
    public override void Initialize()
    {
        var ExampleUsernameContainer = new LolibarContainer()
        {
            Name = "ExampleUsernameContainer",
            Parent = Lolibar.BarLeftContainer,
            Text = LolibarDefaults.GetUserInfo(), // your username in the OS
            MouseLeftButtonUpEvent = OpenUserSettingsEvent, // let's add a `leftclick` mouse event to this container
        };
        ExampleUsernameContainer.Create();
    }
    public override void Update() { }

    void OpenUserSettingsEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
    }
}
