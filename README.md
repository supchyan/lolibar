#### lolibar | windows [polybar](https://github.com/polybar/polybar) alternative | c#
</br>

> How is this work? Also, see [Config.cs](https://github.com/supchyan/lolibar/blob/master/Config.cs)
```csharp
// [Config.cs]

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
// [Config.cs]

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

![image](https://github.com/user-attachments/assets/66a5ccae-9563-4009-a822-0e1f76ef6df8)
</br>
*<div align=center>Only started development, so it's just a WIP, but look at this! ✨</div>*
