using System.Collections.Generic;

namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class GemTableShapeSize
{
	public long GemTableShapeSizeId { get; set; }

	public long GemTableTypeId { get; set; }

	public long GemTableShapeId { get; set; }

	public decimal Carat { get; set; }

	public decimal Width { get; set; }

	public decimal Height { get; set; }

	public decimal Depth { get; set; }

	public virtual ICollection<DesignGemTable> DesignGemTables { get; set; }

	public GemTableShapeSize()
	{
		DesignGemTables = new HashSet<DesignGemTable>();
	}
}
