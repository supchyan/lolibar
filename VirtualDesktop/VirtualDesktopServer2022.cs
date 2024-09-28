// Author: Markus Scholtes, 2024
// Version 1.19, 2024-09-01
// Version for Windows Server 2022
// Compile with:
// C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe VirtualDesktopServer2022.cs

using System.Runtime.InteropServices;
using System.Text;

// Based on http://stackoverflow.com/a/32417530, Windows 10 SDK, github project Grabacr07/VirtualDesktopServer2022 and own research

namespace VirtualDesktopServer2022
{
	#region COM API
	internal static class Guids
	{
		public static readonly Guid CLSID_ImmersiveShell = new Guid("C2F03A33-21F5-47FA-B4BB-156362A2F239");
		public static readonly Guid CLSID_VirtualDesktopServer2022ManagerInternal = new Guid("C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B");
		public static readonly Guid CLSID_VirtualDesktopServer2022Manager = new Guid("AA509086-5CA9-4C25-8F95-589D3C07B48A");
		public static readonly Guid CLSID_VirtualDesktopServer2022PinnedApps = new Guid("B5A399E7-1C87-46B8-88E9-FC5747B171BD");
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct Size
	{
		public int X;
		public int Y;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct Rect
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}

	internal enum APPLICATION_VIEW_CLOAK_TYPE : int
	{
		AVCT_NONE = 0,
		AVCT_DEFAULT = 1,
		AVCT_VIRTUAL_DESKTOP = 2
	}

