using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.Utilities.Extensions;

public static class HyperlinkExtensions
{
	public static readonly DependencyProperty IsExternalProperty = DependencyProperty.RegisterAttached("IsExternal", typeof(bool), typeof(HyperlinkExtensions), new UIPropertyMetadata(false, OnIsExternalChanged));

	public static bool GetIsExternal(DependencyObject obj)
	{
		return (bool)obj.GetValue(IsExternalProperty);
	}

	public static void SetIsExternal(DependencyObject obj, bool value)
	{
		obj.SetValue(IsExternalProperty, value);
	}

	private static void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		ViewModelLocator.Current.MainVM.OpenURLInOurBrowserWindow(e.Uri.AbsoluteUri);
		e.Handled = true;
	}

	private static void OnIsExternalChanged(object sender, DependencyPropertyChangedEventArgs args)
	{
		Hyperlink hyperlink = sender as Hyperlink;
		if ((bool)args.NewValue)
		{
			hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
		}
		else
		{
			hyperlink.RequestNavigate -= Hyperlink_RequestNavigate;
		}
	}
}
