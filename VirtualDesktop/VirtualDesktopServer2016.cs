// Author: Markus Scholtes, 2024
// Version 1.19, 2024-09-01
// Version for Windows 10 1607 to 1709 or Windows Server 2016
// Compile with:
// C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe VirtualDesktopServer20161607.cs

using System.Runtime.InteropServices;
using System.Text;

// Based on http://stackoverflow.com/a/32417530, Windows 10 SDK, github project Grabacr07/VirtualDesktopServer2016 and own research

namespace VirtualDesktopServer2016
{
	#region COM API
	internal static class Guids
	{
		public static readonly Guid CLSID_ImmersiveShell = new Guid("C2F03A33-21F5-47FA-B4BB-156362A2F239");
		public static readonly Guid CLSID_VirtualDesktopServer2016ManagerInternal = new Guid("C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B");
		public static readonly Guid CLSID_VirtualDesktopServer2016Manager = new Guid("AA509086-5CA9-4C25-8F95-589D3C07B48A");
		public static readonly Guid CLSID_VirtualDesktopServer2016PinnedApps = new Guid("B5A399E7-1C87-46B8-88E9-FC5747B171BD");
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
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9AC0B5C8-1484-4C5B-9533-4134A0F97CEA")]
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
		int GetVirtualDesktopServer2016Id(out Guid guid);
		int SetVirtualDesktopServer2016Id(ref Guid guid);
		int GetShowInSwitchers(out int flag);
		int SetShowInSwitchers(int flag);
		int GetScaleFactor(out int factor);
		int CanReceiveInput(out bool canReceiveInput);
		int GetCompatibilityPolicyType(out APPLICATION_VIEW_COMPATIBILITY_POLICY flags);
		int SetCompatibilityPolicyType(APPLICATION_VIEW_COMPATIBILITY_POLICY flags);
		int GetPositionPriority(out IntPtr /* IShellPositionerPriority** */ priority);
		int SetPositionPriority(IntPtr /* IShellPositionerPriority* */ priority);
		int GetSizeConstraints(IntPtr /* IImmersiveMonitor* */ monitor, out Size size1, out Size size2);
		int GetSizeConstraintsForDpi(uint uint1, out Size size1, out Size size2);
		int SetSizeConstraintsForDpi(ref uint uint1, ref Size size1, ref Size size2);
		int QuerySizeConstraintsFromApp();
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
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("2C08ADF0-A386-4B35-9250-0FE183476FCC")]
	internal interface IApplicationViewCollection
	{
		int GetViews(out IObjectArray array);
		int GetViewsByZOrder(out IObjectArray array);
		int GetViewsByAppUserModelId(string id, out IObjectArray array);
		int GetViewForHwnd(IntPtr hwnd, out IApplicationView view);
		int GetViewForApplication(object application, out IApplicationView view);
		int GetViewForAppUserModelId(string id, out IApplicationView view);
		int GetViewInFocus(out IntPtr view);
		void RefreshCollection();
		int RegisterForApplicationViewChanges(object listener, out int cookie);
		int RegisterForApplicationViewPositionChanges(object listener, out int cookie);
		int UnregisterForApplicationViewChanges(int cookie);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("FF72FFDD-BE7E-43FC-9C03-AD81681E88E4")]
	internal interface IVirtualDesktopServer2016
	{
		bool IsViewVisible(IApplicationView view);
		Guid GetId();
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("F31574D6-B682-4CDC-BD56-1827860ABEC6")]
	internal interface IVirtualDesktopServer2016ManagerInternal
	{
		int GetCount();
		void MoveViewToDesktop(IApplicationView view, IVirtualDesktopServer2016 desktop);
		bool CanViewMoveDesktops(IApplicationView view);
		IVirtualDesktopServer2016 GetCurrentDesktop();
		void GetDesktops(out IObjectArray desktops);
		[PreserveSig]
		int GetAdjacentDesktop(IVirtualDesktopServer2016 from, int direction, out IVirtualDesktopServer2016 desktop);
		void SwitchDesktop(IVirtualDesktopServer2016 desktop);
		IVirtualDesktopServer2016 CreateDesktop();
		void RemoveDesktop(IVirtualDesktopServer2016 desktop, IVirtualDesktopServer2016 fallback);
		IVirtualDesktopServer2016 FindDesktop(ref Guid desktopid);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A5CD92FF-29BE-454C-8D04-D82879FB3F1B")]
	internal interface IVirtualDesktopServer2016Manager
	{
		bool IsWindowOnCurrentVirtualDesktopServer2016(IntPtr topLevelWindow);
		Guid GetWindowDesktopId(IntPtr topLevelWindow);
		void MoveWindowToDesktop(IntPtr topLevelWindow, ref Guid desktopId);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4CE81583-1E4C-4632-A621-07A53543148F")]
	internal interface IVirtualDesktopServer2016PinnedApps
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
			VirtualDesktopServer2016ManagerInternal = (IVirtualDesktopServer2016ManagerInternal)shell.QueryService(Guids.CLSID_VirtualDesktopServer2016ManagerInternal, typeof(IVirtualDesktopServer2016ManagerInternal).GUID);
			VirtualDesktopServer2016Manager = (IVirtualDesktopServer2016Manager)Activator.CreateInstance(Type.GetTypeFromCLSID(Guids.CLSID_VirtualDesktopServer2016Manager));
			ApplicationViewCollection = (IApplicationViewCollection)shell.QueryService(typeof(IApplicationViewCollection).GUID, typeof(IApplicationViewCollection).GUID);
			VirtualDesktopServer2016PinnedApps = (IVirtualDesktopServer2016PinnedApps)shell.QueryService(Guids.CLSID_VirtualDesktopServer2016PinnedApps, typeof(IVirtualDesktopServer2016PinnedApps).GUID);
		}

