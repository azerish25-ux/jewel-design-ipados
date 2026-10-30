using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms.Obsolete;

[Obsolete]
[CSBlock("TransformObjectCenter_V1", "{7EE23285-4C64-4769-A1E5-569C505AD749}")]
public sealed class CSTransformObjectCenter1 : CSBlockExpandableInput
{
	public CSInputChannel InObject { get; }

	public CSInputChannel InCenter { get; }

	public CSOutputChannel OutObjectTransformed { get; }

	protected override string ExpandableInputChannelIdentifier => "InTransformation";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Transform;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public CSTransformObjectCenter1(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(GetPropertyName(() => InObject), this, ChannelDataType.Any, ChannelArrayType.Any);
		InCenter = new CSInputChannel(GetPropertyName(() => InCenter), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		CreateExpansionInputChannel();
		OutObjectTransformed = new CSOutputChannel(GetPropertyName(() => OutObjectTransformed), this, ChannelDataType.Any, ChannelArrayType.Any);
	}

	public override bool Update()
	{
		CSData data = InObject.Data;
		Transform transform = Transform.Identity;
		Transform transform2 = Transform.Identity;
		bool value = false;
		InCenter.GetBoolean(ref value);
		if (data != null && value)
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			data.GetTightBoundingBox(ref boundingBox);
			transform2 = Transform.Translation(0.0 - boundingBox.Center.X, 0.0 - boundingBox.Center.Y, 0.0 - boundingBox.Center.Z);
			transform = Transform.Translation(boundingBox.Center.X, boundingBox.Center.Y, boundingBox.Center.Z);
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
		if (InObject == null)
		{
			UpdateMissing("Missing Object");
		}
		if (InObject.Data == null)
		{
			UpdateMissing("Missing Object");
		}
		CSData data3 = InObject.Data.Duplicate();
		CSDataHelper.Transform(data3, transform * transform3 * transform2);
		OutObjectTransformed.SetData(data3);
		return base.Update();
	}
}
