Basic implementation in the **[ModClass.cs](https://github.com/supchyan/lolibar/blob/master/Modding/ModClass.cs)**:
```cs
// [ModClass.cs]

class ModClass : LolibarProperties
{
    public override void Initialize()
    {
        base.Initialize();

        ExampleGnomeLinuxLeftCornerSimulation.Apply();
    }

    public override void Update()
    {
        base.Update();
    }
}
```