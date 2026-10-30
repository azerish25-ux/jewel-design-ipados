using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("CurveMatchDir", "{A78369D9-31F3-4499-BDCF-0B8D9A09F182}")]
public sealed class CSCurveMatchDir : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iCurveMatch;

	public readonly CSOutputChannel oMatchedCurve;

	public CSCurveMatchDir(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurveMatch = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oMatchedCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curves");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Curves");
		}
		CSDataArray flattenArray2 = iCurveMatch.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Match Curve");
		}
		Vector3d vector3d = Vector3d.Unset;
		if (flattenArray2.GetArrayCount() > 0)
		{
			CSDataHelper.GetNurbsCurve(flattenArray2.GetArrayItem(0), out var value2);
			if (value2 == null)
			{
				return UpdateMissing("Missing Match Curve");
			}
			if (CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)value2, out Plane plane))
			{
				vector3d = plane.Normal;
			}
		}
		if (!CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)value, out Plane plane2))
		{
			oMatchedCurve.SetData(null);
			return UpdateError("Input Curve not planar");
		}
		if (vector3d != Vector3d.Unset)
		{
			Vector3d normal = plane2.Normal;
			if (Vector3d.Multiply(vector3d, normal) < 0.0)
			{
				value.Reverse();
				plane2.Flip();
			}
		}
		oMatchedCurve.SetData(new CSOnGeometryData(value));
		return base.Update();
	}
}
