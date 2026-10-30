using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("DistanceFromCurve", "{3A4A7C76-FB41-4f96-A5BD-0F3ADBCDE4B4}")]
public sealed class CSDistanceFromCurve : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iObject;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oNegated;

	public readonly CSOutputChannel oOneMinus;

	public CSDistanceFromCurve(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		oValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oNegated = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oOneMinus = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iObject.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Object");
		}
		CSDataHelper.GetNurbsCurve(iCurve.Data, out var value);
		if (value == null)
		{
			return UpdateError("Error geting input curve");
		}
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out var value2);
			if (!value2.IsValid)
			{
				return UpdateError("Error getting transform");
			}
			Point3d point3d = default(Point3d);
			point3d.Transform(value2);
			Vector3d vector = new Vector3d(Vector3d.ZAxis);
			vector.Transform(value2);
			if (!value.ClosestPoint(point3d, out var t))
			{
				return UpdateError("Error getting closest point");
			}
			Point3d point3d2 = value.PointAt(t);
			Vector3d vector2 = point3d - point3d2;
			double num = point3d.DistanceTo(point3d2);
			if (Vector3d.Multiply(vector, vector2) < 0.0)
			{
				num *= -1.0;
			}
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
