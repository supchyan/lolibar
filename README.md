#### <div align=center>lolibar | windows [polybar](https://github.com/polybar/polybar) alternative | c#</div>
<div align=center><img src="https://github.com/user-attachments/assets/b235a712-dd60-46f8-92f8-d436f10c7a7f" /></div>
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

> Example
```csharp
// [Lolibar_Config.cs]

int someTimer;

void Initialize()
{
  // Keep it in mind, that Resources[]* are pre-included and initialized in the project.
  // Also, resources has self-update on every "UpdateDelay" value elapsed (brings to default, if nothing overwritting it),
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
  SomeTimer++;
  Resources["IconSize"] *= 16.0 * Math.Abs(Math.Sin(someTimer));
}

// Simple enough, isn't it?
```

##### <div align=center>Have an idea for this project? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div> </br>
<div align=center><img src="https://github.com/user-attachments/assets/69208a59-6092-4855-b165-44a277779592" /></div>

##### <div align=center>showcase ✨ [ work in progress tho ]</div>
