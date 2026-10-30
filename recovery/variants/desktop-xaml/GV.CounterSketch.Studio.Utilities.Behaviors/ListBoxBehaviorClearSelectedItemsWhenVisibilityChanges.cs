using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to clear selected items from the listbox if visibility changes.")]
public class ListBoxBehaviorClearSelectedItemsWhenVisibilityChanges : Behavior<ListBox>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.IsVisibleChanged -= AssociatedObject_IsVisibleChanged;
	}

	private void AssociatedObject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		base.AssociatedObject.SelectedItems.Clear();
		base.AssociatedObject.SelectedItem = null;
	}
}
