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
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
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
			Point3d val = new Point3d(0.0, 10.0, 0.0);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(0.0, 0.0, 0.0);
			LineCurve val3 = new LineCurve(val, val2);
			profile = ((Curve)val3).ToNurbsCurve();
			if (((CommonObject)profile).IsValid)
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
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
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
			Point3d val = new Point3d(0.0, 10.0, 0.0);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(0.0, 0.0, 0.0);
			value8 = ((Curve)new LineCurve(val, val2)).ToNurbsCurve();
			if (((CommonObject)value8).IsValid)
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
		Point3d value12 = default(Point3d);
		((Point3d)(ref value12))._002Ector(Point3d.Origin);
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		msg = string.Empty;
		underBezel = null;
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(gemData.Xform, out var xUp, out var xDown))
		{
			xUp = Transform.Identity;
			((Transform)(ref xUp)).TryGetInverse(ref xDown);
		}
		CSDataHelper.GetNurbsCurve(gemData, out var value);
		if (value == null)
		{
			return false;
		}
		((GeometryBase)((Curve)value).DuplicateCurve().ToNurbsCurve()).Transform(xDown);
		Transform val = Transform.Translation(0.0, 0.0, placement);
		if (!GemOffsetCurveV2.GemOffset(gemData, girdleSpacing, out Curve curve, doTransformUp: false, out msg, 0.0))
		{
			return false;
		}
		if (curve == null)
		{
			CS.CSUtil.Print("Under Bezel Top Outer Offset Curve Unable to Create Output\n");
			return false;
		}
		((GeometryBase)curve).Transform(val);
		NurbsCurve rail = curve.ToNurbsCurve();
		NurbsCurve profile2 = ((Curve)profile).DuplicateCurve().ToNurbsCurve();
		if (bRail)
		{
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(0.0, 0.0, 0.0);
			((Point3d)(ref val2)).Transform(xUp);
			((Point3d)(ref val2)).Y = 0.0;
			height = ((Point3d)(ref val2)).DistanceTo(Point3d.Origin) - railRadius - railOffset + placement;
		}
		else if (bPoint)
		{
			Point3d val3 = default(Point3d);
			((Point3d)(ref val3))._002Ector(0.0, 0.0, 0.0);
			((Point3d)(ref val3)).Transform(xUp);
			((Point3d)(ref val3)).Y = 0.0;
			height = ((Point3d)(ref point)).DistanceTo(val3) + placement;
		}
		NurbsCurve crv = null;
		Brep srf = null;
		if (SurfaceBezelOutside(rail, profile2, angle, height, ref srf, ref crv))
		{
			BoundingBox boundingBox = ((GeometryBase)crv).GetBoundingBox(true);
			Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double x = ((Vector3d)(ref diagonal)).X;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double y = ((Vector3d)(ref diagonal)).Y;
			CSGemData obj = (CSGemData)gemData.Duplicate();
			obj.ScaleToSize(x, y);
			if (!GemOffsetCurveV2.GemOffset(obj, 0.0 - baseThickness, out Curve curve2, doTransformUp: false, out msg, 0.0))
			{
				return false;
			}
			Transform val4 = Transform.Translation(0.0, 0.0, 0.0 - height + placement);
			((GeometryBase)curve2).Transform(val4);
			BoundingBox boundingBox2 = ((GeometryBase)curve.ToNurbsCurve()).GetBoundingBox(true);
			diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
			x = ((Vector3d)(ref diagonal)).X;
			diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
			y = ((Vector3d)(ref diagonal)).Y;
			CSGemData obj2 = (CSGemData)gemData.Duplicate();
			obj2.ScaleToSize(x, y);
			if (!GemOffsetCurveV2.GemOffset(obj2, 0.0 - topThickness, out Curve curve3, doTransformUp: false, out msg, 0.0))
			{
				return false;
			}
			((GeometryBase)curve3).Transform(val);
			NurbsCurve obj3 = curve3.ToNurbsCurve();
			double distance = (0.0 - Math.Sin(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			double num = (0.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			Transform val5 = Transform.Translation(0.0, 0.0, num + placement);
			BoundingBox boundingBox3 = ((GeometryBase)obj3).GetBoundingBox(true);
			diagonal = ((BoundingBox)(ref boundingBox3)).Diagonal;
			x = ((Vector3d)(ref diagonal)).X;
			diagonal = ((BoundingBox)(ref boundingBox3)).Diagonal;
			y = ((Vector3d)(ref diagonal)).Y;
			CSGemData obj4 = (CSGemData)gemData.Duplicate();
			obj4.ScaleToSize(x, y);
			if (!GemOffsetCurveV2.GemOffset(obj4, distance, out Curve curve4, doTransformUp: false, out msg, 0.0))
			{
				return false;
			}
			if (curve4 == null)
			{
				msg = "Bezel Rail Seat Offset Curve Unable to Create Output";
				return false;
			}
			((GeometryBase)curve4).Transform(val5);
			NurbsCurve rail2 = curve.ToNurbsCurve();
			NurbsCurve rail3 = curve3.ToNurbsCurve();
			Brep srf2 = null;
			if (!SurfaceBezelTop(rail2, rail3, fillet, ref srf2))
			{
				return false;
			}
			List<Curve> list = new List<Curve>();
			list.Add((Curve)(object)crv);
			list.Add(curve2);
			if (seatLength > 0.0)
			{
				list.Add(curve4);
			}
			list.Add(curve3);
			Brep item = Brep.CreateFromLoft((IEnumerable<Curve>)list.ToArray(), Point3d.Unset, Point3d.Unset, (LoftType)3, false)[0];
			Brep[] array = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { item, srf2, srf }.ToArray(), CSApplicationHelper.Application.Tolerance);
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
			((GeometryBase)array[0]).Transform(xUp);
			CSDataHelper.SetSettingType(gemData, GemSettingType.BZ);
			CSUtilGeometry.FixBrepSolidOrientation(ref array[0]);
			underBezel = new CSOnGeometryData((GeometryBase)(object)array[0], xUp, applyTransform: false);
			return true;
		}
		return false;
	}

	private static bool SurfaceBezelOutside(NurbsCurve rail, NurbsCurve profile, double angle, double height, ref Brep srf, ref NurbsCurve crv)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		if (rail == null || profile == null)
		{
			return false;
		}
		Point3d pointAtStart = ((Curve)profile).PointAtStart;
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(pointAtStart);
		((Vector3d)(ref val)).Reverse();
		Transform val2 = Transform.Translation(val);
		Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
		Transform val4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)profile).Transform(val4 * val3 * val2);
		Point3d pointAtEnd = ((Curve)profile).PointAtEnd;
		double num = CSBlockUtil.CSUtilMath.AngleOfPointD(((Point3d)(ref pointAtEnd)).Y, 0.0 - ((Point3d)(ref pointAtEnd)).Z);
		Transform val5 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle) - num, Vector3d.XAxis, Point3d.Origin);
		((GeometryBase)profile).Transform(val5);
		Point3d pointAtEnd2 = ((Curve)profile).PointAtEnd;
		double num2 = 0.0 - ((Point3d)(ref pointAtEnd2)).Z;
		double num3 = height / num2;
		Transform val6 = Transform.Scale(Plane.WorldXY, num3, num3, num3);
		((GeometryBase)profile).Transform(val6);
		Point3d pointAtStart2 = ((Curve)rail).PointAtStart;
		pointAtStart = ((Curve)profile).PointAtStart;
		val2 = Transform.Translation(pointAtStart2 - pointAtStart);
		((GeometryBase)profile).Transform(val2);
		NurbsSurface val7 = CSUtilRailRevolve.RailRevolveRC(profile, rail, 0);
		if (val7 == null)
		{
			return false;
		}
		if (!((CommonObject)val7).IsValid)
		{
			return false;
		}
		srf = ((Surface)val7).ToBrep();
		Point3d pointAtStart3 = ((Curve)profile).PointAtStart;
		((Point3d)(ref pointAtStart3)).Z = 0.0;
		Point3d pointAtEnd3 = ((Curve)profile).PointAtEnd;
		((Point3d)(ref pointAtEnd3)).Z = 0.0;
		double num4 = ((Point3d)(ref pointAtStart3)).DistanceTo(Point3d.Origin);
		num3 = ((Point3d)(ref pointAtEnd3)).DistanceTo(Point3d.Origin) / num4;
		val6 = Transform.Scale(Plane.WorldXY, num3, num3, 1.0);
		((GeometryBase)rail).Transform(val6);
		val2 = Transform.Translation(0.0, 0.0, 0.0 - height);
		((GeometryBase)rail).Transform(val2);
		crv = rail;
		return true;
	}

	private static bool SurfaceBezelTop(NurbsCurve rail1, NurbsCurve rail2, double fillet, ref Brep srf)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		if (rail1 == null || rail2 == null)
		{
			return false;
		}
		Point3d pointAtStart = ((Curve)rail1).PointAtStart;
		Point3d pointAtStart2 = ((Curve)rail2).PointAtStart;
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
		domain = ((Curve)rail1).Domain;
		array[0] = ((Interval)(ref domain)).Min;
		double[] array2 = new double[1];
		domain = ((Curve)rail2).Domain;
		array2[0] = ((Interval)(ref domain)).Min;
		if (!CSUtilSweepRC.Sweep2((Curve)(object)rail1, (Curve)(object)rail2, profile, array, array2, out var breps, closed: true, maintainHeight: true))
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
}
