using System;
using System.Windows;
using System.Windows.Input;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

internal class FocusOnMouseEnterProperty : DependencyObject
{
	public static readonly DependencyProperty UseFocusOnMouseEnterProperty = DependencyProperty.RegisterAttached("UseFocusOnMouseEnter", typeof(bool), typeof(UIElement), new PropertyMetadata(false, UseFocuseOnMouseEnterChangedCallback));

	private static void UseFocuseOnMouseEnterChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		FrameworkElement uiElement = d as FrameworkElement;
		if (uiElement == null)
		{
			throw new ArgumentException("Element is not a UIControl");
		}
		uiElement.MouseEnter += delegate
		{
			FocusManager.SetFocusedElement(uiElement, uiElement);
		};
	}

	public static void SetFocusOnMouseEnter(UIElement element, bool value)
	{
		element.SetValue(UseFocusOnMouseEnterProperty, value);
	}

	public static bool GetFocusOnMouseEnter(UIElement element)
	{
		return (bool)element.GetValue(UseFocusOnMouseEnterProperty);
	}
}
