<div align=center><img src="https://github.com/user-attachments/assets/7e5daeb0-ee0c-4e9c-b584-21164433649d" height=80 /></div>

#### <div align=center>lolibar | [polybar](https://github.com/polybar/polybar) alternative for windows platform | c#</div>

> [!IMPORTANT]  
> This project is **toolkit** for developers, which grants capabilities to create statusbars, so there is no `ready-to-use` executable on **[Releases](https://github.com/supchyan/lolibar/releases)** page.
>
> If you want one, you can build it by yourself. All you need to start modding this project get `Visual Studio 2022` with `.NET SDK 8.0`. Run the solution and enjoy!
> 
> This project is for **Windows Platform** only! Please, check **[polybar](https://github.com/polybar/polybar)** repo, if you're looking for the Linux one.

</br>
<div align=center><img src="https://github.com/user-attachments/assets/272cd6bf-415e-494a-a5a0-2d4c4a19847b" /></div>
</br>

> How does it work? Reference from → **[ModClass.cs](https://github.com/supchyan/lolibar/blob/master/Modding/ModClass.cs)**
```csharp
// [ModClass.cs]

class ModClass : LolibarProperties
{
    // --- Runs once after launch ---
    public override void Initialize()
    {
        base.Initialize();
    }

    // --- Updates every `BarUpdateDelay` ---
    public override void Update()
    {
        base.Update();
    }
}
```

> Example:
```csharp
// [ModClass.cs]

// --- You can freely customize Lolibar's appearance here ---
class ModClass : LolibarProperties
{
    // --- Runs once after launch ---
    public override void Initialize()
    {
        // --- Properties ---
        BarUpdateDelay            = 300;
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
// Simple enough, isn't it? 🐳
```

<div align=center><img src="https://github.com/user-attachments/assets/1a9708d8-9cdc-46e0-8673-494672a53514" /></div>

##### <div align=center> ☕Have questions or suggestions? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div>
