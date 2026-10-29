using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondHeart : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem01;

	public GemTableShape Shape => GemTableShape.Heart;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.001676;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(2.5, 2.5, 60.0),
			new GemSizePercent(3.0, 3.0, 60.0),
			new GemSizePercent(3.25, 3.25, 60.0),
			new GemSizePercent(3.5, 3.5, 60.0),
			new GemSizePercent(4.0, 4.0, 60.0),
			new GemSizePercent(4.25, 4.25, 60.0),
			new GemSizePercent(4.75, 4.75, 60.0),
			new GemSizePercent(5.0, 5.0, 60.0, iteration: true),
			new GemSizePercent(5.5, 5.5, 60.0),
			new GemSizePercent(6.0, 6.0, 60.0, iteration: true),
			new GemSizePercent(6.1, 6.1, 60.0),
			new GemSizePercent(6.4, 6.4, 60.0),
			new GemSizePercent(6.5, 6.5, 60.0),
			new GemSizePercent(6.75, 6.75, 60.0),
			new GemSizePercent(7.0, 7.0, 60.0, iteration: true),
			new GemSizePercent(8.0, 8.0, 60.0, iteration: true),
			new GemSizePercent(8.5, 8.5, 60.0),
			new GemSizePercent(9.0, 9.0, 60.0, iteration: true),
			new GemSizePercent(9.5, 9.5, 60.0),
			new GemSizePercent(10.0, 10.0, 60.0, iteration: true),
			new GemSizePercent(10.5, 10.5, 60.0),
			new GemSizePercent(11.0, 11.0, 60.0)
		};
	}
}
