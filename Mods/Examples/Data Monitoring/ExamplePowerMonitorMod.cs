﻿using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;

class ExamplePowerMonitorMod : LolibarMod
{
    LolibarContainer? PowerMonitorContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        PowerMonitorContainer = new()
        {
            Name = "ExamplePowerMonitorContainer",
            Parent = Lolibar.BarRightContainer,
            Text = LolibarDefaults.GetPowerInfo(),
            Icon = LolibarDefaults.GetPowerIcon(),
            MouseLeftButtonUpEvent = OpenPowerSettingsEvent
        };
        PowerMonitorContainer.Create();
    }
    public override void Update()
    {
        PowerMonitorContainer.Text = LolibarDefaults.GetPowerInfo();
        PowerMonitorContainer.Icon = LolibarDefaults.GetPowerIcon();
        PowerMonitorContainer.Update();
    }
    void OpenPowerSettingsEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = "Start-Process ms-settings:batterysaver",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        }.Start();
    }
}