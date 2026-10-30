using System.Collections.Generic;

namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class DesignGroup
{
	public long DesignGroupId { get; set; }

	public string Name { get; set; }

	public long? Rank { get; set; }

	public virtual ICollection<DesignGroupDesign> DesignGroupDesigns { get; set; }

	public DesignGroup()
	{
		DesignGroupDesigns = new HashSet<DesignGroupDesign>();
	}
}
