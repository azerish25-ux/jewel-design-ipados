using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class SimpleOval : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem01;

	public GemTableShape Shape => GemTableShape.Oval;

	public GemTableType Type => GemTableType.Simple;

	public double FudgeFactor => 0.001903;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(4.0, 2.0, 61.0),
			new GemSizePercent(4.5, 2.5, 61.0),
			new GemSizePercent(5.0, 3.0, 61.0),
			new GemSizePercent(5.3, 3.3, 61.0),
			new GemSizePercent(5.8, 3.8, 61.0),
			new GemSizePercent(6.0, 4.0, 61.0),
			new GemSizePercent(6.1, 4.1, 61.0),
			new GemSizePercent(6.2, 4.2, 61.0),
			new GemSizePercent(6.5, 4.5, 61.0),
			new GemSizePercent(6.6, 4.6, 61.0),
			new GemSizePercent(6.9, 4.9, 61.0),
			new GemSizePercent(7.0, 5.0, 61.0),
			new GemSizePercent(7.2, 5.2, 61.0),
			new GemSizePercent(7.5, 5.5, 61.0),
			new GemSizePercent(8.0, 6.0, 61.0),
			new GemSizePercent(8.25, 6.25, 61.0),
			new GemSizePercent(8.5, 6.5, 61.0),
			new GemSizePercent(9.0, 7.0, 61.0),
			new GemSizePercent(10.0, 8.0, 61.0),
			new GemSizePercent(10.0, 8.5, 61.0),
			new GemSizePercent(11.0, 9.0, 61.0),
			new GemSizePercent(11.0, 9.5, 61.0),
			new GemSizePercent(12.0, 10.0, 61.0)
		};
	}
}
