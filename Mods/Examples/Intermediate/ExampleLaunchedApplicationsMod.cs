using System.Windows.Input;
using LolibarApp.Source.Tools;
using LolibarApp.Source;

namespace LolibarApp.Mods;

class ExampleLaunchedApplicationsMod : LolibarMod
{
    string FirefoxPath  = @"C:\Program Files\Mozilla Firefox\firefox.exe";
    string TelegramPath = @"C:\Users\supchyan\AppData\Roaming\Telegram Desktop\telegram.exe";

    LolibarContainer ApplicationsContainerParent    = new();
    LolibarContainer FirefoxApplicationContainer    = new();
    LolibarContainer TelegramApplicationContainer   = new();

    public override void PreInitialize() { }
    public override void Initialize()
    {
        // --- Apps Shortcuts ---

        ApplicationsContainerParent     = new()
        {
            Name                        = "ApplicationsContainerParent",
            Parent                      = Lolibar.BarRightContainer,
        };
        ApplicationsContainerParent.Create();

        FirefoxApplicationContainer     = new()
        {
            Name                        = "FirefoxApplicationContainer",
            Icon                        = LolibarProcess.GetAssociatedIcon(FirefoxPath),
            Parent                      = ApplicationsContainerParent.GetBody(),
            MouseLeftButtonUpEvent      = InvokeFirefox_Event
        };
        FirefoxApplicationContainer.Create();

        // This will handle some stuff belonged to firefox process (running / closed / active / bged / etc.)
        LolibarProcess.TranslateApplicationStateToContainer(FirefoxPath, FirefoxApplicationContainer);

        TelegramApplicationContainer    = new()
        {
            Name                        = "TelegramApplicationContainer",
            Icon                        = LolibarProcess.GetAssociatedIcon(TelegramPath),
            Parent                      = ApplicationsContainerParent.GetBody(),
            MouseLeftButtonUpEvent      = InvokeTelegram_Event
        };
        TelegramApplicationContainer.Create();

        // This will handle some stuff belonged to telegram process (running / closed / active / bged / etc.)
        LolibarProcess.TranslateApplicationStateToContainer(TelegramPath, TelegramApplicationContainer);
    }
    public override void Update()
    {
        // Update all translatable apps (which were in the Initialize() hook)
        LolibarProcess.FetchTranslatableApplications();
    }
    void InvokeFirefox_Event(object sender, MouseButtonEventArgs e)
    {
        // Run or Show app's process / window, when app on the background or hidden.
        LolibarProcess.InvokeApplicationByPath(FirefoxPath);
    }
    void InvokeTelegram_Event(object sender, MouseButtonEventArgs e)
    {
        // Run or Show app's process / window, when app on the background or hidden.
        LolibarProcess.InvokeApplicationByPath(TelegramPath);
    }
}
