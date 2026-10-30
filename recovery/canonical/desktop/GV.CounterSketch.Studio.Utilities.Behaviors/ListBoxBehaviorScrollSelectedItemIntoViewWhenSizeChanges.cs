using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

public class ListBoxBehaviorScrollSelectedItemIntoViewWhenSizeChanges : Behavior<ListBox>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
	}

	private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		base.AssociatedObject.ScrollIntoView(base.AssociatedObject.SelectedItem);
	}
}
