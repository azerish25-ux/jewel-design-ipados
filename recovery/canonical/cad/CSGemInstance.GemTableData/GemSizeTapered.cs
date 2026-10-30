namespace CSGemInstance.GemTableData;

public sealed class GemSizeTapered : GemSize
{
	public double Width2 { get; private set; }

	public GemSizeTapered(double height, double width, double width2, double depth = 0.0, bool iteration = false)
		: base(height, width, (depth == 0.0) ? (width * 0.64) : depth, iteration)
	{
		Width2 = width2;
	}
}
