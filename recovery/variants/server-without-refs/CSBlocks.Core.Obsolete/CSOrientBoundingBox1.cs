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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
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
		if (((Transform)(ref value)).IsValid && !CSBlockUtil.CSUtilMath.GetUnscaledXforms(value, out xUp, out xDown))
		{
			xDown = Transform.Identity;
			xUp = Transform.Identity;
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox, doGrow: false, xDown))
		{
			return UpdateError("CSOrientBoundingBox: Get Tight Bounding Box Failed");
		}
		oMinPoint.SetData(new CSPointData(((BoundingBox)(ref boundingBox)).Min));
		oMaxPoint.SetData(new CSPointData(((BoundingBox)(ref boundingBox)).Max));
		oSizePoint.SetData(new CSPointData(new Point3d(((BoundingBox)(ref boundingBox)).Max - ((BoundingBox)(ref boundingBox)).Min)));
		CSOutputChannel cSOutputChannel = oSizeX;
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel.SetData(new CSNumberData(x - ((Point3d)(ref val)).X));
		CSOutputChannel cSOutputChannel2 = oSizeY;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel2.SetData(new CSNumberData(y - ((Point3d)(ref val)).Y));
		CSOutputChannel cSOutputChannel3 = oSizeZ;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double z = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel3.SetData(new CSNumberData(z - ((Point3d)(ref val)).Z));
		CSOutputChannel cSOutputChannel4 = oMaxZ;
		val = ((BoundingBox)(ref boundingBox)).Max;
		cSOutputChannel4.SetData(new CSNumberData(((Point3d)(ref val)).Z));
		CSOutputChannel cSOutputChannel5 = oLowerLeft;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double x2 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double y2 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel5.SetData(new CSPointData(new Point3d(x2, y2, ((Point3d)(ref val)).Z)));
		CSOutputChannel cSOutputChannel6 = oLowerRight;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double x3 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double y3 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel6.SetData(new CSPointData(new Point3d(x3, y3, ((Point3d)(ref val)).Z)));
		CSOutputChannel cSOutputChannel7 = oUpperLeft;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double x4 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y4 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel7.SetData(new CSPointData(new Point3d(x4, y4, ((Point3d)(ref val)).Z)));
		CSOutputChannel cSOutputChannel8 = oUpperRight;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double x5 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y5 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel8.SetData(new CSPointData(new Point3d(x5, y5, ((Point3d)(ref val)).Z)));
		return base.Update();
	}
}
