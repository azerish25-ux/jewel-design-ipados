using System.Runtime.Serialization;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Model.Objects;

[DataContract]
public class ValueListDisplayItem
{
	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public bool UIDisabled { get; set; }

	[DataMember]
	public DrawingImage Icon { get; set; }

	[DataMember]
	public int Index { get; set; }

	[DataMember]
	public string Name { get; set; }
}
