using System;
using System.Collections.Generic;

namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class Design
{
	public long DesignId { get; set; }

	public long Version { get; set; }

	public long DesignVersionId { get; set; }

	public string StullerNo { get; set; }

	public string SeriesNumber { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Designer { get; set; }

	public long? DesignTypeId { get; set; }

	public DateTime? CreationDate { get; set; }

	public DateTime? LastUpdateDate { get; set; }

	public long? FirstReleaseLibraryId { get; set; }

	public long? CSGLFound { get; set; }

	public virtual ICollection<DesignAttribute> DesignAttributes { get; set; }

	public virtual ICollection<DesignComponent> DesignComponents { get; set; }

	public virtual ICollection<DesignGem> DesignGems { get; set; }

	public virtual ICollection<DesignGemTable> DesignGemTables { get; set; }

	public virtual ICollection<DesignGroupDesign> DesignGroupDesigns { get; set; }

	public virtual DesignSearch DesignSearch { get; set; }

	public Design()
	{
		DesignAttributes = new HashSet<DesignAttribute>();
		DesignComponents = new HashSet<DesignComponent>();
		DesignGems = new HashSet<DesignGem>();
		DesignGemTables = new HashSet<DesignGemTable>();
		DesignGroupDesigns = new HashSet<DesignGroupDesign>();
	}
}
