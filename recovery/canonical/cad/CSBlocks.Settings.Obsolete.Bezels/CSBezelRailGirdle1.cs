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
[CSBlock("BezelRailGirdle_V1", "{061B2104-7445-4750-BAB6-DEFDF0C3344E}")]
public sealed class CSBezelRailGirdle1 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSOutputChannel oCurve { get; }

	public CSBezelRailGirdle1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
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
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform.Convert(), out var xUp, out var xDown);
			OnNurbsCurve onNurbsCurve = cSGemData.GetNurbsCurve().Convert();
			if (onNurbsCurve != null)
			{
				OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
				onNurbsCurve2.Transform(xDown);
				OnXform onXform = new OnXform();
				onXform.Translation(0.0, 0.0, value2);
				OnCurve onCurve = CSUtilGeometry.OriginOffsetXY(onNurbsCurve2, value, closed: true);
				if (onCurve == null)
				{
					CS.CSUtil.Print("Bezel Rail Girdle Offset Curve Unable to Create Output\n");
					continue;
				}
				onCurve.Transform(onXform);
				cSDataArray.Append(new CSOnGeometryData(onCurve.Convert(), xUp.Convert()));
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
