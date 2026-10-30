using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlocks.Shank.Helpers;
using CSDatas;

namespace CSBlocks.Shank.ShankV1;

[CSBlock("ShankMath", "{EE07E4BF-3CB6-4288-A3A4-F5BE7AA4F4A7}")]
public sealed class CSShankMath : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGems;

	public readonly CSOutputChannel OutGemCount;

	public readonly CSOutputChannel OutGem0SizeX;

	public readonly CSOutputChannel OutGem0SizeY;

	public readonly CSOutputChannel OutGem1SizeY;

	public readonly CSOutputChannel OutGem0Position;

	public readonly CSOutputChannel OutGem1Position;

	public readonly CSOutputChannel OutGemLast0SizeX;

	public readonly CSOutputChannel OutGemLast0SizeY;

	public readonly CSOutputChannel OutGemLast1SizeY;

	public readonly CSOutputChannel OutGemLast0Position;

	public readonly CSOutputChannel OutGemLast1Position;

	public readonly CSOutputChannel OutCurveLength;

	public CSShankMath(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutGemCount = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutGem0SizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem0SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem1SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem0Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem1Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast0SizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast0SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast1SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast0Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast1Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutCurveLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!(InGems.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Ultimate GemData Array");
		}
		List<UltimateGemData> list = new List<UltimateGemData>();
		for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
		{
			if (!(cSDataArray.GetArrayItem(i) is CSDataArray gemInfo))
			{
				return UpdateMissing("Ultimate GemData GemArray");
			}
			UltimateGemData item = UltimateGemData.ReadFromArray(gemInfo);
			list.Add(item);
		}
		if (!CSDataHelper.GetCurve(InCurve, out var value))
		{
			return UpdateMissing("Curve");
		}
		double length = value.GetLength();
		int arrayCount = cSDataArray.GetArrayCount();
		double number = 0.0;
		double number2 = 0.0;
		double number3 = 0.0;
		double number4 = 0.0;
		double number5 = 0.0;
		double number6 = 0.0;
		double number7 = 0.0;
		double number8 = 0.0;
		double number9 = 0.0;
		double number10 = 0.0;
		if (arrayCount > 0)
		{
			UltimateGemData ultimateGemData = list[0];
			number = ultimateGemData.SizeX;
			number2 = ultimateGemData.SizeY;
			number4 = ultimateGemData.NormParam;
			UltimateGemData ultimateGemData2 = list[arrayCount - 1];
			number6 = ultimateGemData2.SizeX;
			number7 = ultimateGemData2.SizeY;
			number9 = ultimateGemData2.NormParam;
		}
		if (arrayCount > 1)
		{
			UltimateGemData ultimateGemData3 = list[1];
			number3 = ultimateGemData3.SizeY;
			number5 = ultimateGemData3.NormParam;
			UltimateGemData ultimateGemData4 = list[arrayCount - 2];
			number8 = ultimateGemData4.SizeY;
			number10 = ultimateGemData4.NormParam;
		}
		OutGemCount.SetData(new CSNumberData(arrayCount));
		OutGem0SizeX.SetData(new CSNumberData(number));
		OutGem0SizeY.SetData(new CSNumberData(number2));
		OutGem1SizeY.SetData(new CSNumberData(number3));
		OutGem0Position.SetData(new CSNumberData(number4));
		OutGem1Position.SetData(new CSNumberData(number5));
		OutGemLast0SizeX.SetData(new CSNumberData(number6));
		OutGemLast0SizeY.SetData(new CSNumberData(number7));
		OutGemLast1SizeY.SetData(new CSNumberData(number8));
		OutGemLast0Position.SetData(new CSNumberData(number9));
		OutGemLast1Position.SetData(new CSNumberData(number10));
		OutCurveLength.SetData(new CSNumberData(length));
		return base.Update();
	}
}
