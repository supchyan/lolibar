This guide represents `LolibarAudio` tools implementation. `LolibarAudio` has the same functionality you can find inside vanilla Windows Volume overlay (somehow this feature has been removed in `Windows 11`, but part of it keeps alive in lockscreen, odd) </br>
The same functionality exists inside variety of keyboards. So probably, you even have one! </br></br>

Basic implementation in the **[ModClass.cs](https://github.com/supchyan/lolibar/blob/master/Modding/ModClass.cs)**:
```cs
// [ModClass.cs]

class ModClass : LolibarProperties
{
    public override void Initialize()
    {
        base.Initialize();

        ExampleAudioStreamController.Create();
    }

    public override void Update()
    {
        base.Update();

        ExampleAudioStreamController.Update();
    }
}
```