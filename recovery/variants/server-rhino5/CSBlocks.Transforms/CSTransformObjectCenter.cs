using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("TransformObjectCenter", "{5A68810A-E29F-42A5-A531-80EA90AEC3F3}")]
public sealed class CSTransformObjectCenter : CSBlockExpandableInput
{
	public readonly CSInputChannel InObject;

	public readonly CSInputChannel InCenter;

	public readonly CSOutputChannel OutObjectTransformed;

	protected override string ExpandableInputChannelIdentifier => "InTransformation";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Transform;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public override bool Update()
	{
		CSData data = InObject.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Object");
		}
		bool value = false;
		InCenter.GetBoolean(ref value);
		Transform transform = Transform.Identity;
		Transform transform2 = Transform.Identity;
		if (value)
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!data.GetTightBoundingBox(ref boundingBox))
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			transform = Transform.Translation(boundingBox.Center.X, boundingBox.Center.Y, boundingBox.Center.Z);
			transform2 = Transform.Translation(0.0 - boundingBox.Center.X, 0.0 - boundingBox.Center.Y, 0.0 - boundingBox.Center.Z);
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 2; i < base.NonExpandableInputChannels.Count; i++)
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
		Transform transform3 = Transform.Identity;
		for (int j = 0; j < flattenArray.GetArrayCount(); j++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(j);
			if (arrayItem != null)
			{
				CSDataHelper.GetTransformNoArray(arrayItem, out var value2);
				if (value2.IsValid)
				{
					transform3 = value2 * transform3;
				}
			}
		}
		CSData data3 = data.Duplicate();
		CSDataHelper.Transform(data3, transform * transform3 * transform2);
		OutObjectTransformed.SetData(data3);
		return base.Update();
	}

	public CSTransformObjectCenter(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InCenter = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		CreateExpansionInputChannel();
		OutObjectTransformed = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}
}
