using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Threading;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using Localization;

namespace GV.CounterSketch.Studio.View;

public class WindowModalWait : Window, IComponentConnector
{
	private readonly DispatcherTimer _timer;

	internal WindowModalWait Window;

	internal Grid grid1;

	internal Border borderMasked;

	internal TextBlock textMessage;

	private bool _contentLoaded;

	public int LeftPosition { get; set; }

	public IntPtr ParentWindowHandle { get; set; }

	public int TopPosition { get; set; }

	public WindowModalWait(string message = "Please Wait")
	{
		if (message == "Please Wait")
		{
			message = TranslationHelper.GetTranslatedString("Please Wait");
		}
		InitializeComponent();
		base.Loaded += WindowModalWaitLoaded;
		base.SourceInitialized += WindowSourceInitialized;
		_timer = new DispatcherTimer
		{
			Interval = new TimeSpan(0, 0, 0, 0, 10)
		};
		_timer.Tick += TimerTick;
		_timer.Start();
		textMessage.Text = message;
	}

	public void SetMessage(string message)
	{
		textMessage.Text = message;
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		base.OnClosing(e);
		if (!e.Cancel && base.Owner != null)
		{
			base.Owner.Focus();
		}
	}

	private void TimerTick(object sender, EventArgs e)
	{
		if (!UIHelper.IsWaitVisible)
		{
			_timer.Stop();
			_timer.Tick -= TimerTick;
			Close();
		}
	}

	private void WindowModalWaitLoaded(object sender, RoutedEventArgs e)
	{
		Size size = new Size(base.Width, base.Height);
		size = this.TransformFromDeviceDPI(size);
		Point pt = new Point(LeftPosition, TopPosition);
		pt = this.TransformFromDeviceDPI(pt);
		Win32Utility.PositionWindow(new WindowInteropHelper(this).Handle, (int)pt.X, (int)pt.Y, (int)size.Width, (int)size.Height);
	}

	~WindowModalWait()
	{
		_timer.Tick -= TimerTick;
	}

	private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{
		if (msg == 36)
		{
			HwndSource hwndSource = HwndSource.FromHwnd(hwnd);
			if (hwndSource == null || ((Window)hwndSource.RootVisual).WindowState == WindowState.Maximized)
			{
				MonitorHelper.WmGetMinMaxInfo(hwnd, lParam);
				handled = true;
			}
		}
		return (IntPtr)0;
	}

	private void WindowSourceInitialized(object sender, EventArgs e)
	{
		HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(WindowProc);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windowmodalwait.xaml", UriKind.Relative);
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
			Window = (WindowModalWait)target;
			break;
		case 2:
			grid1 = (Grid)target;
			break;
		case 3:
			borderMasked = (Border)target;
			break;
		case 4:
			textMessage = (TextBlock)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
