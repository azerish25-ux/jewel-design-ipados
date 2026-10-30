using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Utilities.AttachedProperties;

public class TextBlockService
{
	public static readonly DependencyPropertyKey IsTextTrimmedKey;

	public static readonly DependencyProperty IsTextTrimmedProperty;

	public static readonly DependencyProperty AutomaticToolTipEnabledProperty;

	static TextBlockService()
	{
		IsTextTrimmedKey = DependencyProperty.RegisterAttachedReadOnly("IsTextTrimmed", typeof(bool), typeof(TextBlockService), new PropertyMetadata(false));
		IsTextTrimmedProperty = IsTextTrimmedKey.DependencyProperty;
		AutomaticToolTipEnabledProperty = DependencyProperty.RegisterAttached("AutomaticToolTipEnabled", typeof(bool), typeof(TextBlockService), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));
		EventManager.RegisterClassHandler(typeof(TextBlock), FrameworkElement.SizeChangedEvent, new SizeChangedEventHandler(OnTextBlockSizeChanged), handledEventsToo: true);
	}

	public static void OnTextBlockSizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (sender is TextBlock textBlock)
		{
			if (textBlock.TextTrimming == TextTrimming.None)
			{
				SetIsTextTrimmed(textBlock, value: false);
			}
			else
			{
				SetIsTextTrimmed(textBlock, CalculateIsTextTrimmed(textBlock));
			}
		}
	}

	private static bool CalculateIsTextTrimmed(TextBlock textBlock)
	{
		if (!textBlock.IsArrangeValid)
		{
			return GetIsTextTrimmed(textBlock);
		}
		Typeface typeface = new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch);
		return new FormattedText(textBlock.Text, Thread.CurrentThread.CurrentCulture, textBlock.FlowDirection, typeface, textBlock.FontSize, textBlock.Foreground)
		{
			MaxTextWidth = textBlock.ActualWidth
		}.Height > textBlock.ActualHeight;
	}

	private static void SetIsTextTrimmed(TextBlock target, bool value)
	{
		target.SetValue(IsTextTrimmedKey, value);
	}

	[AttachedPropertyBrowsableForType(typeof(TextBlock))]
	public static bool GetIsTextTrimmed(TextBlock target)
	{
		return (bool)target.GetValue(IsTextTrimmedProperty);
	}

	[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
	public static bool GetAutomaticToolTipEnabled(DependencyObject element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		return (bool)element.GetValue(AutomaticToolTipEnabledProperty);
	}

	public static void SetAutomaticToolTipEnabled(DependencyObject element, bool value)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		element.SetValue(AutomaticToolTipEnabledProperty, value);
	}
}
