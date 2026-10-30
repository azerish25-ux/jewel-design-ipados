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
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				continue;
			}
			Transform xUp = Transform.Unset;
			CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData2.Xform, out xUp);
			Vector3d vector3d = new Vector3d(Vector3d.ZAxis);
			vector3d.Transform(xUp);
			Transform transform = Transform.Translation(vector3d * value2);
			Transform transform2 = Transform.Identity;
			Curve curve = null;
			string error2;
			if (cSGemData != null)
			{
				if (!GemOffsetCurveV2.GemOffset(cSGemData, value, out curve, false, out string error, 0.0))
				{
					return UpdateError(error);
				}
				transform2 = cSGemData2.Xform;
			}
			else if (!GemOffsetCurveV2.GemOffset(cSGemData2, value, out curve, true, out error2, 0.0))
			{
				return UpdateError(error2);
			}
			curve.Transform(transform2 * transform);
			cSDataArray.Append(new CSOnGeometryData(curve, transform2 * transform, applyTransform: false));
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
