using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

public class TextBoxWatermarkBehavior : Behavior<TextBox>
{
	private static readonly DependencyPropertyKey IsWatermarkedPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsWatermarked", typeof(bool), typeof(TextBoxWatermarkBehavior), new FrameworkPropertyMetadata(false));

	public static readonly DependencyProperty IsWatermarkedProperty = IsWatermarkedPropertyKey.DependencyProperty;

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBoxWatermarkBehavior), new FrameworkPropertyMetadata(string.Empty));

	public bool IsWatermarked
	{
		get
		{
			return GetIsWatermarked(base.AssociatedObject);
		}
		private set
		{
			base.AssociatedObject.SetValue(IsWatermarkedPropertyKey, value);
		}
	}

	public string Text
	{
		get
		{
			return (string)((DependencyObject)(object)this).GetValue(TextProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(TextProperty, (object)value);
		}
	}

	public static bool GetIsWatermarked(TextBox tb)
	{
		return (bool)tb.GetValue(IsWatermarkedProperty);
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.GotFocus += OnGotFocus;
		base.AssociatedObject.LostFocus += OnLostFocus;
		OnLostFocus(null, null);
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.GotFocus -= OnGotFocus;
		base.AssociatedObject.LostFocus -= OnLostFocus;
	}

	private void OnGotFocus(object sender, RoutedEventArgs e)
	{
		if (string.Compare(base.AssociatedObject.Text, Text, StringComparison.OrdinalIgnoreCase) == 0)
		{
			base.AssociatedObject.Text = string.Empty;
			IsWatermarked = false;
		}
	}

	private void OnLostFocus(object sender, RoutedEventArgs e)
	{
		if (string.IsNullOrEmpty(base.AssociatedObject.Text))
		{
			base.AssociatedObject.Text = Text;
			IsWatermarked = true;
		}
	}
}
