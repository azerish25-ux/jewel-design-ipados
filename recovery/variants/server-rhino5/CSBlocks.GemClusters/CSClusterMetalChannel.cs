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
			cSDataArray.Append(new CSOnGeometryData(outputMetals[i], outputTransforms[i], applyTransform: false));
		}
		OutMetal.SetData(cSDataArray);
		return base.Update();
	}

	private static bool CurveIntersectHighest(Curve c1, Curve c2, ref double param)
	{
		CurveIntersections curveIntersections = Intersection.CurveCurve(c1, c2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections.Count == 0)
		{
			return false;
		}
		double num = -999.0;
		foreach (IntersectionEvent item in curveIntersections)
		{
			if (item.PointA.Y > num)
			{
				num = item.PointA.Y;
				param = item.ParameterA;
			}
		}
		return true;
	}

	private static bool MakeCluster(CSGemData centerGem, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, NurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double placement, double cornerBlend, double gemSizeZ, bool showChannel, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, double innerWallExtra, double outerWallExtra, out List<Brep> outputMetals, out List<Transform> outputTransforms, out string outError)
	{
		outputMetals = new List<Brep>();
		outputTransforms = new List<Transform>();
		BoundingBox boundingBox = profile.GetBoundingBox(accurate: true);
		Transform transform = Transform.Translation(0.0 - boundingBox.Min.X, 0.0 - boundingBox.Min.Y, 0.0);
		Transform transform2 = Transform.Scale(Plane.WorldXY, 1.0 / (boundingBox.Max.X - boundingBox.Min.X), 1.0 / (boundingBox.Max.Y - boundingBox.Min.Y), 0.0);
		Transform transform3 = Transform.Scale(Plane.WorldXY, profileWidth + innerWallExtra + outerWallExtra, 1.0, 1.0);
		Transform transform4 = Transform.Translation(0.0 - innerWallExtra, 0.0, 0.0);
		profile.Transform(transform4 * transform3 * transform2 * transform);
		Curve curve3;
		if (showChannel)
		{
			LineCurve c = new LineCurve(new Point3d(channelWallThicknessInner, -1.0, 0.0), new Point3d(channelWallThicknessInner, 2.0, 0.0));
			LineCurve c2 = new LineCurve(new Point3d(profileWidth - channelWallThicknessOuter, -1.0, 0.0), new Point3d(profileWidth - channelWallThicknessOuter, 2.0, 0.0));
			double param = 0.0;
			double param2 = 0.0;
			if (!CurveIntersectHighest(profile, c, ref param))
			{
				outError = "Intersect for channel Inner Failed";
				return false;
			}
			if (!CurveIntersectHighest(profile, c2, ref param2))
			{
				outError = "Intersect for channel Failed";
				return false;
			}
			Point3d point3d = profile.PointAt(param);
			Point3d point3d2 = profile.PointAt(param2);
			double num = ((point3d.Y > point3d2.Y) ? point3d.Y : point3d2.Y);
			Transform xform = Transform.Scale(Plane.WorldXY, 1.0, profileHeight / num, 1.0);
			profile.Transform(xform);
			Curve curve = profile.DuplicateCurve();
			curve = curve.Trim(new Interval(profile.Domain.Min, param));
			Curve curve2 = profile.DuplicateCurve();
			curve2 = curve2.Trim(new Interval(param2, profile.Domain.Max));
			double x = channelWallThicknessInner + (profileWidth - channelWallThicknessInner - channelWallThicknessOuter) / 2.0;
			double y = profileHeight - gemSizeZ;
			Point3d point3d3 = new Point3d(x, y, 0.0);
			Curve crv5;
			Curve crv6;
			if (channelWallDepth > cornerBlend * 2.0)
			{
				Point3d pointAtEnd = curve.PointAtEnd;
				Curve crv = new LineCurve(pointAtEnd, new Point3d(pointAtEnd.X, pointAtEnd.Y - channelWallDepth, 0.0));
				Curve crv2 = new LineCurve(new Point3d(pointAtEnd.X, pointAtEnd.Y - channelWallDepth, 0.0), point3d3);
				CSUtilGeometry.TrimEndOfCrv(ref crv, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(ref crv2, cornerBlend);
				Curve item = CSUtilCurves.BlendCurve(crv, crv2);
				Point3d pointAtStart = curve2.PointAtStart;
				Curve crv3 = new LineCurve(point3d3, new Point3d(pointAtStart.X, pointAtStart.Y - channelWallDepth, 0.0));
				Curve crv4 = new LineCurve(new Point3d(pointAtStart.X, pointAtStart.Y - channelWallDepth, 0.0), pointAtStart);
				CSUtilGeometry.TrimEndOfCrv(ref crv3, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(ref crv4, cornerBlend);
				Curve item2 = CSUtilCurves.BlendCurve(crv3, crv4);
				CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { crv, item, crv2 }, out Curve[] mergedCrvs, CSApplicationHelper.Application.Tolerance);
				crv5 = mergedCrvs[0];
				CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { crv3, item2, crv4 }, out mergedCrvs, CSApplicationHelper.Application.Tolerance);
				crv6 = mergedCrvs[0];
			}
			else
			{
				crv5 = new LineCurve(curve.PointAtEnd, point3d3);
				crv6 = new LineCurve(point3d3, curve2.PointAtStart);
			}
			CSUtilGeometry.TrimEndOfCrv(ref curve, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(ref crv5, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(ref crv6, cornerBlend);
			CSUtilGeometry.TrimStartOfCrv(ref curve2, cornerBlend);
			Curve item3 = CSUtilCurves.BlendCurve(curve, crv5);
			Curve item4 = CSUtilCurves.BlendCurve(crv5, crv6);
			Curve item5 = CSUtilCurves.BlendCurve(crv6, curve2);
			if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { curve, item3, crv5, item4, crv6, item5, curve2 }, out Curve[] mergedCrvs2, CSApplicationHelper.Application.Tolerance))
			{
				outError = "channel Merge failed";
				return false;
			}
			if (mergedCrvs2.Length != 1)
			{
				outError = "channel Merge failed";
				return false;
			}
			curve3 = mergedCrvs2[0];
		}
		else
		{
			Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0, profileHeight, 1.0);
			profile.Transform(xform2);
			curve3 = profile;
		}
		Transform transform5 = Transform.Translation(0.0, 0.0 - profileHeight, 0.0);
		Transform transform6 = Transform.Rotation(angle, Vector3d.ZAxis, Point3d.Origin);
		curve3.Transform(transform6 * transform5);
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
		up.Unitize();
		if (!GemOffsetCurve.GemOffset(gemShape, centerGemShapeOverride, centerGemSpacing, out Curve curve4, false, out string error, 0.0, 0.0, 0.0))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		NurbsCurve nurbsCurve = curve4.ToNurbsCurve();
		NurbsCurve nurbsCurve2 = curve3.ToNurbsCurve();
		NurbsSurface nurbsSurface = NurbsSurface.Create(3, isRational: true, nurbsCurve2.Order, nurbsCurve.Order, nurbsCurve2.Points.Count, nurbsCurve.Points.Count);
		bool flag = false;
		List<Curve> list = new List<Curve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		for (int i = 0; i < nurbsCurve2.Points.Count; i++)
		{
			ControlPoint controlPoint = nurbsCurve2.Points[i];
			if (!GemOffsetCurve.GemOffset(gemShape, gem, centerGemSpacing + controlPoint.Location.X, out curve4, false, out error, 0.0, 0.0, 0.0))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			curve4.Translate(up * (placement + controlPoint.Location.Y));
			list.Add(curve4);
			NurbsCurve nurbsCurve3 = curve4.ToNurbsCurve();
			if (nurbsCurve.Points.Count != nurbsCurve3.Points.Count)
			{
				flag = true;
			}
			if (!flag)
			{
				for (int j = 0; j < nurbsCurve.Points.Count; j++)
				{
					ControlPoint controlPoint2 = nurbsCurve3.Points[j];
					double weight = nurbsCurve2.Points[i].Weight * nurbsCurve3.Points[j].Weight;
					nurbsSurface.Points.SetControlPoint(i, j, new ControlPoint(controlPoint2.Location.X, controlPoint2.Location.Y, controlPoint2.Location.Z, weight));
				}
			}
		}
		if (!flag)
		{
			for (int k = 0; k < nurbsCurve2.Knots.Count; k++)
			{
				nurbsSurface.KnotsU[k] = nurbsCurve2.Knots[k];
			}
			for (int l = 0; l < nurbsCurve.Knots.Count; l++)
			{
				nurbsSurface.KnotsV[l] = nurbsCurve.Knots[l];
			}
			Brep brep = nurbsSurface.ToBrep();
			brep.Transform(xUp);
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			if (!brep.IsSolid)
			{
				Brep brep2 = brep.DuplicateBrep();
				brep2.JoinNakedEdges(CSApplicationHelper.Application.Tolerance);
				if (!brep2.IsSolid)
				{
					brep2.Flip();
				}
				brep = brep2;
			}
			outputMetals.Add(brep);
			outputTransforms.Add(xUp);
		}
		else
		{
			Brep brep3 = CSUtilSweepRC.Loft(list, LoftType.Loose);
			if (brep3 != null)
			{
				brep3.Transform(xUp);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep3);
				outputMetals.Add(brep3);
				outputTransforms.Add(xUp);
			}
		}
		outError = string.Empty;
		return true;
	}
}
