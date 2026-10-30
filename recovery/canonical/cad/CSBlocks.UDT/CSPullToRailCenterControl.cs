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
				if (cSGemData.Xform.IsValid)
				{
					xOriginal = cSGemData.Xform;
				}
				else
				{
					CSDataHelper.GetTransformNoArray(arrayItem, out var value3);
					if (value3.IsValid)
					{
						xOriginal = value3;
					}
				}
				if (xOriginal.IsValid)
				{
					CSBlockUtil.CSUtilMath.GetUnscaledXform(xOriginal, out xUp);
				}
			}
			Point3d point3d = new Point3d(Point3d.Origin);
			point3d.Transform(xUp);
			Point3d point3d2 = new Point3d(Vector3d.ZAxis);
			point3d2.Transform(xUp);
			double angle = CSBlockUtil.CSUtilMath.AngleOfPointD(point3d2.X - point3d.X, point3d2.Z - point3d.Z);
			BoundingBox boundingBox = BoundingBox.Unset;
			if (arrayItem.GetTightBoundingBox(ref boundingBox, doGrow: true, PullToRailMorph.TopCenterXformFromAngle(angle).Convert()))
			{
				CSPullToRailCenterMorph spaceMorph = new CSPullToRailCenterMorph(angle, value2, value, boundingBox)
				{
					Tolerance = 0.05
				};
				CSData cSData = arrayItem.Duplicate();
				if (cSData.Morph(spaceMorph))
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
