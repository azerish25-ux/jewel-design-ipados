using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace GV.CounterSketch.Shared;

public static class MonitorHelper
{
	[Flags]
	public enum DISPLAY_DEVICE_STATE_FLAGS
	{
		AttachedToDesktop = 1,
		MultiDriver = 2,
		PrimaryDevice = 4,
		MirroringDriver = 8,
		VGACompatible = 0x16,
		Removable = 0x20,
		ModesPruned = 0x8000000,
		Remote = 0x4000000,
		Disconnect = 0x2000000
	}

	public struct DEVMODE
	{
		public int dmBitsPerPel;

		private readonly short dmCollate;

		private readonly short dmColor;

		private readonly short dmCopies;

		private readonly short dmDefaultSource;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string dmDeviceName;

		public int dmDisplayFlags;

		public int dmDisplayFrequency;

		public int dmDisplayOrientation;

		public short dmDriverExtra;

		public short dmDriverVersion;

		private readonly short dmDuplex;

		public int dmFields;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string dmFormName;

		public short dmLogPixels;

		private readonly short dmOrientation;

		private readonly short dmPaperLength;

		private readonly short dmPaperSize;

		private readonly short dmPaperWidth;

		public int dmPelsHeight;

		public int dmPelsWidth;

		private readonly short dmPrintQuality;

		private readonly short dmScale;

		public short dmSize;

		public short dmSpecVersion;

		private readonly short dmTTOption;

		private readonly short dmYResolution;
	}

	public struct DISPLAY_DEVICE
	{
		[MarshalAs(UnmanagedType.U4)]
		public int cb;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceKey;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DeviceName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceString;

		[MarshalAs(UnmanagedType.U4)]
		public DISPLAY_DEVICE_STATE_FLAGS StateFlags;
	}

	public struct Minmaxinfo
	{
		public PointData ptMaxPosition;

		public PointData ptMaxSize;

		public PointData ptMaxTrackSize;

		public PointData ptMinTrackSize;

		public PointData ptReserved;
	}

	private delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public sealed class Monitorinfo
	{
		public int cbSize = Marshal.SizeOf(typeof(Monitorinfo));

		public RectData rcMonitor;

		public RectData rcWork;

		public int dwFlags;
	}

	public struct PointData
	{
		public int x;

		public int y;

		public PointData(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public struct RectData
	{
		public int bottom;

		public static readonly RectData Empty;

		public int left;

		public int right;

		public int top;

		public int Height => Math.Abs(bottom - top);

		public bool IsEmpty
		{
			get
			{
				if (left < right)
				{
					return top >= bottom;
				}
				return true;
			}
		}

		public int Width => Math.Abs(right - left);

		public static bool operator !=(RectData rect1, RectData rect2)
		{
			return !(rect1 == rect2);
		}

		public static bool operator ==(RectData rect1, RectData rect2)
		{
			if (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right)
			{
				return rect1.bottom == rect2.bottom;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Rect))
			{
				return false;
			}
			return this == (RectData)obj;
		}

		public override int GetHashCode()
		{
			return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
		}

		public RectData(RectData rcSrc)
		{
			left = rcSrc.left;
			top = rcSrc.top;
			right = rcSrc.right;
			bottom = rcSrc.bottom;
		}

		public RectData(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public override string ToString()
		{
			if (this == Empty)
			{
				return "RectData {Empty}";
			}
			return "RectData { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
		}
	}

	private const int CHANGE_DISPLAY_SETTINGS_TEST = 2;

	private const int DISPLAY_QUERY_ORIENTATION = 16777216;

	private const int ENUM_CURRENT_SETTINGS = -1;

	private const int ENUM_REGISTRY_SETTINGS = -2;

	private const int MonitorDefaultToNearest = 2;

	[DllImport("user32.dll")]
	public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, int dwflags, IntPtr lParam);

	[DllImport("user32.dll")]
	public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

	[DllImport("user32.dll")]
	public static extern bool EnumDisplaySettingsEx([MarshalAs(UnmanagedType.LPStr)] string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode, uint dwFlags);

	public static RectData GetCurrentMonitorSize(Window sourceWindow)
	{
		return GetCurrentMonitorSize(new WindowInteropHelper(sourceWindow).Handle);
	}

