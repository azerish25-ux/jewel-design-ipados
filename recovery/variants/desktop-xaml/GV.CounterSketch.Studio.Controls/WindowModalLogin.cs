using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Controls;

public partial class WindowModalLogin : Window, INotifyPropertyChanged, IComponentConnector
{
	private SecurityHelper.LoginMode _loginControlMode;

	private string _reasonForPrompt;

	public string ReasonForPrompt
	{
		get
		{
			return _reasonForPrompt;
		}
		set
		{
			_reasonForPrompt = value;
			RaisePropertyChanged("ReasonForPrompt");
		}
	}

	public SecurityHelper.LoginMode LoginControlMode
	{
		get
		{
			return _loginControlMode;
		}
		set
		{
			_loginControlMode = value;
			csLoginControl.LoginControlMode = value;
			RaisePropertyChanged("LoginControlMode");
		}
	}

	public int TopPosition { get; set; }

	public int LeftPosition { get; set; }

	public IntPtr ParentWindowHandle { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public WindowModalLogin()
	{
		InitializeComponent();
		base.Loaded += Window_Loaded;
		base.SourceInitialized += Window_SourceInitialized;
	}

	private void RaisePropertyChanged(string info)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(info));
		}
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		Win32Utility.PositionWindow(new WindowInteropHelper(this).Handle, LeftPosition, TopPosition, (int)base.MaxWidth, (int)base.MaxHeight);
	}

	private void Window_SourceInitialized(object sender, EventArgs e)
	{
		HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(WindowProc);
	}

	private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{
		if (msg == 36 && ((Window)HwndSource.FromHwnd(hwnd).RootVisual).WindowState == WindowState.Maximized)
		{
			MonitorHelper.WmGetMinMaxInfo(hwnd, lParam);
			handled = true;
		}
		return (IntPtr)0;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
