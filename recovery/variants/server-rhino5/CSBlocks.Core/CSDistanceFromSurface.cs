using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Core;

[CSBlock("DistanceFromSurface", "{FA3D15FA-50AB-479D-8FC8-FBFB9CF2C6B3}")]
public sealed class CSDistanceFromSurface : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iObject;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oNegated;

	public readonly CSOutputChannel oOneMinus;

	public CSDistanceFromSurface(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
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
		CSData data = iSurface.Data;
		if (data == null)
		{
			return UpdateMissing("Surface");
		}
		if (!CSDataHelper.GetBrep(data, out var value))
		{
			return UpdateMissing("Surface");
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
			Vector3d vector3d = new Vector3d(Vector3d.ZAxis);
			vector3d.Transform(value2);
			vector3d.Unitize();
			Point3d point3d2 = point3d + vector3d * 100.0;
			Point3d to = point3d - vector3d * 100.0;
			if (!Intersection.CurveBrep(new LineCurve(point3d2, to), value, CSApplicationHelper.Application.Tolerance, out var overlapCurves, out var intersectionPoints))
			{
				return UpdateError("No intersection");
			}
			if (intersectionPoints.Length == 0 && overlapCurves.Length == 0)
			{
				return UpdateError("No intersection");
			}
			Point3d point3d3 = Point3d.Unset;
			double num = double.NaN;
			Curve[] array = overlapCurves;
			foreach (Curve obj in array)
			{
				obj.ClosestPoint(point3d, out var t);
				Point3d point3d4 = obj.PointAt(t);
				t = point3d4.DistanceTo(point3d);
				if (double.IsNaN(num) || num > t)
				{
					point3d3 = point3d4;
					num = t;
				}
			}
			for (int k = 0; k < intersectionPoints.Length; k++)
			{
				Point3d point3d5 = intersectionPoints[k];
				double num2 = point3d5.DistanceTo(point3d);
				if (double.IsNaN(num) || num > num2)
				{
					point3d3 = point3d5;
					num = num2;
				}
			}
			if (Vector3d.Multiply(vector3d, point3d - point3d3) < 0.0)
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
