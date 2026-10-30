using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Utilities.AttachedProperties;

public class WatermarkAdorner : Adorner
{
	private readonly ContentPresenter contentPresenter;

	protected override int VisualChildrenCount => 1;

	private Control Control => (Control)base.AdornedElement;

	public WatermarkAdorner(UIElement adornedElement, object watermark)
		: base(adornedElement)
	{
		base.IsHitTestVisible = false;
		contentPresenter = new ContentPresenter();
		contentPresenter.Content = watermark;
		contentPresenter.Opacity = 0.5;
		contentPresenter.Margin = new Thickness(Control.Margin.Left + Control.Padding.Left, Control.Margin.Top + Control.Padding.Top, 0.0, 0.0);
		if (Control is ItemsControl && !(Control is ComboBox))
		{
			contentPresenter.VerticalAlignment = VerticalAlignment.Center;
			contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
		}
		Binding binding = new Binding("IsVisible")
		{
			Source = adornedElement,
			Converter = new BooleanToVisibilityConverter()
		};
		SetBinding(UIElement.VisibilityProperty, binding);
	}

	protected override Size ArrangeOverride(Size finalSize)
	{
		contentPresenter.Arrange(new Rect(finalSize));
		return finalSize;
	}

	protected override Visual GetVisualChild(int index)
	{
		return contentPresenter;
	}

	protected override Size MeasureOverride(Size constraint)
	{
		contentPresenter.Measure(Control.RenderSize);
		return Control.RenderSize;
	}
}
