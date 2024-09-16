using lolibar.tools;
using System.Windows;

// This is config part of the Lolibar Class. You can handle statusbar's content logic here
namespace lolibar
{
    partial class Lolibar : Window
    {
        #region Config
        // Runs once after launch
        void Initialize()
        {
            // My personal setup, that fits with my UI surrounding (hope i'll remove it later)
            // Resources["BarColor"]       = LolibarHelper.SetColor("#121e46");
            // Resources["ElementColor"]   = LolibarHelper.SetColor("#8981bd");
            Resources["BarCenterContainerIsVisible"] = false;
        }

        // Updates every "UpdateDelay".
        void Update()
        {

        }
        #endregion
    }
}
