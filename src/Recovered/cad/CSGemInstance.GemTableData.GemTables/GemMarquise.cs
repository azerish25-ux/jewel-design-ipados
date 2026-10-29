using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class GemMarquise : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem04;

	public GemTableShape Shape => GemTableShape.Marquise;

	public GemTableType Type => GemTableType.Gem;

	public double FudgeFactor => 0.0019;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(2.5, 1.25, 60.0),
			new GemSizePercent(3.0, 1.5, 60.0),
			new GemSizePercent(3.5, 1.75, 60.0),
			new GemSizePercent(4.0, 2.0, 60.0),
			new GemSizePercent(4.25, 2.25, 60.0),
			new GemSizePercent(4.5, 2.5, 60.0),
			new GemSizePercent(5.0, 2.5, 60.0),
			new GemSizePercent(5.0, 3.0, 60.0),
			new GemSizePercent(6.0, 3.0, 60.0),
			new GemSizePercent(7.0, 3.5, 60.0),
			new GemSizePercent(8.0, 4.0, 60.0),
			new GemSizePercent(9.0, 4.5, 60.0),
			new GemSizePercent(10.0, 5.0, 60.0),
			new GemSizePercent(10.5, 5.25, 60.0),
			new GemSizePercent(11.0, 5.5, 60.0),
			new GemSizePercent(12.0, 6.0, 60.0),
			new GemSizePercent(13.0, 6.5, 60.0),
			new GemSizePercent(14.0, 7.0, 60.0),
			new GemSizePercent(14.0, 7.5, 60.0),
			new GemSizePercent(16.0, 8.0, 60.0),
			new GemSizePercent(16.0, 8.5, 60.0)
		};
	}
}
