using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Runtime;

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
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
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
			Interval domain = ((Curve)value5).Domain;
			double min = ((Interval)(ref domain)).Min;
			domain = ((Curve)value5).Domain;
			double max = ((Interval)(ref domain)).Max;
			if (!((Curve)value5).NormalizedLengthParameter(value / 100.0, ref min))
			{
				return UpdateError("GetNormalizedArcLengthPoint failed (Start)");
			}
			if (!((Curve)value5).NormalizedLengthParameter(value2 / 100.0, ref max))
			{
				return UpdateError("GetNormalizedArcLengthPoint failed (End)");
			}
			double num = min;
			domain = ((Curve)value5).Domain;
			if (!(num > ((Interval)(ref domain)).Min))
			{
				double num2 = max;
				domain = ((Curve)value5).Domain;
				if (!(num2 < ((Interval)(ref domain)).Max))
				{
					goto IL_0163;
				}
			}
			value5 = ((Curve)value5).Trim(new Interval(min, max)).ToNurbsCurve();
			if (!((CommonObject)value5).IsValid)
			{
				return UpdateError("Trim failed");
			}
			goto IL_0163;
			IL_0163:
			if (value4)
			{
				((Curve)value5).Reverse();
			}
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)value5));
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
