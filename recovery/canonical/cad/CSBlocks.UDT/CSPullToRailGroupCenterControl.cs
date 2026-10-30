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
				if (value3.IsValid)
				{
					CSBlockUtil.CSUtilMath.GetUnscaledXform(value3, out xUp);
				}
			}
			else
			{
				CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData.Xform, out xUp);
			}
			Point3d point3d = new Point3d(Point3d.Origin);
			point3d.Transform(xUp);
			Point3d point3d2 = new Point3d(Vector3d.ZAxis);
			point3d2.Transform(xUp);
			double angle = CSBlockUtil.CSUtilMath.AngleOfPointD(point3d2.X - point3d.X, point3d2.Z - point3d.Z);
			BoundingBox boundingBox = BoundingBox.Unset;
			Transform transform = PullToRailMorph.TopCenterXformFromAngle(angle).Convert();
			for (int j = 0; j < iCSArray2.GetArrayCount(); j++)
			{
				iCSArray2.GetArrayItem(j)?.GetTightBoundingBox(ref boundingBox, doGrow: true, transform);
			}
			CSPullToRailCenterMorph spaceMorph = new CSPullToRailCenterMorph(angle, value2, value, boundingBox)
			{
				Tolerance = 0.05
			};
			CSDataArray cSDataArray2 = new CSDataArray();
			for (int k = 0; k < iCSArray2.GetArrayCount(); k++)
			{
				CSData arrayItem = iCSArray2.GetArrayItem(k);
				if (arrayItem != null)
				{
					CSData cSData = arrayItem.Duplicate();
					if (cSData.Morph(spaceMorph))
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
