using System.Runtime.InteropServices;

namespace LolibarApp.Source.Tools
{
    public class LolibarExtern
    {
        [DllImport("user32.dll", SetLastError = true)] public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll", SetLastError = true)] public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)] public static extern int GetWindowThreadProcessId(nint hwnd, out uint lpdwProcessId);
        [DllImport("user32.dll", SetLastError = true)] public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll",  SetLastError = true)] public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    }
}