		internal static IVirtualDesktopServer2016ManagerInternal VirtualDesktopServer2016ManagerInternal;
		internal static IVirtualDesktopServer2016Manager VirtualDesktopServer2016Manager;
		internal static IApplicationViewCollection ApplicationViewCollection;
		internal static IVirtualDesktopServer2016PinnedApps VirtualDesktopServer2016PinnedApps;

		internal static IVirtualDesktopServer2016 GetDesktop(int index)
		{	// get desktop with index
			int count = VirtualDesktopServer2016ManagerInternal.GetCount();
			if (index < 0 || index >= count) throw new ArgumentOutOfRangeException("index");
			IObjectArray desktops;
			VirtualDesktopServer2016ManagerInternal.GetDesktops(out desktops);
			object objdesktop;
			desktops.GetAt(index, typeof(IVirtualDesktopServer2016).GUID, out objdesktop);
			Marshal.ReleaseComObject(desktops);
			return (IVirtualDesktopServer2016)objdesktop;
		}

		internal static int GetDesktopIndex(IVirtualDesktopServer2016 desktop)
		{ // get index of desktop
			int index = -1;
			Guid IdSearch = desktop.GetId();
			IObjectArray desktops;
			VirtualDesktopServer2016ManagerInternal.GetDesktops(out desktops);
			object objdesktop;
			for (int i = 0; i < VirtualDesktopServer2016ManagerInternal.GetCount(); i++)
			{
				desktops.GetAt(i, typeof(IVirtualDesktopServer2016).GUID, out objdesktop);
				if (IdSearch.CompareTo(((IVirtualDesktopServer2016)objdesktop).GetId()) == 0)
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

		private IVirtualDesktopServer2016 ivd;
		private Desktop(IVirtualDesktopServer2016 desktop) { this.ivd = desktop; }

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
			get { return DesktopManager.VirtualDesktopServer2016ManagerInternal.GetCount(); }
		}

		public static Desktop Current
		{ // returns current desktop
			get { return new Desktop(DesktopManager.VirtualDesktopServer2016ManagerInternal.GetCurrentDesktop()); }
		}

		public static Desktop FromIndex(int index)
		{ // return desktop object from index (-> index = 0..Count-1)
			return new Desktop(DesktopManager.GetDesktop(index));
		}

		public static Desktop FromWindow(IntPtr hWnd)
		{ // return desktop object to desktop on which window <hWnd> is displayed
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			Guid id = DesktopManager.VirtualDesktopServer2016Manager.GetWindowDesktopId(hWnd);
			if ((id.CompareTo(AppOnAllDesktops) == 0) || (id.CompareTo(WindowOnAllDesktops) == 0))
				return new Desktop(DesktopManager.VirtualDesktopServer2016ManagerInternal.GetCurrentDesktop());
			else
				return new Desktop(DesktopManager.VirtualDesktopServer2016ManagerInternal.FindDesktop(ref id));
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
				desktopName = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VirtualDesktopServer2016s\\Desktops\\{" + guid.ToString() + "}", "Name", null);
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
				desktopName = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VirtualDesktopServer2016s\\Desktops\\{" + guid.ToString() + "}", "Name", null);
			}
			catch { }

			// no name found, generate generic name
			if (string.IsNullOrEmpty(desktopName))
			{ // create name "Desktop n" (n = number starting with 1)
				desktopName = "Desktop " + (index + 1).ToString();
			}
			return desktopName;
		}

