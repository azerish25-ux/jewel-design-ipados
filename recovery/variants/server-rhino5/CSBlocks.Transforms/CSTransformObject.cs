using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("TransformObject", "{27F4537A-6FC3-404f-9ABD-EB53B7B3D8A1}")]
public sealed class CSTransformObject : CSBlockExpandableInput
{
	public readonly CSInputChannel InObject;

	public readonly CSOutputChannel OutObjectTransformed;

	protected override string ExpandableInputChannelIdentifier => "InTransformation";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Transform;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public CSTransformObject(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		CreateExpansionInputChannel();
		OutObjectTransformed = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (InObject == null)
		{
			return UpdateMissing("Missing Object");
		}
		CSData data = InObject.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Object");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 1; i < base.NonExpandableInputChannels.Count; i++)
		{
			if (base.NonExpandableInputChannels[i] != null)
			{
				CSData data2 = base.NonExpandableInputChannels[i].Data;
				if (data2 != null)
				{
					cSDataArray.Append(data2);
				}
			}
		}
		CSDataArray flattenArray = cSDataArray.GetFlattenArray();
		Transform transform = Transform.Identity;
		for (int j = 0; j < flattenArray.GetArrayCount(); j++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(j);
			if (arrayItem != null)
			{
				CSDataHelper.GetTransformNoArray(arrayItem, out var value);
				if (value.IsValid)
				{
					transform = value * transform;
				}
			}
		}
		CSData data3 = data.Duplicate();
		CSDataHelper.Transform(data3, transform);
		OutObjectTransformed.SetData(data3);
		return base.Update();
	}
}
