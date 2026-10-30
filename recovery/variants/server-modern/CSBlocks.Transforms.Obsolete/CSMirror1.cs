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
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
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
		Transform val = Transform.Identity;
		Transform val2 = Transform.Identity;
		Transform val3 = default(Transform);
		if (((Transform)(ref value5)).IsValid && ((Transform)(ref value5)).TryGetInverse(ref val3))
		{
			val = val3;
			val2 = value5;
		}
		if (value)
		{
			if (value2)
			{
				Transform val4 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
				double num = cSDataArray.GetArrayCount();
				Transform transform = val2 * val4 * val;
				for (int i = 0; (double)i < num; i++)
				{
					CSData data = cSDataArray.GetArrayItem(i).Duplicate();
					CSDataHelper.Transform(data, transform);
					cSDataArray.Append(data);
				}
				for (int j = 0; j < flattenArray.GetArrayCount(); j++)
				{
					CSData data2 = flattenArray.GetArrayItem(j).Duplicate();
					CSDataHelper.Transform(data2, transform);
					cSDataArray.Append(data2);
				}
			}
			if (value3)
			{
				Transform val4 = Transform.Scale(Plane.WorldXY, 1.0, -1.0, 1.0);
				double num = cSDataArray.GetArrayCount();
				Transform transform2 = val2 * val4 * val;
				for (int k = 0; (double)k < num; k++)
				{
					CSData data3 = cSDataArray.GetArrayItem(k).Duplicate();
					CSDataHelper.Transform(data3, transform2);
					cSDataArray.Append(data3);
				}
				for (int l = 0; l < flattenArray.GetArrayCount(); l++)
				{
					CSData data4 = flattenArray.GetArrayItem(l).Duplicate();
					CSDataHelper.Transform(data4, transform2);
					cSDataArray.Append(data4);
				}
			}
			if (value4)
			{
				Transform val4 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, -1.0);
				double num = cSDataArray.GetArrayCount();
				Transform transform3 = val2 * val4 * val;
				for (int m = 0; (double)m < num; m++)
				{
					CSData data5 = cSDataArray.GetArrayItem(m).Duplicate();
					CSDataHelper.Transform(data5, transform3);
					cSDataArray.Append(data5);
				}
				for (int n = 0; n < flattenArray.GetArrayCount(); n++)
				{
					CSData data6 = flattenArray.GetArrayItem(n).Duplicate();
					CSDataHelper.Transform(data6, transform3);
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
			Transform val4 = Transform.Scale(Plane.WorldXY, (double)num2, (double)num3, (double)num4);
			Transform transform4 = val2 * val4 * val;
			for (int num5 = 0; num5 < flattenArray.GetArrayCount(); num5++)
			{
				CSData data7 = flattenArray.GetArrayItem(num5).Duplicate();
				CSDataHelper.Transform(data7, transform4);
				cSDataArray.Append(data7);
			}
		}
		OutObj.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
