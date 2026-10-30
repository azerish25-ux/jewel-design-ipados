using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondEmerald : IGemTable
{
	public int CurrentVersion { get; } = 2;

	public GemTableLayer Layer => GemTableLayer.Gem02;

	public GemTableShape Shape => GemTableShape.Emerald;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.00254381286646363;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		if (version == 1)
		{
			return new List<GemSize>
			{
				new GemSizePercent(3.0, 1.5, 65.0),
				new GemSizePercent(3.5, 1.5, 65.0),
				new GemSizePercent(3.75, 1.75, 65.0),
				new GemSizePercent(4.0, 2.0, 65.0),
				new GemSizePercent(5.0, 3.0, 65.0, iteration: true),
				new GemSizePercent(5.3, 3.3, 65.0),
				new GemSizePercent(5.8, 3.8, 65.0),
				new GemSizePercent(6.0, 4.0, 65.0, iteration: true),
				new GemSizePercent(6.5, 4.5, 65.0),
				new GemSizePercent(7.0, 5.0, 65.0, iteration: true),
				new GemSizePercent(7.5, 5.5, 65.0),
				new GemSizePercent(8.0, 6.0, 65.0, iteration: true),
				new GemSizePercent(8.5, 6.5, 65.0),
				new GemSizePercent(9.0, 7.0, 65.0, iteration: true),
				new GemSizePercent(10.0, 8.0, 65.0, iteration: true),
				new GemSizePercent(11.0, 9.0, 65.0, iteration: true),
				new GemSizePercent(12.0, 10.0, 65.0, iteration: true),
				new GemSizePercent(14.0, 10.0, 65.0, iteration: true),
				new GemSizePercent(15.0, 11.0, 65.0, iteration: true),
				new GemSizePercent(16.0, 12.0, 65.0, iteration: true)
			};
		}
		return new List<GemSize>
		{
			new GemSizePercent(3.0, 1.5, 65.0),
			new GemSizePercent(3.5, 1.5, 65.0),
			new GemSizePercent(3.75, 1.75, 65.0),
			new GemSizePercent(3.75, 2.5, 65.0, iteration: true),
			new GemSizePercent(4.0, 2.0, 65.0),
			new GemSizePercent(4.5, 3.0, 65.0, iteration: true),
			new GemSizePercent(5.0, 3.0, 65.0, iteration: true),
			new GemSizePercent(5.3, 3.3, 65.0),
			new GemSizePercent(5.8, 3.8, 65.0),
			new GemSizePercent(6.0, 4.0, 65.0, iteration: true),
			new GemSizePercent(6.5, 4.5, 65.0),
			new GemSizePercent(7.0, 5.0, 65.0, iteration: true),
			new GemSizePercent(7.5, 5.5, 65.0),
			new GemSizePercent(8.0, 6.0, 65.0, iteration: true),
			new GemSizePercent(8.5, 6.5, 65.0),
			new GemSizePercent(9.0, 7.0, 65.0, iteration: true),
			new GemSizePercent(10.0, 8.0, 65.0, iteration: true),
			new GemSizePercent(11.0, 9.0, 65.0, iteration: true),
			new GemSizePercent(12.0, 10.0, 65.0, iteration: true),
			new GemSizePercent(14.0, 10.0, 65.0, iteration: true),
			new GemSizePercent(15.0, 11.0, 65.0, iteration: true),
			new GemSizePercent(16.0, 12.0, 65.0, iteration: true)
		};
	}
}
