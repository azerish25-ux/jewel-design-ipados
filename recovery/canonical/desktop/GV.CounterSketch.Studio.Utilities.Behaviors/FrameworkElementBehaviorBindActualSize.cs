using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to gain access to ActualHeight and ActualWidth properties of the FrameworkElement, which is not bindable otherwise.")]
public class FrameworkElementBehaviorBindActualSize : Behavior<FrameworkElement>
{
	[Description("Property that can be used to access the ActualHeight of the FrameworkElement, to be used for binding.")]
	[Category("ElementProperties")]
	public static readonly DependencyProperty ElementActualHeightProperty = DependencyProperty.Register("ElementActualHeight", typeof(double), typeof(FrameworkElementBehaviorBindActualSize), new UIPropertyMetadata(0.0));

	[Description("Property that can be used to access the ActualWidth of the FrameworkElement, to be used for binding.")]
	[Category("ElementProperties")]
	public static readonly DependencyProperty ElementActualWidthProperty = DependencyProperty.Register("ElementActualWidth", typeof(double), typeof(FrameworkElementBehaviorBindActualSize), new UIPropertyMetadata(0.0));

	public double ElementActualHeight
	{
		get
		{
			return (double)GetValue(ElementActualHeightProperty);
		}
		set
		{
			SetValue(ElementActualHeightProperty, value);
		}
	}

	public double ElementActualWidth
	{
		get
		{
			return (double)GetValue(ElementActualWidthProperty);
		}
		set
		{
			SetValue(ElementActualWidthProperty, value);
		}
	}

	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
	}

	protected override void OnDetaching()
	{
		base.AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
	}

	private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (base.AssociatedObject != null)
		{
			ElementActualWidth = base.AssociatedObject.ActualWidth;
			ElementActualHeight = base.AssociatedObject.ActualHeight;
		}
	}
}
