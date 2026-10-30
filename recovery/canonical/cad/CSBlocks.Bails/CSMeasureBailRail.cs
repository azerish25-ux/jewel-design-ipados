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
		BoundingBox boundingBox = InRail.NurbsCurve.GetBoundingBox(accurate: true);
		if (!boundingBox.IsValid)
		{
			return UpdateError("Error Get Tight Bounding Box from Output Rail");
		}
		double number = boundingBox.Max.Y - boundingBox.Min.Y;
		double number2 = boundingBox.Max.Z - boundingBox.Min.Z;
		OutBailOpeningInsideHeight.SetData(new CSNumberData(number));
		OutBailOpeningInsideDepth.SetData(new CSNumberData(number2));
		return base.Update();
	}
}
