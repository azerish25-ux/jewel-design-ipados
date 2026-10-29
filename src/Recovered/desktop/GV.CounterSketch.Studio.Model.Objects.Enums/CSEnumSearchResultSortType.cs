using System.ComponentModel;

namespace GV.CounterSketch.Studio.Model.Objects.Enums;

public enum CSEnumSearchResultSortType
{
	[Description("Recently Updated")]
	LastUpdateDateDesc,
	[Description("Newest in Library")]
	CreationDateDesc,
	[Description("Oldest in Library")]
	CreationDate,
	[Description("Updated Last")]
	LastUpdateDate,
	[Description("SCS Number")]
	SCSNumber,
	[Description("SCS Number (reverse)")]
	SCSNumberDesc,
	[Description("Least Flexible")]
	Flexibility,
	[Description("Most Flexible")]
	FlexibilityDesc,
	[Description("Collection")]
	Attribute,
	[Description("Collection (reverse)")]
	AttributeDesc
}
