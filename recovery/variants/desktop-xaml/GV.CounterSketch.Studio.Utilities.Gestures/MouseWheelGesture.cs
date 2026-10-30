using System.Windows.Input;

namespace GV.CounterSketch.Studio.Utilities.Gestures;

public class MouseWheelGesture : MouseGesture
{
	public MouseWheelDirection Direction { get; set; }

	public MouseWheelGesture(ModifierKeys keys, MouseWheelDirection direction)
		: base(MouseAction.WheelClick, keys)
	{
		Direction = direction;
	}

	public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
	{
		if (!(inputEventArgs is MouseWheelEventArgs e))
		{
			return false;
		}
		if (!base.Matches(targetElement, inputEventArgs))
		{
			return false;
		}
		if ((Direction == MouseWheelDirection.Up && e.Delta > 0) || (Direction == MouseWheelDirection.Down && e.Delta < 0))
		{
			inputEventArgs.Handled = true;
			return true;
		}
		return false;
	}
}
