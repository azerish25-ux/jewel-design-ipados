using System.Collections.Generic;
using System.Linq;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Tools;

[CSBlock("PointOnSurface", "{2EA36412-BF47-40e4-BCD1-708323973258}")]
public sealed class CSPointOnSurface : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iPositionU;

	public readonly CSInputChannel iPositionV;

	public readonly CSOutputChannel oParameterU;

	public readonly CSOutputChannel oParameterV;

	public readonly CSOutputChannel oPoint;

	public CSPointOnSurface(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPositionU = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPositionV = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParameterU = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		oParameterV = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		oPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface");
		}
		double value = 0.5;
		iPositionU.GetDouble(ref value);
		double value2 = 0.5;
		iPositionV.GetDouble(ref value2);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			CSDataHelper.GetBrep(arrayItem, out var value3);
			if (value3 != null && value3.Faces.Count > 0)
			{
				Surface val = (Surface)(object)((IEnumerable<BrepFace>)value3.Faces).First();
				if (val != null)
				{
					Interval val2 = val.Domain(0);
					double num = ((value <= 0.0) ? ((Interval)(ref val2)).Min : ((!(value >= 1.0)) ? ((Interval)(ref val2)).ParameterAt(value) : ((Interval)(ref val2)).Max));
					Interval val3 = val.Domain(1);
					double num2 = ((value2 <= 0.0) ? ((Interval)(ref val3)).Min : ((!(value2 >= 1.0)) ? ((Interval)(ref val3)).ParameterAt(value2) : ((Interval)(ref val3)).Max));
					CSNumberData data = new CSNumberData(num);
					cSDataArray2.Append(data);
					CSNumberData data2 = new CSNumberData(num2);
					cSDataArray3.Append(data2);
					CSPointData data3 = new CSPointData(val.PointAt(num, num2));
					cSDataArray.Append(data3);
				}
			}
		}
		if (cSDataArray2.GetArrayCount() == 0)
		{
			oParameterU.SetData(null);
		}
		else if (cSDataArray2.GetArrayCount() == 1)
		{
			oParameterU.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			oParameterU.SetData(cSDataArray2);
		}
		if (cSDataArray3.GetArrayCount() == 0)
		{
			oParameterV.SetData(null);
		}
		else if (cSDataArray3.GetArrayCount() == 1)
		{
			oParameterV.SetData(cSDataArray3.GetArrayItem(0));
		}
		else
		{
			oParameterV.SetData(cSDataArray3);
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
		oParameterU.SetData(cSDataArray2);
		oParameterV.SetData(cSDataArray3);
		oPoint.SetData(cSDataArray);
		return base.Update();
	}
}