	internal enum APPLICATION_VIEW_COMPATIBILITY_POLICY : int
	{
		AVCP_NONE = 0,
		AVCP_SMALL_SCREEN = 1,
		AVCP_TABLET_SMALL_SCREEN = 2,
		AVCP_VERY_SMALL_SCREEN = 3,
		AVCP_HIGH_SCALE_FACTOR = 4
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
	[Guid("372E1D3B-38D3-42E4-A15B-8AB2B178F513")]
	internal interface IApplicationView
	{
		int SetFocus();
		int SwitchTo();
		int TryInvokeBack(IntPtr /* IAsyncCallback* */ callback);
		int GetThumbnailWindow(out IntPtr hwnd);
		int GetMonitor(out IntPtr /* IImmersiveMonitor */ immersiveMonitor);
		int GetVisibility(out int visibility);
		int SetCloak(APPLICATION_VIEW_CLOAK_TYPE cloakType, int unknown);
		int GetPosition(ref Guid guid /* GUID for IApplicationViewPosition */, out IntPtr /* IApplicationViewPosition** */ position);
		int SetPosition(ref IntPtr /* IApplicationViewPosition* */ position);
		int InsertAfterWindow(IntPtr hwnd);
		int GetExtendedFramePosition(out Rect rect);
		int GetAppUserModelId([MarshalAs(UnmanagedType.LPWStr)] out string id);
		int SetAppUserModelId(string id);
		int IsEqualByAppUserModelId(string id, out int result);
		int GetViewState(out uint state);
		int SetViewState(uint state);
		int GetNeediness(out int neediness);
		int GetLastActivationTimestamp(out ulong timestamp);
		int SetLastActivationTimestamp(ulong timestamp);
		int GetVirtualDesktopServer2022Id(out Guid guid);
		int SetVirtualDesktopServer2022Id(ref Guid guid);
		int GetShowInSwitchers(out int flag);
		int SetShowInSwitchers(int flag);
		int GetScaleFactor(out int factor);
		int CanReceiveInput(out bool canReceiveInput);
		int GetCompatibilityPolicyType(out APPLICATION_VIEW_COMPATIBILITY_POLICY flags);
		int SetCompatibilityPolicyType(APPLICATION_VIEW_COMPATIBILITY_POLICY flags);
		int GetSizeConstraints(IntPtr /* IImmersiveMonitor* */ monitor, out Size size1, out Size size2);
		int GetSizeConstraintsForDpi(uint uint1, out Size size1, out Size size2);
		int SetSizeConstraintsForDpi(ref uint uint1, ref Size size1, ref Size size2);
		int OnMinSizePreferencesUpdated(IntPtr hwnd);
		int ApplyOperation(IntPtr /* IApplicationViewOperation* */ operation);
		int IsTray(out bool isTray);
		int IsInHighZOrderBand(out bool isInHighZOrderBand);
		int IsSplashScreenPresented(out bool isSplashScreenPresented);
		int Flash();
		int GetRootSwitchableOwner(out IApplicationView rootSwitchableOwner);
		int EnumerateOwnershipTree(out IObjectArray ownershipTree);
		int GetEnterpriseId([MarshalAs(UnmanagedType.LPWStr)] out string enterpriseId);
		int IsMirrored(out bool isMirrored);
		int Unknown1(out int unknown);
		int Unknown2(out int unknown);
		int Unknown3(out int unknown);
		int Unknown4(out int unknown);
		int Unknown5(out int unknown);
		int Unknown6(int unknown);
		int Unknown7();
		int Unknown8(out int unknown);
		int Unknown9(int unknown);
		int Unknown10(int unknownX, int unknownY);
		int Unknown11(int unknown);
		int Unknown12(out Size size1);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1841C6D7-4F9D-42C0-AF41-8747538F10E5")]
	internal interface IApplicationViewCollection
	{
		int GetViews(out IObjectArray array);
		int GetViewsByZOrder(out IObjectArray array);
		int GetViewsByAppUserModelId(string id, out IObjectArray array);
		int GetViewForHwnd(IntPtr hwnd, out IApplicationView view);
		int GetViewForApplication(object application, out IApplicationView view);
		int GetViewForAppUserModelId(string id, out IApplicationView view);
		int GetViewInFocus(out IntPtr view);
		int Unknown1(out IntPtr view);
		void RefreshCollection();
		int RegisterForApplicationViewChanges(object listener, out int cookie);
		int UnregisterForApplicationViewChanges(int cookie);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("62fdf88b-11ca-4afb-8bd8-2296dfae49e2")]
	internal interface IVirtualDesktopServer2022
	{
		bool IsViewVisible(IApplicationView view);
		Guid GetId();
		IntPtr Unknown1();
		[return: MarshalAs(UnmanagedType.HString)]
		string GetName();
	}

/*
IVirtualDesktopServer20222 not used now (available since Win 10 2004), instead reading names out of registry for compatibility reasons
Excample code:
IVirtualDesktopServer20222 ivd2;
string desktopName;
ivd2.GetName(out desktopName);
Console.WriteLine("Name of desktop: " + desktopName);

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("31EBDE3F-6EC3-4CBD-B9FB-0EF6D09B41F4")]
	internal interface IVirtualDesktopServer20222
	{
		bool IsViewVisible(IApplicationView view);
		Guid GetId();
		void GetName([MarshalAs(UnmanagedType.HString)] out string name);
	}
*/

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("094afe11-44f2-4ba0-976f-29a97e263ee0")]
	internal interface IVirtualDesktopServer2022ManagerInternal
	{
		int GetCount(IntPtr hWndOrMon);
		void MoveViewToDesktop(IApplicationView view, IVirtualDesktopServer2022 desktop);
		bool CanViewMoveDesktops(IApplicationView view);
		IVirtualDesktopServer2022 GetCurrentDesktop(IntPtr hWndOrMon);
		void GetDesktops(IntPtr hWndOrMon, out IObjectArray desktops);
		[PreserveSig]
		int GetAdjacentDesktop(IVirtualDesktopServer2022 from, int direction, out IVirtualDesktopServer2022 desktop);
		void SwitchDesktop(IntPtr hWndOrMon, IVirtualDesktopServer2022 desktop);
		IVirtualDesktopServer2022 CreateDesktop(IntPtr hWndOrMon);
		void RemoveDesktop(IVirtualDesktopServer2022 desktop, IVirtualDesktopServer2022 fallback);
		IVirtualDesktopServer2022 FindDesktop(ref Guid desktopid);
		void GetDesktopSwitchIncludeExcludeViews(IVirtualDesktopServer2022 desktop, out IObjectArray unknown1, out IObjectArray unknown2);
		void SetDesktopName(IVirtualDesktopServer2022 desktop, [MarshalAs(UnmanagedType.HString)] string name);
		void CopyDesktopState(IApplicationView pView0, IApplicationView pView1);
		int GetDesktopIsPerMonitor();
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A5CD92FF-29BE-454C-8D04-D82879FB3F1B")]
	internal interface IVirtualDesktopServer2022Manager
	{
		bool IsWindowOnCurrentVirtualDesktopServer2022(IntPtr topLevelWindow);
		Guid GetWindowDesktopId(IntPtr topLevelWindow);
		void MoveWindowToDesktop(IntPtr topLevelWindow, ref Guid desktopId);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4CE81583-1E4C-4632-A621-07A53543148F")]
	internal interface IVirtualDesktopServer2022PinnedApps
	{
		bool IsAppIdPinned(string appId);
		void PinAppID(string appId);
		void UnpinAppID(string appId);
		bool IsViewPinned(IApplicationView applicationView);
		void PinView(IApplicationView applicationView);
		void UnpinView(IApplicationView applicationView);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")]
	internal interface IObjectArray
	{
		void GetCount(out int count);
		void GetAt(int index, ref Guid iid, [MarshalAs(UnmanagedType.Interface)]out object obj);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
	internal interface IServiceProvider10
	{
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object QueryService(ref Guid service, ref Guid riid);
	}
	#endregion

