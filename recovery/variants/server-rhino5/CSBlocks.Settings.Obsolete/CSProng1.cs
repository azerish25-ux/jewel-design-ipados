using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("Prong_V1", "{0A9A8005-5C55-4820-803C-22DDA04C72BE}")]
public sealed class CSProng1 : CSBlock
{
	public CSInputChannel iDiameter { get; }

	public CSInputChannel iHeight { get; }

	public CSInputChannel iFillet { get; }

	public CSInputChannel iTaper { get; }

	public CSOutputChannel oSurface { get; }

	public CSProng1(string identifier)
		: base(identifier)
	{
		iDiameter = new CSInputChannel(GetPropertyName(() => iDiameter), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeight = new CSInputChannel(GetPropertyName(() => iHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFillet = new CSInputChannel(GetPropertyName(() => iFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTaper = new CSInputChannel(GetPropertyName(() => iTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		double value = 0.0;
		iDiameter.GetDouble(ref value);
		double value2 = 0.0;
		iHeight.GetDouble(ref value2);
		if (CSApplicationHelper.Application.IsCamMode)
		{
			value2 += CSApplicationHelper.Application.CamProngExtensionHeight;
		}
		double value3 = 0.0;
		iFillet.GetDouble(ref value3);
		value3 /= 100.0;
		double value4 = 0.0;
		iTaper.GetDouble(ref value4);
		value4 /= 100.0;
		OnNurbsSurface nurbsSurface = new OnNurbsSurface();
		if (!CSUtilGeometry.CreateProng_Old_V1(value, value2, value3, value4, ref nurbsSurface))
		{
			return UpdateError("CreateProng failed");
		}
		oSurface.SetData(new CSOnGeometryData(nurbsSurface.Convert()));
		return base.Update();
	}
}
