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

namespace CSBlocks.Settings.Bezels;

[CSBlock("UnderBezel", "{974D3A0F-2A13-4ABF-8B70-4D31D69C268E}")]
public sealed class CSUnderBezel : CSBlock
{
	public CSInputChannel InGem { get; }

	public CSInputChannel InGirdleSpacing { get; }

	public CSInputChannel InPlacement { get; }

	public CSInputChannel InOutsideProfile { get; }

	public CSInputChannel InAngle { get; }

	public CSInputChannel InHeight { get; }

	public CSInputChannel InBaseThickness { get; }

	public CSInputChannel InTopThickness { get; }

	public CSInputChannel InFillet { get; }

	public CSInputChannel InSeatAngle { get; }

	public CSInputChannel InSeatLength { get; }

	public CSInputChannel InRailRadius { get; }

	public CSInputChannel InRailOffset { get; }

	public CSInputChannel InPoint { get; }

	public CSOutputChannel OutBrep { get; }

	public CSUnderBezel(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOutsideProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBaseThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTopThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSeatAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSeatLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Origin);
		OutBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
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
		CSDataArray gemDatas = InGem.GetFlattenArray();
		if (gemDatas.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		double girdleSpacing = InGirdleSpacing.Double;
		double placement = InPlacement.Double;
		double angle = InAngle.Double;
		double topThickness = InTopThickness.Double;
		double fillet = InFillet.Double;
		fillet /= 100.0;
		double seatAngle = InSeatAngle.Double;
		double seatLength = InSeatLength.Double;
		NurbsCurve profile = InOutsideProfile.NurbsCurve;
		if (profile == null)
		{
			Point3d point3d = new Point3d(0.0, 10.0, 0.0);
			Point3d to = new Point3d(0.0, 0.0, 0.0);
			LineCurve lineCurve = new LineCurve(point3d, to);
			profile = lineCurve.ToNurbsCurve();
			if (profile.IsValid)
			{
				return UpdateError("Unable to create profile");
			}
		}
		double railRadius = InRailRadius.Double;
		double railOffset = InRailOffset.Double;
		bool bRail = Math.Abs(railRadius) > 0.001;
		double height = InHeight.Double;
		Point3d point = new Point3d(Point3d.Origin);
		bool bPoint = point != Point3d.Origin;
		double baseThickness = InBaseThickness.Double;
		CSDataArray cSDataArray = new CSDataArray();
		ParallelOptions parallelOptions = GetBlockParallelOptions();
		int arrayCount = gemDatas.GetArrayCount();
		CSData[] arrUnderBezels = new CSData[arrayCount];
		string error;
		Parallel.For(0, arrayCount, parallelOptions, delegate(int i, ParallelLoopState loopState)
		{
			if (!loopState.ShouldExitCurrentIteration && !parallelOptions.CancellationToken.IsCancellationRequested && !loopState.IsStopped && !loopState.IsExceptional)
			{
				CSOnGeometryData underBezel;
				if (!(gemDatas.GetArrayItem(i) is CSGemData gemData))
				{
					UpdateError("Invalid gem");
					loopState.Stop();
				}
				else if (!MakeUnderBezel(gemData, placement, girdleSpacing, profile, bRail, height, railRadius, railOffset, bPoint, point, seatAngle, seatLength, angle, fillet, baseThickness, topThickness, out underBezel, out error))
				{
					UpdateError(error);
					loopState.Stop();
				}
				else
				{
					arrUnderBezels[i] = underBezel;
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
			if (arrUnderBezels[num] != null)
			{
				cSDataArray.Append(arrUnderBezels[num]);
			}
		}
		OutBrep.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private bool UpdateSync()
	{
		CSDataArray flattenArray = InGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		double girdleSpacing = InGirdleSpacing.Double;
		double placement = InPlacement.Double;
		double angle = InAngle.Double;
		double topThickness = InTopThickness.Double;
		double num = InFillet.Double;
		num /= 100.0;
		double seatAngle = InSeatAngle.Double;
		double seatLength = InSeatLength.Double;
		NurbsCurve nurbsCurve = InOutsideProfile.NurbsCurve;
		if (nurbsCurve == null)
		{
			Point3d point3d = new Point3d(0.0, 10.0, 0.0);
			Point3d to = new Point3d(0.0, 0.0, 0.0);
			nurbsCurve = new LineCurve(point3d, to).ToNurbsCurve();
			if (nurbsCurve.IsValid)
			{
				return UpdateError("Unable to create profile");
			}
		}
		double num2 = InRailRadius.Double;
		double railOffset = InRailOffset.Double;
		bool bRail = Math.Abs(num2) > 0.001;
		double height = InHeight.Double;
		Point3d point3d2 = new Point3d(Point3d.Origin);
		bool bPoint = point3d2 != Point3d.Origin;
		double baseThickness = InBaseThickness.Double;
		CSDataArray cSDataArray = new CSDataArray();
		int arrayCount = flattenArray.GetArrayCount();
		CSData[] array = new CSData[arrayCount];
		for (int i = 0; i < arrayCount; i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData gemData))
			{
				return UpdateError("Invalid gem");
			}
			if (!MakeUnderBezel(gemData, placement, girdleSpacing, nurbsCurve, bRail, height, num2, railOffset, bPoint, point3d2, seatAngle, seatLength, angle, num, baseThickness, topThickness, out var underBezel, out var msg))
			{
				return UpdateError(msg);
			}
			array[i] = underBezel;
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
		OutBrep.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private static bool MakeUnderBezel(CSGemData gemData, double placement, double girdleSpacing, NurbsCurve profile, bool bRail, double height, double railRadius, double railOffset, bool bPoint, Point3d point, double seatAngle, double seatLength, double angle, double fillet, double baseThickness, double topThickness, out CSOnGeometryData underBezel, out string msg)
	{
		msg = string.Empty;
		underBezel = null;
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(gemData.Xform, out var xUp, out var xDown))
		{
			xUp = Transform.Identity;
			xUp.TryGetInverse(out xDown);
		}
		CSDataHelper.GetNurbsCurve(gemData, out var value);
		if (value == null)
		{
			return false;
		}
		value.DuplicateCurve().ToNurbsCurve().Transform(xDown);
		Transform xform = Transform.Translation(0.0, 0.0, placement);
		if (!GemOffsetCurve.GemOffset(gemData, girdleSpacing, out Curve curve, false, out msg, 0.0))
		{
			return false;
		}
		if (curve == null)
		{
			CS.CSUtil.Print("Under Bezel Top Outer Offset Curve Unable to Create Output\n");
			return false;
		}
		curve.Transform(xform);
		NurbsCurve rail = curve.ToNurbsCurve();
		NurbsCurve profile2 = profile.DuplicateCurve().ToNurbsCurve();
		if (bRail)
		{
			Point3d point3d = new Point3d(0.0, 0.0, 0.0);
			point3d.Transform(xUp);
			point3d.Y = 0.0;
			height = point3d.DistanceTo(Point3d.Origin) - railRadius - railOffset + placement;
		}
		else if (bPoint)
		{
			Point3d other = new Point3d(0.0, 0.0, 0.0);
			other.Transform(xUp);
			other.Y = 0.0;
			height = point.DistanceTo(other) + placement;
		}
		NurbsCurve crv = null;
		Brep srf = null;
		if (SurfaceBezelOutside(rail, profile2, angle, height, ref srf, ref crv))
		{
			BoundingBox boundingBox = crv.GetBoundingBox(accurate: true);
			double x = boundingBox.Diagonal.X;
			double y = boundingBox.Diagonal.Y;
			CSGemData obj = (CSGemData)gemData.Duplicate();
			obj.ScaleToSize(x, y);
			if (!GemOffsetCurve.GemOffset(obj, 0.0 - baseThickness, out Curve curve2, false, out msg, 0.0))
			{
				return false;
			}
			Transform xform2 = Transform.Translation(0.0, 0.0, 0.0 - height + placement);
			curve2.Transform(xform2);
			BoundingBox boundingBox2 = curve.ToNurbsCurve().GetBoundingBox(accurate: true);
			x = boundingBox2.Diagonal.X;
			y = boundingBox2.Diagonal.Y;
			CSGemData obj2 = (CSGemData)gemData.Duplicate();
			obj2.ScaleToSize(x, y);
			if (!GemOffsetCurve.GemOffset(obj2, 0.0 - topThickness, out Curve curve3, false, out msg, 0.0))
			{
				return false;
			}
			curve3.Transform(xform);
			NurbsCurve nurbsCurve = curve3.ToNurbsCurve();
			double distance = (0.0 - Math.Sin(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			double num = (0.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			Transform xform3 = Transform.Translation(0.0, 0.0, num + placement);
			BoundingBox boundingBox3 = nurbsCurve.GetBoundingBox(accurate: true);
			x = boundingBox3.Diagonal.X;
			y = boundingBox3.Diagonal.Y;
			CSGemData obj3 = (CSGemData)gemData.Duplicate();
			obj3.ScaleToSize(x, y);
			if (!GemOffsetCurve.GemOffset(obj3, distance, out Curve curve4, false, out msg, 0.0))
			{
				return false;
			}
			if (curve4 == null)
			{
				msg = "Bezel Rail Seat Offset Curve Unable to Create Output";
				return false;
			}
			curve4.Transform(xform3);
			NurbsCurve rail2 = curve.ToNurbsCurve();
			NurbsCurve rail3 = curve3.ToNurbsCurve();
			Brep srf2 = null;
			if (!SurfaceBezelTop(rail2, rail3, fillet, ref srf2))
			{
				return false;
			}
			List<Curve> list = new List<Curve> { crv, curve2 };
			if (seatLength > 0.0)
			{
				list.Add(curve4);
			}
			list.Add(curve3);
			Brep item = Brep.CreateFromLoft(list.ToArray(), Point3d.Unset, Point3d.Unset, LoftType.Straight, closed: false)[0];
			Brep[] array = Brep.JoinBreps(new List<Brep> { item, srf2, srf }.ToArray(), CSApplicationHelper.Application.Tolerance);
			if (array == null)
			{
				msg = "Unable to create Under Bezel";
				return false;
			}
			if (array.Length != 1)
			{
				msg = "Unable to create Under Bezel";
				return false;
			}
			array[0].Transform(xUp);
			CSUtilGeometry.FixBrepSolidOrientation(ref array[0]);
			underBezel = new CSOnGeometryData(array[0], xUp, applyTransform: false);
			return true;
		}
		return false;
	}

	private static bool SurfaceBezelOutside(NurbsCurve rail, NurbsCurve profile, double angle, double height, ref Brep srf, ref NurbsCurve crv)
	{
		if (rail == null || profile == null)
		{
			return false;
		}
		Point3d pointAtStart = profile.PointAtStart;
		Vector3d motion = new Vector3d(pointAtStart);
		motion.Reverse();
		Transform transform = Transform.Translation(motion);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		profile.Transform(transform3 * transform2 * transform);
		Point3d pointAtEnd = profile.PointAtEnd;
		double num = CSBlockUtil.CSUtilMath.AngleOfPointD(pointAtEnd.Y, 0.0 - pointAtEnd.Z);
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle) - num, Vector3d.XAxis, Point3d.Origin);
		profile.Transform(xform);
		double num2 = 0.0 - profile.PointAtEnd.Z;
		double num3 = height / num2;
		Transform xform2 = Transform.Scale(Plane.WorldXY, num3, num3, num3);
		profile.Transform(xform2);
		Point3d pointAtStart2 = rail.PointAtStart;
		pointAtStart = profile.PointAtStart;
		transform = Transform.Translation(pointAtStart2 - pointAtStart);
		profile.Transform(transform);
		NurbsSurface nurbsSurface = CSUtilRailRevolve.RailRevolveRC(profile, rail, 0);
		if (nurbsSurface == null)
		{
			return false;
		}
		if (!nurbsSurface.IsValid)
		{
			return false;
		}
		srf = nurbsSurface.ToBrep();
		Point3d pointAtStart3 = profile.PointAtStart;
		pointAtStart3.Z = 0.0;
		Point3d pointAtEnd2 = profile.PointAtEnd;
		pointAtEnd2.Z = 0.0;
		double num4 = pointAtStart3.DistanceTo(Point3d.Origin);
		num3 = pointAtEnd2.DistanceTo(Point3d.Origin) / num4;
		xform2 = Transform.Scale(Plane.WorldXY, num3, num3, 1.0);
		rail.Transform(xform2);
		transform = Transform.Translation(0.0, 0.0, 0.0 - height);
		rail.Transform(transform);
		crv = rail;
		return true;
	}

	private static bool SurfaceBezelTop(Curve rail1, Curve rail2, double fillet, ref Brep srf)
	{
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
			CS.CSUtil.PrintLine("Failed Sweep2");
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
}
