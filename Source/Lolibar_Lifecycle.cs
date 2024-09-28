using LolibarApp.Source.Tools;
using LolibarApp.Mods;

namespace LolibarApp.Source;

partial class Lolibar
{

    #region Lifecycle
    void InitializeCycle()
    {
        // --- Initialize ---
        config.Initialize();

        // -- PostInitialize ---
        PostInitializeSnapping();
        ReloadResources();
    }
    async void UpdateCycle()
    {
        while (true)
        {
            await Task.Delay(Config.BarUpdateDelay);

            // --- Update ---
            config.Update();
        }
    }
    #endregion
}
