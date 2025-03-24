using Windows.Media.Control;
using WindowsMediaController;

namespace LolibarApp.Source.Tools;
public class LolibarAudio
{
    static MediaManager Manager = new MediaManager();

    public static GlobalSystemMediaTransportControlsSessionMediaProperties?     MediaProperties     { get; private set; }
    public static GlobalSystemMediaTransportControlsSessionTimelineProperties?  TimelineProperties  { get; private set; }
    public static GlobalSystemMediaTransportControlsSessionPlaybackInfo?        PlaybackInfo        { get; private set; }

    /// <summary>
    /// Starts `LolibarAudio` logic. Already called upon lolibar's launch.
    /// </summary>
    public static void Start()
    {
        Manager.OnAnyMediaPropertyChanged      += Manager_OnAnyMediaPropertyChanged;
        Manager.OnAnyPlaybackStateChanged      += Manager_OnAnyPlaybackStateChanged;
        Manager.OnAnyTimelinePropertyChanged   += Manager_OnAnyTimelinePropertyChanged;

        Manager.Start();
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
        await Manager.GetFocusedSession()?.ControlSession.TryTogglePlayPauseAsync();
    }
    public static async void Play()
    {
        await Manager.GetFocusedSession()?.ControlSession.TryPlayAsync();
    }
    public static async void Pause()
    {
        await Manager.GetFocusedSession()?.ControlSession.TryPauseAsync();
    }
    public static async void Stop()
    {
        await Manager.GetFocusedSession()?.ControlSession.TryStopAsync();
    }
    public static async void Next()
    {
        await Manager.GetFocusedSession()?.ControlSession.TrySkipNextAsync();
    }
    public static async void Previous()
    {
        await Manager.GetFocusedSession()?.ControlSession.TrySkipPreviousAsync();
    }
    public static bool IsPlaying
    {
        get
        {
            if (Manager.GetFocusedSession() != null)
            {
                return Manager.GetFocusedSession().ControlSession.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion
}