	public static RectData GetCurrentMonitorSize(IntPtr windowHandle)
	{
		IntPtr intPtr = MonitorFromWindow(windowHandle, 2);
		if (intPtr != IntPtr.Zero)
		{
			Monitorinfo monitorinfo = new Monitorinfo();
			GetMonitorInfo(intPtr, monitorinfo);
			RectData rcWork = monitorinfo.rcWork;
			RectData rcMonitor = monitorinfo.rcMonitor;
			if (monitorinfo.dwFlags != 1)
			{
				rcWork.right = Math.Abs(rcWork.right - rcWork.bottom);
				rcWork.bottom = 0;
				rcWork.top = Math.Abs(rcWork.top);
				rcWork.left = Math.Abs(rcWork.left);
				rcMonitor.right = Math.Abs(rcMonitor.bottom);
				rcMonitor.bottom = 0;
			}
			TaskBarHelper.TaskbarState taskbarState = TaskBarHelper.GetTaskbarState();
			if (taskbarState.T_SIZE.Top == 0 && taskbarState.T_SIZE.Left == 0)
			{
				rcWork.left = rcWork.bottom;
				rcWork.bottom = rcWork.top;
				rcWork.top = 0;
				rcMonitor.bottom = rcMonitor.top;
				rcMonitor.top = 0;
				rcMonitor.left = 0;
			}
			return rcWork;
		}
		return default(RectData);
	}

	public static string GetDisplayDevices()
	{
		string text = string.Empty;
		DISPLAY_DEVICE lpDisplayDevice = default(DISPLAY_DEVICE);
		lpDisplayDevice.cb = Marshal.SizeOf(lpDisplayDevice);
		try
		{
			for (uint num = 0u; EnumDisplayDevices(null, num, ref lpDisplayDevice, 0u); num++)
			{
				text += $"{num}, {lpDisplayDevice.DeviceName}, {lpDisplayDevice.DeviceString}, {lpDisplayDevice.StateFlags}, {lpDisplayDevice.DeviceID}, {lpDisplayDevice.DeviceKey}\n";
				lpDisplayDevice.cb = Marshal.SizeOf(lpDisplayDevice);
			}
		}
		catch (Exception)
		{
		}
		return text;
	}

	[DllImport("user32.dll")]
	public static extern bool GetMonitorInfo(IntPtr hMonitor, Monitorinfo lpmi);

	public static int GetMonitorOrientation()
	{
		DEVMODE devMode = new DEVMODE
		{
			dmSize = (short)Marshal.SizeOf(typeof(DEVMODE))
		};
		if (EnumDisplaySettings(null, 2, ref devMode))
		{
			return devMode.dmDisplayOrientation * 90;
		}
		return devMode.dmDisplayOrientation;
	}

	[DllImport("User32.dll")]
	public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

	public static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
	{
		Minmaxinfo structure = (Minmaxinfo)Marshal.PtrToStructure(lParam, typeof(Minmaxinfo));
		IntPtr intPtr = MonitorFromWindow(hwnd, 2);
		if (!(intPtr != IntPtr.Zero))
		{
			return;
		}
		Monitorinfo monitorinfo = new Monitorinfo();
		GetMonitorInfo(intPtr, monitorinfo);
		RectData rcWork = monitorinfo.rcWork;
		RectData rcMonitor = monitorinfo.rcMonitor;
		if (monitorinfo.dwFlags != 1)
		{
			Marshal.StructureToPtr(structure, lParam, fDeleteOld: true);
			return;
		}
		TaskBarHelper.TaskbarState taskbarState = TaskBarHelper.GetTaskbarState();
		if (taskbarState.T_SIZE.Top == 0 && taskbarState.T_SIZE.Left == 0)
		{
			rcWork.left = rcWork.bottom;
			rcWork.bottom = rcWork.top;
			rcWork.top = 0;
			rcMonitor.bottom = rcMonitor.top;
			rcMonitor.top = 0;
			rcMonitor.left = 0;
			structure.ptMaxPosition.x = Math.Abs(rcWork.left - rcMonitor.left);
			structure.ptMaxPosition.y = Math.Abs(rcWork.top - rcMonitor.top);
			structure.ptMaxSize.x = Math.Abs(rcWork.right - rcWork.left);
			structure.ptMaxSize.y = Math.Abs(rcWork.bottom - rcWork.top);
			structure.ptMaxTrackSize.x = rcWork.left;
			Marshal.StructureToPtr(structure, lParam, fDeleteOld: true);
		}
		else
		{
			structure.ptMaxPosition.x = Math.Abs(rcWork.left - rcMonitor.left);
			structure.ptMaxPosition.y = Math.Abs(rcWork.top - rcMonitor.top);
			structure.ptMaxSize.x = Math.Abs(rcWork.right - rcWork.left);
			structure.ptMaxSize.y = Math.Abs(rcWork.bottom - rcWork.top);
			Marshal.StructureToPtr(structure, lParam, fDeleteOld: true);
		}
	}

	[DllImport("user32.dll")]
	private static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

	[DllImport("user32.dll")]
	private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);
}
