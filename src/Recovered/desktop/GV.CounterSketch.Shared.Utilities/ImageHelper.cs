using System.Drawing;

namespace GV.CounterSketch.Shared.Utilities;

public class ImageHelper
{
	public static uint ColorToUInt(Color color)
	{
		return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B);
	}
}
