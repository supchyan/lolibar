using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;

namespace LolibarApp.Mods;

class MaintainerShowcaseMod : LolibarMod
{
    #region Animation definitions
    byte BrailleCodeAnimationFrame                  = 0;
    byte AltBrailleCodeAnimationFrame               = 0;
    readonly string[] BrailleCodeAnimationFrames    = [ "⠋", "⠙", "⠸", "⠴", "⠦", "⠇" ];
    readonly string[] AltBrailleCodeAnimationFrames = ["⠖", "⠦", "⠴", "⠲", "⠓", "⠋", "⠙", "⠚"];
    #endregion

    #region Icons
    readonly Geometry GearIcon          = LolibarIcon.ParseSVG("./Defaults/gear.svg");
    readonly Geometry BellIcon          = LolibarIcon.ParseSVG("./Defaults/bell.svg");
    readonly Geometry AudioPlayIcon     = LolibarIcon.ParseSVG("./Defaults/audio_play.svg");
    readonly Geometry AudioPauseIcon    = LolibarIcon.ParseSVG("./Defaults/audio_pause.svg");
    readonly Geometry AudioRewindIcon   = LolibarIcon.ParseSVG("./Defaults/audio_rewind.svg");
    readonly Geometry AudioNextIcon     = LolibarIcon.ParseSVG("./Defaults/audio_next.svg");
    #endregion

    #region Color Codes
    const string PrimaryColorCode       = "#bc1b1c1f"; // semi-transparent
    const string SecondaryColorCode     = "#ffcbd6ea";
    const string SecondaryColorCodeT    = "#55cbd6ea"; // semi-transparent
    const string TernaryColorCode       = "#ffe6524c";
    #endregion

    #region Containers
    // P.S. 
    // '*P' - stands to Parent container
    LolibarContainer WorkspacesContainer        = new();

    LolibarContainer AudioContainerP            = new();
    LolibarContainer AudioContainer             = new();
    LolibarContainer AudioSwitchButtonContainer = new();

    LolibarContainer AppsContainer              = new();

    LolibarContainer DateTimeContainer          = new();
    LolibarContainer LanguageContainer          = new();
    
    LolibarContainer PowerMonitorContainer      = new();
    LolibarContainer NotificationsContainer     = new();
    LolibarContainer QuickSettingsContainer     = new();

    #endregion

    #region Variables
    int     oldAudioPlaybackState   = -1;
    string  oldLanguage             = "";
    #endregion

