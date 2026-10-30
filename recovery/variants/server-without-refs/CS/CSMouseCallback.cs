using System.Windows.Forms;
using Rhino.UI;

namespace CS;

public sealed class CSMouseCallback : MouseCallback
{
	public static bool IsLeftMouseButtonDown { get; set; }

	public static bool IsMiddleMouseButtonDown { get; set; }

	public static bool IsRightMouseButtonDown { get; set; }

	public CSMouseCallback()
	{
		((MouseCallback)this).Enabled = true;
	}

	protected override void OnMouseDown(MouseCallbackEventArgs mouseCallbackEventArgs)
	{
		switch (mouseCallbackEventArgs.Button)
		{
		case MouseButtons.Left:
			IsLeftMouseButtonDown = true;
			break;
		case MouseButtons.Right:
			IsRightMouseButtonDown = true;
			break;
		case MouseButtons.Middle:
			IsMiddleMouseButtonDown = true;
			break;
		}
		((MouseCallback)this).OnMouseDown(mouseCallbackEventArgs);
	}

	protected override void OnMouseMove(MouseCallbackEventArgs mouseCallbackEventArgs)
	{
		IsLeftMouseButtonDown = false;
		IsRightMouseButtonDown = false;
		IsMiddleMouseButtonDown = false;
		((MouseCallback)this).OnMouseMove(mouseCallbackEventArgs);
	}
}
