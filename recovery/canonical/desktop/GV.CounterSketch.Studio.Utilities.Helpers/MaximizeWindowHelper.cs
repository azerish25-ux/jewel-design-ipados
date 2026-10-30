using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class MaximizeWindowHelper
{
	public struct POINT
	{
		public int x;

		public int y;

		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public struct MINMAXINFO
	{
		public POINT ptReserved;

		public POINT ptMaxSize;

		public POINT ptMaxPosition;

		public POINT ptMinTrackSize;

		public POINT ptMaxTrackSize;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class MONITORINFO
	{
		public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

		public RECT rcMonitor;

		public RECT rcWork;

		public int dwFlags;
	}

	public struct RECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;

		public static readonly RECT Empty;

		public int Width => Math.Abs(right - left);

		public int Height => bottom - top;

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

		public RECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public RECT(RECT rcSrc)
		{
			left = rcSrc.left;
			top = rcSrc.top;
			right = rcSrc.right;
			bottom = rcSrc.bottom;
		}

		public override string ToString()
		{
			if (this == Empty)
			{
				return "RECT {Empty}";
			}
			return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Rect))
			{
				return false;
			}
			return this == (RECT)obj;
		}

		public override int GetHashCode()
		{
			return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
		}

		public static bool operator ==(RECT rect1, RECT rect2)
		{
			if (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right)
			{
				return rect1.bottom == rect2.bottom;
			}
			return false;
		}

		public static bool operator !=(RECT rect1, RECT rect2)
		{
			return !(rect1 == rect2);
		}
	}

	[DllImport("user32")]
	internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

	[DllImport("user32")]
	internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

	public static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam, int minWidth, int minHeight)
	{
		MINMAXINFO structure = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
		int flags = 2;
		IntPtr intPtr = MonitorFromWindow(hwnd, flags);
		if (intPtr != IntPtr.Zero)
		{
			MONITORINFO mONITORINFO = new MONITORINFO();
			GetMonitorInfo(intPtr, mONITORINFO);
			RECT rcWork = mONITORINFO.rcWork;
			RECT rcMonitor = mONITORINFO.rcMonitor;
			structure.ptMaxPosition.x = Math.Abs(rcWork.left - rcMonitor.left);
			structure.ptMaxPosition.y = Math.Abs(rcWork.top - rcMonitor.top);
			structure.ptMaxSize.x = Math.Abs(rcWork.right - rcWork.left);
			structure.ptMaxSize.y = Math.Abs(rcWork.bottom - rcWork.top);
			structure.ptMinTrackSize.x = minWidth;
			structure.ptMinTrackSize.y = minHeight;
		}
		Marshal.StructureToPtr(structure, lParam, fDeleteOld: true);
	}
}