	#region COM wrapper
	internal static class DesktopManager
	{
		static DesktopManager()
		{
			var shell = (IServiceProvider10)Activator.CreateInstance(Type.GetTypeFromCLSID(Guids.CLSID_ImmersiveShell));
			VirtualDesktopServer2022ManagerInternal = (IVirtualDesktopServer2022ManagerInternal)shell.QueryService(Guids.CLSID_VirtualDesktopServer2022ManagerInternal, typeof(IVirtualDesktopServer2022ManagerInternal).GUID);
			VirtualDesktopServer2022Manager = (IVirtualDesktopServer2022Manager)Activator.CreateInstance(Type.GetTypeFromCLSID(Guids.CLSID_VirtualDesktopServer2022Manager));
			ApplicationViewCollection = (IApplicationViewCollection)shell.QueryService(typeof(IApplicationViewCollection).GUID, typeof(IApplicationViewCollection).GUID);
			VirtualDesktopServer2022PinnedApps = (IVirtualDesktopServer2022PinnedApps)shell.QueryService(Guids.CLSID_VirtualDesktopServer2022PinnedApps, typeof(IVirtualDesktopServer2022PinnedApps).GUID);
		}

		internal static IVirtualDesktopServer2022ManagerInternal VirtualDesktopServer2022ManagerInternal;
		internal static IVirtualDesktopServer2022Manager VirtualDesktopServer2022Manager;
		internal static IApplicationViewCollection ApplicationViewCollection;
		internal static IVirtualDesktopServer2022PinnedApps VirtualDesktopServer2022PinnedApps;

		internal static IVirtualDesktopServer2022 GetDesktop(int index)
		{	// get desktop with index
			int count = VirtualDesktopServer2022ManagerInternal.GetCount(IntPtr.Zero);
			if (index < 0 || index >= count) throw new ArgumentOutOfRangeException("index");
			IObjectArray desktops;
			VirtualDesktopServer2022ManagerInternal.GetDesktops(IntPtr.Zero, out desktops);
			object objdesktop;
			desktops.GetAt(index, typeof(IVirtualDesktopServer2022).GUID, out objdesktop);
			Marshal.ReleaseComObject(desktops);
			return (IVirtualDesktopServer2022)objdesktop;
		}