    #region Body
    public override void PreInitialize()
    {
        BarUpdateDelay              = 120;

        BarShadowColor              = LolibarColor.FromHEX("#ff000000");

        BarSeparatorHeight          = 14.0;
        BarSeparatorWidth           = 3.0;
        BarSeparatorRadius          = 1.5;
        BarContextMenuChildMargin   = 10.0;
        BarFontSize                 = 13;

        BarColor                    = LolibarColor.FromHEX(PrimaryColorCode);
        BarContainersColor          = LolibarColor.FromHEX(SecondaryColorCode);
        
        BarStrokeColor              = LolibarColor.FromHEX(SecondaryColorCodeT);
        BarStrokeThickness          = new Thickness(1);

        BarHideVanillaTaskBar       = true;
        BarSnapToTop                = false;
    }
    public override void Initialize()
    {
        // --- Desktop Workspaces (Tabs) ---
        WorkspacesContainer         = new()
        {
            Parent                  = Lolibar.BarLeftContainer,
            MouseWheelDelta         = SwapWorkspacesByMouseWheel,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Right,
            LeftMarginOffset        = -4.0
        };
        WorkspacesContainer.Create();

        // WorkspacesContainer will be filled and updated per time with current virtual desktops
        LolibarVirtualDesktop.DrawWorkspacesInParent
        (
            parent:                 WorkspacesContainer.GetBody(),
            showDesktopNames:       true
        );

        // --- Pinned Apps ---
        AppsContainer = new()
        {
            Parent = Lolibar.BarLeftContainer,
        };
        AppsContainer.Create();

        // AppsContainerP will be filled and updated per time with taskbar pinned apps
        LolibarProcess.AddPinnedAppsToContainer
        (
            parent: AppsContainer.GetBody(),
            // show app name when app is active (selected / focused)
            appContainerTitleState: LolibarEnums.AppContainerTitleState.OnlyActive,
            appTitleMaxLength:      24
        );

        // --- Audio Player ---
        AudioContainerP             = new()
        {
            Parent                  = Lolibar.BarRightContainer,
            RightMarginOffset       = 5.0,
        };
        AudioContainerP.Create();

        AudioContainer              = new()
        {
            Parent                  = AudioContainerP.GetBody(),
            HasBackground           = true,
            MouseLeftButtonUp       = PlayOrPause,
            MouseMiddleButtonUp     = ShowCurrentPlayingAudioToast,
            MouseRightButtonUp      = OpenAudioContextMenu,
            Color                   = LolibarColor.FromHEX(TernaryColorCode)
        };
        AudioContainer.Create();

        // --- Date / Time ---
        DateTimeContainer           = new()
        {
            Parent                  = Lolibar.BarRightContainer,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Both
        };
        DateTimeContainer.Create();

        // --- Language ---
        LanguageContainer           = new()
        {
            Parent                  = Lolibar.BarRightContainer,
            SeparatorPosition       = LolibarEnums.SeparatorPosition.Right
        };
        LanguageContainer.Create();

        // --- Power ---
        PowerMonitorContainer       = new()
        {
            Parent                  = Lolibar.BarRightContainer,
            MouseRightButtonUp      = OpenPowerContextMenu,
            HasBackground           = true,
            LeftMarginOffset        = 14.0
        };
        PowerMonitorContainer.Create();

        // --- Notifications ---
        NotificationsContainer      = new()
        {
            Parent                  = Lolibar.BarRightContainer,
            Icon                    = BellIcon,
            MouseLeftButtonUp       = OpenNotificationsOverlay,
            HasBackground           = true,
            LeftMarginOffset        = 9.0,
            RightMarginOffset       = 10.0
        };
        NotificationsContainer.Create();

        // --- Quick Settings ---
        QuickSettingsContainer      = new()
        {
            Parent                  = Lolibar.BarRightContainer,
            Icon                    = GearIcon,
            MouseLeftButtonUp       = OpenQuickSettingsUI,
            MouseRightButtonUp      = OpenQuickSettingsContextMenu,
            HasBackground           = true,
            RightMarginOffset       = 10.0
        };
        QuickSettingsContainer.Create();
    }

