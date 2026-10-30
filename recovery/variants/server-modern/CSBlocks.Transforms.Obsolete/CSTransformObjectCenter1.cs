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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InObject.Data;
		Transform val = Transform.Identity;
		Transform val2 = Transform.Identity;
		bool value = false;
		InCenter.GetBoolean(ref value);
		if (data != null && value)
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			data.GetTightBoundingBox(ref boundingBox);
			Point3d center = ((BoundingBox)(ref boundingBox)).Center;
			double num = 0.0 - ((Point3d)(ref center)).X;
			center = ((BoundingBox)(ref boundingBox)).Center;
			double num2 = 0.0 - ((Point3d)(ref center)).Y;
			center = ((BoundingBox)(ref boundingBox)).Center;
			val2 = Transform.Translation(num, num2, 0.0 - ((Point3d)(ref center)).Z);
			center = ((BoundingBox)(ref boundingBox)).Center;
			double x = ((Point3d)(ref center)).X;
			center = ((BoundingBox)(ref boundingBox)).Center;
			double y = ((Point3d)(ref center)).Y;
			center = ((BoundingBox)(ref boundingBox)).Center;
			val = Transform.Translation(x, y, ((Point3d)(ref center)).Z);
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
		Transform val3 = Transform.Identity;
		for (int j = 0; j < flattenArray.GetArrayCount(); j++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(j);
			if (arrayItem != null)
			{
				CSDataHelper.GetTransformNoArray(arrayItem, out var value2);
				if (((Transform)(ref value2)).IsValid)
				{
					val3 = value2 * val3;
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
		CSDataHelper.Transform(data3, val * val3 * val2);
		OutObjectTransformed.SetData(data3);
		return base.Update();
	}
}
