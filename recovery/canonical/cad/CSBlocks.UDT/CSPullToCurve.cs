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
		Curve curve = (arrayItem as ICSCurve)?.GetCurve();
		if (curve != null)
		{
			Curve curve2 = curve.DuplicateCurve();
			Curve curve3 = curve.DuplicateCurve();
			curve2.Translate(new Vector3d(0.0, 1.0, 0.0) * -50.0);
			curve3.Translate(new Vector3d(0.0, 1.0, 0.0) * 50.0);
			inBrep = NurbsSurface.CreateRuledSurface(curve2, curve3).ToBrep();
		}
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
			if (xUp.TryGetInverse(out var inverseTransform))
			{
				BoundingBox boundingBox = default(BoundingBox);
				arrayItem2.GetTightBoundingBox(ref boundingBox, doGrow: true, inverseTransform);
				CSPullToBaseBrep spaceMorph = new CSPullToBaseBrep(boundingBox, inBrep, xUp, inverseTransform)
				{
					Count = 0,
					Tolerance = 0.05,
					PreserveStructure = false,
					QuickPreview = true
				};
				CSData cSData = arrayItem2.Duplicate();
				if (cSData.Morph(spaceMorph))
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
