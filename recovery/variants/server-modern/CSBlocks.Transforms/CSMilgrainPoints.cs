using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Collections;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("MilgrainPoints", "{70FF39F6-692E-46ab-B552-B425092D7310}")]
public sealed class CSMilgrainPoints : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iSize;

	public readonly CSInputChannel iSpacing;

	public readonly CSOutputChannel oPoints;

	public CSMilgrainPoints(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oPoints = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		double value = 0.0;
		double value2 = 0.0;
		iSize.GetDouble(ref value);
		iSpacing.GetDouble(ref value2);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(i), out var value3);
			Point3dList val = GeneratePointsFromCurve((Curve)(object)value3, value, value2);
			if (val == null)
			{
				continue;
			}
			if (flattenArray.GetArrayCount() > 1)
			{
				CSDataArray cSDataArray2 = new CSDataArray();
				for (int j = 0; j < ((RhinoList<Point3d>)(object)val).Count; j++)
				{
					cSDataArray2.Append(new CSPointData(((RhinoList<Point3d>)(object)val)[j]));
				}
				cSDataArray.Append(cSDataArray2);
			}
			else
			{
				for (int k = 0; k < ((RhinoList<Point3d>)(object)val).Count; k++)
				{
					cSDataArray.Append(new CSPointData(((RhinoList<Point3d>)(object)val)[k]));
				}
			}
		}
		oPoints.SetData(cSDataArray);
		return base.Update();
	}

	private static Point3dList GeneratePointsFromCurve(Curve crv, double size, double spacing)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		if (crv == null)
		{
			return null;
		}
		if (size * 2.0 + spacing < 0.001)
		{
			return null;
		}
		double num = crv.GetLength() / (size * 2.0 + spacing);
		double[] array = new double[(int)num];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (double)i / num;
		}
		_ = new double[(int)num];
		double[] array2 = crv.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		Point3dList val = new Point3dList();
		double[] array3 = array2;
		foreach (double num2 in array3)
		{
			((RhinoList<Point3d>)(object)val).Add(crv.PointAt(num2));
		}
		return val;
	}
}
