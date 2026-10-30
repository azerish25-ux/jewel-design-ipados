using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.CAD;

[DataContract]
public sealed class CurvePoint
{
	[DataMember]
	public bool ClampXWhenMirror { get; set; }

	[DataMember]
	public bool IsControlPoint { get; set; }

	[DataMember]
	public int MirrorPoint { get; set; }

	[DataMember]
	public int NcIndex { get; set; }

	[DataMember]
	public int NcIndex2 { get; set; }

	[DataMember]
	public Point2D P2 { get; set; }

	[DataMember]
	public Point4D P4 { get; set; }

	[DataMember]
	public bool Selected { get; set; }

	public CurvePoint()
	{
		P2 = new Point2D();
		P4 = new Point4D();
		Selected = false;
		NcIndex = -1;
		NcIndex2 = -1;
		IsControlPoint = false;
		ClampXWhenMirror = false;
		MirrorPoint = -1;
	}
}