		internal static int GetDesktopIndex(IVirtualDesktopServer2022 desktop)
		{ // get index of desktop
			int index = -1;
			Guid IdSearch = desktop.GetId();
			IObjectArray desktops;
			VirtualDesktopServer2022ManagerInternal.GetDesktops(IntPtr.Zero, out desktops);
			object objdesktop;
			for (int i = 0; i < VirtualDesktopServer2022ManagerInternal.GetCount(IntPtr.Zero); i++)
			{
				desktops.GetAt(i, typeof(IVirtualDesktopServer2022).GUID, out objdesktop);
				if (IdSearch.CompareTo(((IVirtualDesktopServer2022)objdesktop).GetId()) == 0)
				{ index = i;
					break;
				}
			}
			Marshal.ReleaseComObject(desktops);
			return index;
		}

		internal static IApplicationView GetApplicationView(this IntPtr hWnd)
		{ // get application view to window handle
			IApplicationView view;
			ApplicationViewCollection.GetViewForHwnd(hWnd, out view);
			return view;
		}

		internal static string GetAppId(IntPtr hWnd)
		{ // get Application ID to window handle
			string appId;
			hWnd.GetApplicationView().GetAppUserModelId(out appId);
			return appId;
		}
	}
	#endregion

	#region public interface
	public class Desktop
	{
		// get window handle to class and window name
		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// get process id to window handle
		[DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

		// get thread id of current process
		[DllImport("kernel32.dll")]
		static extern uint GetCurrentThreadId();

		// attach input to thread
		[DllImport("user32.dll")]
		static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

		// get handle of active window
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		// try to set foreground window
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]static extern bool SetForegroundWindow(IntPtr hWnd);

		// send message to window
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		private const int SW_MINIMIZE = 6;

		private static readonly Guid AppOnAllDesktops = new Guid("BB64D5B7-4DE3-4AB2-A87C-DB7601AEA7DC");
		private static readonly Guid WindowOnAllDesktops = new Guid("C2DDEA68-66F2-4CF9-8264-1BFD00FBBBAC");

		private IVirtualDesktopServer2022 ivd;
		private Desktop(IVirtualDesktopServer2022 desktop) { this.ivd = desktop; }

		public override int GetHashCode()
		{ // get hash
			return ivd.GetHashCode();
		}

		public override bool Equals(object obj)
		{ // compare with object
			var desk = obj as Desktop;
			return desk != null && object.ReferenceEquals(this.ivd, desk.ivd);
		}

		public static int Count
		{ // return the number of desktops
			get { return DesktopManager.VirtualDesktopServer2022ManagerInternal.GetCount(IntPtr.Zero); }
		}

		public static Desktop Current
		{ // returns current desktop
			get { return new Desktop(DesktopManager.VirtualDesktopServer2022ManagerInternal.GetCurrentDesktop(IntPtr.Zero)); }
		}

		public static Desktop FromIndex(int index)
		{ // return desktop object from index (-> index = 0..Count-1)
			return new Desktop(DesktopManager.GetDesktop(index));
		}

		public static Desktop FromWindow(IntPtr hWnd)
		{ // return desktop object to desktop on which window <hWnd> is displayed
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			Guid id = DesktopManager.VirtualDesktopServer2022Manager.GetWindowDesktopId(hWnd);
			if ((id.CompareTo(AppOnAllDesktops) == 0) || (id.CompareTo(WindowOnAllDesktops) == 0))
				return new Desktop(DesktopManager.VirtualDesktopServer2022ManagerInternal.GetCurrentDesktop(IntPtr.Zero));
			else
				return new Desktop(DesktopManager.VirtualDesktopServer2022ManagerInternal.FindDesktop(ref id));
		}

