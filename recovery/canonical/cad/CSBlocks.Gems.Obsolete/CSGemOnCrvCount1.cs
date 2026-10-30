using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnCrvCount_V1", "{863B4664-ACD4-471d-8E8E-2C504BC3D27F}")]
public sealed class CSGemOnCrvCount1 : CSBlock
{
	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InSurface;

	public readonly CSInputChannel InDiameterEnd;

	public readonly CSInputChannel InParamStart;

	public readonly CSInputChannel InParamEnd;

	public readonly CSInputChannel InZOffsetStart;

	public readonly CSInputChannel InZOffsetEnd;

	public readonly CSInputChannel InRoll;

	public readonly CSInputChannel InTilt;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSInputChannel InFlip;

	public readonly CSInputChannel InMirror;

	public readonly CSInputChannel InTaperDiameter;

	public readonly CSInputChannel InTaperZOffset;

	public readonly CSInputChannel InCount;

	public readonly CSOutputChannel OutArray;

	public CSGemOnCrvCount1(string identifier)
		: base(identifier)
	{
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InParamEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffsetStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffsetEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRoll = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaperDiameter = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaperZOffset = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 1);
		OutArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSGemData gem = InGemDefinition.Gem;
		OnCurve onCurve = InCurve.NurbsCurve.Convert();
		if (onCurve == null)
		{
			return UpdateMissing("Curve");
		}
		OnBrep onBrep = InSurface.Brep.Convert();
		if (onBrep == null)
		{
			return UpdateMissing("Surface");
		}
		int integer = InCount.Integer;
		int integer2 = InPlacementMethod.Integer;
		double gemXSize = gem.GemXSize;
		double num = InDiameterEnd.Double;
		double num2 = InParamStart.Double;
		double num3 = InParamEnd.Double;
		double num4 = InZOffsetStart.Double;
		double num5 = InZOffsetEnd.Double;
		double angleDegrees = InRoll.Double;
		double angleDegrees2 = InTilt.Double;
		bool boolean = InFlip.Boolean;
		_ = InMirror.Boolean;
		bool boolean2 = InTaperDiameter.Boolean;
		bool boolean3 = InTaperZOffset.Boolean;
		if (!boolean2)
		{
			num = gemXSize;
		}
		if (!boolean3)
		{
			num5 = num4;
		}
		CSDataArray cSDataArray = new CSDataArray();
		double num6;
		double num7;
		if (integer == 1)
		{
			num6 = 0.0;
			num7 = (gemXSize + num) / 2.0;
		}
		else
		{
			num6 = (num - gemXSize) / (double)(integer - 1);
			num7 = gemXSize;
		}
		double[] array = new double[integer];
		for (int i = 0; i < integer; i++)
		{
			array[i] = num7 + (double)i * num6;
		}
		double num8;
		double num9;
		if (integer == 1)
		{
			num8 = 0.0;
			num9 = (num4 + num5) / 2.0;
		}
		else
		{
			num8 = (num5 - num4) / (double)(integer - 1);
			num9 = num4;
		}
		double[] array2 = new double[integer];
		for (int j = 0; j < integer; j++)
		{
			array2[j] = num9 + (double)j * num8;
		}
		double num10;
		double num11;
		if (integer == 1)
		{
			num10 = 0.0;
			num11 = num2 + (num3 - num2) / 2.0;
		}
		else
		{
			num10 = (num3 - num2) / (double)(integer - 1);
			num11 = num2;
		}
		double[] array3 = new double[integer];
		for (int k = 0; k < integer; k++)
		{
			if (boolean2)
			{
				array3[k] = num11 + (double)k * num10;
			}
			else
			{
				array3[k] = num11 + (double)k * num10;
			}
		}
		Array.Sort(array3);
		double[] t = new double[integer];
		onCurve.GetNormalizedArcLengthPoints(array3, ref t);
		for (int l = 0; l < integer; l++)
		{
			double num12 = array[l] / 2.0 / gem.GemXMax;
			OnXform onXform = new OnXform();
			onXform.Scale(num12, num12, num12);
			OnXform onXform2 = new OnXform();
			onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), OnUtil.On_zaxis, OnUtil.On_origin);
			OnXform onXform3 = new OnXform();
			onXform3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2), OnUtil.On_xaxis, OnUtil.On_origin);
			CSGemData cSGemData = new CSGemData(gem);
			cSGemData.ApplyTransform(onXform.Convert());
			double zOffset = 0.0;
			CSUtilGem.GetGemZOffset(cSGemData, (CSUtilGem.EGemPlacementMethod)integer2, ref zOffset);
			OnXform onXform4 = new OnXform();
			onXform4.Translation(0.0, 0.0, zOffset + array2[l]);
			On3dPoint on3dPoint = onCurve.PointAt(t[l]);
			On3dVector brepNormal = new On3dVector(OnUtil.On_zaxis);
			CSUtilGeometry.GetClosestNormalToBrep(onBrep, on3dPoint, ref brepNormal);
			On3dVector destination = onCurve.TangentAt(t[l]);
			OnXform xform = (boolean ? CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(OnUtil.On_origin), -new On3dVector(OnUtil.On_zaxis), -new On3dVector(OnUtil.On_xaxis), on3dPoint, brepNormal, destination) : CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(OnUtil.On_origin), new On3dVector(OnUtil.On_zaxis), new On3dVector(OnUtil.On_xaxis), on3dPoint, brepNormal, destination));
			CSData data = gem.Duplicate();
			CSDataHelper.Transform(data, xform.Convert() * onXform4.Convert() * onXform2.Convert() * onXform3.Convert() * onXform.Convert());
			cSDataArray.Append(data);
		}
		OutArray.SetData(cSDataArray);
		return base.Update();
	}
}
