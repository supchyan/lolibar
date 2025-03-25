using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleDateTimeMod : LolibarMod
{
    LolibarContainer DateTimeContainer = new();

    public override void PreInitialize() { }
    public override void Initialize()
    {
        DateTimeContainer       = new()
        {
            Name                = "ExampleDateTimeContainer",
            Parent              = Lolibar.BarLeftContainer,
            MouseLeftButtonUp   = OpenTimeSettingsEvent
        };
        DateTimeContainer.Create();
    }
    public override void Update()
    {
        // Looks heavy, but it's just a default c# String.Format() method, which is formats time in the way like windows do.
        // For example:
        // Instead of: 6.6.25, you will get 06.06.25, which is better, don't you think? The same for time.
        var date = $"{DateTime.Now.DayOfWeek}, {String.Format("0:00", DateTime.Now.Day)}.{String.Format("0:00", DateTime.Now.Month)}.{DateTime.Now.Year}";
        
        var time = $"{String.Format("0:00", DateTime.Now.Hour)}:{String.Format("0:00", DateTime.Now.Minute)}";
        
        DateTimeContainer.Text = $"{time} {date}";
        DateTimeContainer.Update();
    }

    static int OpenTimeSettingsEvent(System.Windows.Input.MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo               = new()
            {
                FileName            = "powershell.exe",
                Arguments           = "Start-Process ms-settings:dateandtime",
                UseShellExecute     = false,
                CreateNoWindow      = true,
            }
        }.Start();

        return 0;
    }
}

