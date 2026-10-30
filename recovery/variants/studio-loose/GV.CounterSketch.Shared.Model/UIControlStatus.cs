using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Model;

[DataContract]
public enum UIControlStatus
{
	[EnumMember]
	New,
	[EnumMember]
	Normal,
	[EnumMember]
	Changed,
	[EnumMember]
	Deleted
}
