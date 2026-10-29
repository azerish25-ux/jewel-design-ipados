using System.ComponentModel;

namespace GV.CounterSketch.Studio.Model.Objects.Enums;

public enum EnumSearchResultSortType
{
	[Description("Sort By:  Recently Updated")]
	LastUpdateDate,
	[Description("Sort By:  Creation Date")]
	CreationDate,
	[Description("Sort By:  SCS Number")]
	SCSNumber,
	[Description("Sort By:  Flexibility")]
	Flexibility,
	[Description("Sort By:  Collection")]
	Attribute
}
