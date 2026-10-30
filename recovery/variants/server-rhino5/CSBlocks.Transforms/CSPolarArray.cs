using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("PolarArray", "{ED5689DB-D513-4f95-92EA-A52FC997598A}")]
public sealed class CSPolarArray : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iCount;

	public readonly CSInputChannel iCountSub;

	public readonly CSInputChannel iInitialRotation;

	public readonly CSInputChannel iRotation;

	public readonly CSInputChannel iAxis;

	public readonly CSInputChannel iShowCount;

	public readonly CSInputChannel iShowCountSub;

	public readonly CSInputChannel iEnableShowCount;

	public readonly CSInputChannel iEnableShowCountSub;

	public readonly CSOutputChannel oObject;

	public CSPolarArray(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCountSub = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iInitialRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAxis = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShowCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShowCountSub = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableShowCount = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iEnableShowCountSub = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iObject.Data;
		int value = 1;
		iCount.GetInteger(ref value);
		int value2 = 1;
		iCountSub.GetInteger(ref value2);
		double value3 = 0.0;
		iInitialRotation.GetDouble(ref value3);
		double value4 = 0.0;
		iRotation.GetDouble(ref value4);
		value4 = CSBlockUtil.CSUtilMath.ToRadians(value4);
		double value5 = -1.0;
		iAxis.GetDouble(ref value5);
		double value6 = -1.0;
		iShowCount.GetDouble(ref value6);
		double value7 = -1.0;
		iShowCountSub.GetDouble(ref value7);
		bool value8 = true;
		iEnableShowCount.GetBoolean(ref value8);
		bool value9 = true;
		iEnableShowCountSub.GetBoolean(ref value9);
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value4, new Vector3d(0.0, 0.0, 1.0), new Point3d(0.0, 0.0, 0.0));
		Vector3d rotationAxis = new Vector3d(0.0, 1.0, 0.0);
		Vector3d rotationAxis2 = new Vector3d(0.0, 0.0, 1.0);
		if (value5 != -1.0)
		{
			if (value5 > 0.5 && value5 < 1.5)
			{
				rotationAxis = new Vector3d(0.0, 0.0, 1.0);
				rotationAxis2 = new Vector3d(0.0, 1.0, 0.0);
			}
			else if (value5 < 2.5)
			{
				rotationAxis = new Vector3d(1.0, 0.0, 0.0);
				rotationAxis2 = new Vector3d(0.0, 1.0, 0.0);
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < value; i++)
		{
			bool flag = true;
			if (value8 && value6 != -1.0 && (double)i > value6 / 2.0 && (double)i < (double)value - value6 / 2.0)
			{
				flag = false;
			}
			for (int j = 0; j < value2 && flag; j++)
			{
				bool flag2 = true;
				if (value9 && value7 != -1.0 && (double)j > value7 / 2.0 && (double)j < (double)value2 - value7 / 2.0)
				{
					flag2 = false;
				}
				if (flag2)
				{
					Transform transform2 = CSBlockUtil.CSUtilMath.Rotation((double)j * CSBlockUtil.CSUtilMath.ToRadians(180.0) / (double)value2, rotationAxis2, Point3d.Origin);
					Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(((double)i + value3) * CSBlockUtil.CSUtilMath.ToRadians(360.0) / (double)value, rotationAxis, Point3d.Origin);
					CSData data2 = data.Duplicate();
					CSDataHelper.Transform(data2, transform3 * transform2 * transform);
					cSDataArray.Append(data2);
				}
			}
		}
		oObject.SetData(cSDataArray);
		return base.Update();
	}
}
