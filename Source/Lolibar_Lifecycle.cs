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

            Config.UseSystemTheme               = true;     // If true, statusbar's default colors will be affected by system's current theme
            Config.SnapBarToTop                 = true;    // If true, snaps Lolibar to top of the screen
            Config.UpdateDelay                  = 1000;      // Delay for Update() method. Low delay affects performance!

            //

            // --- Global UI properties ---

            Config.BarMargin                    = 8.0;
            Config.BarHeight                    = 42.0;
            Config.BarWidth                     = LolibarHelper.Inch_ScreenWidth - 2 * Config.BarMargin; // Fit to screen width as default

            Config.BarColor                     = LolibarHelper.SetColor("#2f2f2f");
            Config.BarCornerRadius              = new CornerRadius(6.0);
            Config.BarOpacity                   = 1.0;
            Config.BarStrokeThickness           = new Thickness(0.0, 0.0, 0.0, 0.0);

            Config.BarContainersContentColor    = LolibarHelper.SetColor("#55b456");
            
            Config.BarContainerMargin           = new Thickness(10.0, 0.0, 10.0, 0.0);
            Config.BarContainerInnerMargin      = new Thickness(9.0, 5.0, 9.0, 5.0);
            Config.BarContainersContentMargin   = new Thickness(5.0, 0.0, 5.0, 0.0);

            Config.BarIconSize                  = 16.0;
            Config.BarIconSizeSmall             = 10.0;

            Config.BarFontSize                  = 12.0;

            Config.BarSeparatorWidth            = 4.0;
            Config.BarSeparatorHeight           = 16.0;
            Config.BarSeparatorRadius           = Config.BarSeparatorWidth / 2.0;

            Config.BarWorkspacesMargin          = new Thickness(8.0, 0.0, 8.0, 0.0);

            Config.BarContainersCornerRadius    = new CornerRadius(3.0);

            Config.BarContainerColor            = System.Windows.Media.Brushes.Transparent;

            //

            // --- Containers initialization ---

            Config.BarUserText                  = string.Empty;

            Config.BarCurProcText               = string.Empty;
            Config.BarCurProcIcon               = LolibarDefaults.CurProcIcon;

            // ---

            Config.BarCpuText                   = string.Empty;
            Config.BarCpuIcon                   = LolibarDefaults.CpuIcon;

            Config.BarRamText                   = string.Empty;
            Config.BarRamIcon                   = LolibarDefaults.RamIcon;

            Config.BarDiskText                  = string.Empty;
            Config.BarDiskIcon                  = LolibarDefaults.GetDiskIcon();

            Config.BarNetworkText               = string.Empty;
            Config.BarNetworkIcon               = LolibarDefaults.GetNetworkIcon();

            Config.BarAddWorkspaceText          = string.Empty;

            // ---

            Config.BarPowerText                 = string.Empty;
            Config.BarPowerIcon                 = LolibarDefaults.GetPowerIcon();

            Config.BarTimeText                  = string.Empty;

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
            lolibarVirtualDesktop.SetEventsToAddWorkspaceContainer(BarAddWorkspaceContainer);
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
