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
		CSDataArray flattenArray = InGem.GetFlattenArray();
		CSGemData obj = InGemDefinition.Data as CSGemData;
		OnNurbsCurve onNurbsCurve = InOutsideProfile.GetNurbsCurve().Duplicate();
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
			On3dPoint on3dPoint = new On3dPoint(0.0, boundingBox2.Max.Y + value8 - boundingBox.Min.Y, 0.0);
			OnXform onXform = new OnXform();
			onXform.Rotation(value6, OnUtil.On_xaxis, on3dPoint);
			OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, on3dPoint.y + boundingBox.Max.Y + num + value4, 0.0), new On3dPoint(0.0, on3dPoint.y + boundingBox.Min.Y - num - value3, 0.0));
			onLineCurve.Transform(onXform);
			OnLineCurve onLineCurve2 = new OnLineCurve(new On3dPoint(0.0, on3dPoint.y + boundingBox.Max.Y + num, 0.0), new On3dPoint(0.0, on3dPoint.y + boundingBox.Min.Y - num, 0.0));
			onLineCurve2.Transform(onXform);
			On3dVector on3dVector = new On3dVector(OnUtil.On_zaxis);
			on3dVector.Transform(onXform);
			on3dVector.Reverse();
			on3dVector.Unitize();
			OnLineCurve onLineCurve3 = onLineCurve2.Duplicate();
			onLineCurve3.Translate(on3dVector * value2);
			OnLineCurve onLineCurve4 = onLineCurve.Duplicate();
			onLineCurve4.Translate(on3dVector * value5);
			On3dVector on3dVector2 = new On3dVector(cSGemData.Up.Convert());
			on3dVector2.Unitize();
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, onLineCurve4.PointAtEnd().y - boundingBox2.Max.Y, out OnCurve curve, true, out string error))
			{
				return UpdateError("Curve Bottom Inside:" + error);
			}
			if (value12)
			{
				curve.Translate(new On3dVector(on3dVector2) * (onLineCurve4.PointAtEnd().z + value9 + value10));
			}
			else
			{
				curve.Translate(new On3dVector(on3dVector2) * (onLineCurve4.PointAtStart().z + value9 + value10));
			}
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, onLineCurve.PointAtEnd().y - boundingBox2.Max.Y, out OnCurve curve2, true, out error))
			{
				return UpdateError("CurveTop Inside:" + error);
			}
			curve2.Translate(new On3dVector(on3dVector2) * (onLineCurve.PointAtEnd().z + value9 + value10));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, onLineCurve2.PointAtEnd().y - boundingBox2.Max.Y, out OnCurve curve3, true, out error))
			{
				return UpdateError("Channel Top Inside:" + error);
			}
			curve3.Translate(new On3dVector(on3dVector2) * (onLineCurve2.PointAtEnd().z + value9 + value10));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, onLineCurve3.PointAtEnd().y - boundingBox2.Max.Y, out OnCurve curve4, true, out error))
			{
				return UpdateError("Channel Bottom Inside:" + error);
			}
			curve4.Translate(new On3dVector(on3dVector2) * (onLineCurve3.PointAtEnd().z + value9 + value10));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, onLineCurve3.PointAtStart().y - boundingBox2.Max.Y, out OnCurve curve5, true, out error))
			{
				return UpdateError("Channel Bottom Outside:" + error);
			}
			curve5.Translate(new On3dVector(on3dVector2) * (onLineCurve3.PointAtStart().z + value9 + value10));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, onLineCurve2.PointAtStart().y - boundingBox2.Max.Y, out OnCurve curve6, true, out error))
			{
				return UpdateError("Channel Top Outside:" + error);
			}
			curve6.Translate(new On3dVector(on3dVector2) * (onLineCurve2.PointAtStart().z + value9 + value10));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, onLineCurve.PointAtStart().y - boundingBox2.Max.Y, out OnCurve curve7, true, out error))
			{
				return UpdateError("Top Outside:" + error);
			}
			curve7.Translate(new On3dVector(on3dVector2) * (onLineCurve.PointAtStart().z + value9 + value10));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData, onLineCurve4.PointAtStart().y - boundingBox2.Max.Y, out OnCurve curve8, true, out error))
			{
				return UpdateError("Channel Bottom Outside:" + error);
			}
			curve8.Translate(new On3dVector(on3dVector2) * (onLineCurve4.PointAtStart().z + value9 + value10));
			OnBrep surface = null;
			if (value11 == 0.0)
			{
				if (!LoftCurves(curve3, curve2, ref surface))
				{
					return UpdateError("Loft Surface Top Inside");
				}
			}
			else if (!FilletCurves(curve3, curve2, value11, ref surface))
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
			else if (!FilletCurves(curve7, curve6, value11, ref surface5))
			{
				return UpdateError("Loft Top Outside");
			}
			OnBrep surface6 = null;
			if (onNurbsCurve == null)
			{
				if (!LoftCurves(curve8, curve7, ref surface6))
				{
					return UpdateError("Loft Outside");
				}
			}
			else
			{
				On3dVector on3dVector3 = onNurbsCurve.PointAtEnd() - onNurbsCurve.PointAtStart();
				on3dVector3.Unitize();
				On3dVector on3dVector4 = new On3dVector(OnUtil.On_yaxis) * -1.0;
				on3dVector4.Unitize();
				OnXform onXform2 = new OnXform();
				onXform2.Rotation(on3dVector3, on3dVector4, onNurbsCurve.PointAtStart());
				onNurbsCurve.Transform(onXform2);
				double num4 = curve7.PointAtStart().DistanceTo(curve8.PointAtStart());
				double num5 = onNurbsCurve.PointAtStart().DistanceTo(onNurbsCurve.PointAtEnd());
				double num6 = num4 / num5;
				OnXform onXform3 = new OnXform();
				onXform3.Scale(num6, num6, 1.0);
				onNurbsCurve.Transform(onXform3);
				On3dPoint on3dPoint2 = curve8.PointAtStart();
				double t = 0.0;
				curve8.GetClosestPoint(on3dPoint2, ref t);
				curve8.TangentAt(t).Unitize();
				On3dVector on3dVector5 = curve7.PointAtStart() - curve8.PointAtStart();
				on3dVector5.Unitize();
				OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(onNurbsCurve.PointAtEnd(), OnUtil.On_zaxis, OnUtil.On_yaxis, on3dPoint2, OnUtil.On_xaxis, on3dVector5);
				onNurbsCurve.Transform(xform);
				OnCurve[] array = new OnCurve[1] { onNurbsCurve };
				OnCurve railCurve = curve7;
				OnCurve railCurve2 = curve8;
				IOnCurve[] profiles = array;
				if (!CSUtilSweep.Sweep2NoParam(railCurve, railCurve2, profiles, out var breps, maintainHeight: false, firstAtStart: false, closed: true, flipProfiles: false))
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
			if (CSUtilRhino.JoinBreps(new OnBrep[8] { surface8, surface, surface2, surface3, surface4, surface5, surface6, surface7 }, out var outputBreps, 0.01))
			{
				OnBrep[] array2 = outputBreps;
				foreach (OnBrep onBrep in array2)
				{
					onBrep.Transform(xUp.Convert());
					cSDataArray.Append(new CSOnGeometryData(onBrep.Convert(), xUp, applyTransform: false));
				}
			}
		}
		OutMetal.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		OutShape.SetData(new CSStringData(output));
		return base.Update();
	}

	private static bool FilletCurves(IOnCurve crvLhs, IOnCurve crvRhs, double fillet, ref OnBrep surface)
	{
		On3dPoint on3dPoint = crvLhs.PointAtStart();
		On3dPoint on3dPoint2 = crvRhs.PointAtStart();
		OnLineCurve onLineCurve = new OnLineCurve(on3dPoint, on3dPoint2);
		On3dPoint on3dPoint3 = onLineCurve.PointAt(onLineCurve.Domain().Mid());
		double length = 0.0;
		if (!onLineCurve.GetLength(ref length))
		{
			return false;
		}
		double num = length / 2.0;
		On3dPoint on3dPoint4 = on3dPoint3;
		on3dPoint4.z += num * fillet;
		On3dPoint r = on3dPoint2;
		OnArcCurve onArcCurve = new OnArcCurve(new OnArc(on3dPoint, on3dPoint4, r));
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onArcCurve.GetNurbForm(ref nurbs_curve);
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		crvLhs.GetNurbForm(ref nurbs_curve2);
		OnCurve[] array = new OnCurve[1] { nurbs_curve };
		OnBrep[] breps = null;
		IOnCurve[] profiles = array;
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
		OnCurve[] array = new OnCurve[2] { crvLhs, crvRhs };
		IOnCurve[] profiles = array;
		surface = CSUtilSweep.Loft(profiles, IArgsRhinoLoft.eLoftType.ltNormal);
		if (surface == null)
		{
			return false;
		}
		return true;
	}
}
