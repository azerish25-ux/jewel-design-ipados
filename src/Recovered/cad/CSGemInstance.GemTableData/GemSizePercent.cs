namespace CSGemInstance.GemTableData;

public sealed class GemSizePercent : GemSize
{
	public GemSizePercent(double height, double width, double depthPercentage, bool iteration = false)
		: base(height, width, width * depthPercentage / 100.0, iteration)
	{
	}
}
