using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Core;

[CSBlock("LineFromTrimBrep", "{724CD432-1166-4730-B450-710ED91B6D78}")]
public sealed class CSLineFromTrimBrep : CSBlock
{
	public readonly CSInputChannel iBrep;

	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iParam1;

	public readonly CSInputChannel iParam2;

	public readonly CSInputChannel iTrim;

	public readonly CSOutputChannel oCurve;

	public readonly CSOutputChannel oTick1;

	public readonly CSOutputChannel oTick2;

	public CSLineFromTrimBrep(string identifier)
		: base(identifier)
	{
		iBrep = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iParam1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iParam2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTrim = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oTick1 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oTick2 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iBrep.GetFlattenArray();
		Brep value = null;
		CSDataHelper.GetBrep(flattenArray.GetArrayItem(0), out value);
		if (value == null)
		{
			return UpdateMissing("No Brep Surface Input");
		}
		NurbsCurve value2 = null;
		CSDataHelper.GetNurbsCurve(iCurve1.Data, out value2);
		if (value2 == null)
		{
			return UpdateError("Error geting curve");
		}
		NurbsCurve value3 = null;
		CSDataHelper.GetNurbsCurve(iCurve2.Data, out value3);
		if (value3 == null)
		{
			return UpdateMissing("No Curve Input");
		}
		bool value4 = false;
		iTrim.GetBoolean(ref value4);
		double value5 = 0.0;
		iParam1.GetDouble(ref value5);
		double value6 = 0.0;
		iParam2.GetDouble(ref value6);
		double num = 0.0;
		((Curve)value2).NormalizedLengthParameter(value5, ref num);
		double num2 = 0.0;
		((Curve)value3).NormalizedLengthParameter(value6, ref num2);
		Point3d val = ((Curve)value2).PointAt(num);
		Point3d val2 = ((Curve)value3).PointAt(num2);
		if (!CSUtilGeometry.GetClosestPointToBrep(value, val, out var faceIndex, out var u, out var v))
		{
			return UpdateError("Closest Point 1");
		}
		if (!CSUtilGeometry.GetClosestPointToBrep(value, val2, out var faceIndex2, out var u2, out var v2))
		{
			return UpdateError("Closest Point 2");
		}
		if (faceIndex != faceIndex2 && faceIndex2 != -1 && faceIndex != -1)
		{
			BrepFace val3 = value.Faces[faceIndex];
			BrepFace val4 = value.Faces[faceIndex2];
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			double num6 = 0.0;
			((Surface)val4).ClosestPoint(val, ref num3, ref num4);
			((Surface)val3).ClosestPoint(val2, ref num5, ref num6);
			Point3d val5 = ((Surface)val4).PointAt(num3, num4);
			double num7 = ((Point3d)(ref val5)).DistanceTo(((Surface)val3).PointAt(u, v));
			val5 = ((Surface)val3).PointAt(num5, num6);
			if (num7 < ((Point3d)(ref val5)).DistanceTo(((Surface)val4).PointAt(u2, v2)))
			{
				faceIndex = faceIndex2;
				u = num3;
				v = num4;
			}
			else
			{
				faceIndex2 = faceIndex;
				u2 = num5;
				v2 = num6;
			}
		}
		if (faceIndex >= 0)
		{
			BrepFace obj = value.Faces[faceIndex];
			Point3d val6 = ((Surface)obj).PointAt(u, v);
			Vector3d val7 = ((Surface)obj).NormalAt(u, v);
			if (value.Faces[faceIndex].OrientationIsReversed)
			{
				((Vector3d)(ref val7)).Reverse();
			}
			LineCurve geometry = new LineCurve(val6, val6 + val7);
			oTick1.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		}
		if (faceIndex2 >= 0)
		{
			BrepFace obj2 = value.Faces[faceIndex2];
			Point3d val8 = ((Surface)obj2).PointAt(u2, v2);
			Vector3d val9 = ((Surface)obj2).NormalAt(u2, v2);
			if (value.Faces[faceIndex2].OrientationIsReversed)
			{
				((Vector3d)(ref val9)).Reverse();
			}
			LineCurve geometry2 = new LineCurve(val8, val8 + val9);
			oTick2.SetData(new CSOnGeometryData((GeometryBase)(object)geometry2));
		}
		if (faceIndex == faceIndex2 && faceIndex >= 0)
		{
			BrepFace val10 = value.Faces[faceIndex];
			Curve val11 = (Curve)new LineCurve(new Point3d(u, v, 0.0), new Point3d(u2, v2, 0.0));
			if (value4)
			{
				List<NurbsCurve> list = new List<NurbsCurve>();
				for (int i = 0; i < val10.Loops.Count; i++)
				{
					BrepLoop val12 = val10.Loops[i];
					for (int j = 0; j < val12.Trims.Count; j++)
					{
						list.Add(((Curve)val12.Trims[j]).ToNurbsCurve());
					}
				}
				List<double> list2 = new List<double>();
				foreach (NurbsCurve item in list)
				{
					CurveIntersections val13 = Intersection.CurveCurve(val11, (Curve)(object)item, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					for (int k = 0; k < val13.Count; k++)
					{
						list2.Add(val13[k].ParameterA);
					}
				}
				if (list2.Count >= 2)
				{
					val11 = val11.Trim(new Interval(list2[0], list2[list2.Count - 1]));
				}
			}
			Curve geometry3 = ((Surface)val10).Pushup(val11, 0.01);
			oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)geometry3));
			return base.Update();
		}
		return UpdateError("Unable to create line");
	}
}
