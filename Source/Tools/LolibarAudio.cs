using Windows.Media.Control;

namespace LolibarApp.Source.Tools;
public class LolibarAudio
{
    /// <summary>
    /// Session Manager.
    /// </summary>
    public static GlobalSystemMediaTransportControlsSessionManager?             SessionManager      { get; set; }
    /// <summary>
    /// Current Audio Stream Session.
    /// </summary>
    public static GlobalSystemMediaTransportControlsSession?                    CurrentSession      { get; set; }
    /// <summary>
    /// Current audio session media properties (i.e. Title, Author, Description, etc.).
    /// </summary>
    public static GlobalSystemMediaTransportControlsSessionMediaProperties?     MediaProperties     { get; set; }
    public static GlobalSystemMediaTransportControlsSessionTimelineProperties?  TimelineProperties  { get; set; }
    public static GlobalSystemMediaTransportControlsSessionPlaybackInfo?        PlaybackInfo        { get; set; }

    #region Private Methods
    static void UpdateCurrentSession(GlobalSystemMediaTransportControlsSession newSession)
    {
        try
        {
            CurrentSession = newSession;

            CurrentSession.MediaPropertiesChanged       -= CurrentSession_MediaPropertiesChanged;
            CurrentSession.PlaybackInfoChanged          -= CurrentSession_PlaybackInfoChanged;
            CurrentSession.TimelinePropertiesChanged    -= CurrentSession_TimelinePropertiesChanged;

            CurrentSession.MediaPropertiesChanged       += CurrentSession_MediaPropertiesChanged;
            CurrentSession.PlaybackInfoChanged          += CurrentSession_PlaybackInfoChanged;
            CurrentSession.TimelinePropertiesChanged    += CurrentSession_TimelinePropertiesChanged;
        }
        catch
        {

        }
    }
    #endregion

    #region Events
    static void SessionManager_CurrentSessionChanged(GlobalSystemMediaTransportControlsSessionManager sender, CurrentSessionChangedEventArgs args)
    {
        UpdateCurrentSession(sender.GetCurrentSession());
    }
    static void CurrentSession_TimelinePropertiesChanged(GlobalSystemMediaTransportControlsSession sender, TimelinePropertiesChangedEventArgs args)
    {
        TimelineProperties  = sender.GetTimelineProperties();
    }
    static void CurrentSession_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
    {
        PlaybackInfo        = sender.GetPlaybackInfo();
    }
    static async void CurrentSession_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
    {
        MediaProperties     = await sender.TryGetMediaPropertiesAsync();
    }
    #endregion

    #region Session Controls
    /// <summary>
    /// Starts `LolibarAudio` logic. Already called upon lolibar's launch.
    /// </summary>
    public static async void Begin()
    {
        SessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
        CurrentSession = SessionManager.GetCurrentSession();

        SessionManager.CurrentSessionChanged += SessionManager_CurrentSessionChanged;
        UpdateCurrentSession(CurrentSession);
    }
    /// <summary>
    /// Attempts to play current audio stream if paused, and pause if opposite.
    /// </summary>
    public static void PlayOrPause()
    {
        CurrentSession?.TryTogglePlayPauseAsync();
    }
    /// <summary>
    /// Attempts to pause current audio stream.
    /// </summary>
    public static void Pause()
    {
        CurrentSession?.TryPauseAsync();
    }
    /// <summary>
    /// Attempts to start playing / resume current audio stream.
    /// </summary>
    public static void Resume()
    {
        CurrentSession?.TryPlayAsync();
    }
    /// <summary>
    /// Attempts to skip current audio stream and start to play the next one.
    /// </summary>
    public static void Next()
    {
        CurrentSession?.TrySkipNextAsync();
    }
    /// <summary>
    /// Attempts to return to previous audio stream and start to play it.
    /// </summary>
    public static void Previous()
    {
        CurrentSession?.TrySkipPreviousAsync();
    }
    /// <summary>
    /// Returns `true`, if current audio stream is playing.
    /// </summary>
    public static bool IsPlaying
    {
        get
        {
            return CurrentSession?.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
        }
    }
    #endregion
}