using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms.Obsolete;

[Obsolete]
[CSBlock("Mirror_V1", "{20208FC4-D012-4524-A54D-41D657D6C5DD}")]
public sealed class CSMirror1 : CSBlock
{
	public CSInputChannel InObj { get; }

	public CSInputChannel InDuplicate { get; }

	public CSInputChannel InX { get; }

	public CSInputChannel InY { get; }

	public CSInputChannel InZ { get; }

	public CSInputChannel InXform { get; }

	public CSOutputChannel OutObj { get; }

	public CSMirror1(string identifier)
		: base(identifier)
	{
		InObj = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InDuplicate = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InX = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InY = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InZ = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InXform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutObj = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InObj.GetFlattenArray();
		bool value = false;
		InDuplicate.GetBoolean(ref value);
		bool value2 = false;
		InX.GetBoolean(ref value2);
		bool value3 = false;
		InY.GetBoolean(ref value3);
		bool value4 = false;
		InZ.GetBoolean(ref value4);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataHelper.GetTransformNoArray(InXform.Data, out var value5);
		Transform transform = Transform.Identity;
		Transform transform2 = Transform.Identity;
		if (value5.IsValid && value5.TryGetInverse(out var inverseTransform))
		{
			transform = inverseTransform;
			transform2 = value5;
		}
		if (value)
		{
			if (value2)
			{
				Transform transform3 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
				double num = cSDataArray.GetArrayCount();
				Transform transform4 = transform2 * transform3 * transform;
				for (int i = 0; (double)i < num; i++)
				{
					CSData data = cSDataArray.GetArrayItem(i).Duplicate();
					CSDataHelper.Transform(data, transform4);
					cSDataArray.Append(data);
				}
				for (int j = 0; j < flattenArray.GetArrayCount(); j++)
				{
					CSData data2 = flattenArray.GetArrayItem(j).Duplicate();
					CSDataHelper.Transform(data2, transform4);
					cSDataArray.Append(data2);
				}
			}
			if (value3)
			{
				Transform transform3 = Transform.Scale(Plane.WorldXY, 1.0, -1.0, 1.0);
				double num = cSDataArray.GetArrayCount();
				Transform transform5 = transform2 * transform3 * transform;
				for (int k = 0; (double)k < num; k++)
				{
					CSData data3 = cSDataArray.GetArrayItem(k).Duplicate();
					CSDataHelper.Transform(data3, transform5);
					cSDataArray.Append(data3);
				}
				for (int l = 0; l < flattenArray.GetArrayCount(); l++)
				{
					CSData data4 = flattenArray.GetArrayItem(l).Duplicate();
					CSDataHelper.Transform(data4, transform5);
					cSDataArray.Append(data4);
				}
			}
			if (value4)
			{
				Transform transform3 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, -1.0);
				double num = cSDataArray.GetArrayCount();
				Transform transform6 = transform2 * transform3 * transform;
				for (int m = 0; (double)m < num; m++)
				{
					CSData data5 = cSDataArray.GetArrayItem(m).Duplicate();
					CSDataHelper.Transform(data5, transform6);
					cSDataArray.Append(data5);
				}
				for (int n = 0; n < flattenArray.GetArrayCount(); n++)
				{
					CSData data6 = flattenArray.GetArrayItem(n).Duplicate();
					CSDataHelper.Transform(data6, transform6);
					cSDataArray.Append(data6);
				}
			}
		}
		else
		{
			int num2 = 1;
			if (value2)
			{
				num2 = -1;
			}
			int num3 = 1;
			if (value3)
			{
				num3 = -1;
			}
			int num4 = 1;
			if (value4)
			{
				num4 = -1;
			}
			Transform transform3 = Transform.Scale(Plane.WorldXY, num2, num3, num4);
			Transform transform7 = transform2 * transform3 * transform;
			for (int num5 = 0; num5 < flattenArray.GetArrayCount(); num5++)
			{
				CSData data7 = flattenArray.GetArrayItem(num5).Duplicate();
				CSDataHelper.Transform(data7, transform7);
				cSDataArray.Append(data7);
			}
		}
		OutObj.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
