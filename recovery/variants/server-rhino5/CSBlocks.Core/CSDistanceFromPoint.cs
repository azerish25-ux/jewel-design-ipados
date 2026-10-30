using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("DistanceFromPoint", "{6BBF3659-BBAB-4241-B86D-C90B7BF1D9AF}")]
public sealed class CSDistanceFromPoint : CSBlock
{
	public readonly CSInputChannel iPoint;

	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iDirect;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oNegated;

	public readonly CSOutputChannel oOneMinus;

	public CSDistanceFromPoint(string identifier)
		: base(identifier)
	{
		iPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		iDirect = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
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
		if (!CSDataHelper.GetPoint(iPoint, out var value))
		{
			value = Point3d.Origin;
		}
		bool value2 = true;
		iDirect.GetBoolean(ref value2);
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
			CSDataHelper.GetTransformNoArray(arrayItem, out var value3);
			if (value3 == Transform.Unset)
			{
				continue;
			}
			Point3d origin = Point3d.Origin;
			origin.Transform(value3);
			Vector3d zAxis = Vector3d.ZAxis;
			zAxis.Transform(value3);
			double num = origin.DistanceTo(value);
			if (Vector3d.Multiply(zAxis, origin - value) < 0.0)
			{
				num *= -1.0;
			}
			if (!value2)
			{
				Point3d point3d = origin + zAxis * num * num;
				Point3d to = origin - zAxis * num * num;
				LineCurve lineCurve = new LineCurve(point3d, to);
				double t = 0.0;
				if (!lineCurve.ClosestPoint(value, out t))
				{
					continue;
				}
				Point3d other = lineCurve.PointAt(t);
				num = origin.DistanceTo(other);
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
