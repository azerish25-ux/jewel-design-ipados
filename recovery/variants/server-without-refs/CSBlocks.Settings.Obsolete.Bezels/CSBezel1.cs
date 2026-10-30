using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("Bezel_V1", "{AA2DD327-D6B1-42E6-ABE8-8301049D34EC}")]
public sealed class CSBezel1 : CSBlock
{
	public readonly CSOutputChannel OutBezel;

	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InGirldeSpacing;

	public readonly CSInputChannel InPlacement;

	public readonly CSInputChannel InAngle;

	public readonly CSInputChannel InHeight;

	public readonly CSInputChannel InTopThickness;

	public readonly CSInputChannel InBottomThickness;

	public readonly CSInputChannel InSeatAngle;

	public readonly CSInputChannel InSeatHeight;

	public readonly CSInputChannel InSeatLength;

	public readonly CSInputChannel InFillet;

	public readonly CSInputChannel InRailRadius;

	public readonly CSInputChannel InRailOffset;

	public readonly CSInputChannel InPoint;

	public readonly CSInputChannel InCornerBlend;

	public CSBezel1(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGirldeSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InTopThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBottomThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSeatAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSeatHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSeatLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRailOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutBezel = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!CSApplicationHelper.Application.IsBlockParallelProcessingEnabled)
		{
			return UpdateSync();
		}
		return UpdateConcurrent();
	}

	private bool UpdateConcurrent()
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		InitializeCancellationTokenSource();
		CSDataArray gemDatas = InGems.GetFlattenArray();
		if (gemDatas.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		NurbsCurve profile = null;
		CSDataHelper.GetNurbsCurve(InProfile.Data, out profile);
		if (profile == null)
		{
			Point3d val = new Point3d(0.0, 10.0, 0.0);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(0.0, 0.0, 0.0);
			LineCurve val3 = new LineCurve(val, val2);
			profile = ((Curve)val3).ToNurbsCurve();
			if (profile == null)
			{
				return false;
			}
		}
		double girdleSpacing = 0.0;
		InGirldeSpacing.GetDouble(ref girdleSpacing);
		double placement = 0.0;
		InPlacement.GetDouble(ref placement);
		double angle = 0.0;
		InAngle.GetDouble(ref angle);
		double height = 0.0;
		InHeight.GetDouble(ref height);
		double topThickness = 0.0;
		InTopThickness.GetDouble(ref topThickness);
		double bottomThickness = 0.0;
		InBottomThickness.GetDouble(ref bottomThickness);
		double seatAngle = 0.0;
		InSeatAngle.GetDouble(ref seatAngle);
		double seatHeight = 0.0;
		InSeatHeight.GetDouble(ref seatHeight);
		double seatLength = 0.0;
		InSeatLength.GetDouble(ref seatLength);
		double fillet = 0.0;
		InFillet.GetDouble(ref fillet);
		InCornerBlend.GetValue(out double cornerBlend);
		double railRadius = 0.0;
		double railOffset = 0.0;
		bool bRail = InRailRadius.GetDouble(ref railRadius);
		if (bRail)
		{
			InRailOffset.GetDouble(ref railOffset);
		}
		Point3d point = Point3d.Origin;
		bool bPoint = CSDataHelper.GetPoint(InPoint, out point);
		CSDataArray cSDataArray = new CSDataArray();
		ParallelOptions parallelOptions = GetBlockParallelOptions();
		int arrayCount = gemDatas.GetArrayCount();
		CSData[] arrBezels = new CSData[arrayCount];
		Parallel.For(0, arrayCount, parallelOptions, delegate(int i, ParallelLoopState loopState)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			if (!loopState.ShouldExitCurrentIteration && !parallelOptions.CancellationToken.IsCancellationRequested && !loopState.IsStopped && !loopState.IsExceptional)
			{
				CSOnGeometryData bezel;
				string msg;
				if (!(gemDatas.GetArrayItem(i) is CSGemData gem))
				{
					UpdateError("Invalid gem");
					loopState.Stop();
				}
				else if (!MakeBezel(gem, profile, point, bPoint, bRail, girdleSpacing, placement, angle, height, topThickness, bottomThickness, seatAngle, seatHeight, seatLength, fillet, railRadius, railOffset, cornerBlend, out bezel, out msg))
				{
					UpdateError(msg);
					loopState.Stop();
				}
				else
				{
					arrBezels[i] = bezel;
					CS.CSUtil.CheckAndTerminateOrphanedParallelTasks();
				}
			}
		});
		if (base.Error)
		{
			return true;
		}
		for (int num = 0; num < arrayCount; num++)
		{
			if (arrBezels[num] != null)
			{
				cSDataArray.Append(arrBezels[num]);
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			OutBezel.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			OutBezel.SetData(cSDataArray);
		}
		return base.Update();
	}

	private bool UpdateSync()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InGems.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		NurbsCurve value = null;
		CSDataHelper.GetNurbsCurve(InProfile.Data, out value);
		if (value == null)
		{
			Point3d val = new Point3d(0.0, 10.0, 0.0);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(0.0, 0.0, 0.0);
			value = ((Curve)new LineCurve(val, val2)).ToNurbsCurve();
			if (value == null)
			{
				return false;
			}
		}
		double value2 = 0.0;
		InGirldeSpacing.GetDouble(ref value2);
		double value3 = 0.0;
		InPlacement.GetDouble(ref value3);
		double value4 = 0.0;
		InAngle.GetDouble(ref value4);
		double value5 = 0.0;
		InHeight.GetDouble(ref value5);
		double value6 = 0.0;
		InTopThickness.GetDouble(ref value6);
		double value7 = 0.0;
		InBottomThickness.GetDouble(ref value7);
		double value8 = 0.0;
		InSeatAngle.GetDouble(ref value8);
		double value9 = 0.0;
		InSeatHeight.GetDouble(ref value9);
		double value10 = 0.0;
		InSeatLength.GetDouble(ref value10);
		double value11 = 0.0;
		InFillet.GetDouble(ref value11);
		InCornerBlend.GetValue(out double value12);
		double value13 = 0.0;
		double value14 = 0.0;
		bool flag = InRailRadius.GetDouble(ref value13);
		if (flag)
		{
			InRailOffset.GetDouble(ref value14);
		}
		Point3d value15;
		bool point = CSDataHelper.GetPoint(InPoint, out value15);
		CSDataArray cSDataArray = new CSDataArray();
		int arrayCount = flattenArray.GetArrayCount();
		CSData[] array = new CSData[arrayCount];
		for (int i = 0; i < arrayCount; i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData gem))
			{
				return UpdateError("Invalid gem");
			}
			if (!MakeBezel(gem, value, value15, point, flag, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value13, value14, value12, out var bezel, out var msg))
			{
				return UpdateError(msg);
			}
			array[i] = bezel;
		}
		if (base.Error)
		{
			return true;
		}
		for (int j = 0; j < arrayCount; j++)
		{
			if (array[j] != null)
			{
				cSDataArray.Append(array[j]);
			}
		}
		OutBezel.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private static bool BezelSurfaceTop(Curve rail1, Curve rail2, double fillet, out Brep srf)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		srf = null;
		if (rail1 == null || rail2 == null)
		{
			return false;
		}
		Point3d pointAtStart = rail1.PointAtStart;
		Point3d pointAtStart2 = rail2.PointAtStart;
		LineCurve val = new LineCurve(pointAtStart, pointAtStart2);
		NurbsCurve val2;
		Interval domain;
		if (fillet == 0.0)
		{
			val2 = ((Curve)val).ToNurbsCurve();
		}
		else
		{
			domain = ((Curve)val).Domain;
			Point3d val3 = ((Curve)val).PointAt(((Interval)(ref domain)).Mid);
			double length = ((Curve)val).GetLength();
			if (double.IsNaN(length) || length < 0.0)
			{
				return false;
			}
			double num = length / 2.0;
			Point3d val4 = val3;
			((Point3d)(ref val4)).Z = ((Point3d)(ref val4)).Z + num * fillet;
			Point3d val5 = pointAtStart2;
			val2 = ((Curve)new ArcCurve(new Arc(pointAtStart, val4, val5))).ToNurbsCurve();
		}
		Curve[] profile = (Curve[])(object)new Curve[1] { (Curve)val2 };
		double[] array = new double[1];
		domain = rail1.Domain;
		array[0] = ((Interval)(ref domain)).Min;
		double[] array2 = new double[1];
		domain = rail2.Domain;
		array2[0] = ((Interval)(ref domain)).Min;
		if (!CSUtilSweepRC.Sweep2(rail1, rail2, profile, array, array2, out var breps, closed: true, maintainHeight: true))
		{
			CS.CSUtil.Print("Failed Sweep2\n");
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		if (!((CommonObject)breps).IsValid)
		{
			return false;
		}
		srf = breps;
		return true;
	}

	private static bool MakeBezel(CSGemData gem, NurbsCurve profile, Point3d point, bool bPoint, bool bRail, double girdleSpacing, double placement, double angle, double height, double topThickness, double bottomThickness, double seatAngle, double seatHeight, double seatLength, double fillet, double railRadius, double railOffset, double cornerBlend, out CSOnGeometryData bezel, out string msg)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		msg = string.Empty;
		bezel = null;
		if (gem == null)
		{
			return false;
		}
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXform(gem.Xform, out var xUp))
		{
			return false;
		}
		if (bRail)
		{
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(0.0, 0.0, 0.0);
			((Point3d)(ref val)).Transform(xUp);
			((Point3d)(ref val)).Y = 0.0;
			height = ((Point3d)(ref val)).DistanceTo(Point3d.Origin) - railRadius - railOffset + seatHeight + placement;
		}
		else if (bPoint)
		{
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(0.0, 0.0, 0.0);
			((Point3d)(ref val2)).Transform(xUp);
			((Point3d)(ref val2)).Y = 0.0;
			height = ((Point3d)(ref point)).DistanceTo(val2) + seatHeight + placement;
		}
		Transform val3 = default(Transform);
		if (!GemOffsetCurveV2.GemOffset(gem, girdleSpacing, out Curve curve, doTransformUp: false, out msg, cornerBlend))
		{
			return false;
		}
		val3 = Transform.Translation(0.0, 0.0, placement);
		((GeometryBase)curve).Transform(val3);
		if (!GemOffsetCurveV2.GemOffset(gem, girdleSpacing, out Curve curve2, doTransformUp: false, out msg, cornerBlend))
		{
			return false;
		}
		val3 = Transform.Translation(0.0, 0.0, seatHeight + placement);
		((GeometryBase)curve2).Transform(val3);
		if (!GemOffsetCurveV2.GemOffset(gem, girdleSpacing + topThickness, out Curve curve3, doTransformUp: false, out msg, cornerBlend))
		{
			return false;
		}
		val3 = Transform.Translation(0.0, 0.0, seatHeight + placement);
		((GeometryBase)curve3).Transform(val3);
		if (!SurfaceBezelOutside(curve3.ToNurbsCurve(), profile, angle, height, out var srf, out var crv))
		{
			return false;
		}
		Curve curve4 = null;
		if (seatLength > 0.0)
		{
			double num = (0.0 - Math.Sin(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			double num2 = (0.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			if (!GemOffsetCurveV2.GemOffset(gem, girdleSpacing + num, out curve4, doTransformUp: false, out msg, cornerBlend))
			{
				return false;
			}
			val3 = Transform.Translation(0.0, 0.0, num2 + placement);
			((GeometryBase)curve4).Transform(val3);
		}
		BoundingBox boundingBox = ((GeometryBase)crv).GetBoundingBox(true);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		CSGemData obj = (CSGemData)gem.Duplicate();
		obj.ScaleToSize(x, y);
		if (!GemOffsetCurveV2.GemOffset(obj, 0.0 - bottomThickness, out Curve curve5, doTransformUp: false, out msg, cornerBlend))
		{
			return false;
		}
		val3 = Transform.Translation(0.0, 0.0, 0.0 - height + seatHeight + placement);
		((GeometryBase)curve5).Transform(val3);
		fillet /= 100.0;
		if (!BezelSurfaceTop(curve2, curve3, fillet, out var srf2))
		{
			return false;
		}
		List<Curve> list = new List<Curve>();
		list.Add((Curve)(object)crv);
		list.Add(curve5);
		if (curve4 != null)
		{
			list.Add(curve4);
		}
		list.Add(curve);
		list.Add(curve2);
		Brep item = Brep.CreateFromLoft((IEnumerable<Curve>)list.ToArray(), Point3d.Unset, Point3d.Unset, (LoftType)3, false)[0];
		Brep[] array = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { item, srf2, srf }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array == null)
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		((GeometryBase)array[0]).Transform(xUp);
		CSDataHelper.SetSettingType(gem, GemSettingType.BZ);
		CSUtilGeometry.FixBrepSolidOrientation(ref array[0]);
		bezel = new CSOnGeometryData((GeometryBase)(object)array[0], xUp, applyTransform: false);
		return true;
	}

	private static bool SurfaceBezelOutside(NurbsCurve rail, NurbsCurve oprofile, double angle, double height, out Brep srf, out NurbsCurve crv)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		srf = null;
		crv = null;
		if (rail == null || oprofile == null)
		{
			return false;
		}
		NurbsCurve val = new NurbsCurve(oprofile);
		Point3d pointAtStart = ((Curve)val).PointAtStart;
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(pointAtStart);
		((Vector3d)(ref val2)).Reverse();
		Transform val3 = Transform.Translation(val2);
		Transform val4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
		Transform val5 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val).Transform(val5 * val4 * val3);
		Point3d pointAtEnd = ((Curve)val).PointAtEnd;
		double num = CSBlockUtil.CSUtilMath.AngleOfPointD(((Point3d)(ref pointAtEnd)).Y, 0.0 - ((Point3d)(ref pointAtEnd)).Z);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle) - num, Vector3d.XAxis, Point3d.Origin);
		((GeometryBase)val).Transform(val6);
		Point3d pointAtEnd2 = ((Curve)val).PointAtEnd;
		double num2 = 0.0 - ((Point3d)(ref pointAtEnd2)).Z;
		double num3 = height / num2;
		Transform val7 = Transform.Scale(Plane.WorldXY, num3, num3, num3);
		((GeometryBase)val).Transform(val7);
		Point3d pointAtStart2 = ((Curve)rail).PointAtStart;
		pointAtStart = ((Curve)val).PointAtStart;
		val3 = Transform.Translation(pointAtStart2 - pointAtStart);
		((GeometryBase)val).Transform(val3);
		NurbsSurface val8 = CSUtilRailRevolve.RailRevolveRC(val, rail, 0);
		if (val8 == null)
		{
			return false;
		}
		if (!((CommonObject)val8).IsValid)
		{
			return false;
		}
		srf = ((Surface)val8).ToBrep();
		Point3d pointAtStart3 = ((Curve)val).PointAtStart;
		((Point3d)(ref pointAtStart3)).Z = 0.0;
		Point3d pointAtEnd3 = ((Curve)val).PointAtEnd;
		((Point3d)(ref pointAtEnd3)).Z = 0.0;
		double num4 = ((Point3d)(ref pointAtStart3)).DistanceTo(Point3d.Origin);
		num3 = ((Point3d)(ref pointAtEnd3)).DistanceTo(Point3d.Origin) / num4;
		val7 = Transform.Scale(Plane.WorldXY, num3, num3, 1.0);
		((GeometryBase)rail).Transform(val7);
		val3 = Transform.Translation(0.0, 0.0, 0.0 - height);
		((GeometryBase)rail).Transform(val3);
		crv = (NurbsCurve)((GeometryBase)rail).Duplicate();
		return true;
	}
}
