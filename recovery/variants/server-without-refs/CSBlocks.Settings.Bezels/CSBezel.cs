using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.Settings.Bezels;

[CSBlock("Bezel", "{F556F7E4-1919-45BB-8317-779D2EC54BA1}")]
public sealed class CSBezel : CSBlock
{
	public CSOutputChannel OutBezel { get; }

	public CSInputChannel InGems { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InGirldeSpacing { get; }

	public CSInputChannel InPlacement { get; }

	public CSInputChannel InAngle { get; }

	public CSInputChannel InHeight { get; }

	public CSInputChannel InTopThickness { get; }

	public CSInputChannel InBottomThickness { get; }

	public CSInputChannel InSeatAngle { get; }

	public CSInputChannel InSeatHeight { get; }

	public CSInputChannel InSeatLength { get; }

	public CSInputChannel InFillet { get; }

	public CSInputChannel InRailRadius { get; }

	public CSInputChannel InRailOffset { get; }

	public CSInputChannel InPoint { get; }

	public CSInputChannel InCornerBlend { get; }

	public CSBezel(string identifier)
		: base(identifier)
	{
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGirldeSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTopThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBottomThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSeatAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSeatHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSeatLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Origin);
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutBezel = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
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
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		InitializeCancellationTokenSource();
		List<CSGemData> gemDatas = InGems.GemList;
		if (gemDatas.Count <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		NurbsCurve profile = InProfile.NurbsCurve;
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
		double girdleSpacing = InGirldeSpacing.Double;
		double placement = InPlacement.Double;
		double angle = InAngle.Double;
		double height = InHeight.Double;
		double topThickness = InTopThickness.Double;
		double bottomThickness = InTopThickness.Double;
		double seatAngle = InSeatAngle.Double;
		double seatHeight = InSeatHeight.Double;
		double seatLength = InSeatLength.Double;
		double fillet = InFillet.Double;
		double cornerBlend = InCornerBlend.Double;
		double railRadius = InRailRadius.Double;
		double railOffset = InRailOffset.Double;
		bool bRail = Math.Abs(railRadius) > 0.001;
		Point3d point = InPoint.Point3d;
		bool bPoint = point != Point3d.Origin;
		CSDataArray cSDataArray = new CSDataArray();
		ParallelOptions parallelOptions = GetBlockParallelOptions();
		int count = gemDatas.Count;
		CSData[] arrBezels = new CSData[count];
		Parallel.For(0, count, parallelOptions, delegate(int i, ParallelLoopState loopState)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			if (!loopState.ShouldExitCurrentIteration && !parallelOptions.CancellationToken.IsCancellationRequested && !loopState.IsStopped && !loopState.IsExceptional)
			{
				CSGemData cSGemData = gemDatas[i];
				CSOnGeometryData bezel;
				string msg;
				if (cSGemData == null)
				{
					UpdateError("Invalid gem");
					loopState.Stop();
				}
				else if (!MakeBezel(cSGemData, profile, point, bPoint, bRail, girdleSpacing, placement, angle, height, topThickness, bottomThickness, seatAngle, seatHeight, seatLength, fillet, railRadius, railOffset, cornerBlend, out bezel, out msg))
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
		for (int num = 0; num < count; num++)
		{
			if (arrBezels[num] != null)
			{
				cSDataArray.Append(arrBezels[num]);
			}
		}
		OutBezel.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private bool UpdateSync()
	{
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		List<CSGemData> gemList = InGems.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		NurbsCurve val = InProfile.NurbsCurve;
		if (val == null)
		{
			Point3d val2 = new Point3d(0.0, 10.0, 0.0);
			Point3d val3 = default(Point3d);
			((Point3d)(ref val3))._002Ector(0.0, 0.0, 0.0);
			val = ((Curve)new LineCurve(val2, val3)).ToNurbsCurve();
			if (val == null)
			{
				return false;
			}
		}
		double girdleSpacing = InGirldeSpacing.Double;
		double placement = InPlacement.Double;
		double angle = InAngle.Double;
		double height = InHeight.Double;
		double topThickness = InTopThickness.Double;
		double bottomThickness = InTopThickness.Double;
		double seatAngle = InSeatAngle.Double;
		double seatHeight = InSeatHeight.Double;
		double seatLength = InSeatLength.Double;
		double fillet = InFillet.Double;
		double cornerBlend = InCornerBlend.Double;
		double num = InRailRadius.Double;
		double railOffset = InRailOffset.Double;
		bool bRail = Math.Abs(num) > 0.001;
		Point3d point3d = InPoint.Point3d;
		bool bPoint = point3d != Point3d.Origin;
		CSDataArray cSDataArray = new CSDataArray();
		int count = gemList.Count;
		CSData[] array = new CSData[count];
		for (int i = 0; i < count; i++)
		{
			CSGemData cSGemData = gemList[i];
			if (cSGemData == null)
			{
				return UpdateError("Invalid gem");
			}
			if (!MakeBezel(cSGemData, val, point3d, bPoint, bRail, girdleSpacing, placement, angle, height, topThickness, bottomThickness, seatAngle, seatHeight, seatLength, fillet, num, railOffset, cornerBlend, out var bezel, out var msg))
			{
				return UpdateError(msg);
			}
			array[i] = bezel;
		}
		if (base.Error)
		{
			return true;
		}
		for (int j = 0; j < count; j++)
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
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
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
		if (!GemOffsetCurve.GemOffset(gem, girdleSpacing, out Curve curve, doTransformUp: false, out msg, cornerBlend))
		{
			return false;
		}
		Transform val3 = Transform.Translation(0.0, 0.0, placement);
		((GeometryBase)curve).Transform(val3);
		if (!GemOffsetCurve.GemOffset(gem, girdleSpacing, out Curve curve2, doTransformUp: false, out msg, cornerBlend))
		{
			return false;
		}
		val3 = Transform.Translation(0.0, 0.0, seatHeight + placement);
		((GeometryBase)curve2).Transform(val3);
		if (!GemOffsetCurve.GemOffset(gem, girdleSpacing + topThickness, out Curve curve3, doTransformUp: false, out msg, cornerBlend))
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
			if (!GemOffsetCurve.GemOffset(gem, girdleSpacing + num, out curve4, doTransformUp: false, out msg, cornerBlend))
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
		if (!GemOffsetCurve.GemOffset(obj, 0.0 - bottomThickness, out Curve curve5, doTransformUp: false, out msg, cornerBlend))
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
		List<Curve> list = new List<Curve>
		{
			(Curve)(object)crv,
			curve5
		};
		if (curve4 != null)
		{
			list.Add(curve4);
		}
		list.Add(curve);
		list.Add(curve2);
		Brep item = Brep.CreateFromLoft((IEnumerable<Curve>)list.ToArray(), Point3d.Unset, Point3d.Unset, (LoftType)3, false)[0];
		Brep[] array = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { item, srf2, srf }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array == null || array.Length != 1)
		{
			return false;
		}
		((GeometryBase)array[0]).Transform(xUp);
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
