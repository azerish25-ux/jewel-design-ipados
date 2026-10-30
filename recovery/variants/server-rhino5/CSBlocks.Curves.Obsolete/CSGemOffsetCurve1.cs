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
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
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
			CSBlockUtil.CSUtilMath.GetUnscaledXform((IOnXform)(object)cSGemData2.Xform.Convert(), out var xUp);
			On3dVector val = new On3dVector(OnUtil.On_zaxis);
			val.Transform((IOnXform)(object)xUp);
			OnXform val2 = new OnXform();
			val2.Translation((IOn3dVector)(object)(val * value2));
			OnXform val3 = new OnXform();
			val3.Identity();
			OnCurve curve;
			string error2;
			if (cSGemData != null)
			{
				if (!GemOffsetCurveV1.GemOffset(cSGemData, value, out curve, false, out string error))
				{
					return UpdateError(error);
				}
				val3 = new OnXform((IOnXform)(object)cSGemData2.Xform.Convert());
			}
			else if (!GemOffsetCurveV1.GemOffset(cSGemData2, value, out curve, true, out error2))
			{
				return UpdateError(error2);
			}
			if (curve == null)
			{
				curve = (OnCurve)new OnNurbsCurve();
			}
			((OnGeometry)curve).Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
			cSDataArray.Append(new CSOnGeometryData(curve.Convert(), val3.Convert() * val2.Convert(), applyTransform: false));
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
