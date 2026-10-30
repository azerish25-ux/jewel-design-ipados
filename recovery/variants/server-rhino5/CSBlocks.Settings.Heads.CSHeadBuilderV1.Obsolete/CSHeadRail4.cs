using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadRail_V4", "{010DC22E-2F6C-4C41-BFE3-3A06EEAED5E2}")]
public sealed class CSHeadRail4 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iHeadSideX { get; }

	public CSInputChannel iHeadSideY { get; }

	public CSInputChannel iEnableRail { get; }

	public CSInputChannel iRailZPos { get; }

	public CSInputChannel iRailProfile { get; }

	public CSInputChannel iRailX { get; }

	public CSInputChannel iRailY { get; }

	public CSInputChannel iEnableSecondControl { get; }

	public CSInputChannel iRailProfile2 { get; }

	public CSInputChannel iRailX2 { get; }

	public CSInputChannel iRailY2 { get; }

	public CSInputChannel iRailOffset { get; }

	public CSInputChannel iRailOffset2 { get; }

	public CSInputChannel iEnableModeDoItYourself { get; }

	public CSInputChannel iShearMultiplier { get; }

	public CSInputChannel iFastMulti { get; }

	public CSInputChannel iBaseStyle { get; }

	public CSOutputChannel oObjects { get; }

	public CSOutputChannel oCurves { get; }

	public CSOutputChannel oZPos { get; }

	public CSOutputChannel oXPos { get; }

	public CSHeadRail4(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iHeadSideX = new CSInputChannel(GetPropertyName(() => iHeadSideX), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iHeadSideY = new CSInputChannel(GetPropertyName(() => iHeadSideY), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iEnableRail = new CSInputChannel(GetPropertyName(() => iEnableRail), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iRailZPos = new CSInputChannel(GetPropertyName(() => iRailZPos), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailProfile = new CSInputChannel(GetPropertyName(() => iRailProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRailX = new CSInputChannel(GetPropertyName(() => iRailX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailY = new CSInputChannel(GetPropertyName(() => iRailY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableSecondControl = new CSInputChannel(GetPropertyName(() => iEnableSecondControl), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iRailProfile2 = new CSInputChannel(GetPropertyName(() => iRailProfile2), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRailX2 = new CSInputChannel(GetPropertyName(() => iRailX2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailY2 = new CSInputChannel(GetPropertyName(() => iRailY2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailOffset = new CSInputChannel(GetPropertyName(() => iRailOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailOffset2 = new CSInputChannel(GetPropertyName(() => iRailOffset2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableModeDoItYourself = new CSInputChannel(GetPropertyName(() => iEnableModeDoItYourself), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iShearMultiplier = new CSInputChannel(GetPropertyName(() => iShearMultiplier), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFastMulti = new CSInputChannel(GetPropertyName(() => iFastMulti), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iBaseStyle = new CSInputChannel(GetPropertyName(() => iBaseStyle), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
		oCurves = new CSOutputChannel(GetPropertyName(() => oCurves), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oZPos = new CSOutputChannel(GetPropertyName(() => oZPos), this, ChannelDataType.Double, ChannelArrayType.Array);
		oXPos = new CSOutputChannel(GetPropertyName(() => oXPos), this, ChannelDataType.Double, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Gems");
		}
		bool value = false;
		iFastMulti.GetBoolean(ref value);
		double value2 = 0.0;
		iRailZPos.GetDouble(ref value2);
		bool value3 = false;
		iEnableRail.GetBoolean(ref value3);
		ValueListItem whichItem = ValueListItem.HeadBuilderProngHead;
		iBaseStyle.GetValueListItem(ref whichItem);
		if (whichItem == ValueListItem.HeadBuilderProngHeadPoint || whichItem == ValueListItem.HeadBuilderProngHeadCutToPoint)
		{
			value3 = false;
		}
		bool value4 = false;
		iEnableSecondControl.GetBoolean(ref value4);
		bool value5 = false;
		iEnableModeDoItYourself.GetBoolean(ref value5);
		double value6 = 1.0;
		double value7 = 1.0;
		double value8 = 0.0;
		Curve curve;
		if (!value4)
		{
			if (!(iRailProfile.Data is ICSCurve iCSCurve))
			{
				return UpdateMissing("Rail Profile Data");
			}
			curve = iCSCurve.GetCurve();
			if (curve == null)
			{
				return UpdateMissing("Rail Profile");
			}
			iRailX.GetDouble(ref value6);
			iRailY.GetDouble(ref value7);
			iRailOffset.GetDouble(ref value8);
		}
		else
		{
			if (!(iRailProfile2.Data is ICSCurve iCSCurve2))
			{
				return UpdateMissing("Rail Profile Data 2");
			}
			curve = iCSCurve2.GetCurve();
			if (curve == null)
			{
				return UpdateMissing("Rail Profile 2");
			}
			iRailX2.GetDouble(ref value6);
			iRailY2.GetDouble(ref value7);
			iRailOffset2.GetDouble(ref value8);
		}
		double value9 = 1.0;
		iShearMultiplier.GetDouble(ref value9);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		int num = flattenArray.GetArrayCount();
		if (value)
		{
			num = 1;
		}
		for (int i = 0; i < num; i++)
		{
			CSDataArray cSDataArray5 = new CSDataArray();
			if (!(iHeadSideX.GetArrayItem(i) is ICSCurve iCSCurve3))
			{
				return UpdateError("Invalid X side profile data");
			}
			Curve curve2 = iCSCurve3.GetCurve();
			if (curve2 == null)
			{
				return UpdateMissing("Invalid X side profile");
			}
			if (!(iHeadSideY.GetArrayItem(i) is ICSCurve iCSCurve4))
			{
				return UpdateError("Invalid Y side profile data");
			}
			Curve curve3 = iCSCurve4.GetCurve();
			if (curve3 == null)
			{
				return UpdateMissing("Invalid Y side profile");
			}
			double num2 = curve2.PointAtEnd.Z * (1.0 - value2);
			cSDataArray3.Append(new CSNumberData(num2 + value7));
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateMissing("Invalid Gem");
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var xDown);
			CurveIntersections curveIntersections = Intersection.CurveCurve(new LineCurve(new Point3d(0.0, 0.0, num2), new Point3d(100.0, 0.0, num2)), curve2, 0.01, 0.01);
			if (curveIntersections.Count <= 0)
			{
				return UpdateError("Unable to intersect X side");
			}
			CurveIntersections curveIntersections2 = Intersection.CurveCurve(new LineCurve(new Point3d(0.0, 0.0, num2), new Point3d(0.0, 100.0, num2)), curve3, 0.01, 0.01);
			if (curveIntersections2.Count <= 0)
			{
				return UpdateError("Unable to intersect Y side");
			}
			double x = curveIntersections[0].PointA.X;
			double y = curveIntersections2[0].PointA.Y;
			cSDataArray4.Append(new CSNumberData(x - cSGemData.GemXMax));
			if (value3)
			{
				double parameterB = curveIntersections[0].ParameterB;
				double num3 = (0.0 - curve2.TangentAt(parameterB).X) * value9;
				Transform transform = Transform.Scale(Plane.WorldXY, x / cSGemData.GemXMax, y / cSGemData.GemYMax, 1.0);
				CSGemData cSGemData2 = new CSGemData(cSGemData);
				cSGemData2.ApplyTransform(transform * xDown);
				if (!GemOffsetCurveV2.GemOffset(cSGemData2, value8, out Curve curve4, false, out string error, 0.0))
				{
					return UpdateError("Gem Offset:  " + error);
				}
				if (curve4 == null)
				{
					return UpdateError("Gem Offset Outside");
				}
				if (!GemOffsetCurveV2.GemOffset(cSGemData2, value8 + (0.0 - value6), out Curve curve5, false, out string error2, 0.0))
				{
					return UpdateError("Gem Offset Inside:  " + error2);
				}
				if (curve5 == null)
				{
					return UpdateError("Gem Offset Inside");
				}
				Point3d testPoint = cSGemData2.GetCurve().PointAt(cSGemData.BezelParam);
				curve4.ClosestPoint(testPoint, out var t);
				Point3d point3d = curve4.PointAt(t);
				Vector3d vector3d = Vector3d.CrossProduct(curve4.TangentAt(t), Vector3d.ZAxis);
				vector3d.Unitize();
				vector3d *= value6;
				double num4 = value8 + (0.0 - value6);
				double t2;
				if (num4 < 0.0 && (Math.Abs(num4) > cSGemData2.GemXSize / 2.0 || Math.Abs(num4) > cSGemData2.GemYSize / 2.0))
				{
					curve5.ClosestPoint(point3d - vector3d, out t2);
				}
				else
				{
					curve5.ClosestPoint(point3d + vector3d, out t2);
				}
				NurbsCurve nurbsCurve = curve.ToNurbsCurve();
				BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
				Transform transform2 = Transform.Translation(0.0 - boundingBox.Min.X, 0.0 - boundingBox.Min.Y, 0.0 - boundingBox.Min.Z);
				Transform transform3 = Transform.Scale(Plane.WorldXY, value6 / boundingBox.Diagonal.X, value7 / boundingBox.Diagonal.Y, 0.0);
				nurbsCurve.Transform(transform3 * transform2);
				for (int j = 0; j < nurbsCurve.Points.Count; j++)
				{
					ControlPoint value10 = nurbsCurve.Points[j];
					Point3d location = value10.Location;
					location.X += num3 * location.Y;
					value10.Location = location;
					nurbsCurve.Points[j] = value10;
				}
				curve4.ClosestPoint(testPoint, out var t3);
				Point3d destination = curve4.PointAt(t3);
				Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(value6, 0.0, 0.0), new Vector3d(-1.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0), destination, vector3d, Vector3d.ZAxis);
				nurbsCurve.Transform(xform);
				Transform transform4 = Transform.Translation(0.0, 0.0, num2);
				curve4.Transform(xUp * transform4);
				curve5.Transform(xUp * transform4);
				nurbsCurve.Transform(xUp * transform4);
				cSDataArray2.Append(new CSOnGeometryData(curve4, xUp * transform4, applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData(curve5, xUp * transform4, applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData(nurbsCurve, xUp * transform4, applyTransform: false));
				if (!value5)
				{
					List<Curve> profile = new List<Curve> { nurbsCurve };
					List<double> parameters = new List<double> { t };
					List<double> parameters2 = new List<double> { t2 };
					if (!CSUtilSweepRC.Sweep2(curve4, curve5, profile, parameters, parameters2, out Brep breps, true, true))
					{
						return UpdateError("Failed Sweep1 on rails");
					}
					cSDataArray5.Append(new CSOnGeometryData(breps));
				}
			}
			cSDataArray.Append(cSDataArray5);
		}
		oObjects.SetData(cSDataArray);
		oCurves.SetData(cSDataArray2);
		oZPos.SetData(cSDataArray3);
		oXPos.SetData(cSDataArray4);
		return base.Update();
	}
}
