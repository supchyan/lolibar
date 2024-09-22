<div align=center><img src="https://github.com/user-attachments/assets/7e5daeb0-ee0c-4e9c-b584-21164433649d" height=80 /></div>

#### <div align=center>lolibar | [polybar](https://github.com/polybar/polybar) alternative for windows platform | c#</div>

> [!IMPORTANT]  
> This project is **toolkit** for developers, which grants capabilities to create statusbars, so there is no `ready-to-use` executable on [Releases](https://github.com/supchyan/lolibar/releases) page.
>
> If you want one, you can build it by yourself. All you need to run this project and create instant executable - just get `Visual Studio 2022` with `.NET SDK 8.0`. Then run solution and enjoy!
> 
> This project is for **Windows Platform** only! Please, check [polybar](https://github.com/polybar/polybar) repo, if you're looking for the Linux one.

</br>
<div align=center><img src="https://github.com/user-attachments/assets/69a80246-5d95-44d2-aa35-3967ca262d6f" /></div>
</br>
<div align=center><img src="https://github.com/user-attachments/assets/dacf73b1-3529-4538-9359-02895769c2dd" /></div>
</br>

> How does it work? Also, check this → [Config.cs](https://github.com/supchyan/lolibar/blob/master/Source/Mods/Config.cs)
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
// [Config.cs]
class Config : ModLolibar
{
    int someTimer;
    public override void Initialize()
    {
        // Let's override "UpdateDelay". Now it's 10 milliseconds.
        UpdateDelay = 10;
    }

    public override void Update()
    {
        // This method updates every "UpdateDelay"
        // So, that means, "IconSize" will be updated every 10 milliseconds here!
        someTimer++;
        IconSize = 16.0 * Math.Abs(Math.Sin(someTimer));
    }
}
// Simple enough, isn't it?
```

##### <div align=center> ☕Have a suggestions for this project? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div> </br>
<div align=center><img src="https://github.com/user-attachments/assets/69208a59-6092-4855-b165-44a277779592" /></div>

##### <div align=center>showcase ✨ [ work in progress tho ]</div>
