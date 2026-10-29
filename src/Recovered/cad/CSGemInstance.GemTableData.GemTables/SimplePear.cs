using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class SimplePear : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem03;

	public GemTableShape Shape => GemTableShape.Pear;

	public GemTableType Type => GemTableType.Simple;

	public double FudgeFactor => 0.001634;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(2.5, 1.3, 64.0),
			new GemSizePercent(3.0, 2.5, 64.0),
			new GemSizePercent(4.0, 2.5, 64.0),
			new GemSizePercent(4.0, 3.0, 64.0),
			new GemSizePercent(4.5, 3.0, 64.0),
			new GemSizePercent(5.0, 3.0, 64.0),
			new GemSizePercent(5.5, 3.5, 64.0),
			new GemSizePercent(5.8, 3.8, 64.0),
			new GemSizePercent(6.0, 4.0, 64.0),
			new GemSizePercent(7.0, 5.0, 64.0),
			new GemSizePercent(8.0, 5.0, 64.0),
			new GemSizePercent(8.5, 5.5, 64.0),
			new GemSizePercent(9.0, 6.0, 64.0),
			new GemSizePercent(10.0, 7.0, 64.0),
			new GemSizePercent(12.0, 7.0, 64.0),
			new GemSizePercent(12.0, 8.0, 64.0),
			new GemSizePercent(13.0, 8.0, 64.0),
			new GemSizePercent(14.0, 8.0, 64.0),
			new GemSizePercent(14.5, 9.0, 64.0),
			new GemSizePercent(15.0, 9.0, 64.0)
		};
	}
}
