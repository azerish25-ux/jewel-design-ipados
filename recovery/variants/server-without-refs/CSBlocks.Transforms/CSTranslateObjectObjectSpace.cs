using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("TranslateObjectObjectSpace", "{B48D800E-F0C6-4A3B-A2EF-74973D00800F}")]
public sealed class CSTranslateObjectObjectSpace : CSBlock
{
	public readonly CSInputChannel InObject;

	public readonly CSInputChannel InX;

	public readonly CSInputChannel InY;

	public readonly CSInputChannel InZ;

	public readonly CSOutputChannel OutObject;

	public CSTranslateObjectObjectSpace(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InObject.Data;
		if (data == null)
		{
			return UpdateMissing("Object");
		}
		double value = 0.0;
		InX.GetDouble(ref value);
		double value2 = 0.0;
		InY.GetDouble(ref value2);
		double value3 = 0.0;
		InZ.GetDouble(ref value3);
		CSDataHelper.GetTransformNoArray(data, out var value4);
		if (!((Transform)(ref value4)).IsValid)
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
		value4 = (((Transform)(ref value4)).IsValid ? CSBlockUtil.CSUtilMath.TransformUnscale(value4) : Transform.Identity);
		Transform val = default(Transform);
		((Transform)(ref value4)).TryGetInverse(ref val);
		if (!((Transform)(ref val)).IsValid)
		{
			return UpdateError("Invalid Inverse Xform");
		}
		CSData cSData = data.Duplicate();
		if (cSData == null)
		{
			return UpdateMissing("Missing Object Duplicates");
		}
		Transform val2 = Transform.Translation(value, value2, value3);
		CSDataHelper.Transform(cSData, value4 * val2 * val);
		OutObject.SetData(cSData);
		return base.Update();
	}
}
