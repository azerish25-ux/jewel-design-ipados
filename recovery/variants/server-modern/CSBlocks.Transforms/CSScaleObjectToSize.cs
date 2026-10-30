using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("ScaleObjectToSize", "{65CD6296-1BB8-4fec-A6B6-ADFB74BC8517}")]
public sealed class CSScaleObjectToSize : CSBlock
{
	public readonly CSInputChannel iObj;

	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oObj;

	public CSScaleObjectToSize(string identifier)
		: base(identifier)
	{
		iObj = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObj = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		if (iObj == null)
		{
			return UpdateMissing("CSScaleObjectToSize: Bad Object\n");
		}
		CSData data = iObj.Data;
		if (data == null)
		{
			return UpdateMissing("CSScaleObjectToSize: Bad Object\n");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("CSScaleObjectToSize: Unable to calculate bounding box\n");
		}
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num = x - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = y - ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double z = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num3 = z - ((Point3d)(ref val)).Z;
		if (num < 0.001)
		{
			num = 1.0;
		}
		if (num2 < 0.001)
		{
			num2 = 1.0;
		}
		if (num3 < 0.001)
		{
			num3 = 1.0;
		}
		double num4 = 1.0;
		double num5 = 1.0;
		double num6 = 1.0;
		double value = 0.0;
		double value2 = 0.0;
		double value3 = 0.0;
		if (iX != null && iX.GetDouble(ref value))
		{
			num4 = value / num;
		}
		if (iY != null && iY.GetDouble(ref value2))
		{
			num5 = value2 / num2;
		}
		if (iZ != null && iZ.GetDouble(ref value3))
		{
			num6 = value3 / num3;
		}
		Transform val2 = default(Transform);
		val2 = Transform.Scale(Plane.WorldXY, num4, num5, num6);
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, val2);
		oObj.SetData(data2);
		return base.Update();
	}
}
