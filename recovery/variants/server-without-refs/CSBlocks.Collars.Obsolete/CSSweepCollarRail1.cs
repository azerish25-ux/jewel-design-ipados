using System;
using System.Collections.Generic;
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

namespace CSBlocks.Collars.Obsolete;

[Obsolete]
[CSBlock("SweepCollarRail_V1", "{7B55FDB0-1904-4d6e-ADCC-C9D9CEEA3987}")]
public sealed class CSSweepCollarRail1 : CSBlock
{
	public CSInputChannel InProfile { get; }

	public CSInputChannel InBottomTrim { get; }

	public CSOutputChannel OutRail { get; }

	public CSSweepCollarRail1(string identifier)
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
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InProfile.GetFlattenArray();
		double num = InBottomTrim.Double;
		CSDataArray cSDataArray = new CSDataArray();
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
			Point3d max = ((BoundingBox)(ref boundingBox)).Max;
			if (!(num < ((Point3d)(ref max)).Y))
			{
				continue;
			}
			LineCurve val = new LineCurve(new Point3d(-50.0, num, 0.0), new Point3d(50.0, num, 0.0));
			CurveIntersections val2 = Intersection.CurveCurve((Curve)(object)value2, (Curve)(object)val, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val2.Count < 2)
			{
				return UpdateError("Error intersection");
			}
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			for (int j = 0; j < val2.Count; j++)
			{
				Point3d val3 = ((Curve)value2).PointAt(val2[j].ParameterA);
				double x = ((Point3d)(ref val3)).X;
				if (j == 0 || x < num3)
				{
					num3 = x;
					num2 = val2[j].ParameterA;
				}
				if (j == 0 || x > num5)
				{
					num5 = x;
					num4 = val2[j].ParameterA;
				}
			}
			value2 = ((Curve)value2).Trim(new Interval(num2, num4)).ToNurbsCurve();
			if (!((CommonObject)value2).IsValid)
			{
				return UpdateError("Error triming");
			}
			((Point3d)(ref max))._002Ector(((Curve)value2).PointAt(num2));
			((Point3d)(ref max)).Y = 0.0;
			Point3d val4 = max;
			((Point3d)(ref max))._002Ector(((Curve)value2).PointAt(num4));
			((Point3d)(ref max)).Y = 0.0;
			Point3d val5 = max;
			LineCurve val6 = new LineCurve(((Curve)value2).PointAt(num2), val4);
			LineCurve val7 = new LineCurve(((Curve)value2).PointAt(num4), val5);
			Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3]
			{
				(Curve)val6,
				(Curve)value2,
				(Curve)val7
			});
			if (array == null)
			{
				return UpdateError("Error merging curves");
			}
			if (array.Length != 1)
			{
				return UpdateError("Error merging curves");
			}
			value2 = array[0].ToNurbsCurve();
			if (value2 == null)
			{
				return UpdateError("Error merging curves");
			}
			((GeometryBase)value2).Transform(xUp);
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)value2, xUp, applyTransform: false));
		}
		OutRail.SetData(cSDataArray);
		return base.Update();
	}
}
