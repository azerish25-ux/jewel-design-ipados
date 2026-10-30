using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("ArrayObject", "{18E671A4-B9FC-4b0a-9B80-3D7A8DB41B08}")]
public sealed class CSArrayObject : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iXCount;

	public readonly CSInputChannel iYCount;

	public readonly CSInputChannel iZCount;

	public readonly CSInputChannel iXSpacing;

	public readonly CSInputChannel iYSpacing;

	public readonly CSInputChannel iZSpacing;

	public readonly CSInputChannel iXAdd;

	public readonly CSInputChannel iYAdd;

	public readonly CSInputChannel iZAdd;

	public readonly CSOutputChannel oObject;

	public CSArrayObject(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iXCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iYCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iXSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iYSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iXAdd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iYAdd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZAdd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		CSData data = iObject.Data;
		double value = 1.0;
		double value2 = 1.0;
		double value3 = 1.0;
		if (iXCount != null)
		{
			iXCount.GetDouble(ref value);
		}
		if (iYCount != null)
		{
			iYCount.GetDouble(ref value2);
		}
		if (iZCount != null)
		{
			iZCount.GetDouble(ref value3);
		}
		double value4 = 0.0;
		double value5 = 0.0;
		double value6 = 0.0;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (data.GetBoundingBox(ref boundingBox))
		{
			Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			value4 = ((Vector3d)(ref diagonal)).X;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			value5 = ((Vector3d)(ref diagonal)).Y;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			value6 = ((Vector3d)(ref diagonal)).Z;
		}
		if (iXSpacing != null)
		{
			iXSpacing.GetDouble(ref value4);
		}
		if (iYSpacing != null)
		{
			iYSpacing.GetDouble(ref value5);
		}
		if (iZSpacing != null)
		{
			iZSpacing.GetDouble(ref value6);
		}
		double value7 = 0.0;
		double value8 = 0.0;
		double value9 = 0.0;
		if (iXAdd != null)
		{
			iXAdd.GetDouble(ref value7);
		}
		if (iYAdd != null)
		{
			iYAdd.GetDouble(ref value8);
		}
		if (iZAdd != null)
		{
			iZAdd.GetDouble(ref value9);
		}
		value4 += value7;
		value5 += value8;
		value6 += value9;
		CSDataArray cSDataArray = new CSDataArray();
		double num = 0.0;
		for (int i = 0; (double)i < value; i++)
		{
			double num2 = 0.0;
			for (int j = 0; (double)j < value2; j++)
			{
				double num3 = 0.0;
				for (int k = 0; (double)k < value3; k++)
				{
					Transform transform = Transform.Translation(num, num2, num3);
					CSData data2 = data.Duplicate();
					CSDataHelper.Transform(data2, transform);
					cSDataArray.Append(data2);
					num3 += value6;
				}
				num2 += value5;
			}
			num += value4;
		}
		oObject.SetData(cSDataArray);
		return base.Update();
	}
}
