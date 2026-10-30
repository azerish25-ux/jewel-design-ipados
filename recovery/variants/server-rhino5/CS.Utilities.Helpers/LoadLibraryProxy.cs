using System;
using System.Runtime.InteropServices;

namespace CS.Utilities.Helpers;

public sealed class LoadLibraryProxy : IDisposable
{
	private IntPtr _sharedObjectIntPtr = IntPtr.Zero;

	private bool _disposed;

	public LoadLibraryProxy(string libraryPath)
	{
		CSUtil.PrintLine($"Loading library from - {libraryPath}");
		LoadSharedObject(libraryPath);
	}

	public void Dispose()
	{
		UnloadSharedObject();
	}

	public Delegate GetMethodDelegate<T>(string methodName)
	{
		if (_sharedObjectIntPtr != IntPtr.Zero && string.IsNullOrWhiteSpace(methodName))
		{
			return Marshal.GetDelegateForFunctionPointer(GetProcAddress(_sharedObjectIntPtr, methodName), typeof(T));
		}
		return null;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			Dispose();
			_disposed = true;
		}
	}

	[DllImport("kernel32", SetLastError = true)]
	internal static extern bool FreeLibrary(IntPtr hModule);

	[DllImport("kernel32.dll")]
	internal static extern uint GetLastError();

	[DllImport("kernel32.dll")]
	internal static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

	[DllImport("kernel32", SetLastError = true)]
	internal static extern IntPtr LoadLibrary(string lpFileName);

	private void LoadSharedObject(string strFileName)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("LoadLibraryProxt has been disposed.  Create a new one.");
		}
		try
		{
			_sharedObjectIntPtr = LoadLibrary(strFileName);
		}
		catch (Exception arg)
		{
			throw new ApplicationException($"Cannot load library {strFileName} - Exception: {arg}");
		}
		if (_sharedObjectIntPtr == IntPtr.Zero)
		{
			throw new ApplicationException($"Cannot open {strFileName} - ErrorCode : {GetLastError()}");
		}
	}

	private void UnloadSharedObject()
	{
		if (_sharedObjectIntPtr != IntPtr.Zero)
		{
			FreeLibrary(_sharedObjectIntPtr);
			FreeLibrary(_sharedObjectIntPtr);
		}
	}
}
