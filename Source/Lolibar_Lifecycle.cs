using System.Windows;
using LolibarApp.Source.Tools;
using LolibarApp.Mods;
using System.Windows.Media;
using System.Windows.Controls;
namespace LolibarApp.Source
{
    partial class Lolibar
    {

        #region Lifecycle
        void InitializeCycle()
        {
            // --- PreInitialize ---
            PerfMonitor.InitializeNetworkCounters();

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

                // --- PreUpdate ---
                lolibarVirtualDesktop.WorkspaceTabsListener(Config.BarWorkspacesContainer.Border);

                // --- Update ---
                config.Update();

                // --- PostUpdate ---
                Config.BarUserContainer.Update(Config.BarUserText);

                Config.BarCurProcContainer.Update(Config.BarCurProcText, Config.BarCurProcIcon);

                Config.BarCpuContainer.Update(Config.BarCpuText, Config.BarCpuIcon);
                Config.BarRamContainer.Update(Config.BarRamText, Config.BarRamIcon);
                Config.BarDiskContainer.Update(Config.BarDiskText, Config.BarDiskIcon);
                Config.BarNetworkContainer.Update(Config.BarNetworkText, Config.BarNetworkIcon);

                Config.BarTimeContainer.Update(Config.BarTimeText);

                Config.BarPowerContainer.Update(Config.BarPowerText, Config.BarPowerIcon);

            }
        }
        #endregion
    }
}
