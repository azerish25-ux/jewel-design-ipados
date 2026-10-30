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
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
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
		double num9 = default(double);
		double item = default(double);
		double num16 = default(double);
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
			Point3d pointAtEnd = curve2.PointAtEnd;
			double num7 = ((Point3d)(ref pointAtEnd)).Z * (1.0 - num);
			cSDataArray3.Append(new CSNumberData(num7 + num3));
			CSGemData cSGemData = gemList[i];
			if (cSGemData == null)
			{
				return UpdateMissing("Invalid Gem");
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var xDown);
			CurveIntersections val = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(0.0, 0.0, num7), new Point3d(100.0, 0.0, num7)), curve2, 0.01, 0.01);
			if (val.Count <= 0)
			{
				return UpdateError("Unable to intersect X side");
			}
			CurveIntersections val2 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(0.0, 0.0, num7), new Point3d(0.0, 100.0, num7)), curve3, 0.01, 0.01);
			if (val2.Count <= 0)
			{
				return UpdateError("Unable to intersect Y side");
			}
			Point3d val3 = val[0].PointA;
			double x = ((Point3d)(ref val3)).X;
			val3 = val2[0].PointA;
			double y = ((Point3d)(ref val3)).Y;
			cSDataArray4.Append(new CSNumberData(x - cSGemData.GemXMax));
			if (flag)
			{
				double parameterB = val[0].ParameterB;
				Vector3d val4 = curve2.TangentAt(parameterB);
				double num8 = (0.0 - ((Vector3d)(ref val4)).X) * num5;
				Transform val5 = Transform.Scale(Plane.WorldXY, x / cSGemData.GemXMax, y / cSGemData.GemYMax, 1.0);
				CSGemData cSGemData2 = new CSGemData(cSGemData);
				cSGemData2.ApplyTransform(val5 * xDown);
				if (!GemOffsetCurve.GemOffset(cSGemData2, num4, out Curve curve4, doTransformUp: false, out string error, 0.0))
				{
					return UpdateError("Gem Offset:  " + error);
				}
				if (curve4 == null)
				{
					return UpdateError("Gem Offset Outside");
				}
				if (!GemOffsetCurve.GemOffset(cSGemData2, num4 + (0.0 - num2), out Curve curve5, doTransformUp: false, out string error2, 0.0))
				{
					return UpdateError("Gem Offset Inside:  " + error2);
				}
				if (curve5 == null)
				{
					return UpdateError("Gem Offset Inside");
				}
				Point3d val6 = cSGemData2.GetCurve().PointAt(cSGemData.BezelParam);
				curve4.ClosestPoint(val6, ref num9);
				Point3d val7 = curve4.PointAt(num9);
				Vector3d val8 = Vector3d.CrossProduct(curve4.TangentAt(num9), Vector3d.ZAxis);
				((Vector3d)(ref val8)).Unitize();
				val8 *= num2;
				double num10 = num4 + (0.0 - num2);
				if (num10 < 0.0 && (Math.Abs(num10) > cSGemData2.GemXSize / 2.0 || Math.Abs(num10) > cSGemData2.GemYSize / 2.0))
				{
					curve5.ClosestPoint(val7 - val8, ref item);
				}
				else
				{
					curve5.ClosestPoint(val7 + val8, ref item);
				}
				NurbsCurve val9 = curve.ToNurbsCurve();
				BoundingBox boundingBox = ((GeometryBase)val9).GetBoundingBox(true);
				val3 = ((BoundingBox)(ref boundingBox)).Min;
				double num11 = 0.0 - ((Point3d)(ref val3)).X;
				val3 = ((BoundingBox)(ref boundingBox)).Min;
				double num12 = 0.0 - ((Point3d)(ref val3)).Y;
				val3 = ((BoundingBox)(ref boundingBox)).Min;
				Transform val10 = Transform.Translation(num11, num12, 0.0 - ((Point3d)(ref val3)).Z);
				Plane worldXY = Plane.WorldXY;
				double num13 = num2;
				Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num14 = num13 / ((Vector3d)(ref diagonal)).X;
				double num15 = num3;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				Transform val11 = Transform.Scale(worldXY, num14, num15 / ((Vector3d)(ref diagonal)).Y, 0.0);
				((GeometryBase)val9).Transform(val11 * val10);
				for (int j = 0; j < val9.Points.Count; j++)
				{
					ControlPoint val12 = val9.Points[j];
					Point3d location = ((ControlPoint)(ref val12)).Location;
					((Point3d)(ref location)).X = ((Point3d)(ref location)).X + num8 * ((Point3d)(ref location)).Y;
					((ControlPoint)(ref val12)).Location = location;
					val9.Points[j] = val12;
				}
				curve4.ClosestPoint(val6, ref num16);
				Point3d destination = curve4.PointAt(num16);
				Transform val13 = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(num2, 0.0, 0.0), new Vector3d(-1.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0), destination, val8, Vector3d.ZAxis);
				((GeometryBase)val9).Transform(val13);
				Transform val14 = Transform.Translation(0.0, 0.0, num7);
				((GeometryBase)curve4).Transform(xUp * val14);
				((GeometryBase)curve5).Transform(xUp * val14);
				((GeometryBase)val9).Transform(xUp * val14);
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)curve4, xUp * val14, applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)curve5, xUp * val14, applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val9, xUp * val14, applyTransform: false));
				if (!boolean3)
				{
					List<Curve> profile = new List<Curve> { (Curve)(object)val9 };
					List<double> parameters = new List<double> { num9 };
					List<double> parameters2 = new List<double> { item };
					if (!CSUtilSweepRC.Sweep2(curve4, curve5, profile, parameters, parameters2, out Brep breps, closed: true, maintainHeight: true))
					{
						return UpdateError("Failed Sweep1 on rails");
					}
					cSDataArray5.Append(new CSOnGeometryData((GeometryBase)(object)breps));
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
