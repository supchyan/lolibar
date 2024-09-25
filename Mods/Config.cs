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
        BarUpdateDelay  = 500;
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
            Icon    = LolibarDefaults.SoundBaseIcon,
            Text    = "Sound",
            MouseLeftButtonUpEvent = OpenSoundSettingsEvent

        }.Create();
    }

    // --- Updates every `BarUpdateDelay` ---
    public override void Update()
    {
        // --- Updates default properties ---
        base.Update();

        // I want to change Content inside User and Time containers, so:
        BarUserText = $"🐳";
        BarTimeText = $"{ DateTime.Now.Day } / { DateTime.Now.Month } / { DateTime.Now.Year } { DateTime.Now.DayOfWeek }";
    }

    // --- Example default containers override ---
    public override void CreateUserContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        base.CreateUserContainer(parent, LolibarEnums.SeparatorPosition.Right);
    }
    public override void CreateCurProcContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        base.CreateCurProcContainer(null, sepPos);
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
