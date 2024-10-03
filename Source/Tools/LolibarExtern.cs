using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
