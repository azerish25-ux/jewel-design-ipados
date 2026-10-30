using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("AlternateStones", "{FD16E211-1019-4080-899B-58B12A4C9321}")]
public sealed class CSAlternateStones : CSBlock
{
	public readonly CSInputChannel InOriginalGems;

	public readonly CSInputChannel InGemToAlternate;

	public readonly CSInputChannel InAlternateNumber;

	public readonly CSInputChannel InOffset;

	public readonly CSInputChannel InRotation;

	public readonly CSOutputChannel OutGems;

	public CSAlternateStones(string identifier)
		: base(identifier)
	{
		InOriginalGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InGemToAlternate = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InAlternateNumber = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 2);
		InOffset = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		List<CSGemData> gemList = InOriginalGems.GemList;
		CSGemData gem = InGemToAlternate.Gem;
		Transform val = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(InRotation.Double), Vector3d.ZAxis, Point3d.Origin);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < gemList.Count; i++)
		{
			if ((i + InOffset.Integer) % InAlternateNumber.Integer != 0)
			{
				cSDataArray.Append(gemList[i]);
				continue;
			}
			CSGemData cSGemData = gemList[i];
			cSGemData = new CSGemData(gem.GemType, gem.GemShape, gem.Def, cSGemData.Xform * val);
			cSDataArray.Append(cSGemData);
		}
		OutGems.SetData(cSDataArray);
		return base.Update();
	}
}
