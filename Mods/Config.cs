using LolibarApp.Source.Tools;

// Handle statusbar's content logic here.
// As for an example, you can see my personal setup, which fits my needs.
namespace LolibarApp.Mods
{
    class Config : ModLolibar
    {
        // Runs once after launch
        public override void Initialize()
        {
            UpdateDelay = 500;
            UseSystemTheme = false;
            BarColor = LolibarHelper.SetColor("#05202d");
            ElementColor = LolibarHelper.SetColor("#e2968b");
            BarHeight = 36;
        }

        // Updates every "UpdateDelay".
        public override void Update()
        {
            BarUserText = "🐳";
            BarTimeText = $"{ DateTime.Now.Day } / { DateTime.Now.Month } / { DateTime.Now.Year } { DateTime.Now.DayOfWeek }";
        }
    }
}
