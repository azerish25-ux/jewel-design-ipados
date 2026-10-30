using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemInfo_V1", "{1E8CCAF3-8F1D-4c81-B95D-F2CBD5BC0723}")]
public sealed class CSGemInfo1 : CSBlock
{
	public readonly CSInputChannel InGem;

	public readonly CSOutputChannel OutMaxSizeX;

	public readonly CSOutputChannel OutMinSizeX;

	public readonly CSOutputChannel OutSizeX;

	public readonly CSOutputChannel OutMaxSizeY;

	public readonly CSOutputChannel OutMinSizeY;

	public readonly CSOutputChannel OutSizeY;

	public readonly CSOutputChannel OutMaxSizeZ;

	public readonly CSOutputChannel OutMinSizeZ;

	public readonly CSOutputChannel OutSizeZ;

	public readonly CSOutputChannel OutXDownUnscaled;

	public readonly CSOutputChannel OutXUpUnscaled;

	public readonly CSOutputChannel OutType;

	public readonly CSOutputChannel OutShape;

	public CSGemInfo1(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutMaxSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutMinSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutMaxSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutMinSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutMaxSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutMinSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutXDownUnscaled = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Array);
		OutXUpUnscaled = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Array);
		OutType = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Array);
		OutShape = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Gem");
		}
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		CSDataArray cSDataArray5 = new CSDataArray();
		CSDataArray cSDataArray6 = new CSDataArray();
		CSDataArray cSDataArray7 = new CSDataArray();
		CSDataArray cSDataArray8 = new CSDataArray();
		CSDataArray cSDataArray9 = new CSDataArray();
		CSDataArray cSDataArray10 = new CSDataArray();
		CSDataArray cSDataArray11 = new CSDataArray();
		CSDataArray cSDataArray12 = new CSDataArray();
		CSDataArray cSDataArray13 = new CSDataArray();
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData = gemList[i];
			if (cSGemData != null)
			{
				cSDataArray.Append(new CSNumberData(cSGemData.GemXMax));
				cSDataArray2.Append(new CSNumberData(cSGemData.GemXMin));
				cSDataArray3.Append(new CSNumberData(cSGemData.GemXSize));
				cSDataArray4.Append(new CSNumberData(cSGemData.GemYMax));
				cSDataArray5.Append(new CSNumberData(cSGemData.GemYMin));
				cSDataArray6.Append(new CSNumberData(cSGemData.GemYSize));
				cSDataArray7.Append(new CSNumberData(cSGemData.GemZMax));
				cSDataArray8.Append(new CSNumberData(cSGemData.GemZMin));
				cSDataArray9.Append(new CSNumberData(cSGemData.GemZSize));
				CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var xDown);
				cSDataArray10.Append(new CSXformData(xDown));
				cSDataArray11.Append(new CSXformData(xUp));
				cSDataArray12.Append(new CSStringData(cSGemData.GemType));
				cSDataArray13.Append(new CSStringData(cSGemData.GemShape));
			}
		}
		OutMaxSizeX.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		OutMinSizeX.SetData((cSDataArray2.GetArrayCount() == 1) ? cSDataArray2.GetArrayItem(0) : cSDataArray2);
		OutSizeX.SetData((cSDataArray3.GetArrayCount() == 1) ? cSDataArray3.GetArrayItem(0) : cSDataArray3);
		OutMaxSizeY.SetData((cSDataArray4.GetArrayCount() == 1) ? cSDataArray4.GetArrayItem(0) : cSDataArray4);
		OutMinSizeY.SetData((cSDataArray5.GetArrayCount() == 1) ? cSDataArray5.GetArrayItem(0) : cSDataArray5);
		OutSizeY.SetData((cSDataArray6.GetArrayCount() == 1) ? cSDataArray6.GetArrayItem(0) : cSDataArray6);
		OutMaxSizeZ.SetData((cSDataArray7.GetArrayCount() == 1) ? cSDataArray7.GetArrayItem(0) : cSDataArray7);
		OutMinSizeZ.SetData((cSDataArray8.GetArrayCount() == 1) ? cSDataArray8.GetArrayItem(0) : cSDataArray8);
		OutSizeZ.SetData((cSDataArray9.GetArrayCount() == 1) ? cSDataArray9.GetArrayItem(0) : cSDataArray9);
		OutXDownUnscaled.SetData((cSDataArray10.GetArrayCount() == 1) ? cSDataArray10.GetArrayItem(0) : cSDataArray10);
		OutXUpUnscaled.SetData((cSDataArray11.GetArrayCount() == 1) ? cSDataArray11.GetArrayItem(0) : cSDataArray11);
		OutType.SetData((cSDataArray11.GetArrayCount() == 1) ? cSDataArray12.GetArrayItem(0) : cSDataArray12);
		OutShape.SetData((cSDataArray11.GetArrayCount() == 1) ? cSDataArray13.GetArrayItem(0) : cSDataArray13);
		return base.Update();
	}
}
