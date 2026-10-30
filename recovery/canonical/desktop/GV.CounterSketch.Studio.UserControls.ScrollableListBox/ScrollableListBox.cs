using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.UserControls.ScrollableListBox;

public class ScrollableListBox : ListBox
{
	public ScrollableListBox()
	{
		base.Loaded += ScrollableListBox_Loaded;
	}

	private static Visual GetDescendantByType(Visual element, Type type)
	{
		if (element == null)
		{
			return null;
		}
		if (element.GetType() == type)
		{
			return element;
		}
		Visual visual = null;
		if (element is FrameworkElement)
		{
			(element as FrameworkElement).ApplyTemplate();
		}
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
		{
			visual = GetDescendantByType(VisualTreeHelper.GetChild(element, i) as Visual, type);
			if (visual != null)
			{
				break;
			}
		}
		return visual;
	}

	private void ScrollableListBox_Loaded(object sender, RoutedEventArgs e)
	{
		if (GetDescendantByType(this, typeof(ScrollViewer)) is ScrollViewer scrollViewer)
		{
			scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
		}
	}

	private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
	{
		if (e.ExtentHeightChange != 0.0 || e.ExtentWidthChange != 0.0)
		{
			ScrollIntoView(base.SelectedItem);
		}
	}
}
