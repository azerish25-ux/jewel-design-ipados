using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace GV.CounterSketch.Studio.Utilities.Animations;

internal class GridLengthAnimation : AnimationTimeline
{
	public static readonly DependencyProperty FromProperty;

	public static readonly DependencyProperty ToProperty;

	public GridLength From
	{
		get
		{
			return (GridLength)GetValue(FromProperty);
		}
		set
		{
			SetValue(FromProperty, value);
		}
	}

	public GridLength To
	{
		get
		{
			return (GridLength)GetValue(ToProperty);
		}
		set
		{
			SetValue(ToProperty, value);
		}
	}

	public override Type TargetPropertyType => typeof(GridLength);

	public IEasingFunction EasingFunction { get; set; }

	static GridLengthAnimation()
	{
		FromProperty = DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));
		ToProperty = DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));
	}

	public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
	{
		double value = ((GridLength)GetValue(FromProperty)).Value;
		double value2 = ((GridLength)GetValue(ToProperty)).Value;
		if (value > value2)
		{
			return new GridLength((1.0 - animationClock.CurrentProgress.Value) * (value - value2) + value2, (!To.IsStar) ? GridUnitType.Pixel : GridUnitType.Star);
		}
		return new GridLength(animationClock.CurrentProgress.Value * (value2 - value) + value, (!To.IsStar) ? GridUnitType.Pixel : GridUnitType.Star);
	}

	protected override Freezable CreateInstanceCore()
	{
		return new GridLengthAnimation();
	}
}
