using System.Collections.Generic;

namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class AttributeLevel
{
	public long AttributeLevelId { get; set; }

	public string Name { get; set; }

	public virtual ICollection<AttributeType> AttributeTypes { get; set; }

	public AttributeLevel()
	{
		AttributeTypes = new HashSet<AttributeType>();
	}
}