		public static int FromDesktop(Desktop desktop)
		{ // return index of desktop object or -1 if not found
			return DesktopManager.GetDesktopIndex(desktop.ivd);
		}

		public static string DesktopNameFromDesktop(Desktop desktop)
		{ // return name of desktop or "Desktop n" if it has no name
			Guid guid = desktop.ivd.GetId();

			// read desktop name in registry
			string desktopName = null;
			try {
				desktopName = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VirtualDesktopServer2022s\\Desktops\\{" + guid.ToString() + "}", "Name", null);
			}
			catch { }

			// no name found, generate generic name
			if (string.IsNullOrEmpty(desktopName))
			{ // create name "Desktop n" (n = number starting with 1)
				desktopName = "Desktop " + (DesktopManager.GetDesktopIndex(desktop.ivd) + 1).ToString();
			}
			return desktopName;
		}

		public static string DesktopNameFromIndex(int index)
		{ // return name of desktop from index (-> index = 0..Count-1) or "Desktop n" if it has no name
			Guid guid = DesktopManager.GetDesktop(index).GetId();

			// read desktop name in registry
			string desktopName = null;
			try {
				desktopName = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VirtualDesktopServer2022s\\Desktops\\{" + guid.ToString() + "}", "Name", null);
			}
			catch { }

			// no name found, generate generic name
			if (string.IsNullOrEmpty(desktopName))
			{ // create name "Desktop n" (n = number starting with 1)
				desktopName = "Desktop " + (index + 1).ToString();
			}
			return desktopName;
		}

		public static bool HasDesktopNameFromIndex(int index)
		{ // return true is desktop is named or false if it has no name
			Guid guid = DesktopManager.GetDesktop(index).GetId();

			// read desktop name in registry
			string desktopName = null;
			try {
				desktopName = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VirtualDesktopServer2022s\\Desktops\\{" + guid.ToString() + "}", "Name", null);
			}
			catch { }

			// name found?
			if (string.IsNullOrEmpty(desktopName))
				return false;
			else
				return true;
		}

		public static int SearchDesktop(string partialName)
		{ // get index of desktop with partial name, return -1 if no desktop found
			int index = -1;

			for (int i = 0; i < DesktopManager.VirtualDesktopServer2022ManagerInternal.GetCount(IntPtr.Zero); i++)
			{ // loop through all virtual desktops and compare partial name to desktop name
				if (DesktopNameFromIndex(i).ToUpper().IndexOf(partialName.ToUpper()) >= 0)
				{ index = i;
					break;
				}
			}

			return index;
		}

		public static Desktop Create()
		{ // create a new desktop
			return new Desktop(DesktopManager.VirtualDesktopServer2022ManagerInternal.CreateDesktop(IntPtr.Zero));
		}

		public void Remove(Desktop fallback = null)
		{ // destroy desktop and switch to <fallback>
			IVirtualDesktopServer2022 fallbackdesktop;
			if (fallback == null)
			{ // if no fallback is given use desktop to the left except for desktop 0.
				Desktop dtToCheck = new Desktop(DesktopManager.GetDesktop(0));
				if (this.Equals(dtToCheck))
				{ // desktop 0: set fallback to second desktop (= "right" desktop)
					DesktopManager.VirtualDesktopServer2022ManagerInternal.GetAdjacentDesktop(ivd, 4, out fallbackdesktop); // 4 = RightDirection
				}
				else
				{ // set fallback to "left" desktop
					DesktopManager.VirtualDesktopServer2022ManagerInternal.GetAdjacentDesktop(ivd, 3, out fallbackdesktop); // 3 = LeftDirection
				}
			}
			else
				// set fallback desktop
				fallbackdesktop = fallback.ivd;

			DesktopManager.VirtualDesktopServer2022ManagerInternal.RemoveDesktop(ivd, fallbackdesktop);
		}

