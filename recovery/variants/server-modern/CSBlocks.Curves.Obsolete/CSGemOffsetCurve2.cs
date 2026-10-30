using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves.Obsolete;

[Obsolete]
[CSBlock("GemOffsetCurve_V2", "{20F3BB2A-C7EA-4BC9-8ED7-0E1FDF885E18}")]
public sealed class CSGemOffsetCurve2 : CSBlock
{
	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iGirdleSpacing;

	public readonly CSInputChannel iZOffset;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSOutputChannel oCurve;

	public CSGemOffsetCurve2(string identifier)
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
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		double value = 0.0;
		iGirdleSpacing.GetDouble(ref value);
		double value2 = 0.0;
		iZOffset.GetDouble(ref value2);
		CSGemData cSGemData = iGemDefinition.Data as CSGemData;
		CSDataArray cSDataArray = new CSDataArray();
		Vector3d val = default(Vector3d);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				continue;
			}
			Transform xUp = Transform.Unset;
			CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData2.Xform, out xUp);
			((Vector3d)(ref val))._002Ector(Vector3d.ZAxis);
			((Vector3d)(ref val)).Transform(xUp);
			Transform val2 = Transform.Translation(val * value2);
			Transform val3 = Transform.Identity;
			Curve curve = null;
			string error2;
			if (cSGemData != null)
			{
				if (!GemOffsetCurveV2.GemOffset(cSGemData, value, out curve, false, out string error, 0.0))
				{
					return UpdateError(error);
				}
				val3 = cSGemData2.Xform;
			}
			else if (!GemOffsetCurveV2.GemOffset(cSGemData2, value, out curve, true, out error2, 0.0))
			{
				return UpdateError(error2);
			}
			((GeometryBase)curve).Transform(val3 * val2);
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)curve, val3 * val2, applyTransform: false));
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
