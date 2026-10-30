using System;
using System.Windows;
using System.Windows.Interactivity;
using CefSharp;
using CefSharp.Wpf;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

public class HoverLinkBehaviour : Behavior<ChromiumWebBrowser>
{
	public static readonly DependencyProperty HoverLinkProperty = DependencyProperty.Register("HoverLink", typeof(string), typeof(HoverLinkBehaviour), new PropertyMetadata(string.Empty));

	public string HoverLink
	{
		get
		{
			return (string)GetValue(HoverLinkProperty);
		}
		set
		{
			SetValue(HoverLinkProperty, value);
		}
	}

	protected override void OnAttached()
	{
		base.AssociatedObject.StatusMessage += OnStatusMessageChanged;
	}

	protected override void OnDetaching()
	{
		base.AssociatedObject.StatusMessage -= OnStatusMessageChanged;
	}

	private void OnStatusMessageChanged(object sender, StatusMessageEventArgs e)
	{
		(sender as ChromiumWebBrowser).Dispatcher.BeginInvoke((Action)delegate
		{
			HoverLink = e.Value;
		});
	}
}
