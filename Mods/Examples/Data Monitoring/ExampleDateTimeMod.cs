﻿using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;

class ExampleDateTimeMod : LolibarMod
{
    LolibarContainer? DateTimeContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        DateTimeContainer = new()
        {
            Name = "ExampleDateTimeContainer",
            Parent = Lolibar.BarLeftContainer,
            Text = LolibarDefaults.GetTimeInfo(), // `LolibarDefaults` has method to get current time ...
            MouseLeftButtonUpEvent = OpenTimeSettingsEvent
        };
        DateTimeContainer.Create();
    }
    public override void Update()
    {
        // ... but I want to make my custom datetime container, so let's totally override its content:
        DateTimeContainer.Text = $"{DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} {DateTime.Now.DayOfWeek}";
        DateTimeContainer.Update();
    }

    void OpenTimeSettingsEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = "Start-Process ms-settings:dateandtime",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        }.Start();
    }
}

