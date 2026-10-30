using Rhino.Geometry;
using Rhino.Runtime;

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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Vector3d result = Vector3d.CrossProduct(Normal, UnitForwardVector);
			((Vector3d)(ref result)).Unitize();
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
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			Plane val = default(Plane);
			((Plane)(ref val))._002Ector(Center, Normal);
			Vector3d result = ((Plane)(ref val)).ClosestPoint(Center + UnitForwardVector) - Center;
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	public Vector3d UnitForwardVector
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _unitForwardVector;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (((Vector3d)(ref copy._unitForwardVector)).IsValid)
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		base.ApplyTransform(transform);
		if (!((Vector3d)(ref _unitForwardVector)).IsValid)
		{
			return true;
		}
		((Vector3d)(ref _unitForwardVector)).Transform(transform);
		((Vector3d)(ref _unitForwardVector)).Unitize();
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSChannelGemData(this);
	}

	public Transform GetMirrorForwardBack()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		Transform val = Transform.Mirror(Point3d.Origin, Vector3d.XAxis);
		Transform val2 = ChangeBasis(Center, ForwardVector, LeftVector, Normal, Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis);
		Transform identity = default(Transform);
		if (!((Transform)(ref val2)).TryGetInverse(ref identity))
		{
			identity = Transform.Identity;
		}
		return val2 * val * identity;
	}

	protected override void Dispose(bool disposing)
	{
		if (_isDisposed)
		{
			return;
		}
		try
		{
			Curve sourceRail = SourceRail;
			if (sourceRail != null)
			{
				((CommonObject)sourceRail).Dispose();
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
		_isDisposed = true;
	}

	private static Transform ChangeBasis(Point3d initialLocation, Vector3d initialBaseX, Vector3d initialBaseY, Vector3d initialBaseZ, Point3d finalLocation, Vector3d finalBaseX, Vector3d finalBaseY, Vector3d finalBaseZ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Transform val = Transform.Translation(new Vector3d(finalLocation) * -1.0);
		Transform val2 = Transform.ChangeBasis(initialBaseX, initialBaseY, initialBaseZ, finalBaseX, finalBaseY, finalBaseZ);
		return Transform.Translation(new Vector3d(initialLocation)) * val2 * val;
	}

	~CSChannelGemData()
	{
		Dispose(disposing: false);
	}
}
