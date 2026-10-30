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
[CSBlock("ClusterRound_V3", "{5ECE7FBA-3FA9-4fad-9DC3-34F340F993AF}")]
public sealed class CSClusterRound3 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iSpacing { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iRotation { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iFixPosition { get; }

	public CSInputChannel iEven { get; }

	public CSOutputChannel oGem { get; }

	public CSOutputChannel oShape { get; }

	public CSClusterRound3(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iSpacing = new CSInputChannel(GetPropertyName(() => iSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(GetPropertyName(() => iRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFixPosition = new CSInputChannel(GetPropertyName(() => iFixPosition), this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		double value4 = 0.5;
		iGirdleSpacing.GetDouble(ref value4);
		double value5 = 0.0;
		iPlacement.GetDouble(ref value5);
		double value6 = 0.0;
		iFixPosition.GetDouble(ref value6);
		bool value7 = false;
		iEven.GetBoolean(ref value7);
		OnXform onXform = new OnXform();
		onXform.Rotation(value3, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(value2, OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform onXform3 = new OnXform();
		onXform3.Translation(0.0, 0.0, value5);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, onXform.Convert());
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Tight Bounding Box");
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				continue;
			}
			if (!GemOffsetCurveV1.GemOffset("Round", cSGemData2, value4 + Math.Abs(boundingBox.Min.Y), out OnCurve curve, true, out string error))
			{
				return UpdateError(error);
			}
			double length = 0.0;
			if (!curve.GetLength(ref length))
			{
				return UpdateError("Curve Length");
			}
			int num2 = (int)(length / (num + value));
			if (value7 && num2 % 2 != 0)
			{
				num2--;
			}
			double[] array = new double[num2];
			double[] t = new double[num2];
			double num3 = 1.0 / (double)num2;
			double num4 = value6 % num3;
			for (int j = 0; j < num2; j++)
			{
				array[j] = num4;
				num4 += num3;
				if (num4 > 1.0)
				{
					num4 -= 1.0;
				}
			}
			Array.Sort(array);
			curve.GetNormalizedArcLengthPoints(array, ref t);
			CSDataArray cSDataArray2 = new CSDataArray();
			for (int k = 0; k < num2; k++)
			{
				OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), curve.PointAt(t[k]), curve.TangentAt(t[k]), cSGemData2.Up.Convert());
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, xform.Convert() * onXform3.Convert() * onXform2.Convert() * onXform.Convert());
				cSDataArray2.Append(data);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		oShape.SetData(new CSStringData("Round"));
		return base.Update();
	}
}
