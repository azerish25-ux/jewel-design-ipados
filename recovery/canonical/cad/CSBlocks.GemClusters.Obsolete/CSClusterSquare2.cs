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
[CSBlock("ClusterSquare_V2", "{FA0D8A4F-5303-4242-B9A1-39B91E15B8A0}")]
public sealed class CSClusterSquare2 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iSpacing { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iRotation { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iEven { get; }

	public CSOutputChannel oGem { get; }

	public CSOutputChannel oShape { get; }

	public CSClusterSquare2(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iSpacing = new CSInputChannel(GetPropertyName(() => iSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(GetPropertyName(() => iRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEven = new CSInputChannel(GetPropertyName(() => iEven), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(GetPropertyName(() => oGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
		oShape = new CSOutputChannel(GetPropertyName(() => oShape), this, ChannelDataType.String, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Gems");
		}
		if (!(iGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem");
		}
		double value = 0.0;
		iSpacing.GetDouble(ref value);
		double value2 = 0.0;
		iAngle.GetDouble(ref value2);
		value2 = CSBlockUtil.CSUtilMath.ToRadians(value2);
		double value3 = 0.0;
		iRotation.GetDouble(ref value3);
		value3 = CSBlockUtil.CSUtilMath.ToRadians(value3);
		double value4 = 0.0;
		iGirdleSpacing.GetDouble(ref value4);
		double value5 = 0.0;
		iPlacement.GetDouble(ref value5);
		int value6 = 0;
		iStyle.GetInteger(ref value6);
		bool value7 = false;
		iEven.GetBoolean(ref value7);
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value3, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = Transform.Translation(0.0, 0.0, value5);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Boundingbox Error");
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				continue;
			}
			OnCurve[] array = GemOffsetCurveV1.GemOffsetCurvesOld("Baguette Straight", cSGemData2, value4 + Math.Abs(boundingBox.Min.Y), doTransformUp: true);
			if (array.Length != 5)
			{
				return UpdateError("Curve Count Error");
			}
			OnCurve onCurve = array[0];
			OnCurve onCurve2 = array[1];
			OnCurve onCurve3 = array[2];
			OnCurve onCurve4 = array[3];
			OnCurve onCurve5 = array[4];
			if (!CSUtilCurves.MergeCurves(new IOnCurve[2] { onCurve, onCurve5 }, out OnCurve mergedCrv, 0.01))
			{
				return UpdateError("CurveTop Merge Error");
			}
			double length = 0.0;
			if (!mergedCrv.GetLength(ref length))
			{
				return UpdateError("curveTop length error");
			}
			double length2 = 0.0;
			if (!onCurve2.GetLength(ref length2))
			{
				return UpdateError("CurveRight length error");
			}
			double length3 = 0.0;
			if (!onCurve3.GetLength(ref length3))
			{
				return UpdateError("CurveBottom length error");
			}
			double length4 = 0.0;
			if (!onCurve4.GetLength(ref length4))
			{
				return UpdateError("CurveLength length error");
			}
			int num2 = (int)((length + length2 + length3 + length4) / (num + value));
			if (value7 && num2 % 2 != 0)
			{
				num2--;
			}
			int num3 = num2 / 4;
			int num4 = num2 / 4;
			int num5 = num2 / 4;
			int num6 = num2 / 4;
			int num7 = num2 % 4;
			if (cSGemData2.GemXSize >= cSGemData2.GemYSize)
			{
				if (num7 > 0)
				{
					num3++;
				}
				if (num7 > 1)
				{
					num5++;
				}
				if (num7 > 2)
				{
					num6++;
				}
			}
			else
			{
				if (num7 > 0)
				{
					num6++;
				}
				if (num7 > 1)
				{
					num4++;
				}
				if (num7 > 2)
				{
					num3++;
				}
			}
			double[] array2 = new double[num3];
			double[] t = new double[num3];
			if (num3 > 0)
			{
				for (int j = 0; j < num3; j++)
				{
					array2[j] = GetUnnormalizedParam(j, num3, value6);
				}
				mergedCrv.GetNormalizedArcLengthPoints(array2, ref t);
			}
			double[] array3 = new double[num4];
			double[] t2 = new double[num4];
			if (num4 > 0)
			{
				for (int k = 0; k < num4; k++)
				{
					array3[k] = GetUnnormalizedParam(k, num4, value6);
				}
				onCurve2.GetNormalizedArcLengthPoints(array3, ref t2);
			}
			double[] array4 = new double[num5];
			double[] t3 = new double[num5];
			if (num5 > 0)
			{
				for (int l = 0; l < num5; l++)
				{
					array4[l] = GetUnnormalizedParam(l, num5, value6);
				}
				onCurve3.GetNormalizedArcLengthPoints(array4, ref t3);
			}
			double[] array5 = new double[num6];
			double[] t4 = new double[num6];
			if (num6 > 0)
			{
				for (int m = 0; m < num6; m++)
				{
					array5[m] = GetUnnormalizedParam(m, num6, value6);
				}
				onCurve4.GetNormalizedArcLengthPoints(array5, ref t4);
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			OnInterval onInterval = mergedCrv.Domain();
			for (int n = 0; n < num3; n++)
			{
				OnXform onXform = new OnXform();
				onXform = ((t[n] != onInterval.Max()) ? ((t[n] != onInterval.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), mergedCrv.PointAt(t[n]), mergedCrv.TangentAt(t[n]), cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, new On3dVector(1.0, -1.0, 0.0), cSGemData.Up.Convert(), mergedCrv.PointAt(t[n]), mergedCrv.TangentAt(t[n]), cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, new On3dVector(-1.0, 1.0, 0.0), cSGemData.Up.Convert(), mergedCrv.PointAt(t[n]), mergedCrv.TangentAt(t[n]), cSGemData2.Up.Convert()));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, onXform.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data);
			}
			OnInterval onInterval2 = onCurve2.Domain();
			for (int num8 = 0; num8 < num4; num8++)
			{
				OnXform xform = ((t2[num8] != onInterval2.Max()) ? ((t2[num8] != onInterval2.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), onCurve2.PointAt(t2[num8]), onCurve2.TangentAt(t2[num8]), cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, new On3dVector(1.0, -1.0, 0.0), cSGemData.Up.Convert(), onCurve2.PointAt(t2[num8]), onCurve2.TangentAt(t2[num8]), cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, new On3dVector(-1.0, 1.0, 0.0), cSGemData.Up.Convert(), onCurve2.PointAt(t2[num8]), onCurve2.TangentAt(t2[num8]), cSGemData2.Up.Convert()));
				CSData data2 = cSGemData.Duplicate();
				CSDataHelper.Transform(data2, xform.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data2);
			}
			OnInterval onInterval3 = onCurve3.Domain();
			for (int num9 = 0; num9 < num5; num9++)
			{
				OnXform xform2 = ((t3[num9] != onInterval3.Max()) ? ((t3[num9] != onInterval3.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), onCurve3.PointAt(t3[num9]), onCurve3.TangentAt(t3[num9]), cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, new On3dVector(1.0, -1.0, 0.0), cSGemData.Up.Convert(), onCurve3.PointAt(t3[num9]), onCurve3.TangentAt(t3[num9]), cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, new On3dVector(-1.0, 1.0, 0.0), cSGemData.Up.Convert(), onCurve3.PointAt(t3[num9]), onCurve3.TangentAt(t3[num9]), cSGemData2.Up.Convert()));
				CSData data3 = cSGemData.Duplicate();
				CSDataHelper.Transform(data3, xform2.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data3);
			}
			OnInterval onInterval4 = onCurve4.Domain();
			for (int num10 = 0; num10 < num6; num10++)
			{
				OnXform xform3 = ((t4[num10] != onInterval4.Max()) ? ((t4[num10] != onInterval4.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), onCurve4.PointAt(t4[num10]), onCurve4.TangentAt(t4[num10]), cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, new On3dVector(1.0, -1.0, 0.0), cSGemData.Up.Convert(), onCurve4.PointAt(t4[num10]), onCurve4.TangentAt(t4[num10]), cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, new On3dVector(-1.0, 1.0, 0.0), cSGemData.Up.Convert(), onCurve4.PointAt(t4[num10]), onCurve4.TangentAt(t4[num10]), cSGemData2.Up.Convert()));
				CSData data4 = cSGemData.Duplicate();
				CSDataHelper.Transform(data4, xform3.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data4);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		oShape.SetData(new CSStringData("Baguette Straight"));
		return base.Update();
	}

	private static double GetUnnormalizedParam(int gemCurrentIndex, int gemCount, double style)
	{
		if (style == 0.0)
		{
			return (double)gemCurrentIndex / (double)gemCount;
		}
		double num = 1.0 / (2.0 * Math.Sqrt(2.0) + (double)(2 * (gemCount - 1)));
		return num * Math.Sqrt(2.0) + num * 2.0 * (double)gemCurrentIndex;
	}
}
