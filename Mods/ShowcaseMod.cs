//using LolibarApp.Source;
//using LolibarApp.Source.Tools;
//using System.Windows.Input;
//using System.Windows.Media;

//// This mod is outside of the Mods namespace, so it won't be loaded
//// You can uncomment namespace to enable (load) it

////namespace LolibarApp.Mods;

//class ShowcaseMod : LolibarMod
//{
//    #region Anime Stuff
//    string OldAudioTitle                            = string.Empty;
//    int OldAudioPlaybackState                       = -1;
//    byte BrailleCodeAnimationFrame                  = 0;
//    readonly string[] BrailleCodeAnimationFrames    =
//    [
//        "⠋",
//        "⠙",
//        "⠸",
//        "⠴",
//        "⠦",
//        "⠇",
//    ];
//    #endregion

//    #region Icons
//    readonly Geometry PlayAudioIcon         = Geometry.Parse("M16 8C16 12.4182 12.4182 16 8 16C3.58172 16 0 12.4182 0 8C0 3.58172 3.58172 0 8 0C12.4182 0 16 3.58172 16 8ZM7.20965 10.8844L10.5812 8.79425C11.1396 8.44805 11.1396 7.55195 10.5812 7.20575L7.20965 5.11564C6.66694 4.77921 6 5.2171 6 5.90987V10.0901C6 10.7829 6.66694 11.2208 7.20965 10.8844Z");
//    readonly Geometry PauseAudioIcon        = Geometry.Parse("M16 8C16 12.4182 12.4182 16 8 16C3.58172 16 0 12.4182 0 8C0 3.58172 3.58172 0 8 0C12.4182 0 16 3.58172 16 8ZM7 6C7 5.44772 6.55228 5 6 5C5.44772 5 5 5.44772 5 6V10C5 10.5523 5.44772 11 6 11C6.55228 11 7 10.5523 7 10V6ZM11 6C11 5.44772 10.5523 5 10 5C9.44771 5 9 5.44772 9 6V10C9 10.5523 9.44771 11 10 11C10.5523 11 11 10.5523 11 10V6Z");
//    readonly Geometry NextAudioIcon         = Geometry.Parse("M8 1.33333C8 0.596954 7.40305 0 6.66667 0C5.93029 0 5.33334 0.596954 5.33334 1.33333V2.46058L1.61286 0.154191C0.88925 -0.29439 0 0.28947 0 1.21316V6.7868C0 7.7105 0.88925 8.2944 1.61286 7.8458L5.33334 5.53942V6.66667C5.33334 7.40305 5.93029 8 6.66667 8C7.40305 8 8 7.40305 8 6.66667V1.33333Z");
//    readonly Geometry PreviousAudioIcon     = Geometry.Parse("M-3.8147e-06 6.66667C-3.8147e-06 7.40305 0.59695 8 1.33333 8C2.06971 8 2.66666 7.40305 2.66666 6.66667V5.53942L6.38714 7.84581C7.11075 8.29439 8 7.71053 8 6.78684V1.21319C8 0.289495 7.11075 -0.294405 6.38714 0.154195L2.66666 2.46058V1.33333C2.66666 0.596953 2.06971 0 1.33333 0C0.59695 0 -3.8147e-06 0.596953 -3.8147e-06 1.33333V6.66667Z");
//    #endregion

//    #region Color Codes
//    const string BaseColorCode      = "#0c1013";
//    const string TextColorCode      = "#f0f4f6";
//    const string AltTextColorCode   = "#f5c28c";
//    #endregion

//    #region Containers
//    LolibarContainer WorkspacesContainer        = new();

//    LolibarContainer DateTimeContainer          = new();

//    LolibarContainer AudioContainerParent       = new();
//    LolibarContainer PreviousButtonContainer    = new();
//    LolibarContainer PlayButtonContainer        = new();
//    LolibarContainer NextButtonContainer        = new();
//    LolibarContainer AudioInfoContainer         = new();

//    LolibarContainer BrailleCodeContainer       = new();
//    #endregion

