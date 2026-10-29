using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GV.CounterSketch.Shared;

public static class WindowSearcher
{
	private delegate bool EnumWindowsProc(IntPtr hWnd, ref SearchData data);

	public sealed class SearchData
	{
		public string Title;

		public string Wndclass;

		public IntPtr hWnd;
	}

	public static bool EnumProc(IntPtr hWnd, ref SearchData data)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		GetClassName(hWnd, stringBuilder, stringBuilder.Capacity);
		if (stringBuilder.ToString().StartsWith(data.Wndclass))
		{
			stringBuilder = new StringBuilder(1024);
			GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);
			if (stringBuilder.ToString().StartsWith(data.Title))
			{
				data.hWnd = hWnd;
				return false;
			}
		}
		return true;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

	public static IntPtr SearchForWindow(string wndclass, string title)
	{
		SearchData data = new SearchData
		{
			Wndclass = wndclass,
			Title = title
		};
		EnumWindows(EnumProc, ref data);
		return data.hWnd;
	}

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref SearchData data);
}
