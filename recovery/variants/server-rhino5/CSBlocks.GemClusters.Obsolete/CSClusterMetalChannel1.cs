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
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("ClusterMetalChannel_V1", "{7B81F03A-F789-4fcd-9FA0-D96422F968A6}")]
public sealed class CSClusterMetalChannel1 : CSBlock
{
	public CSInputChannel InGem { get; }

	public CSInputChannel InGemDefinition { get; }

	public CSInputChannel InOutsideProfile { get; }

	public CSInputChannel InChannelWidthOffset { get; }

	public CSInputChannel InChannelDepth { get; }

	public CSInputChannel InInsideWidth { get; }

	public CSInputChannel InOutsideWidth { get; }

	public CSInputChannel InThickness { get; }

	public CSInputChannel InAngle { get; }

	public CSInputChannel InRotation { get; }

	public CSInputChannel InGirdleSpacing { get; }

	public CSInputChannel InPlacement { get; }

	public CSInputChannel InZOffset { get; }

	public CSInputChannel InShape { get; }

	public CSInputChannel InAngleBottom { get; }

	public CSInputChannel InFillet { get; }

	public CSOutputChannel OutMetal { get; }

	public CSOutputChannel OutShape { get; }

	public CSClusterMetalChannel1(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InOutsideProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InChannelWidthOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInsideWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOutsideWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShape = new CSInputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		InAngleBottom = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMetal = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutShape = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Expected O, but got Unknown
		//IL_037a: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Expected O, but got Unknown
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Expected O, but got Unknown
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Expected O, but got Unknown
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Expected O, but got Unknown
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Expected O, but got Unknown
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Expected O, but got Unknown
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Expected O, but got Unknown
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Expected O, but got Unknown
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Expected O, but got Unknown
		CSDataArray flattenArray = InGem.GetFlattenArray();
		CSGemData obj = InGemDefinition.Data as CSGemData;
		OnNurbsCurve val = InOutsideProfile.GetNurbsCurve().Duplicate();
		double value = 0.0;
		InChannelWidthOffset.GetDouble(ref value);
		double value2 = 0.0;
		InChannelDepth.GetDouble(ref value2);
		double value3 = 0.0;
		InInsideWidth.GetDouble(ref value3);
		double value4 = 0.0;
		InOutsideWidth.GetDouble(ref value4);
		double value5 = 0.0;
		InThickness.GetDouble(ref value5);
		double value6 = 0.0;
		InAngle.GetDouble(ref value6);
		value6 = CSBlockUtil.CSUtilMath.ToRadians(value6);
		double value7 = 0.0;
		InRotation.GetDouble(ref value7);
		value7 = CSBlockUtil.CSUtilMath.ToRadians(value7);
		double value8 = 0.5;
		InGirdleSpacing.GetDouble(ref value8);
		double value9 = 0.0;
		InPlacement.GetDouble(ref value9);
		double value10 = 0.0;
		InZOffset.GetDouble(ref value10);
		double value11 = 0.0;
		InFillet.GetDouble(ref value11);
		value11 /= 100.0;
		string output = string.Empty;
		InShape.GetString(ref output);
		bool value12 = false;
		InAngleBottom.GetBoolean(ref value12);
		double num = value / 2.0;
		double num2 = value5 - 0.02;
		if (value2 > num2)
		{
			value2 = num2;
		}
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value7, Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = obj.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateMissing("Missing Tight Bounding Box");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData = flattenArray.GetArrayItem(i).Duplicate() as CSGemData;
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var xDown))
			{
				return UpdateMissing("Gem X form");
			}
			cSGemData.ApplyTransform(xDown);
			if (output == "")
			{
				output = cSGemData.GemShape;
			}
			BoundingBox boundingBox2 = BoundingBox.Unset;
			if (!cSGemData.GetTightBoundingBox(ref boundingBox2))
			{
				return UpdateMissing("Gem Invalid Bounding Box");
			}
			double num3 = boundingBox2.Max.Y + value8 + value3 - 0.04;
			if (num > num3)
			{
				num = num3 - 0.02;
				value3 = 0.02;
			}
			else if (num + value3 > num3)
			{
				value3 = num3 - num;
			}
			On3dPoint val2 = new On3dPoint(0.0, boundingBox2.Max.Y + value8 - boundingBox.Min.Y, 0.0);
			OnXform val3 = new OnXform();
			val3.Rotation(value6, OnUtil.On_xaxis, (IOn3dPoint)(object)val2);
			OnLineCurve val4 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, val2.y + boundingBox.Max.Y + num + value4, 0.0), (IOn3dPoint)new On3dPoint(0.0, val2.y + boundingBox.Min.Y - num - value3, 0.0));
			val4.Transform((IOnXform)(object)val3);
			OnLineCurve val5 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, val2.y + boundingBox.Max.Y + num, 0.0), (IOn3dPoint)new On3dPoint(0.0, val2.y + boundingBox.Min.Y - num, 0.0));
			val5.Transform((IOnXform)(object)val3);
			On3dVector val6 = new On3dVector(OnUtil.On_zaxis);
			val6.Transform((IOnXform)(object)val3);
			val6.Reverse();
			val6.Unitize();
			OnLineCurve val7 = val5.Duplicate();
			((OnGeometry)val7).Translate((IOn3dVector)(object)(val6 * value2));
			OnLineCurve val8 = val4.Duplicate();
			((OnGeometry)val8).Translate((IOn3dVector)(object)(val6 * value5));
			On3dVector val9 = new On3dVector((IOn3dVector)(object)cSGemData.Up.Convert());
			val9.Unitize();
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, ((OnCurve)val8).PointAtEnd().y - boundingBox2.Max.Y, out OnCurve curve, true, out string error))
			{
				return UpdateError("Curve Bottom Inside:" + error);
			}
			if (value12)
			{
				((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val9) * (((OnCurve)val8).PointAtEnd().z + value9 + value10)));
			}
			else
			{
				((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val9) * (((OnCurve)val8).PointAtStart().z + value9 + value10)));
			}
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, ((OnCurve)val4).PointAtEnd().y - boundingBox2.Max.Y, out OnCurve curve2, true, out error))
			{
				return UpdateError("CurveTop Inside:" + error);
			}
			((OnGeometry)curve2).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val9) * (((OnCurve)val4).PointAtEnd().z + value9 + value10)));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, ((OnCurve)val5).PointAtEnd().y - boundingBox2.Max.Y, out OnCurve curve3, true, out error))
			{
				return UpdateError("Channel Top Inside:" + error);
			}
			((OnGeometry)curve3).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val9) * (((OnCurve)val5).PointAtEnd().z + value9 + value10)));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, ((OnCurve)val7).PointAtEnd().y - boundingBox2.Max.Y, out OnCurve curve4, true, out error))
			{
				return UpdateError("Channel Bottom Inside:" + error);
			}
			((OnGeometry)curve4).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val9) * (((OnCurve)val7).PointAtEnd().z + value9 + value10)));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, ((OnCurve)val7).PointAtStart().y - boundingBox2.Max.Y, out OnCurve curve5, true, out error))
			{
				return UpdateError("Channel Bottom Outside:" + error);
			}
			((OnGeometry)curve5).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val9) * (((OnCurve)val7).PointAtStart().z + value9 + value10)));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, ((OnCurve)val5).PointAtStart().y - boundingBox2.Max.Y, out OnCurve curve6, true, out error))
			{
				return UpdateError("Channel Top Outside:" + error);
			}
			((OnGeometry)curve6).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val9) * (((OnCurve)val5).PointAtStart().z + value9 + value10)));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, ((OnCurve)val4).PointAtStart().y - boundingBox2.Max.Y, out OnCurve curve7, true, out error))
			{
				return UpdateError("Top Outside:" + error);
			}
			((OnGeometry)curve7).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val9) * (((OnCurve)val4).PointAtStart().z + value9 + value10)));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, ((OnCurve)val8).PointAtStart().y - boundingBox2.Max.Y, out OnCurve curve8, true, out error))
			{
				return UpdateError("Channel Bottom Outside:" + error);
			}
			((OnGeometry)curve8).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val9) * (((OnCurve)val8).PointAtStart().z + value9 + value10)));
			OnBrep surface = null;
			if (value11 == 0.0)
			{
				if (!LoftCurves(curve3, curve2, ref surface))
				{
					return UpdateError("Loft Surface Top Inside");
				}
			}
			else if (!FilletCurves((IOnCurve)(object)curve3, (IOnCurve)(object)curve2, value11, ref surface))
			{
				return UpdateError("Loft Surface Top Inside");
			}
			OnBrep surface2 = null;
			if (!LoftCurves(curve4, curve3, ref surface2))
			{
				return UpdateError("Loft Channel Inside");
			}
			OnBrep surface3 = null;
			if (!LoftCurves(curve5, curve4, ref surface3))
			{
				return UpdateError("Loft Channel Top");
			}
			OnBrep surface4 = null;
			if (!LoftCurves(curve6, curve5, ref surface4))
			{
				return UpdateError("Loft Channel Outside");
			}
			OnBrep surface5 = null;
			if (value11 == 0.0)
			{
				if (!LoftCurves(curve7, curve6, ref surface5))
				{
					return UpdateError("Loft Top Outside");
				}
			}
			else if (!FilletCurves((IOnCurve)(object)curve7, (IOnCurve)(object)curve6, value11, ref surface5))
			{
				return UpdateError("Loft Top Outside");
			}
			OnBrep surface6 = null;
			if (val == null)
			{
				if (!LoftCurves(curve8, curve7, ref surface6))
				{
					return UpdateError("Loft Outside");
				}
			}
			else
			{
				On3dVector val10 = ((OnCurve)val).PointAtEnd() - (IOn3dPoint)(object)((OnCurve)val).PointAtStart();
				val10.Unitize();
				On3dVector val11 = new On3dVector(OnUtil.On_yaxis) * -1.0;
				val11.Unitize();
				OnXform val12 = new OnXform();
				val12.Rotation((IOn3dVector)(object)val10, (IOn3dVector)(object)val11, (IOn3dPoint)(object)((OnCurve)val).PointAtStart());
				val.Transform((IOnXform)(object)val12);
				double num4 = curve7.PointAtStart().DistanceTo((IOn3dPoint)(object)curve8.PointAtStart());
				double num5 = ((OnCurve)val).PointAtStart().DistanceTo((IOn3dPoint)(object)((OnCurve)val).PointAtEnd());
				double num6 = num4 / num5;
				OnXform val13 = new OnXform();
				val13.Scale(num6, num6, 1.0);
				val.Transform((IOnXform)(object)val13);
				On3dPoint val14 = curve8.PointAtStart();
				double num7 = 0.0;
				curve8.GetClosestPoint((IOn3dPoint)(object)val14, ref num7);
				curve8.TangentAt(num7).Unitize();
				On3dVector val15 = curve7.PointAtStart() - (IOn3dPoint)(object)curve8.PointAtStart();
				val15.Unitize();
				OnXform val16 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)((OnCurve)val).PointAtEnd(), OnUtil.On_zaxis, OnUtil.On_yaxis, (IOn3dPoint)(object)val14, OnUtil.On_xaxis, (IOn3dVector)(object)val15);
				val.Transform((IOnXform)(object)val16);
				OnCurve[] array = (OnCurve[])(object)new OnCurve[1] { (OnCurve)val };
				OnCurve railCurve = curve7;
				OnCurve railCurve2 = curve8;
				IOnCurve[] profiles = (IOnCurve[])(object)array;
				if (!CSUtilSweep.Sweep2NoParam((IOnCurve)(object)railCurve, (IOnCurve)(object)railCurve2, profiles, out var breps, maintainHeight: false, firstAtStart: false, closed: true, flipProfiles: false))
				{
					return UpdateError("Sweep 2");
				}
				if (breps.Length != 1)
				{
					return UpdateError("Sweep 2 Number Of Surfaces");
				}
				surface6 = breps[0];
				if (surface6 == null)
				{
					return UpdateError("Sweep 2 Surface");
				}
			}
			OnBrep surface7 = null;
			if (!LoftCurves(curve, curve8, ref surface7))
			{
				return UpdateError("Loft Surface Bottom");
			}
			OnBrep surface8 = null;
			if (!LoftCurves(curve2, curve, ref surface8))
			{
				return UpdateError("Loft Surface Inside");
			}
			if (CSUtilRhino.JoinBreps((OnBrep[])(object)new OnBrep[8] { surface8, surface, surface2, surface3, surface4, surface5, surface6, surface7 }, out var outputBreps, 0.01))
			{
				OnBrep[] array2 = outputBreps;
				foreach (OnBrep val17 in array2)
				{
					((OnGeometry)val17).Transform((IOnXform)(object)xUp.Convert());
					cSDataArray.Append(new CSOnGeometryData(val17.Convert(), xUp, applyTransform: false));
				}
			}
		}
		OutMetal.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		OutShape.SetData(new CSStringData(output));
		return base.Update();
	}

	private static bool FilletCurves(IOnCurve crvLhs, IOnCurve crvRhs, double fillet, ref OnBrep surface)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		On3dPoint val = crvLhs.PointAtStart();
		On3dPoint val2 = crvRhs.PointAtStart();
		OnLineCurve val3 = new OnLineCurve((IOn3dPoint)(object)val, (IOn3dPoint)(object)val2);
		On3dPoint val4 = ((OnCurve)val3).PointAt(((OnCurve)val3).Domain().Mid());
		double num = 0.0;
		if (!((OnCurve)val3).GetLength(ref num))
		{
			return false;
		}
		double num2 = num / 2.0;
		On3dPoint val5 = val4;
		val5.z += num2 * fillet;
		On3dPoint val6 = val2;
		OnArcCurve val7 = new OnArcCurve((IOnArc)new OnArc((IOn3dPoint)(object)val, (IOn3dPoint)(object)val5, (IOn3dPoint)(object)val6));
		OnNurbsCurve val8 = new OnNurbsCurve();
		((OnCurve)val7).GetNurbForm(ref val8);
		OnNurbsCurve val9 = new OnNurbsCurve();
		crvLhs.GetNurbForm(ref val9);
		OnCurve[] array = (OnCurve[])(object)new OnCurve[1] { (OnCurve)val8 };
		OnBrep[] breps = null;
		IOnCurve[] profiles = (IOnCurve[])(object)array;
		if (!CSUtilSweep.Sweep2NoParam(crvLhs, crvRhs, profiles, out breps, maintainHeight: false, firstAtStart: false, closed: true, flipProfiles: false))
		{
			return false;
		}
		if (breps.Length != 1)
		{
			return false;
		}
		surface = breps[0];
		return true;
	}

	private static bool LoftCurves(OnCurve crvLhs, OnCurve crvRhs, ref OnBrep surface)
	{
		OnCurve[] array = (OnCurve[])(object)new OnCurve[2] { crvLhs, crvRhs };
		IOnCurve[] profiles = (IOnCurve[])(object)array;
		surface = CSUtilSweep.Loft(profiles, (eLoftType)0);
		if (surface == null)
		{
			return false;
		}
		return true;
	}
}
