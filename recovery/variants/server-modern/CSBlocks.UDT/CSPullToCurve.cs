using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.UDT;

[CSBlock("PullToCurve", "{C331B291-4D2B-4AB2-8830-CAB0020266E0}")]
public sealed class CSPullToCurve : CSBlock
{
	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InPullTop;

	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InSurface;

	public readonly CSOutputChannel OutSurface;

	public CSPullToCurve(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		CSData arrayItem = InRail.GetFlattenArray().GetArrayItem(0);
		CSDataArray flattenArray = InSurface.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Missing Surfaces");
		}
		if (flattenArray.GetArrayCount() == 0)
		{
			return UpdateMissing("Missing Surfaces");
		}
		CSDataArray flattenArray2 = InGem.GetFlattenArray();
		CSGemData cSGemData = null;
		if (flattenArray2 != null && flattenArray2.GetArrayCount() > 0)
		{
			cSGemData = flattenArray2.GetArrayItem(0) as CSGemData;
		}
		CSDataArray cSDataArray = new CSDataArray();
		Brep inBrep = new Brep();
		Curve val = (arrayItem as ICSCurve)?.GetCurve();
		if (val != null)
		{
			Curve val2 = val.DuplicateCurve();
			Curve val3 = val.DuplicateCurve();
			((GeometryBase)val2).Translate(new Vector3d(0.0, 1.0, 0.0) * -50.0);
			((GeometryBase)val3).Translate(new Vector3d(0.0, 1.0, 0.0) * 50.0);
			inBrep = ((Surface)NurbsSurface.CreateRuledSurface(val2, val3)).ToBrep();
		}
		Transform val4 = default(Transform);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem2 = flattenArray.GetArrayItem(i);
			if (arrayItem2 == null)
			{
				continue;
			}
			Transform value;
			if (cSGemData != null)
			{
				value = cSGemData.Xform;
			}
			else
			{
				CSDataHelper.GetTransform(arrayItem2, out value);
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXform(value, out var xUp);
			if (((Transform)(ref xUp)).TryGetInverse(ref val4))
			{
				BoundingBox boundingBox = default(BoundingBox);
				arrayItem2.GetTightBoundingBox(ref boundingBox, doGrow: true, val4);
				CSPullToBaseBrep obj = new CSPullToBaseBrep(boundingBox, inBrep, xUp, val4)
				{
					Count = 0
				};
				((SpaceMorph)obj).Tolerance = 0.05;
				((SpaceMorph)obj).PreserveStructure = false;
				((SpaceMorph)obj).QuickPreview = true;
				CSPullToBaseBrep spaceMorph = obj;
				CSData cSData = arrayItem2.Duplicate();
				if (cSData.Morph((SpaceMorph)(object)spaceMorph))
				{
					cSDataArray.Append(cSData);
				}
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			OutSurface.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			OutSurface.SetData(cSDataArray);
		}
		return base.Update();
	}
}