//    #region Body
//    public override void PreInitialize()
//    {
//        BarUpdateDelay              = 100;
//        BarHeight                   = 40;
//        BarColor                    = LolibarHelper.SetColor(BaseColorCode);
//        BarContainersColor          = LolibarHelper.SetColor(TextColorCode);
//    }
//    public override void Initialize()
//    {
//        // --- Left Side ---

//        // --- Desktop Workspaces (Tabs) ---
//        WorkspacesContainer = new()
//        {
//            Name                = "WorkspacesContainer",
//            Parent              = Lolibar.BarLeftContainer,
//            SeparatorPosition   = LolibarEnums.SeparatorPosition.None,
//            MouseWheelEvent     = SwapWorkspacesByMouseWheelEvent
//        };
//        WorkspacesContainer.Create();

//        LolibarVirtualDesktop.DrawWorkspacesInParent(
//            parent:             WorkspacesContainer.GetBody(),
//            showDesktopNames:   true
//        );

//        DateTimeContainer = new()
//        {
//            Name                    = "DateTimeContainer",
//            Parent                  = Lolibar.BarLeftContainer,
//            SeparatorPosition       = LolibarEnums.SeparatorPosition.Left,
//            MouseLeftButtonUpEvent  = OpenCalendarEvent
//        };
//        DateTimeContainer.Create();

//        // --- Right Side ---

//        // --- Audio Player ---
//        AudioInfoContainer          = new()
//        {
//            Name                    = "AudioInfoContainer",
//            Parent                  = Lolibar.BarRightContainer,
//            HasBackground           = true,
//            Color                   = LolibarHelper.SetColor(AltTextColorCode)
//        };
//        AudioInfoContainer.Create();

//        AudioContainerParent        = new()
//        {
//            Name                    = "AudioContainerParent",
//            Parent                  = Lolibar.BarRightContainer,
//        };
//        AudioContainerParent.Create();

//        PreviousButtonContainer     = new()
//        {
//            Name                    = "AudioPreviousButton",
//            Parent                  = AudioContainerParent.GetBody(),
//            Icon                    = PreviousAudioIcon,
//            Color                   = LolibarHelper.SetColor(AltTextColorCode),
//            MouseLeftButtonUpEvent  = PreviousStreamCallEvent
//        };
//        PreviousButtonContainer.Create();

//        PlayButtonContainer         = new()
//        {
//            Name                    = "AudioPlayButton",
//            Parent                  = AudioContainerParent.GetBody(),
//            Icon                    = PlayAudioIcon,
//            Color                   = LolibarHelper.SetColor(AltTextColorCode),
//            MouseLeftButtonUpEvent  = PlayOrPauseStreamCallEvent
//        };
//        PlayButtonContainer.Create();

//        NextButtonContainer         = new()
//        {
//            Name                    = "AudioNextButton",
//            Parent                  = AudioContainerParent.GetBody(),
//            Icon                    = NextAudioIcon,
//            Color                   = LolibarHelper.SetColor(AltTextColorCode),
//            MouseLeftButtonUpEvent  = NextStreamCallEvent
//        };
//        NextButtonContainer.Create();

//        BrailleCodeContainer        = new()
//        {
//            Name                    = "BrailleCodeContainer",
//            Parent                  = Lolibar.BarRightContainer,
//            SeparatorPosition       = LolibarEnums.SeparatorPosition.Left
//        };
//        BrailleCodeContainer.Create();
//    }
//    public override void Update() 
//    {
//        // --- Properties ---
//        BarWidth = Lolibar.Inch_Screen.X > 2 * BarMargin ? Lolibar.Inch_Screen.X - 2 * BarMargin : BarWidth;
//        BarLeft  = (Lolibar.Inch_Screen.X - BarWidth) / 2;

//        // --- Date / Time ---
//        DateTimeContainer.Text = $"{String.Format("{0:00}", DateTime.Now.Hour)}:{String.Format("{0:00}", DateTime.Now.Minute)} / {DateTime.Now.DayOfWeek}, {DateTime.Now.Day} {DateTime.Now.ToString("MMMM")} {DateTime.Now.Year}";
//        DateTimeContainer.Update();

//        // --- Audio Player ---
//        PlayButtonContainer.Icon = LolibarAudio.IsPlaying ? PauseAudioIcon : PlayAudioIcon;
//        PlayButtonContainer.Update();

