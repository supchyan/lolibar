﻿using LolibarApp.Source.Tools;
using System.Diagnostics;
using LolibarApp.Source;
using System.Windows.Controls;

namespace LolibarApp.Mods;

// --- You can freely customize Lolibar's appearance here ---
class Config : LolibarMod
{
    // --- Runs once after launch ---
    public override void Initialize()
    {
        // --- Properties ---
        BarUpdateDelay            = 300;
        BarHeight                 = 36;
        BarColor                  = LolibarHelper.SetColor("#2a3247");
        BarContainersContentColor = LolibarHelper.SetColor("#6f85bd");
        
        base.Initialize(); // Should be invoked after static style changes

        // --- Let's add a new custom container ---
        new LolibarContainer()
        {
            Name    = "CustomSoundContainer",
            Parent  = Lolibar.BarRightContainer,
            Icon    = Icons.SoundIcon,
            Text    = "Sound",
            MouseLeftButtonUpEvent = OpenSoundSettingsCustomEvent

        }.Create();
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
    }

    // --- Example override of the default containers ---
    public override void CreateCurProcContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
    {
        base.CreateCurProcContainer(Lolibar.BarRightContainer, LolibarEnums.SeparatorPosition.Left);
    }
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
                FileName        = "powershell.exe",
                Arguments       = "Start-Process ms-settings:sound",
                UseShellExecute = false,
                CreateNoWindow  = true,
            }
        }.Start();
    }
}