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
			Point3d point3d = new Point3d(0.0, 10.0, 0.0);
			Point3d to = new Point3d(0.0, 0.0, 0.0);
			LineCurve lineCurve = new LineCurve(point3d, to);
			profile = lineCurve.ToNurbsCurve();
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
		CSDataArray flattenArray = InGems.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		NurbsCurve value = null;
		CSDataHelper.GetNurbsCurve(InProfile.Data, out value);
		if (value == null)
		{
			Point3d point3d = new Point3d(0.0, 10.0, 0.0);
			Point3d to = new Point3d(0.0, 0.0, 0.0);
			value = new LineCurve(point3d, to).ToNurbsCurve();
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
		srf = null;
		if (rail1 == null || rail2 == null)
		{
			return false;
		}
		Point3d pointAtStart = rail1.PointAtStart;
		Point3d pointAtStart2 = rail2.PointAtStart;
		LineCurve lineCurve = new LineCurve(pointAtStart, pointAtStart2);
		NurbsCurve nurbsCurve;
		if (fillet == 0.0)
		{
			nurbsCurve = lineCurve.ToNurbsCurve();
		}
		else
		{
			Point3d point3d = lineCurve.PointAt(lineCurve.Domain.Mid);
			double length = lineCurve.GetLength();
			if (double.IsNaN(length) || length < 0.0)
			{
				return false;
			}
			double num = length / 2.0;
			Point3d pointOnInterior = point3d;
			pointOnInterior.Z += num * fillet;
			Point3d endPoint = pointAtStart2;
			nurbsCurve = new ArcCurve(new Arc(pointAtStart, pointOnInterior, endPoint)).ToNurbsCurve();
		}
		if (!CSUtilSweepRC.Sweep2(rail1, rail2, new Curve[1] { nurbsCurve }, new double[1] { rail1.Domain.Min }, new double[1] { rail2.Domain.Min }, out var breps, closed: true, maintainHeight: true))
		{
			CS.CSUtil.Print("Failed Sweep2\n");
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		if (!breps.IsValid)
		{
			return false;
		}
		srf = breps;
		return true;
	}

	private static bool MakeBezel(CSGemData gem, NurbsCurve profile, Point3d point, bool bPoint, bool bRail, double girdleSpacing, double placement, double angle, double height, double topThickness, double bottomThickness, double seatAngle, double seatHeight, double seatLength, double fillet, double railRadius, double railOffset, double cornerBlend, out CSOnGeometryData bezel, out string msg)
	{
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
			Point3d point3d = new Point3d(0.0, 0.0, 0.0);
			point3d.Transform(xUp);
			point3d.Y = 0.0;
			height = point3d.DistanceTo(Point3d.Origin) - railRadius - railOffset + seatHeight + placement;
		}
		else if (bPoint)
		{
			Point3d other = new Point3d(0.0, 0.0, 0.0);
			other.Transform(xUp);
			other.Y = 0.0;
			height = point.DistanceTo(other) + seatHeight + placement;
		}
		Transform transform = default(Transform);
		if (!GemOffsetCurveV2.GemOffset(gem, girdleSpacing, out Curve curve, false, out msg, cornerBlend))
		{
			return false;
		}
		transform = Transform.Translation(0.0, 0.0, placement);
		curve.Transform(transform);
		if (!GemOffsetCurveV2.GemOffset(gem, girdleSpacing, out Curve curve2, false, out msg, cornerBlend))
		{
			return false;
		}
		transform = Transform.Translation(0.0, 0.0, seatHeight + placement);
		curve2.Transform(transform);
		if (!GemOffsetCurveV2.GemOffset(gem, girdleSpacing + topThickness, out Curve curve3, false, out msg, cornerBlend))
		{
			return false;
		}
		transform = Transform.Translation(0.0, 0.0, seatHeight + placement);
		curve3.Transform(transform);
		if (!SurfaceBezelOutside(curve3.ToNurbsCurve(), profile, angle, height, out var srf, out var crv))
		{
			return false;
		}
		Curve curve4 = null;
		if (seatLength > 0.0)
		{
			double num = (0.0 - Math.Sin(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			double num2 = (0.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			if (!GemOffsetCurveV2.GemOffset(gem, girdleSpacing + num, out curve4, false, out msg, cornerBlend))
			{
				return false;
			}
			transform = Transform.Translation(0.0, 0.0, num2 + placement);
			curve4.Transform(transform);
		}
		BoundingBox boundingBox = crv.GetBoundingBox(accurate: true);
		double x = boundingBox.Diagonal.X;
		double y = boundingBox.Diagonal.Y;
		CSGemData obj = (CSGemData)gem.Duplicate();
		obj.ScaleToSize(x, y);
		if (!GemOffsetCurveV2.GemOffset(obj, 0.0 - bottomThickness, out Curve curve5, false, out msg, cornerBlend))
		{
			return false;
		}
		transform = Transform.Translation(0.0, 0.0, 0.0 - height + seatHeight + placement);
		curve5.Transform(transform);
		fillet /= 100.0;
		if (!BezelSurfaceTop(curve2, curve3, fillet, out var srf2))
		{
			return false;
		}
		List<Curve> list = new List<Curve>();
		list.Add(crv);
		list.Add(curve5);
		if (curve4 != null)
		{
			list.Add(curve4);
		}
		list.Add(curve);
		list.Add(curve2);
		Brep item = Brep.CreateFromLoft(list.ToArray(), Point3d.Unset, Point3d.Unset, LoftType.Straight, closed: false)[0];
		Brep[] array = Brep.JoinBreps(new List<Brep> { item, srf2, srf }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array == null)
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		array[0].Transform(xUp);
		CSDataHelper.SetSettingType(gem, GemSettingType.BZ);
		CSUtilGeometry.FixBrepSolidOrientation(ref array[0]);
		bezel = new CSOnGeometryData(array[0], xUp, applyTransform: false);
		return true;
	}

	private static bool SurfaceBezelOutside(NurbsCurve rail, NurbsCurve oprofile, double angle, double height, out Brep srf, out NurbsCurve crv)
	{
		srf = null;
		crv = null;
		if (rail == null || oprofile == null)
		{
			return false;
		}
		NurbsCurve nurbsCurve = new NurbsCurve(oprofile);
		Point3d pointAtStart = nurbsCurve.PointAtStart;
		Vector3d motion = new Vector3d(pointAtStart);
		motion.Reverse();
		Transform transform = Transform.Translation(motion);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		nurbsCurve.Transform(transform3 * transform2 * transform);
		Point3d pointAtEnd = nurbsCurve.PointAtEnd;
		double num = CSBlockUtil.CSUtilMath.AngleOfPointD(pointAtEnd.Y, 0.0 - pointAtEnd.Z);
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle) - num, Vector3d.XAxis, Point3d.Origin);
		nurbsCurve.Transform(xform);
		double num2 = 0.0 - nurbsCurve.PointAtEnd.Z;
		double num3 = height / num2;
		Transform xform2 = Transform.Scale(Plane.WorldXY, num3, num3, num3);
		nurbsCurve.Transform(xform2);
		Point3d pointAtStart2 = rail.PointAtStart;
		pointAtStart = nurbsCurve.PointAtStart;
		transform = Transform.Translation(pointAtStart2 - pointAtStart);
		nurbsCurve.Transform(transform);
		NurbsSurface nurbsSurface = CSUtilRailRevolve.RailRevolveRC(nurbsCurve, rail, 0);
		if (nurbsSurface == null)
		{
			return false;
		}
		if (!nurbsSurface.IsValid)
		{
			return false;
		}
		srf = nurbsSurface.ToBrep();
		Point3d pointAtStart3 = nurbsCurve.PointAtStart;
		pointAtStart3.Z = 0.0;
		Point3d pointAtEnd2 = nurbsCurve.PointAtEnd;
		pointAtEnd2.Z = 0.0;
		double num4 = pointAtStart3.DistanceTo(Point3d.Origin);
		num3 = pointAtEnd2.DistanceTo(Point3d.Origin) / num4;
		xform2 = Transform.Scale(Plane.WorldXY, num3, num3, 1.0);
		rail.Transform(xform2);
		transform = Transform.Translation(0.0, 0.0, 0.0 - height);
		rail.Transform(transform);
		crv = (NurbsCurve)rail.Duplicate();
		return true;
	}
}
