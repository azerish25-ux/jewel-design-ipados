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
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
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
		Point3d val = default(Point3d);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData = (CSGemData)flattenArray.GetArrayItem(i);
			Transform xform = cSGemData.Xform;
			((Point3d)(ref val))._002Ector(0.0, 0.0, 0.0);
			((Point3d)(ref val)).Transform(xform);
			double item = 0.0;
			((Curve)value).ClosestPoint(val, ref item);
			list.Add(item);
			list2.Add(cSGemData.GemXSize / 2.0);
		}
		NurbsSurface ns = null;
		NurbsCurve StartProfile = null;
		NurbsCurve EndProfile = null;
		ProngClass((Curve)(object)value, value2, value3, list, list2, value4, ref ns, ref StartProfile, ref EndProfile);
		oSurface.SetData(new CSOnGeometryData((GeometryBase)(object)ns));
		oStartCurve.SetData(new CSOnGeometryData((GeometryBase)(object)StartProfile));
		oEndCurve.SetData(new CSOnGeometryData((GeometryBase)(object)EndProfile));
		return base.Update();
	}

	private static void ProngClass(Curve crv, NurbsCurve baseCurve, double ringRailRadius, List<double> lParam, List<double> lWidth, double Z, ref NurbsSurface ns, ref NurbsCurve StartProfile, ref NurbsCurve EndProfile)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Expected O, but got Unknown
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		int num = lParam.Count + 1;
		int num2 = 10;
		int num3 = num2 * (num - 1) + 1;
		int num4 = 15;
		int count = baseCurve.Points.Count;
		int num5 = num4 + count * 2;
		int num6 = 0;
		double num7 = 0.0;
		Interval domain = ((Curve)baseCurve).Domain;
		_ = ((Interval)(ref domain)).Min;
		num7 = ((Interval)(ref domain)).Max;
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		ControlPoint[,] array = new ControlPoint[num3, num5];
		double length = crv.GetLength();
		int num8 = 0;
		list.Add(0.0);
		list2.Add(0.0);
		for (int i = 0; i < lParam.Count; i++)
		{
			double num9 = lParam[i];
			Interval domain2 = crv.Domain;
			double length2 = crv.GetLength(0.01, new Interval(((Interval)(ref domain2)).Min, num9));
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
		Vector3d val3 = default(Vector3d);
		Point3d linePoint = default(Point3d);
		for (int j = 0; j < num; j++)
		{
			double num10 = list[j];
			double num11 = list[j + 1] - num10;
			double num12 = list2[j];
			double num13 = list2[j + 1] - num12;
			int num14 = 0;
			int num15 = num2;
			if (j == 0)
			{
				num14 = num2 / 2;
			}
			if (j == num - 1)
			{
				num15 = num2 / 2 + 1;
			}
			for (int k = num14; k < num15; k++)
			{
				Point3d location = ((ControlPoint)(ref Prong1.cv[k, 0])).Location;
				double num16 = num10 + num11 * ((Point3d)(ref location)).X;
				double num17 = num12 + num13 * ((double)k / (double)num2);
				double num18 = 0.0;
				crv.NormalizedLengthParameter(num16, ref num18);
				Point3d val = crv.PointAt(num18);
				Vector3d val2 = crv.TangentAt(num18);
				((Vector3d)(ref val3))._002Ector(0.0, 1.0, 0.0);
				Vector3d val4 = Vector3d.CrossProduct(val2, val3);
				((Vector3d)(ref val4)).Unitize();
				Vector3d val5 = Vector3d.CrossProduct(val4, val2);
				((Vector3d)(ref val4)).Y = 0.0;
				((Vector3d)(ref val4)).Unitize();
				((Point3d)(ref linePoint))._002Ector(val);
				((Point3d)(ref linePoint)).Y = 0.0;
				double lineParamOfClosestPoint = 0.0;
				double paramAlongLine = 0.0;
				Prong1.PointOnLineAtDistanceFromPoint(linePoint, val4, new Point3d(0.0, 0.0, 0.0), ringRailRadius, ref lineParamOfClosestPoint, ref paramAlongLine);
				double num19 = 0.0 - lineParamOfClosestPoint - paramAlongLine;
				int num20 = baseCurve.Points.Count - 1;
				num8 = 0;
				for (int l = 0; l < baseCurve.Points.Count; l++)
				{
					ControlPoint val6 = baseCurve.Points[num20 - l];
					location = ((ControlPoint)(ref val6)).Location;
					double num21 = ((Point3d)(ref location)).X / 5.0;
					location = ((ControlPoint)(ref val6)).Location;
					double num22 = ((Point3d)(ref location)).Y / 2.0;
					Vector3d val7 = val5 * num21;
					location = ((ControlPoint)(ref Prong1.cv[k, 0])).Location;
					Point3d val8 = val + val7 * ((Point3d)(ref location)).Y * num17 + val4 * (num22 - num19);
					array[num6, num8] = new ControlPoint(((Point3d)(ref val8)).X, ((Point3d)(ref val8)).Y, ((Point3d)(ref val8)).Z, 1.0);
					num8++;
				}
				for (int m = 0; m < num4; m++)
				{
					location = ((ControlPoint)(ref Prong1.cv[k, m])).Location;
					double num23 = ((Point3d)(ref location)).Z * Z;
					location = ((ControlPoint)(ref Prong1.cv[k, m])).Location;
					Point3d val9 = val + val5 * ((Point3d)(ref location)).Y * num17 + val4 * num23;
					array[num6, num8] = new ControlPoint(((Point3d)(ref val9)).X, ((Point3d)(ref val9)).Y, ((Point3d)(ref val9)).Z, 1.0);
					num8++;
				}
				for (int n = 0; n < baseCurve.Points.Count; n++)
				{
					ControlPoint val10 = baseCurve.Points[n];
					location = ((ControlPoint)(ref val10)).Location;
					double num24 = (0.0 - ((Point3d)(ref location)).X) / 5.0;
					location = ((ControlPoint)(ref val10)).Location;
					double num25 = ((Point3d)(ref location)).Y / 2.0;
					Vector3d val11 = val5 * num24;
					location = ((ControlPoint)(ref Prong1.cv[k, 0])).Location;
					Point3d val12 = val + val11 * ((Point3d)(ref location)).Y * num17 + val4 * (num25 - num19);
					array[num6, num8] = new ControlPoint(((Point3d)(ref val12)).X, ((Point3d)(ref val12)).Y, ((Point3d)(ref val12)).Z, 1.0);
					num8++;
				}
				num6++;
			}
		}
		double[] array2 = new double[num3 + 2];
		array2[0] = 0.0;
		array2[1] = 0.0;
		for (int num26 = 0; num26 < num3 - 2; num26++)
		{
			array2[num26 + 2] = num26;
		}
		array2[num3] = num3 - 3;
		array2[num3 + 1] = num3 - 3;
		double[] array3 = new double[num5 + 2];
		int num27 = baseCurve.Knots.Count - 1;
		num8 = 0;
		for (int num28 = 0; num28 < baseCurve.Points.Count + 1; num28++)
		{
			array3[num8] = num7 - baseCurve.Knots[num27 - num28];
			num8++;
		}
		for (int num29 = 0; num29 < num4; num29++)
		{
			array3[num8] = num7 + (double)num29;
			num8++;
		}
		double num30 = num7 + (double)num4 - 1.0;
		for (int num31 = 0; num31 < baseCurve.Points.Count + 1; num31++)
		{
			array3[num8] = num30 + baseCurve.Knots[num31 + 1];
			num8++;
		}
		ns = NurbsSurface.Create(3, false, 4, 4, num3, num5);
		for (int num32 = 0; num32 < array2.Length; num32++)
		{
			ns.KnotsU[num32] = array2[num32];
		}
		for (int num33 = 0; num33 < array3.Length; num33++)
		{
			ns.KnotsV[num33] = array3[num33];
		}
		for (int num34 = 0; num34 < num3; num34++)
		{
			for (int num35 = 0; num35 < num5; num35++)
			{
				ns.Points.SetControlPoint(num34, num35, array[num34, num35]);
			}
		}
		StartProfile = new NurbsCurve(3, false, 4, num5);
		EndProfile = new NurbsCurve(3, false, 4, num5);
		for (int num36 = 0; num36 < array3.Length; num36++)
		{
			StartProfile.Knots[num36] = array3[num36];
			EndProfile.Knots[num36] = array3[num36];
		}
		for (int num37 = 0; num37 < num5; num37++)
		{
			StartProfile.Points[num37] = array[0, num37];
			EndProfile.Points[num37] = array[num3 - 1, num37];
		}
	}
}
