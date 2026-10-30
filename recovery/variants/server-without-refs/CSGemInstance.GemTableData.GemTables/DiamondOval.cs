using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondOval : IGemTable
{
	public int CurrentVersion { get; } = 5;

	public GemTableLayer Layer => GemTableLayer.Gem01;

	public GemTableShape Shape => GemTableShape.Oval;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.00175708357743902;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		switch (version)
		{
		case 1:
		case 2:
		case 3:
			return new List<GemSize>
			{
				new GemSizePercent(4.0, 2.0, 61.0),
				new GemSizePercent(4.0, 3.0, 61.0, iteration: true),
				new GemSizePercent(4.5, 2.5, 61.0),
				new GemSizePercent(4.5, 3.5, 61.0, iteration: true),
				new GemSizePercent(5.0, 3.0, 61.0, iteration: true),
				new GemSizePercent(5.0, 4.0, 61.0, iteration: true),
				new GemSizePercent(5.3, 3.3, 61.0),
				new GemSizePercent(5.8, 3.8, 61.0),
				new GemSizePercent(6.0, 4.0, 61.0, iteration: true),
				new GemSizePercent(6.1, 4.1, 61.0),
				new GemSizePercent(6.2, 4.2, 61.0),
				new GemSizePercent(6.5, 4.5, 61.0),
				new GemSizePercent(6.6, 4.6, 61.0),
				new GemSizePercent(6.9, 4.9, 61.0),
				new GemSizePercent(7.0, 5.0, 61.0, iteration: true),
				new GemSizePercent(7.2, 5.2, 61.0),
				new GemSizePercent(7.5, 5.5, 61.0),
				new GemSizePercent(8.0, 6.0, 61.0, iteration: true),
				new GemSizePercent(8.25, 6.25, 61.0),
				new GemSizePercent(8.5, 6.5, 61.0),
				new GemSizePercent(9.0, 7.0, 61.0, iteration: true),
				new GemSizePercent(10.0, 8.0, 61.0, iteration: true),
				new GemSizePercent(10.0, 8.5, 61.0),
				new GemSizePercent(11.0, 9.0, 61.0, iteration: true),
				new GemSizePercent(11.0, 9.5, 61.0),
				new GemSizePercent(12.0, 10.0, 61.0, iteration: true),
				new GemSizePercent(14.0, 10.0, 61.0, iteration: true),
				new GemSizePercent(15.0, 11.0, 61.0, iteration: true),
				new GemSizePercent(16.0, 12.0, 61.0, iteration: true)
			};
		case 4:
			return new List<GemSize>
			{
				new GemSizePercent(4.0, 2.0, 61.0),
				new GemSizePercent(4.0, 3.0, 61.0),
				new GemSizePercent(4.5, 2.5, 61.0),
				new GemSizePercent(4.5, 3.5, 61.0),
				new GemSizePercent(5.0, 3.0, 61.0, iteration: true),
				new GemSizePercent(5.0, 4.0, 61.0),
				new GemSizePercent(5.3, 3.3, 61.0),
				new GemSizePercent(5.8, 3.8, 61.0),
				new GemSizePercent(6.0, 4.0, 61.0, iteration: true),
				new GemSizePercent(6.1, 4.1, 61.0),
				new GemSizePercent(6.2, 4.2, 61.0),
				new GemSizePercent(6.5, 4.5, 61.0),
				new GemSizePercent(6.6, 4.6, 61.0),
				new GemSizePercent(6.9, 4.9, 61.0),
				new GemSizePercent(7.0, 5.0, 61.0, iteration: true),
				new GemSizePercent(7.2, 5.2, 61.0),
				new GemSizePercent(7.5, 5.5, 61.0),
				new GemSizePercent(8.0, 6.0, 61.0, iteration: true),
				new GemSizePercent(8.25, 6.25, 61.0),
				new GemSizePercent(8.5, 6.5, 61.0),
				new GemSizePercent(9.0, 7.0, 61.0, iteration: true),
				new GemSizePercent(10.0, 8.0, 61.0, iteration: true),
				new GemSizePercent(10.0, 8.5, 61.0),
				new GemSizePercent(11.0, 9.0, 61.0, iteration: true),
				new GemSizePercent(11.0, 9.5, 61.0),
				new GemSizePercent(12.0, 10.0, 61.0, iteration: true),
				new GemSizePercent(14.0, 10.0, 61.0, iteration: true),
				new GemSizePercent(15.0, 11.0, 61.0, iteration: true),
				new GemSizePercent(16.0, 12.0, 61.0, iteration: true)
			};
		default:
			return new List<GemSize>
			{
				new GemSizePercent(4.0, 2.0, 61.0),
				new GemSizePercent(4.0, 3.0, 61.0, iteration: true),
				new GemSizePercent(4.5, 2.5, 61.0),
				new GemSizePercent(4.5, 3.5, 61.0, iteration: true),
				new GemSizePercent(5.0, 3.0, 61.0, iteration: true),
				new GemSizePercent(5.0, 4.0, 61.0),
				new GemSizePercent(5.3, 3.3, 61.0),
				new GemSizePercent(5.8, 3.8, 61.0),
				new GemSizePercent(6.0, 4.0, 61.0, iteration: true),
				new GemSizePercent(6.1, 4.1, 61.0),
				new GemSizePercent(6.2, 4.2, 61.0),
				new GemSizePercent(6.5, 4.5, 61.0),
				new GemSizePercent(6.6, 4.6, 61.0),
				new GemSizePercent(6.9, 4.9, 61.0),
				new GemSizePercent(7.0, 5.0, 61.0, iteration: true),
				new GemSizePercent(7.2, 5.2, 61.0),
				new GemSizePercent(7.5, 5.5, 61.0),
				new GemSizePercent(8.0, 6.0, 61.0, iteration: true),
				new GemSizePercent(8.25, 6.25, 61.0),
				new GemSizePercent(8.5, 6.5, 61.0),
				new GemSizePercent(9.0, 7.0, 61.0, iteration: true),
				new GemSizePercent(10.0, 8.0, 61.0, iteration: true),
				new GemSizePercent(10.0, 8.5, 61.0),
				new GemSizePercent(11.0, 9.0, 61.0, iteration: true),
				new GemSizePercent(11.0, 9.5, 61.0),
				new GemSizePercent(12.0, 10.0, 61.0, iteration: true),
				new GemSizePercent(14.0, 10.0, 61.0, iteration: true),
				new GemSizePercent(15.0, 11.0, 61.0, iteration: true),
				new GemSizePercent(16.0, 12.0, 61.0, iteration: true)
			};
		}
	}
}
