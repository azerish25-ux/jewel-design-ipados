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
[CSBlock("ClusterMarquise_V2", "{199A731C-57A3-4c4b-B48E-5F0CE9EBBEE1}")]
public sealed class CSClusterMarquise2 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iSpacing { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iRotation { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iEven { get; }

	public CSOutputChannel oGem { get; }

	public CSOutputChannel oShape { get; }

	public CSClusterMarquise2(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iSpacing = new CSInputChannel(GetPropertyName(() => iSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(GetPropertyName(() => iRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEven = new CSInputChannel(GetPropertyName(() => iEven), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(GetPropertyName(() => oGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
		oShape = new CSOutputChannel(GetPropertyName(() => oShape), this, ChannelDataType.String, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Expected O, but got Unknown
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
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
		bool value6 = false;
		iEven.GetBoolean(ref value6);
		Transform val = CSBlockUtil.CSUtilMath.Rotation(value3, Vector3d.ZAxis, Point3d.Origin);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		Transform val3 = Transform.Translation(0.0, 0.0, value5);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, val);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("BoundingBox Error");
		}
		Point3d val4 = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val4)).X;
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double num = x - ((Point3d)(ref val4)).X;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				return UpdateMissing("Gem");
			}
			double num2 = value4;
			val4 = ((BoundingBox)(ref boundingBox)).Min;
			OnCurve[] array = GemOffsetCurveV1.GemOffsetCurvesOld("Marquise", cSGemData2, num2 + Math.Abs(((Point3d)(ref val4)).Y), doTransformUp: true);
			if (array.Length != 2)
			{
				return UpdateError("GemOffsetCurve Failed");
			}
			OnCurve val5 = array[0];
			OnCurve val6 = array[1];
			double num3 = 0.0;
			if (!val5.GetLength(ref num3))
			{
				return UpdateError("curveLHS length failed");
			}
			double num4 = 0.0;
			if (!val6.GetLength(ref num4))
			{
				return UpdateError("curveRHS length failed");
			}
			int num5 = (int)((num3 + num4) / (num + value));
			if (value6 && num5 % 2 != 0)
			{
				num5--;
			}
			int num6 = num5 / 2 + num5 % 2;
			double[] array2 = new double[num6];
			double[] array3 = new double[num6];
			if (num6 > 0)
			{
				for (int j = 0; j < num6; j++)
				{
					array2[j] = (double)j / (double)num6;
				}
				val5.GetNormalizedArcLengthPoints(array2, ref array3);
			}
			int num7 = num5 / 2;
			double[] array4 = new double[num7];
			double[] array5 = new double[num7];
			if (num7 > 0)
			{
				for (int k = 0; k < num7; k++)
				{
					array4[k] = (double)k / (double)num7;
				}
				val6.GetNormalizedArcLengthPoints(array4, ref array5);
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			OnInterval val7 = val5.Domain();
			for (int l = 0; l < num6; l++)
			{
				OnXform xform = ((array3[l] != val7.Max()) ? ((array3[l] != val7.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val5.PointAt(array3[l]), (IOn3dVector)(object)val5.TangentAt(array3[l]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val5.PointAt(array3[l]), (IOn3dVector)(object)cSGemData2.North.Convert(), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val5.PointAt(array3[l]), (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSGemData2.North.Convert())), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, xform.Convert() * val3 * val2 * val);
				cSDataArray2.Append(data);
			}
			OnInterval val8 = val6.Domain();
			for (int m = 0; m < num7; m++)
			{
				OnXform xform2 = ((array5[m] != val8.Max()) ? ((array5[m] != val8.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val6.PointAt(array5[m]), (IOn3dVector)(object)val6.TangentAt(array5[m]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val6.PointAt(array5[m]), (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSGemData2.North.Convert())), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val6.PointAt(array5[m]), (IOn3dVector)(object)cSGemData2.North.Convert(), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data2 = cSGemData.Duplicate();
				CSDataHelper.Transform(data2, xform2.Convert() * val3 * val2 * val);
				cSDataArray2.Append(data2);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		oShape.SetData(new CSStringData("Marquise"));
		return base.Update();
	}
}
