using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("TransformObjectXform", "{BAE265DE-E413-4c18-949F-B984A32F698D}")]
public sealed class CSTransformObjectXform : CSBlockExpandableInput
{
	public CSOutputChannel OutTransformation { get; }

	protected override string ExpandableInputChannelIdentifier => "InTransformation";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Transform;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public CSTransformObjectXform(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
		OutTransformation = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray cSDataArray = new CSDataArray();
		foreach (CSInputChannel nonExpandableInputChannel in base.NonExpandableInputChannels)
		{
			if (nonExpandableInputChannel != null)
			{
				CSData data = nonExpandableInputChannel.Data;
				if (data != null)
				{
					cSDataArray.Append(data);
				}
			}
		}
		CSDataArray flattenArray = cSDataArray.GetFlattenArray();
		Transform transform = Transform.Identity;
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem != null)
			{
				CSDataHelper.GetTransformNoArray(arrayItem, out var value);
				if (value.IsValid)
				{
					transform = value * transform;
				}
			}
		}
		OutTransformation.SetData(new CSXformData(transform));
		return base.Update();
	}
}
