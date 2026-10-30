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
[CSBlock("ClusterMarquise_V1", "{64CF8156-C6C4-4c72-A9F2-FEBAB0FC7EE8}")]
public sealed class CSClusterMarquise1 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iCount { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iRotation { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSOutputChannel oGem { get; }

	public CSClusterMarquise1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iCount = new CSInputChannel(GetPropertyName(() => iCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(GetPropertyName(() => iRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(GetPropertyName(() => oGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Expected O, but got Unknown
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iGem.GetFlattenArray();
		CSGemData cSGemData = iGemDefinition.Data as CSGemData;
		int value = 0;
		iCount.GetInteger(ref value);
		if (value <= 0)
		{
			return UpdateMissing("Gem Input");
		}
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
		Transform val = CSBlockUtil.CSUtilMath.Rotation(value3, Vector3d.ZAxis, Point3d.Origin);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		Transform val3 = Transform.Translation(0.0, 0.0, value5) * val2 * val;
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, val3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("BoundingBox Error");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData2 = flattenArray.GetArrayItem(i) as CSGemData;
			CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData2.Xform.Convert(), out var _, out var _);
			double num = value4;
			Point3d min = ((BoundingBox)(ref boundingBox)).Min;
			OnCurve[] array = GemOffsetCurveV1.GemOffsetCurvesOld("Marquise", cSGemData2, num + Math.Abs(((Point3d)(ref min)).Y), doTransformUp: true);
			if (array.Length != 2)
			{
				return UpdateError("GemOffsetCurve Failed");
			}
			OnCurve val4 = array[0];
			OnCurve val5 = array[1];
			int num2 = value / 2 + value % 2;
			double[] array2 = new double[num2];
			double[] array3 = new double[num2];
			if (num2 > 0)
			{
				for (int j = 0; j < num2; j++)
				{
					array2[j] = (double)j / (double)num2;
				}
				val4.GetNormalizedArcLengthPoints(array2, ref array3);
			}
			int num3 = value / 2;
			double[] array4 = new double[num3];
			double[] array5 = new double[num3];
			if (num3 > 0)
			{
				for (int k = 0; k < num3; k++)
				{
					array4[k] = (double)k / (double)num3;
				}
				val5.GetNormalizedArcLengthPoints(array4, ref array5);
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			OnInterval val6 = val4.Domain();
			for (int l = 0; l < num2; l++)
			{
				OnXform xform = ((array3[l] != val6.Max()) ? ((array3[l] != val6.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(array3[l]), (IOn3dVector)(object)val4.TangentAt(array3[l]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(array3[l]), (IOn3dVector)(object)cSGemData2.North.Convert(), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(array3[l]), (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSGemData2.North.Convert())), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, xform.Convert() * val3);
				cSDataArray2.Append(data);
			}
			OnInterval val7 = val5.Domain();
			for (int m = 0; m < num3; m++)
			{
				OnXform xform2 = ((array5[m] != val7.Max()) ? ((array5[m] != val7.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val5.PointAt(array5[m]), (IOn3dVector)(object)val5.TangentAt(array5[m]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val5.PointAt(array5[m]), (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSGemData2.North.Convert())), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val5.PointAt(array5[m]), (IOn3dVector)(object)cSGemData2.North.Convert(), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data2 = cSGemData.Duplicate();
				CSDataHelper.Transform(data2, xform2.Convert() * val3);
				cSDataArray2.Append(data2);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		return base.Update();
	}
}
