namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class DesignAttribute
{
	public long DesignAttributeId { get; set; }

	public long DesignId { get; set; }

	public long? ComponentId { get; set; }

	public long? AttributeId { get; set; }

	public string AttributeValue { get; set; }

	public virtual Attribute Attribute { get; set; }

	public virtual Component Component { get; set; }

	public virtual Design Design { get; set; }
}
