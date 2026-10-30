using System.Windows;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Utilities.Extensions;

public static class VisualExtensions
{
	public static Point PointToScreenDPI(this Visual visual, Point pt)
	{
		Point pt2 = visual.PointToScreen(pt);
		return visual.TransformToDeviceDPI(pt2);
	}

	public static Size TransformFromDeviceDPI(this Visual visual, Size size)
	{
		Matrix transformToDevice = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
		return new Size(size.Width * transformToDevice.M11, size.Height * transformToDevice.M22);
	}

	public static Point TransformFromDeviceDPI(this Visual visual, Point pt)
	{
		Matrix transformToDevice = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
		return new Point(pt.X * transformToDevice.M11, pt.Y * transformToDevice.M22);
	}

	public static Point TransformToDeviceDPI(this Visual visual, Point pt)
	{
		Matrix transformToDevice = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
		return new Point(pt.X / transformToDevice.M11, pt.Y / transformToDevice.M22);
	}

	public static Size TransformToDeviceDPI(this Visual visual, Size size)
	{
		Matrix transformToDevice = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
		return new Size(size.Width / transformToDevice.M11, size.Height / transformToDevice.M22);
	}
}
