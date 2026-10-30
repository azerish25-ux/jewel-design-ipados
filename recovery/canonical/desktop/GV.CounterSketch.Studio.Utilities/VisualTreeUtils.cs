using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GV.CounterSketch.Studio.Utilities;

internal static class VisualTreeUtils
{
	private class VisualState
	{
		internal Visual Visual { get; }

		internal uint Depth { get; }

		internal VisualState(Visual visual, uint depth)
		{
			Visual = visual;
			Depth = depth;
		}
	}

	private const int MaxSearchDepth = 5;

	private const int MaxAllowedTreeDepth = 250;

	public static T GetNamedChild<T>(DependencyObject container, string name) where T : FrameworkElement
	{
		return GetNamedChild<T>(container, name, 0);
	}

	public static T GetNamedChild<T>(DependencyObject container, string name, int searchDepth) where T : FrameworkElement
	{
		if (container == null || string.IsNullOrEmpty(name) || searchDepth < 0)
		{
			return null;
		}
		if (container is T && string.Equals(name, ((T)container).Name))
		{
			return (T)container;
		}
		int childrenCount = VisualTreeHelper.GetChildrenCount(container);
		if (childrenCount == 0)
		{
			return null;
		}
		for (int i = 0; i < childrenCount; i++)
		{
			if (VisualTreeHelper.GetChild(container, i) is FrameworkElement frameworkElement)
			{
				T val = (frameworkElement.FindName(name) as T) ?? ((searchDepth > 0) ? GetNamedChild<T>(frameworkElement, name, searchDepth - 1) : null);
				if (val != null)
				{
					return val;
				}
			}
		}
		return null;
	}

	public static T GetTemplateChild<T>(DependencyObject element) where T : DependencyObject
	{
		int num = 5;
		while (num > 0 && element != null)
		{
			if (VisualTreeHelper.GetChildrenCount(element) < 1)
			{
				return null;
			}
			element = VisualTreeHelper.GetChild(element, 0);
			if (element is T result)
			{
				return result;
			}
			num--;
		}
		return null;
	}

	public static T FindVisualAncestor<T>(DependencyObject child) where T : DependencyObject
	{
		if (child == null)
		{
			return null;
		}
		do
		{
			child = VisualTreeHelper.GetParent(child);
		}
		while (child != null && !typeof(T).IsAssignableFrom(child.GetType()));
		return child as T;
	}

	public static T FindFocusableElement<T>(T reference) where T : UIElement
	{
		if (reference == null || (reference.Focusable && reference.Visibility == Visibility.Visible))
		{
			return reference;
		}
		int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
		for (int i = 0; i < childrenCount; i++)
		{
			if (VisualTreeHelper.GetChild(reference, i) is T val && val.Visibility == Visibility.Visible)
			{
				if (val.Focusable)
				{
					return val;
				}
				T val2 = FindFocusableElement(val);
				if (val2 != null)
				{
					return val2;
				}
			}
		}
		return null;
	}

	public static T FindFocusableParent<T>(UIElement reference) where T : UIElement
	{
		if (reference != null)
		{
			for (UIElement uIElement = VisualTreeHelper.GetParent(reference) as UIElement; uIElement != null; uIElement = VisualTreeHelper.GetParent(uIElement) as UIElement)
			{
				if (uIElement.Visibility == Visibility.Visible && uIElement is T && uIElement.Focusable)
				{
					return uIElement as T;
				}
			}
		}
		return null;
	}

	public static DependencyObject GetIndexedVisualParent(DependencyObject child, out int childrenCount, out int childIndex)
	{
		childrenCount = 0;
		DependencyObject parent = VisualTreeHelper.GetParent(child);
		if (parent != null)
		{
			childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (childIndex = 0; childIndex < childrenCount; childIndex++)
			{
				if (child.Equals(VisualTreeHelper.GetChild(parent, childIndex)))
				{
					return parent;
				}
			}
		}
		childIndex = -1;
		return null;
	}

	public static bool IsVisible(UIElement element)
	{
		for (UIElement uIElement = element; uIElement != null; uIElement = VisualTreeHelper.GetParent(uIElement) as UIElement)
		{
			if (uIElement.Visibility != Visibility.Visible)
			{
				return false;
			}
		}
		return true;
	}

	public static ICollection<T> PrunVisualTree<T>(Visual root) where T : DependencyObject
	{
		HashSet<T> hashSet = new HashSet<T>();
		Stack<VisualState> stack = new Stack<VisualState>();
		VisualState item = new VisualState(root, GetTreeDepth(root));
		stack.Push(item);
		int num = 0;
		while (stack.Count > 0)
		{
			item = stack.Pop();
			if (item.Depth + 1 >= 250)
			{
				if (item.Visual is T item2)
				{
					hashSet.Add(item2);
					continue;
				}
				T val = FindVisualAncestor<T>(item.Visual);
				if (val != null)
				{
					hashSet.Add(val);
				}
				continue;
			}
			num = VisualTreeHelper.GetChildrenCount(item.Visual);
			for (int i = 0; i < num; i++)
			{
				if (VisualTreeHelper.GetChild(item.Visual, i) is Visual visual)
				{
					stack.Push(new VisualState(visual, item.Depth + 1));
				}
			}
		}
		return hashSet;
	}

	public static uint GetTreeDepth(DependencyObject element)
	{
		uint num = 0u;
		while (element != null)
		{
			num++;
			element = ((!(element is Visual) && !(element is Visual3D)) ? LogicalTreeHelper.GetParent(element) : VisualTreeHelper.GetParent(element));
		}
		return num;
	}
}
