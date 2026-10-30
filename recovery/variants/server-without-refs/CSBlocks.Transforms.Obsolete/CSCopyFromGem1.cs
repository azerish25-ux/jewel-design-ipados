using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms.Obsolete;

[Obsolete]
[CSBlock("CopyFromGem_V1", "{EDF51D8D-BA7D-463d-A94F-401FE550135B}")]
public sealed class CSCopyFromGem1 : CSBlock
{
	public readonly CSInputChannel InGemSource;

	public readonly CSInputChannel InGemDestination;

	public readonly CSInputChannel InObject;

	public readonly CSInputChannel InScaleToGem;

	public readonly CSOutputChannel OutObject;

	public CSCopyFromGem1(string identifier)
		: base(identifier)
	{
		InGemSource = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemDestination = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		bool value = false;
		InScaleToGem.GetBoolean(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		if (value)
		{
			CSDataHelper.GetTransformNoArray(InGemSource.Data, out var value2);
			Transform val = default(Transform);
			((Transform)(ref value2)).TryGetInverse(ref val);
			CSDataArray flattenArray = InGemDestination.GetFlattenArray();
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out var value3);
				CSDataArray cSDataArray2 = new CSDataArray();
				CSDataArray flattenArray2 = InObject.GetFlattenArray();
				for (int j = 0; j < flattenArray2.GetArrayCount(); j++)
				{
					CSData cSData = flattenArray2.GetArrayItem(j).Duplicate();
					if (cSData != null)
					{
						CSDataHelper.Transform(cSData, value3 * val);
					}
					cSDataArray2.Append(cSData);
				}
				cSDataArray.Append(cSDataArray2);
			}
		}
		else
		{
			CSDataHelper.GetTransformNoArray(InGemSource.Data, out var value4);
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(value4, out var _, out var xDown);
			CSDataArray flattenArray3 = InGemDestination.GetFlattenArray();
			for (int k = 0; k < flattenArray3.GetArrayCount(); k++)
			{
				CSDataHelper.GetTransformNoArray(flattenArray3.GetArrayItem(k), out var value5);
				CSBlockUtil.CSUtilMath.GetUnscaledXforms(value5, out var xUp2, out var _);
				CSDataArray cSDataArray3 = new CSDataArray();
				CSDataArray flattenArray4 = InObject.GetFlattenArray();
				for (int l = 0; l < flattenArray4.GetArrayCount(); l++)
				{
					CSData cSData2 = flattenArray4.GetArrayItem(l).Duplicate();
					if (cSData2 != null)
					{
						CSDataHelper.Transform(cSData2, xUp2 * xDown);
					}
					cSDataArray3.Append(cSData2);
				}
				cSDataArray.Append(cSDataArray3);
			}
		}
		OutObject.SetData(cSDataArray);
		return base.Update();
	}
}
