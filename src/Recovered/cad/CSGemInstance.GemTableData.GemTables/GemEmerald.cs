using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class GemEmerald : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem02;

	public GemTableShape Shape => GemTableShape.Emerald;

	public GemTableType Type => GemTableType.Gem;

	public double FudgeFactor => 0.0027;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(3.0, 1.5, 67.0),
			new GemSizePercent(3.5, 1.5, 67.0),
			new GemSizePercent(3.75, 1.75, 67.0),
			new GemSizePercent(4.0, 2.0, 67.0),
			new GemSizePercent(5.0, 3.0, 67.0),
			new GemSizePercent(5.3, 3.3, 67.0),
			new GemSizePercent(5.8, 3.8, 67.0),
			new GemSizePercent(6.0, 4.0, 67.0),
			new GemSizePercent(6.5, 4.5, 67.0),
			new GemSizePercent(7.0, 5.0, 67.0),
			new GemSizePercent(7.5, 5.5, 67.0),
			new GemSizePercent(8.0, 6.0, 67.0),
			new GemSizePercent(8.5, 6.5, 67.0),
			new GemSizePercent(9.0, 7.0, 67.0),
			new GemSizePercent(10.0, 8.0, 67.0),
			new GemSizePercent(11.0, 9.0, 67.0),
			new GemSizePercent(12.0, 10.0, 67.0)
		};
	}
}
