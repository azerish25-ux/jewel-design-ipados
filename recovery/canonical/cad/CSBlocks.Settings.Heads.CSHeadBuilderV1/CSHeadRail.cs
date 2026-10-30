using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1;

[CSBlock("HeadRail", "{41E1FAF3-0431-4161-B1DF-41E494AA4233}")]
public sealed class CSHeadRail : CSBlock
{
	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InHeadSideX;

	public readonly CSInputChannel InHeadSideY;

	public readonly CSInputChannel InEnableRail;

	public readonly CSInputChannel InRailZPos;

	public readonly CSInputChannel InRailProfile;

	public readonly CSInputChannel InRailX;

	public readonly CSInputChannel InRailY;

	public readonly CSInputChannel InEnableSecondControl;

	public readonly CSInputChannel InRailProfile2;

	public readonly CSInputChannel InRailX2;

	public readonly CSInputChannel InRailY2;

	public readonly CSInputChannel InRailOffset;

	public readonly CSInputChannel InRailOffset2;

	public readonly CSInputChannel InEnableModeDoItYourself;

	public readonly CSInputChannel InShearMultiplier;

	public readonly CSInputChannel InFastMulti;

	public readonly CSInputChannel InBaseStyle;

	public readonly CSOutputChannel OutObjects;

	public readonly CSOutputChannel OutCurves;

	public readonly CSOutputChannel OutZPos;

	public readonly CSOutputChannel OutXPos;

	public CSHeadRail(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InHeadSideX = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InHeadSideY = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InEnableRail = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InRailZPos = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InRailY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InEnableSecondControl = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InRailProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InRailX2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InRailY2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InRailOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailOffset2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEnableModeDoItYourself = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InShearMultiplier = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InFastMulti = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InBaseStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 122);
		OutObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutZPos = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutXPos = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGem.GemList;
		bool boolean = InFastMulti.Boolean;
		double num = InRailZPos.Double;
		bool flag = InEnableRail.Boolean;
		ValueListItem valueListItem = InBaseStyle.ValueListItem;
		if (valueListItem == ValueListItem.HeadBuilderProngHeadPoint || valueListItem == ValueListItem.HeadBuilderProngHeadCutToPoint)
		{
			flag = false;
		}
		bool boolean2 = InEnableSecondControl.Boolean;
		bool boolean3 = InEnableModeDoItYourself.Boolean;
		Curve curve;
		double num2;
		double num3;
		double num4;
		if (!boolean2)
		{
			curve = InRailProfile.Curve;
			if (curve == null)
			{
				return UpdateMissing("Rail Profile");
			}
			num2 = InRailX.Double;
			num3 = InRailY.Double;
			num4 = InRailOffset.Double;
		}
		else
		{
			curve = InRailProfile2.Curve;
			if (curve == null)
			{
				return UpdateMissing("Rail Profile 2");
			}
			num2 = InRailX2.Double;
			num3 = InRailY2.Double;
			num4 = InRailOffset2.Double;
		}
		double num5 = InShearMultiplier.Double;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		int num6 = gemList.Count;
		if (boolean)
		{
			num6 = 1;
		}
		for (int i = 0; i < num6; i++)
		{
			CSDataArray cSDataArray5 = new CSDataArray();
			Curve curve2 = InHeadSideX.Curve;
			if (curve2 == null)
			{
				return UpdateMissing("Invalid X side profile");
			}
			Curve curve3 = InHeadSideY.Curve;
			if (curve3 == null)
			{
				return UpdateMissing("Invalid Y side profile");
			}
			double num7 = curve2.PointAtEnd.Z * (1.0 - num);
			cSDataArray3.Append(new CSNumberData(num7 + num3));
			CSGemData cSGemData = gemList[i];
			if (cSGemData == null)
			{
				return UpdateMissing("Invalid Gem");
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var xDown);
			CurveIntersections curveIntersections = Intersection.CurveCurve(new LineCurve(new Point3d(0.0, 0.0, num7), new Point3d(100.0, 0.0, num7)), curve2, 0.01, 0.01);
			if (curveIntersections.Count <= 0)
			{
				return UpdateError("Unable to intersect X side");
			}
			CurveIntersections curveIntersections2 = Intersection.CurveCurve(new LineCurve(new Point3d(0.0, 0.0, num7), new Point3d(0.0, 100.0, num7)), curve3, 0.01, 0.01);
			if (curveIntersections2.Count <= 0)
			{
				return UpdateError("Unable to intersect Y side");
			}
			double x = curveIntersections[0].PointA.X;
			double y = curveIntersections2[0].PointA.Y;
			cSDataArray4.Append(new CSNumberData(x - cSGemData.GemXMax));
			if (flag)
			{
				double parameterB = curveIntersections[0].ParameterB;
				double num8 = (0.0 - curve2.TangentAt(parameterB).X) * num5;
				Transform transform = Transform.Scale(Plane.WorldXY, x / cSGemData.GemXMax, y / cSGemData.GemYMax, 1.0);
				CSGemData cSGemData2 = new CSGemData(cSGemData);
				cSGemData2.ApplyTransform(transform * xDown);
				if (!GemOffsetCurve.GemOffset(cSGemData2, num4, out Curve curve4, false, out string error, 0.0))
				{
					return UpdateError("Gem Offset:  " + error);
				}
				if (curve4 == null)
				{
					return UpdateError("Gem Offset Outside");
				}
				if (!GemOffsetCurve.GemOffset(cSGemData2, num4 + (0.0 - num2), out Curve curve5, false, out string error2, 0.0))
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
				vector3d *= num2;
				double num9 = num4 + (0.0 - num2);
				double t2;
				if (num9 < 0.0 && (Math.Abs(num9) > cSGemData2.GemXSize / 2.0 || Math.Abs(num9) > cSGemData2.GemYSize / 2.0))
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
				Transform transform3 = Transform.Scale(Plane.WorldXY, num2 / boundingBox.Diagonal.X, num3 / boundingBox.Diagonal.Y, 0.0);
				nurbsCurve.Transform(transform3 * transform2);
				for (int j = 0; j < nurbsCurve.Points.Count; j++)
				{
					ControlPoint value = nurbsCurve.Points[j];
					Point3d location = value.Location;
					location.X += num8 * location.Y;
					value.Location = location;
					nurbsCurve.Points[j] = value;
				}
				curve4.ClosestPoint(testPoint, out var t3);
				Point3d destination = curve4.PointAt(t3);
				Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(num2, 0.0, 0.0), new Vector3d(-1.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0), destination, vector3d, Vector3d.ZAxis);
				nurbsCurve.Transform(xform);
				Transform transform4 = Transform.Translation(0.0, 0.0, num7);
				curve4.Transform(xUp * transform4);
				curve5.Transform(xUp * transform4);
				nurbsCurve.Transform(xUp * transform4);
				cSDataArray2.Append(new CSOnGeometryData(curve4, xUp * transform4, applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData(curve5, xUp * transform4, applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData(nurbsCurve, xUp * transform4, applyTransform: false));
				if (!boolean3)
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
		OutObjects.SetData(cSDataArray);
		OutCurves.SetData(cSDataArray2);
		OutZPos.SetData(cSDataArray3);
		OutXPos.SetData(cSDataArray4);
		return base.Update();
	}
}
