using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Surfaces.Obsolete;

[Obsolete]
[CSBlock("MicroPave_V1", "{8821412F-048D-4819-BD1B-7894D67DECF8}")]
public sealed class CSMicroPave1 : CSBlock
{
	public readonly CSInputChannel iGems;

	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iBaseCurve;

	public readonly CSInputChannel iRingRail;

	public readonly CSInputChannel iProngHeight;

	public readonly CSOutputChannel oSurface;

	public readonly CSOutputChannel oStartCurve;

	public readonly CSOutputChannel oEndCurve;

	public CSMicroPave1(string identifier)
		: base(identifier)
	{
		iGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iBaseCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iRingRail = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		oStartCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oEndCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		CSDataArray flattenArray2 = iCurve.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve");
		}
		CSData arrayItem = flattenArray2.GetArrayItem(0);
		if (arrayItem == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataHelper.GetNurbsCurve(arrayItem, out var value);
		CSDataArray flattenArray3 = iBaseCurve.GetFlattenArray();
		if (flattenArray3.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve");
		}
		CSData arrayItem2 = flattenArray3.GetArrayItem(0);
		if (arrayItem2 == null)
		{
			return UpdateMissing("BaseCurve");
		}
		CSDataHelper.GetNurbsCurve(arrayItem2, out var value2);
		double value3 = 0.0;
		iRingRail.GetDouble(ref value3);
		double value4 = 1.0;
		iProngHeight.GetDouble(ref value4);
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData = (CSGemData)flattenArray.GetArrayItem(i);
			Transform xform = cSGemData.Xform;
			Point3d testPoint = new Point3d(0.0, 0.0, 0.0);
			testPoint.Transform(xform);
			double t = 0.0;
			value.ClosestPoint(testPoint, out t);
			list.Add(t);
			list2.Add(cSGemData.GemXSize / 2.0);
		}
		NurbsSurface ns = null;
		NurbsCurve StartProfile = null;
		NurbsCurve EndProfile = null;
		ProngClass(value, value2, value3, list, list2, value4, ref ns, ref StartProfile, ref EndProfile);
		oSurface.SetData(new CSOnGeometryData(ns));
		oStartCurve.SetData(new CSOnGeometryData(StartProfile));
		oEndCurve.SetData(new CSOnGeometryData(EndProfile));
		return base.Update();
	}

	private static void ProngClass(Curve crv, NurbsCurve baseCurve, double ringRailRadius, List<double> lParam, List<double> lWidth, double Z, ref NurbsSurface ns, ref NurbsCurve StartProfile, ref NurbsCurve EndProfile)
	{
		int num = lParam.Count + 1;
		int num2 = 10;
		int num3 = num2 * (num - 1) + 1;
		int num4 = 15;
		int count = baseCurve.Points.Count;
		int num5 = num4 + count * 2;
		int num6 = 0;
		double num7 = 0.0;
		Interval domain = baseCurve.Domain;
		_ = domain.Min;
		num7 = domain.Max;
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		ControlPoint[,] array = new ControlPoint[num3, num5];
		double length = crv.GetLength();
		int num8 = 0;
		list.Add(0.0);
		list2.Add(0.0);
		for (int i = 0; i < lParam.Count; i++)
		{
			double t = lParam[i];
			double length2 = crv.GetLength(0.01, new Interval(crv.Domain.Min, t));
			length2 /= length;
			list.Add(length2);
			list2.Add(lWidth[i]);
		}
		list.Add(0.0);
		list2.Add(0.0);
		list[0] = list[1] - (list[2] - list[1]);
		list[list.Count - 1] = list[list.Count - 2] - (list[list.Count - 3] - list[list.Count - 2]);
		list2[0] = list2[1] - (list2[2] - list2[1]);
		list2[list2.Count - 1] = list2[list2.Count - 2] - (list2[list2.Count - 3] - list2[list2.Count - 2]);
		for (int j = 0; j < num; j++)
		{
			double num9 = list[j];
			double num10 = list[j + 1] - num9;
			double num11 = list2[j];
			double num12 = list2[j + 1] - num11;
			int num13 = 0;
			int num14 = num2;
			if (j == 0)
			{
				num13 = num2 / 2;
			}
			if (j == num - 1)
			{
				num14 = num2 / 2 + 1;
			}
			for (int k = num13; k < num14; k++)
			{
				double s = num9 + num10 * Prong1.cv[k, 0].Location.X;
				double num15 = num11 + num12 * ((double)k / (double)num2);
				double t2 = 0.0;
				crv.NormalizedLengthParameter(s, out t2);
				Point3d point3d = crv.PointAt(t2);
				Vector3d vector3d = crv.TangentAt(t2);
				Vector3d b = new Vector3d(0.0, 1.0, 0.0);
				Vector3d vector3d2 = Vector3d.CrossProduct(vector3d, b);
				vector3d2.Unitize();
				Vector3d vector3d3 = Vector3d.CrossProduct(vector3d2, vector3d);
				vector3d2.Y = 0.0;
				vector3d2.Unitize();
				Point3d linePoint = new Point3d(point3d);
				linePoint.Y = 0.0;
				double lineParamOfClosestPoint = 0.0;
				double paramAlongLine = 0.0;
				Prong1.PointOnLineAtDistanceFromPoint(linePoint, vector3d2, new Point3d(0.0, 0.0, 0.0), ringRailRadius, ref lineParamOfClosestPoint, ref paramAlongLine);
				double num16 = 0.0 - lineParamOfClosestPoint - paramAlongLine;
				int num17 = baseCurve.Points.Count - 1;
				num8 = 0;
				for (int l = 0; l < baseCurve.Points.Count; l++)
				{
					ControlPoint controlPoint = baseCurve.Points[num17 - l];
					double num18 = controlPoint.Location.X / 5.0;
					double num19 = controlPoint.Location.Y / 2.0;
					Point3d point3d2 = point3d + vector3d3 * num18 * Prong1.cv[k, 0].Location.Y * num15 + vector3d2 * (num19 - num16);
					array[num6, num8] = new ControlPoint(point3d2.X, point3d2.Y, point3d2.Z, 1.0);
					num8++;
				}
				for (int m = 0; m < num4; m++)
				{
					double num20 = Prong1.cv[k, m].Location.Z * Z;
					Point3d point3d3 = point3d + vector3d3 * Prong1.cv[k, m].Location.Y * num15 + vector3d2 * num20;
					array[num6, num8] = new ControlPoint(point3d3.X, point3d3.Y, point3d3.Z, 1.0);
					num8++;
				}
				for (int n = 0; n < baseCurve.Points.Count; n++)
				{
					ControlPoint controlPoint2 = baseCurve.Points[n];
					double num21 = (0.0 - controlPoint2.Location.X) / 5.0;
					double num22 = controlPoint2.Location.Y / 2.0;
					Point3d point3d4 = point3d + vector3d3 * num21 * Prong1.cv[k, 0].Location.Y * num15 + vector3d2 * (num22 - num16);
					array[num6, num8] = new ControlPoint(point3d4.X, point3d4.Y, point3d4.Z, 1.0);
					num8++;
				}
				num6++;
			}
		}
		double[] array2 = new double[num3 + 2];
		array2[0] = 0.0;
		array2[1] = 0.0;
		for (int num23 = 0; num23 < num3 - 2; num23++)
		{
			array2[num23 + 2] = num23;
		}
		array2[num3] = num3 - 3;
		array2[num3 + 1] = num3 - 3;
		double[] array3 = new double[num5 + 2];
		int num24 = baseCurve.Knots.Count - 1;
		num8 = 0;
		for (int num25 = 0; num25 < baseCurve.Points.Count + 1; num25++)
		{
			array3[num8] = num7 - baseCurve.Knots[num24 - num25];
			num8++;
		}
		for (int num26 = 0; num26 < num4; num26++)
		{
			array3[num8] = num7 + (double)num26;
			num8++;
		}
		double num27 = num7 + (double)num4 - 1.0;
		for (int num28 = 0; num28 < baseCurve.Points.Count + 1; num28++)
		{
			array3[num8] = num27 + baseCurve.Knots[num28 + 1];
			num8++;
		}
		ns = NurbsSurface.Create(3, isRational: false, 4, 4, num3, num5);
		for (int num29 = 0; num29 < array2.Length; num29++)
		{
			ns.KnotsU[num29] = array2[num29];
		}
		for (int num30 = 0; num30 < array3.Length; num30++)
		{
			ns.KnotsV[num30] = array3[num30];
		}
		for (int num31 = 0; num31 < num3; num31++)
		{
			for (int num32 = 0; num32 < num5; num32++)
			{
				ns.Points.SetControlPoint(num31, num32, array[num31, num32]);
			}
		}
		StartProfile = new NurbsCurve(3, rational: false, 4, num5);
		EndProfile = new NurbsCurve(3, rational: false, 4, num5);
		for (int num33 = 0; num33 < array3.Length; num33++)
		{
			StartProfile.Knots[num33] = array3[num33];
			EndProfile.Knots[num33] = array3[num33];
		}
		for (int num34 = 0; num34 < num5; num34++)
		{
			StartProfile.Points[num34] = array[0, num34];
			EndProfile.Points[num34] = array[num3 - 1, num34];
		}
	}
}
