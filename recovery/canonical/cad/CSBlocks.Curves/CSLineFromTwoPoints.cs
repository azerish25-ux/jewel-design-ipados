using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("LineFromTwoPoints", "{EB68DB02-F27D-4de5-A7B1-F8C6CDEA0FBD}")]
public sealed class CSLineFromTwoPoints : CSBlock
{
	public readonly CSInputChannel iPointStart;

	public readonly CSInputChannel iPointEnd;

	public readonly CSOutputChannel oLine;

	public CSLineFromTwoPoints(string identifier)
		: base(identifier)
	{
		iPointStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPointEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oLine = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (base.InputChannels.Contains(null))
		{
			return UpdateMissing("Missing inputs");
		}
		CSDataArray flattenArray = iPointStart.GetFlattenArray();
		if (flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Point Start");
		}
		Point3d value = default(Point3d);
		if (!CSDataHelper.GetPoint(flattenArray.GetArrayItem(0), out value))
		{
			return UpdateMissing("Missing Point Start");
		}
		CSDataArray flattenArray2 = iPointEnd.GetFlattenArray();
		if (flattenArray2.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Point End");
		}
		Point3d value2 = default(Point3d);
		if (!CSDataHelper.GetPoint(flattenArray2.GetArrayItem(0), out value2))
		{
			return UpdateMissing("Missing Point End");
		}
		if (value == value2)
		{
			oLine.SetData(null);
			return base.Update();
		}
		NurbsCurve geometry = new LineCurve(value, value2).ToNurbsCurve();
		oLine.SetData(new CSOnGeometryData(geometry));
		return base.Update();
	}
}
