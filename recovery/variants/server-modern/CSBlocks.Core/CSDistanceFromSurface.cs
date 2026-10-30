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
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
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
		Vector3d val2 = default(Vector3d);
		Curve[] array = default(Curve[]);
		Point3d[] array2 = default(Point3d[]);
		double num2 = default(double);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out var value2);
			if (!((Transform)(ref value2)).IsValid)
			{
				return UpdateError("Error getting transform");
			}
			Point3d val = default(Point3d);
			((Point3d)(ref val)).Transform(value2);
			((Vector3d)(ref val2))._002Ector(Vector3d.ZAxis);
			((Vector3d)(ref val2)).Transform(value2);
			((Vector3d)(ref val2)).Unitize();
			Point3d val3 = val + val2 * 100.0;
			Point3d val4 = val - val2 * 100.0;
			if (!Intersection.CurveBrep((Curve)new LineCurve(val3, val4), value, CSApplicationHelper.Application.Tolerance, ref array, ref array2))
			{
				return UpdateError("No intersection");
			}
			if (array2.Length == 0 && array.Length == 0)
			{
				return UpdateError("No intersection");
			}
			Point3d val5 = Point3d.Unset;
			double num = double.NaN;
			Curve[] array3 = array;
			foreach (Curve obj in array3)
			{
				obj.ClosestPoint(val, ref num2);
				Point3d val6 = obj.PointAt(num2);
				num2 = ((Point3d)(ref val6)).DistanceTo(val);
				if (double.IsNaN(num) || num > num2)
				{
					val5 = val6;
					num = num2;
				}
			}
			for (int k = 0; k < array2.Length; k++)
			{
				Point3d val7 = array2[k];
				double num3 = ((Point3d)(ref val7)).DistanceTo(val);
				if (double.IsNaN(num) || num > num3)
				{
					val5 = val7;
					num = num3;
				}
			}
			if (Vector3d.Multiply(val2, val - val5) < 0.0)
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
