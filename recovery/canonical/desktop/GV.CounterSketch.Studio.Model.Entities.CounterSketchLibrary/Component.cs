using System.Collections.Generic;

namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class Component
{
	public long ComponentId { get; set; }

	public string Name { get; set; }

	public virtual ICollection<DesignAttribute> DesignAttributes { get; set; }

	public virtual ICollection<DesignComponent> DesignComponents { get; set; }

	public virtual ICollection<DesignGem> DesignGems { get; set; }

	public Component()
	{
		DesignAttributes = new HashSet<DesignAttribute>();
		DesignComponents = new HashSet<DesignComponent>();
		DesignGems = new HashSet<DesignGem>();
	}
}
