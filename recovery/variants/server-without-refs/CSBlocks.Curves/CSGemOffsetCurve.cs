using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("GemOffsetCurve", "{A00F9888-46AF-40DE-8220-10683ADF9FBC}")]
public sealed class CSGemOffsetCurve : CSBlock
{
	public CSInputChannel InGem { get; }

	public CSInputChannel InGirdleSpacing { get; }

	public CSInputChannel InOffsetZ { get; }

	public CSInputChannel InGemDefinition { get; }

	public CSInputChannel InCornerBlend { get; }

	public CSOutputChannel OutCurve { get; }

	public CSGemOffsetCurve(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		InGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		double distance = InGirdleSpacing.Double;
		double num = InOffsetZ.Double;
		double cornerBlend = InCornerBlend.Double;
		CSGemData cSGemData = InGemDefinition.Data as CSGemData;
		CSDataArray cSDataArray = new CSDataArray();
		Vector3d val = default(Vector3d);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				continue;
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData2.Xform, out var xUp);
			((Vector3d)(ref val))._002Ector(Vector3d.ZAxis);
			((Vector3d)(ref val)).Transform(xUp);
			Transform val2 = Transform.Translation(val * num);
			Transform val3 = Transform.Identity;
			Curve curve;
			string error2;
			if (cSGemData != null)
			{
				if (!GemOffsetCurve.GemOffset(cSGemData, distance, out curve, doTransformUp: false, out string error, cornerBlend))
				{
					return UpdateError(error);
				}
				val3 = cSGemData2.Xform;
			}
			else if (!GemOffsetCurve.GemOffset(cSGemData2, distance, out curve, doTransformUp: true, out error2, cornerBlend))
			{
				return UpdateError(error2);
			}
			((GeometryBase)curve).Transform(val3 * val2);
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)curve, val3 * val2, applyTransform: false));
		}
		OutCurve.SetData(cSDataArray);
		return base.Update();
	}
}
