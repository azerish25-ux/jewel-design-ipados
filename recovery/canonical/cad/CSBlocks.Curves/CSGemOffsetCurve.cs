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
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				continue;
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData2.Xform, out var xUp);
			Vector3d vector3d = new Vector3d(Vector3d.ZAxis);
			vector3d.Transform(xUp);
			Transform transform = Transform.Translation(vector3d * num);
			Transform transform2 = Transform.Identity;
			Curve curve;
			string error2;
			if (cSGemData != null)
			{
				if (!GemOffsetCurve.GemOffset(cSGemData, distance, out curve, false, out string error, cornerBlend))
				{
					return UpdateError(error);
				}
				transform2 = cSGemData2.Xform;
			}
			else if (!GemOffsetCurve.GemOffset(cSGemData2, distance, out curve, true, out error2, cornerBlend))
			{
				return UpdateError(error2);
			}
			curve.Transform(transform2 * transform);
			cSDataArray.Append(new CSOnGeometryData(curve, transform2 * transform, applyTransform: false));
		}
		OutCurve.SetData(cSDataArray);
		return base.Update();
	}
}
