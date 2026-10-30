using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core.Obsolete;

[Obsolete]
[CSBlock("OrientBoundingBox_V1", "{D4BC99F4-A658-414f-9D3B-9785FB913A73}")]
public sealed class CSOrientBoundingBox1 : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSOutputChannel oMinPoint;

	public readonly CSOutputChannel oMaxPoint;

	public readonly CSOutputChannel oSizePoint;

	public readonly CSOutputChannel oSizeX;

	public readonly CSOutputChannel oSizeY;

	public readonly CSOutputChannel oSizeZ;

	public readonly CSOutputChannel oMaxZ;

	public readonly CSOutputChannel oLowerLeft;

	public readonly CSOutputChannel oLowerRight;

	public readonly CSOutputChannel oUpperLeft;

	public readonly CSOutputChannel oUpperRight;

	public CSOrientBoundingBox1(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Any);
		oMinPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oMaxPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oSizePoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMaxZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oLowerLeft = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oLowerRight = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oUpperLeft = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oUpperRight = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (iObject == null)
		{
			return UpdateMissing("Bounding Box Down Null Input");
		}
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateError("Bounding Box Down Null Data");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		Transform xDown = Transform.Identity;
		Transform xUp = Transform.Identity;
		if (value.IsValid && !CSBlockUtil.CSUtilMath.GetUnscaledXforms(value, out xUp, out xDown))
		{
			xDown = Transform.Identity;
			xUp = Transform.Identity;
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox, doGrow: false, xDown))
		{
			return UpdateError("CSOrientBoundingBox: Get Tight Bounding Box Failed");
		}
		oMinPoint.SetData(new CSPointData(boundingBox.Min));
		oMaxPoint.SetData(new CSPointData(boundingBox.Max));
		oSizePoint.SetData(new CSPointData(new Point3d(boundingBox.Max - boundingBox.Min)));
		oSizeX.SetData(new CSNumberData(boundingBox.Max.X - boundingBox.Min.X));
		oSizeY.SetData(new CSNumberData(boundingBox.Max.Y - boundingBox.Min.Y));
		oSizeZ.SetData(new CSNumberData(boundingBox.Max.Z - boundingBox.Min.Z));
		oMaxZ.SetData(new CSNumberData(boundingBox.Max.Z));
		oLowerLeft.SetData(new CSPointData(new Point3d(boundingBox.Min.X, boundingBox.Min.Y, boundingBox.Min.Z)));
		oLowerRight.SetData(new CSPointData(new Point3d(boundingBox.Max.X, boundingBox.Min.Y, boundingBox.Min.Z)));
		oUpperLeft.SetData(new CSPointData(new Point3d(boundingBox.Min.X, boundingBox.Max.Y, boundingBox.Min.Z)));
		oUpperRight.SetData(new CSPointData(new Point3d(boundingBox.Max.X, boundingBox.Max.Y, boundingBox.Min.Z)));
		return base.Update();
	}
}
