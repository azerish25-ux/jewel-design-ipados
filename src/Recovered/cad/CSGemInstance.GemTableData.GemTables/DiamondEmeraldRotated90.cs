using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondEmeraldRotated90 : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem02;

	public GemTableShape Shape => GemTableShape.EmeraldRotated90;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.00254381286646363;

	public double RotationAngle => 90.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSize(1.5, 3.0, 0.975),
			new GemSize(1.5, 3.5, 0.975),
			new GemSize(1.75, 3.75, 1.1375),
			new GemSize(2.0, 4.0, 1.3),
			new GemSize(3.0, 5.0, 1.95, iteration: true),
			new GemSize(3.3, 5.3, 2.145),
			new GemSize(3.8, 5.8, 2.47),
			new GemSize(4.0, 6.0, 2.6, iteration: true),
			new GemSize(4.5, 6.5, 2.925),
			new GemSize(5.0, 7.0, 3.25, iteration: true),
			new GemSize(5.5, 7.5, 3.575),
			new GemSize(6.0, 8.0, 3.9, iteration: true),
			new GemSize(6.5, 8.5, 4.225),
			new GemSize(7.0, 9.0, 4.55, iteration: true),
			new GemSize(8.0, 10.0, 5.2, iteration: true),
			new GemSize(9.0, 11.0, 5.85, iteration: true),
			new GemSize(10.0, 12.0, 6.5, iteration: true),
			new GemSize(10.0, 14.0, 6.5, iteration: true),
			new GemSize(11.0, 15.0, 7.15, iteration: true),
			new GemSize(12.0, 16.0, 7.8, iteration: true)
		};
	}
}
