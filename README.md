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

![image](https://github.com/user-attachments/assets/0a3ff146-8898-4a8c-9f07-a9eeaae76c90)
</br>
*<div align=center>Only started development, so it's just a WIP, but look at this! ✨</div>*
