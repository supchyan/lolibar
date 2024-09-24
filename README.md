<div align=center><img src="https://github.com/user-attachments/assets/7e5daeb0-ee0c-4e9c-b584-21164433649d" height=80 /></div>

#### <div align=center>lolibar | [polybar](https://github.com/polybar/polybar) alternative for windows platform | c#</div>

> [!IMPORTANT]  
> This project is **toolkit** for developers, which grants capabilities to create statusbars, so there is no `ready-to-use` executable on **[Releases](https://github.com/supchyan/lolibar/releases)** page.
>
> If you want one, you can build it by yourself. All you need to start modding this project get `Visual Studio 2022` with `.NET SDK 8.0`. Run the solution and enjoy!
> 
> This project is for **Windows Platform** only! Please, check **[polybar](https://github.com/polybar/polybar)** repo, if you're looking for the Linux one.

</br>
<div align=center><img src="https://github.com/user-attachments/assets/61e8207e-10a6-4e7a-b8a0-775204f900a0" /></div>
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
        UpdateDelay     = 500;
        UseSystemTheme  = false;
        BarColor        = LolibarHelper.SetColor("#05202d");
        BarElementColor = LolibarHelper.SetColor("#e2968b");
        BarHeight       = 36;
    }

    // Updates every "UpdateDelay".
    public override void Update()
    {
        BarUserText     = "🐳";
        BarTimeText     = $"{ DateTime.Now.Day } / { DateTime.Now.Month } / { DateTime.Now.Year } { DateTime.Now.DayOfWeek }";
    }
}
// Simple enough, isn't it?
```

<div align=center><img src="https://github.com/user-attachments/assets/fb7365e6-a60f-4e95-bf83-e8f364ecbe21" /></div>

##### <div align=center> ☕Have a suggestions for this project? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div>