		public static void RemoveAll()
		{ // remove all desktops but visible
			int desktopcount = DesktopManager.VirtualDesktopServer2022ManagerInternal.GetCount(IntPtr.Zero);
			int desktopcurrent = DesktopManager.GetDesktopIndex(DesktopManager.VirtualDesktopServer2022ManagerInternal.GetCurrentDesktop(IntPtr.Zero));

			if (desktopcurrent < desktopcount-1)
			{ // remove all desktops "right" from current
				for (int i = desktopcount-1; i > desktopcurrent; i--)
					DesktopManager.VirtualDesktopServer2022ManagerInternal.RemoveDesktop(DesktopManager.GetDesktop(i), DesktopManager.VirtualDesktopServer2022ManagerInternal.GetCurrentDesktop(IntPtr.Zero));
			}
			if (desktopcurrent > 0)
			{ // remove all desktops "left" from current
				for (int i = 0; i < desktopcurrent; i++)
					DesktopManager.VirtualDesktopServer2022ManagerInternal.RemoveDesktop(DesktopManager.GetDesktop(0), DesktopManager.VirtualDesktopServer2022ManagerInternal.GetCurrentDesktop(IntPtr.Zero));
			}
		}

		public void SetName(string Name)
		{ // set name for desktop, empty string removes name
			DesktopManager.VirtualDesktopServer2022ManagerInternal.SetDesktopName(this.ivd, Name);
		}

		public bool IsVisible
		{ // return true if this desktop is the current displayed one
			get { return object.ReferenceEquals(ivd, DesktopManager.VirtualDesktopServer2022ManagerInternal.GetCurrentDesktop(IntPtr.Zero)); }
		}

		public void MakeVisible()
		{ // make this desktop visible
			IntPtr hWnd = FindWindow("Progman", "Program Manager");

			// activate desktop to prevent flashing icons in taskbar
			int dummy;
			uint DesktopThreadId = GetWindowThreadProcessId(hWnd, out dummy);
			uint ForegroundThreadId = GetWindowThreadProcessId(GetForegroundWindow(), out dummy);
			uint CurrentThreadId = GetCurrentThreadId();

			if ((DesktopThreadId != 0) && (ForegroundThreadId != 0) && (ForegroundThreadId != CurrentThreadId))
			{
				AttachThreadInput(DesktopThreadId, CurrentThreadId, true);
				AttachThreadInput(ForegroundThreadId, CurrentThreadId, true);
				SetForegroundWindow(hWnd);
				AttachThreadInput(ForegroundThreadId, CurrentThreadId, false);
				AttachThreadInput(DesktopThreadId, CurrentThreadId, false);
			}

			DesktopManager.VirtualDesktopServer2022ManagerInternal.SwitchDesktop(IntPtr.Zero, ivd);

			// direct desktop to give away focus
			ShowWindow(hWnd, SW_MINIMIZE);
		}

		public Desktop Left
		{ // return desktop at the left of this one, null if none
			get
			{
				IVirtualDesktopServer2022 desktop;
				int hr = DesktopManager.VirtualDesktopServer2022ManagerInternal.GetAdjacentDesktop(ivd, 3, out desktop); // 3 = LeftDirection
				if (hr == 0)
					return new Desktop(desktop);
				else
					return null;
			}
		}

		public Desktop Right
		{ // return desktop at the right of this one, null if none
			get
			{
				IVirtualDesktopServer2022 desktop;
				int hr = DesktopManager.VirtualDesktopServer2022ManagerInternal.GetAdjacentDesktop(ivd, 4, out desktop); // 4 = RightDirection
				if (hr == 0)
					return new Desktop(desktop);
				else
					return null;
			}
		}

