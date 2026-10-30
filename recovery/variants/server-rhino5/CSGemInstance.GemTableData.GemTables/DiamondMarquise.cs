using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondMarquise : IGemTable
{
	public int CurrentVersion { get; } = 2;

	public GemTableLayer Layer => GemTableLayer.Gem02;

	public GemTableShape Shape => GemTableShape.Marquise;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.00164337743490066;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		if (version == 1)
		{
			return new List<GemSize>
			{
				new GemSizePercent(2.5, 1.25, 56.0),
				new GemSizePercent(3.0, 1.5, 56.0),
				new GemSizePercent(3.5, 1.75, 56.0),
				new GemSizePercent(4.0, 2.0, 56.0, iteration: true),
				new GemSizePercent(4.25, 2.25, 56.0, iteration: true),
				new GemSizePercent(4.5, 2.5, 56.0, iteration: true),
				new GemSizePercent(5.0, 2.5, 56.0, iteration: true),
				new GemSizePercent(5.0, 3.0, 56.0, iteration: true),
				new GemSizePercent(6.0, 3.0, 56.0, iteration: true),
				new GemSizePercent(6.0, 3.5, 56.0, iteration: true),
				new GemSizePercent(7.0, 3.5, 56.0, iteration: true),
				new GemSizePercent(8.0, 4.0, 56.0, iteration: true),
				new GemSizePercent(9.0, 4.5, 56.0, iteration: true),
				new GemSizePercent(10.0, 5.0, 56.0, iteration: true),
				new GemSizePercent(10.5, 5.25, 56.0),
				new GemSizePercent(11.0, 5.5, 56.0),
				new GemSizePercent(12.0, 6.0, 56.0, iteration: true),
				new GemSizePercent(13.0, 6.5, 56.0),
				new GemSizePercent(14.0, 7.0, 56.0),
				new GemSizePercent(14.0, 7.5, 56.0),
				new GemSizePercent(16.0, 8.0, 56.0),
				new GemSizePercent(16.0, 8.5, 56.0)
			};
		}
		return new List<GemSize>
		{
			new GemSizePercent(2.5, 1.25, 56.0),
			new GemSizePercent(3.0, 1.5, 56.0, iteration: true),
			new GemSizePercent(3.5, 1.75, 56.0, iteration: true),
			new GemSizePercent(3.5, 2.0, 56.0, iteration: true),
			new GemSizePercent(4.0, 2.0, 56.0, iteration: true),
			new GemSizePercent(4.25, 2.25, 56.0, iteration: true),
			new GemSizePercent(4.5, 2.5, 56.0, iteration: true),
			new GemSizePercent(5.0, 2.5, 56.0, iteration: true),
			new GemSizePercent(5.0, 3.0, 56.0, iteration: true),
			new GemSizePercent(6.0, 3.0, 56.0, iteration: true),
			new GemSizePercent(6.0, 3.5, 56.0, iteration: true),
			new GemSizePercent(7.0, 3.5, 56.0, iteration: true),
			new GemSizePercent(8.0, 4.0, 56.0, iteration: true),
			new GemSizePercent(9.0, 4.5, 56.0, iteration: true),
			new GemSizePercent(10.0, 5.0, 56.0, iteration: true),
			new GemSizePercent(10.5, 5.25, 56.0),
			new GemSizePercent(11.0, 5.5, 56.0),
			new GemSizePercent(12.0, 6.0, 56.0, iteration: true),
			new GemSizePercent(13.0, 6.5, 56.0),
			new GemSizePercent(14.0, 7.0, 56.0),
			new GemSizePercent(14.0, 7.5, 56.0),
			new GemSizePercent(16.0, 8.0, 56.0),
			new GemSizePercent(16.0, 8.5, 56.0)
		};
	}
}