		public static int SearchDesktop(string partialName)
		{ // get index of desktop with partial name, return -1 if no desktop found
			int index = -1;

			for (int i = 0; i < DesktopManager.VirtualDesktopServer2016ManagerInternal.GetCount(); i++)
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
			return new Desktop(DesktopManager.VirtualDesktopServer2016ManagerInternal.CreateDesktop());
		}

		public void Remove(Desktop fallback = null)
		{ // destroy desktop and switch to <fallback>
			IVirtualDesktopServer2016 fallbackdesktop;
			if (fallback == null)
			{ // if no fallback is given use desktop to the left except for desktop 0.
				Desktop dtToCheck = new Desktop(DesktopManager.GetDesktop(0));
				if (this.Equals(dtToCheck))
				{ // desktop 0: set fallback to second desktop (= "right" desktop)
					DesktopManager.VirtualDesktopServer2016ManagerInternal.GetAdjacentDesktop(ivd, 4, out fallbackdesktop); // 4 = RightDirection
				}
				else
				{ // set fallback to "left" desktop
					DesktopManager.VirtualDesktopServer2016ManagerInternal.GetAdjacentDesktop(ivd, 3, out fallbackdesktop); // 3 = LeftDirection
				}
			}
			else
				// set fallback desktop
				fallbackdesktop = fallback.ivd;

			DesktopManager.VirtualDesktopServer2016ManagerInternal.RemoveDesktop(ivd, fallbackdesktop);
		}

		public static void RemoveAll()
		{ // remove all desktops but visible
			int desktopcount = DesktopManager.VirtualDesktopServer2016ManagerInternal.GetCount();
			int desktopcurrent = DesktopManager.GetDesktopIndex(DesktopManager.VirtualDesktopServer2016ManagerInternal.GetCurrentDesktop());

			if (desktopcurrent < desktopcount-1)
			{ // remove all desktops "right" from current
				for (int i = desktopcount-1; i > desktopcurrent; i--)
					DesktopManager.VirtualDesktopServer2016ManagerInternal.RemoveDesktop(DesktopManager.GetDesktop(i), DesktopManager.VirtualDesktopServer2016ManagerInternal.GetCurrentDesktop());
			}
			if (desktopcurrent > 0)
			{ // remove all desktops "left" from current
				for (int i = 0; i < desktopcurrent; i++)
					DesktopManager.VirtualDesktopServer2016ManagerInternal.RemoveDesktop(DesktopManager.GetDesktop(0), DesktopManager.VirtualDesktopServer2016ManagerInternal.GetCurrentDesktop());
			}
		}

		public bool IsVisible
		{ // return true if this desktop is the current displayed one
			get { return object.ReferenceEquals(ivd, DesktopManager.VirtualDesktopServer2016ManagerInternal.GetCurrentDesktop()); }
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

			DesktopManager.VirtualDesktopServer2016ManagerInternal.SwitchDesktop(ivd);

			// direct desktop to give away focus
			ShowWindow(hWnd, SW_MINIMIZE);
		}

