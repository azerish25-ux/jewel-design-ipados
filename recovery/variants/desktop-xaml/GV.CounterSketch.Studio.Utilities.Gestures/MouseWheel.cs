using System;
using System.Windows.Input;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Gestures;

public class MouseWheel : MarkupExtension
{
	public MouseWheelDirection Direction { get; set; }

	public ModifierKeys Keys { get; set; }

	public MouseWheel()
	{
		Keys = ModifierKeys.None;
		Direction = MouseWheelDirection.Down;
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return new MouseWheelGesture(Keys, Direction);
	}
}
