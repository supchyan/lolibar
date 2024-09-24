<div align=center><img src="https://github.com/user-attachments/assets/7e5daeb0-ee0c-4e9c-b584-21164433649d" height=80 /></div>

#### <div align=center>lolibar | [polybar](https://github.com/polybar/polybar) alternative for windows platform | c#</div>

> [!IMPORTANT]  
> This project is **toolkit** for developers, which grants capabilities to create statusbars, so there is no `ready-to-use` executable on **[Releases](https://github.com/supchyan/lolibar/releases)** page.
>
> If you want one, you can build it by yourself. All you need to start modding this project get `Visual Studio 2022` with `.NET SDK 8.0`. Run the solution and enjoy!
> 
> This project is for **Windows Platform** only! Please, check **[polybar](https://github.com/polybar/polybar)** repo, if you're looking for the Linux one.

</br>
<div align=center><img src="https://github.com/user-attachments/assets/0933f6ed-1ba8-479a-be8a-465b8a4f71f5" /></div>
</br>

> How does it work? Also, check this → **[Config.cs](https://github.com/supchyan/lolibar/blob/master/Mods/Config.cs)**
```csharp
// [Config.cs]

class Config : ModLolibar
{
    // Runs once after launch
    public override void Initialize()
    {
        
    }

    // Updates every "UpdateDelay".
    public override void Update()
    {

    }
}
```

> Example
```csharp
// [Config.cs] - My personal setup, which fits my needs
class Config : ModLolibar
{
    // Runs once after launch
    public override void Initialize()
    {
        UpdateDelay                 = 500;
        UseSystemTheme              = false;

        BarHeight                   = 36;

        BarColor                    = LolibarHelper.SetColor("#452a25");
        BarContainersContentColor   = LolibarHelper.SetColor("#b56e5c");

        HideBarInfoContainer        = true;

        // Let's add a clickable container!
        ContainerGenerator.CreateContainer(
            Lolibar.barRightContainer, // parent container
            LolibarDefaults.SoundIcon, // icon content
            "Sound",                   // text content
            OpenSoundSettings,         // onleftclick event
            default                    // onrightclick event [ which is null here ]
        );
    }

    // Updates every "UpdateDelay".
    public override void Update()
    {
        BarUserText = $"🐳";
        BarTimeText = $"{ DateTime.Now.Day } / { DateTime.Now.Month } / { DateTime.Now.Year } { DateTime.Now.DayOfWeek }";
    }

    // My custom event...
    void OpenSoundSettings(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
// Simple enough, isn't it?
```

<div align=center><img src="https://github.com/user-attachments/assets/000354cc-e5c5-4de5-9d7e-3e9a123e91e9" /></div>

##### <div align=center> ☕Have a suggestions for this project? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div>
