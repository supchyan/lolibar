using LolibarApp.Source.Tools;
using System.Diagnostics;
using LolibarApp.Source;
using System.Windows.Controls;

namespace LolibarApp.Mods;

// --- You can freely customize Lolibar's appearance here ---
class Config : ModLolibar
{
    // --- Runs once after launch ---
    public override void Initialize()
    {
        // --- Properties ---
        BarHeight       = 36;
        BarColor        = LolibarHelper.SetColor("#452a25");
        BarContainersContentColor = LolibarHelper.SetColor("#b56e5c");

        // --- Initializes default containers ---
        base.Initialize();

        // --- Let's add a new custom container ---
        new LolibarContainer()
        {
            Name    = "CustomSoundContainer",
            Parent  = Lolibar.BarRightContainer,
            Icon    = Icons.SoundIcon,
            Text    = "Sound",
            MouseLeftButtonUpEvent = OpenSoundSettingsEvent

        }.Create();
    }

    // --- Updates every `BarUpdateDelay` ---
    public override void Update()
    {
        // --- Updates default properties ---
        base.Update();

        // I want to change a content inside User and Time containers, so:
        BarUserText = $"🐳";
        BarTimeText = $"{ DateTime.Now.Day } / { DateTime.Now.Month } / { DateTime.Now.Year } { DateTime.Now.DayOfWeek }";
    }

    // --- Example overriding of the default containers ---
    public override void CreateCurProcContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        base.CreateCurProcContainer(parent, LolibarEnums.SeparatorPosition.Left);
    }
    public override void CreateCpuContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        base.CreateCpuContainer(null, sepPos);
    }
    public override void CreateRamContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        base.CreateRamContainer(null, sepPos);
    }
    public override void CreateDiskContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        base.CreateDiskContainer(null, sepPos);
    }
    public override void CreateNetworkContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        base.CreateNetworkContainer(null, sepPos);
    }

    // --- Example custom event ---
    void OpenSoundSettingsEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName        = "powershell.exe",
                Arguments       = "Start-Process ms-settings:sound",
                UseShellExecute = false,
                CreateNoWindow  = true,
            }
        }.Start();
    }
}
