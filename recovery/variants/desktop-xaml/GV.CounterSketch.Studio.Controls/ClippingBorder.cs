using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Controls;

public class ClippingBorder : Border
{
	private readonly RectangleGeometry _clipRect = new RectangleGeometry();

	private object _oldClip;

	public override UIElement Child
	{
		get
		{
			return base.Child;
		}
		set
		{
			if (Child != value)
			{
				if (Child != null)
				{
					Child.SetValue(UIElement.ClipProperty, _oldClip);
				}
				if (value != null)
				{
					_oldClip = value.ReadLocalValue(UIElement.ClipProperty);
				}
				else
				{
					_oldClip = null;
				}
				base.Child = value;
			}
		}
	}

	protected virtual void OnApplyChildClip()
	{
		UIElement child = Child;
		if (child != null)
		{
			RectangleGeometry clipRect = _clipRect;
			double radiusX = (_clipRect.RadiusY = Math.Max(0.0, base.CornerRadius.TopLeft - base.BorderThickness.Left * 0.5));
			clipRect.RadiusX = radiusX;
			_clipRect.Rect = new Rect(Child.RenderSize);
			child.Clip = _clipRect;
		}
	}

	protected override void OnRender(DrawingContext dc)
	{
		OnApplyChildClip();
		base.OnRender(dc);
	}
}
