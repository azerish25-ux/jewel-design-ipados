using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondMarquiseRotated90 : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem02;

	public GemTableShape Shape => GemTableShape.MarquiseRotated90;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.00164337743490066;

	public double RotationAngle => 90.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSize(1.25, 2.5, 0.7),
			new GemSize(1.5, 3.0, 0.84),
			new GemSize(1.75, 3.5, 0.98),
			new GemSize(2.0, 4.0, 1.12, iteration: true),
			new GemSize(2.25, 4.25, 1.26, iteration: true),
			new GemSize(2.5, 4.5, 1.4, iteration: true),
			new GemSize(2.5, 5.0, 1.4, iteration: true),
			new GemSize(3.0, 5.0, 1.68, iteration: true),
			new GemSize(3.0, 6.0, 1.68, iteration: true),
			new GemSize(3.5, 6.0, 1.96, iteration: true),
			new GemSize(3.5, 7.0, 1.96, iteration: true),
			new GemSize(4.0, 8.0, 2.24, iteration: true),
			new GemSize(4.5, 9.0, 2.52, iteration: true),
			new GemSize(5.0, 10.0, 2.8, iteration: true),
			new GemSize(5.25, 10.5, 2.94),
			new GemSize(5.5, 11.0, 3.08),
			new GemSize(6.0, 12.0, 3.36, iteration: true),
			new GemSize(6.5, 13.0, 3.64),
			new GemSize(7.0, 14.0, 3.92),
			new GemSize(7.5, 14.0, 4.2),
			new GemSize(8.0, 16.0, 4.48),
			new GemSize(8.5, 16.0, 4.76)
		};
	}
}
