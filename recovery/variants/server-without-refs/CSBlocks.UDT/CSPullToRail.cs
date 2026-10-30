using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.UDT;

[CSBlock("PullToRail", "{AB0B1915-63D5-447e-AA30-AB28E9422C1E}")]
public sealed class CSPullToRail : CSBlock
{
	public readonly CSInputChannel iRailRadius;

	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iPullTop;

	public readonly CSOutputChannel oSurface;

	public CSPullToRail(string identifier)
		: base(identifier)
	{
		iRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPullTop = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		bool value = false;
		iPullTop.GetBoolean(ref value);
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
			Transform xUp = Transform.Unset;
			Transform value3;
			if (cSGemData != null)
			{
				cSGemData.GetTransform(out value3);
			}
			else
			{
				CSDataHelper.GetTransformNoArray(arrayItem, out value3);
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXform(value3, out xUp);
			((Point3d)(ref val))._002Ector(Point3d.Origin);
			((Point3d)(ref val)).Transform(xUp);
			((Point3d)(ref val2))._002Ector(Vector3d.ZAxis);
			((Point3d)(ref val2)).Transform(xUp);
			double num = CSBlockUtil.CSUtilMath.AngleOfPointD(((Point3d)(ref val2)).X - ((Point3d)(ref val)).X, ((Point3d)(ref val2)).Z - ((Point3d)(ref val)).Z);
			BoundingBox boundingBox = BoundingBox.Unset;
			if (arrayItem.GetTightBoundingBox(ref boundingBox, doGrow: true, PullToRailMorph.TopCenterXformFromAngle(num).Convert()))
			{
				PullToRailMorph pullToRailMorph = new PullToRailMorph(num, value, value2, boundingBox.Convert());
				((SpaceMorph)pullToRailMorph).Tolerance = 0.05;
				PullToRailMorph spaceMorph = pullToRailMorph;
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
