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
  // Keep it in mind, that resources are pre-included and initialized in the project.
  // Also, it updates every "UpdateDelay" value elapsed, so you can do anything with it with no toughs.
  // Override it for your sake!
  // List with all definitions about every resource I will add later.

  // Let's override "UpdateDelay". Now it's 10 milliseconds.
  Resources["UpdateDelay"]  = 10;
}
void Update()
{
  // This method updates every "UpdateDelay"

  // So, that means, "IconSize" will be updated every 10 milliseconds here!
  SomeTimer++;
  Resources["IconSize"] *= Math.Abs(Math.Sin(someTimer));
}

// Simple enough, isn't it?
```

##### <div align=center>Have an idea for this project? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div> </br>
<div align=center><img src="https://github.com/user-attachments/assets/69208a59-6092-4855-b165-44a277779592" /></div>

