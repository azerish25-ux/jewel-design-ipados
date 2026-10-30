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
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Expected O, but got Unknown
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Expected O, but got Unknown
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Expected O, but got Unknown
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Expected O, but got Unknown
		//IL_0960: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Expected O, but got Unknown
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
			OnCurve val = array[0];
			OnCurve val2 = array[1];
			OnCurve val3 = array[2];
			OnCurve val4 = array[3];
			OnCurve val5 = array[4];
			if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
			{
				(IOnCurve)val,
				(IOnCurve)val5
			}, out OnCurve mergedCrv, 0.01))
			{
				return UpdateError("CurveTop Merge Error");
			}
			double num2 = 0.0;
			if (!mergedCrv.GetLength(ref num2))
			{
				return UpdateError("curveTop length error");
			}
			double num3 = 0.0;
			if (!val2.GetLength(ref num3))
			{
				return UpdateError("CurveRight length error");
			}
			double num4 = 0.0;
			if (!val3.GetLength(ref num4))
			{
				return UpdateError("CurveBottom length error");
			}
			double num5 = 0.0;
			if (!val4.GetLength(ref num5))
			{
				return UpdateError("CurveLength length error");
			}
			int num6 = (int)((num2 + num3 + num4 + num5) / (num + value));
			if (value7 && num6 % 2 != 0)
			{
				num6--;
			}
			int num7 = num6 / 4;
			int num8 = num6 / 4;
			int num9 = num6 / 4;
			int num10 = num6 / 4;
			int num11 = num6 % 4;
			if (cSGemData2.GemXSize >= cSGemData2.GemYSize)
			{
				if (num11 > 0)
				{
					num7++;
				}
				if (num11 > 1)
				{
					num9++;
				}
				if (num11 > 2)
				{
					num10++;
				}
			}
			else
			{
				if (num11 > 0)
				{
					num10++;
				}
				if (num11 > 1)
				{
					num8++;
				}
				if (num11 > 2)
				{
					num7++;
				}
			}
			double[] array2 = new double[num7];
			double[] array3 = new double[num7];
			if (num7 > 0)
			{
				for (int j = 0; j < num7; j++)
				{
					array2[j] = GetUnnormalizedParam(j, num7, value6);
				}
				mergedCrv.GetNormalizedArcLengthPoints(array2, ref array3);
			}
			double[] array4 = new double[num8];
			double[] array5 = new double[num8];
			if (num8 > 0)
			{
				for (int k = 0; k < num8; k++)
				{
					array4[k] = GetUnnormalizedParam(k, num8, value6);
				}
				val2.GetNormalizedArcLengthPoints(array4, ref array5);
			}
			double[] array6 = new double[num9];
			double[] array7 = new double[num9];
			if (num9 > 0)
			{
				for (int l = 0; l < num9; l++)
				{
					array6[l] = GetUnnormalizedParam(l, num9, value6);
				}
				val3.GetNormalizedArcLengthPoints(array6, ref array7);
			}
			double[] array8 = new double[num10];
			double[] array9 = new double[num10];
			if (num10 > 0)
			{
				for (int m = 0; m < num10; m++)
				{
					array8[m] = GetUnnormalizedParam(m, num10, value6);
				}
				val4.GetNormalizedArcLengthPoints(array8, ref array9);
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			OnInterval val6 = mergedCrv.Domain();
			for (int n = 0; n < num7; n++)
			{
				OnXform val7 = new OnXform();
				val7 = ((array3[n] != val6.Max()) ? ((array3[n] != val6.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)mergedCrv.PointAt(array3[n]), (IOn3dVector)(object)mergedCrv.TangentAt(array3[n]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(1.0, -1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)mergedCrv.PointAt(array3[n]), (IOn3dVector)(object)mergedCrv.TangentAt(array3[n]), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(-1.0, 1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)mergedCrv.PointAt(array3[n]), (IOn3dVector)(object)mergedCrv.TangentAt(array3[n]), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, val7.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data);
			}
			OnInterval val8 = val2.Domain();
			for (int num12 = 0; num12 < num8; num12++)
			{
				OnXform xform = ((array5[num12] != val8.Max()) ? ((array5[num12] != val8.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val2.PointAt(array5[num12]), (IOn3dVector)(object)val2.TangentAt(array5[num12]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(1.0, -1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val2.PointAt(array5[num12]), (IOn3dVector)(object)val2.TangentAt(array5[num12]), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(-1.0, 1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val2.PointAt(array5[num12]), (IOn3dVector)(object)val2.TangentAt(array5[num12]), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data2 = cSGemData.Duplicate();
				CSDataHelper.Transform(data2, xform.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data2);
			}
			OnInterval val9 = val3.Domain();
			for (int num13 = 0; num13 < num9; num13++)
			{
				OnXform xform2 = ((array7[num13] != val9.Max()) ? ((array7[num13] != val9.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val3.PointAt(array7[num13]), (IOn3dVector)(object)val3.TangentAt(array7[num13]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(1.0, -1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val3.PointAt(array7[num13]), (IOn3dVector)(object)val3.TangentAt(array7[num13]), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(-1.0, 1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val3.PointAt(array7[num13]), (IOn3dVector)(object)val3.TangentAt(array7[num13]), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data3 = cSGemData.Duplicate();
				CSDataHelper.Transform(data3, xform2.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data3);
			}
			OnInterval val10 = val4.Domain();
			for (int num14 = 0; num14 < num10; num14++)
			{
				OnXform xform3 = ((array9[num14] != val10.Max()) ? ((array9[num14] != val10.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(array9[num14]), (IOn3dVector)(object)val4.TangentAt(array9[num14]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(1.0, -1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(array9[num14]), (IOn3dVector)(object)val4.TangentAt(array9[num14]), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(-1.0, 1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(array9[num14]), (IOn3dVector)(object)val4.TangentAt(array9[num14]), (IOn3dVector)(object)cSGemData2.Up.Convert()));
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
