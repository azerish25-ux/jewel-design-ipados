using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to clear selected items of the listbox when it becomes disabled.")]
public class ListBoxBehaviorClearSelectedItemsWhenDisabled : Behavior<ListBox>
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
			base.AssociatedObject.SelectedItems.Clear();
		}
	}
}
