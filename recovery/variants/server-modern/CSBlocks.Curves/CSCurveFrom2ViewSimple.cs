using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Curves;

[CSBlock("CurveFrom2ViewSimple", "{A33A708A-E462-4333-BC3D-C7EA9AB2F890}")]
public sealed class CSCurveFrom2ViewSimple : CSBlock
{
	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSOutputChannel oCurve;

	public CSCurveFrom2ViewSimple(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iCurve1.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve from two views curves 1");
		}
		CSDataArray flattenArray2 = iCurve2.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve from two views curves 2");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Curve1");
		}
		CSDataHelper.GetNurbsCurve(flattenArray2.GetArrayItem(0), out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Missing Curve2");
		}
		Plane val = default(Plane);
		if (!((Curve)value).TryGetPlane(ref val))
		{
			return UpdateMissing("Curve 1 is not planar");
		}
		Plane val2 = default(Plane);
		if (!((Curve)value2).TryGetPlane(ref val2))
		{
			return UpdateMissing("Curve 2 is not planar");
		}
		BoundingBox boundingBox = ((GeometryBase)value).GetBoundingBox(false);
		BoundingBox boundingBox2 = ((GeometryBase)value2).GetBoundingBox(false);
		Point3d val3 = ((BoundingBox)(ref boundingBox)).FurthestPoint(((BoundingBox)(ref boundingBox2)).Center);
		Point3d val4 = ((BoundingBox)(ref boundingBox2)).FurthestPoint(val3);
		double num = ((Point3d)(ref val3)).DistanceTo(val4);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(((Plane)(ref val)).Normal);
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(((Plane)(ref val2)).Normal);
		NurbsCurve val7 = ((Curve)value).ToNurbsCurve();
		NurbsCurve val8 = ((Curve)value).ToNurbsCurve();
		NurbsCurve val9 = ((Curve)value2).ToNurbsCurve();
		NurbsCurve val10 = ((Curve)value2).ToNurbsCurve();
		((GeometryBase)val7).Translate(val5 * num);
		((GeometryBase)val9).Translate(val6 * num);
		((GeometryBase)val8).Translate(val5 * (0.0 - num));
		((GeometryBase)val10).Translate(val6 * (0.0 - num));
		NurbsSurface obj = NurbsSurface.CreateRuledSurface((Curve)(object)val7, (Curve)(object)val8);
		NurbsSurface val11 = NurbsSurface.CreateRuledSurface((Curve)(object)val9, (Curve)(object)val10);
		Curve[] array = default(Curve[]);
		Point3d[] array2 = default(Point3d[]);
		Intersection.SurfaceSurface((Surface)(object)obj, (Surface)(object)val11, CSApplicationHelper.Application.Tolerance, ref array, ref array2);
		Curve[] array3 = Curve.JoinCurves((IEnumerable<Curve>)array, CSApplicationHelper.Application.Tolerance);
		if (array3 == null)
		{
			return UpdateError("Error with merge");
		}
		base.OutputChannels.Clear();
		for (int i = 0; i < array3.Length; i++)
		{
			Curve val12 = array3[i];
			if (val12 != null)
			{
				NurbsCurve val13 = val12.ToNurbsCurve();
				if (val13 != null)
				{
					new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.Curve, ChannelArrayType.Individual).SetData(new CSOnGeometryData((GeometryBase)(object)val13));
				}
			}
		}
		return base.Update();
	}
}
