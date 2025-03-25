using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;
using System.Windows.Media;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleAudioPlayerMod : LolibarMod
{
    // This mod represents capabilities to handle audio streams in your OS.
    // Lolibar containes library called `LolibarAudio`, so this mod about it.

    // We will use this container as a parent for other containers:
    LolibarContainer? BaseContainer             = new();

    // --- Button containers ---
    LolibarContainer? PreviousButtonContainer   = new();
    LolibarContainer? PlayButtonContainer       = new();
    LolibarContainer? NextButtonContainer       = new();

    // Info container. We will show information about audio stream here.
    // Means Title / Artist / etc.
    LolibarContainer? AudioInfoContainer        = new();

    // Define these icons, we need it later:
    Geometry playIcon   = LolibarIcon.ParseSVG("./Examples/ExampleAudioPlayerMod/play.svg");
    Geometry pauseIcon  = LolibarIcon.ParseSVG("./Examples/ExampleAudioPlayerMod/pause.svg");

    public override void PreInitialize() { }
    public override void Initialize()
    {
        BaseContainer           = new()
        {
            Name                = "ExampleAudioBaseContainer",
            Parent              = Lolibar.BarCenterContainer,
        };
        BaseContainer.Create();

        PreviousButtonContainer = new()
        {
            Name                = "ExampleAudioPreviousButton",
            Parent              = BaseContainer.GetBody(), // Use BaseContainer as a parent here
            Icon                = LolibarIcon.ParseSVG("./Examples/ExampleAudioPlayerMod/previous.svg"),
            MouseLeftButtonUp   = Previous
        };
        PreviousButtonContainer.Create();

        PlayButtonContainer = new()
        {
            Name = "ExampleAudioPlayButton",
            Parent = BaseContainer.GetBody(),
            MouseLeftButtonUp = PlayOrPause
        };
        PlayButtonContainer.Create();

        NextButtonContainer = new()
        {
            Name = "ExampleAudioNextButton",
            Parent = BaseContainer.GetBody(),
            Icon = LolibarIcon.ParseSVG("./Examples/ExampleAudioPlayerMod/next.svg"),
            MouseLeftButtonUp = Next
        };
        NextButtonContainer.Create();

        AudioInfoContainer = new()
        {
            Name = "ExampleAudioInfoContainer",
            // I don't want to mix controls and the audio info,
            // so let's set a default center container as a parent for this one:
            Parent = Lolibar.BarCenterContainer,
            // And let's separate it via background:
            HasBackground = true,
        };
        AudioInfoContainer.Create();
    }
    public override void Update()
    {
        if (PlayButtonContainer != null)
        {
            // Let's check if Audio Stream exists and playing.
            // Yes => Set icon to `PauseAudioIcon`,
            // No  => Set icon to `PlayAudioIcon`.
            PlayButtonContainer.Icon = LolibarAudio.IsPlaying ? pauseIcon : playIcon;

            // Update button container
            PlayButtonContainer.Update();
        }

        if (AudioInfoContainer != null)
        {
            // Let's check if Stream's info exists.
            // Yes => Sets Text content equals to audio title,
            // No  => Sets Text content equals to "No Audio".
            AudioInfoContainer.Text = LolibarAudio.MediaProperties?.Title ?? "No Audio";

            // Update info container
            AudioInfoContainer.Update();
        }
    }
    int Previous(MouseButtonEventArgs e)
    {
        LolibarAudio.Previous();
        return 0; 
    }
    int PlayOrPause(MouseButtonEventArgs e)
    {
        LolibarAudio.PlayOrPause();
        return 0;
    }
    int Next(MouseButtonEventArgs e)
    {
        LolibarAudio.Next();
        return 0;
    }
}
