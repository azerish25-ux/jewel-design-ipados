namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class DesignGem
{
	public long DesignGemId { get; set; }

	public long DesignId { get; set; }

	public long? ComponentId { get; set; }

	public string Name { get; set; }

	public string Shape { get; set; }

	public double? GemX { get; set; }

	public double? GemXMax { get; set; }

	public double? GemXMin { get; set; }

	public double? GemY { get; set; }

	public double? GemYMax { get; set; }

	public double? GemYMin { get; set; }

	public double? GemZ { get; set; }

	public double? FFactor { get; set; }

	public double? GemW { get; set; }

	public double? GemWMax { get; set; }

	public double? GemWMin { get; set; }

	public string Mounting { get; set; }

	public double? MountingVolume { get; set; }

	public long? Flags { get; set; }

	public long? GemPlacement { get; set; }

	public virtual Component Component { get; set; }

	public virtual Design Design { get; set; }
}
