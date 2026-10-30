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
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Expected O, but got Unknown
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Expected O, but got Unknown
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Expected O, but got Unknown
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Expected O, but got Unknown
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Unknown result type (might be due to invalid IL or missing references)
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Expected O, but got Unknown
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0960: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Expected O, but got Unknown
		//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
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
		Transform val = CSBlockUtil.CSUtilMath.Rotation(value3, Vector3d.ZAxis, Point3d.Origin);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		Transform val3 = Transform.Translation(0.0, 0.0, value5);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, val);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Boundingbox Error");
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
				continue;
			}
			double num2 = value4;
			val4 = ((BoundingBox)(ref boundingBox)).Min;
			OnCurve[] array = GemOffsetCurveV1.GemOffsetCurvesOld("Baguette Straight", cSGemData2, num2 + Math.Abs(((Point3d)(ref val4)).Y), doTransformUp: true);
			if (array.Length != 5)
			{
				return UpdateError("Curve Count Error");
			}
			OnCurve val5 = array[0];
			OnCurve val6 = array[1];
			OnCurve val7 = array[2];
			OnCurve val8 = array[3];
			OnCurve val9 = array[4];
			if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
			{
				(IOnCurve)val5,
				(IOnCurve)val9
			}, out OnCurve mergedCrv, 0.01))
			{
				return UpdateError("CurveTop Merge Error");
			}
			double num3 = 0.0;
			if (!mergedCrv.GetLength(ref num3))
			{
				return UpdateError("curveTop length error");
			}
			double num4 = 0.0;
			if (!val6.GetLength(ref num4))
			{
				return UpdateError("CurveRight length error");
			}
			double num5 = 0.0;
			if (!val7.GetLength(ref num5))
			{
				return UpdateError("CurveBottom length error");
			}
			double num6 = 0.0;
			if (!val8.GetLength(ref num6))
			{
				return UpdateError("CurveLength length error");
			}
			int num7 = (int)((num3 + num4 + num5 + num6) / (num + value));
			if (value7 && num7 % 2 != 0)
			{
				num7--;
			}
			int num8 = num7 / 4;
			int num9 = num7 / 4;
			int num10 = num7 / 4;
			int num11 = num7 / 4;
			int num12 = num7 % 4;
			if (cSGemData2.GemXSize >= cSGemData2.GemYSize)
			{
				if (num12 > 0)
				{
					num8++;
				}
				if (num12 > 1)
				{
					num10++;
				}
				if (num12 > 2)
				{
					num11++;
				}
			}
			else
			{
				if (num12 > 0)
				{
					num11++;
				}
				if (num12 > 1)
				{
					num9++;
				}
				if (num12 > 2)
				{
					num8++;
				}
			}
			double[] array2 = new double[num8];
			double[] array3 = new double[num8];
			if (num8 > 0)
			{
				for (int j = 0; j < num8; j++)
				{
					array2[j] = GetUnnormalizedParam(j, num8, value6);
				}
				mergedCrv.GetNormalizedArcLengthPoints(array2, ref array3);
			}
			double[] array4 = new double[num9];
			double[] array5 = new double[num9];
			if (num9 > 0)
			{
				for (int k = 0; k < num9; k++)
				{
					array4[k] = GetUnnormalizedParam(k, num9, value6);
				}
				val6.GetNormalizedArcLengthPoints(array4, ref array5);
			}
			double[] array6 = new double[num10];
			double[] array7 = new double[num10];
			if (num10 > 0)
			{
				for (int l = 0; l < num10; l++)
				{
					array6[l] = GetUnnormalizedParam(l, num10, value6);
				}
				val7.GetNormalizedArcLengthPoints(array6, ref array7);
			}
			double[] array8 = new double[num11];
			double[] array9 = new double[num11];
			if (num11 > 0)
			{
				for (int m = 0; m < num11; m++)
				{
					array8[m] = GetUnnormalizedParam(m, num11, value6);
				}
				val8.GetNormalizedArcLengthPoints(array8, ref array9);
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			OnInterval val10 = mergedCrv.Domain();
			for (int n = 0; n < num8; n++)
			{
				OnXform val11 = new OnXform();
				val11 = ((array3[n] != val10.Max()) ? ((array3[n] != val10.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)mergedCrv.PointAt(array3[n]), (IOn3dVector)(object)mergedCrv.TangentAt(array3[n]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(1.0, -1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)mergedCrv.PointAt(array3[n]), (IOn3dVector)(object)mergedCrv.TangentAt(array3[n]), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(-1.0, 1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)mergedCrv.PointAt(array3[n]), (IOn3dVector)(object)mergedCrv.TangentAt(array3[n]), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, val11.Convert() * val3 * val2 * val);
				cSDataArray2.Append(data);
			}
			OnInterval val12 = val6.Domain();
			for (int num13 = 0; num13 < num9; num13++)
			{
				OnXform xform = ((array5[num13] != val12.Max()) ? ((array5[num13] != val12.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val6.PointAt(array5[num13]), (IOn3dVector)(object)val6.TangentAt(array5[num13]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(1.0, -1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val6.PointAt(array5[num13]), (IOn3dVector)(object)val6.TangentAt(array5[num13]), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(-1.0, 1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val6.PointAt(array5[num13]), (IOn3dVector)(object)val6.TangentAt(array5[num13]), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data2 = cSGemData.Duplicate();
				CSDataHelper.Transform(data2, xform.Convert() * val3 * val2 * val);
				cSDataArray2.Append(data2);
			}
			OnInterval val13 = val7.Domain();
			for (int num14 = 0; num14 < num10; num14++)
			{
				OnXform xform2 = ((array7[num14] != val13.Max()) ? ((array7[num14] != val13.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val7.PointAt(array7[num14]), (IOn3dVector)(object)val7.TangentAt(array7[num14]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(1.0, -1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val7.PointAt(array7[num14]), (IOn3dVector)(object)val7.TangentAt(array7[num14]), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(-1.0, 1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val7.PointAt(array7[num14]), (IOn3dVector)(object)val7.TangentAt(array7[num14]), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data3 = cSGemData.Duplicate();
				CSDataHelper.Transform(data3, xform2.Convert() * val3 * val2 * val);
				cSDataArray2.Append(data3);
			}
			OnInterval val14 = val8.Domain();
			for (int num15 = 0; num15 < num11; num15++)
			{
				OnXform xform3 = ((array9[num15] != val14.Max()) ? ((array9[num15] != val14.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val8.PointAt(array9[num15]), (IOn3dVector)(object)val8.TangentAt(array9[num15]), (IOn3dVector)(object)cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(1.0, -1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val8.PointAt(array9[num15]), (IOn3dVector)(object)val8.TangentAt(array9[num15]), (IOn3dVector)(object)cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(-1.0, 1.0, 0.0), (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)val8.PointAt(array9[num15]), (IOn3dVector)(object)val8.TangentAt(array9[num15]), (IOn3dVector)(object)cSGemData2.Up.Convert()));
				CSData data4 = cSGemData.Duplicate();
				CSDataHelper.Transform(data4, xform3.Convert() * val3 * val2 * val);
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
