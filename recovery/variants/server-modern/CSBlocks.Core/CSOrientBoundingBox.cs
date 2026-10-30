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
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
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
		Transform xDown = default(Transform);
		if (!((Transform)(ref value4)).IsValid || ((Transform)(ref value4)).Equals(Transform.Identity))
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!data.GetBoundingBox(ref boundingBox))
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			xUp = Transform.Translation(((BoundingBox)(ref boundingBox)).Center - Point3d.Origin);
			((Transform)(ref xUp)).TryGetInverse(ref xDown);
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
		Point3d min = ((BoundingBox)(ref boundingBox2)).Min;
		((Point3d)(ref min)).X = ((Point3d)(ref min)).X - value;
		((Point3d)(ref min)).Y = ((Point3d)(ref min)).Y - value2;
		((Point3d)(ref min)).Z = ((Point3d)(ref min)).Z - value3;
		((BoundingBox)(ref boundingBox2)).Min = min;
		Point3d max = ((BoundingBox)(ref boundingBox2)).Max;
		((Point3d)(ref max)).X = ((Point3d)(ref max)).X + value;
		((Point3d)(ref max)).Y = ((Point3d)(ref max)).Y + value2;
		((Point3d)(ref max)).Z = ((Point3d)(ref max)).Z + value3;
		((BoundingBox)(ref boundingBox2)).Max = max;
		Point3d pointStore = default(Point3d);
		((Point3d)(ref pointStore))._002Ector(((BoundingBox)(ref boundingBox2)).Center);
		((Point3d)(ref pointStore)).Transform(xUp);
		Point3d pointStore2 = default(Point3d);
		((Point3d)(ref pointStore2))._002Ector(((BoundingBox)(ref boundingBox2)).Max);
		((Point3d)(ref pointStore2)).Transform(xUp);
		Point3d pointStore3 = default(Point3d);
		((Point3d)(ref pointStore3))._002Ector(((BoundingBox)(ref boundingBox2)).Min);
		((Point3d)(ref pointStore3)).Transform(xUp);
		Point3d val = ((BoundingBox)(ref boundingBox2)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox2)).Min;
		double number = x - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox2)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox2)).Min;
		double number2 = y - ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox2)).Max;
		double z = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBox2)).Min;
		double number3 = z - ((Point3d)(ref val)).Z;
		OutCenter.SetData(new CSPointData(pointStore));
		OutMax.SetData(new CSPointData(pointStore2));
		OutMin.SetData(new CSPointData(pointStore3));
		CSOutputChannel outMaxX = OutMaxX;
		val = ((BoundingBox)(ref boundingBox2)).Max;
		outMaxX.SetData(new CSNumberData(((Point3d)(ref val)).X));
		CSOutputChannel outMinX = OutMinX;
		val = ((BoundingBox)(ref boundingBox2)).Min;
		outMinX.SetData(new CSNumberData(((Point3d)(ref val)).X));
		OutSizeX.SetData(new CSNumberData(number));
		CSOutputChannel outMaxY = OutMaxY;
		val = ((BoundingBox)(ref boundingBox2)).Max;
		outMaxY.SetData(new CSNumberData(((Point3d)(ref val)).Y));
		CSOutputChannel outMinY = OutMinY;
		val = ((BoundingBox)(ref boundingBox2)).Min;
		outMinY.SetData(new CSNumberData(((Point3d)(ref val)).Y));
		OutSizeY.SetData(new CSNumberData(number2));
		CSOutputChannel outMaxZ = OutMaxZ;
		val = ((BoundingBox)(ref boundingBox2)).Max;
		outMaxZ.SetData(new CSNumberData(((Point3d)(ref val)).Z));
		CSOutputChannel outMinZ = OutMinZ;
		val = ((BoundingBox)(ref boundingBox2)).Min;
		outMinZ.SetData(new CSNumberData(((Point3d)(ref val)).Z));
		OutSizeZ.SetData(new CSNumberData(number3));
		OutTransformOrientUp.SetData(new CSXformData(xUp));
		OutTransformOrientDown.SetData(new CSXformData(xDown));
		return base.Update();
	}
}
