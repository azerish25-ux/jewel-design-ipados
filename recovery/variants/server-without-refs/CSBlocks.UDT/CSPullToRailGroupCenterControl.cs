using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.UDT;

[CSBlock("PullToRailGroupCenterControl", "{82B10075-A2A1-4169-BDE6-C14B0D1DC980}")]
public sealed class CSPullToRailGroupCenterControl : CSBlock
{
	public readonly CSInputChannel iRailRadius;

	public readonly CSInputChannel iSurfaceGroup;

	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iPullAmount;

	public readonly CSOutputChannel oSurfaceGroup;

	public CSPullToRailGroupCenterControl(string identifier)
		: base(identifier)
	{
		iRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSurfaceGroup = new CSInputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		iPullAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurfaceGroup = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		double value = 0.0;
		iPullAmount.GetDouble(ref value);
		if (value < 0.0)
		{
			value = 0.0;
		}
		if (value > 1.0)
		{
			value = 1.0;
		}
		double value2 = 10.0;
		iRailRadius.GetDouble(ref value2);
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (!(iSurfaceGroup.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Missing Surfaces");
		}
		if (iCSArray.GetArrayCount() == 0)
		{
			return UpdateMissing("Missing Surfaces");
		}
		CSDataArray cSDataArray = new CSDataArray();
		Point3d val = default(Point3d);
		Point3d val2 = default(Point3d);
		for (int i = 0; i < iCSArray.GetArrayCount(); i++)
		{
			if (!(iCSArray.GetArrayItem(i) is ICSArray iCSArray2) || iCSArray2.GetArrayCount() == 0)
			{
				continue;
			}
			CSGemData cSGemData = null;
			if (flattenArray != null && flattenArray.GetArrayCount() > 0)
			{
				cSGemData = flattenArray.GetArrayItem(i) as CSGemData;
			}
			Transform xUp = Transform.Identity;
			if (cSGemData == null)
			{
				CSDataHelper.GetTransformNoArray(iCSArray2.GetArrayItem(0), out var value3);
				if (((Transform)(ref value3)).IsValid)
				{
					CSBlockUtil.CSUtilMath.GetUnscaledXform(value3, out xUp);
				}
			}
			else
			{
				CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData.Xform, out xUp);
			}
			((Point3d)(ref val))._002Ector(Point3d.Origin);
			((Point3d)(ref val)).Transform(xUp);
			((Point3d)(ref val2))._002Ector(Vector3d.ZAxis);
			((Point3d)(ref val2)).Transform(xUp);
			double angle = CSBlockUtil.CSUtilMath.AngleOfPointD(((Point3d)(ref val2)).X - ((Point3d)(ref val)).X, ((Point3d)(ref val2)).Z - ((Point3d)(ref val)).Z);
			BoundingBox boundingBox = BoundingBox.Unset;
			Transform transform = PullToRailMorph.TopCenterXformFromAngle(angle).Convert();
			for (int j = 0; j < iCSArray2.GetArrayCount(); j++)
			{
				iCSArray2.GetArrayItem(j)?.GetTightBoundingBox(ref boundingBox, doGrow: true, transform);
			}
			CSPullToRailCenterMorph cSPullToRailCenterMorph = new CSPullToRailCenterMorph(angle, value2, value, boundingBox);
			((SpaceMorph)cSPullToRailCenterMorph).Tolerance = 0.05;
			CSPullToRailCenterMorph spaceMorph = cSPullToRailCenterMorph;
			CSDataArray cSDataArray2 = new CSDataArray();
			for (int k = 0; k < iCSArray2.GetArrayCount(); k++)
			{
				CSData arrayItem = iCSArray2.GetArrayItem(k);
				if (arrayItem != null)
				{
					CSData cSData = arrayItem.Duplicate();
					if (cSData.Morph((SpaceMorph)(object)spaceMorph))
					{
						cSDataArray2.Append(cSData);
					}
				}
			}
			cSDataArray.Append(cSDataArray2);
		}
		oSurfaceGroup.SetData(cSDataArray);
		return base.Update();
	}
}
