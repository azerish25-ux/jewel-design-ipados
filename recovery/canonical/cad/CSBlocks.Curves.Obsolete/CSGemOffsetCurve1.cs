using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Curves.Obsolete;

[Obsolete]
[CSBlock("GemOffsetCurve_V1", "{06FF406A-F238-46f1-B27E-C0E9BA300CE7}")]
public sealed class CSGemOffsetCurve1 : CSBlock
{
	public CSInputChannel iGem;

	public CSInputChannel iGirdleSpacing;

	public CSInputChannel iZOffset;

	public CSInputChannel iGemDefinition;

	public CSOutputChannel oCurve;

	public CSGemOffsetCurve1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGem.GetFlattenArray();
		double value = 0.0;
		iGirdleSpacing.GetDouble(ref value);
		double value2 = 0.0;
		iZOffset.GetDouble(ref value2);
		CSGemData cSGemData = iGemDefinition.Data as CSGemData;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				continue;
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData2.Xform.Convert(), out var xUp);
			On3dVector on3dVector = new On3dVector(OnUtil.On_zaxis);
			on3dVector.Transform(xUp);
			OnXform onXform = new OnXform();
			onXform.Translation(on3dVector * value2);
			OnXform onXform2 = new OnXform();
			onXform2.Identity();
			OnCurve curve;
			string error2;
			if (cSGemData != null)
			{
				if (!GemOffsetCurveV1.GemOffset(cSGemData, value, out curve, false, out string error))
				{
					return UpdateError(error);
				}
				onXform2 = new OnXform(cSGemData2.Xform.Convert());
			}
			else if (!GemOffsetCurveV1.GemOffset(cSGemData2, value, out curve, true, out error2))
			{
				return UpdateError(error2);
			}
			if (curve == null)
			{
				curve = new OnNurbsCurve();
			}
			curve.Transform(onXform2 * onXform);
			cSDataArray.Append(new CSOnGeometryData(curve.Convert(), onXform2.Convert() * onXform.Convert(), applyTransform: false));
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
