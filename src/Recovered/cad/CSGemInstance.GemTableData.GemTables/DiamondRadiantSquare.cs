using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondRadiantSquare : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem03;

	public GemTableShape Shape => GemTableShape.RadiantSquare;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.00202211102754885;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(3.0, 3.0, 72.0),
			new GemSizePercent(3.5, 3.5, 72.0),
			new GemSizePercent(4.0, 4.0, 72.0),
			new GemSizePercent(4.5, 4.5, 72.0),
			new GemSizePercent(5.0, 5.0, 72.0),
			new GemSizePercent(5.5, 5.5, 72.0),
			new GemSizePercent(6.0, 6.0, 72.0),
			new GemSizePercent(6.5, 6.5, 72.0),
			new GemSizePercent(7.0, 7.0, 72.0),
			new GemSizePercent(7.5, 7.5, 72.0),
			new GemSizePercent(8.0, 8.0, 72.0),
			new GemSizePercent(8.5, 8.5, 72.0),
			new GemSizePercent(9.0, 9.0, 72.0),
			new GemSizePercent(9.5, 9.5, 72.0),
			new GemSizePercent(10.0, 10.0, 72.0),
			new GemSizePercent(10.5, 10.5, 72.0),
			new GemSizePercent(11.0, 11.0, 72.0),
			new GemSizePercent(11.5, 11.5, 72.0),
			new GemSizePercent(12.0, 12.0, 72.0),
			new GemSizePercent(12.5, 12.5, 72.0),
			new GemSizePercent(13.0, 13.0, 72.0),
			new GemSizePercent(13.5, 13.5, 72.0),
			new GemSizePercent(14.0, 14.0, 72.0),
			new GemSizePercent(14.5, 14.5, 72.0),
			new GemSizePercent(15.0, 15.0, 72.0),
			new GemSizePercent(15.5, 15.5, 72.0),
			new GemSizePercent(16.0, 16.0, 72.0)
		};
	}
}
