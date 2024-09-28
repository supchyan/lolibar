<div align=center><img src="https://github.com/user-attachments/assets/7e5daeb0-ee0c-4e9c-b584-21164433649d" height=80 /></div>

#### <div align=center>lolibar | [polybar](https://github.com/polybar/polybar) alternative for windows platform | c#</div>

> [!IMPORTANT]  
> This project is **toolkit** for developers, which grants capabilities to create statusbars, so there is no `ready-to-use` executable on **[Releases](https://github.com/supchyan/lolibar/releases)** page.
>
> If you want one, you can build it by yourself. All you need to start modding this project get `Visual Studio 2022` with `.NET SDK 8.0`. Run the solution and enjoy!
> 
> This project is for **Windows Platform** only! Please, check **[polybar](https://github.com/polybar/polybar)** repo, if you're looking for the Linux one.

</br>
<div align=center><img src="https://github.com/user-attachments/assets/61c31ab5-b0aa-420f-81c0-5cd19cd136f4" /></div>

</br>

> How does it work? Also, check this → **[Config.cs](https://github.com/supchyan/lolibar/blob/master/Mods/Config.cs)**
```csharp
// [Config.cs]
class Config : LolibarMod
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

> Example
```csharp
// [Config.cs]
class Config : LolibarMod
{
    // --- Runs once after launch ---
    public override void Initialize()
    {
        // --- Properties ---
        BarUpdateDelay  = 300;
        BarHeight       = 36;
        BarColor        = LolibarHelper.SetColor("#452a25");
        BarContainersContentColor = LolibarHelper.SetColor("#b56e5c");

        base.Initialize(); // Should be invoked after style changes

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

        // This, how you can set custom info of the updatable container:
        BarUserContainer.Text = "🐳";
        BarUserContainer.Update();

        // Another example for the `Time Container`:
        BarTimeContainer.Text = $"{DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year} {DateTime.Now.DayOfWeek}";
        BarTimeContainer.Update();
    }

    // --- Example override of the default containers ---
    public override void CreateUserContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
    {
        base.CreateUserContainer(parent, LolibarEnums.SeparatorPosition.Right);
    }
    public override void CreateCurProcContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos) { }

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
// Simple enough, isn't it? 
```

<div align=center><img src="https://github.com/user-attachments/assets/244f5cd3-9a2a-47a4-851b-c1f604418d56" /></div>

##### <div align=center> ☕Have questions or suggestions? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div>
