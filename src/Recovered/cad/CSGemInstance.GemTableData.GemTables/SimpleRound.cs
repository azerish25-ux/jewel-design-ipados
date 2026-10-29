using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class SimpleRound : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem03;

	public GemTableShape Shape => GemTableShape.Round;

	public GemTableType Type => GemTableType.Simple;

	public double FudgeFactor => 0.001733;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(1.0, 1.0, 62.0),
			new GemSizePercent(1.1, 1.1, 62.0),
			new GemSizePercent(1.2, 1.2, 62.0),
			new GemSizePercent(1.3, 1.3, 62.0),
			new GemSizePercent(1.4, 1.4, 62.0),
			new GemSizePercent(1.5, 1.5, 62.0),
			new GemSizePercent(1.6, 1.6, 62.0),
			new GemSizePercent(1.7, 1.7, 62.0),
			new GemSizePercent(1.8, 1.8, 62.0),
			new GemSizePercent(1.9, 1.9, 62.0),
			new GemSizePercent(2.0, 2.0, 62.0),
			new GemSizePercent(2.1, 2.1, 62.0),
			new GemSizePercent(2.2, 2.2, 62.0),
			new GemSizePercent(2.4, 2.4, 62.0),
			new GemSizePercent(2.5, 2.5, 62.0),
			new GemSizePercent(2.7, 2.7, 62.0),
			new GemSizePercent(2.8, 2.8, 62.0),
			new GemSizePercent(2.9, 2.9, 62.0),
			new GemSizePercent(3.0, 3.0, 62.0),
			new GemSizePercent(3.1, 3.1, 62.0),
			new GemSizePercent(3.2, 3.2, 62.0),
			new GemSizePercent(3.3, 3.3, 62.0),
			new GemSizePercent(3.4, 3.4, 62.0),
			new GemSizePercent(3.5, 3.5, 62.0),
			new GemSizePercent(3.6, 3.6, 62.0),
			new GemSizePercent(3.7, 3.7, 62.0),
			new GemSizePercent(3.8, 3.8, 62.0),
			new GemSizePercent(3.9, 3.9, 62.0),
			new GemSizePercent(4.0, 4.0, 62.0),
			new GemSizePercent(4.1, 4.1, 62.0),
			new GemSizePercent(4.2, 4.2, 62.0),
			new GemSizePercent(4.4, 4.4, 62.0),
			new GemSizePercent(4.6, 4.6, 62.0),
			new GemSizePercent(4.8, 4.8, 62.0),
			new GemSizePercent(5.0, 5.0, 62.0),
			new GemSizePercent(5.2, 5.2, 62.0),
			new GemSizePercent(5.4, 5.4, 62.0),
			new GemSizePercent(5.6, 5.6, 62.0),
			new GemSizePercent(5.8, 5.8, 62.0),
			new GemSizePercent(6.0, 6.0, 62.0),
			new GemSizePercent(6.4, 6.4, 62.0),
			new GemSizePercent(6.5, 6.5, 62.0),
			new GemSizePercent(6.6, 6.6, 62.0),
			new GemSizePercent(6.8, 6.8, 62.0),
			new GemSizePercent(7.0, 7.0, 62.0),
			new GemSizePercent(7.2, 7.2, 62.0),
			new GemSizePercent(7.4, 7.4, 62.0),
			new GemSizePercent(7.6, 7.6, 62.0),
			new GemSizePercent(7.8, 7.8, 62.0),
			new GemSizePercent(8.0, 8.0, 62.0),
			new GemSizePercent(8.2, 8.2, 62.0),
			new GemSizePercent(8.4, 8.4, 62.0),
			new GemSizePercent(8.6, 8.6, 62.0),
			new GemSizePercent(8.8, 8.8, 62.0),
			new GemSizePercent(9.2, 9.2, 62.0),
			new GemSizePercent(9.4, 9.4, 62.0),
			new GemSizePercent(9.6, 9.6, 62.0),
			new GemSizePercent(9.8, 9.8, 62.0),
			new GemSizePercent(10.0, 10.0, 62.0),
			new GemSizePercent(10.2, 10.2, 62.0),
			new GemSizePercent(10.4, 10.4, 62.0),
			new GemSizePercent(10.6, 10.6, 62.0),
			new GemSizePercent(10.8, 10.8, 62.0),
			new GemSizePercent(11.0, 11.0, 62.0),
			new GemSizePercent(11.2, 11.2, 62.0)
		};
	}
}
