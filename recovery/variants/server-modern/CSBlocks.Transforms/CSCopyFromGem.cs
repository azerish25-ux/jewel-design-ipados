using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("CopyFromGem", "{A2F5E526-1E97-4AA0-B793-1DF2B7FE9B71}")]
public sealed class CSCopyFromGem : CSBlock
{
	public CSInputChannel iGemSource { get; }

	public CSInputChannel iGemDestination { get; }

	public CSInputChannel iObject { get; }

	public CSInputChannel iScaleToGem { get; }

	public CSOutputChannel oObject { get; }

	public CSCopyFromGem(string identifier)
		: base(identifier)
	{
		iGemSource = new CSInputChannel(GetPropertyName(() => iGemSource), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGemDestination = new CSInputChannel(GetPropertyName(() => iGemDestination), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iObject = new CSInputChannel(GetPropertyName(() => iObject), this, ChannelDataType.Any, ChannelArrayType.Any);
		iScaleToGem = new CSInputChannel(GetPropertyName(() => iScaleToGem), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(GetPropertyName(() => oObject), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iGemSource.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Source Object");
		}
		CSData arrayItem = flattenArray.GetArrayItem(0);
		if (arrayItem == null)
		{
			return UpdateError("Invalid gem");
		}
		CSDataHelper.GetTransformNoArray(arrayItem, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			return UpdateError("Error Source Xform");
		}
		CSDataArray flattenArray2 = iGemDestination.GetFlattenArray();
		CSDataArray flattenArray3 = iObject.GetFlattenArray();
		bool value2 = false;
		if (iScaleToGem != null)
		{
			iScaleToGem.GetBoolean(ref value2);
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (value2)
		{
			ScaleToGem(flattenArray2, flattenArray3, cSDataArray, value);
		}
		else
		{
			OrientToGem(flattenArray2, flattenArray3, cSDataArray, value);
		}
		oObject.SetData(cSDataArray);
		return base.Update();
	}

	private void OrientToGem(ICSArray gemDestinations, ICSArray inputObjects, CSDataArray outputObjects, Transform source)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(source, out var _, out var xDown);
		for (int i = 0; i < gemDestinations.GetArrayCount(); i++)
		{
			CSDataHelper.GetTransformNoArray(gemDestinations.GetArrayItem(i), out var value);
			CSBlockUtil.CSUtilMath.GetUnscaledXform(value, out var xUp2);
			CSDataArray cSDataArray = new CSDataArray();
			for (int j = 0; j < inputObjects.GetArrayCount(); j++)
			{
				CSData cSData = inputObjects.GetArrayItem(j).Duplicate();
				if (cSData != null)
				{
					CSDataHelper.Transform(cSData, xUp2 * xDown);
				}
				cSDataArray.Append(cSData);
			}
			outputObjects.Append(cSDataArray);
		}
	}

	private void ScaleToGem(ICSArray gemDestinations, CSDataArray inputObjects, CSDataArray outputObjects, Transform xSourceGemUp)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		Transform val = default(Transform);
		((Transform)(ref xSourceGemUp)).TryGetInverse(ref val);
		for (int i = 0; i < gemDestinations.GetArrayCount(); i++)
		{
			CSDataHelper.GetTransformNoArray(gemDestinations.GetArrayItem(i), out var value);
			if (!((Transform)(ref value)).IsValid)
			{
				continue;
			}
			CSDataArray cSDataArray = new CSDataArray();
			for (int j = 0; j < inputObjects.GetArrayCount(); j++)
			{
				CSData cSData = inputObjects.GetArrayItem(j).Duplicate();
				if (cSData != null)
				{
					CSDataHelper.Transform(cSData, value * val);
				}
				cSDataArray.Append(cSData);
			}
			outputObjects.Append(cSDataArray);
		}
	}
}
