using System.Runtime.InteropServices;
using static LolibarApp.Source.Tools.LolibarExtern;

namespace LolibarApp.Source.Tools
{
    public class LolibarExtern
    {
        /// <summary>
        /// Synthesizes a keystroke.
        /// The system can use such a synthesized keystroke to generate a WM_KEYUP or WM_KEYDOWN message.
        /// The keyboard driver's interrupt handler calls the keybd_event function.
        /// </summary>
        /// <param name="bVk">A virtual-key code. The code must be a value in the range 1 to 254. For a complete list, see Virtual Key Codes.</param>
        /// <param name="bScan">A hardware scan code for the key.</param>
        /// <param name="dwFlags">Controls various aspects of function operation. This parameter can be one or more of the following values.</param>
        /// <param name="dwExtraInfo">An additional value associated with the key stroke.</param>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        /// <summary>
        /// Retrieves the identifier of the thread that created the specified window and, optionally, the identifier of the process that created the window.
        /// </summary>
        /// <param name="hwnd">A handle to the window.</param>
        /// <param name="lpdwProcessId">A pointer to a variable that receives the process identifier. If this parameter is not NULL, GetWindowThreadProcessId copies the identifier of the process to the variable; otherwise, it does not. If the function fails, the value of the variable is unchanged.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowThreadProcessId(nint hwnd, out uint lpdwProcessId);
        /// <summary>
        /// The GetDC function retrieves a handle to a device context (DC) for the client area of a specified window or for the entire screen.
        /// You can use the returned handle in subsequent GDI functions to draw in the DC.
        /// The device context is an opaque data structure, whose values are used internally by GDI.
        /// </summary>
        /// <param name="hwnd">A handle to the window whose DC is to be retrieved. If this value is NULL, GetDC retrieves the DC for the entire screen.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hwnd);
        /// <summary>
        /// The GetDeviceCaps function retrieves device-specific information for the specified device.
        /// </summary>
        /// <param name="hdc">A handle to the DC.</param>
        /// <param name="nIndex">The item to be returned. This parameter can be one of the following values.</param>
        /// <returns></returns>
        [DllImport("gdi32.dll",  SetLastError = true)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        /// <summary>
        /// Retrieves a handle to the top-level window whose class name and window name match the specified strings.
        /// This function does not search child windows.
        /// This function does not perform a case-sensitive search.
        /// </summary>
        /// <param name="className">The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpClassName; the high-order word must be zero.</param>
        /// <param name="windowTitle">The window name (the window's title). If this parameter is NULL, all window names match.</param>
        /// <returns>If the function succeeds, the return value is a handle to the window that has the specified class name and window name.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowTitle);
        /// <summary>
        /// Sets the specified window's show state.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hwnd, LolibarEnums.WindowStateEnum flags);
        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working).
        /// The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
        /// </summary>
        /// <returns>The return value is a handle to the foreground window.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();
        /// <summary>
        /// Brings the thread that created the specified window into the foreground and activates the window.
        /// Keyboard input is directed to the window, and various visual cues are changed for the user.
        /// The system assigns a slightly higher priority to the thread that created the foreground window than it does to other threads.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        /// <summary>
        /// Contains window information.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            /// <summary>
            /// The size of the structure, in bytes. The caller must set this member to sizeof(WINDOWINFO).
            /// </summary>
            public uint cbSize;
            /// <summary>
            /// The coordinates of the window.
            /// </summary>
            public Rectangle rcWindow;
            /// <summary>
            /// The coordinates of the client area.
            /// </summary>
            public Rectangle rcClient;
            /// <summary>
            /// The window styles. For a table of window styles, see Window Styles.
            /// </summary>
            public uint dwStyle;
            /// <summary>
            /// The extended window styles. For a table of extended window styles, see Extended Window Styles.
            /// </summary>
            public uint dwExStyle;
            /// <summary>
            /// The window status. If this member is WS_ACTIVECAPTION (0x0001), the window is active. Otherwise, this member is zero.
            /// </summary>
            public uint dwWindowStatus;
            /// <summary>
            /// The width of the window border, in pixels.
            /// </summary>
            public uint cxWindowBorders;
            /// <summary>
            /// The height of the window border, in pixels.
            /// </summary>
            public uint cyWindowBorders;
            /// <summary>
            /// The window class atom (see RegisterClass).
            /// </summary>
            public uint atomWindowType;
            /// <summary>
            /// The Windows version of the application that created the window.
            /// </summary>
            public uint wCreatorVersion;
        }
        /// <summary>
        /// Retrieves information about the specified window.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose information is to be retrieved.</param>
        /// <param name="pwi">A pointer to a WINDOWINFO structure to receive the information. Note that you must set the cbSize member to sizeof(WINDOWINFO) before calling this function.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hWnd, out WINDOWINFO pwi);
        /// <summary>
        /// Retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window.
        /// </summary>
        /// <param name="hWnd">A handle to a window. The window handle retrieved is relative to this window, based on the value of the uCmd parameter.</param>
        /// <param name="uCmd">The relationship between the specified window and the window whose handle is to be retrieved. This parameter can be one of the following values in LolibarEnums.GW_Enum.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, LolibarEnums.GW_Enum uCmd);
        /// <summary>
        /// Determines the visibility state of the specified window.
        /// </summary>
        /// <param name="hWnd">A handle to a window.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fAltTab"></param>

        [DllImport("User32.dll", SetLastError = true)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
    }
}
