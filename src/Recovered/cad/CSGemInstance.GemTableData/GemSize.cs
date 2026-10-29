namespace CSGemInstance.GemTableData;

public class GemSize
{
	public double Height { get; }

	public double Width { get; }

	public double Depth { get; }

	public double X => Width;

	public double Y => Height;

	public double Z => Depth;

	public double DepthPercentage => Depth / Width * 100.0;

	public bool Iteration { get; private set; }

	public GemSize(double height, double width, double depth, bool iteration = false)
	{
		Height = height;
		Width = width;
		Depth = depth;
		Iteration = iteration;
	}

	public int CompareTo(GemSize v)
	{
		if (v != null)
		{
			return CornerLength().CompareTo(v.CornerLength());
		}
		return 1;
	}

	private double CornerLength()
	{
		return Height * Height + Width * Width + Depth * Depth;
	}
}
