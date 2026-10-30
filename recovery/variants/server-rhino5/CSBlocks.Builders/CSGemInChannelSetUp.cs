using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Builders;

[CSBlock("GemInChannelSetUp", "{9B4BCEAD-848D-41BE-A7EF-875EE23ECFDD}")]
public sealed class CSGemInChannelSetUp : CSBlock
{
	public CSInputChannel iChannelMode { get; }

	public CSInputChannel iGemSpacingProng { get; }

	public CSInputChannel iWallDropProng { get; }

	public CSInputChannel iChannelWallProng { get; }

	public CSInputChannel iChannelSpacingProng { get; }

	public CSInputChannel iChannelEndSpacingProng { get; }

	public CSInputChannel iGemSpacingChannel { get; }

	public CSInputChannel iWallDropChannel { get; }

	public CSInputChannel iChannelWallChannel { get; }

	public CSInputChannel iChannelSpacingChannel { get; }

	public CSInputChannel iChannelEndSpacingChannel { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iProfileWidth { get; }

	public CSInputChannel iGemSize { get; }

	public CSInputChannel iExtraWidth { get; }

	public CSInputChannel iGemRotation { get; }

	public CSOutputChannel oBaseGem { get; }

	public CSOutputChannel oChannelHeight { get; }

	public CSOutputChannel oProfileParamLeft { get; }

	public CSOutputChannel oProfileParamRight { get; }

	public CSOutputChannel oProfile { get; }

	public CSOutputChannel oGemSpacing { get; }

	public CSOutputChannel oWallDrop { get; }

	public CSOutputChannel oChannelSpacing { get; }

	public CSOutputChannel oChannelEndSpacing { get; }

	public CSOutputChannel oGemSizeLoopBack { get; }

	public CSOutputChannel oProfileWidthLoopBack { get; }

	public CSGemInChannelSetUp(string identifier)
		: base(identifier)
	{
		iChannelMode = new CSInputChannel(GetPropertyName(() => iChannelMode), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iGemSpacingProng = new CSInputChannel(GetPropertyName(() => iGemSpacingProng), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWallDropProng = new CSInputChannel(GetPropertyName(() => iWallDropProng), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallProng = new CSInputChannel(GetPropertyName(() => iChannelWallProng), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacingProng = new CSInputChannel(GetPropertyName(() => iChannelSpacingProng), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelEndSpacingProng = new CSInputChannel(GetPropertyName(() => iChannelEndSpacingProng), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSpacingChannel = new CSInputChannel(GetPropertyName(() => iGemSpacingChannel), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWallDropChannel = new CSInputChannel(GetPropertyName(() => iWallDropChannel), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallChannel = new CSInputChannel(GetPropertyName(() => iChannelWallChannel), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacingChannel = new CSInputChannel(GetPropertyName(() => iChannelSpacingChannel), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelEndSpacingChannel = new CSInputChannel(GetPropertyName(() => iChannelEndSpacingChannel), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iProfileWidth = new CSInputChannel(GetPropertyName(() => iProfileWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSize = new CSInputChannel(GetPropertyName(() => iGemSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iExtraWidth = new CSInputChannel(GetPropertyName(() => iExtraWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemRotation = new CSInputChannel(GetPropertyName(() => iGemRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBaseGem = new CSOutputChannel(GetPropertyName(() => oBaseGem), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		oChannelHeight = new CSOutputChannel(GetPropertyName(() => oChannelHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfileParamLeft = new CSOutputChannel(GetPropertyName(() => oProfileParamLeft), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfileParamRight = new CSOutputChannel(GetPropertyName(() => oProfileParamRight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(GetPropertyName(() => oProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oGemSpacing = new CSOutputChannel(GetPropertyName(() => oGemSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oWallDrop = new CSOutputChannel(GetPropertyName(() => oWallDrop), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oChannelSpacing = new CSOutputChannel(GetPropertyName(() => oChannelSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oChannelEndSpacing = new CSOutputChannel(GetPropertyName(() => oChannelEndSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGemSizeLoopBack = new CSOutputChannel(GetPropertyName(() => oGemSizeLoopBack), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfileWidthLoopBack = new CSOutputChannel(GetPropertyName(() => oProfileWidthLoopBack), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Expected O, but got Unknown
		//IL_05c6: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Expected O, but got Unknown
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Expected O, but got Unknown
		//IL_0695: Expected O, but got Unknown
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Expected O, but got Unknown
		bool value = false;
		iChannelMode.GetBoolean(ref value);
		if (value)
		{
			oGemSpacing.SetData(iGemSpacingChannel.Data);
			oWallDrop.SetData(iWallDropChannel.Data);
			oChannelSpacing.SetData(iChannelSpacingChannel.Data);
			oChannelEndSpacing.SetData(iChannelEndSpacingChannel.Data);
		}
		else
		{
			oGemSpacing.SetData(iGemSpacingProng.Data);
			oWallDrop.SetData(iWallDropProng.Data);
			oChannelSpacing.SetData(iChannelSpacingProng.Data);
			oChannelEndSpacing.SetData(iChannelEndSpacingProng.Data);
		}
		CSData data = iProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Curve");
		}
		OnCurve val = (OnCurve)(object)data.GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Profile Missing");
		}
		if (!(iGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem");
		}
		BeenUpdatedAfterLastUpdate(iGemSize.OutputChannelBlock);
		double value2 = 0.0;
		iProfileWidth.GetDouble(ref value2);
		double value3 = 0.0;
		double value4 = 0.0;
		bool num = BeenUpdatedAfterLastUpdate(iProfileWidth.OutputChannelBlock);
		bool flag;
		bool flag2;
		if (value)
		{
			flag = BeenUpdatedAfterLastUpdate(iChannelWallChannel.OutputChannelBlock);
			flag2 = BeenUpdatedAfterLastUpdate(iChannelSpacingChannel.OutputChannelBlock);
			iChannelWallChannel.GetDouble(ref value3);
			iChannelSpacingChannel.GetDouble(ref value4);
		}
		else
		{
			flag = BeenUpdatedAfterLastUpdate(iChannelWallProng.OutputChannelBlock);
			flag2 = BeenUpdatedAfterLastUpdate(iChannelSpacingProng.OutputChannelBlock);
			iChannelWallProng.GetDouble(ref value3);
			iChannelSpacingProng.GetDouble(ref value4);
		}
		double value5 = 0.0;
		double requestedSizeY = 0.0;
		if (iGemSize.Data is CSGemData cSGemData2)
		{
			value5 = cSGemData2.GemXSize;
			requestedSizeY = cSGemData2.GemYSize;
		}
		else
		{
			iGemSize.GetDouble(ref value5);
		}
		double value6 = 0.0;
		if (iGemRotation != null)
		{
			iGemRotation.GetDouble(ref value6);
		}
		OnXform xBestScale = new OnXform();
		OnXform val2 = new OnXform();
		val2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value6), OnUtil.On_zaxis, OnUtil.On_origin);
		if (num || flag || flag2)
		{
			double num2 = value2 - (value3 + value4) * 2.0;
			CSUtilGem.GetGemSizeFromTable(cSGemData, num2, requestedSizeY, useLargestPossible: false, useIterationOnly: false, out var bestSize);
			CSUtilGem.GetGemScaleFromTable(cSGemData, bestSize.X, bestSize.Y, false, false, out OnXform xBestScale2);
			CSData cSData = cSGemData.Duplicate();
			CSDataHelper.Transform(cSData, val2.Convert() * xBestScale2.Convert());
			BoundingBox boundingBox = BoundingBox.Unset;
			cSData.GetTightBoundingBox(ref boundingBox);
			double num3 = boundingBox.Max.X - boundingBox.Min.X;
			value5 = bestSize.X / (num3 / num2);
			requestedSizeY = bestSize.Y / (num3 / num2);
			CSUtilGem.GetGemSizeFromTable(cSGemData, value5, requestedSizeY, useLargestPossible: false, useIterationOnly: false, out bestSize);
			value5 = bestSize.X;
			requestedSizeY = bestSize.Y;
		}
		else
		{
			CSUtilGem.GetGemScaleFromTable(cSGemData, value5, requestedSizeY, false, false, out xBestScale);
			CSData cSData2 = cSGemData.Duplicate();
			if (xBestScale != null)
			{
				CSDataHelper.Transform(cSData2, val2.Convert() * xBestScale.Convert());
			}
			else
			{
				CSDataHelper.Transform(cSData2, val2.Convert());
			}
			BoundingBox boundingBox2 = BoundingBox.Unset;
			cSData2.GetTightBoundingBox(ref boundingBox2);
			value2 = boundingBox2.Max.X - boundingBox2.Min.X + (value3 + value4) * 2.0;
		}
		oGemSizeLoopBack.SetData(new CSNumberData(value5));
		oProfileWidthLoopBack.SetData(new CSNumberData(value2));
		double value7 = 0.0;
		iExtraWidth.GetDouble(ref value7);
		value2 += value7;
		value3 += value7 / 2.0;
		if (!CSUtilGem.GetGemScaleFromTable(cSGemData, value5, requestedSizeY, false, false, out xBestScale))
		{
			return UpdateError("GetGemSizeFromTable failed");
		}
		CSData data2 = cSGemData.Duplicate();
		CSDataHelper.Transform(data2, val2.Convert() * xBestScale.Convert());
		OnBoundingBox val3 = new OnBoundingBox();
		((OnGeometry)val).GetTightBoundingBox(ref val3);
		OnXform val4 = new OnXform();
		val4.Translation((0.0 - (val3.Max().x + val3.Min().x)) / 2.0, 0.0 - val3.Min().y, 0.0);
		OnXform val5 = new OnXform();
		val5.Scale(1.0 / (val3.Max().x - val3.Min().x), 1.0 / (val3.Max().y - val3.Min().y), 0.0);
		OnXform val6 = new OnXform();
		val6.Scale(value2, 1.0, 1.0);
		OnCurve val7 = val.Duplicate();
		((OnGeometry)val7).Transform((IOnXform)(object)(val6 * (IOnXform)(object)val5 * (IOnXform)(object)val4));
		double num4 = value2 / 2.0 - value3;
		OnLineCurve val8 = new OnLineCurve((IOn3dPoint)new On3dPoint(num4, -1.0, 0.0), (IOn3dPoint)new On3dPoint(num4, 2.0, 0.0));
		ArrayOnX_EVENT val9 = new ArrayOnX_EVENT();
		val7.IntersectCurve((IOnCurve)(object)val8, ref val9, CSApplicationHelper.Application.Tolerance);
		double num5 = 0.0;
		double number = 0.0;
		foreach (OnX_EVENT item in val9)
		{
			OnX_EVENT val10 = item;
			On3dPoint val11 = val10[0];
			if (val11.y > num5)
			{
				num5 = val11.y;
				number = val10[0];
			}
		}
		OnLineCurve val12 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0 - num4, -1.0, 0.0), (IOn3dPoint)new On3dPoint(0.0 - num4, 2.0, 0.0));
		val9 = new ArrayOnX_EVENT();
		val7.IntersectCurve((IOnCurve)(object)val12, ref val9, CSApplicationHelper.Application.Tolerance);
		double num6 = 0.0;
		double number2 = 0.0;
		foreach (OnX_EVENT item2 in val9)
		{
			OnX_EVENT val13 = item2;
			On3dPoint val14 = val13[0];
			if (val14.y > num6)
			{
				num6 = val14.y;
				number2 = val13[0];
			}
		}
		double number3 = (num5 + num6) / 2.0;
		oBaseGem.SetData(data2);
		oChannelHeight.SetData(new CSNumberData(number3));
		oProfileParamLeft.SetData(new CSNumberData(number2));
		oProfileParamRight.SetData(new CSNumberData(number));
		oProfile.SetData(new CSOnGeometryData(val7.Convert()));
		return base.Update();
	}
}
