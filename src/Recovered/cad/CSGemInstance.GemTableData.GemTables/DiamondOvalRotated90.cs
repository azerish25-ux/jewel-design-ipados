using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondOvalRotated90 : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem01;

	public GemTableShape Shape => GemTableShape.OvalRotated90;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.00175708357743902;

	public double RotationAngle => 90.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSize(2.0, 4.0, 1.22),
			new GemSize(3.0, 4.0, 1.83, iteration: true),
			new GemSize(2.5, 4.5, 1.525),
			new GemSize(3.5, 4.5, 2.135, iteration: true),
			new GemSize(3.0, 5.0, 1.83, iteration: true),
			new GemSize(4.0, 5.0, 2.44, iteration: true),
			new GemSize(3.3, 5.3, 2.013),
			new GemSize(3.8, 5.8, 2.318),
			new GemSize(4.0, 6.0, 2.44, iteration: true),
			new GemSize(4.1, 6.1, 2.501),
			new GemSize(4.2, 6.2, 2.562),
			new GemSize(4.5, 6.5, 2.745),
			new GemSize(4.6, 6.6, 2.806),
			new GemSize(4.9, 6.9, 2.989),
			new GemSize(5.0, 7.0, 3.05, iteration: true),
			new GemSize(5.2, 7.2, 3.172),
			new GemSize(5.5, 7.5, 3.355),
			new GemSize(6.0, 8.0, 3.66, iteration: true),
			new GemSize(6.25, 8.25, 3.8125),
			new GemSize(6.5, 8.5, 3.965),
			new GemSize(7.0, 9.0, 4.27, iteration: true),
			new GemSize(8.0, 10.0, 4.88, iteration: true),
			new GemSize(8.5, 10.0, 5.185),
			new GemSize(9.0, 11.0, 5.49, iteration: true),
			new GemSize(9.5, 11.0, 5.795),
			new GemSize(10.0, 12.0, 6.1, iteration: true),
			new GemSize(10.0, 14.0, 6.1, iteration: true),
			new GemSize(11.0, 15.0, 6.71, iteration: true),
			new GemSize(12.0, 16.0, 7.32, iteration: true)
		};
	}
}
