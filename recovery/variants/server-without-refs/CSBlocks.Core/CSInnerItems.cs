using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("InnerItems", "{831D6CD2-8B62-46dd-A029-947D7CC5F9E5}")]
public sealed class CSInnerItems : CSBlock
{
	public CSInputChannel iObject;

	public CSOutputChannel oMinPoint;

	public CSOutputChannel oMaxPoint;

	public CSOutputChannel oSizePoint;

	public CSOutputChannel oSizeX;

	public CSOutputChannel oSizeY;

	public CSOutputChannel oSizeZ;

	public CSOutputChannel oMaxZ;

	public CSOutputChannel oObjects;

	public CSInnerItems(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		oMinPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oMaxPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oSizePoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMaxZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("Bounding Box Null Data");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Bound Box failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		Point3d val;
		for (int i = 0; i < CSDataHelper.GetArrayCount(data); i++)
		{
			CSData arrayItem = CSDataHelper.GetArrayItem(data, i);
			BoundingBox boundingBox2 = BoundingBox.Unset;
			arrayItem.GetTightBoundingBox(ref boundingBox2);
			double num = 0.0;
			double num2 = 0.0;
			val = ((BoundingBox)(ref boundingBox)).Max;
			double x = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Min;
			if (x != ((Point3d)(ref val)).X)
			{
				num2 += 1.0;
				val = ((BoundingBox)(ref boundingBox2)).Max;
				double x2 = ((Point3d)(ref val)).X;
				val = ((BoundingBox)(ref boundingBox2)).Min;
				if (x2 == ((Point3d)(ref val)).X)
				{
					val = ((BoundingBox)(ref boundingBox)).Max;
					double x3 = ((Point3d)(ref val)).X;
					val = ((BoundingBox)(ref boundingBox2)).Max;
					if (x3 == ((Point3d)(ref val)).X)
					{
						num += 1.0;
					}
					else
					{
						val = ((BoundingBox)(ref boundingBox)).Min;
						double x4 = ((Point3d)(ref val)).X;
						val = ((BoundingBox)(ref boundingBox2)).Min;
						if (x4 == ((Point3d)(ref val)).X)
						{
							num += 1.0;
						}
					}
				}
				else
				{
					val = ((BoundingBox)(ref boundingBox)).Max;
					double x5 = ((Point3d)(ref val)).X;
					val = ((BoundingBox)(ref boundingBox2)).Max;
					if (x5 == ((Point3d)(ref val)).X)
					{
						val = ((BoundingBox)(ref boundingBox)).Min;
						double x6 = ((Point3d)(ref val)).X;
						val = ((BoundingBox)(ref boundingBox2)).Min;
						if (x6 == ((Point3d)(ref val)).X)
						{
							num += 1.0;
						}
					}
				}
			}
			val = ((BoundingBox)(ref boundingBox)).Max;
			double y = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Min;
			if (y != ((Point3d)(ref val)).Y)
			{
				num2 += 1.0;
				val = ((BoundingBox)(ref boundingBox2)).Max;
				double y2 = ((Point3d)(ref val)).Y;
				val = ((BoundingBox)(ref boundingBox2)).Min;
				if (y2 == ((Point3d)(ref val)).Y)
				{
					val = ((BoundingBox)(ref boundingBox)).Max;
					double y3 = ((Point3d)(ref val)).Y;
					val = ((BoundingBox)(ref boundingBox2)).Max;
					if (y3 == ((Point3d)(ref val)).Y)
					{
						num += 1.0;
					}
					else
					{
						val = ((BoundingBox)(ref boundingBox)).Min;
						double y4 = ((Point3d)(ref val)).Y;
						val = ((BoundingBox)(ref boundingBox2)).Min;
						if (y4 == ((Point3d)(ref val)).Y)
						{
							num += 1.0;
						}
					}
				}
				else
				{
					val = ((BoundingBox)(ref boundingBox)).Max;
					double y5 = ((Point3d)(ref val)).Y;
					val = ((BoundingBox)(ref boundingBox2)).Max;
					if (y5 == ((Point3d)(ref val)).Y)
					{
						val = ((BoundingBox)(ref boundingBox)).Min;
						double y6 = ((Point3d)(ref val)).Y;
						val = ((BoundingBox)(ref boundingBox2)).Min;
						if (y6 == ((Point3d)(ref val)).Y)
						{
							num += 1.0;
						}
					}
				}
			}
			val = ((BoundingBox)(ref boundingBox)).Max;
			double z = ((Point3d)(ref val)).Z;
			val = ((BoundingBox)(ref boundingBox)).Min;
			if (z != ((Point3d)(ref val)).Z)
			{
				num2 += 1.0;
				val = ((BoundingBox)(ref boundingBox2)).Max;
				double z2 = ((Point3d)(ref val)).Z;
				val = ((BoundingBox)(ref boundingBox2)).Min;
				if (z2 == ((Point3d)(ref val)).Z)
				{
					val = ((BoundingBox)(ref boundingBox)).Max;
					double z3 = ((Point3d)(ref val)).Z;
					val = ((BoundingBox)(ref boundingBox2)).Max;
					if (z3 == ((Point3d)(ref val)).Z)
					{
						num += 1.0;
					}
					else
					{
						val = ((BoundingBox)(ref boundingBox)).Min;
						double z4 = ((Point3d)(ref val)).Z;
						val = ((BoundingBox)(ref boundingBox2)).Min;
						if (z4 == ((Point3d)(ref val)).Z)
						{
							num += 1.0;
						}
					}
				}
				else
				{
					val = ((BoundingBox)(ref boundingBox)).Max;
					double z5 = ((Point3d)(ref val)).Z;
					val = ((BoundingBox)(ref boundingBox2)).Max;
					if (z5 == ((Point3d)(ref val)).Z)
					{
						val = ((BoundingBox)(ref boundingBox)).Min;
						double z6 = ((Point3d)(ref val)).Z;
						val = ((BoundingBox)(ref boundingBox2)).Min;
						if (z6 == ((Point3d)(ref val)).Z)
						{
							num += 1.0;
						}
					}
				}
			}
			if (num2 > 0.0 && num / num2 < 0.5)
			{
				cSDataArray.Append(arrayItem);
			}
		}
		boundingBox = BoundingBox.Unset;
		cSDataArray.GetTightBoundingBox(ref boundingBox);
		oMinPoint.SetData(new CSPointData(((BoundingBox)(ref boundingBox)).Min));
		oMaxPoint.SetData(new CSPointData(((BoundingBox)(ref boundingBox)).Max));
		oSizePoint.SetData(new CSPointData(new Point3d(((BoundingBox)(ref boundingBox)).Max - ((BoundingBox)(ref boundingBox)).Min)));
		CSOutputChannel cSOutputChannel = oSizeX;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double x7 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel.SetData(new CSNumberData(x7 - ((Point3d)(ref val)).X));
		CSOutputChannel cSOutputChannel2 = oSizeY;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y7 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel2.SetData(new CSNumberData(y7 - ((Point3d)(ref val)).Y));
		CSOutputChannel cSOutputChannel3 = oSizeZ;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double z7 = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel3.SetData(new CSNumberData(z7 - ((Point3d)(ref val)).Z));
		CSOutputChannel cSOutputChannel4 = oMaxZ;
		val = ((BoundingBox)(ref boundingBox)).Max;
		cSOutputChannel4.SetData(new CSNumberData(((Point3d)(ref val)).Z));
		oObjects.SetData(cSDataArray);
		return base.Update();
	}
}
