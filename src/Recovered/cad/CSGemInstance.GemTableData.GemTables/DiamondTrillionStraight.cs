using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondTrillionStraight : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem01;

	public GemTableShape Shape => GemTableShape.TrillionStraight;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.001903;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(3.0, 3.0, 50.0, iteration: true),
			new GemSizePercent(3.5, 3.5, 50.0, iteration: true),
			new GemSizePercent(3.75, 3.75, 50.0),
			new GemSizePercent(4.0, 4.0, 50.0, iteration: true),
			new GemSizePercent(4.25, 4.25, 50.0),
			new GemSizePercent(4.5, 4.5, 50.0, iteration: true),
			new GemSizePercent(5.0, 5.0, 50.0, iteration: true),
			new GemSizePercent(5.5, 5.5, 50.0, iteration: true),
			new GemSizePercent(6.0, 6.0, 50.0, iteration: true),
			new GemSizePercent(6.1, 6.1, 50.0),
			new GemSizePercent(6.25, 6.25, 50.0),
			new GemSizePercent(6.5, 6.5, 50.0, iteration: true),
			new GemSizePercent(6.75, 6.75, 50.0),
			new GemSizePercent(7.0, 7.0, 50.0, iteration: true),
			new GemSizePercent(7.5, 7.5, 50.0),
			new GemSizePercent(8.0, 8.0, 50.0, iteration: true),
			new GemSizePercent(9.0, 9.0, 50.0, iteration: true),
			new GemSizePercent(10.0, 10.0, 50.0, iteration: true),
			new GemSizePercent(11.0, 11.0, 50.0, iteration: true),
			new GemSizePercent(12.0, 12.0, 50.0, iteration: true)
		};
	}
}
