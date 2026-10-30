using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to scroll first item into view when the scrollviewer extent changes.")]
public class ScrollViewerBehaviorBringFirstItemIntoView : Behavior<ScrollViewer>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
	}

	protected override void OnDetaching()
	{
		base.AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
	}

	private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
	{
		if (e.ExtentHeightChange != 0.0)
		{
			base.AssociatedObject.ScrollToTop();
		}
		if (e.ExtentWidthChange != 0.0)
		{
			base.AssociatedObject.ScrollToHome();
		}
	}
}
