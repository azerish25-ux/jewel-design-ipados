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
			double num2 = boundingBox2.Max.Y + value5 - 0.04;
			if (num > num2)
			{
				num = num2;
			}
			On3dPoint on3dPoint = new On3dPoint(0.0, boundingBox2.Max.Y + value5 - boundingBox.Min.Y, 0.0);
			OnXform onXform = new OnXform();
			onXform.Rotation(value3, OnUtil.On_xaxis, on3dPoint);
			OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, on3dPoint.y + boundingBox.Max.Y + num, 0.0), new On3dPoint(0.0, on3dPoint.y + boundingBox.Min.Y - num, 0.0));
			onLineCurve.Transform(onXform);
			On3dVector on3dVector = new On3dVector(OnUtil.On_zaxis);
			on3dVector.Transform(onXform);
			on3dVector.Reverse();
			on3dVector.Unitize();
			OnLineCurve onLineCurve2 = onLineCurve.Duplicate();
			onLineCurve2.Translate(on3dVector * value2);
			On3dVector on3dVector2 = new On3dVector(cSGemData2.Up.Convert());
			on3dVector2.Unitize();
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData2, onLineCurve2.PointAtEnd().y - boundingBox2.Max.Y, out OnCurve curve, true, out string error))
			{
				return UpdateError(error);
			}
			if (value8)
			{
				curve.Translate(new On3dVector(on3dVector2) * (onLineCurve2.PointAtEnd().z + value6 + value7));
			}
			else
			{
				curve.Translate(new On3dVector(on3dVector2) * (onLineCurve2.PointAtStart().z + value6 + value7));
			}
			OnCurve onCurve = curve.DuplicateCurve();
			onCurve.Transform(xUp.Convert());
			cSDataArray2.Append(new CSOnGeometryData(onCurve.Convert(), xUp, applyTransform: false));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData2, onLineCurve.PointAtEnd().y - boundingBox2.Max.Y, out OnCurve curve2, true, out error))
			{
				return UpdateError(error);
			}
			curve2.Translate(new On3dVector(on3dVector2) * (onLineCurve.PointAtEnd().z + value6 + value7));
			OnCurve onCurve2 = curve2.DuplicateCurve();
			onCurve2.Transform(xUp.Convert());
			cSDataArray2.Append(new CSOnGeometryData(onCurve2.Convert(), xUp, applyTransform: false));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData2, onLineCurve.PointAtStart().y - boundingBox2.Max.Y, out OnCurve curve3, true, out error))
			{
				return UpdateError(error);
			}
			curve3.Translate(new On3dVector(on3dVector2) * (onLineCurve.PointAtStart().z + value6 + value7));
			OnCurve onCurve3 = curve3.DuplicateCurve();
			onCurve3.Transform(xUp.Convert());
			cSDataArray2.Append(new CSOnGeometryData(onCurve3.Convert(), xUp, applyTransform: false));
			if (!GemOffsetCurveV1.GemOffset(output, cSGemData2, onLineCurve2.PointAtStart().y - boundingBox2.Max.Y, out OnCurve curve4, true, out error))
			{
				return UpdateError(error);
			}
			curve4.Translate(new On3dVector(on3dVector2) * (onLineCurve2.PointAtStart().z + value6 + value7));
			OnCurve onCurve4 = curve4.DuplicateCurve();
			onCurve4.Transform(xUp.Convert());
			cSDataArray2.Append(new CSOnGeometryData(onCurve4.Convert(), xUp, applyTransform: false));
			OnBrep srf = null;
			if (!SweepNoProfile(curve3, curve2, ref srf))
			{
				return UpdateError("Sweep");
			}
			OnBrep srf2 = null;
			if (nurbsCurve == null)
			{
				if (!SweepNoProfile(curve4, curve3, ref srf2))
				{
					return UpdateError("Sweep Outside Surface");
				}
			}
			else
			{
				On3dPoint on3dPoint2 = curve4.PointAtStart();
				On3dVector on3dVector3 = curve4.TangentAt(curve4.Domain().Min());
				on3dVector3.Unitize();
				On3dPoint on3dPoint3 = curve3.PointAtStart();
				On3dVector on3dVector4 = curve3.TangentAt(curve3.Domain().Min());
				on3dVector4.Unitize();
				On3dPoint on3dPoint4 = new On3dPoint(on3dPoint2);
				On3dVector on3dVector5 = new On3dVector(on3dPoint3 - on3dPoint2);
				on3dVector5.Unitize();
				On3dVector on3dVector6 = new On3dVector(on3dVector4 + on3dVector3);
				on3dVector6.Unitize();
				OnXform onXform2 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, on3dPoint4, on3dVector5, on3dVector6);
				double num3 = Math.Abs(nurbsCurve.PointAtStart().DistanceTo(nurbsCurve.PointAtEnd()));
				double scale_factor = Math.Abs(on3dPoint2.DistanceTo(on3dPoint3)) / num3;
				OnXform onXform3 = new OnXform();
				onXform3.Identity();
				onXform3.Scale(on3dPoint4, scale_factor);
				OnCurve onCurve5 = nurbsCurve.Duplicate();
				onCurve5.Transform(onXform3 * onXform2);
				OnCurve onCurve6 = onCurve5.DuplicateCurve();
				onCurve6.Transform(xUp.Convert());
				cSDataArray2.Append(new CSOnGeometryData(onCurve6.Convert(), xUp, applyTransform: false));
				OnCurve[] array = new OnCurve[1] { onCurve5 };
				double[] array2 = new double[1];
				double[] array3 = new double[1];
				array2[0] = curve4.Domain().Min();
				array3[0] = curve3.Domain().Min();
				OnCurve railCurve = curve4;
				OnCurve railCurve2 = curve3;
				IOnCurve[] profiles = array;
				CSUtilSweep.Sweep2(railCurve, railCurve2, profiles, array2, array3, out var breps);
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
			if (!SweepNoProfile(curve, curve4, ref srf3))
			{
				return UpdateError("Sweep Surface Bottom");
			}
			OnBrep srf4 = null;
			if (!SweepNoProfile(curve2, curve, ref srf4))
			{
				return UpdateError("Sweep Surface Inside");
			}
			if (CSUtilRhino.JoinBreps(new OnBrep[4] { srf4, srf, srf2, srf3 }, out var outputBreps, 0.01))
			{
				OnBrep[] array4 = outputBreps;
				foreach (OnBrep onBrep in array4)
				{
					onBrep.Transform(xUp.Convert());
					cSDataArray.Append(new CSOnGeometryData(onBrep.Convert(), xUp, applyTransform: false));
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
		if (!breps[0].IsValid())
		{
			return false;
		}
		srf = breps[0];
		return true;
	}
}
