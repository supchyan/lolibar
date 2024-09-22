<div align=center><img src="https://github.com/user-attachments/assets/7e5daeb0-ee0c-4e9c-b584-21164433649d" height=80 /></div>

#### <div align=center>lolibar | [polybar](https://github.com/polybar/polybar) alternative for windows platform | c#</div>

> [!IMPORTANT]  
> This project is **toolkit** for developers, which grants capabilities to create statusbars, so there is no `ready-to-use` executable on **[Releases](https://github.com/supchyan/lolibar/releases)** page.
>
> If you want one, you can build it by yourself. All you need to start modding this project are getting `Visual Studio 2022` with `.NET SDK 8.0`. Run the solution and enjoy!
> 
> This project is for **Windows Platform** only! Please, check **[polybar](https://github.com/polybar/polybar)** repo, if you're looking for the Linux one.

</br>
<div align=center><img src="https://github.com/user-attachments/assets/59b2e841-52fe-4484-80f2-38672efd701a" /></div>
</br>

> How does it work? Also, check this → **[Config.cs](https://github.com/supchyan/lolibar/blob/master/Source/Mods/Config.cs)**
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
        UpdateDelay = 500;
        UseSystemTheme = false;
        BarStrokeSize = 2;
        BarColor = LolibarHelper.SetColor("#05202d");
        ElementColor = LolibarHelper.SetColor("#e2968b");
    }
    
    // Updates every "UpdateDelay".
    public override void Update()
    {
        BarUserText = "🐳";
        BarTimeText = $"{ DateTime.Now.Day } / { DateTime.Now.Month } / { DateTime.Now.Year } { DateTime.Now.DayOfWeek }";
    }
}
// Simple enough, isn't it?
```

<div align=center><img src="https://github.com/user-attachments/assets/cac83864-8692-4200-bebf-43072cb8c15a" /></div>

##### <div align=center> ☕Have a suggestions for this project? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div>
