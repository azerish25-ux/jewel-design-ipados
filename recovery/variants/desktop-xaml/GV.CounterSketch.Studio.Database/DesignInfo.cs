using System;
using System.Collections.Generic;

namespace GV.CounterSketch.Studio.Database;

[Serializable]
public class DesignInfo
{
	public int DesignID;

	public int Version;

	public string StullerNo;

	public string SeriesNumber;

	public string Name;

	public string Description;

	public string Designer;

	public int DesignTypeId;

	public DateTime CreationDate;

	public DateTime LastUpdateDate;

	public int FirstReleaseLibraryId;

	public List<DesignGem> DG;

	public List<DesignAttribute> DA;

	public List<DesignComponent> DC;

	public List<DesignGroupDesign> DGD;
}
