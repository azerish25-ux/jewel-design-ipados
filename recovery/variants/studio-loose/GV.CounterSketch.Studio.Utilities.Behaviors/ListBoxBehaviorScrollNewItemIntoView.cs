using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to scroll a new item into view when the listbox item collection changes.")]
public class ListBoxBehaviorScrollNewItemIntoView : Behavior<ListBox>
{
	private int _firstNewItemIndex;

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		_firstNewItemIndex = -1;
		base.AssociatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
		base.AssociatedObject.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
		base.AssociatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGenerator_ItemsChanged;
	}

	private void ItemContainerGenerator_ItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		if (e.Action == NotifyCollectionChangedAction.Add)
		{
			if (_firstNewItemIndex == -1)
			{
				_firstNewItemIndex = e.Position.Index + 1;
			}
		}
		else if (e.Action == NotifyCollectionChangedAction.Remove)
		{
			_firstNewItemIndex = -1;
		}
	}

	private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
	{
		if (base.AssociatedObject.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated && _firstNewItemIndex > -1)
		{
			object obj = base.AssociatedObject.Items[_firstNewItemIndex];
			base.AssociatedObject.ScrollIntoView(obj);
			base.AssociatedObject.SelectedItem = obj;
			_firstNewItemIndex = -1;
		}
	}
}
