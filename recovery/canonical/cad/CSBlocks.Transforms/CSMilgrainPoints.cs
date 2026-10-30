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
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		double value = 0.0;
		double value2 = 0.0;
		iSize.GetDouble(ref value);
		iSpacing.GetDouble(ref value2);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(i), out var value3);
			Point3dList point3dList = GeneratePointsFromCurve(value3, value, value2);
			if (point3dList == null)
			{
				continue;
			}
			if (flattenArray.GetArrayCount() > 1)
			{
				CSDataArray cSDataArray2 = new CSDataArray();
				for (int j = 0; j < point3dList.Count; j++)
				{
					cSDataArray2.Append(new CSPointData(point3dList[j]));
				}
				cSDataArray.Append(cSDataArray2);
			}
			else
			{
				for (int k = 0; k < point3dList.Count; k++)
				{
					cSDataArray.Append(new CSPointData(point3dList[k]));
				}
			}
		}
		oPoints.SetData(cSDataArray);
		return base.Update();
	}

	private static Point3dList GeneratePointsFromCurve(Curve crv, double size, double spacing)
	{
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
		Point3dList point3dList = new Point3dList();
		double[] array3 = array2;
		foreach (double t in array3)
		{
			point3dList.Add(crv.PointAt(t));
		}
		return point3dList;
	}
}
