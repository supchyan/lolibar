using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;
using System.Windows.Media;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;

class ExampleSoundSettingsMod : LolibarMod
{
    // --- Custom Icon ---
    Geometry SoundIcon = Geometry.Parse("M5 1C5 0.447715 4.55228 0 4 0C3.44772 0 3 0.447715 3 1L3 13C3 13.5523 3.44772 14 4 14C4.55229 14 5 13.5523 5 13L5 1ZM10 2C10.5523 2 11 2.44772 11 3V11C11 11.5523 10.5523 12 10 12C9.44771 12 9 11.5523 9 11V3C9 2.44772 9.44771 2 10 2ZM1 4C1.55228 4 2 4.44772 2 5L2 9C2 9.55229 1.55228 10 1 10C0.447715 10 0 9.55229 0 9V5C0 4.44772 0.447715 4 1 4ZM14 5C14 4.44772 13.5523 4 13 4C12.4477 4 12 4.44772 12 5V9C12 9.55229 12.4477 10 13 10C13.5523 10 14 9.55229 14 9V5ZM7 3C7.55228 3 8 3.44772 8 4V10C8 10.5523 7.55229 11 7 11C6.44772 11 6 10.5523 6 10L6 4C6 3.44772 6.44772 3 7 3Z");

    public override void PreInitialize() { }
    public override void Initialize()
    {
        // Creating new container with `LolibarContainer` class:
        new LolibarContainer()
        {
            Name = "ExampleSoundSettingsContainer",
            Parent = Lolibar.BarRightContainer,
            Icon = SoundIcon,
            Text = "Sound",
            MouseLeftButtonUpEvent = OpenSoundSettingsCustomEvent

        }.Create();
    }
    public override void Update() { }

    // This event will occure on Left Click, so let's bind `Open Sound Settings` feature here:
    void OpenSoundSettingsCustomEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = "Start-Process ms-settings:sound",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        }.Start();
    }
}