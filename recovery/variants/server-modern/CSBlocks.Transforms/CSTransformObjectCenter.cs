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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InObject.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Object");
		}
		bool value = false;
		InCenter.GetBoolean(ref value);
		Transform val = Transform.Identity;
		Transform val2 = Transform.Identity;
		if (value)
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!data.GetTightBoundingBox(ref boundingBox))
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			Point3d center = ((BoundingBox)(ref boundingBox)).Center;
			double x = ((Point3d)(ref center)).X;
			center = ((BoundingBox)(ref boundingBox)).Center;
			double y = ((Point3d)(ref center)).Y;
			center = ((BoundingBox)(ref boundingBox)).Center;
			val = Transform.Translation(x, y, ((Point3d)(ref center)).Z);
			center = ((BoundingBox)(ref boundingBox)).Center;
			double num = 0.0 - ((Point3d)(ref center)).X;
			center = ((BoundingBox)(ref boundingBox)).Center;
			double num2 = 0.0 - ((Point3d)(ref center)).Y;
			center = ((BoundingBox)(ref boundingBox)).Center;
			val2 = Transform.Translation(num, num2, 0.0 - ((Point3d)(ref center)).Z);
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
		CSData data3 = data.Duplicate();
		CSDataHelper.Transform(data3, val * val3 * val2);
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
