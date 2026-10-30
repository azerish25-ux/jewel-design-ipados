using System.Windows;
using System.Windows.Controls;

namespace GV.CounterSketch.Studio.Utilities;

public static class ResourceKeyBindings
{
	public static DependencyProperty BackgroundResourceKeyBindingProperty = ResourceKeyBindingPropertyFactory.CreateResourceKeyBindingProperty(Border.BackgroundProperty, typeof(ResourceKeyBindings));

	public static object GetBackgroundResourceKeyBinding(DependencyObject dp)
	{
		return dp.GetValue(BackgroundResourceKeyBindingProperty);
	}

	public static void SetBackgroundResourceKeyBinding(DependencyObject dp, object resourceKey)
	{
		if (resourceKey != null)
		{
			dp.SetValue(BackgroundResourceKeyBindingProperty, resourceKey);
		}
	}
}
