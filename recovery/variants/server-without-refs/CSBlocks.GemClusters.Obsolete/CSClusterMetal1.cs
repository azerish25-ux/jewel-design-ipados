using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("ClusterMetal_V1", "{5DF2C7FE-E243-43db-B1BB-44CDCA3DDE9C}")]
public sealed class CSClusterMetal1 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iOutsideProfile { get; }

	public CSInputChannel iWidthOffset { get; }

	public CSInputChannel iThickness { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iRotation { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iZOffset { get; }

	public CSInputChannel iShape { get; }

	public CSInputChannel iAngleBottom { get; }

	public CSOutputChannel oMetal { get; }

	public CSOutputChannel oShape { get; }

	public CSOutputChannel oStuff { get; }

	public CSClusterMetal1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iOutsideProfile = new CSInputChannel(GetPropertyName(() => iOutsideProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iWidthOffset = new CSInputChannel(GetPropertyName(() => iWidthOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iThickness = new CSInputChannel(GetPropertyName(() => iThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(GetPropertyName(() => iRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZOffset = new CSInputChannel(GetPropertyName(() => iZOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShape = new CSInputChannel(GetPropertyName(() => iShape), this, ChannelDataType.String, ChannelArrayType.Individual);
		iAngleBottom = new CSInputChannel(GetPropertyName(() => iAngleBottom), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oMetal = new CSOutputChannel(GetPropertyName(() => oMetal), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		oShape = new CSOutputChannel(GetPropertyName(() => oShape), this, ChannelDataType.String, ChannelArrayType.Individual);
		oStuff = new CSOutputChannel(GetPropertyName(() => oStuff), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_02f2: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Expected O, but got Unknown
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Expected O, but got Unknown
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Expected O, but got Unknown
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Expected O, but got Unknown
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		if (!(iGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem Definition");
		}
		OnNurbsCurve nurbsCurve = iOutsideProfile.GetNurbsCurve();
		double value = 0.0;
		iWidthOffset.GetDouble(ref value);
		double value2 = 0.0;
		iThickness.GetDouble(ref value2);
		double value3 = 0.0;
		iAngle.GetDouble(ref value3);
		value3 = CSBlockUtil.CSUtilMath.ToRadians(value3);
		double value4 = 0.0;
		iRotation.GetDouble(ref value4);
		value4 = CSBlockUtil.CSUtilMath.ToRadians(value4);
		double value5 = 0.5;
		iGirdleSpacing.GetDouble(ref value5);
		double value6 = 0.0;
		iPlacement.GetDouble(ref value6);
		double value7 = 0.0;
		iZOffset.GetDouble(ref value7);
		string output = "";
		iShape.GetString(ref output);
		bool value8 = false;
		iAngleBottom.GetBoolean(ref value8);
		double num = value / 2.0;
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value4, Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("BoundingBox Failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i).Duplicate() is CSGemData cSGemData2))
			{
				continue;
			}
			if (output == "")
			{
				output = cSGemData2.GemShape;
			}
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData2.Xform, out var xUp, out var xDown))
			{
				return UpdateError("Error getting transform");
			}
			cSGemData2.ApplyTransform(xDown);
			BoundingBox boundingBox2 = BoundingBox.Unset;
			if (!cSGemData2.GetTightBoundingBox(ref boundingBox2))
			{
				return UpdateError("BoundingBox Error");
			}
			Point3d val = ((BoundingBox)(ref boundingBox2)).Max;
			double num2 = ((Point3d)(ref val)).Y + value5 - 0.04;
			if (num > num2)
			{
				num = num2;
			}
			val = ((BoundingBox)(ref boundingBox2)).Max;
			double num3 = ((Point3d)(ref val)).Y + value5;
			val = ((BoundingBox)(ref boundingBox)).Min;
			On3dPoint val2 = new On3dPoint(0.0, num3 - ((Point3d)(ref val)).Y, 0.0);
			OnXform val3 = new OnXform();
			val3.Rotation(value3, OnUtil.On_xaxis, (IOn3dPoint)(object)val2);
			double y = val2.y;
			val = ((BoundingBox)(ref boundingBox)).Max;
			On3dPoint val4 = new On3dPoint(0.0, y + ((Point3d)(ref val)).Y + num, 0.0);
			double y2 = val2.y;
			val = ((BoundingBox)(ref boundingBox)).Min;
			OnLineCurve val5 = new OnLineCurve((IOn3dPoint)val4, (IOn3dPoint)new On3dPoint(0.0, y2 + ((Point3d)(ref val)).Y - num, 0.0));
			val5.Transform((IOnXform)(object)val3);
			On3dVector val6 = new On3dVector(OnUtil.On_zaxis);
			val6.Transform((IOnXform)(object)val3);
			val6.Reverse();
			val6.Unitize();
			OnLineCurve val7 = val5.Duplicate();
			((OnGeometry)val7).Translate((IOn3dVector)(object)(val6 * value2));
			On3dVector val8 = new On3dVector((IOn3dVector)(object)cSGemData2.Up.Convert());
			val8.Unitize();
			string shape = output;
			double y3 = ((OnCurve)val7).PointAtEnd().y;
			val = ((BoundingBox)(ref boundingBox2)).Max;
			if (!GemOffsetCurveV1.GemOffset(shape, cSGemData2, y3 - ((Point3d)(ref val)).Y, out OnCurve curve, doTransformUp: true, out string error))
			{
				return UpdateError(error);
			}
			if (value8)
			{
				((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val8) * (((OnCurve)val7).PointAtEnd().z + value6 + value7)));
			}
			else
			{
				((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val8) * (((OnCurve)val7).PointAtStart().z + value6 + value7)));
			}
			OnCurve val9 = curve.DuplicateCurve();
			((OnGeometry)val9).Transform((IOnXform)(object)xUp.Convert());
			cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val9.Convert(), xUp, applyTransform: false));
			string shape2 = output;
			double y4 = ((OnCurve)val5).PointAtEnd().y;
			val = ((BoundingBox)(ref boundingBox2)).Max;
			if (!GemOffsetCurveV1.GemOffset(shape2, cSGemData2, y4 - ((Point3d)(ref val)).Y, out OnCurve curve2, doTransformUp: true, out error))
			{
				return UpdateError(error);
			}
			((OnGeometry)curve2).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val8) * (((OnCurve)val5).PointAtEnd().z + value6 + value7)));
			OnCurve val10 = curve2.DuplicateCurve();
			((OnGeometry)val10).Transform((IOnXform)(object)xUp.Convert());
			cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val10.Convert(), xUp, applyTransform: false));
			string shape3 = output;
			double y5 = ((OnCurve)val5).PointAtStart().y;
			val = ((BoundingBox)(ref boundingBox2)).Max;
			if (!GemOffsetCurveV1.GemOffset(shape3, cSGemData2, y5 - ((Point3d)(ref val)).Y, out OnCurve curve3, doTransformUp: true, out error))
			{
				return UpdateError(error);
			}
			((OnGeometry)curve3).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val8) * (((OnCurve)val5).PointAtStart().z + value6 + value7)));
			OnCurve val11 = curve3.DuplicateCurve();
			((OnGeometry)val11).Transform((IOnXform)(object)xUp.Convert());
			cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val11.Convert(), xUp, applyTransform: false));
			string shape4 = output;
			double y6 = ((OnCurve)val7).PointAtStart().y;
			val = ((BoundingBox)(ref boundingBox2)).Max;
			if (!GemOffsetCurveV1.GemOffset(shape4, cSGemData2, y6 - ((Point3d)(ref val)).Y, out OnCurve curve4, doTransformUp: true, out error))
			{
				return UpdateError(error);
			}
			((OnGeometry)curve4).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val8) * (((OnCurve)val7).PointAtStart().z + value6 + value7)));
			OnCurve val12 = curve4.DuplicateCurve();
			((OnGeometry)val12).Transform((IOnXform)(object)xUp.Convert());
			cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val12.Convert(), xUp, applyTransform: false));
			OnBrep srf = null;
			if (!SweepNoProfile((IOnCurve)(object)curve3, (IOnCurve)(object)curve2, ref srf))
			{
				return UpdateError("Sweep");
			}
			OnBrep srf2 = null;
			if (nurbsCurve == null)
			{
				if (!SweepNoProfile((IOnCurve)(object)curve4, (IOnCurve)(object)curve3, ref srf2))
				{
					return UpdateError("Sweep Outside Surface");
				}
			}
			else
			{
				On3dPoint val13 = curve4.PointAtStart();
				On3dVector val14 = curve4.TangentAt(curve4.Domain().Min());
				val14.Unitize();
				On3dPoint val15 = curve3.PointAtStart();
				On3dVector obj = curve3.TangentAt(curve3.Domain().Min());
				obj.Unitize();
				On3dPoint val16 = new On3dPoint((IOn3dPoint)(object)val13);
				On3dVector val17 = new On3dVector((IOn3dVector)(object)(val15 - (IOn3dPoint)(object)val13));
				val17.Unitize();
				On3dVector val18 = new On3dVector((IOn3dVector)(object)(obj + (IOn3dVector)(object)val14));
				val18.Unitize();
				OnXform val19 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val16, (IOn3dVector)(object)val17, (IOn3dVector)(object)val18);
				double num4 = Math.Abs(((OnCurve)nurbsCurve).PointAtStart().DistanceTo((IOn3dPoint)(object)((OnCurve)nurbsCurve).PointAtEnd()));
				double num5 = Math.Abs(val13.DistanceTo((IOn3dPoint)(object)val15)) / num4;
				OnXform val20 = new OnXform();
				val20.Identity();
				val20.Scale((IOn3dPoint)(object)val16, num5);
				OnCurve val21 = (OnCurve)(object)nurbsCurve.Duplicate();
				((OnGeometry)val21).Transform((IOnXform)(object)(val20 * (IOnXform)(object)val19));
				OnCurve val22 = val21.DuplicateCurve();
				((OnGeometry)val22).Transform((IOnXform)(object)xUp.Convert());
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val22.Convert(), xUp, applyTransform: false));
				OnCurve[] array = (OnCurve[])(object)new OnCurve[1] { val21 };
				double[] array2 = new double[1];
				double[] array3 = new double[1];
				array2[0] = curve4.Domain().Min();
				array3[0] = curve3.Domain().Min();
				OnCurve railCurve = curve4;
				OnCurve railCurve2 = curve3;
				IOnCurve[] profiles = (IOnCurve[])(object)array;
				CSUtilSweep.Sweep2((IOnCurve)(object)railCurve, (IOnCurve)(object)railCurve2, profiles, array2, array3, out var breps);
				if (breps.Length != 1)
				{
					return UpdateError("Sweep 2");
				}
				srf2 = breps[0];
				if (srf2 == null)
				{
					return UpdateError("Sweep 2");
				}
			}
			OnBrep srf3 = null;
			if (!SweepNoProfile((IOnCurve)(object)curve, (IOnCurve)(object)curve4, ref srf3))
			{
				return UpdateError("Sweep Surface Bottom");
			}
			OnBrep srf4 = null;
			if (!SweepNoProfile((IOnCurve)(object)curve2, (IOnCurve)(object)curve, ref srf4))
			{
				return UpdateError("Sweep Surface Inside");
			}
			if (CSUtilRhino.JoinBreps((OnBrep[])(object)new OnBrep[4] { srf4, srf, srf2, srf3 }, out var outputBreps, 0.01))
			{
				OnBrep[] array4 = outputBreps;
				foreach (OnBrep val23 in array4)
				{
					((OnGeometry)val23).Transform((IOnXform)(object)xUp.Convert());
					cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val23.Convert(), xUp, applyTransform: false));
				}
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oMetal.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oMetal.SetData(cSDataArray);
		}
		oShape.SetData(new CSStringData(output));
		oStuff.SetData(cSDataArray2);
		return base.Update();
	}

	private static bool SweepNoProfile(IOnCurve rail1, IOnCurve rail2, ref OnBrep srf)
	{
		if (rail1 == null || rail2 == null)
		{
			return false;
		}
		if (!CSUtilSweep.Sweep2NoProfile(rail1, rail2, out var breps))
		{
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		if (breps.Length < 1)
		{
			return false;
		}
		if (!((OnObject)breps[0]).IsValid())
		{
			return false;
		}
		srf = breps[0];
		return true;
	}
}
