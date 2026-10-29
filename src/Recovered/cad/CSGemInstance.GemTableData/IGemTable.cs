using System.Collections.Generic;

namespace CSGemInstance.GemTableData;

public interface IGemTable
{
	GemTableLayer Layer { get; }

	int CurrentVersion { get; }

	GemTableShape Shape { get; }

	GemTableType Type { get; }

	double FudgeFactor { get; }

	double RotationAngle { get; }

	List<GemSize> SizeTable(int version = 0);
}
