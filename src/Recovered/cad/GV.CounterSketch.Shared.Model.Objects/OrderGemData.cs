using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Model.Objects;

[DataContract]
public sealed class OrderGemData
{
	[DataMember]
	public string GemDescription { get; set; }

	[DataMember]
	public double GemHeight { get; set; }

	[DataMember]
	public double GemLength { get; set; }

	[DataMember]
	public string GemList { get; set; }

	[DataMember]
	public int GemPlacement { get; set; }

	[DataMember]
	public string GemShape { get; set; }

	[DataMember]
	public double GemWidth { get; set; }

	[DataMember]
	public int Id { get; set; }
}
