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

[CSBlock("PullToRailGroup", "{3604A4A1-1926-4b83-BDCA-679D9E67A77B}")]
public sealed class CSPullToRailGroup : CSBlock
{
	public readonly CSInputChannel iRailRadius;

	public readonly CSInputChannel iSurfaceGroup;

	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iPullTop;

	public readonly CSOutputChannel oSurfaceGroup;

	public CSPullToRailGroup(string identifier)
		: base(identifier)
	{
		iRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSurfaceGroup = new CSInputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPullTop = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oSurfaceGroup = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		bool value = false;
		iPullTop.GetBoolean(ref value);
		double value2 = 10.0;
		iRailRadius.GetDouble(ref value2);
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (!(iSurfaceGroup.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Missing Surfaces");
		}
		if (iCSArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surfaces");
		}
		CSDataArray cSDataArray = new CSDataArray();
		Point3d val = default(Point3d);
		for (int i = 0; i < iCSArray.GetArrayCount(); i++)
		{
			if (!(iCSArray.GetArrayItem(i) is ICSArray iCSArray2) || iCSArray2.GetArrayCount() <= 0)
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
			Point3d origin = Point3d.Origin;
			((Point3d)(ref origin)).Transform(xUp);
			((Point3d)(ref val))._002Ector(Vector3d.ZAxis);
			((Point3d)(ref val)).Transform(xUp);
			double num = CSBlockUtil.CSUtilMath.AngleOfPointD(((Point3d)(ref val)).X - ((Point3d)(ref origin)).X, ((Point3d)(ref val)).Z - ((Point3d)(ref origin)).Z);
			BoundingBox boundingBox = BoundingBox.Unset;
			Transform transform = PullToRailMorph.TopCenterXformFromAngle(num).Convert();
			for (int j = 0; j < iCSArray2.GetArrayCount(); j++)
			{
				iCSArray2.GetArrayItem(j)?.GetTightBoundingBox(ref boundingBox, doGrow: true, transform);
			}
			PullToRailMorph pullToRailMorph = new PullToRailMorph(num, value, value2, boundingBox.Convert());
			((SpaceMorph)pullToRailMorph).Tolerance = 0.05;
			PullToRailMorph spaceMorph = pullToRailMorph;
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
