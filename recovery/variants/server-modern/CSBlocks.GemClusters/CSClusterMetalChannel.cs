using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Rhino.Geometry.Intersect;

namespace CSBlocks.GemClusters;

[CSBlock("ClusterMetalChannel", "{04F8619D-4207-4037-AD6D-BF5EBCE7F821}")]
public sealed class CSClusterMetalChannel : CSBlock
{
	public readonly CSInputChannel InCenterGem;

	public readonly CSInputChannel InCenterGemShapeOverrideEnabled;

	public readonly CSInputChannel InCenterGemShapeOverride;

	public readonly CSInputChannel InCenterGemSpacing;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InProfileWidth;

	public readonly CSInputChannel InProfileHeight;

	public readonly CSInputChannel InChannelWallThicknessInner;

	public readonly CSInputChannel InChannelWallThicknessOuter;

	public readonly CSInputChannel InChannelSpacing;

	public readonly CSInputChannel InChannelWallDepth;

	public readonly CSInputChannel InAngle;

	public readonly CSInputChannel InPlacement;

	public readonly CSInputChannel InCornerBlend;

	public readonly CSInputChannel InGemSizeZ;

	public readonly CSInputChannel InShowChannel;

	public readonly CSInputChannel InXYScaleToGem;

	public readonly CSInputChannel InCenterGemShapeOverrideMode;

	public readonly CSInputChannel InOffsetFix;

	public readonly CSInputChannel InInnerWallExtra;

	public readonly CSInputChannel InOuterWallExtra;

	public readonly CSInputChannel InRailX;

	public readonly CSInputChannel InRailZ;

	public readonly CSOutputChannel OutMetal;

