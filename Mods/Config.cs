using LolibarApp.Source.Tools;
using LolibarApp.Source;
using System.Diagnostics;

// Handle statusbar's content logic here.
// As for an example, you can see my personal setup, which fits my needs.
namespace LolibarApp.Mods
{
    class Config : ModLolibar
    {
        // Runs once after launch
        public override void Initialize()
        {
            UpdateDelay                 = 500;
            UseSystemTheme              = false;

            BarHeight                   = 36;

            BarColor                    = LolibarHelper.SetColor("#452a25");
            BarContainersContentColor   = LolibarHelper.SetColor("#b56e5c");

            HideBarInfoContainer        = true;

            // Let's add a clickable container!
            ContainerGenerator.CreateContainer(
                Lolibar.barRightContainer, // parent container
                LolibarDefaults.SoundIcon, // icon content
                "Sound",                   // text content
                OpenSoundSettings,         // onleftclick event
                default                    // onrightclick event [ which is null here ]
            );
        }

        // Updates every "UpdateDelay".
        public override void Update()
        {
            BarUserText = $"🐳";
            BarTimeText = $"{ DateTime.Now.Day } / { DateTime.Now.Month } / { DateTime.Now.Year } { DateTime.Now.DayOfWeek }";
        }

        // My custom event...
        void OpenSoundSettings(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new Process
            {
                StartInfo = new()
                {
                    FileName = "powershell.exe",
                    Arguments = "Start-Process ms-settings:sound",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            }.Start();
        }
    }
}
