using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("UnderBezelRailTopOuter_V1", "{DD1E8B0B-C53F-42c5-ADBC-D53C516B62AF}")]
public sealed class CSUnderBezelRailTopOuter1 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSOutputChannel oCurve { get; }

	public CSUnderBezelRailTopOuter1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		CSDataArray flattenArray = iGem.GetFlattenArray();
		double value = 0.0;
		iGirdleSpacing.GetDouble(ref value);
		double value2 = 0.0;
		iPlacement.GetDouble(ref value2);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				continue;
			}
			CSDataHelper.SetSettingType(cSGemData, GemSettingType.BZ);
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData.Xform.Convert(), out var xUp, out var xDown))
			{
				xUp.Identity();
				xDown = xUp.Inverse();
			}
			OnNurbsCurve val = cSGemData.GetNurbsCurve().Convert();
			if (val != null)
			{
				OnNurbsCurve obj = val.Duplicate();
				obj.Transform((IOnXform)(object)xDown);
				OnXform val2 = new OnXform();
				val2.Translation(0.0, 0.0, value2);
				OnCurve val3 = CSUtilGeometry.OriginOffsetXY((IOnCurve)(object)obj, value, closed: true);
				if (val3 == null)
				{
					CS.CSUtil.Print("Under Bezel Top Outer Offset Curve Unable to Create Output\n");
					continue;
				}
				((OnGeometry)val3).Transform((IOnXform)(object)val2);
				cSDataArray.Append(new CSOnGeometryData(val3.Convert(), xUp.Convert()));
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oCurve.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oCurve.SetData(cSDataArray);
		}
		return base.Update();
	}
}
