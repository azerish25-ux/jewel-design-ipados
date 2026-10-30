using Rhino.Geometry;

namespace CSDatas;

public sealed class CSChannelGemData : CSGemData
{
	public readonly Curve SourceRail;

	public double Size;

	public double SizeForward;

	public double VectorLength;

	public ChannelCornerType Corner;

	public double CornerAngle;

	private Vector3d _unitForwardVector;

	public Vector3d UnitLeftVector
	{
		get
		{
			Vector3d result = Vector3d.CrossProduct(Normal, UnitForwardVector);
			result.Unitize();
			return result;
		}
	}

	public Vector3d UnitRightVector => -UnitLeftVector;

	public Vector3d LeftVector => UnitLeftVector * VectorLength;

	public Vector3d RightVector => UnitRightVector * VectorLength;

	public Vector3d ForwardVector => _unitForwardVector * SizeForward;

	public Vector3d ForwardVectorInGemPlane => UnitForwardVectorInGemPlane * SizeForward;

	public Point3d Left => Center + LeftVector;

	public Point3d Right => Center + RightVector;

	public Point3d TableCenterPoint => Center + Normal * base.GemZMax;

	public double SizeAverage => (base.GemXSize / 2.0 + base.GemYSize / 2.0) / 2.0;

	private Vector3d UnitForwardVectorInGemPlane
	{
		get
		{
			Vector3d result = new Plane(Center, Normal).ClosestPoint(Center + UnitForwardVector) - Center;
			result.Unitize();
			return result;
		}
	}

	public Vector3d UnitForwardVector
	{
		get
		{
			return _unitForwardVector;
		}
		set
		{
			_unitForwardVector = new Vector3d(value);
		}
	}

	public CSChannelGemData(CSGemData gemData)
		: base(gemData)
	{
		Size = base.GemYSize / 2.0;
	}

	public CSChannelGemData(CSGemData gemData, Curve sourceRail)
		: base(gemData)
	{
		SourceRail = sourceRail;
		Size = base.GemYSize / 2.0;
	}

	public CSChannelGemData(CSChannelGemData copy)
		: base(copy)
	{
		if (copy._unitForwardVector.IsValid)
		{
			_unitForwardVector = copy._unitForwardVector;
		}
		if (copy.SourceRail != null)
		{
			SourceRail = copy.SourceRail.DuplicateCurve();
		}
		VectorLength = copy.VectorLength;
		Size = copy.Size;
		SizeForward = copy.SizeForward;
	}

	public override bool ApplyTransform(Transform transform)
	{
		base.ApplyTransform(transform);
		if (!_unitForwardVector.IsValid)
		{
			return true;
		}
		_unitForwardVector.Transform(transform);
		_unitForwardVector.Unitize();
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSChannelGemData(this);
	}

	public Transform GetMirrorForwardBack()
	{
		Transform transform = Transform.Mirror(Point3d.Origin, Vector3d.XAxis);
		Transform transform2 = ChangeBasis(Center, ForwardVector, LeftVector, Normal, Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis);
		if (!transform2.TryGetInverse(out var inverseTransform))
		{
			inverseTransform = Transform.Identity;
		}
		return transform2 * transform * inverseTransform;
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			try
			{
				SourceRail?.Dispose();
			}
			finally
			{
				base.Dispose(disposing);
			}
			_isDisposed = true;
		}
	}

	private static Transform ChangeBasis(Point3d initialLocation, Vector3d initialBaseX, Vector3d initialBaseY, Vector3d initialBaseZ, Point3d finalLocation, Vector3d finalBaseX, Vector3d finalBaseY, Vector3d finalBaseZ)
	{
		Transform transform = Transform.Translation(new Vector3d(finalLocation) * -1.0);
		Transform transform2 = Transform.ChangeBasis(initialBaseX, initialBaseY, initialBaseZ, finalBaseX, finalBaseY, finalBaseZ);
		return Transform.Translation(new Vector3d(initialLocation)) * transform2 * transform;
	}

	~CSChannelGemData()
	{
		Dispose(disposing: false);
	}
}
