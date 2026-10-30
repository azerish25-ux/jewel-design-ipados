using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to set tag property of each item to it's index in the itemcontrol.")]
public class ItemsControlBehaviorSetItemTagToIndex : Behavior<ItemsControl>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		if (base.AssociatedObject.ItemContainerGenerator != null)
		{
			base.AssociatedObject.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
			base.AssociatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
		}
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		if (base.AssociatedObject.ItemContainerGenerator != null)
		{
			base.AssociatedObject.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
			base.AssociatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGenerator_ItemsChanged;
		}
	}

	private void AffectItem(int i)
	{
		if (base.AssociatedObject.ItemContainerGenerator != null && base.AssociatedObject.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement frameworkElement)
		{
			frameworkElement.Tag = i + 1;
		}
	}

	private void ItemContainerGenerator_ItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		if (e.Action == NotifyCollectionChangedAction.Remove)
		{
			for (int i = 0; i < base.AssociatedObject.Items.Count; i++)
			{
				AffectItem(i);
			}
		}
	}

	private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
	{
		if (base.AssociatedObject.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
		{
			for (int i = 0; i < base.AssociatedObject.Items.Count; i++)
			{
				AffectItem(i);
			}
		}
	}
}
