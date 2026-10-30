using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms.Obsolete;

[Obsolete]
[CSBlock("MirrorOptions_V1", "{E8202B35-77A7-434A-846F-532C1927A94B}")]
public sealed class CSMirrorOptions1 : CSBlock
{
	public CSInputChannel InObj { get; }

	public CSInputChannel InOption { get; }

	public CSOutputChannel OutObj { get; }

	public CSMirrorOptions1(string identifier)
		: base(identifier)
	{
		InObj = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InOption = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutObj = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InObj.GetFlattenArray();
		int value = 0;
		InOption.GetInteger(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		switch (value)
		{
		case 1:
		{
			Transform transform6 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
			for (int l = 0; l < flattenArray.GetArrayCount(); l++)
			{
				CSData data4 = flattenArray.GetArrayItem(l).Duplicate();
				CSDataHelper.Transform(data4, transform6);
				cSDataArray.Append(data4);
			}
			break;
		}
		case 2:
		{
			Transform transform4 = Transform.Scale(Plane.WorldXY, -1.0, -1.0, 1.0);
			for (int j = 0; j < flattenArray.GetArrayCount(); j++)
			{
				CSData data2 = flattenArray.GetArrayItem(j).Duplicate();
				CSDataHelper.Transform(data2, transform4);
				cSDataArray.Append(data2);
			}
			break;
		}
		case 3:
		{
			Transform transform5 = Transform.Scale(Plane.WorldXY, 1.0, -1.0, 1.0);
			for (int k = 0; k < flattenArray.GetArrayCount(); k++)
			{
				CSData data3 = flattenArray.GetArrayItem(k).Duplicate();
				CSDataHelper.Transform(data3, transform5);
				cSDataArray.Append(data3);
			}
			break;
		}
		case 4:
		{
			Transform transform = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
			Transform transform2 = Transform.Scale(Plane.WorldXY, 1.0, -1.0, 1.0);
			Transform transform3 = Transform.Scale(Plane.WorldXY, -1.0, -1.0, 1.0);
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSData data = flattenArray.GetArrayItem(i).Duplicate();
				CSDataHelper.Transform(data, transform);
				cSDataArray.Append(data);
				data = flattenArray.GetArrayItem(i).Duplicate();
				CSDataHelper.Transform(data, transform2);
				cSDataArray.Append(data);
				data = flattenArray.GetArrayItem(i).Duplicate();
				CSDataHelper.Transform(data, transform3);
				cSDataArray.Append(data);
			}
			break;
		}
		}
		OutObj.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
