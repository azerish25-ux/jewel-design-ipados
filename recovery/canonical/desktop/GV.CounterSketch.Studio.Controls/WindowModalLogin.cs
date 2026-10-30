using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Controls;

public class WindowModalLogin : Window, INotifyPropertyChanged, IComponentConnector
{
	private SecurityHelper.LoginMode _loginControlMode;

	private string _reasonForPrompt;

	internal WindowModalLogin Window;

	internal Grid LoginPrompt;

	internal Button button;

	internal CSLoginControl csLoginControl;

	private bool _contentLoaded;

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
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/windowmodallogin.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			Window = (WindowModalLogin)target;
			break;
		case 2:
			LoginPrompt = (Grid)target;
			break;
		case 3:
			button = (Button)target;
			break;
		case 4:
			csLoginControl = (CSLoginControl)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
