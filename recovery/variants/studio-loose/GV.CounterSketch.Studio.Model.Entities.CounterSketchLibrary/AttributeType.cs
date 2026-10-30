using System.Collections.Generic;

namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class AttributeType
{
	public long AttributeTypeId { get; set; }

	public string AttributeType1 { get; set; }

	public string AttributeCSName { get; set; }

	public long? AttributeCSOrder { get; set; }

	public long? AttributeCSDisplayType { get; set; }

	public long? AttributeLevelId { get; set; }

	public virtual ICollection<Attribute> Attributes { get; set; }

	public virtual AttributeLevel AttributeLevel { get; set; }

	public AttributeType()
	{
		Attributes = new HashSet<Attribute>();
	}
}
