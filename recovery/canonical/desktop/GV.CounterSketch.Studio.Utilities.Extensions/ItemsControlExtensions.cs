using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace GV.CounterSketch.Studio.Utilities.Extensions;

public static class ItemsControlExtensions
{
	public static void ScrollToCenterOfView(this ItemsControl itemsControl, object item)
	{
		if (!itemsControl.TryScrollToCenterOfView(item))
		{
			if (itemsControl is ListBox)
			{
				((ListBox)itemsControl).ScrollIntoView(item);
			}
			itemsControl.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Action)delegate
			{
				itemsControl.TryScrollToCenterOfView(item);
			});
		}
	}

	private static double CenteringOffset(double center, double viewport, double extent)
	{
		return Math.Min(extent - viewport, Math.Max(0.0, center - viewport / 2.0));
	}

	private static DependencyObject FirstVisualChild(Visual visual)
	{
		if (visual == null)
		{
			return null;
		}
		if (VisualTreeHelper.GetChildrenCount(visual) == 0)
		{
			return null;
		}
		return VisualTreeHelper.GetChild(visual, 0);
	}

	private static bool TryScrollToCenterOfView(this ItemsControl itemsControl, object item)
	{
		if (!(itemsControl.ItemContainerGenerator.ContainerFromItem(item) is UIElement uIElement))
		{
			return false;
		}
		ScrollContentPresenter scrollContentPresenter = null;
		Visual visual = uIElement;
		while (visual != null && visual != itemsControl && (scrollContentPresenter = visual as ScrollContentPresenter) == null)
		{
			visual = VisualTreeHelper.GetParent(visual) as Visual;
		}
		if (scrollContentPresenter == null)
		{
			return false;
		}
		object obj;
		if (scrollContentPresenter.CanContentScroll)
		{
			obj = (scrollContentPresenter.Content as IScrollInfo) ?? (FirstVisualChild(scrollContentPresenter.Content as ItemsPresenter) as IScrollInfo) ?? scrollContentPresenter;
		}
		else
		{
			IScrollInfo scrollInfo = scrollContentPresenter;
			obj = scrollInfo;
		}
		IScrollInfo scrollInfo2 = (IScrollInfo)obj;
		Size renderSize = uIElement.RenderSize;
		Point point = uIElement.TransformToAncestor((Visual)scrollInfo2).Transform(new Point(renderSize.Width / 2.0, renderSize.Height / 2.0));
		point.Y += scrollInfo2.VerticalOffset;
		point.X += scrollInfo2.HorizontalOffset;
		if (scrollInfo2 is StackPanel || scrollInfo2 is VirtualizingStackPanel)
		{
			double num = (double)itemsControl.ItemContainerGenerator.IndexFromContainer(uIElement) + 0.5;
			if (((scrollInfo2 is StackPanel) ? ((StackPanel)scrollInfo2).Orientation : ((VirtualizingStackPanel)scrollInfo2).Orientation) == Orientation.Horizontal)
			{
				point.X = num;
			}
			else
			{
				point.Y = num;
			}
		}
		if (scrollInfo2.CanVerticallyScroll)
		{
			scrollInfo2.SetVerticalOffset(CenteringOffset(point.Y, scrollInfo2.ViewportHeight, scrollInfo2.ExtentHeight));
		}
		if (scrollInfo2.CanHorizontallyScroll)
		{
			scrollInfo2.SetHorizontalOffset(CenteringOffset(point.X, scrollInfo2.ViewportWidth, scrollInfo2.ExtentWidth));
		}
		return true;
	}
}
