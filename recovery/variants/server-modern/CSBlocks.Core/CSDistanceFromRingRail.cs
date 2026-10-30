using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("DistanceFromRingRail", "{EF924650-5BE9-4417-AE59-664B69DD76AD}")]
public sealed class CSDistanceFromRingRail : CSBlock
{
	public readonly CSInputChannel iRadius;

	public readonly CSInputChannel iObject;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oNegated;

	public readonly CSOutputChannel oOneMinus;

	public CSDistanceFromRingRail(string identifier)
		: base(identifier)
	{
		iRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		oValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oNegated = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oOneMinus = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iObject.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Object");
		}
		double value = 0.0;
		iRadius.GetDouble(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		Point3d val = default(Point3d);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out var value2);
			if (!((Transform)(ref value2)).IsValid)
			{
				return UpdateError("Error get transform");
			}
			((Point3d)(ref val))._002Ector(0.0, 0.0, 0.0);
			((Point3d)(ref val)).Transform(value2);
			((Point3d)(ref val)).Y = 0.0;
			double num = ((Point3d)(ref val)).DistanceTo(Point3d.Origin) - value;
			cSDataArray.Append(new CSNumberData(num));
			cSDataArray2.Append(new CSNumberData(0.0 - num));
			cSDataArray3.Append(new CSNumberData(1.0 - num));
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oValue.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oValue.SetData(cSDataArray);
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oNegated.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			oNegated.SetData(cSDataArray2);
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oOneMinus.SetData(cSDataArray3.GetArrayItem(0));
		}
		else
		{
			oOneMinus.SetData(cSDataArray3);
		}
		return base.Update();
	}
}
