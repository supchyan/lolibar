#### <div align=center>this is lolibar | windows [polybar](https://github.com/polybar/polybar) alternative | c#</div>
<div align=center><img src="https://github.com/user-attachments/assets/83e14fa1-1afc-4966-a583-5de564953f8a" /></div>
</br>

> How does it work? Also, see [Lolibar_Config.cs](https://github.com/supchyan/lolibar/blob/master/Lolibar_Config.cs)
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

int SomeTimer;

void Initialize()
{
  // Remember, ALL resources are pre-included into a project. You just customize it for your sake.
  // I'll add a full list with definitions later.
  Resources["UpdateDelay"]  = 10;   // in milliseconds
  Resources["IconSize"]     = 16.0;
}
void Update()
{
  SomeTimer++; // Updates every "UpdateDelay".
  Resources["IconSize"] = 16.0 * Math.Abs(Math.Sin(SomeTimer));
}
```
</br>
<div align=center><img src="https://github.com/user-attachments/assets/863863d6-946c-4cc7-b7c3-aa322448fdaf" /></div>

*<div align=center>Only started development, so it's just a WIP, but look at this! ✨</div>*