    public override void Update()
    {
        // --- Audio player ---
        //
        // Get playing audio title or "" if nothing is playing
        var audioTitle = LolibarAudio.MediaProperties?.Title ?? "";

        // Set audio container text.
        // Audio is playing: audio title + braille animation,
        // Audio paused: audio title,
        // ...
        AudioContainer.Text = LolibarAudio.IsPlaying ? $"{audioTitle.Truncate(24)} {AltBrailleAudioPlayerAnimation()}" : audioTitle.Truncate(24);

        // ...but
        if (audioTitle == "")
        {
            // show some braille animation in audio container if no audio detected/playing
            AudioContainer.Text = BrailleAudioPlayerAnimation();
        }

        // Logic when audio playback state changed
        if (oldAudioPlaybackState != LolibarAudio.IsPlaying.GetHashCode())
        {
            // If audio is playing now, make audio player container visible
            if (LolibarAudio.IsPlaying)
            {
                LolibarAnimator.Common.IncreaseTransparency(AudioContainer.GetBody());
            }
            else // make it semi-transparent
            {
                LolibarAnimator.Common.DecreaseTransparency(AudioContainer.GetBody());
            }
            oldAudioPlaybackState = LolibarAudio.IsPlaying.GetHashCode();
        }

        // Update all of above
        AudioContainer.Update();

        // Change audio switch container icon when audio stream switches its state
        AudioSwitchButtonContainer.Icon = LolibarAudio.IsPlaying ? AudioPauseIcon : AudioPlayIcon;

        // Update changes
        AudioSwitchButtonContainer.Update();

        // --- Date / Time ---
        //
        // Use String.Format("{0:00}") to convert '9:13' into '09:13' for example
        DateTimeContainer.Text = $"{String.Format("{0:00}", DateTime.Now.Day)}.{String.Format("{0:00}", DateTime.Now.Month)} ({String.Format("{0:00}", DateTime.Now.Hour)}:{String.Format("{0:00}", DateTime.Now.Minute)})";
        DateTimeContainer.Update();

        // --- Language ---
        //
        // Getting current input language and transform it to better look
        var currentLanguage = LolibarStats.CurrentInputLanguage?.Split(" (")[0].ToUpper()[..3] ?? "Undefined";
        
        // Logic to handle input language change
        if (oldLanguage != currentLanguage)
        {
            if (oldLanguage != "")
            {
                // Show new language info as a toast 
                LolibarToast currentLanguageToast = new()
                {
                    Text      = currentLanguage,
                    FontSize  = 18,
                    IsBold    = true,
                    ShowTime  = 1000, // 1s
                };
                currentLanguageToast.Create();
            }

            oldLanguage = currentLanguage;
        }
        // Set current input language as container text
        LanguageContainer.Text = currentLanguage;

        LanguageContainer.Update();

        // --- Power ---
        //
        // Power in percent returns something like '15%'
        PowerMonitorContainer.Text = $"{LolibarStats.PowerInPercent}";
        // SmartPowerIcon changes its visuals as battery current power level
        PowerMonitorContainer.Icon = LolibarStats.SmartPowerIcon;

        PowerMonitorContainer.Update();
    }
    #endregion

    #region Click events
    // --- Quick Settings ---
    int OpenQuickSettingsUI(MouseButtonEventArgs e)
    {
        // Built-in hotkey to open settings overlay in windows
        LolibarHelper.KeyDown(Keys.LWin);
        LolibarHelper.KeyDown(Keys.A);
        LolibarHelper.KeyUp(Keys.A);
        LolibarHelper.KeyUp(Keys.LWin);

        return 0;
    }
    int OpenQuickSettingsContextMenu(MouseButtonEventArgs e)
    {
        // Create context menu
        LolibarContextMenu menu = new();

        // Add children as LolibarContainers
        menu.Children.Add(new()
        {
            Text = $"Lolibar Menu",
            Icon = LolibarIcon.GetApplicationIcon(Process.GetCurrentProcess().MainModule?.FileName ?? ""),
        });

        menu.Children.Add(new()
        {
            Text = $"Restart Lolibar",
            HasBackground = true,

            MouseLeftButtonUp = RestartLolibar
        });

        menu.Children.Add(new()
        {
            Text = $"Close Lolibar",
            HasBackground = true,

            MouseLeftButtonUp = CloseLolibar
        });

        // Create (show) menu
        menu.Create();

        return 0;
    }
    int CloseLolibar(MouseButtonEventArgs e)
    {
        LolibarHelper.CloseApplicationGently(); 
        return 0;
    }
    int RestartLolibar(MouseButtonEventArgs e)
    {
        LolibarHelper.RestartApplicationGently();
        return 0;
    }


    // --- Notifications ---
    int OpenNotificationsOverlay(MouseButtonEventArgs e)
    {
        // Built-in hotkey to open notifications overlay in windows
        LolibarHelper.KeyDown(Keys.LWin);
        LolibarHelper.KeyDown(Keys.N);
        LolibarHelper.KeyUp(Keys.N);
        LolibarHelper.KeyUp(Keys.LWin);

        return 0;
    }

