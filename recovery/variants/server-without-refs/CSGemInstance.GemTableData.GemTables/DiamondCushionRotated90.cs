using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondCushionRotated90 : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem01;

	public GemTableShape Shape => GemTableShape.CushionRotated90;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.002955;

	public double RotationAngle => 90.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSize(2.0, 4.0, 1.2),
			new GemSize(2.5, 4.5, 1.5),
			new GemSize(3.0, 5.0, 1.8),
			new GemSize(3.3, 5.3, 1.98),
			new GemSize(3.8, 5.8, 2.28),
			new GemSize(4.0, 6.0, 2.4),
			new GemSize(4.1, 6.1, 2.46),
			new GemSize(4.2, 6.2, 2.52),
			new GemSize(4.5, 6.5, 2.7),
			new GemSize(4.6, 6.6, 2.76),
			new GemSize(4.9, 6.9, 2.94),
			new GemSize(5.0, 7.0, 3.0),
			new GemSize(5.2, 7.2, 3.12),
			new GemSize(5.5, 7.5, 3.0),
			new GemSize(6.0, 8.0, 3.6, iteration: true),
			new GemSize(6.25, 8.25, 3.75),
			new GemSize(6.5, 8.5, 3.9),
			new GemSize(7.0, 9.0, 4.2, iteration: true),
			new GemSize(8.0, 10.0, 4.8, iteration: true),
			new GemSize(8.5, 10.0, 5.1),
			new GemSize(9.0, 11.0, 5.4),
			new GemSize(9.5, 11.0, 5.7),
			new GemSize(10.0, 12.0, 6.0, iteration: true),
			new GemSize(10.0, 14.0, 6.0, iteration: true),
			new GemSize(10.0, 25.0, 6.0, iteration: true),
			new GemSize(10.0, 30.0, 6.0, iteration: true)
		};
	}
}
