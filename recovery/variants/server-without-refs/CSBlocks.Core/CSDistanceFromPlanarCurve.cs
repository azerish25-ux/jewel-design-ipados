using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("DistanceFromPlanarCurve", "{799049B6-C679-43bb-9076-0A347E3CD91F}")]
public sealed class CSDistanceFromPlanarCurve : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iObject;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oNegated;

	public readonly CSOutputChannel oOneMinus;

	public CSDistanceFromPlanarCurve(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		oValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oNegated = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oOneMinus = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iObject.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Object");
		}
		CSDataHelper.GetNurbsCurve(iCurve.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Curve");
		}
		if (!((Curve)value).IsPlanar())
		{
			return UpdateError("Is not planar");
		}
		Point3d pointAtStart = ((Curve)value).PointAtStart;
		NurbsCurve obj = value;
		Interval domain = ((Curve)value).Domain;
		Plane val = default(Plane);
		((Plane)(ref val))._002Ector(pointAtStart, ((Curve)obj).PointAt(((Interval)(ref domain)).Mid), ((Curve)value).PointAtEnd);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		Vector3d val3 = default(Vector3d);
		double num = default(double);
		double num2 = default(double);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out var value2);
			if (!((Transform)(ref value2)).IsValid)
			{
				return UpdateError("Error getting Transform");
			}
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2)).Transform(value2);
			((Vector3d)(ref val3))._002Ector(Vector3d.ZAxis);
			((Vector3d)(ref val3)).Transform(value2);
			if (!((Plane)(ref val)).ClosestParameter(val2, ref num, ref num2))
			{
				return UpdateError("Error getting closest point");
			}
			Point3d val4 = ((Plane)(ref val)).PointAt(num, num2);
			Vector3d val5 = val2 - val4;
			double num3 = ((Point3d)(ref val2)).DistanceTo(val4);
			if (Vector3d.Multiply(val3, val5) < 0.0)
			{
				num3 *= -1.0;
			}
			cSDataArray.Append(new CSNumberData(num3));
			cSDataArray2.Append(new CSNumberData(0.0 - num3));
			cSDataArray3.Append(new CSNumberData(1.0 - num3));
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
