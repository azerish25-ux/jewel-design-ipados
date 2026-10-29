using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Model.Objects;

[DataContract]
public sealed class GemTableSizeInfo
{
	[DataMember]
	public double Carat { get; set; }

	[DataMember]
	public double Depth { get; set; }

	[DataMember]
	public double Height { get; set; }

	[DataMember]
	public double DepthPercentage { get; set; }

	[DataMember]
	public double Width { get; set; }
}
