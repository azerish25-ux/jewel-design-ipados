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
[CSBlock("Bezel_V2", "{C2D4CE05-41DF-424B-8B0D-9A21EFEE6113}")]
public sealed class CSBezel2 : CSBlock
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

	public CSBezel2(string identifier)
		: base(identifier)
	{
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
		InitializeCancellationTokenSource();
		List<CSGemData> gemDatas = InGems.GemList;
		if (gemDatas.Count <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		NurbsCurve profile = InProfile.NurbsCurve;
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
		List<CSGemData> gemList = InGems.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		NurbsCurve nurbsCurve = InProfile.NurbsCurve;
		if (nurbsCurve == null)
		{
			Point3d point3d = new Point3d(0.0, 10.0, 0.0);
			Point3d to = new Point3d(0.0, 0.0, 0.0);
			nurbsCurve = new LineCurve(point3d, to).ToNurbsCurve();
			if (nurbsCurve == null)
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
		Point3d point3d2 = InPoint.Point3d;
		bool bPoint = point3d2 != Point3d.Origin;
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
			if (!MakeBezel(cSGemData, nurbsCurve, point3d2, bPoint, bRail, girdleSpacing, placement, angle, height, topThickness, bottomThickness, seatAngle, seatHeight, seatLength, fillet, num, railOffset, cornerBlend, out var bezel, out var msg))
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
		if (!GemOffsetCurve.GemOffset(gem, girdleSpacing, out Curve curve, false, out msg, cornerBlend))
		{
			return false;
		}
		Transform xform = Transform.Translation(0.0, 0.0, placement);
		curve.Transform(xform);
		if (!GemOffsetCurve.GemOffset(gem, girdleSpacing, out Curve curve2, false, out msg, cornerBlend))
		{
			return false;
		}
		xform = Transform.Translation(0.0, 0.0, seatHeight + placement);
		curve2.Transform(xform);
		if (!GemOffsetCurve.GemOffset(gem, girdleSpacing + topThickness, out Curve curve3, false, out msg, cornerBlend))
		{
			return false;
		}
		xform = Transform.Translation(0.0, 0.0, seatHeight + placement);
		curve3.Transform(xform);
		if (!SurfaceBezelOutside(curve3.ToNurbsCurve(), profile, angle, height, out var srf, out var crv))
		{
			return false;
		}
		Curve curve4 = null;
		if (seatLength > 0.0)
		{
			double num = (0.0 - Math.Sin(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			double num2 = (0.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			if (!GemOffsetCurve.GemOffset(gem, girdleSpacing + num, out curve4, false, out msg, cornerBlend))
			{
				return false;
			}
			xform = Transform.Translation(0.0, 0.0, num2 + placement);
			curve4.Transform(xform);
		}
		BoundingBox boundingBox = crv.GetBoundingBox(accurate: true);
		double x = boundingBox.Diagonal.X;
		double y = boundingBox.Diagonal.Y;
		CSGemData obj = (CSGemData)gem.Duplicate();
		obj.ScaleToSize(x, y);
		if (!GemOffsetCurve.GemOffset(obj, 0.0 - bottomThickness, out Curve curve5, false, out msg, cornerBlend))
		{
			return false;
		}
		xform = Transform.Translation(0.0, 0.0, 0.0 - height + seatHeight + placement);
		curve5.Transform(xform);
		fillet /= 100.0;
		if (!BezelSurfaceTop(curve2, curve3, fillet, out var srf2))
		{
			return false;
		}
		List<Curve> list = new List<Curve> { crv, curve5 };
		if (curve4 != null)
		{
			list.Add(curve4);
		}
		list.Add(curve);
		list.Add(curve2);
		Brep item = Brep.CreateFromLoft(list.ToArray(), Point3d.Unset, Point3d.Unset, LoftType.Straight, closed: false)[0];
		Brep[] array = Brep.JoinBreps(new List<Brep> { item, srf2, srf }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array == null || array.Length != 1)
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
