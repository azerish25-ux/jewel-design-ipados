using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("TrimCurve", "{13B099BE-1A98-4885-AAAC-17A0B0596288}")]
public sealed class CSTrimCurve : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iStart;

	public readonly CSInputChannel iMirrorStart;

	public readonly CSInputChannel iEnd;

	public readonly CSInputChannel iFlipCurve;

	public readonly CSOutputChannel oCurves;

	public CSTrimCurve(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMirrorStart = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFlipCurve = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curves");
		}
		double value = 0.0;
		iStart.GetDouble(ref value);
		double value2 = 100.0;
		iEnd.GetDouble(ref value2);
		bool value3 = false;
		iMirrorStart.GetBoolean(ref value3);
		bool value4 = false;
		iFlipCurve.GetBoolean(ref value4);
		if (value3)
		{
			value2 = 100.0 - value;
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(i), out var value5);
			if (value5 == null)
			{
				continue;
			}
			double t = value5.Domain.Min;
			double t2 = value5.Domain.Max;
			if (!value5.NormalizedLengthParameter(value / 100.0, out t))
			{
				return UpdateError("GetNormalizedArcLengthPoint failed (Start)");
			}
			if (!value5.NormalizedLengthParameter(value2 / 100.0, out t2))
			{
				return UpdateError("GetNormalizedArcLengthPoint failed (End)");
			}
			if (t > value5.Domain.Min || t2 < value5.Domain.Max)
			{
				value5 = value5.Trim(new Interval(t, t2)).ToNurbsCurve();
				if (!value5.IsValid)
				{
					return UpdateError("Trim failed");
				}
			}
			if (value4)
			{
				value5.Reverse();
			}
			cSDataArray.Append(new CSOnGeometryData(value5));
		}
		switch (cSDataArray.GetArrayCount())
		{
		case 0:
			oCurves.SetData(null);
			break;
		case 1:
			oCurves.SetData(cSDataArray.GetArrayItem(0));
			break;
		default:
			oCurves.SetData(cSDataArray);
			break;
		}
		return base.Update();
	}
}
