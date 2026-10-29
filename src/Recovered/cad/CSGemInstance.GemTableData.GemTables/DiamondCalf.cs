using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondCalf : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem04;

	public GemTableShape Shape => GemTableShape.Calf;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.00189;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(3.0, 3.0, 49.0),
			new GemSizePercent(3.5, 3.5, 49.0),
			new GemSizePercent(3.75, 3.75, 49.0),
			new GemSizePercent(4.0, 4.0, 49.0),
			new GemSizePercent(4.25, 4.25, 49.0),
			new GemSizePercent(4.5, 4.5, 49.0),
			new GemSizePercent(5.0, 5.0, 49.0),
			new GemSizePercent(5.5, 5.5, 49.0),
			new GemSizePercent(6.0, 6.0, 49.0),
			new GemSizePercent(6.1, 6.1, 49.0),
			new GemSizePercent(6.25, 6.25, 49.0),
			new GemSizePercent(6.5, 6.5, 49.0),
			new GemSizePercent(6.75, 6.75, 49.0),
			new GemSizePercent(7.0, 7.0, 49.0),
			new GemSizePercent(7.5, 7.5, 49.0),
			new GemSizePercent(8.0, 8.0, 49.0),
			new GemSizePercent(9.0, 9.0, 49.0),
			new GemSizePercent(10.0, 10.0, 49.0)
		};
	}
}