//        UseAudioTitleBlinkAnimation();

//        var AudioTitle = LolibarAudio.MediaProperties?.Title ?? "";

//        AudioInfoContainer.Text = AudioTitle == ""          ?
//            $"Nothing to play..."                           :
//            $"{LolibarAudio.MediaProperties?.Title}"             ;

//        // Smooth opacity animtaion upon audio playback state change
//        if (AudioInfoContainer.GetBody() != null && OldAudioPlaybackState != LolibarAudio.IsPlaying.GetHashCode())
//        {
//            if (LolibarAudio.IsPlaying)
//            {
//                LolibarAnimator.BeginIncOpacityAnimation(AudioInfoContainer.GetBody());
//            }
//            else
//            {
//                LolibarAnimator.BeginDecOpacityAnimation(AudioInfoContainer.GetBody());
//            }
//            OldAudioPlaybackState = LolibarAudio.IsPlaying.GetHashCode();
//        }

//        AudioInfoContainer.Update();

//        BrailleCodeContainer.Text = BrailleCodeAnimation();
//        BrailleCodeContainer.Update();
//    }
//    #endregion

//    #region Events
//    // date / time
//    void OpenCalendarEvent(object sender, MouseButtonEventArgs e)
//    {
//        LolibarHelper.KeyDown(Keys.LWin);
//        LolibarHelper.KeyDown(Keys.C);
//        LolibarHelper.KeyUp(Keys.C);
//        LolibarHelper.KeyUp(Keys.LWin);
//    }

//    // Audio Player
//    void PreviousStreamCallEvent(object sender, MouseButtonEventArgs e)
//    {
//        LolibarAudio.Previous();
//    }
//    void PlayOrPauseStreamCallEvent(object sender, MouseButtonEventArgs e)
//    {
//        if (AudioInfoContainer.GetBody() == null)
//        {
//            return;
//        }
//        if (LolibarAudio.IsPlaying)
//        {
//            LolibarAudio.Pause();
//        }
//        else
//        {
//            LolibarAudio.Resume();
//        }
//    }
//    void NextStreamCallEvent(object sender, MouseButtonEventArgs e)
//    {
//        LolibarAudio.Next();
//    }

//    // RAM
//    void SwapRamDisplayEvent(object sender, MouseButtonEventArgs e)
//    {
//        LolibarDefaults.SwapRamInfo();
//    }

//    // Desktop Workspaces
//    void SwapWorkspacesByMouseWheelEvent(object sender, MouseWheelEventArgs e)
//    {
//        if (e.Delta > 0)
//        {
//            LolibarVirtualDesktop.GoToDesktopLeft();
//        }

//        if (e.Delta < 0)
//        {
//            LolibarVirtualDesktop.GoToDesktopRight();
//        }
//    }
//    #endregion

//    #region Anime Stuff Methods
//    /// <summary>
//    /// Some Braille code animation logic, just for the visuals.
//    /// </summary>
//    /// <returns></returns>
//    string BrailleCodeAnimation()
//    {
//        BrailleCodeAnimationFrame++;
//        if (BrailleCodeAnimationFrame >= BrailleCodeAnimationFrames.Length)
//        {
//            BrailleCodeAnimationFrame = 0;
//            return BrailleCodeAnimationFrames[BrailleCodeAnimationFrame];
//        }
//        else
//        {
//            return BrailleCodeAnimationFrames[BrailleCodeAnimationFrame];
//        }
//    }
//    /// <summary>
//    /// My hook to blink audio title in the container whenever audio title has been changed.
//    /// Like... visual audio streams transition effect.
//    /// </summary>
//    void UseAudioTitleBlinkAnimation()
//    {
//        if (AudioInfoContainer.GetBody() == null) return;

//        if (OldAudioTitle != LolibarAudio.MediaProperties?.Title)
//        {
//            LolibarAnimator.BeginBlinkOpacityAnimation(AudioInfoContainer.GetBody());
//            OldAudioTitle = LolibarAudio.MediaProperties?.Title ?? string.Empty;
//        }
//    }
//    #endregion
//}