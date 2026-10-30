using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to automatically select the first item when the listbox is enabled.")]
public class ListBoxBehaviorSelectHitTestItemWhenEnabled : Behavior<ListBox>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.IsEnabledChanged += AssociatedObject_IsEnabledChanged;
	}

	protected override void OnDetaching()
	{
		base.AssociatedObject.IsEnabledChanged += AssociatedObject_IsEnabledChanged;
	}

	private void AssociatedObject_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (!base.AssociatedObject.IsEnabled)
		{
			return;
		}
		HitTestResult hitTestResult = VisualTreeHelper.HitTest(base.AssociatedObject, Mouse.GetPosition(base.AssociatedObject));
		if (hitTestResult != null)
		{
			ListBoxItem listBoxItem = hitTestResult.VisualHit.TryFindParent<ListBoxItem>();
			if (listBoxItem != null)
			{
				base.AssociatedObject.SelectedIndex = base.AssociatedObject.ItemContainerGenerator.IndexFromContainer(listBoxItem);
				return;
			}
		}
		if (base.AssociatedObject.Items.Count > 0)
		{
			base.AssociatedObject.SelectedIndex = 0;
		}
	}
}
