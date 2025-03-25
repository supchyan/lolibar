using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;

namespace LolibarApp.Mods;

class SupchyanMod : LolibarMod
{
    #region Anime Stuff
    string OldAudioTitle                            = string.Empty;
    int OldAudioPlaybackState                       = -1;
    byte BrailleCodeAnimationFrame                  = 0;
    readonly string[] BrailleCodeAnimationFrames    =
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
    readonly Geometry PlayAudioIcon      = LolibarIcon.ParseSVG("./supchyan/play.svg");
    readonly Geometry PauseAudioIcon     = LolibarIcon.ParseSVG("./supchyan/pause.svg");
    readonly Geometry PreviousAudioIcon  = LolibarIcon.ParseSVG("./supchyan/prev.svg");
    readonly Geometry NextAudioIcon      = LolibarIcon.ParseSVG("./supchyan/next.svg");
    #endregion

    #region Color Codes
    const string PrimaryColorCode     = "#272426";
    const string SecondaryColorCode   = "#c4a2a0";
    const string TernaryColorCode     = "#9a7a7b";
    #endregion

    #region Containers
    LolibarContainer DateTimeContainerParent    = new();
    LolibarContainer DateTimeContainer          = new();

    LolibarContainer AudioContainerParent       = new();
    LolibarContainer PreviousButtonContainer    = new();
    LolibarContainer PlayButtonContainer        = new();
    LolibarContainer NextButtonContainer        = new();
    LolibarContainer AudioInfoContainer         = new();

    LolibarContainer PowerMonitorContainer      = new();

    LolibarContainer WorkspacesContainer        = new();
    #endregion

    #region Body
    public override void PreInitialize()
    {
        BarUpdateDelay              = 120;
        BarHeight                   = 40;
        BarColor                    = LolibarHelper.SetColor(PrimaryColorCode);
        BarContainersColor          = LolibarHelper.SetColor(SecondaryColorCode);
    }
    public override void Initialize()
    {
        // --- Date / Time ---
        DateTimeContainerParent     = new()
        {
            Name                    = "DateTimeContainerParent",
            Parent                  = Lolibar.BarLeftContainer,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Right,
        };
        DateTimeContainerParent.Create();

        DateTimeContainer = new()
        {
            Name                    = "DateTimeContainer",
            Parent                  = DateTimeContainerParent.GetBody(),
            MouseLeftButtonUpEvent  = OpenCalendarEvent,
            HasBackground           = true,
        };
        DateTimeContainer.Create();

        // --- Power ---
        PowerMonitorContainer       = new()
        {
            Name                    = "PowerMonitorContainer",
            Parent                  = Lolibar.BarLeftContainer,
            MouseLeftButtonUpEvent  = OpenPowerSettingsEvent
        };
        PowerMonitorContainer.Create();

        // --- Audio Player ---
        AudioContainerParent        = new()
        {
            Name                    = "AudioContainerParent",
            Parent                  = Lolibar.BarCenterContainer,
        };
        AudioContainerParent.Create();

        PreviousButtonContainer     = new()
        {
            Name                    = "AudioPreviousButton",
            Parent                  = AudioContainerParent.GetBody(),
            Icon                    = PreviousAudioIcon,
            MouseLeftButtonUpEvent  = PreviousStreamCallEvent
        };
        PreviousButtonContainer.Create();

        PlayButtonContainer         = new()
        {
            Name                    = "AudioPlayButton",
            Parent                  = AudioContainerParent.GetBody(),
            MouseLeftButtonUpEvent  = PlayOrPauseStreamCallEvent,
        };
        PlayButtonContainer.Create();

        NextButtonContainer         = new()
        {
            Name                    = "AudioNextButton",
            Parent                  = AudioContainerParent.GetBody(),
            Icon                    = NextAudioIcon,
            MouseLeftButtonUpEvent  = NextStreamCallEvent
        };
        NextButtonContainer.Create();

        AudioInfoContainer          = new()
        {
            Name                    = "AudioInfoContainer",
            Parent                  = Lolibar.BarCenterContainer,
            HasBackground           = true,
            Color                   = LolibarHelper.SetColor(TernaryColorCode)
        };
        AudioInfoContainer.Create();

        // --- Desktop Workspaces (Tabs) ---
        WorkspacesContainer         = new()
        {
            Name                    = "WorkspacesContainer",
            Parent                  = Lolibar.BarRightContainer,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Left,
            MouseWheelEvent         = SwapWorkspacesByMouseWheelEvent
        };
        WorkspacesContainer.Create();

        LolibarVirtualDesktop.DrawWorkspacesInParent(
            parent:             WorkspacesContainer.GetBody(),
            showDesktopNames:   true
        );
    }
    public override void Update() 
    {
        // --- Auto resize logic ---
        BarWidth = Lolibar.Inch_Screen.X > 2 * BarMargin ? Lolibar.Inch_Screen.X  - 2 * BarMargin : BarWidth;
        BarLeft  = (Lolibar.Inch_Screen.X - BarWidth) / 2;

        // --- Date / Time ---
        DateTimeContainer.Text = $"{String.Format("{0:00}", DateTime.Now.Day)}.{String.Format("{0:00}", DateTime.Now.Month)}.{DateTime.Now.Year} ({String.Format("{0:00}", DateTime.Now.Hour)}:{String.Format("{0:00}", DateTime.Now.Minute)})";
        DateTimeContainer.Update();

        // --- Audio player ---
        PlayButtonContainer.Icon = LolibarAudio.IsPlaying ? PauseAudioIcon : PlayAudioIcon;
        PlayButtonContainer.Update();

        UseAudioTitleBlinkAnimation();

        var AudioTitle = LolibarAudio.MediaProperties?.Title.Truncate(50) ?? "";

        AudioInfoContainer.Text = LolibarAudio.IsPlaying ? $"{AudioTitle} {BrailleAudioPlayerAnimation()}" : AudioTitle;

        if (AudioTitle == "")
        {
            AudioInfoContainer.Text = $"U_U";
        }

        // Smooth opacity animtaion upon audio playback state change
        if (OldAudioPlaybackState != LolibarAudio.IsPlaying.GetHashCode())
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

        // --- Power ---
        PowerMonitorContainer.Text = LolibarDefaults.GetPowerInfo();
        PowerMonitorContainer.Icon = LolibarDefaults.GetPowerIcon();
        PowerMonitorContainer.Update();
    }
    #endregion

