namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class DesignComponent
{
	public long DesignComponentId { get; set; }

	public long DesignId { get; set; }

	public long ComponentId { get; set; }

	public virtual Component Component { get; set; }

	public virtual Design Design { get; set; }
}