    // --- Power ---
    int OpenPowerContextMenu(MouseButtonEventArgs e)
    {
        // Create context menu
        LolibarContextMenu menu = new();

        // Add children as LolibarContainers
        menu.Children.Add(new()
        {
            Text = $"Power left: {LolibarStats.PowerInPercent}",
            Icon = LolibarStats.SmartPowerIcon,
        });

        menu.Children.Add(new()
        {
            Text = $"Open settings",
            Icon = GearIcon,
            HasBackground = true,

            MouseLeftButtonUp = OpenPowerSettings
        });

        // Create (show) menu
        menu.Create();

        return 0;
    }
    int OpenPowerSettings(MouseButtonEventArgs e)
    {
        // Launches default windows settings with battery tab oppened
        _ = Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:batterysaver"));

        return 0;
    }

    // --- Desktop Workspaces ---
    int Previous(MouseButtonEventArgs e)
    {
        // Go to previous audio
        LolibarAudio.Previous();
        return 0;
    }
    int PlayOrPause(MouseButtonEventArgs e)
    {
        // Play or Pause current audio
        LolibarAudio.PlayOrPause();
        return 0;
    }
    int Next(MouseButtonEventArgs e)
    {
        // Go to next audio
        LolibarAudio.Next();
        return 0;
    }
    int SwapWorkspacesByMouseWheel(MouseWheelEventArgs e)
    {
        // If mouse wheel scrolls down, go to next desktop if possible
        if (e.Delta > 0)
        {
            LolibarVirtualDesktop.GoToDesktopLeft();
        }

        // If mouse wheel scrolls up, go to previous desktop if possible
        if (e.Delta < 0)
        {
            LolibarVirtualDesktop.GoToDesktopRight();
        }

        return 0;
    }
    int OpenAudioContextMenu(MouseButtonEventArgs args)
    {
        var audioTitle = LolibarAudio.MediaProperties?.Title ?? "";

        // Don't show context menu, if no audio to control
        if (audioTitle == "")
        {
            ShowCurrentPlayingAudioToast(args);
            return 0;
        }

        // Context menu class body
        LolibarContextMenu menu = new()
        {
            Orientation = System.Windows.Controls.Orientation.Horizontal,
        };

        // Children as LolibarContainers
        menu.Children.Add(new()
        {
            Icon = AudioRewindIcon,
            HasBackground = true,
            MouseLeftButtonUp = Previous,
        });


        // This container will be updatable in Update() hook, so initialize it globally
        AudioSwitchButtonContainer = new()
        {
            Icon = AudioPauseIcon,
            HasBackground = true,
            MouseLeftButtonUp = PlayOrPause
        };
        AudioSwitchButtonContainer.Initialize();

        menu.Children.Add(
            AudioSwitchButtonContainer
        );

        menu.Children.Add(new()
        {
            Icon = AudioNextIcon,
            HasBackground = true,
            MouseLeftButtonUp = Next
        });

        // Show menu
        menu.Create();

        return 0;
    }
    int ShowCurrentPlayingAudioToast(MouseButtonEventArgs args)
    {
        var audioTitle = LolibarAudio.MediaProperties?.Title ?? "";

        // Show current audio info as toast or placeholder, if no audio is playing
        new LolibarToast()
        {
            FontSize    = 18,
            IsBold      = false,
            Text        = audioTitle == "" ? "No active audio detected" : $"Now Playing: {audioTitle.Truncate(128)}",
            ShowTime    = 2000
        }.Create();

        return 0;
    }
    #endregion

    #region Anime Stuff Methods
    string AltBrailleAudioPlayerAnimation()
    {
        // Braille animation.
        // Swaps frames per "update tick" and returns different char.
        AltBrailleCodeAnimationFrame++;
        if (AltBrailleCodeAnimationFrame >= AltBrailleCodeAnimationFrames.Length)
        {
            AltBrailleCodeAnimationFrame = 0;
            return AltBrailleCodeAnimationFrames[AltBrailleCodeAnimationFrame];
        }
        else
        {
            return AltBrailleCodeAnimationFrames[AltBrailleCodeAnimationFrame];
        }
    }
    string BrailleAudioPlayerAnimation()
    {
        // Braille animation.
        // Swaps frames per "update tick" and returns different char.
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
    #endregion
}