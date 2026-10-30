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
[CSBlock("UnderBezel_V1", "{311421E3-B1E7-4210-AEA9-CB10F28E7275}")]
public sealed class CSUnderBezel1 : CSBlock
{
	public readonly CSOutputChannel oBrep;

	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iGirdleSpacing;

	public readonly CSInputChannel iPlacement;

	public readonly CSInputChannel iOutsideProfile;

	public readonly CSInputChannel iAngle;

	public readonly CSInputChannel iHeight;

	public readonly CSInputChannel iBaseThickness;

	public readonly CSInputChannel iTopThickness;

	public readonly CSInputChannel iFillet;

	public readonly CSInputChannel iSeatAngle;

	public readonly CSInputChannel iSeatLength;

	public readonly CSInputChannel iRailRadius;

	public readonly CSInputChannel iRailOffset;

	public readonly CSInputChannel iPoint;

	public CSUnderBezel1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBaseThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSeatAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSeatLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
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
		CSDataArray gemDatas = iGem.GetFlattenArray();
		if (gemDatas.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		double girdleSpacing = 0.0;
		iGirdleSpacing.GetDouble(ref girdleSpacing);
		double placement = 0.0;
		iPlacement.GetDouble(ref placement);
		double angle = 0.0;
		iAngle.GetDouble(ref angle);
		double topThickness = 0.0;
		iTopThickness.GetDouble(ref topThickness);
		double fillet = 0.0;
		iFillet.GetDouble(ref fillet);
		fillet /= 100.0;
		double seatAngle = 0.0;
		iSeatAngle.GetDouble(ref seatAngle);
		double seatLength = 0.0;
		iSeatLength.GetDouble(ref seatLength);
		CSDataHelper.GetNurbsCurve(iOutsideProfile.Data, out var profile);
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
		double railRadius = 0.0;
		double railOffset = 0.0;
		bool bRail = iRailRadius.GetDouble(ref railRadius);
		if (bRail)
		{
			iRailOffset.GetDouble(ref railOffset);
		}
		double height = 0.0;
		iHeight.GetDouble(ref height);
		Point3d point = new Point3d(Point3d.Origin);
		bool bPoint = CSDataHelper.GetPoint(iPoint.Data, out point);
		double baseThickness = 0.0;
		iBaseThickness.GetDouble(ref baseThickness);
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
		if (cSDataArray.GetArrayCount() == 1)
		{
			oBrep.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oBrep.SetData(cSDataArray);
		}
		return base.Update();
	}

	private bool UpdateSync()
	{
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing input gems");
		}
		double value = 0.0;
		iGirdleSpacing.GetDouble(ref value);
		double value2 = 0.0;
		iPlacement.GetDouble(ref value2);
		double value3 = 0.0;
		iAngle.GetDouble(ref value3);
		double value4 = 0.0;
		iTopThickness.GetDouble(ref value4);
		double value5 = 0.0;
		iFillet.GetDouble(ref value5);
		value5 /= 100.0;
		double value6 = 0.0;
		iSeatAngle.GetDouble(ref value6);
		double value7 = 0.0;
		iSeatLength.GetDouble(ref value7);
		CSDataHelper.GetNurbsCurve(iOutsideProfile.Data, out var value8);
		if (value8 == null)
		{
			Point3d point3d = new Point3d(0.0, 10.0, 0.0);
			Point3d to = new Point3d(0.0, 0.0, 0.0);
			value8 = new LineCurve(point3d, to).ToNurbsCurve();
			if (value8.IsValid)
			{
				return UpdateError("Unable to create profile");
			}
		}
		double value9 = 0.0;
		double value10 = 0.0;
		bool flag = iRailRadius.GetDouble(ref value9);
		if (flag)
		{
			iRailOffset.GetDouble(ref value10);
		}
		double value11 = 0.0;
		iHeight.GetDouble(ref value11);
		Point3d value12 = new Point3d(Point3d.Origin);
		bool point = CSDataHelper.GetPoint(iPoint.Data, out value12);
		double value13 = 0.0;
		iBaseThickness.GetDouble(ref value13);
		CSDataArray cSDataArray = new CSDataArray();
		int arrayCount = flattenArray.GetArrayCount();
		CSData[] array = new CSData[arrayCount];
		for (int i = 0; i < arrayCount; i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData gemData))
			{
				return UpdateError("Invalid gem");
			}
			if (!MakeUnderBezel(gemData, value2, value, value8, flag, value11, value9, value10, point, value12, value6, value7, value3, value5, value13, value4, out var underBezel, out var msg))
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
		if (cSDataArray.GetArrayCount() == 1)
		{
			oBrep.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oBrep.SetData(cSDataArray);
		}
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
		if (!GemOffsetCurveV2.GemOffset(gemData, girdleSpacing, out Curve curve, false, out msg, 0.0))
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
			if (!GemOffsetCurveV2.GemOffset(obj, 0.0 - baseThickness, out Curve curve2, false, out msg, 0.0))
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
			if (!GemOffsetCurveV2.GemOffset(obj2, 0.0 - topThickness, out Curve curve3, false, out msg, 0.0))
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
			if (!GemOffsetCurveV2.GemOffset(obj3, distance, out Curve curve4, false, out msg, 0.0))
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
			List<Curve> list = new List<Curve>();
			list.Add(crv);
			list.Add(curve2);
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
			CSDataHelper.SetSettingType(gemData, GemSettingType.BZ);
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

	private static bool SurfaceBezelTop(NurbsCurve rail1, NurbsCurve rail2, double fillet, ref Brep srf)
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
}
