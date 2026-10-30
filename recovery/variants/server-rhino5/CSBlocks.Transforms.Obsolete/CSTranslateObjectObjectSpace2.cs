using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms.Obsolete;

[Obsolete]
[CSBlock("TranslateObjectObjectSpace_V2", "{9D6E85A2-E126-4f06-B133-3441D672B947}")]
public sealed class CSTranslateObjectObjectSpace2 : CSBlock
{
	public readonly CSInputChannel InObject;

	public readonly CSInputChannel InX;

	public readonly CSInputChannel InY;

	public readonly CSInputChannel InZ;

	public readonly CSOutputChannel OutObject;

	public CSTranslateObjectObjectSpace2(string identifier)
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
		if (!value4.IsValid)
		{
			CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out value4);
				if (value4.IsValid)
				{
					break;
				}
			}
		}
		if (!value4.IsValid)
		{
			value4 = Transform.Identity;
		}
		value4 = CSBlockUtil.CSUtilMath.TransformUnscale_BackwardCompatibility(value4);
		value4.TryGetInverse(out var inverseTransform);
		if (inverseTransform == Transform.Unset)
		{
			return UpdateError("Invalid Inverse Xform");
		}
		CSData cSData = data.Duplicate();
		if (cSData == null)
		{
			return UpdateMissing("Missing Object Duplicates");
		}
		Transform transform = Transform.Translation(value, value2, value3);
		CSDataHelper.Transform(cSData, value4 * transform * inverseTransform);
		OutObject.SetData(cSData);
		return base.Update();
	}
}