    #region Events
    //user
    void OpenUserSettingsEvent(object sender, MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName        = "powershell.exe",
                Arguments       = "Start-Process ms-settings:accounts",
                UseShellExecute = false,
                CreateNoWindow  = true,
            }
        }.Start();
    }

    // date / time
    void OpenCalendarEvent(object sender, MouseButtonEventArgs e)
    {
        LolibarHelper.KeyDown(Keys.LWin);
        LolibarHelper.KeyDown(Keys.C);
        LolibarHelper.KeyUp(Keys.C);
        LolibarHelper.KeyUp(Keys.LWin);
    }
    void OpenTimeSettingsEvent(object sender, MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName        = "powershell.exe",
                Arguments       = "Start-Process ms-settings:dateandtime",
                UseShellExecute = false,
                CreateNoWindow  = true,
            }
        }.Start();
    }

    // audio player
    void PreviousStreamCallEvent(object sender, MouseButtonEventArgs e)
    {
        LolibarAudio.Previous();
    }
    void PlayOrPauseStreamCallEvent(object sender, MouseButtonEventArgs e)
    {
        if (AudioInfoContainer.GetBody() == null)
        {
            return;
        }
        LolibarAudio.PlayOrPause();
    }
    void NextStreamCallEvent(object sender, MouseButtonEventArgs e)
    {
        LolibarAudio.Next();
    }

    // power
    void OpenPowerSettingsEvent(object sender, MouseButtonEventArgs e)
    {
        new Process
        {
            StartInfo = new()
            {
                FileName        = "powershell.exe",
                Arguments       = "Start-Process ms-settings:batterysaver",
                UseShellExecute = false,
                CreateNoWindow  = true,
            }
        }.Start();
    }

    // desktop workspaces
    void SwapWorkspacesByMouseWheelEvent(object sender, MouseWheelEventArgs e)
    {
        if (e.Delta > 0)
        {
            LolibarVirtualDesktop.GoToDesktopLeft();
        }

        if (e.Delta < 0)
        {
            LolibarVirtualDesktop.GoToDesktopRight();
        }
    }
    #endregion

    #region Anime Stuff Methods
    /// <summary>
    /// This is my placeholder animation for the Audio Player when no audio stream in it.
    /// </summary>
    /// <returns></returns>
    string BrailleAudioPlayerAnimation()
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
        if (AudioInfoContainer.GetBody() == null || LolibarAudio.MediaProperties?.Title == null) return;

        if (OldAudioTitle != LolibarAudio.MediaProperties?.Title)
        {
            LolibarAnimator.BeginBlinkOpacityAnimation(AudioInfoContainer.GetBody());
            OldAudioTitle = LolibarAudio.MediaProperties?.Title ?? string.Empty;
        }
    }
    #endregion
}