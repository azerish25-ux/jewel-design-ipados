using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Tools;

[CSBlock("PointOnCurve", "{2172ACF5-2C47-48a2-B3D6-383155CBC117}")]
public sealed class CSPointOnCurve : CSBlock
{
	public CSInputChannel iCurve;

	public CSInputChannel iPosition;

	public CSInputChannel iUseMMValue;

	public CSOutputChannel oParameter;

	public CSOutputChannel oPoint;

	public CSPointOnCurve(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iUseMMValue = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oParameter = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		oPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curve");
		}
		double value = 0.5;
		iPosition.GetDouble(ref value);
		bool value2 = false;
		iUseMMValue.GetBoolean(ref value2);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			CSDataHelper.GetNurbsCurve(arrayItem, out var value3);
			if (value3 != null)
			{
				double num = value;
				if (value2)
				{
					double length = value3.GetLength();
					num = value / length;
				}
				double t;
				if (num <= 0.0)
				{
					t = value3.Domain.Min;
				}
				else if (num >= 1.0)
				{
					t = value3.Domain.Max;
				}
				else if (!value3.NormalizedLengthParameter(num, out t))
				{
					continue;
				}
				if (!double.IsNaN(t))
				{
					CSNumberData data = new CSNumberData(t);
					cSDataArray2.Append(data);
					CSPointData data2 = new CSPointData(value3.PointAt(t));
					cSDataArray.Append(data2);
				}
			}
		}
		if (cSDataArray2.GetArrayCount() == 0)
		{
			oParameter.SetData(null);
		}
		else if (cSDataArray2.GetArrayCount() == 1)
		{
			oParameter.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			oParameter.SetData(cSDataArray2);
		}
		if (cSDataArray.GetArrayCount() == 0)
		{
			oPoint.SetData(null);
		}
		else if (cSDataArray.GetArrayCount() == 1)
		{
			oPoint.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oPoint.SetData(cSDataArray);
		}
		return base.Update();
	}
}
