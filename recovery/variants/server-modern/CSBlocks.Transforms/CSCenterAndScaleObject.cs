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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("Object");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		data.GetTightBoundingBox(ref boundingBox);
		double value = 1.0;
		iSize.GetDouble(ref value);
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Center;
		double val2 = x - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Center;
		double x2 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double val3 = Math.Max(val2, x2 - ((Point3d)(ref val)).X);
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Center;
		double val4 = y - ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Center;
		double y2 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double val5 = Math.Max(val4, y2 - ((Point3d)(ref val)).Y);
		double num = Math.Max(val3, val5);
		val = ((BoundingBox)(ref boundingBox)).Center;
		double num2 = 0.0 - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Center;
		Vector3d val6 = new Vector3d(num2, 0.0 - ((Point3d)(ref val)).Y, 0.0);
		Transform val7 = Transform.Scale(Plane.WorldXY, value / num, value / num, value / num);
		Transform val8 = Transform.Translation(val6);
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, val7 * val8);
		oObject.SetData(data2);
		return base.Update();
	}
}
