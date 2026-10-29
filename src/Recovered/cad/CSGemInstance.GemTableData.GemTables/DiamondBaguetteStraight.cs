using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondBaguetteStraight : IGemTable
{
	public int CurrentVersion { get; } = 2;

	public GemTableLayer Layer => GemTableLayer.Gem03;

	public GemTableShape Shape => GemTableShape.BaguetteStraight;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.003011;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		if (version == 1)
		{
			return new List<GemSize>
			{
				new GemSizePercent(1.25, 1.0, 64.0),
				new GemSizePercent(1.5, 1.25, 64.0),
				new GemSizePercent(1.5, 1.0, 64.0),
				new GemSizePercent(1.75, 1.0, 64.0),
				new GemSizePercent(1.75, 1.25, 64.0),
				new GemSizePercent(1.75, 1.5, 64.0),
				new GemSizePercent(2.0, 1.0, 64.0),
				new GemSizePercent(2.0, 1.25, 64.0),
				new GemSizePercent(2.0, 1.5, 64.0),
				new GemSizePercent(2.0, 1.75, 64.0),
				new GemSizePercent(2.25, 1.0, 64.0),
				new GemSizePercent(2.25, 1.25, 64.0),
				new GemSizePercent(2.25, 1.5, 64.0),
				new GemSizePercent(2.25, 1.75, 64.0),
				new GemSizePercent(2.25, 2.0, 64.0),
				new GemSizePercent(2.5, 1.0, 64.0),
				new GemSizePercent(2.5, 1.25, 64.0),
				new GemSizePercent(2.5, 1.5, 64.0),
				new GemSizePercent(2.5, 1.75, 64.0),
				new GemSizePercent(2.5, 2.0, 64.0),
				new GemSizePercent(2.75, 1.25, 64.0),
				new GemSizePercent(2.75, 1.5, 64.0),
				new GemSizePercent(2.75, 1.75, 64.0),
				new GemSizePercent(2.75, 2.0, 64.0),
				new GemSizePercent(2.75, 2.5, 64.0),
				new GemSizePercent(3.0, 1.0, 64.0),
				new GemSizePercent(3.0, 1.25, 64.0),
				new GemSizePercent(3.0, 1.5, 64.0, iteration: true),
				new GemSizePercent(3.0, 1.75, 64.0, iteration: true),
				new GemSizePercent(3.0, 2.0, 64.0, iteration: true),
				new GemSizePercent(3.0, 2.5, 64.0),
				new GemSizePercent(3.25, 1.25, 64.0),
				new GemSizePercent(3.25, 1.5, 64.0),
				new GemSizePercent(3.25, 1.75, 64.0),
				new GemSizePercent(3.25, 2.0, 64.0),
				new GemSizePercent(3.25, 2.5, 64.0),
				new GemSizePercent(3.5, 1.5, 64.0, iteration: true),
				new GemSizePercent(3.5, 1.75, 64.0),
				new GemSizePercent(3.5, 2.0, 64.0, iteration: true),
				new GemSizePercent(3.5, 2.5, 64.0),
				new GemSizePercent(3.75, 1.25, 64.0),
				new GemSizePercent(3.75, 1.5, 64.0),
				new GemSizePercent(3.75, 1.75, 64.0),
				new GemSizePercent(3.75, 2.0, 64.0, iteration: true),
				new GemSizePercent(3.75, 2.5, 64.0),
				new GemSizePercent(3.75, 3.0, 64.0),
				new GemSizePercent(4.0, 1.5, 64.0),
				new GemSizePercent(4.0, 1.75, 64.0),
				new GemSizePercent(4.0, 2.0, 64.0, iteration: true),
				new GemSizePercent(4.0, 2.5, 64.0),
				new GemSizePercent(4.0, 3.0, 64.0, iteration: true),
				new GemSizePercent(4.5, 2.0, 64.0, iteration: true),
				new GemSizePercent(4.5, 2.5, 64.0, iteration: true),
				new GemSizePercent(5.0, 2.0, 64.0, iteration: true),
				new GemSizePercent(5.0, 2.5, 64.0, iteration: true),
				new GemSizePercent(5.0, 3.0, 64.0, iteration: true),
				new GemSizePercent(5.5, 2.5, 64.0),
				new GemSizePercent(6.0, 3.0, 64.0, iteration: true),
				new GemSizePercent(8.0, 4.0, 64.0, iteration: true)
			};
		}
		return new List<GemSize>
		{
			new GemSizePercent(1.25, 1.0, 64.0),
			new GemSizePercent(1.5, 1.25, 64.0),
			new GemSizePercent(1.5, 1.0, 64.0),
			new GemSizePercent(1.75, 1.0, 64.0),
			new GemSizePercent(1.75, 1.25, 64.0),
			new GemSizePercent(1.75, 1.5, 64.0),
			new GemSizePercent(2.0, 1.0, 64.0),
			new GemSizePercent(2.0, 1.25, 64.0),
			new GemSizePercent(2.0, 1.5, 64.0),
			new GemSizePercent(2.0, 1.75, 64.0),
			new GemSizePercent(2.25, 1.0, 64.0),
			new GemSizePercent(2.25, 1.25, 64.0),
			new GemSizePercent(2.25, 1.5, 64.0),
			new GemSizePercent(2.25, 1.75, 64.0),
			new GemSizePercent(2.25, 2.0, 64.0),
			new GemSizePercent(2.5, 1.0, 64.0),
			new GemSizePercent(2.5, 1.25, 64.0),
			new GemSizePercent(2.5, 1.5, 64.0),
			new GemSizePercent(2.5, 1.75, 64.0),
			new GemSizePercent(2.5, 2.0, 64.0),
			new GemSizePercent(2.75, 1.25, 64.0),
			new GemSizePercent(2.75, 1.5, 64.0),
			new GemSizePercent(2.75, 1.75, 64.0),
			new GemSizePercent(2.75, 2.0, 64.0),
			new GemSizePercent(2.75, 2.5, 64.0),
			new GemSizePercent(3.0, 1.0, 64.0),
			new GemSizePercent(3.0, 1.25, 64.0),
			new GemSizePercent(3.0, 1.5, 64.0, iteration: true),
			new GemSizePercent(3.0, 1.75, 64.0, iteration: true),
			new GemSizePercent(3.0, 2.0, 64.0, iteration: true),
			new GemSizePercent(3.0, 2.5, 64.0),
			new GemSizePercent(3.25, 1.25, 64.0),
			new GemSizePercent(3.25, 1.5, 64.0),
			new GemSizePercent(3.25, 1.75, 64.0),
			new GemSizePercent(3.25, 2.0, 64.0, iteration: true),
			new GemSizePercent(3.25, 2.5, 64.0),
			new GemSizePercent(3.5, 1.5, 64.0, iteration: true),
			new GemSizePercent(3.5, 1.75, 64.0),
			new GemSizePercent(3.5, 2.0, 64.0, iteration: true),
			new GemSizePercent(3.5, 2.5, 64.0),
			new GemSizePercent(3.75, 1.25, 64.0),
			new GemSizePercent(3.75, 1.5, 64.0),
			new GemSizePercent(3.75, 1.75, 64.0),
			new GemSizePercent(3.75, 2.0, 64.0, iteration: true),
			new GemSizePercent(3.75, 2.5, 64.0),
			new GemSizePercent(3.75, 3.0, 64.0),
			new GemSizePercent(4.0, 1.5, 64.0),
			new GemSizePercent(4.0, 1.75, 64.0),
			new GemSizePercent(4.0, 2.0, 64.0, iteration: true),
			new GemSizePercent(4.0, 2.5, 64.0),
			new GemSizePercent(4.0, 3.0, 64.0, iteration: true),
			new GemSizePercent(4.5, 2.0, 64.0, iteration: true),
			new GemSizePercent(4.5, 2.5, 64.0, iteration: true),
			new GemSizePercent(4.5, 3.0, 64.0, iteration: true),
			new GemSizePercent(5.0, 2.0, 64.0, iteration: true),
			new GemSizePercent(5.0, 2.5, 64.0, iteration: true),
			new GemSizePercent(5.0, 3.0, 64.0, iteration: true),
			new GemSizePercent(5.5, 2.5, 64.0),
			new GemSizePercent(6.0, 3.0, 64.0, iteration: true),
			new GemSizePercent(8.0, 4.0, 64.0, iteration: true)
		};
	}
}
