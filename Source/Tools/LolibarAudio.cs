using Windows.Media.Control;

namespace LolibarApp.Source.Tools;
public class LolibarAudio
{
    static async Task<GlobalSystemMediaTransportControlsSessionManager?> GetSystemMediaTransportControlsSessionManager() => await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
    static async Task<GlobalSystemMediaTransportControlsSessionMediaProperties?> GetMediaProperties(GlobalSystemMediaTransportControlsSession? session) => session != null ? await session.TryGetMediaPropertiesAsync() : null;

    /// <summary>
    /// Current audio stream instance. (Returns `null` if nothing is buffered / playing)
    /// </summary>
    public static GlobalSystemMediaTransportControlsSessionManager? Stream { get; private set; }
    /// <summary>
    /// Current audio stream info. (Returns `null` if stream doesn't exist or stream's info is unreachable)
    /// </summary>
    public static GlobalSystemMediaTransportControlsSessionMediaProperties? StreamInfo { get; private set; }

    #region Method Calls
    /// <summary>
    /// Updates audio stream data.
    /// </summary>
    public static async void UpdateStreamData()
    {
        Stream = await GetSystemMediaTransportControlsSessionManager();
        if (Stream != null) StreamInfo = await GetMediaProperties(Stream.GetCurrentSession());
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
    /// <returns></returns>
    public static bool IsPlaying()
    {
        var session = Stream?.GetCurrentSession();
        if (session != null) return session.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
        else return false;
    }
    #endregion
}