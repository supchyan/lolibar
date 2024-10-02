using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace LolibarApp.Modding.Examples.ExampleAudioStreamController;
public class ExampleAudioStreamController
{
    // We will use this container as a parent for the other containers:
    static LolibarContainer BaseContainer;

    // --- Button containers ---
    static LolibarContainer PreviousButtonContainer;
    static LolibarContainer PlayButtonContainer;
    static LolibarContainer NextButtonContainer;

    // Info container. We will show information about audio stream here.
    // Means Title / Artist / etc.
    static LolibarContainer AudioInfoContainer;

    // To make things easier, let's combine
    // container's generate methods into the one.
    // Just call `ExampleAudioStreamController.Create()` in the ModClass.cs
    // to create all logic from this file.
    public static void Create()
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
            Icon = ModIcons.PreviousAudioIcon,
            MouseLeftButtonUpEvent = PreviousStreamCallEvent
        };
        PreviousButtonContainer.Create();

        PlayButtonContainer = new()
        {
            Name = "ExampleAudioPlayButton",
            Parent = parent,
            Icon = ModIcons.PlayAudioIcon, // just a placeholder, we will update it later
            MouseLeftButtonUpEvent = PlayStreamCallEvent
        };
        PlayButtonContainer.Create();

        NextButtonContainer = new()
        {
            Name = "ExampleAudioNextButton",
            Parent = parent,
            Icon = ModIcons.NextAudioIcon,
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

    // Local update method. We have to call it into `Update()` inside ModClass.cs
    // if we want to update information inside our example audio containers.
    public static void Update()
    {
        // Let's check if Audio Stream exists and playing.
        // Yes => Set icon to `PauseIcon`,
        // No  => Set icon to `PlayIcon`.
        PlayButtonContainer.Icon = LolibarAudio.IsPlaying() ? ModIcons.PauseAudioIcon : ModIcons.PlayAudioIcon;

        // Let's check if Stream's info exists.
        // Yes => Sets info equals audio title,
        // No  => Sets info equals "No Title".
        AudioInfoContainer.Text = LolibarAudio.StreamInfo?.Title ?? "Whale Audio";

        // Now, apply changes by updating containers:
        PlayButtonContainer.Update();
         AudioInfoContainer.Update();
    }

    // We will use these events to handle Audio Stream from the statusbar:
    static void PreviousStreamCallEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LolibarAudio.Previous();
    }
    static void PlayStreamCallEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (LolibarAudio.IsPlaying()) LolibarAudio.Pause();

        else LolibarAudio.Resume();
    }
    static void NextStreamCallEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        LolibarAudio.Next();
    }
}