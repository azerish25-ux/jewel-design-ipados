using System.Collections.Generic;

namespace CSGemInstance.GemTableData.GemTables;

[CSGem]
public sealed class CabochonBullet : IGemTable
{
	public int CurrentVersion { get; } = 1;

	public GemTableLayer Layer => GemTableLayer.Gem01;

	public GemTableShape Shape => GemTableShape.Bullet;

	public GemTableType Type => GemTableType.Cabochon;

	public double FudgeFactor => 0.0022;

	public double RotationAngle => 0.0;

	public List<GemSize> SizeTable(int version = 0)
	{
		return new List<GemSize>
		{
			new GemSizePercent(1.0, 1.0, 50.0),
			new GemSizePercent(1.1, 1.1, 50.0),
			new GemSizePercent(1.2, 1.2, 50.0),
			new GemSizePercent(1.3, 1.3, 50.0),
			new GemSizePercent(1.4, 1.4, 50.0),
			new GemSizePercent(1.5, 1.5, 50.0),
			new GemSizePercent(1.6, 1.6, 50.0),
			new GemSizePercent(1.7, 1.7, 50.0),
			new GemSizePercent(1.8, 1.8, 50.0),
			new GemSizePercent(1.9, 1.9, 50.0),
			new GemSizePercent(2.0, 2.0, 50.0),
			new GemSizePercent(2.1, 2.1, 50.0),
			new GemSizePercent(2.2, 2.2, 50.0),
			new GemSizePercent(2.4, 2.4, 50.0),
			new GemSizePercent(2.5, 2.5, 50.0),
			new GemSizePercent(2.7, 2.7, 50.0),
			new GemSizePercent(2.8, 2.8, 50.0),
			new GemSizePercent(2.9, 2.9, 50.0),
			new GemSizePercent(3.0, 3.0, 50.0),
			new GemSizePercent(3.1, 3.1, 50.0),
			new GemSizePercent(3.2, 3.2, 50.0),
			new GemSizePercent(3.3, 3.3, 50.0),
			new GemSizePercent(3.4, 3.4, 50.0),
			new GemSizePercent(3.5, 3.5, 50.0),
			new GemSizePercent(3.6, 3.6, 50.0),
			new GemSizePercent(3.7, 3.7, 50.0),
			new GemSizePercent(3.8, 3.8, 50.0),
			new GemSizePercent(3.9, 3.9, 50.0),
			new GemSizePercent(4.0, 4.0, 50.0),
			new GemSizePercent(4.1, 4.1, 50.0),
			new GemSizePercent(4.2, 4.2, 50.0),
			new GemSizePercent(4.4, 4.4, 50.0),
			new GemSizePercent(4.6, 4.6, 50.0),
			new GemSizePercent(4.8, 4.8, 50.0),
			new GemSizePercent(5.0, 5.0, 50.0),
			new GemSizePercent(5.2, 5.2, 50.0),
			new GemSizePercent(5.4, 5.4, 50.0),
			new GemSizePercent(5.6, 5.6, 50.0),
			new GemSizePercent(5.8, 5.8, 50.0),
			new GemSizePercent(6.0, 6.0, 50.0),
			new GemSizePercent(6.4, 6.4, 50.0),
			new GemSizePercent(6.5, 6.5, 50.0),
			new GemSizePercent(6.6, 6.6, 50.0),
			new GemSizePercent(6.8, 6.8, 50.0),
			new GemSizePercent(7.0, 7.0, 50.0),
			new GemSizePercent(7.2, 7.2, 50.0),
			new GemSizePercent(7.4, 7.4, 50.0),
			new GemSizePercent(7.6, 7.6, 50.0),
			new GemSizePercent(7.8, 7.8, 50.0),
			new GemSizePercent(8.0, 8.0, 50.0),
			new GemSizePercent(8.2, 8.2, 50.0),
			new GemSizePercent(8.4, 8.4, 50.0),
			new GemSizePercent(8.6, 8.6, 50.0),
			new GemSizePercent(8.8, 8.8, 50.0),
			new GemSizePercent(9.2, 9.2, 50.0),
			new GemSizePercent(9.4, 9.4, 50.0),
			new GemSizePercent(9.6, 9.6, 50.0),
			new GemSizePercent(9.8, 9.8, 50.0),
			new GemSizePercent(10.0, 10.0, 50.0),
			new GemSizePercent(10.2, 10.2, 50.0),
			new GemSizePercent(10.4, 10.4, 50.0),
			new GemSizePercent(10.6, 10.6, 50.0),
			new GemSizePercent(10.8, 10.8, 50.0),
			new GemSizePercent(11.0, 11.0, 50.0),
			new GemSizePercent(11.2, 11.2, 50.0)
		};
	}
}
