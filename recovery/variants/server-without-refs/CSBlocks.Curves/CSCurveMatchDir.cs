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
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
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
		Vector3d val = Vector3d.Unset;
		if (flattenArray2.GetArrayCount() > 0)
		{
			CSDataHelper.GetNurbsCurve(flattenArray2.GetArrayItem(0), out var value2);
			if (value2 == null)
			{
				return UpdateMissing("Missing Match Curve");
			}
			if (CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)(object)value2, out var plane))
			{
				val = ((Plane)(ref plane)).Normal;
			}
		}
		if (!CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)(object)value, out var plane2))
		{
			oMatchedCurve.SetData(null);
			return UpdateError("Input Curve not planar");
		}
		if (val != Vector3d.Unset)
		{
			Vector3d normal = ((Plane)(ref plane2)).Normal;
			if (Vector3d.Multiply(val, normal) < 0.0)
			{
				((Curve)value).Reverse();
				((Plane)(ref plane2)).Flip();
			}
		}
		oMatchedCurve.SetData(new CSOnGeometryData((GeometryBase)(object)value));
		return base.Update();
	}
}
