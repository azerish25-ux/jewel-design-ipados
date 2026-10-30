using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("CurveOffsetWithOrigin", "{74EC9B54-5D96-48A7-86C9-E13DCCA60A4B}")]
public sealed class CSCurveOffsetWithOrigin : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iOffset;

	public readonly CSInputChannel iOrigin;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oCurve;

	public CSCurveOffsetWithOrigin(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOrigin = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Curves");
		}
		double value = 0.0;
		iOffset.GetDouble(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		double value2 = 0.0;
		if (iZ != null)
		{
			iZ.GetDouble(ref value2);
		}
		CSDataArray flattenArray2 = iOrigin.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Origin");
		}
		if (!CSDataHelper.GetPoint(flattenArray2.GetArrayItem(0), out var value3))
		{
			return UpdateMissing("Origin");
		}
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetCurve(flattenArray.GetArrayItem(i), out var value4);
			if (value4 == null)
			{
				return UpdateMissing("Bad Curves");
			}
			Vector3d normalDir = Vector3d.ZAxis;
			if (value4.TryGetPlane(out var plane))
			{
				normalDir = plane.Normal;
				value = 0.0 - value;
			}
			Curve curve = CSUtilGeometry.Offset(value4, normalDir, value3, value);
			if (curve == null)
			{
				return UpdateError("Unable to offset");
			}
			if (value2 != 0.0)
			{
				Transform transform = Transform.Translation(0.0, 0.0, value2);
				curve.Transform(transform);
				cSDataArray.Append(new CSOnGeometryData(curve, transform, applyTransform: false));
			}
			else
			{
				cSDataArray.Append(new CSOnGeometryData(curve));
			}
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
