using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;
using System.Windows.Media;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ShowcaseMod : LolibarMod
{
    #region Anime Stuff
    string OldAudioTitle = string.Empty;
    int OldAudioPlaybackState = -1;
    byte BrailleCodeAnimationFrame = 0;
    readonly string[] BrailleCodeAnimationFrames =
    [
        "⠋",
        "⠙",
        "⠸",
        "⠴",
        "⠦",
        "⠇",
    ];
    #endregion

    #region Icons
    readonly Geometry PlayAudioIcon = LolibarIcon.ParseSVG("./showcase/play.svg");
    readonly Geometry PauseAudioIcon = LolibarIcon.ParseSVG("./showcase/pause.svg");
    readonly Geometry PreviousAudioIcon = LolibarIcon.ParseSVG("./showcase/previous.svg");
    readonly Geometry NextAudioIcon = LolibarIcon.ParseSVG("./showcase/next.svg");
    #endregion

    #region Color Codes
    const string BaseColorCode = "#0c1013";
    const string TextColorCode = "#f0f4f6";
    const string AltTextColorCode = "#f5c28c";
    #endregion

    #region Containers
    LolibarContainer WorkspacesContainer = new();

    LolibarContainer DateTimeContainer = new();

    LolibarContainer AudioContainerParent = new();
    LolibarContainer PreviousButtonContainer = new();
    LolibarContainer PlayButtonContainer = new();
    LolibarContainer NextButtonContainer = new();
    LolibarContainer AudioInfoContainer = new();

    LolibarContainer BrailleCodeContainer = new();
    #endregion

    #region Body
    public override void PreInitialize()
    {
        BarUpdateDelay = 100;
        BarHeight = 40;
        BarColor = LolibarColor.FromHEX(BaseColorCode);
        BarContainersColor = LolibarColor.FromHEX(TextColorCode);
    }
    public override void Initialize()
    {
        // --- Left Side ---

        // --- Desktop Workspaces (Tabs) ---
        WorkspacesContainer = new()
        {
            Name = "WorkspacesContainer",
            Parent = Lolibar.BarLeftContainer,
            SeparatorPosition = LolibarEnums.SeparatorPosition.None,
            MouseWheelDelta = SwapWorkspacesByMouseWheelEvent
        };
        WorkspacesContainer.Create();

        LolibarVirtualDesktop.DrawWorkspacesInParent(
            parent: WorkspacesContainer.GetBody(),
            showDesktopNames: true
        );

        DateTimeContainer = new()
        {
            Name = "DateTimeContainer",
            Parent = Lolibar.BarLeftContainer,
            SeparatorPosition = LolibarEnums.SeparatorPosition.Left,
            MouseLeftButtonUp = OpenCalendarEvent
        };
        DateTimeContainer.Create();

        // --- Right Side ---

        // --- Audio Player ---
        AudioInfoContainer = new()
        {
            Name = "AudioInfoContainer",
            Parent = Lolibar.BarRightContainer,
            HasBackground = true,
            Color = LolibarColor.FromHEX(AltTextColorCode)
        };
        AudioInfoContainer.Create();

        AudioContainerParent = new()
        {
            Name = "AudioContainerParent",
            Parent = Lolibar.BarRightContainer,
        };
        AudioContainerParent.Create();

        PreviousButtonContainer = new()
        {
            Name = "AudioPreviousButton",
            Parent = AudioContainerParent.GetBody(),
            Icon = PreviousAudioIcon,
            Color = LolibarColor.FromHEX(AltTextColorCode),
            MouseLeftButtonUp = PreviousStreamCallEvent
        };
        PreviousButtonContainer.Create();

        PlayButtonContainer = new()
        {
            Name = "AudioPlayButton",
            Parent = AudioContainerParent.GetBody(),
            Color = LolibarColor.FromHEX(AltTextColorCode),
            MouseLeftButtonUp = PlayOrPauseStreamCallEvent
        };
        PlayButtonContainer.Create();

        NextButtonContainer = new()
        {
            Name = "AudioNextButton",
            Parent = AudioContainerParent.GetBody(),
            Icon = NextAudioIcon,
            Color = LolibarColor.FromHEX(AltTextColorCode),
            MouseLeftButtonUp = NextStreamCallEvent
        };
        NextButtonContainer.Create();

        BrailleCodeContainer = new()
        {
            Name = "BrailleCodeContainer",
            Parent = Lolibar.BarRightContainer,
            SeparatorPosition = LolibarEnums.SeparatorPosition.Left
        };
        BrailleCodeContainer.Create();
    }
    public override void Update()
    {
        // --- Properties ---
        BarWidth = Lolibar.Inch_Screen.X > 2 * BarMargin ? Lolibar.Inch_Screen.X - 2 * BarMargin : BarWidth;
        BarLeft = (Lolibar.Inch_Screen.X - BarWidth) / 2;

        // --- Date / Time ---
        DateTimeContainer.Text = $"{String.Format("{0:00}", DateTime.Now.Hour)}:{String.Format("{0:00}", DateTime.Now.Minute)} / {DateTime.Now.DayOfWeek}, {DateTime.Now.Day} {DateTime.Now.ToString("MMMM")} {DateTime.Now.Year}";
        DateTimeContainer.Update();

        // --- Audio Player ---
        PlayButtonContainer.Icon = LolibarAudio.IsPlaying ? PauseAudioIcon : PlayAudioIcon;
        PlayButtonContainer.Update();

        UseAudioTitleBlinkAnimation();

        var AudioTitle = LolibarAudio.MediaProperties?.Title ?? "";

        AudioInfoContainer.Text = AudioTitle == "" ? $"Nothing to play..." : $"{LolibarAudio.MediaProperties?.Title}";

        // Smooth opacity animtaion upon audio playback state change
        if (AudioInfoContainer.GetBody() != null && OldAudioPlaybackState != LolibarAudio.IsPlaying.GetHashCode())
        {
            if (LolibarAudio.IsPlaying)
            {
                LolibarAnimator.BeginIncOpacityAnimation(AudioInfoContainer.GetBody());
            }
            else
            {
                LolibarAnimator.BeginDecOpacityAnimation(AudioInfoContainer.GetBody());
            }
            OldAudioPlaybackState = LolibarAudio.IsPlaying.GetHashCode();
        }

        AudioInfoContainer.Update();

        BrailleCodeContainer.Text = BrailleCodeAnimation();
        BrailleCodeContainer.Update();
    }
    #endregion

    #region Events
    // date / time
    int OpenCalendarEvent(MouseButtonEventArgs e)
    {
        LolibarHelper.KeyDown(Keys.LWin);
        LolibarHelper.KeyDown(Keys.C);
        LolibarHelper.KeyUp(Keys.C);
        LolibarHelper.KeyUp(Keys.LWin);

        return 0;
    }

    // Audio Player
    int PreviousStreamCallEvent(MouseButtonEventArgs e)
    {
        LolibarAudio.Previous();

        return 0;
    }
    int PlayOrPauseStreamCallEvent(MouseButtonEventArgs e)
    {
        if (AudioInfoContainer.GetBody() == null)
        {
            return 0;
        }
        if (LolibarAudio.IsPlaying)
        {
            LolibarAudio.Pause();
        }
        else
        {
            LolibarAudio.Play();
        }

        return 0;
    }
    int NextStreamCallEvent(MouseButtonEventArgs e)
    {
        LolibarAudio.Next();

        return 0;
    }

    // RAM
    int SwapRamDisplayEvent(MouseButtonEventArgs e)
    {
        LolibarDefaults.SwapRamInfo();

        return 0;
    }

    // Desktop Workspaces
    int SwapWorkspacesByMouseWheelEvent(MouseWheelEventArgs e)
    {
        if (e.Delta > 0)
        {
            LolibarVirtualDesktop.GoToDesktopLeft();
        }

        if (e.Delta < 0)
        {
            LolibarVirtualDesktop.GoToDesktopRight();
        }

        return 0;
    }
    #endregion

    #region Anime Stuff Methods
    /// <summary>
    /// Some Braille code animation logic, just for the visuals.
    /// </summary>
    /// <returns></returns>
    string BrailleCodeAnimation()
    {
        BrailleCodeAnimationFrame++;
        if (BrailleCodeAnimationFrame >= BrailleCodeAnimationFrames.Length)
        {
            BrailleCodeAnimationFrame = 0;
            return BrailleCodeAnimationFrames[BrailleCodeAnimationFrame];
        }
        else
        {
            return BrailleCodeAnimationFrames[BrailleCodeAnimationFrame];
        }
    }
    /// <summary>
    /// My hook to blink audio title in the container whenever audio title has been changed.
    /// Like... visual audio streams transition effect.
    /// </summary>
    void UseAudioTitleBlinkAnimation()
    {
        if (AudioInfoContainer.GetBody() == null) return;

        if (OldAudioTitle != LolibarAudio.MediaProperties?.Title)
        {
            LolibarAnimator.BeginBlinkOpacityAnimation(AudioInfoContainer.GetBody());
            OldAudioTitle = LolibarAudio.MediaProperties?.Title ?? string.Empty;
        }
    }
    #endregion
}