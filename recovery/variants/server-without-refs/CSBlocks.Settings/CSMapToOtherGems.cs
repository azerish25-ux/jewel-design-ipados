using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings;

[CSBlock("MapToOtherGems", "{8CA69829-4713-41db-B2F8-A7AB7D082D8D}")]
public sealed class CSMapToOtherGems : CSBlock
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InObjects;

	public readonly CSInputChannel InEnabled;

	public readonly CSOutputChannel OutObjects;

	public CSMapToOtherGems(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		OutObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		List<CSGemData> gemList = InGems.GemList;
		CSDataArray flattenArray = InObjects.GetFlattenArray();
		if (!InEnabled.Boolean)
		{
			OutObjects.SetData(InObjects.Data.Duplicate());
			return base.Update();
		}
		CSDataArray cSDataArray = new CSDataArray();
		CSGemData cSGemData = gemList[0];
		Transform xUp = Transform.Identity;
		Transform xDown = Transform.Identity;
		cSGemData?.GetGemXformRC(out xDown, out xUp);
		for (int i = 0; i < gemList.Count; i++)
		{
			CSData data = flattenArray.Duplicate();
			CSGemData cSGemData2 = gemList[i];
			Transform xUp2 = Transform.Identity;
			Transform xDown2 = Transform.Identity;
			cSGemData2?.GetGemXformRC(out xDown2, out xUp2);
			CSDataHelper.Transform(data, xUp2 * xDown);
			cSDataArray.Append(data);
		}
		OutObjects.SetData(cSDataArray);
		return base.Update();
	}
}
