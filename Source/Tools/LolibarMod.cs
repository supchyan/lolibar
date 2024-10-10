namespace LolibarApp.Source.Tools;

public abstract class LolibarMod : LolibarProperties
{
    /// <summary>
    /// Pre-Initialization method. Invokes once at lolibar's launch before Initialize() does it's job.
    /// Highly recommended to setup lolibar's styles here.
    /// </summary>
    public virtual void PreInitialize()
    {
        LolibarModLoader.PreInitializeMods();
    }
    /// <summary>
    /// Initialization method. Invokes once at lolibar's launch.
    /// Highly recommended to setup lolibar's containers here.
    /// </summary>
    public virtual void Initialize()
    {
        LolibarModLoader.InitializeMods();
    }

    /// <summary>
    /// Update method. Invokes repeatedly every "UpdateDelay".
    /// </summary>
    public virtual void Update()
    {
        LolibarModLoader.UpdateMods();
    }
}