using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class DiamondHalfMoon : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem01;

	public GemTableShape Shape => GemTableShape.HalfMoon;

	public GemTableType Type => GemTableType.Diamond;

	public double FudgeFactor => 0.00177;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(1.0, 0.577, 80.0),
			new GemSizePercent(1.1, 0.634, 80.0),
			new GemSizePercent(1.2, 0.692, 80.0),
			new GemSizePercent(1.3, 0.749, 80.0),
			new GemSizePercent(1.4, 0.807, 80.0),
			new GemSizePercent(1.5, 0.865, 80.0),
			new GemSizePercent(1.6, 0.922, 80.0),
			new GemSizePercent(1.7, 0.98, 80.0),
			new GemSizePercent(1.8, 1.038, 80.0),
			new GemSizePercent(1.9, 1.095, 80.0),
			new GemSizePercent(2.0, 1.153, 80.0),
			new GemSizePercent(2.1, 1.211, 80.0),
			new GemSizePercent(2.2, 1.268, 80.0),
			new GemSizePercent(2.4, 1.384, 80.0),
			new GemSizePercent(2.5, 1.441, 80.0),
			new GemSizePercent(2.7, 1.557, 80.0),
			new GemSizePercent(2.8, 1.614, 80.0),
			new GemSizePercent(2.9, 1.672, 80.0),
			new GemSizePercent(3.0, 1.73, 80.0),
			new GemSizePercent(3.1, 1.787, 80.0),
			new GemSizePercent(3.2, 1.845, 80.0),
			new GemSizePercent(3.3, 1.902, 80.0),
			new GemSizePercent(3.4, 1.96, 80.0),
			new GemSizePercent(3.5, 2.018, 80.0),
			new GemSizePercent(3.6, 2.075, 80.0),
			new GemSizePercent(3.7, 2.133, 80.0),
			new GemSizePercent(3.8, 2.191, 80.0),
			new GemSizePercent(3.9, 2.248, 80.0),
			new GemSizePercent(4.0, 2.306, 80.0),
			new GemSizePercent(4.1, 2.364, 80.0),
			new GemSizePercent(4.2, 2.421, 80.0),
			new GemSizePercent(4.4, 2.537, 80.0),
			new GemSizePercent(4.6, 2.652, 80.0),
			new GemSizePercent(4.8, 2.767, 80.0),
			new GemSizePercent(5.0, 2.883, 80.0),
			new GemSizePercent(5.2, 2.998, 80.0),
			new GemSizePercent(5.4, 3.113, 80.0),
			new GemSizePercent(5.6, 3.228, 80.0),
			new GemSizePercent(5.8, 3.344, 80.0),
			new GemSizePercent(6.0, 3.459, 80.0),
			new GemSizePercent(6.4, 3.69, 80.0),
			new GemSizePercent(6.5, 3.747, 80.0),
			new GemSizePercent(6.6, 3.805, 80.0),
			new GemSizePercent(6.8, 3.92, 80.0),
			new GemSizePercent(7.0, 4.036, 80.0),
			new GemSizePercent(7.2, 4.151, 80.0),
			new GemSizePercent(7.4, 4.266, 80.0),
			new GemSizePercent(7.6, 4.381, 80.0),
			new GemSizePercent(7.8, 4.497, 80.0),
			new GemSizePercent(8.0, 4.612, 80.0),
			new GemSizePercent(8.2, 4.727, 80.0),
			new GemSizePercent(8.4, 4.843, 80.0),
			new GemSizePercent(8.6, 4.958, 80.0),
			new GemSizePercent(8.8, 5.073, 80.0),
			new GemSizePercent(9.2, 5.304, 80.0),
			new GemSizePercent(9.4, 5.419, 80.0),
			new GemSizePercent(9.6, 5.534, 80.0),
			new GemSizePercent(9.8, 5.65, 80.0),
			new GemSizePercent(10.0, 5.765, 80.0),
			new GemSizePercent(10.2, 5.88, 80.0),
			new GemSizePercent(10.4, 5.996, 80.0),
			new GemSizePercent(10.6, 6.111, 80.0),
			new GemSizePercent(10.8, 6.226, 80.0),
			new GemSizePercent(11.0, 6.342, 80.0),
			new GemSizePercent(11.2, 6.457, 80.0)
		};
	}
}
