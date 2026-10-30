using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Transforms;

[CSBlock("ObjectsOnTransforms", "{D1E92527-BFF6-45C8-83F3-0C0D6A262D34}")]
public sealed class CSObjectsOnTransforms : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iTransformArray;

	public readonly CSOutputChannel oObject;

	public CSObjectsOnTransforms(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iTransformArray = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Array);
		oObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Object");
		}
		CSDataArray flattenArray = iTransformArray.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Missing Points");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out var value);
			CSData cSData = data.Duplicate();
			if (cSData != null)
			{
				CSDataHelper.Transform(cSData, value);
				cSDataArray.Append(cSData);
			}
		}
		oObject.SetData(cSDataArray);
		return base.Update();
	}
}
