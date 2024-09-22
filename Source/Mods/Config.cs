using LolibarApp.Source.Tools;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using LolibarApp.Source;

// You can handle statusbar's content logic here
namespace LolibarApp.Source.Mods
{
    partial class Config : ModLolibar
    {
        // Runs once after launch
        public override void Initialize()
        {
            //BarCenterContainer.Visibility = Visibility.Collapsed;
            // My personal setup, that fits with my UI surrounding [ remove it later ]
            UseSystemTheme = false;
            UpdateDelay = 500;
            BarColor = LolibarHelper.SetColor("#c2b99e");
            ElementColor = LolibarHelper.SetColor("#47464c");
        }

        // Updates every "UpdateDelay".
        public override void Update()
        {

        }
    }
}
