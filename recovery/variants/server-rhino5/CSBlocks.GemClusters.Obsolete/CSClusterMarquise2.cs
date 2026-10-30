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
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Expected O, but got Unknown
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
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value3, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = Transform.Translation(0.0, 0.0, value5);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("BoundingBox Error");
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				return UpdateMissing("Gem");
			}
			OnCurve[] array = GemOffsetCurveV1.GemOffsetCurvesOld("Marquise", cSGemData2, value4 + Math.Abs(boundingBox.Min.Y), doTransformUp: true);
			if (array.Length != 2)
			{
				return UpdateError("GemOffsetCurve Failed");
			}
			OnCurve val = array[0];
			OnCurve val2 = array[1];
			double num2 = 0.0;
			if (!val.GetLength(ref num2))
			{
				return UpdateError("curveLHS length failed");
			}
			double num3 = 0.0;
			if (!val2.GetLength(ref num3))
			{
				return UpdateError("curveRHS length failed");
			}
			int num4 = (int)((num2 + num3) / (num + value));
			if (value6 && num4 % 2 != 0)
			{
				num4--;
			}
			int num5 = num4 / 2 + num4 % 2;
			double[] array2 = new double[num5];
			double[] array3 = new double[num5];
			if (num5 > 0)
			{
				for (int j = 0; j < num5; j++)
				{
					array2[j] = (double)j / (double)num5;
				}
				val.GetNormalizedArcLengthPoints(array2, ref array3);
			}
			int num6 = num4 / 2;
			double[] array4 = new double[num6];
			double[] array5 = new double[num6];
			if (num6 > 0)
			{
				for (int k = 0; k < num6; k++)
				{
					array4[k] = (double)k / (double)num6;
				}
				val2.GetNormalizedArcLengthPoints(array4, ref array5);
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			OnInterval val3 = val.Domain();
			for (int l = 0; l < num5; l++)
			{
				OnXform xform = ((array3[l] != val3.Max()) ? ((array3[l] != val3.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val.PointAt(array3[l]), (IOn3dVector)(object)val.TangentAt(array3[l]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val.PointAt(array3[l]), (IOn3dVector)(object)cSGemData2.North.Convert(), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val.PointAt(array3[l]), (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSGemData2.North.Convert())), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, xform.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data);
			}
			OnInterval val4 = val2.Domain();
			for (int m = 0; m < num6; m++)
			{
				OnXform xform2 = ((array5[m] != val4.Max()) ? ((array5[m] != val4.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val2.PointAt(array5[m]), (IOn3dVector)(object)val2.TangentAt(array5[m]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val2.PointAt(array5[m]), (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSGemData2.North.Convert())), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val2.PointAt(array5[m]), (IOn3dVector)(object)cSGemData2.North.Convert(), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data2 = cSGemData.Duplicate();
				CSDataHelper.Transform(data2, xform2.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data2);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		oShape.SetData(new CSStringData("Marquise"));
		return base.Update();
	}
}
