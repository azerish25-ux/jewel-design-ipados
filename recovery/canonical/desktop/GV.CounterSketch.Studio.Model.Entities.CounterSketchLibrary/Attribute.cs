using System.Collections.Generic;

namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class Attribute
{
	public long AttributeId { get; set; }

	public long AttributeTypeId { get; set; }

	public string AttributeName { get; set; }

	public long AttributeCSOrder { get; set; }

	public virtual AttributeType AttributeType { get; set; }

	public virtual ICollection<DesignAttribute> DesignAttributes { get; set; }

	public Attribute()
	{
		DesignAttributes = new HashSet<DesignAttribute>();
	}
}
