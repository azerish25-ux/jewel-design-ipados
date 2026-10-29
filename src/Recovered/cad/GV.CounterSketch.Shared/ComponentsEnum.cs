using System.ComponentModel;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared;

[DataContract]
public enum ComponentsEnum
{
	[EnumMember]
	None,
	[EnumMember]
	[Description("Ring")]
	Ring,
	[EnumMember]
	[Description("Band 1")]
	Band1,
	[EnumMember]
	[Description("Band 2")]
	Band2,
	[EnumMember]
	[Description("Pendant")]
	Pendant
}
