using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("CenterAndScaleObject", "{A7D85C2D-2B4D-405c-B252-3FE5007FC66F}")]
public sealed class CSCenterAndScaleObject : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iSize;

	public readonly CSOutputChannel oObject;

	public CSCenterAndScaleObject(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("Object");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		data.GetTightBoundingBox(ref boundingBox);
		double value = 1.0;
		iSize.GetDouble(ref value);
		double val = Math.Max(boundingBox.Max.X - boundingBox.Center.X, boundingBox.Center.X - boundingBox.Min.X);
		double val2 = Math.Max(boundingBox.Max.Y - boundingBox.Center.Y, boundingBox.Center.Y - boundingBox.Min.Y);
		double num = Math.Max(val, val2);
		Vector3d motion = new Vector3d(0.0 - boundingBox.Center.X, 0.0 - boundingBox.Center.Y, 0.0);
		Transform transform = Transform.Scale(Plane.WorldXY, value / num, value / num, value / num);
		Transform transform2 = Transform.Translation(motion);
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, transform * transform2);
		oObject.SetData(data2);
		return base.Update();
	}
}
