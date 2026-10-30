using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("ClusterSquare_V1", "{DA51AEBC-2C02-4879-8C38-21A57BE955A0}")]
public sealed class CSClusterSquare1 : CSBlock
{
	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSInputChannel iCount;

	public readonly CSInputChannel iAngle;

	public readonly CSInputChannel iRotation;

	public readonly CSInputChannel iGirdleSpacing;

	public readonly CSInputChannel iPlacement;

	public readonly CSInputChannel iStyle;

	public readonly CSOutputChannel oGem;

	public CSClusterSquare1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStyle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iGem.GetFlattenArray();
		CSGemData cSGemData = iGemDefinition.Data as CSGemData;
		int value = 0;
		iCount.GetInteger(ref value);
		if (value <= 0)
		{
			return UpdateMissing("Count");
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
		double value6 = 0.0;
		iStyle.GetDouble(ref value6);
		Transform val = CSBlockUtil.CSUtilMath.Rotation(value3, Vector3d.ZAxis, Point3d.Origin);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		Transform val3 = Transform.Translation(0.0, 0.0, value5) * val2 * val;
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, val3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Boundingbox Error");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData2 = flattenArray.GetArrayItem(i) as CSGemData;
			double num = value4;
			Point3d min = ((BoundingBox)(ref boundingBox)).Min;
			Curve[] array = GemOffsetCurveV1.GemOffsetCurves("Baguette Straight", cSGemData2, num + Math.Abs(((Point3d)(ref min)).Y), doTransformUp: true);
			if (array.Length != 5)
			{
				return UpdateError("Curve Count error");
			}
			Curve val4 = array[0];
			Curve val5 = array[1];
			Curve val6 = array[2];
			Curve val7 = array[3];
			Curve val8 = array[4];
			Curve[] array2 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val4, val8 });
			if (array2 == null)
			{
				return UpdateError("Merge Curve Error");
			}
			Curve val9 = array2[0];
			int num2 = value / 4;
			int num3 = value / 4;
			int num4 = value / 4;
			int num5 = value / 4;
			int num6 = value % 4;
			if (cSGemData2.GemXSize >= cSGemData2.GemYSize)
			{
				if (num6 > 0)
				{
					num2++;
				}
				if (num6 > 1)
				{
					num4++;
				}
				if (num6 > 2)
				{
					num5++;
				}
			}
			else
			{
				if (num6 > 0)
				{
					num5++;
				}
				if (num6 > 1)
				{
					num3++;
				}
				if (num6 > 2)
				{
					num2++;
				}
			}
			double[] array3 = new double[num2];
			double[] array4 = new double[num2];
			if (num2 > 0)
			{
				for (int j = 0; j < num2; j++)
				{
					array3[j] = GemPosition(j, num2, value6);
				}
				array4 = val9.NormalizedLengthParameters(array3, CSApplicationHelper.Application.Tolerance);
			}
			double[] array5 = new double[num3];
			double[] array6 = new double[num3];
			if (num3 > 0)
			{
				for (int k = 0; k < num3; k++)
				{
					array5[k] = GemPosition(k, num3, value6);
				}
				array6 = val5.NormalizedLengthParameters(array5, CSApplicationHelper.Application.Tolerance);
			}
			double[] array7 = new double[num4];
			double[] array8 = new double[num4];
			if (num4 > 0)
			{
				for (int l = 0; l < num4; l++)
				{
					array7[l] = GemPosition(l, num4, value6);
				}
				array8 = val6.NormalizedLengthParameters(array7, CSApplicationHelper.Application.Tolerance);
			}
			double[] array9 = new double[num5];
			double[] array10 = new double[num5];
			if (num5 > 0)
			{
				for (int m = 0; m < num5; m++)
				{
					array9[m] = GemPosition(m, num5, value6);
				}
				array10 = val7.NormalizedLengthParameters(array9, CSApplicationHelper.Application.Tolerance);
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			Interval domain = val9.Domain;
			for (int n = 0; n < num2; n++)
			{
				Transform val10 = default(Transform);
				val10 = ((array4[n] != ((Interval)(ref domain)).Max) ? ((array4[n] != ((Interval)(ref domain)).Min) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, val9.PointAt(array4[n]), val9.TangentAt(array4[n]), cSGemData2.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, -1.0, 0.0), cSGemData.Up, val9.PointAt(array4[n]), val9.TangentAt(array4[n]), cSGemData2.Up)) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(-1.0, 1.0, 0.0), cSGemData.Up, val9.PointAt(array4[n]), val9.TangentAt(array4[n]), cSGemData2.Up));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, val10 * val3);
				cSDataArray2.Append(data);
			}
			Interval domain2 = val5.Domain;
			for (int num7 = 0; num7 < num3; num7++)
			{
				Transform val11 = default(Transform);
				val11 = ((array6[num7] != ((Interval)(ref domain2)).Max) ? ((array6[num7] != ((Interval)(ref domain2)).Min) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, val5.PointAt(array6[num7]), val5.TangentAt(array6[num7]), cSGemData2.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, -1.0, 0.0), cSGemData.Up, val5.PointAt(array6[num7]), val5.TangentAt(array6[num7]), cSGemData2.Up)) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(-1.0, 1.0, 0.0), cSGemData.Up, val5.PointAt(array6[num7]), val5.TangentAt(array6[num7]), cSGemData2.Up));
				CSData data2 = cSGemData.Duplicate();
				CSDataHelper.Transform(data2, val11 * val3);
				cSDataArray2.Append(data2);
			}
			Interval domain3 = val6.Domain;
			for (int num8 = 0; num8 < num4; num8++)
			{
				Transform val12 = default(Transform);
				val12 = ((array8[num8] != ((Interval)(ref domain3)).Max) ? ((array8[num8] != ((Interval)(ref domain3)).Min) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, val6.PointAt(array8[num8]), val6.TangentAt(array8[num8]), cSGemData2.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, -1.0, 0.0), cSGemData.Up, val6.PointAt(array8[num8]), val6.TangentAt(array8[num8]), cSGemData2.Up)) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(-1.0, 1.0, 0.0), cSGemData.Up, val6.PointAt(array8[num8]), val6.TangentAt(array8[num8]), cSGemData2.Up));
				CSData data3 = cSGemData.Duplicate();
				CSDataHelper.Transform(data3, val12 * val3);
				cSDataArray2.Append(data3);
			}
			Interval domain4 = val7.Domain;
			for (int num9 = 0; num9 < num5; num9++)
			{
				Transform val13 = default(Transform);
				val13 = ((array10[num9] != ((Interval)(ref domain4)).Max) ? ((array10[num9] != ((Interval)(ref domain4)).Min) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, val7.PointAt(array10[num9]), val7.TangentAt(array10[num9]), cSGemData2.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, -1.0, 0.0), cSGemData.Up, val7.PointAt(array10[num9]), val7.TangentAt(array10[num9]), cSGemData2.Up)) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(-1.0, 1.0, 0.0), cSGemData.Up, val7.PointAt(array10[num9]), val7.TangentAt(array10[num9]), cSGemData2.Up));
				CSData data4 = cSGemData.Duplicate();
				CSDataHelper.Transform(data4, val13 * val3);
				cSDataArray2.Append(data4);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		return base.Update();
	}

	private static double GemPosition(int gem, int totalGems, double style)
	{
		if (style == 0.0)
		{
			return (double)gem / (double)totalGems;
		}
		double num = 1.0 / (2.0 * Math.Sqrt(2.0) + (double)(2 * (totalGems - 1)));
		return num * Math.Sqrt(2.0) + num * 2.0 * (double)gem;
	}
}
