using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Runtime;

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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = InProfile.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateError("Profile Invalid");
		}
		Transform val = Transform.Mirror(Point3d.Origin, Vector3d.XAxis);
		double num = 0.0;
		if (!((Curve)nurbsCurve).NormalizedLengthParameter(0.5, ref num))
		{
			return UpdateError("Error finding middle of curve");
		}
		Interval domain = ((Curve)nurbsCurve).Domain;
		Curve val2 = ((Curve)nurbsCurve).Trim(new Interval(((Interval)(ref domain)).Min, num));
		if (!((CommonObject)val2).IsValid)
		{
			return UpdateError("Error timming curve");
		}
		LineCurve val3 = new LineCurve(val2.PointAtEnd, val2.PointAtStart);
		Curve[] obj = new Curve[2]
		{
			val2,
			(Curve)val3
		};
		Curve[] array = null;
		array = Curve.JoinCurves((IEnumerable<Curve>)(object)obj);
		if (array == null)
		{
			return UpdateError("Error merging curves");
		}
		if (array.Length != 1)
		{
			return UpdateError("Error merging curves");
		}
		NurbsCurve val4 = array[0].ToNurbsCurve();
		BoundingBox boundingBox = ((GeometryBase)val4).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			return UpdateError("Error getting boundingbox");
		}
		double num2 = 0.0;
		Point3d min = ((BoundingBox)(ref boundingBox)).Min;
		double num3 = ((Point3d)(ref min)).X / 2.0;
		min = ((BoundingBox)(ref boundingBox)).Min;
		if (!((Curve)val4).ClosestPoint(new Point3d(num3, ((Point3d)(ref min)).Y, 0.0), ref num2))
		{
			return UpdateError("Error getting closest point");
		}
		if (!((Curve)val4).ChangeClosedCurveSeam(num2))
		{
			return UpdateError("Error changing seam point");
		}
		((Curve)val4).Domain = ((Curve)nurbsCurve).Domain;
		if (((Curve)val4).Domain != ((Curve)nurbsCurve).Domain)
		{
			return UpdateError("Error Setting domain");
		}
		NurbsCurve val5 = new NurbsCurve(val4);
		((GeometryBase)val5).Transform(val);
		((Curve)val5).Reverse();
		OutHalfProfileLhs.SetData(new CSOnGeometryData((GeometryBase)(object)val4));
		OutHalfProfileRhs.SetData(new CSOnGeometryData((GeometryBase)(object)val5, val, applyTransform: false));
		return base.Update();
	}
}
