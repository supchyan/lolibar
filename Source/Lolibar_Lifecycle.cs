using System.Windows;
using LolibarApp.Source.Tools;
using LolibarApp.Source.Mods;
namespace LolibarApp.Source
{
    partial class Lolibar
    {

        #region Lifecycle
        void PreInitialize()
        {
            // --- Setup properties ---

            Config.UseSystemTheme           = true;     // If true, statusbar's default colors will be affected by system's current theme
            Config.SnapToTop                = false;    // If true, snaps Lolibar to top of the screen
            Config.UpdateDelay              = 500;      // Delay for Update() method. Low delay affects performance!

            //

            // --- Global UI properties ---

            Config.BarMargin               = 8.0;
            Config.BarHeight               = 42.0;
            Config.BarWidth                = LolibarHelper.Inch_ScreenWidth - 2 * Config.BarMargin; // Fit to screen width as default

            Config.BarColor                = LolibarHelper.SetColor("#eeeeee");
            Config.BarBorderRadius         = 6.0;
            Config.BarOpacity              = 1.0;
            Config.BarStroke               = 0.0;

            Config.ElementColor            = LolibarHelper.SetColor("#2d2d2d");
            Config.ElementMargin           = new Thickness(16.0, 0.0, 16.0, 0.0);
            Config.IconSize                = 16.0;
            Config.FontSize                = 12.0;

            Config.SeparatorWidth          = 4.0;
            Config.SeparatorBorderRadius   = Config.SeparatorWidth / 2.0;

            //

            // --- Containers initialization ---

            Config.BarUserText             = "";

            Config.BarCurProcText          = "";
            Config.BarCurProcIcon          = LolibarDefaults.CurProcIcon;

            // ---

            Config.BarCpuText              = "";
            Config.BarCpuIcon              = LolibarDefaults.CpuIcon;

            Config.BarRamText              = "";
            Config.BarRamIcon              = LolibarDefaults.RamIcon;

            Config.BarDiskText             = "";
            Config.BarDiskIcon             = LolibarDefaults.GetDiskIcon();

            Config.BarNetworkText          = "";
            Config.BarNetworkIcon          = LolibarDefaults.GetNetworkIcon();

            // ---

            Config.BarSoundText            = "";
            Config.BarSoundIcon            = LolibarDefaults.SoundIcon;

            Config.BarPowerText            = "";
            Config.BarPowerIcon            = LolibarDefaults.GetPowerIcon();

            Config.BarTimeText             = ""; // No icon slot for this

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
            PostInitializeSnapping();
            UpdateResources();
        }

        
        async void UpdateCycle()
        {
            while (true)
            {
                await Task.Delay(Config.UpdateDelay);

                // --- PreUpdate ---
                UpdateDefaultInfo();

                // --- Update ---
                config.Update();

                // --- PostUpdate ---
                UpdateResources();
            }
        }
        #endregion
    }
}
