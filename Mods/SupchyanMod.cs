using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;

namespace LolibarApp.Mods;

class SupchyanMod : LolibarMod
{
    #region Anime Stuff
    string OldAudioTitle                            = string.Empty;
    int OldAudioPlaybackState                       = -1;
    byte BrailleCodeAnimationFrame                  = 0;
    readonly string[] BrailleCodeAnimationFrames    = [ "⠋", "⠙", "⠸", "⠴", "⠦", "⠇" ];
    #endregion

    #region Icons
    readonly Geometry GearIcon                  = LolibarIcon.ParseSVG("./Defaults/gear.svg");
    readonly Geometry BellIcon                  = LolibarIcon.ParseSVG("./Defaults/bell.svg");
    readonly Geometry PlayAudioIcon             = LolibarIcon.ParseSVG("./Defaults/audio_play.svg");
    readonly Geometry PauseAudioIcon            = LolibarIcon.ParseSVG("./Defaults/audio_pause.svg");
    readonly Geometry PreviousAudioIcon         = LolibarIcon.ParseSVG("./Defaults/audio_rewind.svg");
    readonly Geometry NextAudioIcon             = LolibarIcon.ParseSVG("./Defaults/audio_next.svg");
    #endregion

    #region Color Codes
    const string PrimaryColorCode               = "#0d0d13";
    const string SecondaryColorCode             = "#573a91";
    const string TernaryColorCode               = "#406158";
    #endregion

    #region Containers
    LolibarContainer WorkspacesContainer        = new();


    LolibarContainer AudioContainerP            = new();
    LolibarContainer PreviousButtonContainer    = new();
    LolibarContainer PlayButtonContainer        = new();
    LolibarContainer NextButtonContainer        = new();

    LolibarContainer AudioInfoContainer         = new();

    LolibarContainer AppsContainerP             = new();

    LolibarContainer DateTimeContainer          = new();
    LolibarContainer LanguageContainer          = new();
    
    LolibarContainer PowerMonitorContainerP     = new();
    LolibarContainer PowerMonitorContainer      = new();

    LolibarContainer NotificationsContainerP    = new();
    LolibarContainer NotificationsContainer     = new();

    LolibarContainer QuickSettingsContainerP    = new();
    LolibarContainer QuickSettingsContainer     = new();

    #endregion

