using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("BooleanOr", "{F8C5A94A-5CC9-4DDE-B2AC-FD7208C2CC5E}")]
public sealed class CSBooleanOr : CSBlock
{
	public readonly CSInputChannel iValue01;

	public readonly CSInputChannel iValue02;

	public readonly CSInputChannel iValue03;

	public readonly CSInputChannel iValue04;

	public readonly CSInputChannel iValue05;

	public readonly CSInputChannel iValue06;

	public readonly CSInputChannel iValue07;

	public readonly CSInputChannel iValue08;

	public readonly CSInputChannel iValue09;

	public readonly CSInputChannel iValue10;

	public readonly CSInputChannel iValue11;

	public readonly CSInputChannel iValue12;

	public readonly CSInputChannel iValue13;

	public readonly CSInputChannel iValue14;

	public readonly CSInputChannel iValue15;

	public readonly CSInputChannel iValue16;

	public readonly CSInputChannel iValue17;

	public readonly CSInputChannel iValue18;

	public readonly CSInputChannel iValue19;

	public readonly CSInputChannel iValue20;

	public readonly CSInputChannel iOutputIfFalse;

	public readonly CSInputChannel iOutputIfTrue;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oOutput;

	public CSBooleanOr(string identifier)
		: base(identifier)
	{
		iValue01 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue02 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue03 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue04 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue05 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue06 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue07 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue08 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue09 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue10 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue11 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue12 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue13 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue14 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue15 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue16 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue17 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue18 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue19 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iValue20 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iOutputIfFalse = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iOutputIfTrue = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		oValue = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oOutput = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		List<bool> list = new List<bool>();
		for (int i = 0; i < 20; i++)
		{
			bool value = false;
			if (base.InputChannels[i].GetBoolean(ref value))
			{
				list.Add(value);
			}
		}
		bool flag = false;
		for (int j = 0; j < list.Count; j++)
		{
			flag = ((j != 0) ? (flag | list[j]) : list[j]);
		}
		oValue.SetData(new CSNumberData(flag ? 1.0 : 0.0));
		oOutput.SetData(flag ? iOutputIfTrue.Data : iOutputIfFalse.Data);
		return base.Update();
	}
}