		public void MoveWindow(IntPtr hWnd)
		{ // move window to this desktop
			int processId;
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			GetWindowThreadProcessId(hWnd, out processId);

			if (System.Diagnostics.Process.GetCurrentProcess().Id == processId)
			{ // window of process
				try // the easy way (if we are owner)
				{
					DesktopManager.VirtualDesktopServer2022Manager.MoveWindowToDesktop(hWnd, ivd.GetId());
				}
				catch // window of process, but we are not the owner
				{
					IApplicationView view;
					DesktopManager.ApplicationViewCollection.GetViewForHwnd(hWnd, out view);
					DesktopManager.VirtualDesktopServer2022ManagerInternal.MoveViewToDesktop(view, ivd);
				}
			}
			else
			{ // window of other process
				IApplicationView view;
				DesktopManager.ApplicationViewCollection.GetViewForHwnd(hWnd, out view);
				try {
					DesktopManager.VirtualDesktopServer2022ManagerInternal.MoveViewToDesktop(view, ivd);
				}
				catch
				{ // could not move active window, try main window (or whatever windows thinks is the main window)
					DesktopManager.ApplicationViewCollection.GetViewForHwnd(System.Diagnostics.Process.GetProcessById(processId).MainWindowHandle, out view);
					DesktopManager.VirtualDesktopServer2022ManagerInternal.MoveViewToDesktop(view, ivd);
				}
			}
		}

		public void MoveActiveWindow()
		{ // move active window to this desktop
			MoveWindow(GetForegroundWindow());
		}

		public bool HasWindow(IntPtr hWnd)
		{ // return true if window is on this desktop
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			Guid id = DesktopManager.VirtualDesktopServer2022Manager.GetWindowDesktopId(hWnd);
			if ((id.CompareTo(AppOnAllDesktops) == 0) || (id.CompareTo(WindowOnAllDesktops) == 0))
				return true;
			else
				return ivd.GetId() == id;
		}

		public static bool IsWindowPinned(IntPtr hWnd)
		{ // return true if window is pinned to all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			return DesktopManager.VirtualDesktopServer2022PinnedApps.IsViewPinned(hWnd.GetApplicationView());
		}

		public static void PinWindow(IntPtr hWnd)
		{ // pin window to all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			var view = hWnd.GetApplicationView();
			if (!DesktopManager.VirtualDesktopServer2022PinnedApps.IsViewPinned(view))
			{ // pin only if not already pinned
				DesktopManager.VirtualDesktopServer2022PinnedApps.PinView(view);
			}
		}

		public static void UnpinWindow(IntPtr hWnd)
		{ // unpin window from all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			var view = hWnd.GetApplicationView();
			if (DesktopManager.VirtualDesktopServer2022PinnedApps.IsViewPinned(view))
			{ // unpin only if not already unpinned
				DesktopManager.VirtualDesktopServer2022PinnedApps.UnpinView(view);
			}
		}

		public static bool IsApplicationPinned(IntPtr hWnd)
		{ // return true if application for window is pinned to all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			return DesktopManager.VirtualDesktopServer2022PinnedApps.IsAppIdPinned(DesktopManager.GetAppId(hWnd));
		}

		public static void PinApplication(IntPtr hWnd)
		{ // pin application for window to all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			string appId = DesktopManager.GetAppId(hWnd);
			if (!DesktopManager.VirtualDesktopServer2022PinnedApps.IsAppIdPinned(appId))
			{ // pin only if not already pinned
				DesktopManager.VirtualDesktopServer2022PinnedApps.PinAppID(appId);
			}
		}

		public static void UnpinApplication(IntPtr hWnd)
		{ // unpin application for window from all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			var view = hWnd.GetApplicationView();
			string appId = DesktopManager.GetAppId(hWnd);
			if (DesktopManager.VirtualDesktopServer2022PinnedApps.IsAppIdPinned(appId))
			{ // unpin only if pinned
				DesktopManager.VirtualDesktopServer2022PinnedApps.UnpinAppID(appId);
			}
		}
	}
	#endregion
}