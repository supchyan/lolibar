using System.Diagnostics;
using Windows.Media.Control;

namespace LolibarApp.Source.Tools;
public class LolibarAudio
{
    static async Task<GlobalSystemMediaTransportControlsSessionManager> GetSystemMediaTransportControlsSessionManager() => await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
    static async Task<GlobalSystemMediaTransportControlsSessionMediaProperties?> GetMediaProperties(GlobalSystemMediaTransportControlsSession? session) => session != null ? await session.TryGetMediaPropertiesAsync() : default;

    /// <summary>
    /// Current audio stream instance. (Returns `null` if nothing is buffered / playing)
    /// </summary>
    public static GlobalSystemMediaTransportControlsSessionManager? Stream { get; private set; }
    /// <summary>
    /// Current audio stream info. (Returns `null` if stream doesn't exist or stream's info is unreachable)
    /// </summary>
    public static GlobalSystemMediaTransportControlsSessionMediaProperties? StreamInfo { get; private set; }

    public static GlobalSystemMediaTransportControlsSession? CurrentSession
    {
        get
        {
            return Stream?.GetCurrentSession();
        }
    }

    #region Private Methods
    public static async Task TryToSubscribeStreamEvents()
    {
        try
        {
            Stream = await GetSystemMediaTransportControlsSessionManager();

            if (Stream == null) 
            {
                StreamInfo = null;
                return;
            }

            Stream.SessionsChanged -= Stream_SessionsChanged;
            Stream.SessionsChanged += Stream_SessionsChanged;
        }
        catch
        {

        }
    }
    public static async Task TryToSubscribeStreamInfoEvents()
    {
        try
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

            CurrentSession.PlaybackInfoChanged -= LolibarAudio_PlaybackInfoChanged;
            CurrentSession.MediaPropertiesChanged -= LolibarAudio_MediaPropertiesChanged;

            CurrentSession.PlaybackInfoChanged += LolibarAudio_PlaybackInfoChanged;
            CurrentSession.MediaPropertiesChanged += LolibarAudio_MediaPropertiesChanged;
        }
        catch
        {

        }
    }
    #endregion

    #region Events
    static async void Stream_SessionsChanged(GlobalSystemMediaTransportControlsSessionManager sender, SessionsChangedEventArgs args)
    {
        await TryToSubscribeStreamEvents();
        await TryToSubscribeStreamInfoEvents();
    }
    static async void LolibarAudio_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
    {
        if (CurrentSession != null)
        {
            StreamInfo = await GetMediaProperties(CurrentSession);
        }
    }
    static async void LolibarAudio_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
    {
        if (CurrentSession != null)
        {
            StreamInfo = await GetMediaProperties(CurrentSession);
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Attempts to play current audio stream if paused, and pause if opposite.
    /// </summary>
    public static async void PlayOrPause()
    {
        if (CurrentSession != null)
        {
            await CurrentSession.TryTogglePlayPauseAsync();
        }
    }
    /// <summary>
    /// Attempts to pause current audio stream.
    /// </summary>
    public static async void Pause()
    {
        if (CurrentSession != null)
        {
            await CurrentSession.TryPauseAsync();
        }
    }
    /// <summary>
    /// Attempts to start playing / resume current audio stream.
    /// </summary>
    public static async void Resume()
    {
        if (CurrentSession != null)
        {
            await CurrentSession.TryPlayAsync();
        }
    }
    /// <summary>
    /// Attempts to skip current audio stream and start to play the next one.
    /// </summary>
    public static async void Next()
    {
        if (CurrentSession != null)
        {
            await CurrentSession.TrySkipNextAsync();
        }
    }
    /// <summary>
    /// Attempts to return to previous audio stream and start to play it.
    /// </summary>
    public static async void Previous()
    {
        if (CurrentSession != null)
        {
            await CurrentSession.TrySkipPreviousAsync();
        }
    }
    /// <summary>
    /// Returns `true`, if current audio stream is playing.
    /// </summary>
    public static bool IsPlaying()
    {
        if (CurrentSession != null)
        {
            return CurrentSession.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
        }
        else
        {
            return false;
        }
    }
    #endregion
}