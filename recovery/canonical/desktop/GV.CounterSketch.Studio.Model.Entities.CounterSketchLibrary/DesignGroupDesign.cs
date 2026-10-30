namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class DesignGroupDesign
{
	public long DesignGroupDesignId { get; set; }

	public long? DesignGroupId { get; set; }

	public long? DesignId { get; set; }

	public long? Rank { get; set; }

	public virtual DesignGroup DesignGroup { get; set; }

	public virtual Design Design { get; set; }
}
