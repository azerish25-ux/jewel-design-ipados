using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Controls;

public class ZoomBorder : Border
{
	private UIElement _child;

	public override UIElement Child
	{
		get
		{
			return base.Child;
		}
		set
		{
			if (value != null && value != Child)
			{
				Initialize(value);
			}
			base.Child = value;
		}
	}

	public void Initialize(UIElement element)
	{
		_child = element;
		if (_child != null)
		{
			TransformGroup transformGroup = new TransformGroup();
			ScaleTransform value = new ScaleTransform
			{
				ScaleX = 1.0,
				ScaleY = 1.0
			};
			transformGroup.Children.Add(value);
			TranslateTransform value2 = new TranslateTransform();
			transformGroup.Children.Add(value2);
			_child.RenderTransform = transformGroup;
			_child.RenderTransformOrigin = new Point(0.0, 0.0);
			base.PreviewMouseWheel += Child_MouseWheel;
			base.MouseEnter += Child_MouseEnter;
			base.MouseLeave += Child_MouseLeave;
			base.MouseMove += Child_MouseMove;
			base.PreviewMouseRightButtonDown += Child_PreviewMouseRightButtonDown;
		}
	}

	public void Reset()
	{
		if (_child != null)
		{
			ScaleTransform scaleTransform = GetScaleTransform(_child);
			scaleTransform.ScaleX = 1.0;
			scaleTransform.ScaleY = 1.0;
			TranslateTransform translateTransform = GetTranslateTransform(_child);
			translateTransform.X = 0.0;
			translateTransform.Y = 0.0;
		}
	}

	private void Child_MouseEnter(object sender, MouseEventArgs e)
	{
		if (_child != null)
		{
			ScaleTransform scaleTransform = GetScaleTransform(_child);
			if (scaleTransform.ScaleX == 1.0)
			{
				scaleTransform.ScaleX = 2.0;
				scaleTransform.ScaleY = 2.0;
			}
			base.Cursor = Cursors.Cross;
		}
	}

	private void Child_MouseLeave(object sender, MouseEventArgs e)
	{
		if (_child != null)
		{
			base.Cursor = Cursors.Arrow;
			Reset();
		}
	}

	private void Child_MouseMove(object sender, MouseEventArgs e)
	{
		if (_child != null)
		{
			ScaleTransform scaleTransform = GetScaleTransform(_child);
			TranslateTransform translateTransform = GetTranslateTransform(_child);
			Point position = e.GetPosition(_child);
			translateTransform.X = 0.0 - (position.X - base.Width / 2.0 + scaleTransform.ScaleX);
			translateTransform.Y = 0.0 - (position.Y - base.Height / 2.0 + scaleTransform.ScaleY);
		}
	}

	private void Child_MouseWheel(object sender, MouseWheelEventArgs e)
	{
		if (_child != null)
		{
			ScaleTransform scaleTransform = GetScaleTransform(_child);
			double scaleX = scaleTransform.ScaleX;
			scaleX = ((e.Delta <= 0) ? (scaleX - 0.5) : (scaleX + 0.5));
			if (scaleX > 10.0)
			{
				scaleX = 10.0;
			}
			if (scaleX < 1.0)
			{
				scaleX = 1.0;
			}
			scaleTransform.ScaleX = scaleX;
			scaleTransform.ScaleY = scaleX;
		}
	}

	private void Child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		Reset();
	}

	private static ScaleTransform GetScaleTransform(UIElement element)
	{
		return (ScaleTransform)((TransformGroup)element.RenderTransform).Children.First((Transform tr) => tr is ScaleTransform);
	}

	private static TranslateTransform GetTranslateTransform(UIElement element)
	{
		return (TranslateTransform)((TransformGroup)element.RenderTransform).Children.First((Transform tr) => tr is TranslateTransform);
	}
}
