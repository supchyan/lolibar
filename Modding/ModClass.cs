using LolibarApp.Source.Tools;
using System.Diagnostics;
using LolibarApp.Source;
using System.Windows.Controls;
using LolibarApp.Modding.Examples.ExampleAudioStreamController;

namespace LolibarApp.Modding;

// --- You can freely customize Lolibar's appearance here ---
class ModClass : LolibarProperties
{
    // --- Runs once after launch ---
    public override void Initialize()
    {
        // --- Properties. Starts with `Bar*`, so easy to remember ---
        BarUpdateDelay = 250;
        BarHeight = 36;
        BarColor = LolibarHelper.SetColor("#2a3247");
        BarContainersContentColor = LolibarHelper.SetColor("#6f85bd");

        base.Initialize(); // Should be invoked after non-dynamic style changes

        // --- Let's add a new custom container ---
        new LolibarContainer()
        {
            Name = "CustomSoundContainer",
            Parent = Lolibar.BarRightContainer,
            Icon = ModIcons.SoundIcon,
            Text = "Sound",
            MouseLeftButtonUpEvent = OpenSoundSettingsCustomEvent

        }.Create();

        // Check Lolibar's examples section to understand where it comes from:
        ExampleAudioStreamController.Create();
    }

    // --- Updates every `BarUpdateDelay` ---
    public override void Update()
    {
        base.Update(); // Use this, if you want to update default properties as well

        // This, how info in containers can be updated:
        BarUserContainer.Text = "🐳";
        BarUserContainer.Update();

        // Another example for the `Time container`:
        BarTimeContainer.Text = $"{DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} {DateTime.Now.DayOfWeek}";
        BarTimeContainer.Update();

        // Check Lolibar's examples section to understand where it comes from:
        ExampleAudioStreamController.Update();
    }

    // Example override initialization of the default containers...
    public override void CreateTimeContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
    {
        // Change spawn parent to `Lolibar.BarLeftContainer`;
        // Change separator position to Left:
        base.CreateTimeContainer(Lolibar.BarLeftContainer, LolibarEnums.SeparatorPosition.Left);
    }
    public override void CreateCurProcContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
    {
        // Change spawn parent to `Lolibar.BarRightContainer`;
        // Change separator position to Left:
        base.CreateCurProcContainer(Lolibar.BarRightContainer, LolibarEnums.SeparatorPosition.Left);
    }
    // ...Empty body here prevents these containers from initializing at all.
    public override void CreateCpuContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos) { }
    public override void CreateRamContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos) { }
    public override void CreateDiskContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos) { }
    public override void CreateNetworkContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos) { }

    // --- Example custom event ---
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
// Simple enough, isn't it? 🐳