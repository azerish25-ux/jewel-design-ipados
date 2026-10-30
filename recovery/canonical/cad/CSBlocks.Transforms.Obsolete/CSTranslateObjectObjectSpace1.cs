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
[CSBlock("TranslateObjectObjectSpace_V1", "{F52BF88D-E3ED-4a2f-BB4D-CB471A453BE7}")]
public sealed class CSTranslateObjectObjectSpace1 : CSBlock
{
	public readonly CSInputChannel InObject;

	public readonly CSInputChannel InX;

	public readonly CSInputChannel InY;

	public readonly CSInputChannel InZ;

	public readonly CSOutputChannel OutObject;

	public CSTranslateObjectObjectSpace1(string identifier)
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
		if (InObject == null)
		{
			return UpdateMissing("Object");
		}
		double value = 0.0;
		InX.GetDouble(ref value);
		double value2 = 0.0;
		InY.GetDouble(ref value2);
		double value3 = 0.0;
		InZ.GetDouble(ref value3);
		CSData data = InObject.Data;
		if (data == null)
		{
			return UpdateMissing("Object Data");
		}
		CSDataHelper.GetTransformNoArray(data, out var value4);
		if (!value4.IsValid)
		{
			value4 = Transform.Identity;
		}
		value4 = CSBlockUtil.CSUtilMath.TransformUnscale_BackwardCompatibility(value4);
		value4.TryGetInverse(out var inverseTransform);
		if (!inverseTransform.IsValid)
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
