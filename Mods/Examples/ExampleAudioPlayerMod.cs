using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Controls;
using System.Windows.Media;

//namespace LolibarApp.Mods;

class ExampleAudioPlayerMod : LolibarMod
{
    // This mod represents capabilities to handle audio streams in your OS.
    // Lolibar containes library called `LolibarAudio`, so this mod about it.

    // --- Custom Icons ---
    Geometry PlayAudioIcon = Geometry.Parse("M2.0764 9.71553C1.22088 10.4124 0 9.74943 0 8.58791V1.41209C0 0.250579 1.22088 -0.412449 2.0764 0.284466L6.4809 3.87237C7.17303 4.43619 7.17304 5.56381 6.4809 6.12763L2.0764 9.71553Z");
    Geometry PauseAudioIcon = Geometry.Parse("M1.16667 10C0.522334 10 0 9.44036 0 8.75V1.25C0 0.559644 0.522334 0 1.16667 0C1.811 0 2.33333 0.559644 2.33333 1.25V8.75C2.33333 9.44036 1.811 10 1.16667 10ZM5.83333 10C5.189 10 4.66666 9.44036 4.66666 8.75V1.25C4.66666 0.559644 5.189 0 5.83333 0C6.47766 0 6.99999 0.559644 6.99999 1.25V8.75C6.99999 9.44036 6.47766 10 5.83333 10Z");
    Geometry PreviousAudioIcon = Geometry.Parse("M1 0C1.55228 0 2 0.559644 2 1.25V8.75C2 9.44036 1.55228 10 1 10C0.447715 10 0 9.44036 0 8.75V1.25C0 0.559644 0.447715 0 1 0ZM7.47 0.210064C8.0168 0.592995 8.16456 1.36906 7.80003 1.94346L5.86022 5L7.80003 8.05654C8.16456 8.63094 8.0168 9.40701 7.47 9.78994C6.9232 10.1729 6.18441 10.0177 5.81988 9.44326L3 5L5.81988 0.556743C6.18441 -0.0176529 6.9232 -0.172866 7.47 0.210064Z");
    Geometry NextAudioIcon = Geometry.Parse("M7 10C6.44772 10 6 9.44036 6 8.75L6 1.25C6 0.559643 6.44772 0 7 0C7.55228 0 8 0.559643 8 1.25L8 8.75C8 9.44036 7.55228 10 7 10ZM0.529999 9.78994C-0.0168047 9.40701 -0.164562 8.63094 0.199974 8.05654L2.13978 5L0.199974 1.94346C-0.164562 1.36906 -0.0168047 0.592995 0.529999 0.210064C1.0768 -0.172867 1.81559 -0.0176525 2.18012 0.556743L5 5L2.18012 9.44326C1.81559 10.0177 1.0768 10.1729 0.529999 9.78994Z");

    // We will use this container as a parent for the other containers:
    LolibarContainer? BaseContainer;

    // --- Button containers ---
    LolibarContainer? PreviousButtonContainer;
    LolibarContainer? PlayButtonContainer;
    LolibarContainer? NextButtonContainer;

    // Info container. We will show information about audio stream here.
    // Means Title / Artist / etc.
    LolibarContainer? AudioInfoContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        BaseContainer = new()
        {
            Name = "ExampleAudioBaseContainer",
            // We should to set a parent for this container to draw it,
            // so in this case, let's choose default center container.
            Parent = Lolibar.BarCenterContainer,
        };
        BaseContainer.Create();

        // Setup local parent component for the containers.
        // It should be StackPanel, which always is a Child of the BorderComponent,
        // so let's get it:
        var parent = (StackPanel)BaseContainer.BorderComponent.Child;

        PreviousButtonContainer = new()
        {
            Name = "ExampleAudioPreviousButton",
            // Now we can use BaseContainer as a parent:
            Parent = parent,
            Icon = PreviousAudioIcon,
            MouseLeftButtonUpEvent = PreviousStreamCallEvent
        };
        PreviousButtonContainer.Create();

        PlayButtonContainer = new()
        {
            Name = "ExampleAudioPlayButton",
            Parent = parent,
            Icon = PlayAudioIcon, // just a placeholder, we will update it later
            MouseLeftButtonUpEvent = PlayStreamCallEvent
        };
        PlayButtonContainer.Create();

        NextButtonContainer = new()
        {
            Name = "ExampleAudioNextButton",
            Parent = parent,
            Icon = NextAudioIcon,
            MouseLeftButtonUpEvent = NextStreamCallEvent
        };
        NextButtonContainer.Create();

        AudioInfoContainer = new()
        {
            Name = "ExampleAudioInfoContainer",
            // I don't want to mix controls and the audio info,
            // so let's set default center container as a parent here:
            Parent = Lolibar.BarCenterContainer,
            Text = "-", // just a placeholder, we will update it later
            HasBackground = true, // adds background to make this container more visible on the statusbar
        };
        AudioInfoContainer.Create();
    }
    public override void Update()
    {
        // Let's check if Audio Stream exists and playing.
        // Yes => Set icon to `PauseIcon`,
        // No  => Set icon to `PlayIcon`.
        PlayButtonContainer.Icon = LolibarAudio.IsPlaying() ? PauseAudioIcon : PlayAudioIcon;

        // Let's check if Stream's info exists.
        // Yes => Sets info equals audio title,
        // No  => Sets info equals "No Title".
        AudioInfoContainer.Text = LolibarAudio.StreamInfo?.Title ?? "Whale Audio";

        // Now, apply changes, updating specified containers:
        PlayButtonContainer.Update();
        AudioInfoContainer.Update();
    }

    // We will use these events to handle Audio Stream from the statusbar:
    void PreviousStreamCallEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LolibarAudio.Previous();
    }
    void PlayStreamCallEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (LolibarAudio.IsPlaying()) LolibarAudio.Pause();

        else LolibarAudio.Resume();
    }
    void NextStreamCallEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LolibarAudio.Next();
    }
}