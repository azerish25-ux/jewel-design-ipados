using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Model;

[DataContract]
public enum UIControlStackLocation
{
	[EnumMember]
	Normal,
	[EnumMember]
	Advanced,
	[EnumMember]
	DesignOption
}
