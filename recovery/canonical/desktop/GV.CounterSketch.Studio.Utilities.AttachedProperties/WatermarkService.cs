using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace GV.CounterSketch.Studio.Utilities.AttachedProperties;

public static class WatermarkService
{
	public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", typeof(object), typeof(WatermarkService), new FrameworkPropertyMetadata(null, OnWatermarkChanged));

	private static readonly Dictionary<object, ItemsControl> itemsControls = new Dictionary<object, ItemsControl>();

	private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		Control control = (Control)d;
		control.Loaded += Control_Loaded;
		if (d is ComboBox || d is TextBox)
		{
			control.GotKeyboardFocus += Control_GotKeyboardFocus;
			control.LostKeyboardFocus += Control_Loaded;
		}
		if (d is ItemsControl && !(d is ComboBox))
		{
			ItemsControl itemsControl = (ItemsControl)d;
			itemsControl.ItemContainerGenerator.ItemsChanged += ItemsChanged;
			itemsControls.Add(itemsControl.ItemContainerGenerator, itemsControl);
			DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, itemsControl.GetType()).AddValueChanged(itemsControl, ItemsSourceChanged);
		}
	}

	public static object GetWatermark(DependencyObject d)
	{
		return d.GetValue(WatermarkProperty);
	}

	public static void SetWatermark(DependencyObject d, object value)
	{
		d.SetValue(WatermarkProperty, value);
	}

	private static void Control_GotKeyboardFocus(object sender, RoutedEventArgs e)
	{
		Control control = (Control)sender;
		if (ShouldShowWatermark(control))
		{
			RemoveWatermark(control);
		}
	}

	private static void Control_Loaded(object sender, RoutedEventArgs e)
	{
		Control control = (Control)sender;
		if (ShouldShowWatermark(control))
		{
			ShowWatermark(control);
		}
	}

	private static void ItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		if (itemsControls.TryGetValue(sender, out var value))
		{
			if (ShouldShowWatermark(value))
			{
				ShowWatermark(value);
			}
			else
			{
				RemoveWatermark(value);
			}
		}
	}

	private static void ItemsSourceChanged(object sender, EventArgs e)
	{
		ItemsControl itemsControl = (ItemsControl)sender;
		if (itemsControl.ItemsSource != null)
		{
			if (ShouldShowWatermark(itemsControl))
			{
				ShowWatermark(itemsControl);
			}
			else
			{
				RemoveWatermark(itemsControl);
			}
		}
		else
		{
			ShowWatermark(itemsControl);
		}
	}

	private static void RemoveWatermark(UIElement control)
	{
		AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(control);
		if (adornerLayer == null)
		{
			return;
		}
		Adorner[] adorners = adornerLayer.GetAdorners(control);
		if (adorners == null)
		{
			return;
		}
		Adorner[] array = adorners;
		foreach (Adorner adorner in array)
		{
			if (adorner is WatermarkAdorner)
			{
				adorner.Visibility = Visibility.Hidden;
				adornerLayer.Remove(adorner);
			}
		}
	}

	private static bool ShouldShowWatermark(Control c)
	{
		if (c is ComboBox)
		{
			return (c as ComboBox).Text == string.Empty;
		}
		if (c is TextBoxBase)
		{
			return (c as TextBox).Text == string.Empty;
		}
		if (c is ItemsControl)
		{
			return (c as ItemsControl).Items.Count == 0;
		}
		return false;
	}

	private static void ShowWatermark(Control control)
	{
		AdornerLayer.GetAdornerLayer(control)?.Add(new WatermarkAdorner(control, GetWatermark(control)));
	}
}
