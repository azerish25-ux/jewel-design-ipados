using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

public class HorizontalScrollingProperty : DependencyObject
{
	public static readonly DependencyProperty UseHorizontalScrollingProperty = DependencyProperty.RegisterAttached("UseHorizontalScrolling", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(false, UseHorizontalScrollingChangedCallback));

	private static void UseHorizontalScrollingChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
	{
		ItemsControl itemsControl = dependencyObject as ItemsControl;
		if (itemsControl == null)
		{
			throw new ArgumentException("Element is not an ItemsControl");
		}
		itemsControl.PreviewMouseWheel += delegate(object sender, MouseWheelEventArgs args)
		{
			ScrollViewer scrollViewer = ChildrenOfTypeExtensions.FindChildByType<ScrollViewer>((DependencyObject)itemsControl);
			if (scrollViewer != null)
			{
				if (args.Delta < 0)
				{
					scrollViewer.LineRight();
				}
				else
				{
					scrollViewer.LineLeft();
				}
			}
		};
	}

	public static bool GetUseHorizontalScrolling(ItemsControl element)
	{
		return (bool)element.GetValue(UseHorizontalScrollingProperty);
	}

	public static void SetUseHorizontalScrolling(ItemsControl element, bool value)
	{
		element.SetValue(UseHorizontalScrollingProperty, value);
	}
}
