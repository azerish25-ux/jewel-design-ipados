using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to scroll the selected item into view when the listbox selected item changes.")]
public class ListBoxBehaviorScrollSelectedItemIntoView : Behavior<ListBox>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
	}

	private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.AddedItems.Count > 0)
		{
			base.AssociatedObject.ScrollIntoView(e.AddedItems[0]);
		}
	}
}
