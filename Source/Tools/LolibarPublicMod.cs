namespace LolibarApp.Source.Tools;

/// <summary>
/// Public reference of the LolibarMod. Uses to invoke `Initialize` and `Update` hooks inside `Lolibar` class.
/// </summary>
public class LolibarPublicMod : LolibarMod
{
    public override void PreInitialize()
    {
        base.PreInitialize();
    }
    public override void Initialize()
    {
        base.Initialize();
    }
    public override void Update()
    {
        base.Update();
    }
}