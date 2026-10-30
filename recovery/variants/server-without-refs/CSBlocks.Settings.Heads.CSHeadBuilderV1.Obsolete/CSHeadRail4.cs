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
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
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
		double num4 = default(double);
		double item = default(double);
		double num11 = default(double);
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
			Point3d pointAtEnd = curve2.PointAtEnd;
			double num2 = ((Point3d)(ref pointAtEnd)).Z * (1.0 - value2);
			cSDataArray3.Append(new CSNumberData(num2 + value7));
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateMissing("Invalid Gem");
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var xDown);
			CurveIntersections val = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(0.0, 0.0, num2), new Point3d(100.0, 0.0, num2)), curve2, 0.01, 0.01);
			if (val.Count <= 0)
			{
				return UpdateError("Unable to intersect X side");
			}
			CurveIntersections val2 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(0.0, 0.0, num2), new Point3d(0.0, 100.0, num2)), curve3, 0.01, 0.01);
			if (val2.Count <= 0)
			{
				return UpdateError("Unable to intersect Y side");
			}
			Point3d val3 = val[0].PointA;
			double x = ((Point3d)(ref val3)).X;
			val3 = val2[0].PointA;
			double y = ((Point3d)(ref val3)).Y;
			cSDataArray4.Append(new CSNumberData(x - cSGemData.GemXMax));
			if (value3)
			{
				double parameterB = val[0].ParameterB;
				Vector3d val4 = curve2.TangentAt(parameterB);
				double num3 = (0.0 - ((Vector3d)(ref val4)).X) * value9;
				Transform val5 = Transform.Scale(Plane.WorldXY, x / cSGemData.GemXMax, y / cSGemData.GemYMax, 1.0);
				CSGemData cSGemData2 = new CSGemData(cSGemData);
				cSGemData2.ApplyTransform(val5 * xDown);
				if (!GemOffsetCurveV2.GemOffset(cSGemData2, value8, out Curve curve4, doTransformUp: false, out string error, 0.0))
				{
					return UpdateError("Gem Offset:  " + error);
				}
				if (curve4 == null)
				{
					return UpdateError("Gem Offset Outside");
				}
				if (!GemOffsetCurveV2.GemOffset(cSGemData2, value8 + (0.0 - value6), out Curve curve5, doTransformUp: false, out string error2, 0.0))
				{
					return UpdateError("Gem Offset Inside:  " + error2);
				}
				if (curve5 == null)
				{
					return UpdateError("Gem Offset Inside");
				}
				Point3d val6 = cSGemData2.GetCurve().PointAt(cSGemData.BezelParam);
				curve4.ClosestPoint(val6, ref num4);
				Point3d val7 = curve4.PointAt(num4);
				Vector3d val8 = Vector3d.CrossProduct(curve4.TangentAt(num4), Vector3d.ZAxis);
				((Vector3d)(ref val8)).Unitize();
				val8 *= value6;
				double num5 = value8 + (0.0 - value6);
				if (num5 < 0.0 && (Math.Abs(num5) > cSGemData2.GemXSize / 2.0 || Math.Abs(num5) > cSGemData2.GemYSize / 2.0))
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
				double num6 = 0.0 - ((Point3d)(ref val3)).X;
				val3 = ((BoundingBox)(ref boundingBox)).Min;
				double num7 = 0.0 - ((Point3d)(ref val3)).Y;
				val3 = ((BoundingBox)(ref boundingBox)).Min;
				Transform val10 = Transform.Translation(num6, num7, 0.0 - ((Point3d)(ref val3)).Z);
				Plane worldXY = Plane.WorldXY;
				double num8 = value6;
				Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num9 = num8 / ((Vector3d)(ref diagonal)).X;
				double num10 = value7;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				Transform val11 = Transform.Scale(worldXY, num9, num10 / ((Vector3d)(ref diagonal)).Y, 0.0);
				((GeometryBase)val9).Transform(val11 * val10);
				for (int j = 0; j < val9.Points.Count; j++)
				{
					ControlPoint val12 = val9.Points[j];
					Point3d location = ((ControlPoint)(ref val12)).Location;
					((Point3d)(ref location)).X = ((Point3d)(ref location)).X + num3 * ((Point3d)(ref location)).Y;
					((ControlPoint)(ref val12)).Location = location;
					val9.Points[j] = val12;
				}
				curve4.ClosestPoint(val6, ref num11);
				Point3d destination = curve4.PointAt(num11);
				Transform val13 = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(value6, 0.0, 0.0), new Vector3d(-1.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0), destination, val8, Vector3d.ZAxis);
				((GeometryBase)val9).Transform(val13);
				Transform val14 = Transform.Translation(0.0, 0.0, num2);
				((GeometryBase)curve4).Transform(xUp * val14);
				((GeometryBase)curve5).Transform(xUp * val14);
				((GeometryBase)val9).Transform(xUp * val14);
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)curve4, xUp * val14, applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)curve5, xUp * val14, applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val9, xUp * val14, applyTransform: false));
				if (!value5)
				{
					List<Curve> profile = new List<Curve> { (Curve)(object)val9 };
					List<double> parameters = new List<double> { num4 };
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
		oObjects.SetData(cSDataArray);
		oCurves.SetData(cSDataArray2);
		oZPos.SetData(cSDataArray3);
		oXPos.SetData(cSDataArray4);
		return base.Update();
	}
}
