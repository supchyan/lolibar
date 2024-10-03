<div align=center><img src="https://github.com/user-attachments/assets/7e5daeb0-ee0c-4e9c-b584-21164433649d" height=80 /></div>

#### <div align=center>lolibar | [polybar](https://github.com/polybar/polybar) alternative for windows platform | c#</div>

> [!IMPORTANT]  
> This project is **toolkit** for developers, which grants capabilities to create statusbars, so there is no `ready-to-use` executable on **[Releases](https://github.com/supchyan/lolibar/releases)** page. If you want one, you can build it by yourself.
> 
> This project is for **Windows Platform** only! Please, check **[polybar](https://github.com/polybar/polybar)** repo, if you're looking for the Linux one.

</br>
<div align=center><img src="https://github.com/user-attachments/assets/272cd6bf-415e-494a-a5a0-2d4c4a19847b" /></div>
</br>

### 🎟️Pre-requirements
All modding operations is highly recommended to do in `Visual Studio 2022`. Moreover, to build this project, you have to install `.NET8.0 SDK`. Actually, you can use other `.net sdks` as well, but `main branch` targets to `.net8.0`, so any issues with different `.net` versions you have to solve locally.

### 🎟️Basics understanding
In two words, as I mentioned before, this toolkit provides capabilites to modify existing setup of the lolibar. </br>
There are no many mod files to force you get into bunch of stuff. All you need to start modding is understand **[ModClass.cs](https://github.com/supchyan/lolibar/blob/master/Modding/ModClass.cs)**

> How does it work? Reference from → **[ModClass.cs](https://github.com/supchyan/lolibar/blob/master/Modding/ModClass.cs)**
```csharp
// [ModClass.cs]

class ModClass : LolibarProperties
{
    // --- Runs once after launch ---
    public override void Initialize()
    {
        // Here you can setup every style of the statusbar;
        // Also, you can initialize and create your own containers via LolibarContainer class.
        // Check examples section to get into advanced custom container creation.

        // Initializes default containers
        base.Initialize();
    }

    // --- Updates every `BarUpdateDelay` ---
    public override void Update()
    {
        // Updates default containers info
        base.Update();

        // Here you can setup / modify updatable content inside containers.
        // If you want to modify default container's content,
        // you have to do it below base.Update(), or even more, just remove it
        // to prevent content updates of the default containers at all.
    }
}
```

> Example mod:
```csharp
// [ModClass.cs]

// --- You can freely customize Lolibar's appearance here ---
class ModClass : LolibarProperties
{
    // --- Runs once after launch ---
    public override void Initialize()
    {
        // --- Properties. Starts with `Bar*`, so easy to remember ---
        BarUpdateDelay            = 250;
        BarHeight                 = 36;
        BarColor                  = LolibarHelper.SetColor("#2a3247");
        BarContainersContentColor = LolibarHelper.SetColor("#6f85bd");
        
        base.Initialize(); // Should be invoked after non-dynamic style changes

        // --- Let's add a new custom container ---
        new LolibarContainer()
        {
            Name    = "CustomSoundContainer",
            Parent  = Lolibar.BarRightContainer,
            Icon    = ModIcons.SoundIcon,
            Text    = "Sound",
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
```

<div align=center><img src="https://github.com/user-attachments/assets/e4524213-3df6-49e1-bdea-33d30c2015b2" /></div>

### 🎟️About `LolibarApp.csproj`
To compile this project, you have to set valid `TargetFramework` for your system in **[LolibarApp.csproj](https://github.com/supchyan/lolibar/blob/master/LolibarApp.csproj)** </br>
As an example of this source, for Windows 11 it should be set to `net8.0-windows10.0.22000.0`. </br>
You can read **[this](https://learn.microsoft.com/en-us/windows/apps/desktop/modernize/desktop-to-uwp-enhance)** article on Microsoft Learn to find out, which `TargetFramework` is for you.
```csproj
﻿<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>net8.0-windows10.0.22000.0</TargetFramework>
        ...other tags
    </PropertyGroup>
</Project>
```
### 🎟️Advanced containers guide
If you want to create enhanced, wide functional containers, you can check **[Examples](https://github.com/supchyan/lolibar/tree/master/Modding/Examples/)** section. In that section, I'm is about to show all, Lolibar Toolkit can provide.

---
##### <div align=center> ☕Have any questions or suggestions? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div>
