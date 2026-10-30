using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("ScaleObjectObjectSpace", "{26C15DA5-4015-4459-8DC0-DD3FDCA00BD3}")]
public sealed class CSScaleObjectObjectSpace : CSBlock
{
	public readonly CSInputChannel iObj;

	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oObj;

	public CSScaleObjectObjectSpace(string identifier)
		: base(identifier)
	{
		iObj = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObj = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		if (iObj == null)
		{
			return UpdateMissing("Missing Object");
		}
		CSData data = iObj.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Object Data");
		}
		double value = 1.0;
		double value2 = 1.0;
		double value3 = 1.0;
		if (iX != null)
		{
			iX.GetDouble(ref value);
		}
		if (iY != null)
		{
			iY.GetDouble(ref value2);
		}
		if (iZ != null)
		{
			iZ.GetDouble(ref value3);
		}
		Transform val = Transform.Scale(Plane.WorldXY, value, value2, value3);
		CSDataHelper.GetTransformNoArray(data, out var value4);
		if (value4 == Transform.Unset)
		{
			CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out value4);
				if (((Transform)(ref value4)).IsValid)
				{
					break;
				}
			}
		}
		if (!((Transform)(ref value4)).IsValid)
		{
			value4 = Transform.Identity;
		}
		Transform val2 = default(Transform);
		if (!((Transform)(ref value4)).TryGetInverse(ref val2))
		{
			return UpdateError("Failure getting inverse");
		}
		if (!((Transform)(ref val2)).IsValid)
		{
			return UpdateError("Invalid Xform");
		}
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, value4 * val * val2);
		oObj.SetData(data2);
		return base.Update();
	}
}
