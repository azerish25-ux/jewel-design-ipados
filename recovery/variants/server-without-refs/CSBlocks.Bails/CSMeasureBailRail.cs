using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Bails;

[CSBlock("MeasureBailRail", "{2F4201B5-F6F2-4944-AD4C-12CBBFA43AA5}")]
public sealed class CSMeasureBailRail : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSOutputChannel OutBailOpeningInsideHeight;

	public readonly CSOutputChannel OutBailOpeningInsideDepth;

	public CSMeasureBailRail(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutBailOpeningInsideHeight = new CSOutputChannel("BailOpeningInsideHeight", this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutBailOpeningInsideDepth = new CSOutputChannel("BailOpeningInsideDepth", this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = ((GeometryBase)InRail.NurbsCurve).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			return UpdateError("Error Get Tight Bounding Box from Output Rail");
		}
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double number = y - ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double z = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double number2 = z - ((Point3d)(ref val)).Z;
		OutBailOpeningInsideHeight.SetData(new CSNumberData(number));
		OutBailOpeningInsideDepth.SetData(new CSNumberData(number2));
		return base.Update();
	}
}