		public Desktop Left
		{ // return desktop at the left of this one, null if none
			get
			{
				IVirtualDesktopServer2016 desktop;
				int hr = DesktopManager.VirtualDesktopServer2016ManagerInternal.GetAdjacentDesktop(ivd, 3, out desktop); // 3 = LeftDirection
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
				IVirtualDesktopServer2016 desktop;
				int hr = DesktopManager.VirtualDesktopServer2016ManagerInternal.GetAdjacentDesktop(ivd, 4, out desktop); // 4 = RightDirection
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
					DesktopManager.VirtualDesktopServer2016Manager.MoveWindowToDesktop(hWnd, ivd.GetId());
				}
				catch // window of process, but we are not the owner
				{
					IApplicationView view;
					DesktopManager.ApplicationViewCollection.GetViewForHwnd(hWnd, out view);
					DesktopManager.VirtualDesktopServer2016ManagerInternal.MoveViewToDesktop(view, ivd);
				}
			}
			else
			{ // window of other process
				IApplicationView view;
				DesktopManager.ApplicationViewCollection.GetViewForHwnd(hWnd, out view);
				try {
					DesktopManager.VirtualDesktopServer2016ManagerInternal.MoveViewToDesktop(view, ivd);
				}
				catch
				{ // could not move active window, try main window (or whatever windows thinks is the main window)
					DesktopManager.ApplicationViewCollection.GetViewForHwnd(System.Diagnostics.Process.GetProcessById(processId).MainWindowHandle, out view);
					DesktopManager.VirtualDesktopServer2016ManagerInternal.MoveViewToDesktop(view, ivd);
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
			Guid id = DesktopManager.VirtualDesktopServer2016Manager.GetWindowDesktopId(hWnd);
			if ((id.CompareTo(AppOnAllDesktops) == 0) || (id.CompareTo(WindowOnAllDesktops) == 0))
				return true;
			else
				return ivd.GetId() == id;
		}

		public static bool IsWindowPinned(IntPtr hWnd)
		{ // return true if window is pinned to all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			return DesktopManager.VirtualDesktopServer2016PinnedApps.IsViewPinned(hWnd.GetApplicationView());
		}

		public static void PinWindow(IntPtr hWnd)
		{ // pin window to all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			var view = hWnd.GetApplicationView();
			if (!DesktopManager.VirtualDesktopServer2016PinnedApps.IsViewPinned(view))
			{ // pin only if not already pinned
				DesktopManager.VirtualDesktopServer2016PinnedApps.PinView(view);
			}
		}

		public static void UnpinWindow(IntPtr hWnd)
		{ // unpin window from all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			var view = hWnd.GetApplicationView();
			if (DesktopManager.VirtualDesktopServer2016PinnedApps.IsViewPinned(view))
			{ // unpin only if not already unpinned
				DesktopManager.VirtualDesktopServer2016PinnedApps.UnpinView(view);
			}
		}

		public static bool IsApplicationPinned(IntPtr hWnd)
		{ // return true if application for window is pinned to all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			return DesktopManager.VirtualDesktopServer2016PinnedApps.IsAppIdPinned(DesktopManager.GetAppId(hWnd));
		}

		public static void PinApplication(IntPtr hWnd)
		{ // pin application for window to all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			string appId = DesktopManager.GetAppId(hWnd);
			if (!DesktopManager.VirtualDesktopServer2016PinnedApps.IsAppIdPinned(appId))
			{ // pin only if not already pinned
				DesktopManager.VirtualDesktopServer2016PinnedApps.PinAppID(appId);
			}
		}

		public static void UnpinApplication(IntPtr hWnd)
		{ // unpin application for window from all desktops
			if (hWnd == IntPtr.Zero) throw new ArgumentNullException();
			var view = hWnd.GetApplicationView();
			string appId = DesktopManager.GetAppId(hWnd);
			if (DesktopManager.VirtualDesktopServer2016PinnedApps.IsAppIdPinned(appId))
			{ // unpin only if pinned
				DesktopManager.VirtualDesktopServer2016PinnedApps.UnpinAppID(appId);
			}
		}
	}
	#endregion
}