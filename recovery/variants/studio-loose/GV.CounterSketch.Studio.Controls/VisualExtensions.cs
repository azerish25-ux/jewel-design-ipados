using System.Windows;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Controls;

public static class VisualExtensions
{
	public static Window GetParentWindow(this Visual visualObject)
	{
		Window result = null;
		if (visualObject != null)
		{
			Visual visualObject2 = VisualTreeHelper.GetParent(visualObject) as Visual;
			result = ((!(visualObject is Window)) ? visualObject2.GetParentWindow() : (visualObject as Window));
		}
		return result;
	}
}
