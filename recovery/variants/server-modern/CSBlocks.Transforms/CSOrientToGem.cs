using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("OrientToGem", "{56BB1A07-57F4-45a8-869F-068EA21960B0}")]
public sealed class CSOrientToGem : CSBlock
{
	public readonly CSInputChannel InGemDestination;

	public readonly CSInputChannel InObject;

	public readonly CSInputChannel InScale;

	public readonly CSOutputChannel OutObject;

	public CSOrientToGem(string identifier)
		: base(identifier)
	{
		InGemDestination = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InScale = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InGemDestination.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		CSDataArray flattenArray2 = InObject.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Objects");
		}
		bool flag = false;
		bool value = false;
		if (InScale.GetBoolean(ref value))
		{
			flag = value;
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem.GetType() == typeof(CSData))
			{
				continue;
			}
			CSGemData cSGemData = arrayItem as CSGemData;
			CSDataHelper.GetTransformNoArray(arrayItem, out var value2);
			Transform xUp;
			if (!flag)
			{
				CSBlockUtil.CSUtilMath.GetUnscaledXform(value2, out xUp);
			}
			else
			{
				xUp = value2;
			}
			if (xUp == Transform.Unset)
			{
				continue;
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			for (int j = 0; j < flattenArray2.GetArrayCount(); j++)
			{
				CSData cSData = flattenArray2.GetArrayItem(j).Duplicate();
				if (cSData != null)
				{
					Transform val = Transform.Identity;
					if (cSData.GetNormalizedToGem() && cSGemData != null)
					{
						val = Transform.Scale(Plane.WorldXY, cSGemData.GemXMax, cSGemData.GemYMax, Math.Abs(cSGemData.GemZMin));
					}
					CSDataHelper.Transform(cSData, xUp * val);
					cSDataArray2.Append(cSData);
				}
			}
			cSDataArray.Append(cSDataArray2);
		}
		OutObject.SetData(cSDataArray);
		return base.Update();
	}
}
