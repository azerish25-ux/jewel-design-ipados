using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlocks.Collars;

[CSBlock("SweepCollarRail", "{0EB40227-05EF-4DE5-98FB-33EF628A72D4}")]
public sealed class CSSweepCollarRail : CSBlock
{
	public CSInputChannel InProfile { get; }

	public CSInputChannel InBottomTrim { get; }

	public CSOutputChannel OutRail { get; }

	public CSSweepCollarRail(string identifier)
		: base(identifier)
	{
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InBottomTrim = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRail = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InProfile.GetFlattenArray();
		double num = InBottomTrim.Double;
		CSDataArray cSDataArray = new CSDataArray();
		Line val11 = default(Line);
		Line val12 = default(Line);
		double num6 = default(double);
		double num7 = default(double);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				return UpdateError("Rail Data Missing");
			}
			Transform value;
			if (arrayItem is ICSTransform iCSTransform)
			{
				if (!iCSTransform.GetTransform(out value))
				{
					value = Transform.Identity;
				}
			}
			else
			{
				value = Transform.Identity;
			}
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value, out var xUp, out var xDown))
			{
				return UpdateError("Error getting Transform");
			}
			CSDataHelper.GetNurbsCurve(arrayItem, out var value2);
			if (value2 == null)
			{
				return UpdateError("Error getting nurbscurve");
			}
			((GeometryBase)value2).Transform(xDown);
			BoundingBox boundingBox = ((GeometryBase)value2).GetBoundingBox(true);
			if (!((BoundingBox)(ref boundingBox)).IsValid)
			{
				return UpdateError("Error getting boundingbox");
			}
			if (!(num > 0.0))
			{
				continue;
			}
			Point3d val = ((BoundingBox)(ref boundingBox)).Max;
			if (!(num < ((Point3d)(ref val)).Y))
			{
				continue;
			}
			LineCurve val2 = new LineCurve(new Point3d(-50.0, num, 0.0), new Point3d(50.0, num, 0.0));
			CurveIntersections val3 = Intersection.CurveCurve((Curve)(object)value2, (Curve)(object)val2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val3.Count < 2)
			{
				return UpdateError("Error intersection");
			}
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			for (int j = 0; j < val3.Count; j++)
			{
				Point3d val4 = ((Curve)value2).PointAt(val3[j].ParameterA);
				double x = ((Point3d)(ref val4)).X;
				if (j == 0 || x < num3)
				{
					num3 = x;
					num2 = val3[j].ParameterA;
				}
				if (j == 0 || x > num5)
				{
					num5 = x;
					num4 = val3[j].ParameterA;
				}
			}
			value2 = ((Curve)value2).Trim(new Interval(num2, num4)).ToNurbsCurve();
			if (!((CommonObject)value2).IsValid)
			{
				return UpdateError("Error triming");
			}
			Point3d val5 = ((Curve)value2).PointAt(num2);
			Point3d val6 = ((Curve)value2).PointAt(num4);
			Vector3d val7 = ((Curve)value2).TangentAt(num2);
			((Vector3d)(ref val7)).Reverse();
			((Vector3d)(ref val7)).Unitize();
			Vector3d val8 = ((Curve)value2).TangentAt(num4);
			((Vector3d)(ref val8)).Unitize();
			Point3d val9 = val5 + val7 * 50.0;
			Point3d val10 = val6 + val8 * 50.0;
			((Line)(ref val11))._002Ector(val5, val9);
			((Line)(ref val12))._002Ector(val6, val10);
			if (!Intersection.LinePlane(val11, Plane.WorldZX, ref num6))
			{
				return UpdateError("ZX intersection failed on Rhs");
			}
			val9 = ((Line)(ref val11)).PointAt(num6);
			if (!Intersection.LinePlane(new Line(val6, val10), Plane.WorldZX, ref num7))
			{
				return UpdateError("ZX intersection failed on Rhs");
			}
			val10 = ((Line)(ref val12)).PointAt(num7);
			val = ((Curve)value2).PointAtStart;
			double num8 = ((Point3d)(ref val)).DistanceTo(val5);
			val = ((Curve)value2).PointAtStart;
			CurveEnd val13;
			CurveEnd val14;
			if (num8 < ((Point3d)(ref val)).DistanceTo(val6))
			{
				val13 = (CurveEnd)1;
				val14 = (CurveEnd)2;
			}
			else
			{
				val13 = (CurveEnd)2;
				val14 = (CurveEnd)1;
			}
			Curve val15 = ((Curve)value2).DuplicateCurve();
			val15 = val15.Extend(val13, ((Point3d)(ref val5)).DistanceTo(val9), (CurveExtensionStyle)0);
			if (val15 == null)
			{
				return UpdateError("Extend failed on rail Lhs");
			}
			val15 = val15.Extend(val14, ((Point3d)(ref val6)).DistanceTo(val10), (CurveExtensionStyle)0);
			if (val15 == null)
			{
				return UpdateError("Extend failed on rail Rhs");
			}
			((GeometryBase)val15).Transform(xUp);
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val15, xUp, applyTransform: false));
		}
		OutRail.SetData(cSDataArray);
		return base.Update();
	}
}
