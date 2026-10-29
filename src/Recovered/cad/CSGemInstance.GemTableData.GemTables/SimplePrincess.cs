using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class SimplePrincess : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem02;

	public GemTableShape Shape => GemTableShape.Princess;

	public GemTableType Type => GemTableType.Simple;

	public double FudgeFactor => 0.003011;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(1.5, 1.5, 75.0),
			new GemSizePercent(2.0, 2.0, 75.0),
			new GemSizePercent(2.25, 2.25, 75.0),
			new GemSizePercent(2.5, 2.5, 75.0),
			new GemSizePercent(2.75, 2.75, 75.0),
			new GemSizePercent(3.0, 3.0, 75.0),
			new GemSizePercent(3.25, 3.25, 75.0),
			new GemSizePercent(3.5, 3.5, 75.0),
			new GemSizePercent(3.75, 3.75, 75.0),
			new GemSizePercent(4.0, 4.0, 75.0),
			new GemSizePercent(4.3, 4.3, 75.0),
			new GemSizePercent(4.5, 4.5, 75.0),
			new GemSizePercent(5.0, 5.0, 75.0),
			new GemSizePercent(5.5, 5.5, 75.0),
			new GemSizePercent(6.0, 6.0, 75.0),
			new GemSizePercent(6.5, 6.5, 75.0),
			new GemSizePercent(6.7, 6.7, 75.0),
			new GemSizePercent(7.0, 7.0, 75.0),
			new GemSizePercent(7.5, 7.5, 75.0),
			new GemSizePercent(8.0, 8.0, 75.0),
			new GemSizePercent(8.5, 8.5, 75.0),
			new GemSizePercent(9.0, 9.0, 75.0),
			new GemSizePercent(9.75, 9.75, 75.0),
			new GemSizePercent(10.5, 10.5, 75.0)
		};
	}
}
