#### lolibar | windows [polybar](https://github.com/polybar/polybar) alternative | c#
</br>

> How is this work? Also, see [Config.cs](https://github.com/supchyan/lolibar/blob/master/Config.cs)
```csharp
// [Config.cs]

void Awake()
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

void Awake()
{
  // Remember, ALL resources are pre-included into a project. You just customize it for your sake.
  Resources["IconSize"] = 16.0;
}
void Update()
{
  SomeTimer++; // Updates every second. Keep it in mind.
  Resources["IconSize"] = 16.0 * Math.Abs(Math.Sin(SomeTimer));
}
```
</br>

![image](https://github.com/user-attachments/assets/d5797fb2-f973-44d7-baa8-5fe4533ef289)</br>
*<div align=center>Only started development, so it's just a WIP, but look at it! ✨</div>*
