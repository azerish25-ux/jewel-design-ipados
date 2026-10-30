namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class DesignGemTable
{
	public long DesignGemTableId { get; set; }

	public long DesignId { get; set; }

	public long GemPlacementId { get; set; }

	public long GemTableShapeSizeId { get; set; }

	public virtual GemTableShapeSize GemTableShapeSize { get; set; }

	public virtual Design Design { get; set; }
}
