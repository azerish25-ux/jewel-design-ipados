using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondBaguetteTapered : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem03;

	public GemTableShape Shape => GemTableShape.BaguetteTapered;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.003011;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizeTapered(1.25, 1.0, 0.75),
			new GemSizeTapered(1.5, 1.0, 0.75),
			new GemSizeTapered(1.5, 1.25, 1.0),
			new GemSizeTapered(1.75, 0.75, 0.5),
			new GemSizeTapered(1.75, 1.0, 0.5),
			new GemSizeTapered(1.75, 1.0, 0.75),
			new GemSizeTapered(1.75, 1.25, 1.0),
			new GemSizeTapered(1.75, 1.5, 1.0),
			new GemSizeTapered(1.75, 1.5, 1.25),
			new GemSizeTapered(2.0, 1.0, 0.5),
			new GemSizeTapered(2.0, 1.0, 0.75),
			new GemSizeTapered(2.0, 1.25, 1.0),
			new GemSizeTapered(2.0, 1.5, 1.0),
			new GemSizeTapered(2.0, 1.5, 1.25),
			new GemSizeTapered(2.25, 1.0, 0.5),
			new GemSizeTapered(2.25, 1.0, 0.5),
			new GemSizeTapered(2.25, 1.25, 0.75),
			new GemSizeTapered(2.25, 1.25, 1.0),
			new GemSizeTapered(2.25, 1.5, 1.0),
			new GemSizeTapered(2.25, 1.5, 1.25),
			new GemSizeTapered(2.25, 1.75, 1.25),
			new GemSizeTapered(2.25, 1.75, 1.5),
			new GemSizeTapered(2.25, 2.0, 1.5),
			new GemSizeTapered(2.5, 1.0, 0.5),
			new GemSizeTapered(2.5, 1.0, 0.75),
			new GemSizeTapered(2.5, 1.25, 1.0),
			new GemSizeTapered(2.5, 1.5, 1.0),
			new GemSizeTapered(2.5, 1.5, 1.25),
			new GemSizeTapered(2.5, 1.75, 1.0),
			new GemSizeTapered(2.5, 1.75, 1.25),
			new GemSizeTapered(2.5, 2.0, 1.5),
			new GemSizeTapered(2.75, 1.0, 0.5),
			new GemSizeTapered(2.75, 1.25, 0.75),
			new GemSizeTapered(2.75, 1.25, 1.0),
			new GemSizeTapered(2.75, 1.5, 1.0),
			new GemSizeTapered(2.75, 1.5, 1.25),
			new GemSizeTapered(2.75, 1.75, 1.25),
			new GemSizeTapered(2.75, 1.75, 1.5),
			new GemSizeTapered(2.75, 2.0, 1.5),
			new GemSizeTapered(3.0, 1.25, 0.75),
			new GemSizeTapered(3.0, 1.25, 1.0),
			new GemSizeTapered(3.0, 1.5, 1.0, 0.0, iteration: true),
			new GemSizeTapered(3.0, 1.5, 1.25),
			new GemSizeTapered(3.0, 1.75, 1.0, 0.0, iteration: true),
			new GemSizeTapered(3.0, 1.75, 1.25),
			new GemSizeTapered(3.0, 1.75, 1.5, 0.0, iteration: true),
			new GemSizeTapered(3.0, 2.0, 1.0),
			new GemSizeTapered(3.0, 2.0, 1.5, 0.0, iteration: true),
			new GemSizeTapered(3.0, 2.0, 1.75),
			new GemSizeTapered(3.0, 2.5, 1.75),
			new GemSizeTapered(3.25, 1.0, 0.5),
			new GemSizeTapered(3.25, 1.25, 0.75),
			new GemSizeTapered(3.25, 1.25, 1.0),
			new GemSizeTapered(3.25, 1.5, 0.75),
			new GemSizeTapered(3.25, 1.5, 1.0),
			new GemSizeTapered(3.25, 1.5, 1.25),
			new GemSizeTapered(3.25, 1.75, 1.0),
			new GemSizeTapered(3.25, 1.75, 1.25),
			new GemSizeTapered(3.25, 1.75, 1.5),
			new GemSizeTapered(3.25, 2.0, 1.0),
			new GemSizeTapered(3.25, 2.0, 1.25),
			new GemSizeTapered(3.25, 2.0, 1.5),
			new GemSizeTapered(3.25, 2.5, 1.75),
			new GemSizeTapered(3.5, 1.25, 1.0),
			new GemSizeTapered(3.5, 1.5, 1.0, 0.0, iteration: true),
			new GemSizeTapered(3.5, 1.5, 1.25),
			new GemSizeTapered(3.5, 1.75, 1.0),
			new GemSizeTapered(3.5, 1.75, 1.25),
			new GemSizeTapered(3.5, 2.0, 1.0),
			new GemSizeTapered(3.5, 2.0, 1.25),
			new GemSizeTapered(3.5, 2.0, 1.5, 0.0, iteration: true),
			new GemSizeTapered(3.5, 2.5, 1.5),
			new GemSizeTapered(3.75, 1.25, 1.0),
			new GemSizeTapered(3.75, 1.5, 1.0),
			new GemSizeTapered(3.75, 1.75, 1.25),
			new GemSizeTapered(3.75, 2.0, 1.25),
			new GemSizeTapered(3.75, 2.0, 1.5),
			new GemSizeTapered(3.75, 2.25, 1.75),
			new GemSizeTapered(4.0, 1.25, 1.0),
			new GemSizeTapered(4.0, 1.5, 1.0),
			new GemSizeTapered(4.0, 1.75, 1.25),
			new GemSizeTapered(4.0, 2.0, 1.0),
			new GemSizeTapered(4.0, 2.0, 1.5, 0.0, iteration: true),
			new GemSizeTapered(4.0, 2.5, 1.5),
			new GemSizeTapered(4.0, 2.5, 1.75),
			new GemSizeTapered(4.25, 2.0, 1.5),
			new GemSizeTapered(4.25, 2.25, 1.5),
			new GemSizeTapered(4.25, 2.5, 1.5),
			new GemSizeTapered(4.25, 2.75, 1.75),
			new GemSizeTapered(4.5, 1.5, 1.25),
			new GemSizeTapered(4.5, 2.0, 1.0),
			new GemSizeTapered(4.5, 2.0, 1.5),
			new GemSizeTapered(4.5, 2.25, 1.5),
			new GemSizeTapered(4.5, 2.5, 1.5),
			new GemSizeTapered(4.5, 2.5, 1.75),
			new GemSizeTapered(4.5, 2.75, 1.75),
			new GemSizeTapered(4.75, 1.75, 1.5),
			new GemSizeTapered(4.75, 2.0, 1.5),
			new GemSizeTapered(4.75, 2.25, 1.5, 0.0, iteration: true),
			new GemSizeTapered(4.75, 2.5, 1.5),
			new GemSizeTapered(4.75, 2.5, 2.0),
			new GemSizeTapered(4.75, 3.0, 2.0),
			new GemSizeTapered(5.0, 2.25, 1.25),
			new GemSizeTapered(5.0, 2.25, 1.75),
			new GemSizeTapered(5.0, 2.75, 2.0),
			new GemSizeTapered(5.5, 2.5, 1.0),
			new GemSizeTapered(5.5, 2.5, 2.25),
			new GemSizeTapered(5.5, 3.0, 2.25),
			new GemSizeTapered(6.0, 2.0, 1.5),
			new GemSizeTapered(6.0, 3.0, 2.25),
			new GemSizeTapered(7.0, 2.25, 1.5)
		};
	}
}
