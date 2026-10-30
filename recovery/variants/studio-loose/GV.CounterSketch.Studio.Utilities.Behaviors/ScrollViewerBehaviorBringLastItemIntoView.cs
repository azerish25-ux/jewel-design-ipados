using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to bring the last item into view when the scrollviewer extent changes.")]
public class ScrollViewerBehaviorBringLastItemIntoView : Behavior<ScrollViewer>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
	}

	protected override void OnDetaching()
	{
		base.AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
	}

	private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
	{
		if (e.ViewportHeightChange == 0.0 && e.ExtentHeightChange > 0.0)
		{
			base.AssociatedObject.ScrollToEnd();
		}
	}
}
