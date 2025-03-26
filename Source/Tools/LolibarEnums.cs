namespace LolibarApp.Source.Tools;

public class LolibarEnums
{
    public enum SeparatorPosition { 
        None,
        Left, 
        Right, 
        Both 
    }
    public enum WinVer
    {
        Unknown,
        Unsupported,
        Win10,
        Win11,
        Win11_24H2,
    }
    public enum BarTargetCorner
    {
        Left,
        Right,
    }
    public enum WindowStateEnum
    {
        Hide = 0,
        ShowNormal = 1,
        ShowMinimized = 2,
        ShowMaximized = 3,
        ShowNormalNoActivate = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActivate = 7,
        ShowNoActivate = 8,
        Restore = 9,
        ShowDefault = 10,
        ForceMinimized = 11
    };
    /// <summary>
    /// LolibarExtern.GetWindow(hWnd, uCmd) -> `uCmd` enum helper
    /// </summary>
    public enum GW_Enum
    {
        /// <summary>
        /// The retrieved handle identifies the specified window's owner window, if any.
        /// For more information, see Owned Windows. (Moirasoft Wiki)
        /// </summary>
        GW_OWNER = 4,
        /// <summary>
        /// The retrieved handle identifies the child window at the top of the Z order, if the specified window is a parent window;
        /// otherwise, the retrieved handle is NULL.
        /// The function examines only child windows of the specified window.
        /// It does not examine descendant windows. (Moirasoft Wiki)
        /// </summary>
        GW_CHILD = 5,
        /// <summary>
        /// The retrieved handle identifies the enabled popup window owned by the specified window (the search uses the first such window found using GW_HWNDNEXT);
        /// otherwise, if there are no enabled popup windows, the retrieved handle is that of the specified window. (Moirasoft Wiki)
        /// </summary>
        GW_ENABLEDPOPUP = 6,
        /// <summary>
        /// The retrieved handle identifies the window of the same type that is highest in the Z order.
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window. (Moirasoft Wiki)
        /// </summary>
        GW_HWNDFIRST = 0,
        /// <summary>
        /// The retrieved handle identifies the window of the same type that is lowest in the Z order.
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window. (Moirasoft Wiki)
        /// </summary>
        GW_HWNDLAST = 1,
        /// <summary>
        /// The retrieved handle identifies the window below the specified window in the Z order.
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window. (Moirasoft Wiki)
        /// </summary>
        GW_HWNDNEXT = 2,
        /// <summary>
        /// The retrieved handle identifies the window above the specified window in the Z order.
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window. (Moirasoft Wiki)
        /// </summary>
        GW_HWNDPREV = 3
    }
    public enum AppContainerTitleState
    {
        Always      = 0,
        OnlyActive  = 1,
        Never       = 2,
    }
}
