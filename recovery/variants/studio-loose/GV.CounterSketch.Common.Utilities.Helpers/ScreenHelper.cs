using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace GV.CounterSketch.Common.Utilities.Helpers;

public class ScreenHelper
{
	public static Rectangle GetWindowRectangle(Window window = null)
	{
		Rectangle windowRectangle = default(Rectangle);
		System.Windows.Application.Current.Dispatcher.Invoke(delegate
		{
			if (window == null)
			{
				window = System.Windows.Application.Current.MainWindow;
			}
			if (window.WindowState == WindowState.Maximized)
			{
				windowRectangle = Screen.GetWorkingArea(new System.Drawing.Point((int)window.Left + (int)(window.ActualWidth / 2.0), (int)window.Top + (int)(window.ActualHeight / 2.0)));
			}
			else
			{
				windowRectangle = new Rectangle((int)window.Left, (int)window.Top, (int)window.ActualWidth, (int)window.ActualHeight);
			}
		});
		return windowRectangle;
	}
}
