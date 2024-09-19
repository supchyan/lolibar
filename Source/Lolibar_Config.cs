using System.Windows;
using LolibarApp.Source.Tools;


// This is config part of the Lolibar Class. You can handle statusbar's content logic here
namespace LolibarApp.Source
{
    partial class Lolibar : Window
    {
        #region Config
        // Runs once after launch
        void Initialize()
        {
            // My personal setup, that fits with my UI surrounding (hope i'll remove it later)
            Resources["BarColor"]                       = LolibarHelper.SetColor("#121e46");
            Resources["ElementColor"]                   = LolibarHelper.SetColor("#8981bd");
            Resources["UseSystemTheme"]                 = false;
            //Resources["IsBarCenterContainerVisible"]    = false;
        }

        // Updates every "UpdateDelay".
        void Update()
        {

        }
        #endregion
    }
}
