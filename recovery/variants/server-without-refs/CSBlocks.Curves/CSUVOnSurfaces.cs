using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("UVOnSurfaces", "{9C07D889-B3B6-480e-A48D-C126490CDA7F}")]
public sealed class CSUVOnSurfaces : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iSurfaces;

	public readonly CSInputChannel iU;

	public readonly CSInputChannel iV;

	public readonly CSInputChannel iAngle;

	public readonly CSOutputChannel oCurve;

	public CSUVOnSurfaces(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iSurfaces = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iU = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iV = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Expected O, but got Unknown
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		CSDataArray flattenArray2 = iSurfaces.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface");
		}
		List<NurbsSurface> list = new List<NurbsSurface>();
		NurbsSurface val = flattenArray.GetArrayItem(0).GetNurbsSurface();
		if (val == null)
		{
			CSDataHelper.GetBrep(flattenArray.GetArrayItem(0), out var value);
			if (value != null)
			{
				if (value.Faces.Count > 0)
				{
					val = ((Surface)value.Faces[0]).ToNurbsSurface();
				}
				for (int i = 1; i < value.Faces.Count; i++)
				{
					list.Add(((Surface)value.Faces[i]).ToNurbsSurface());
				}
			}
		}
		if (val == null)
		{
			return UpdateMissing("Surface");
		}
		double value2 = 0.5;
		iU.GetDouble(ref value2);
		double value3 = 0.5;
		iV.GetDouble(ref value3);
		double value4 = 0.0;
		iAngle.GetDouble(ref value4);
		for (int j = 0; j < flattenArray2.GetArrayCount(); j++)
		{
			NurbsSurface nurbsSurface = flattenArray2.GetArrayItem(j).GetNurbsSurface();
			if (nurbsSurface != null)
			{
				list.Add(nurbsSurface);
				continue;
			}
			CSDataHelper.GetBrep(flattenArray2.GetArrayItem(j), out var value5);
			if (value5 != null)
			{
				for (int k = 0; k < value5.Faces.Count; k++)
				{
					list.Add(((Surface)value5.Faces[k]).ToNurbsSurface());
				}
			}
		}
		Interval val2 = ((Surface)val).Domain(0);
		Interval val3 = ((Surface)val).Domain(1);
		Point3d val4 = new Point3d(((Interval)(ref val2)).ParameterAt(value2), ((Interval)(ref val3)).ParameterAt(value3), 0.0);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(0.0, 1.0, 0.0);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value4), Vector3d.ZAxis, Point3d.Origin);
		((Vector3d)(ref val5)).Transform(val6);
		Point3d val7 = val4 - val5 * (((Interval)(ref val2)).Length + ((Interval)(ref val3)).Length);
		Point3d val8 = val4 + val5 * (((Interval)(ref val2)).Length + ((Interval)(ref val3)).Length);
		Curve curve2D = (Curve)new LineCurve(val7, val8);
		curve2D = CSUtilGeometry.Trim2dLineCurveToSurface(curve2D, (Surface)(object)val);
		Curve val9 = ((Surface)val).Pushup(curve2D, 0.01);
		if (val9 == null)
		{
			return UpdateError("Error Pushup");
		}
		Curve val10 = val9;
		List<Curve> list2 = new List<Curve>();
		list2.Add(val10);
		Point3d val14 = default(Point3d);
		Vector3d val15 = default(Vector3d);
		while (list.Count > 0)
		{
			Point3d pointAtEnd = val10.PointAtEnd;
			double num = 0.0;
			double num2 = 0.0;
			int num3 = -1;
			double num4 = double.MaxValue;
			for (int l = 0; l < list.Count; l++)
			{
				if (((Surface)list[l]).ClosestPoint(pointAtEnd, ref num, ref num2))
				{
					Point3d val11 = ((Surface)list[l]).PointAt(num, num2);
					if (((Point3d)(ref val11)).DistanceTo(pointAtEnd) < num4)
					{
						num3 = l;
					}
				}
			}
			if (num3 < 0)
			{
				break;
			}
			Surface val12 = (Surface)(object)list[num3];
			list.RemoveAt(num3);
			Curve obj = val10;
			Interval domain = val10.Domain;
			Vector3d val13 = obj.TangentAt(((Interval)(ref domain)).Max);
			val12.ClosestPoint(pointAtEnd, ref num, ref num2);
			((Point3d)(ref val14))._002Ector(num, num2, 0.0);
			val12.ClosestPoint(pointAtEnd + val13, ref num, ref num2);
			((Vector3d)(ref val15))._002Ector(num - ((Point3d)(ref val14)).X, num2 - ((Point3d)(ref val14)).Y, 0.0);
			((Vector3d)(ref val15)).Unitize();
			Interval val16 = val12.Domain(0);
			Interval val17 = val12.Domain(1);
			Curve curve2D2 = (Curve)new LineCurve(val14, val14 + val15 * (((Interval)(ref val16)).Length + ((Interval)(ref val17)).Length));
			curve2D2 = CSUtilGeometry.Trim2dLineCurveToSurface(curve2D2, val12);
			Curve val18 = val12.Pushup(curve2D2, 0.01);
			if (val18 == null)
			{
				break;
			}
			list2.Add(val18);
			val10 = val18;
		}
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)list2.ToArray());
		if (array == null)
		{
			return UpdateError("Failed Merge");
		}
		if (array.Length == 0)
		{
			return UpdateError("Failed Merge");
		}
		NurbsCurve geometry = array[0].ToNurbsCurve();
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		return base.Update();
	}
}
