using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Bails;

[CSBlock("HalfProfile", "{39BB238D-8560-4756-85E0-4538E99A3A07}")]
public sealed class CSHalfProfile : CSBlock
{
	public readonly CSInputChannel InProfile;

	public readonly CSOutputChannel OutHalfProfileLhs;

	public readonly CSOutputChannel OutHalfProfileRhs;

	public CSHalfProfile(string identifier)
		: base(identifier)
	{
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutHalfProfileLhs = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutHalfProfileRhs = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InProfile.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateError("Profile Invalid");
		}
		Transform transform = Transform.Mirror(Point3d.Origin, Vector3d.XAxis);
		double t = 0.0;
		if (!nurbsCurve.NormalizedLengthParameter(0.5, out t))
		{
			return UpdateError("Error finding middle of curve");
		}
		Curve curve = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Min, t));
		if (!curve.IsValid)
		{
			return UpdateError("Error timming curve");
		}
		LineCurve lineCurve = new LineCurve(curve.PointAtEnd, curve.PointAtStart);
		Curve[] inputCurves = new Curve[2] { curve, lineCurve };
		Curve[] array = null;
		array = Curve.JoinCurves(inputCurves);
		if (array == null)
		{
			return UpdateError("Error merging curves");
		}
		if (array.Length != 1)
		{
			return UpdateError("Error merging curves");
		}
		NurbsCurve nurbsCurve2 = array[0].ToNurbsCurve();
		BoundingBox boundingBox = nurbsCurve2.GetBoundingBox(accurate: true);
		if (!boundingBox.IsValid)
		{
			return UpdateError("Error getting boundingbox");
		}
		double t2 = 0.0;
		if (!nurbsCurve2.ClosestPoint(new Point3d(boundingBox.Min.X / 2.0, boundingBox.Min.Y, 0.0), out t2))
		{
			return UpdateError("Error getting closest point");
		}
		if (!nurbsCurve2.ChangeClosedCurveSeam(t2))
		{
			return UpdateError("Error changing seam point");
		}
		nurbsCurve2.Domain = nurbsCurve.Domain;
		if (nurbsCurve2.Domain != nurbsCurve.Domain)
		{
			return UpdateError("Error Setting domain");
		}
		NurbsCurve nurbsCurve3 = new NurbsCurve(nurbsCurve2);
		nurbsCurve3.Transform(transform);
		nurbsCurve3.Reverse();
		OutHalfProfileLhs.SetData(new CSOnGeometryData(nurbsCurve2));
		OutHalfProfileRhs.SetData(new CSOnGeometryData(nurbsCurve3, transform, applyTransform: false));
		return base.Update();
	}
}
