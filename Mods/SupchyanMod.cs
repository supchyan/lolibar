using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

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
    readonly Geometry AppsIcon                  = LolibarIcon.ParseSVG("./Defaults/apps.svg");
    readonly Geometry BellIcon                  = LolibarIcon.ParseSVG("./Defaults/bell.svg");
    readonly Geometry PlayAudioIcon             = LolibarIcon.ParseSVG("./Defaults/audio_play.svg");
    readonly Geometry PauseAudioIcon            = LolibarIcon.ParseSVG("./Defaults/audio_pause.svg");
    readonly Geometry PreviousAudioIcon         = LolibarIcon.ParseSVG("./Defaults/audio_rewind.svg");
    readonly Geometry NextAudioIcon             = LolibarIcon.ParseSVG("./Defaults/audio_next.svg");
    #endregion

    #region Color Codes
    const string PrimaryColorCode               = "#080c0e";
    const string SecondaryColorCode             = "#328087";
    const string TernaryColorCode               = "#f24646";
    #endregion

    #region Containers
    LolibarContainer WinContainerP              = new();
    LolibarContainer WinContainer               = new();

    LolibarContainer DateTimeContainerP         = new();
    LolibarContainer DateTimeContainer          = new();

    LolibarContainer AppsContainerP             = new();

    LolibarContainer AudioContainerP            = new();
    LolibarContainer PreviousButtonContainer    = new();
    LolibarContainer PlayButtonContainer        = new();
    LolibarContainer NextButtonContainer        = new();

    LolibarContainer AudioInfoContainer         = new();

    LolibarContainer PowerMonitorContainer      = new();
    LolibarContainer LanguageContainer          = new();

    LolibarContainer WorkspacesContainer        = new();

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
        BarSeparatorWidth           = 2.0;
        BarSeparatorRadius          = 0.0;

        BarColor                    = LolibarColor.FromHEX(PrimaryColorCode);
        BarContainersColor          = LolibarColor.FromHEX(SecondaryColorCode);
    }
    public override void Initialize()
    {
        // --- Win ---
        WinContainerP               = new()
        {
            Name                    = "WinContainerP",
            Parent                  = Lolibar.BarLeftContainer,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Right,
        };
        WinContainerP.Create();

        WinContainer                = new()
        {
            Name                    = "WinContainer",
            Parent                  = WinContainerP.GetBody(),
            Icon                    = AppsIcon,
            MouseLeftButtonUp       = OpenAppsMenu,
            HasBackground           = true,
        };
        WinContainer.Create();

        // --- Date / Time ---
        DateTimeContainerP          = new()
        {
            Name                    = "DateTimeContainerParent",
            Parent                  = Lolibar.BarLeftContainer,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Right,
        };
        DateTimeContainerP.Create();

        DateTimeContainer           = new()
        {
            Name                    = "DateTimeContainer",
            Parent                  = DateTimeContainerP.GetBody(),
            MouseLeftButtonUp       = OpenCalendar,
            HasBackground           = true,
        };
        DateTimeContainer.Create();

        AppsContainerP         = new()
        {
            Name                    = "AppsContainerParent",
            Parent                  = Lolibar.BarLeftContainer,
        };
        AppsContainerP.Create();

        LolibarProcess.AddPinnedAppsToContainer
        (
            parent: AppsContainerP.GetBody(),
            appContainerTitleState: LolibarEnums.AppContainerTitleState.OnlyActive
        );

        // --- Audio Player ---
        AudioContainerP             = new()
        {
            Name                    = "AudioContainerParent",
            Parent                  = Lolibar.BarRightContainer,
        };
        AudioContainerP.Create();

        AudioInfoContainer          = new()
        {
            Name                    = "AudioInfoContainer",
            Parent                  = AudioContainerP.GetBody(),
            HasBackground           = true,
            Color                   = LolibarColor.FromHEX(TernaryColorCode)
        };
        AudioInfoContainer.Create();

        PreviousButtonContainer     = new()
        {
            Name                    = "AudioPreviousButton",
            Parent                  = AudioContainerP.GetBody(),
            Icon                    = PreviousAudioIcon,
            MouseLeftButtonUp       = Previous,
            Color                   = LolibarColor.FromHEX(TernaryColorCode)
        };
        PreviousButtonContainer.Create();

        PlayButtonContainer         = new()
        {
            Name                    = "AudioPlayButton",
            Parent                  = AudioContainerP.GetBody(),
            MouseLeftButtonUp       = PlayOrPause,
            Color                   = LolibarColor.FromHEX(TernaryColorCode)
        };
        PlayButtonContainer.Create();

        NextButtonContainer         = new()
        {
            Name                    = "AudioNextButton",
            Parent                  = AudioContainerP.GetBody(),
            Icon                    = NextAudioIcon,
            MouseLeftButtonUp       = Next,
            Color                   = LolibarColor.FromHEX(TernaryColorCode)
        };
        NextButtonContainer.Create();

        // --- Power ---
        PowerMonitorContainer       = new()
        {
            Name                    = "PowerMonitorContainer",
            Parent                  = Lolibar.BarRightContainer,
            MouseLeftButtonUp       = OpenPowerSettings,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Left
        };
        PowerMonitorContainer.Create();

        // --- Language ---
        LanguageContainer           = new()
        {
            Name                    = "LanguageContainer",
            Icon                    = LolibarIcon.ParseSVG("./supchyan/pen.svg"),
            Parent                  = Lolibar.BarRightContainer,
        };
        LanguageContainer.Create();

        // --- Desktop Workspaces (Tabs) ---
        WorkspacesContainer         = new()
        {
            Name                    = "WorkspacesContainer",
            Parent                  = Lolibar.BarRightContainer,
            MouseWheelDelta         = SwapWorkspacesByMouseWheel,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Both,
        };
        WorkspacesContainer.Create();

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

        LolibarVirtualDesktop.DrawWorkspacesInParent
        (
            parent:             WorkspacesContainer.GetBody(),
            showDesktopNames:   true
        );
    }
    public override void Update()
    {
        LolibarHelper.HideWindowsTaskbar();

        // --- Auto resize logic ---
        (BarWidth, BarLeft) = LolibarHelper.OffsetLolibarToCenter(BarWidth, BarMargin);

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

        // --- Power ---
        PowerMonitorContainer.Text = LolibarDefaults.GetPowerInfo();
        PowerMonitorContainer.Icon = LolibarDefaults.GetPowerIcon();
        PowerMonitorContainer.Update();

        // --- Language ---
        LanguageContainer.Text = LolibarDefaults.CurrentInputLanguage?.Split(" (")[0];
        LanguageContainer.Update();
    }
    #endregion

    #region Click events
    // --- Win ---
    int OpenAppsMenu(MouseButtonEventArgs e)
    {
        LolibarHelper.KeyDown(Keys.LWin);
        LolibarHelper.KeyUp(Keys.LWin);

        return 0;
    }
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