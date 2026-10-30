using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to set item opacity from the displayed position in the listbox.")]
public class ListBoxBehaviorItemOpacityFromPosition : Behavior<ListBox>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGenerator_ItemsChanged;
	}

	private void ApplyTransform(ListBoxItem item, double scaleFactor)
	{
		ScaleTransform layoutTransform = new ScaleTransform(scaleFactor, scaleFactor);
		item.LayoutTransform = layoutTransform;
		item.RenderTransformOrigin = new Point(0.5, 0.5);
		item.UpdateLayout();
	}

	private void ItemContainerGenerator_ItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		foreach (ListBoxItem item in base.AssociatedObject.FindChildren<ListBoxItem>())
		{
			if (base.AssociatedObject.Items.IndexOf(item.Content) == base.AssociatedObject.Items.Count)
			{
				ApplyTransform(item, 1.75);
				item.Opacity = 0.9;
			}
			else if (base.AssociatedObject.Items.IndexOf(item.Content) == base.AssociatedObject.Items.Count - 1)
			{
				ApplyTransform(item, 1.5);
				item.Opacity = 0.7;
			}
			else if (base.AssociatedObject.Items.IndexOf(item.Content) == base.AssociatedObject.Items.Count - 2)
			{
				ApplyTransform(item, 1.25);
				item.Opacity = 0.5;
			}
			else
			{
				ApplyTransform(item, 1.0);
				item.Opacity = 0.0;
			}
		}
	}
}
