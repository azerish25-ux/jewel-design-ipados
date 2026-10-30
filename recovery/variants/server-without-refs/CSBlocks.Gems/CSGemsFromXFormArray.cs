using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Gems;

[CSBlock("GemsFromXFormArray", "{1421287F-4356-45E8-9E82-9355AF7397FE}")]
public sealed class CSGemsFromXFormArray : CSBlock
{
	public readonly CSInputChannel InGemShape;

	public readonly CSInputChannel InGemXFormArray;

	public readonly CSInputChannel InShowGems;

	public readonly CSOutputChannel OutGemArray;

	public CSGemsFromXFormArray(string identifier)
		: base(identifier)
	{
		InGemShape = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemXFormArray = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Array);
		InShowGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		OutGemArray = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		CSGemData gem = InGemShape.Gem;
		CSDataArray flattenArray = InGemXFormArray.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		if (flattenArray != null && InShowGems.Boolean)
		{
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSData data = gem.Duplicate();
				if (flattenArray.GetArrayItem(i) is CSXformData cSXformData)
				{
					cSXformData.GetTransform(out var value);
					CSDataHelper.Transform(data, value);
				}
				cSDataArray.Append(data);
			}
		}
		OutGemArray.SetData(cSDataArray);
		return base.Update();
	}
}
