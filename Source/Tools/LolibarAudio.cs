using Windows.Media.Control;

namespace LolibarApp.Source.Tools;
public class LolibarAudio
{
    static async Task<GlobalSystemMediaTransportControlsSessionManager> GetSystemMediaTransportControlsSessionManager() => await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
    static async Task<GlobalSystemMediaTransportControlsSessionMediaProperties?> GetMediaProperties(GlobalSystemMediaTransportControlsSession? session) => session != null ? await session.TryGetMediaPropertiesAsync() : default;

    /// <summary>
    /// Current audio stream instance. (Returns `null` if nothing is buffered / playing)
    /// </summary>
    public static GlobalSystemMediaTransportControlsSessionManager? Stream { get; private set; } = GetSystemMediaTransportControlsSessionManager().Result;
    /// <summary>
    /// Current audio stream info. (Returns `null` if stream doesn't exist or stream's info is unreachable)
    /// </summary>
    public static GlobalSystemMediaTransportControlsSessionMediaProperties? StreamInfo { get; private set; }

    static GlobalSystemMediaTransportControlsSession? CurrentSession
    {
        get
        {
            return Stream?.GetCurrentSession();
        }
    }

    #region Methods
    public static void TryToResubscribeStreamEvents()
    {
        if (Stream == null) return;

        Stream.SessionsChanged -= Stream_SessionsChanged;
        Stream.SessionsChanged += Stream_SessionsChanged;
    }
    public static async void TryToResubscribeStreamInfoEvents()
    {
        if (CurrentSession == null)
        {
            StreamInfo = null;
            return;
        } 
        else
        {
            StreamInfo = await GetMediaProperties(CurrentSession);
        }

        CurrentSession.PlaybackInfoChanged    += LolibarAudio_PlaybackInfoChanged;
        CurrentSession.MediaPropertiesChanged += LolibarAudio_MediaPropertiesChanged;
    }
    #endregion

    #region Events
    static void Stream_SessionsChanged(GlobalSystemMediaTransportControlsSessionManager sender, SessionsChangedEventArgs args)
    {
        TryToResubscribeStreamEvents();
        TryToResubscribeStreamInfoEvents();
    }
    static async void LolibarAudio_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
    {
        StreamInfo = await GetMediaProperties(CurrentSession);
    }

    static async void LolibarAudio_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
    {
        StreamInfo = await GetMediaProperties(CurrentSession);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Attempts to play current audio stream if paused, and pause if opposite.
    /// </summary>
    public static async void PlayOrPause()
    {
        if (CurrentSession != null)
            await CurrentSession.TryTogglePlayPauseAsync();
    }
    /// <summary>
    /// Attempts to pause current audio stream.
    /// </summary>
    public static async void Pause()
    {
        if (CurrentSession != null) 
            await CurrentSession.TryPauseAsync();
    }
    /// <summary>
    /// Attempts to start playing / resume current audio stream.
    /// </summary>
    public static async void Resume()
    {
        if (CurrentSession != null)
            await CurrentSession.TryPlayAsync();
    }
    /// <summary>
    /// Attempts to skip current audio stream and start to play the next one.
    /// </summary>
    public static async void Next()
    {
        if (CurrentSession != null)
            await CurrentSession.TrySkipNextAsync();
    }
    /// <summary>
    /// Attempts to return to previous audio stream and start to play it.
    /// </summary>
    public static async void Previous()
    {
        if (CurrentSession != null)
            await CurrentSession.TrySkipPreviousAsync();
    }
    /// <summary>
    /// Returns `true`, if current audio stream is playing.
    /// </summary>
    public static bool IsPlaying()
    {
        if (CurrentSession != null) return CurrentSession.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
        else return false;
    }
    #endregion
}