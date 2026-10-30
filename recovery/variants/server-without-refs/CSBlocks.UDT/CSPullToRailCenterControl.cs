using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.UDT;

[CSBlock("PullToRailCenterControl", "{74239759-7FBF-4E00-9355-8819BAA66EA1}")]
public sealed class CSPullToRailCenterControl : CSBlock
{
	public readonly CSInputChannel iRailRadius;

	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iPullAmount;

	public readonly CSOutputChannel oSurface;

	public CSPullToRailCenterControl(string identifier)
		: base(identifier)
	{
		iRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPullAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
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
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Missing Surfaces");
		}
		if (flattenArray.GetArrayCount() == 0)
		{
			return UpdateMissing("Missing Surfaces");
		}
		CSDataArray flattenArray2 = iGem.GetFlattenArray();
		CSGemData cSGemData = null;
		if (flattenArray2 != null && flattenArray2.GetArrayCount() > 0)
		{
			cSGemData = flattenArray2.GetArrayItem(0) as CSGemData;
		}
		CSDataArray cSDataArray = new CSDataArray();
		Point3d val = default(Point3d);
		Point3d val2 = default(Point3d);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			Transform xUp = Transform.Identity;
			if (cSGemData != null)
			{
				Transform xOriginal = Transform.Unset;
				Transform xform = cSGemData.Xform;
				if (((Transform)(ref xform)).IsValid)
				{
					xOriginal = cSGemData.Xform;
				}
				else
				{
					CSDataHelper.GetTransformNoArray(arrayItem, out var value3);
					if (((Transform)(ref value3)).IsValid)
					{
						xOriginal = value3;
					}
				}
				if (((Transform)(ref xOriginal)).IsValid)
				{
					CSBlockUtil.CSUtilMath.GetUnscaledXform(xOriginal, out xUp);
				}
			}
			((Point3d)(ref val))._002Ector(Point3d.Origin);
			((Point3d)(ref val)).Transform(xUp);
			((Point3d)(ref val2))._002Ector(Vector3d.ZAxis);
			((Point3d)(ref val2)).Transform(xUp);
			double angle = CSBlockUtil.CSUtilMath.AngleOfPointD(((Point3d)(ref val2)).X - ((Point3d)(ref val)).X, ((Point3d)(ref val2)).Z - ((Point3d)(ref val)).Z);
			BoundingBox boundingBox = BoundingBox.Unset;
			if (arrayItem.GetTightBoundingBox(ref boundingBox, doGrow: true, PullToRailMorph.TopCenterXformFromAngle(angle).Convert()))
			{
				CSPullToRailCenterMorph cSPullToRailCenterMorph = new CSPullToRailCenterMorph(angle, value2, value, boundingBox);
				((SpaceMorph)cSPullToRailCenterMorph).Tolerance = 0.05;
				CSPullToRailCenterMorph spaceMorph = cSPullToRailCenterMorph;
				CSData cSData = arrayItem.Duplicate();
				if (cSData.Morph((SpaceMorph)(object)spaceMorph))
				{
					cSDataArray.Append(cSData);
				}
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oSurface.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oSurface.SetData(cSDataArray);
		}
		return base.Update();
	}
}