	public CSClusterMetalChannel(string identifier)
		: base(identifier)
	{
		InCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSizeZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest);
		InOffsetFix = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InInnerWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOuterWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutMetal = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		CSGemData gem = InCenterGem.Gem;
		if (gem == null)
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData gem2 = InCenterGemShapeOverride.Gem;
		NurbsCurve nurbsCurve = InProfile.NurbsCurve;
		double num = InCenterGemSpacing.Double;
		double profileWidth = InProfileWidth.Double;
		double profileHeight = InProfileHeight.Double;
		double channelWallThicknessInner = InChannelWallThicknessInner.Double;
		double channelWallThicknessOuter = InChannelWallThicknessOuter.Double;
		double channelWallDepth = InChannelWallDepth.Double;
		double angleDegrees = InAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num2 = InPlacement.Double;
		double cornerBlend = InCornerBlend.Double;
		double gemSizeZ = InGemSizeZ.Double;
		double num3 = InOffsetFix.Double;
		num += num3;
		bool boolean = InCenterGemShapeOverrideEnabled.Boolean;
		bool boolean2 = InShowChannel.Boolean;
		bool boolean3 = InXYScaleToGem.Boolean;
		double innerWallExtra = InInnerWallExtra.Double;
		double outerWallExtra = InOuterWallExtra.Double;
		ValueListItem valueListItem = InCenterGemShapeOverrideMode.ValueListItem;
		double num4 = InRailX.Double;
		num += num4;
		double num5 = InRailZ.Double;
		num2 += num5;
		if (!MakeCluster(gem, boolean, gem2, num, nurbsCurve, profileWidth, profileHeight, channelWallThicknessInner, channelWallThicknessOuter, channelWallDepth, angleDegrees, num2, cornerBlend, gemSizeZ, boolean2, boolean3, valueListItem, innerWallExtra, outerWallExtra, out var outputMetals, out var outputTransforms, out var outError))
		{
			return UpdateError(outError);
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < outputMetals.Count; i++)
		{
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)outputMetals[i], outputTransforms[i], applyTransform: false));
		}
		OutMetal.SetData(cSDataArray);
		return base.Update();
	}

	private static bool CurveIntersectHighest(Curve c1, Curve c2, ref double param)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		CurveIntersections val = Intersection.CurveCurve(c1, c2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val.Count == 0)
		{
			return false;
		}
		double num = -999.0;
		foreach (IntersectionEvent item in val)
		{
			Point3d pointA = item.PointA;
			if (((Point3d)(ref pointA)).Y > num)
			{
				pointA = item.PointA;
				num = ((Point3d)(ref pointA)).Y;
				param = item.ParameterA;
			}
		}
		return true;
	}

	private static bool MakeCluster(CSGemData centerGem, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, NurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double placement, double cornerBlend, double gemSizeZ, bool showChannel, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, double innerWallExtra, double outerWallExtra, out List<Brep> outputMetals, out List<Transform> outputTransforms, out string outError)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a32: Unknown result type (might be due to invalid IL or missing references)
		outputMetals = new List<Brep>();
		outputTransforms = new List<Transform>();
		BoundingBox boundingBox = ((GeometryBase)profile).GetBoundingBox(true);
		Point3d val = ((BoundingBox)(ref boundingBox)).Min;
		double num = 0.0 - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		Transform val2 = Transform.Translation(num, 0.0 - ((Point3d)(ref val)).Y, 0.0);
		Plane worldXY = Plane.WorldXY;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = 1.0 / (x - ((Point3d)(ref val)).X);
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		Transform val3 = Transform.Scale(worldXY, num2, 1.0 / (y - ((Point3d)(ref val)).Y), 0.0);
		Transform val4 = Transform.Scale(Plane.WorldXY, profileWidth + innerWallExtra + outerWallExtra, 1.0, 1.0);
		Transform val5 = Transform.Translation(0.0 - innerWallExtra, 0.0, 0.0);
		((GeometryBase)profile).Transform(val5 * val4 * val3 * val2);
		Curve val12;
		if (showChannel)
		{
			LineCurve c = new LineCurve(new Point3d(channelWallThicknessInner, -1.0, 0.0), new Point3d(channelWallThicknessInner, 2.0, 0.0));
			LineCurve c2 = new LineCurve(new Point3d(profileWidth - channelWallThicknessOuter, -1.0, 0.0), new Point3d(profileWidth - channelWallThicknessOuter, 2.0, 0.0));
			double param = 0.0;
			double param2 = 0.0;
			if (!CurveIntersectHighest((Curve)(object)profile, (Curve)(object)c, ref param))
			{
				outError = "Intersect for channel Inner Failed";
				return false;
			}
			if (!CurveIntersectHighest((Curve)(object)profile, (Curve)(object)c2, ref param2))
			{
				outError = "Intersect for channel Failed";
				return false;
			}
			Point3d val6 = ((Curve)profile).PointAt(param);
			Point3d val7 = ((Curve)profile).PointAt(param2);
			double num3 = ((((Point3d)(ref val6)).Y > ((Point3d)(ref val7)).Y) ? ((Point3d)(ref val6)).Y : ((Point3d)(ref val7)).Y);
			Transform val8 = Transform.Scale(Plane.WorldXY, 1.0, profileHeight / num3, 1.0);
			((GeometryBase)profile).Transform(val8);
			Curve val9 = ((Curve)profile).DuplicateCurve();
			Curve obj = val9;
			Interval domain = ((Curve)profile).Domain;
			val9 = obj.Trim(new Interval(((Interval)(ref domain)).Min, param));
			Curve val10 = ((Curve)profile).DuplicateCurve();
			Curve obj2 = val10;
			double num4 = param2;
			domain = ((Curve)profile).Domain;
			val10 = obj2.Trim(new Interval(num4, ((Interval)(ref domain)).Max));
			double num5 = channelWallThicknessInner + (profileWidth - channelWallThicknessInner - channelWallThicknessOuter) / 2.0;
			double num6 = profileHeight - gemSizeZ;
			Point3d val11 = default(Point3d);
			((Point3d)(ref val11))._002Ector(num5, num6, 0.0);
			Curve crv5;
			Curve crv6;
			if (channelWallDepth > cornerBlend * 2.0)
			{
				Point3d pointAtEnd = val9.PointAtEnd;
				Curve crv = (Curve)new LineCurve(pointAtEnd, new Point3d(((Point3d)(ref pointAtEnd)).X, ((Point3d)(ref pointAtEnd)).Y - channelWallDepth, 0.0));
				Curve crv2 = (Curve)new LineCurve(new Point3d(((Point3d)(ref pointAtEnd)).X, ((Point3d)(ref pointAtEnd)).Y - channelWallDepth, 0.0), val11);
				CSUtilGeometry.TrimEndOfCrv(ref crv, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(ref crv2, cornerBlend);
				Curve item = (Curve)(object)CSUtilCurves.BlendCurve(crv, crv2);
				Point3d pointAtStart = val10.PointAtStart;
				Curve crv3 = (Curve)new LineCurve(val11, new Point3d(((Point3d)(ref pointAtStart)).X, ((Point3d)(ref pointAtStart)).Y - channelWallDepth, 0.0));
				Curve crv4 = (Curve)new LineCurve(new Point3d(((Point3d)(ref pointAtStart)).X, ((Point3d)(ref pointAtStart)).Y - channelWallDepth, 0.0), pointAtStart);
				CSUtilGeometry.TrimEndOfCrv(ref crv3, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(ref crv4, cornerBlend);
				Curve item2 = (Curve)(object)CSUtilCurves.BlendCurve(crv3, crv4);
				CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { crv, item, crv2 }, out Curve[] mergedCrvs, CSApplicationHelper.Application.Tolerance);
				crv5 = mergedCrvs[0];
				CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { crv3, item2, crv4 }, out mergedCrvs, CSApplicationHelper.Application.Tolerance);
				crv6 = mergedCrvs[0];
			}
			else
			{
				crv5 = (Curve)new LineCurve(val9.PointAtEnd, val11);
				crv6 = (Curve)new LineCurve(val11, val10.PointAtStart);
			}
			CSUtilGeometry.TrimEndOfCrv(ref val9, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(ref crv5, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(ref crv6, cornerBlend);
			CSUtilGeometry.TrimStartOfCrv(ref val10, cornerBlend);
			Curve item3 = (Curve)(object)CSUtilCurves.BlendCurve(val9, crv5);
			Curve item4 = (Curve)(object)CSUtilCurves.BlendCurve(crv5, crv6);
			Curve item5 = (Curve)(object)CSUtilCurves.BlendCurve(crv6, val10);
			if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { val9, item3, crv5, item4, crv6, item5, val10 }, out Curve[] mergedCrvs2, CSApplicationHelper.Application.Tolerance))
			{
				outError = "channel Merge failed";
				return false;
			}
			if (mergedCrvs2.Length != 1)
			{
				outError = "channel Merge failed";
				return false;
			}
			val12 = mergedCrvs2[0];
		}
		else
		{
			Transform val13 = Transform.Scale(Plane.WorldXY, 1.0, profileHeight, 1.0);
			((GeometryBase)profile).Transform(val13);
			val12 = (Curve)(object)profile;
		}
		Transform val14 = Transform.Translation(0.0, 0.0 - profileHeight, 0.0);
		Transform val15 = Transform.Rotation(angle, Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val12).Transform(val15 * val14);
		string gemShape;
		if (centerGemShapeOverrideEnabled)
		{
			if (centerGemShapeOverride == null)
			{
				centerGemShapeOverride = new CSGemData(centerGem);
			}
			else
			{
				double scaleFactorX;
				switch (centerGemShapeOverrideMode)
				{
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
					scaleFactorX = centerGem.GemXSize / centerGemShapeOverride.GemXSize;
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
					scaleFactorX = centerGem.GemYSize / centerGemShapeOverride.GemYSize;
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
					scaleFactorX = ((!(centerGem.GemXSize > centerGem.GemYSize)) ? (centerGem.GemYSize / centerGemShapeOverride.GemYSize) : (centerGem.GemXSize / centerGemShapeOverride.GemXSize));
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
					scaleFactorX = ((!(centerGem.GemXSize < centerGem.GemYSize)) ? (centerGem.GemYSize / centerGemShapeOverride.GemYSize) : (centerGem.GemXSize / centerGemShapeOverride.GemXSize));
					break;
				default:
					outError = "Invalid Gem Cluster Center Shape Override Mode";
					return false;
				}
				centerGemShapeOverride.Scale(scaleFactorX);
			}
			gemShape = centerGemShapeOverride.GemShape;
		}
		else
		{
			centerGemShapeOverride = new CSGemData(centerGem);
			gemShape = centerGem.GemShape;
		}
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(centerGem.Xform, out var xUp, out var xDown))
		{
			outError = "Gem X form";
			return false;
		}
		CSGemData cSGemData = new CSGemData(centerGem);
		cSGemData.ApplyTransform(xDown);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox2))
		{
			outError = "Gem Invalid Bounding Box";
			return false;
		}
		Vector3d up = cSGemData.Up;
		((Vector3d)(ref up)).Unitize();
		if (!GemOffsetCurve.GemOffset(gemShape, centerGemShapeOverride, centerGemSpacing, out Curve curve, false, out string error, 0.0, 0.0, 0.0))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		NurbsCurve val16 = curve.ToNurbsCurve();
		NurbsCurve val17 = val12.ToNurbsCurve();
		NurbsSurface val18 = NurbsSurface.Create(3, true, val17.Order, val16.Order, val17.Points.Count, val16.Points.Count);
		bool flag = false;
		List<Curve> list = new List<Curve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		for (int i = 0; i < val17.Points.Count; i++)
		{
			ControlPoint val19 = val17.Points[i];
			string shape = gemShape;
			val = ((ControlPoint)(ref val19)).Location;
			if (!GemOffsetCurve.GemOffset(shape, gem, centerGemSpacing + ((Point3d)(ref val)).X, out curve, false, out error, 0.0, 0.0, 0.0))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			Curve obj3 = curve;
			Vector3d val20 = up;
			val = ((ControlPoint)(ref val19)).Location;
			((GeometryBase)obj3).Translate(val20 * (placement + ((Point3d)(ref val)).Y));
			list.Add(curve);
			NurbsCurve val21 = curve.ToNurbsCurve();
			if (val16.Points.Count != val21.Points.Count)
			{
				flag = true;
			}
			if (!flag)
			{
				for (int j = 0; j < val16.Points.Count; j++)
				{
					ControlPoint val22 = val21.Points[j];
					ControlPoint val23 = val17.Points[i];
					double weight = ((ControlPoint)(ref val23)).Weight;
					val23 = val21.Points[j];
					double num7 = weight * ((ControlPoint)(ref val23)).Weight;
					NurbsSurfacePointList points = val18.Points;
					int num8 = i;
					int num9 = j;
					val = ((ControlPoint)(ref val22)).Location;
					double x2 = ((Point3d)(ref val)).X;
					val = ((ControlPoint)(ref val22)).Location;
					double y2 = ((Point3d)(ref val)).Y;
					val = ((ControlPoint)(ref val22)).Location;
					points.SetControlPoint(num8, num9, new ControlPoint(x2, y2, ((Point3d)(ref val)).Z, num7));
				}
			}
		}
		if (!flag)
		{
			for (int k = 0; k < val17.Knots.Count; k++)
			{
				val18.KnotsU[k] = val17.Knots[k];
			}
			for (int l = 0; l < val16.Knots.Count; l++)
			{
				val18.KnotsV[l] = val16.Knots[l];
			}
			Brep brep = ((Surface)val18).ToBrep();
			((GeometryBase)brep).Transform(xUp);
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			if (!brep.IsSolid)
			{
				Brep val24 = brep.DuplicateBrep();
				val24.JoinNakedEdges(CSApplicationHelper.Application.Tolerance);
				if (!val24.IsSolid)
				{
					val24.Flip();
				}
				brep = val24;
			}
			outputMetals.Add(brep);
			outputTransforms.Add(xUp);
		}
		else
		{
			Brep brep2 = CSUtilSweepRC.Loft(list, (LoftType)1);
			if (brep2 != null)
			{
				((GeometryBase)brep2).Transform(xUp);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep2);
				outputMetals.Add(brep2);
				outputTransforms.Add(xUp);
			}
		}
		outError = string.Empty;
		return true;
	}
}
