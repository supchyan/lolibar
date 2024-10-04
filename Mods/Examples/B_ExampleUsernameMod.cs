using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;

namespace LolibarApp.Mods;

class B_ExampleUsernameMod : LolibarMod
{
    public override void PreInitialize() { }
    public override void Initialize()
    {
        var ExampleUsernameContainer = new LolibarContainer()
        {
            Name = "ExampleUsernameContainer",
            Parent = Lolibar.BarLeftContainer,
            Text = LolibarDefaults.GetUserInfo(),
            SeparatorPosition = LolibarEnums.SeparatorPosition.Right,
            MouseLeftButtonUpEvent = OpenUserSettingsEvent,

        };
        ExampleUsernameContainer.Create();

        // Now you will see a container contains your current username,
        // but I like whales, so let's change it to whale's emoji:
        ExampleUsernameContainer.Text = "🐳";
        ExampleUsernameContainer.Update();
    }
    public override void Update()
    {
        // Put your Updates here...
    }

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