using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.CAD;

[DataContract]
public class BoundingBox
{
	private Point3D _min;

	private Point3D _max;

	[DataMember]
	public Point3D Min
	{
		get
		{
			return _min ?? (_min = new Point3D());
		}
		set
		{
			_min = value;
		}
	}

	[DataMember]
	public Point3D Max
	{
		get
		{
			return _max ?? (_max = new Point3D());
		}
		set
		{
			_max = value;
		}
	}
}
