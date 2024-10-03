using Windows.Media.Control;

namespace LolibarApp.Source.Tools;
public class LolibarAudio
{
    static async Task<GlobalSystemMediaTransportControlsSessionManager?> GetSystemMediaTransportControlsSessionManager() => await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
    static async Task<GlobalSystemMediaTransportControlsSessionMediaProperties?> GetMediaProperties(GlobalSystemMediaTransportControlsSession? session) => session != null ? await session.TryGetMediaPropertiesAsync() : default;

    /// <summary>
    /// Current audio stream instance. (Returns `null` if nothing is buffered / playing)
    /// </summary>
    public static GlobalSystemMediaTransportControlsSessionManager? Stream { get; private set; }
    /// <summary>
    /// Current audio stream info. (Returns `null` if stream doesn't exist or stream's info is unreachable)
    /// </summary>
    public static GlobalSystemMediaTransportControlsSessionMediaProperties? StreamInfo { get; private set; }

    #region Method Calls
    static async void TryToInitializeInfoEvents()
    {
        if (Stream == null) return;

        StreamInfo = await GetMediaProperties(Stream.GetCurrentSession());

        if (Stream.GetCurrentSession() == null)
        {
            StreamInfo = null;
            return;
        }

        Stream.GetCurrentSession().PlaybackInfoChanged      -= LolibarAudio_PlaybackInfoChanged;
        Stream.GetCurrentSession().MediaPropertiesChanged   -= LolibarAudio_MediaPropertiesChanged;

        Stream.GetCurrentSession().PlaybackInfoChanged      += LolibarAudio_PlaybackInfoChanged;
        Stream.GetCurrentSession().MediaPropertiesChanged   += LolibarAudio_MediaPropertiesChanged;
    }
    public static async void InitializeStreamEvents()
    {
        Stream = await GetSystemMediaTransportControlsSessionManager();
        
        if (Stream != null) Stream.SessionsChanged += Stream_SessionsChanged;

        TryToInitializeInfoEvents();
    }

    private static void Stream_SessionsChanged(GlobalSystemMediaTransportControlsSessionManager sender, SessionsChangedEventArgs args)
    {
        TryToInitializeInfoEvents();
    }

    private static async void LolibarAudio_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
    {
        var session = Stream?.GetCurrentSession();
        if (session != null) StreamInfo = await GetMediaProperties(session);
    }

    private static async void LolibarAudio_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
    {
        var session = Stream?.GetCurrentSession();
        if (session != null) StreamInfo = await GetMediaProperties(session);
    }

    /// <summary>
    /// Attempts to pause current audio stream.
    /// </summary>
    public static async void Pause()
    {
        var session = Stream?.GetCurrentSession();
        if (session != null) await session.TryPauseAsync();
    }
    /// <summary>
    /// Attempts to start playing / resume current audio stream.
    /// </summary>
    public static async void Resume()
    {
        var session = Stream?.GetCurrentSession();
        if (session != null) await session.TryPlayAsync();
    }
    /// <summary>
    /// Attempts to skip current audio stream and start to play the next one.
    /// </summary>
    public static async void Next()
    {
        var session = Stream?.GetCurrentSession();
        if (session != null) await session.TrySkipNextAsync();
    }
    /// <summary>
    /// Attempts to return to previous audio stream and start to play it.
    /// </summary>
    public static async void Previous()
    {
        var session = Stream?.GetCurrentSession();
        if (session != null) await session.TrySkipPreviousAsync();
    }
    /// <summary>
    /// Returns `true`, if current audio stream is playing.
    /// </summary>
    public static bool IsPlaying()
    {
        var session = Stream?.GetCurrentSession();
        if (session != null) return session.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
        else return false;
    }
    #endregion
}