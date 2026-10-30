using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("ProcessCrv", "{5068B809-3C7E-480c-BD90-D78E3BFA488D}")]
public sealed class CSProcessCrv : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSOutputChannel oParameters;

	public readonly CSOutputChannel oPoints;

	public readonly CSOutputChannel oTangents;

	public CSProcessCrv(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oParameters = new CSOutputChannel(GetPropertyName(() => oParameters), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		oPoints = new CSOutputChannel(GetPropertyName(() => oPoints), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		oTangents = new CSOutputChannel(GetPropertyName(() => oTangents), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curve");
		}
		CSData arrayItem = flattenArray.GetArrayItem(0);
		if (arrayItem == null)
		{
			return UpdateMissing("Missing Curve");
		}
		CSDataHelper.GetNurbsCurve(arrayItem, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Curve");
		}
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		double length = value.GetLength();
		if (double.IsNaN(length) || length <= 0.0)
		{
			return UpdateError("GetLength failed");
		}
		int num = (int)(length * 50.0);
		int num2 = num + 1;
		for (int i = 0; i < num2; i++)
		{
			if (value.NormalizedLengthParameter((double)i / (double)num, out var t))
			{
				cSDataArray.Append(new CSNumberData(t));
				Point3d pointStore = value.PointAt(t);
				cSDataArray2.Append(new CSPointData(pointStore));
				Vector3d vectorStore = value.TangentAt(t);
				cSDataArray3.Append(new CSVectorData(vectorStore));
			}
		}
		oParameters.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		oPoints.SetData((cSDataArray2.GetArrayCount() == 1) ? cSDataArray2.GetArrayItem(0) : cSDataArray2);
		oTangents.SetData((cSDataArray3.GetArrayCount() == 1) ? cSDataArray3.GetArrayItem(0) : cSDataArray3);
		return base.Update();
	}
}
