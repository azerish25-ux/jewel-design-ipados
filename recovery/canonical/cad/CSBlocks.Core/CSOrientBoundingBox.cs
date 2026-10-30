using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("OrientBoundingBox", "{084ADD70-CD92-4E53-9AF4-74B2E2384745}")]
public sealed class CSOrientBoundingBox : CSBlock
{
	public CSInputChannel InObject { get; }

	public CSInputChannel InGrowX { get; }

	public CSInputChannel InGrowY { get; }

	public CSInputChannel InGrowZ { get; }

	public CSOutputChannel OutCenter { get; }

	public CSOutputChannel OutMax { get; }

	public CSOutputChannel OutMin { get; }

	public CSOutputChannel OutMaxX { get; }

	public CSOutputChannel OutMinX { get; }

	public CSOutputChannel OutSizeX { get; }

	public CSOutputChannel OutMaxY { get; }

	public CSOutputChannel OutMinY { get; }

	public CSOutputChannel OutSizeY { get; }

	public CSOutputChannel OutMaxZ { get; }

	public CSOutputChannel OutMinZ { get; }

	public CSOutputChannel OutSizeZ { get; }

	public CSOutputChannel OutTransformOrientUp { get; }

	public CSOutputChannel OutTransformOrientDown { get; }

	public CSOrientBoundingBox(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(GetPropertyName(() => InObject), this, ChannelDataType.Any, ChannelArrayType.Individual);
		InGrowX = new CSInputChannel(GetPropertyName(() => InGrowX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGrowY = new CSInputChannel(GetPropertyName(() => InGrowY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGrowZ = new CSInputChannel(GetPropertyName(() => InGrowZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutCenter = new CSOutputChannel(GetPropertyName(() => OutCenter), this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutMax = new CSOutputChannel(GetPropertyName(() => OutMax), this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutMin = new CSOutputChannel(GetPropertyName(() => OutMin), this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutMaxX = new CSOutputChannel(GetPropertyName(() => OutMaxX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinX = new CSOutputChannel(GetPropertyName(() => OutMinX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSizeX = new CSOutputChannel(GetPropertyName(() => OutSizeX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMaxY = new CSOutputChannel(GetPropertyName(() => OutMaxY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinY = new CSOutputChannel(GetPropertyName(() => OutMinY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSizeY = new CSOutputChannel(GetPropertyName(() => OutSizeY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMaxZ = new CSOutputChannel(GetPropertyName(() => OutMaxZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinZ = new CSOutputChannel(GetPropertyName(() => OutMinZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSizeZ = new CSOutputChannel(GetPropertyName(() => OutSizeZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutTransformOrientUp = new CSOutputChannel(GetPropertyName(() => OutTransformOrientUp), this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutTransformOrientDown = new CSOutputChannel(GetPropertyName(() => OutTransformOrientDown), this, ChannelDataType.Transform, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSData data = InObject.Data;
		if (data == null)
		{
			return UpdateMissing("Missing object");
		}
		double value = 0.0;
		InGrowX.GetDouble(ref value);
		double value2 = 0.0;
		InGrowY.GetDouble(ref value2);
		double value3 = 0.0;
		InGrowZ.GetDouble(ref value3);
		CSDataHelper.GetTransformNoArray(data, out var value4);
		Transform xUp;
		Transform xDown;
		if (!value4.IsValid || value4.Equals(Transform.Identity))
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!data.GetBoundingBox(ref boundingBox))
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			xUp = Transform.Translation(boundingBox.Center - Point3d.Origin);
			xUp.TryGetInverse(out xDown);
		}
		else if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value4, out xUp, out xDown))
		{
			return UpdateError("GetUnscaledXforms failed");
		}
		CSData cSData = data.Duplicate();
		CSDataHelper.Transform(cSData, xDown);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSData.GetBoundingBox(ref boundingBox2))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		Point3d min = boundingBox2.Min;
		min.X -= value;
		min.Y -= value2;
		min.Z -= value3;
		boundingBox2.Min = min;
		Point3d max = boundingBox2.Max;
		max.X += value;
		max.Y += value2;
		max.Z += value3;
		boundingBox2.Max = max;
		Point3d pointStore = new Point3d(boundingBox2.Center);
		pointStore.Transform(xUp);
		Point3d pointStore2 = new Point3d(boundingBox2.Max);
		pointStore2.Transform(xUp);
		Point3d pointStore3 = new Point3d(boundingBox2.Min);
		pointStore3.Transform(xUp);
		double number = boundingBox2.Max.X - boundingBox2.Min.X;
		double number2 = boundingBox2.Max.Y - boundingBox2.Min.Y;
		double number3 = boundingBox2.Max.Z - boundingBox2.Min.Z;
		OutCenter.SetData(new CSPointData(pointStore));
		OutMax.SetData(new CSPointData(pointStore2));
		OutMin.SetData(new CSPointData(pointStore3));
		OutMaxX.SetData(new CSNumberData(boundingBox2.Max.X));
		OutMinX.SetData(new CSNumberData(boundingBox2.Min.X));
		OutSizeX.SetData(new CSNumberData(number));
		OutMaxY.SetData(new CSNumberData(boundingBox2.Max.Y));
		OutMinY.SetData(new CSNumberData(boundingBox2.Min.Y));
		OutSizeY.SetData(new CSNumberData(number2));
		OutMaxZ.SetData(new CSNumberData(boundingBox2.Max.Z));
		OutMinZ.SetData(new CSNumberData(boundingBox2.Min.Z));
		OutSizeZ.SetData(new CSNumberData(number3));
		OutTransformOrientUp.SetData(new CSXformData(xUp));
		OutTransformOrientDown.SetData(new CSXformData(xDown));
		return base.Update();
	}
}
