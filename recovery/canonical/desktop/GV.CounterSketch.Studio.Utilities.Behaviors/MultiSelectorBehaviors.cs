using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

public class MultiSelectorBehaviors : Behavior<ListBox>
{
	private class SynchronizationManager
	{
		private readonly Selector _multiSelector;

		private TwoListSynchronizer _synchronizer;

		internal SynchronizationManager(Selector selector)
		{
			_multiSelector = selector;
		}

		public static IList GetSelectedItemsCollection(Selector selector)
		{
			if (selector is MultiSelector)
			{
				return (selector as MultiSelector).SelectedItems;
			}
			if (selector is ListBox)
			{
				return (selector as ListBox).SelectedItems;
			}
			throw new InvalidOperationException("Target object has no SelectedItems property to bind.");
		}

		public void StartSynchronizingList()
		{
			IList synchronizedSelectedItems = GetSynchronizedSelectedItems(_multiSelector);
			if (synchronizedSelectedItems != null)
			{
				_synchronizer = new TwoListSynchronizer(GetSelectedItemsCollection(_multiSelector), synchronizedSelectedItems);
				_synchronizer.StartSynchronizing();
			}
		}

		public void StopSynchronizing()
		{
			_synchronizer.StopSynchronizing();
		}
	}

	private static readonly DependencyProperty SynchronizationManagerProperty = DependencyProperty.RegisterAttached("SynchronizationManager", typeof(SynchronizationManager), typeof(MultiSelectorBehaviors), new PropertyMetadata(null));

	public static readonly DependencyProperty SynchronizedSelectedItems = DependencyProperty.RegisterAttached("SynchronizedSelectedItems", typeof(IList), typeof(MultiSelectorBehaviors), new PropertyMetadata(null, OnSynchronizedSelectedItemsChanged));

	private static SynchronizationManager GetSynchronizationManager(DependencyObject dependencyObject)
	{
		return (SynchronizationManager)dependencyObject.GetValue(SynchronizationManagerProperty);
	}

	private static void SetSynchronizationManager(DependencyObject dependencyObject, SynchronizationManager value)
	{
		dependencyObject.SetValue(SynchronizationManagerProperty, value);
	}

	private static void OnSynchronizedSelectedItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
	{
		if (e.OldValue != null)
		{
			GetSynchronizationManager(dependencyObject).StopSynchronizing();
			SetSynchronizationManager(dependencyObject, null);
		}
		IList obj = e.NewValue as IList;
		Selector selector = dependencyObject as Selector;
		if (obj != null && selector != null)
		{
			SynchronizationManager synchronizationManager = GetSynchronizationManager(dependencyObject);
			if (synchronizationManager == null)
			{
				synchronizationManager = new SynchronizationManager(selector);
				SetSynchronizationManager(dependencyObject, synchronizationManager);
			}
			synchronizationManager.StartSynchronizingList();
		}
	}

	public static IList GetSynchronizedSelectedItems(DependencyObject dependencyObject)
	{
		return (IList)dependencyObject.GetValue(SynchronizedSelectedItems);
	}

	public static void SetSynchronizedSelectedItems(DependencyObject dependencyObject, IList value)
	{
		dependencyObject.SetValue(SynchronizedSelectedItems, value);
	}
}
