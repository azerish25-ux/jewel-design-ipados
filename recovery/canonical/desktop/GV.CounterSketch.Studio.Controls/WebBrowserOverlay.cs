using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Markup;
using GV.CounterSketch.Shared;

namespace GV.CounterSketch.Studio.Controls;

public class WebBrowserOverlay : Window, IComponentConnector
{
	private readonly FrameworkElement _placementTarget;

	internal WebBrowser _wb;

	private bool _contentLoaded;

	public WebBrowser Browser => _wb;

	public WebBrowserOverlay(FrameworkElement placementTarget)
	{
		InitializeComponent();
		_placementTarget = placementTarget;
		Window owner = Window.GetWindow(placementTarget);
		if (owner != null)
		{
			owner.LocationChanged += delegate
			{
				OnSizeLocationChanged();
			};
		}
		_placementTarget.SizeChanged += delegate
		{
			OnSizeLocationChanged();
		};
		if (owner != null)
		{
			if (owner.IsVisible)
			{
				base.Owner = owner;
				Show();
			}
			else
			{
				owner.IsVisibleChanged += delegate
				{
					if (owner.IsVisible)
					{
						base.Owner = owner;
						Show();
					}
				};
			}
		}
		OnSizeLocationChanged();
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		base.OnClosing(e);
		if (!e.Cancel)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				base.Owner.Close();
			});
		}
	}

	private void OnSizeLocationChanged()
	{
		Point point = _placementTarget.TranslatePoint(default(Point), base.Owner);
		Point point2 = new Point(_placementTarget.ActualWidth, _placementTarget.ActualHeight);
		HwndSource hwndSource = (HwndSource)PresentationSource.FromVisual(base.Owner);
		if (hwndSource != null)
		{
			HwndTarget compositionTarget = hwndSource.CompositionTarget;
			point = compositionTarget.TransformToDevice.Transform(point);
			point2 = compositionTarget.TransformToDevice.Transform(point2);
		}
		WindowExtensions.POINT lpPoint = new WindowExtensions.POINT(point);
		if (hwndSource != null)
		{
			WindowExtensions.ClientToScreen(hwndSource.Handle, ref lpPoint);
		}
		WindowExtensions.POINT pOINT = new WindowExtensions.POINT(point2);
		WindowExtensions.MoveWindow(((HwndSource)PresentationSource.FromVisual(this)).Handle, lpPoint.X, lpPoint.Y, pOINT.X, pOINT.Y, bRepaint: true);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/webbrowseroverlay.xaml", UriKind.Relative);
			System.Windows.Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		if (connectionId == 1)
		{
			_wb = (WebBrowser)target;
		}
		else
		{
			_contentLoaded = true;
		}
	}
}
