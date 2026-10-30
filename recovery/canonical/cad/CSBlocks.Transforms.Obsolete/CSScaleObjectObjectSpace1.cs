using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms.Obsolete;

[Obsolete]
[CSBlock("ScaleObjectObjectSpace_V1", "{9CE4C2D9-CE35-4b7f-9A76-869D80722545}")]
public sealed class CSScaleObjectObjectSpace1 : CSBlock
{
	public CSInputChannel iObj { get; }

	public CSInputChannel iX { get; }

	public CSInputChannel iY { get; }

	public CSInputChannel iZ { get; }

	public CSOutputChannel oObj { get; }

	public CSScaleObjectObjectSpace1(string identifier)
		: base(identifier)
	{
		iObj = new CSInputChannel(GetPropertyName(() => iObj), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iX = new CSInputChannel(GetPropertyName(() => iX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(GetPropertyName(() => iY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(GetPropertyName(() => iZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObj = new CSOutputChannel(GetPropertyName(() => oObj), this, ChannelDataType.Any, ChannelArrayType.Any);
	}

	public override bool Update()
	{
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
		Transform transform = Transform.Scale(Plane.WorldXY, value, value2, value3);
		CSDataHelper.GetTransformNoArray(data, out var value4);
		if (!value4.IsValid)
		{
			value4 = Transform.Identity;
		}
		value4.TryGetInverse(out var inverseTransform);
		if (!inverseTransform.IsValid)
		{
			return UpdateError("Invalid Xform");
		}
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, value4 * transform * inverseTransform);
		oObj.SetData(data2);
		return base.Update();
	}
}