    #region Body
    public override void PreInitialize()
    {
        BarUpdateDelay              = 120;
        BarHeight                   = 40.0;

        BarSeparatorHeight          = 14.0;
        BarSeparatorWidth           = 3.0;
        BarSeparatorRadius          = 1.5;

        BarColor                    = LolibarColor.FromHEX(PrimaryColorCode);
        BarContainersColor          = LolibarColor.FromHEX(SecondaryColorCode);
    }
    public override void Initialize()
    {
        // --- Desktop Workspaces (Tabs) ---
        WorkspacesContainer = new()
        {
            Name = "WorkspacesContainer",
            Parent = Lolibar.BarLeftContainer,
            MouseWheelDelta = SwapWorkspacesByMouseWheel,
            SeparatorPosition = LolibarEnums.SeparatorPosition.Right,
        };
        WorkspacesContainer.Create();

        LolibarVirtualDesktop.DrawWorkspacesInParent
        (
            parent: WorkspacesContainer.GetBody(),
            showDesktopNames: true
        );

        // --- Audio Player ---
        AudioContainerP = new()
        {
            Name = "AudioContainerParent",
            Parent = Lolibar.BarLeftContainer,
        };
        AudioContainerP.Create();

        AudioInfoContainer = new()
        {
            Name = "AudioInfoContainer",
            Parent = AudioContainerP.GetBody(),
            HasBackground = true,
            Color = LolibarColor.FromHEX(TernaryColorCode)
        };
        AudioInfoContainer.Create();

        PreviousButtonContainer = new()
        {
            Name = "AudioPreviousButton",
            Parent = AudioContainerP.GetBody(),
            Icon = PreviousAudioIcon,
            MouseLeftButtonUp = Previous,
            Color = LolibarColor.FromHEX(TernaryColorCode)
        };
        PreviousButtonContainer.Create();

        PlayButtonContainer = new()
        {
            Name = "AudioPlayButton",
            Parent = AudioContainerP.GetBody(),
            MouseLeftButtonUp = PlayOrPause,
            Color = LolibarColor.FromHEX(TernaryColorCode)
        };
        PlayButtonContainer.Create();

        NextButtonContainer = new()
        {
            Name = "AudioNextButton",
            Parent = AudioContainerP.GetBody(),
            Icon = NextAudioIcon,
            MouseLeftButtonUp = Next,
            Color = LolibarColor.FromHEX(TernaryColorCode)
        };
        NextButtonContainer.Create();

        // --- Pinned Apps ---
        AppsContainerP              = new()
        {
            Name                    = "AppsContainerParent",
            Parent                  = Lolibar.BarRightContainer,
        };
        AppsContainerP.Create();

        LolibarProcess.AddPinnedAppsToContainer
        (
            parent: AppsContainerP.GetBody(),
            appContainerTitleState: LolibarEnums.AppContainerTitleState.OnlyActive
        );

        // --- Date / Time ---
        DateTimeContainer           = new()
        {
            Name                    = "DateTimeContainer",
            Parent                  = Lolibar.BarRightContainer,
            MouseLeftButtonUp       = OpenCalendar,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Both
        };
        DateTimeContainer.Create();

        // --- Language ---
        LanguageContainer           = new()
        {
            Name                    = "LanguageContainer",
            Parent                  = Lolibar.BarRightContainer,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Right
        };
        LanguageContainer.Create();

        // --- Power ---
        PowerMonitorContainerP = new()
        {
            Name = "PowerMonitorContainerParent",
            Parent = Lolibar.BarRightContainer,
            SeparatorPosition = LolibarEnums.SeparatorPosition.Right,
        };
        PowerMonitorContainerP.Create();

        PowerMonitorContainer       = new()
        {
            Name                    = "PowerMonitorContainer",
            Parent                  = PowerMonitorContainerP.GetBody(),
            MouseLeftButtonUp       = OpenPowerSettings,
            HasBackground           = true,
        };
        PowerMonitorContainer.Create();

        // --- Notifications ---
        NotificationsContainerP     = new()
        {
            Name                    = "NotificationsContainerParent",
            Parent                  = Lolibar.BarRightContainer,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Right,
        };
        NotificationsContainerP.Create();

        NotificationsContainer      = new()
        {
            Name                    = "NotificationsContainer",
            Parent                  = NotificationsContainerP.GetBody(),
            Icon                    = BellIcon,
            MouseLeftButtonUp       = OpenNotificationsOverlay,
            HasBackground           = true,
        };
        NotificationsContainer.Create();

        // --- Quick Settings ---
        QuickSettingsContainerP     = new()
        {
            Name                    = "QuickSettingsContainerParent",
            Parent                  = Lolibar.BarRightContainer,
        };
        QuickSettingsContainerP.Create();

        QuickSettingsContainer      = new()
        {
            Name                    = "QuickSettingsContainer",
            Parent                  = QuickSettingsContainerP.GetBody(),
            Icon                    = GearIcon,
            MouseLeftButtonUp       = OpenQuickSettingsOverlay,
            HasBackground           = true,
        };
        QuickSettingsContainer.Create();
    }
    public override void Update()
    {
        // Hide windows taskbar calls [ better to be in Update() hook ]
        LolibarHelper.HideWindowsTaskbar();

        // --- Auto resize logic ---
        (BarWidth, BarLeft) = LolibarHelper.OffsetLolibarToCenter(BarWidth, BarMargin);

        // --- Audio player ---
        PlayButtonContainer.Icon = LolibarAudio.IsPlaying ? PauseAudioIcon : PlayAudioIcon;
        PlayButtonContainer.Update();

        UseAudioTitleBlinkAnimation();

        var AudioTitle = LolibarAudio.MediaProperties?.Title.Truncate(50) ?? "";

        AudioInfoContainer.Text = LolibarAudio.IsPlaying ? $"{AudioTitle} {BrailleAudioPlayerAnimation()}" : AudioTitle;

        if (AudioTitle == "")
        {
            AudioInfoContainer.Text = $"///";
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

        // --- Date / Time ---
        DateTimeContainer.Text = $"{String.Format("{0:00}", DateTime.Now.Day)}.{String.Format("{0:00}", DateTime.Now.Month)} ({String.Format("{0:00}", DateTime.Now.Hour)}:{String.Format("{0:00}", DateTime.Now.Minute)})";
        DateTimeContainer.Update();

        // --- Language ---
        LanguageContainer.Text = LolibarStats.CurrentInputLanguage?.Split(" (")[0].ToUpper()[..3];
        LanguageContainer.Update();

        // --- Power ---
        //PowerMonitorContainer.Text = LolibarStats.IsBatteryCharging ? $"⚡️{LolibarStats.PowerInPercent}" : LolibarStats.PowerInPercent;
        PowerMonitorContainer.Icon = LolibarStats.SmartPowerIcon;
        PowerMonitorContainer.Update();
    }
    #endregion

    #region Click events
    // --- Date / Time ---
    int OpenCalendar(MouseButtonEventArgs e)
    {
        LolibarHelper.KeyDown(Keys.LWin);
        LolibarHelper.KeyDown(Keys.C);
        LolibarHelper.KeyUp(Keys.C);
        LolibarHelper.KeyUp(Keys.LWin);

        return 0;
    }

    // --- Quick Settings ---
    int OpenQuickSettingsOverlay(MouseButtonEventArgs e)
    {
        LolibarHelper.KeyDown(Keys.LWin);
        LolibarHelper.KeyDown(Keys.A);
        LolibarHelper.KeyUp(Keys.A);
        LolibarHelper.KeyUp(Keys.LWin);

        return 0;
    }

    // --- Notifications ---
    int OpenNotificationsOverlay(MouseButtonEventArgs e)
    {
        LolibarHelper.KeyDown(Keys.LWin);
        LolibarHelper.KeyDown(Keys.N);
        LolibarHelper.KeyUp(Keys.N);
        LolibarHelper.KeyUp(Keys.LWin);

        return 0;
    }

    // --- Power ---
    int OpenPowerSettings(MouseButtonEventArgs e)
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

        return 0;
    }

    // --- Desktop Workspaces ---
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
    int SwapWorkspacesByMouseWheel(MouseWheelEventArgs e)
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