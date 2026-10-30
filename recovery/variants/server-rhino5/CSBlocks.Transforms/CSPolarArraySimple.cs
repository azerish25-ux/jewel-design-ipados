using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("PolarArraySimple", "{CD14165D-1A8E-4c8a-8F89-246D9E518F2A}")]
public sealed class CSPolarArraySimple : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iCount;

	public readonly CSInputChannel iAngle;

	public readonly CSInputChannel iAngleOffset;

	public readonly CSInputChannel iAxis;

	public readonly CSInputChannel iRotation;

	public readonly CSOutputChannel oObject;

	public CSPolarArraySimple(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngleOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAxis = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iObject.Data;
		double value = 1.0;
		iCount.GetDouble(ref value);
		double value2 = 360.0;
		iAngle.GetDouble(ref value2);
		double value3 = 0.0;
		iAngleOffset.GetDouble(ref value3);
		double value4 = -1.0;
		iAxis.GetDouble(ref value4);
		double value5 = 0.0;
		iRotation.GetDouble(ref value5);
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value5), new Vector3d(0.0, 0.0, 1.0), new Point3d(0.0, 0.0, 0.0));
		Vector3d rotationAxis = new Vector3d(0.0, 1.0, 0.0);
		if (!CSBlockUtil.CSUtilMath.DoubleEqual(value4, -1.0))
		{
			if (value4 > 0.5 && value4 < 1.5)
			{
				rotationAxis = new Vector3d(0.0, 0.0, 1.0);
			}
			else if (value4 > 1.5 && value4 < 2.5)
			{
				rotationAxis = new Vector3d(1.0, 0.0, 0.0);
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		double num;
		double num2;
		if (value < 2.0)
		{
			num = 0.0;
			num2 = value3;
		}
		else
		{
			num = value2 / (value - 1.0);
			num2 = value3 - value2 / 2.0;
		}
		for (int i = 0; (double)i < value; i++)
		{
			Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num * (double)i + num2), rotationAxis, Point3d.Origin);
			CSData data2 = data.Duplicate();
			CSDataHelper.Transform(data2, transform2 * transform);
			cSDataArray.Append(data2);
		}
		oObject.SetData(cSDataArray);
		return base.Update();
	}
}
