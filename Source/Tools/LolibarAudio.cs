using Windows.Media.Control;
using WindowsMediaController;

namespace LolibarApp.Source.Tools;
public class LolibarAudio
{
    static readonly MediaManager Manager = new();
    static GlobalSystemMediaTransportControlsSession? Session
    {
        get
        {
            return Manager.GetFocusedSession()?.ControlSession;
        }
    }

    public static GlobalSystemMediaTransportControlsSessionMediaProperties?     MediaProperties     { get; private set; }
    public static GlobalSystemMediaTransportControlsSessionTimelineProperties?  TimelineProperties  { get; private set; }
    public static GlobalSystemMediaTransportControlsSessionPlaybackInfo?        PlaybackInfo        { get; private set; }

    /// <summary>
    /// Starts `LolibarAudio` logic. Already called upon lolibar's launch.
    /// </summary>
    public static void Start()
    {
        Manager.OnAnySessionClosed              += Manager_OnAnySessionClosed;
        Manager.OnAnySessionOpened              += Manager_OnAnySessionOpened;
        Manager.OnAnyMediaPropertyChanged       += Manager_OnAnyMediaPropertyChanged;
        Manager.OnAnyPlaybackStateChanged       += Manager_OnAnyPlaybackStateChanged;
        Manager.OnAnyTimelinePropertyChanged    += Manager_OnAnyTimelinePropertyChanged;

        Manager.Start();
    }

    static async void Manager_OnAnySessionOpened(MediaManager.MediaSession mediaSession)
    {
        if (Session != null)
        {
            MediaProperties     = await Session.TryGetMediaPropertiesAsync();
            TimelineProperties  = Session.GetTimelineProperties();
            PlaybackInfo        = Session.GetPlaybackInfo();
        }
    }

    static void Manager_OnAnySessionClosed(MediaManager.MediaSession mediaSession)
    {
        MediaProperties     = null;
        TimelineProperties  = null;
        PlaybackInfo        = null;
    }
    #region Events
    private static void Manager_OnAnyTimelinePropertyChanged(MediaManager.MediaSession mediaSession, GlobalSystemMediaTransportControlsSessionTimelineProperties timelineProperties)
    {
        TimelineProperties  = timelineProperties;
    }

    private static void Manager_OnAnyPlaybackStateChanged(MediaManager.MediaSession mediaSession, GlobalSystemMediaTransportControlsSessionPlaybackInfo playbackInfo)
    {
        PlaybackInfo        = playbackInfo;
    }

    private static void Manager_OnAnyMediaPropertyChanged(MediaManager.MediaSession mediaSession, GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
    {
        MediaProperties     = mediaProperties;
    }
    #endregion

    #region Controls
    public static async void PlayOrPause()
    {
        if (Session != null)
            await Session.TryTogglePlayPauseAsync();
    }
    public static async void Play()
    {
        if (Session != null)
            await Session?.TryPlayAsync();
    }
    public static async void Pause()
    {
        if (Session != null)
            await Session?.TryPauseAsync();
    }
    public static async void Stop()
    {
        if (Session != null)
            await Session?.TryStopAsync();
    }
    public static async void Next()
    {
        if (Session != null)
            await Session?.TrySkipNextAsync();
    }
    public static async void Previous()
    {
        if (Session != null)
            await Session?.TrySkipPreviousAsync();
    }
    public static bool IsPlaying
    {
        get
        {
            if (Session != null)
            {
                return Session.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion
}