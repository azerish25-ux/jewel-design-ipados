using System.ComponentModel;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared;

[DataContract]
public enum Font
{
	[EnumMember]
	[Description("Unknown")]
	Unknown,
	[EnumMember]
	[Description("Arial")]
	Arial,
	[EnumMember]
	[Description("Arial Black")]
	ArialBlack,
	[EnumMember]
	[Description("Arial Narrow")]
	ArialNarrow,
	[EnumMember]
	[Description("Arial Rounded MT Bold")]
	ArialRoundedMTBold,
	[EnumMember]
	[Description("Bauhaus 93")]
	Bauhaus93,
	[EnumMember]
	[Description("Bookman Old Style")]
	BookmanOldStyle,
	[EnumMember]
	[Description("Britannic Bold")]
	BritannicBold,
	[EnumMember]
	[Description("Century Schoolbook")]
	CenturySchoolbook,
	[EnumMember]
	[Description("Copperplate Gothic Bold")]
	CopperplateGothicBold,
	[EnumMember]
	[Description("Franklin Gothic Medium")]
	FranklinGothicMedium,
	[EnumMember]
	[Description("Franklin Gothic Medium Cond")]
	FranklinGothicMediumCond,
	[EnumMember]
	[Description("Lithograph")]
	Lithograph,
	[EnumMember]
	[Description("Lucida Handwriting")]
	LucidaHandwriting,
	[EnumMember]
	[Description("Machine BT")]
	MachineBT,
	[EnumMember]
	[Description("Revue BT")]
	RevueBT
}
