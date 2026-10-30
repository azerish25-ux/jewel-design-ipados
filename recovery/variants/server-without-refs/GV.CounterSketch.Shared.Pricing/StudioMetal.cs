using System.Runtime.Serialization;
using GV.CounterSketch.Shared.Pricing.Enums;

namespace GV.CounterSketch.Shared.Pricing;

[DataContract]
public sealed class StudioMetal
{
	[DataMember]
	public string ItemNumber;

	[DataMember(IsRequired = true)]
	public StudioMetalQuality MetalQualityOption;

	[DataMember(IsRequired = true)]
	public decimal Volume;

	[DataMember]
	public decimal Price;
}
