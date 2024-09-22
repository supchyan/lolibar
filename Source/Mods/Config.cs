using LolibarApp.Source.Tools;

// You can handle statusbar's content logic here
namespace LolibarApp.Source.Mods
{
    class Config : ModLolibar
    {
        // Runs once after launch
        public override void Initialize()
        {
            // My personal setup, which fits my wallpaper
            UpdateDelay = 500;
            UseSystemTheme = false;
            BarColor = LolibarHelper.SetColor("#08121b");
            ElementColor = LolibarHelper.SetColor("#429ec3");
        }

        // Updates every "UpdateDelay".
        public override void Update()
        {
            BarUserText = "🐳";
            BarTimeText = $"{ DateTime.Now.Day } / { DateTime.Now.Month } / { DateTime.Now.Year } { DateTime.Now.DayOfWeek }";
        }
    }
}
