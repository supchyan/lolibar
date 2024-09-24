using System.Windows;
using LolibarApp.Source.Tools;
using LolibarApp.Mods;
namespace LolibarApp.Source
{
    partial class Lolibar
    {

        #region Lifecycle
        void PreInitialize()
        {
            // --- Setup properties ---

            Config.UseSystemTheme           = true;     // If true, statusbar's default colors will be affected by system's current theme
            Config.SnapBarToTop             = false;    // If true, snaps Lolibar to top of the screen
            Config.UpdateDelay              = 1000;      // Delay for Update() method. Low delay affects performance!

            //

            // --- Global UI properties ---

            Config.BarMargin                    = 8.0;
            Config.BarHeight                    = 42.0;
            Config.BarWidth                     = LolibarHelper.Inch_ScreenWidth - 2 * Config.BarMargin; // Fit to screen width as default

            Config.BarColor                     = LolibarHelper.SetColor("#2f2f2f");
            Config.BarBorderRadius              = 6.0;
            Config.BarOpacity                   = 1.0;
            Config.BarStrokeSize                = 0.0;

            Config.BarElementColor              = LolibarHelper.SetColor("#55b456");
            
            Config.BarElementMargin             = new Thickness(16.0, 0.0, 16.0, 0.0);
            
            Config.BarIconSize                  = 16.0;
            Config.BarIconSizeSmall             = 10.0;

            Config.BarFontSize                  = 12.0;

            Config.BarSeparatorWidth            = 4.0;
            Config.BarSeparatorHeight           = 16.0;
            Config.BarSeparatorBorderRadius     = Config.BarSeparatorWidth / 2.0;

            Config.BarWorkspacesMargin          = new Thickness(8.0, 0.0, 8.0, 0.0);
            Config.BarWorkspacesInnerMargin     = new Thickness(8.0, 5.0, 8.0, 5.0);
            Config.BarWorkspacesBorderRadius    = 2.0;

            //

            // --- Containers initialization ---

            Config.BarAddTabText            = "";

            Config.BarUserText              = "";

            Config.BarCurProcText           = "";
            Config.BarCurProcIcon           = LolibarDefaults.CurProcIcon;

            // ---

            Config.BarCpuText               = "";
            Config.BarCpuIcon               = LolibarDefaults.CpuIcon;

            Config.BarRamText               = "";
            Config.BarRamIcon               = LolibarDefaults.RamIcon;

            Config.BarDiskText              = "";
            Config.BarDiskIcon              = LolibarDefaults.GetDiskIcon();

            Config.BarNetworkText           = "";
            Config.BarNetworkIcon           = LolibarDefaults.GetNetworkIcon();

            // ---

            Config.BarSoundText             = "";
            Config.BarSoundIcon             = LolibarDefaults.SoundIcon;

            Config.BarPowerText             = "";
            Config.BarPowerIcon             = LolibarDefaults.GetPowerIcon();

            Config.BarTimeText              = ""; // No icon slot for this

            //
        }
        void InitializeCycle()
        {
            // --- PreInitialize ---
            PreInitialize();
            PerfMonitor.InitializeNetworkCounters();

            // --- Initialize ---
            config.Initialize();

            // -- PostInitialize ---
            PostInitializeContainersVisibility();
            PostInitializeSnapping();
            UpdateResources();
            lolibarVirtualDesktop.SetEventsToAddTabContainer(BarAddTabContainer);
        }

        
        async void UpdateCycle()
        {
            while (true)
            {
                await Task.Delay(Config.UpdateDelay);

                // --- PreUpdate ---
                UpdateDefaultInfo();
                lolibarVirtualDesktop.WorkspaceTabsListener(BarWorkspacesContainerTabs);

                // --- Update ---
                config.Update();

                // --- PostUpdate ---
                UpdateResources();
            }
        }
        #endregion
    }
}
