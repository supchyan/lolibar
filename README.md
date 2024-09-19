<div align=center><img src="https://github.com/user-attachments/assets/453c304f-2082-481a-9c32-7b4fdff2afef" width=300/></div>

#### <div align=center>lolibar | customizable windows [polybar](https://github.com/polybar/polybar) alternative | c#</div>
</br>

> [!WARNING]  
> This project is under development process, so there is no `ready-to-use` version on [Releases](https://github.com/supchyan/lolibar/releases) page. But you can get it, building it by yourself. All you need to build this project is `Visual Studio 2022+` with `.NET SDK 8.0+`. This project is for Windows Platform only! Please, check [polybar](https://github.com/polybar/polybar) repo, if you're looking for the Linux one.
</br>

> How does it work? Also, check this → [Lolibar_Config.cs](https://github.com/supchyan/lolibar/blob/master/Lolibar_Config.cs)
```csharp
// [Lolibar_Config.cs]

void Initialize()
{
  // Define resources...
}
void Update()
{
  // ...Then update it here if you supposed to
}
```
</br>
<div align=center><img src="https://github.com/user-attachments/assets/059d87a6-2116-4ab0-a757-0f3a313ccb8e" /></div>
</br>

> Example
```csharp
// [Lolibar_Config.cs]

int someTimer;

void Initialize()
{
  // Keep it in mind, that Resources[]* are pre-included and initialized in the project.
  // Also, resources has self-update on every "UpdateDelay" value elapsed (i.e. brings to default, if nothing overwritting it),
  // so you can override it for your sake.
  // ---
  // * List with all definitions about every resource in Resources[], about it's type and capability - I'll be able to add later.

  // Let's override "UpdateDelay". Now it's 10 milliseconds.
  Resources["UpdateDelay"]  = 10;
}
void Update()
{
  // This method updates every "UpdateDelay"

  // So, that means, "IconSize" will be updated every 10 milliseconds here!
  someTimer++;
  Resources["IconSize"] *= 16.0 * Math.Abs(Math.Sin(someTimer));
}

// Simple enough, isn't it?
```

##### <div align=center> ☕Have a suggestions for this project? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div> </br>
<div align=center><img src="https://github.com/user-attachments/assets/69208a59-6092-4855-b165-44a277779592" /></div>

##### <div align=center>showcase ✨ [ work in progress tho ]</div>
