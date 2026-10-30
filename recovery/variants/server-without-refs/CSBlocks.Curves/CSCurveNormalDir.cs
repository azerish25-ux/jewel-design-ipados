using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("CurveNormalDir", "{8952884D-213C-4C47-BAC1-63476266B895}")]
public sealed class CSCurveNormalDir : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iCurveMatch;

	public readonly CSInputChannel iPosition;

	public readonly CSInputChannel iDirection;

	public readonly CSOutputChannel oCurve;

	public CSCurveNormalDir(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurveMatch = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iDirection = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curves");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		double value2 = 0.0;
		if (iPosition != null)
		{
			iPosition.GetDouble(ref value2);
		}
		bool value3 = false;
		if (iDirection != null)
		{
			iDirection.GetBoolean(ref value3);
		}
		CSDataArray flattenArray2 = iCurveMatch.GetFlattenArray();
		Vector3d val = Vector3d.Unset;
		if (flattenArray2.GetArrayCount() > 0)
		{
			CSDataHelper.GetNurbsCurve(flattenArray2.GetArrayItem(0), out var value4);
			if (CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)(object)value4, out var plane))
			{
				val = ((Plane)(ref plane)).Normal;
			}
		}
		if (!CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)(object)value, out var plane2))
		{
			oCurve.SetData(null);
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
		double num = 0.0;
		if (!((Curve)value).NormalizedLengthParameter(value2, ref num))
		{
			oCurve.SetData(null);
			return UpdateError("Invalid point on crv");
		}
		Point3d val2 = ((Curve)value).PointAt(num);
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(((Plane)(ref plane2)).Normal);
		if (value3)
		{
			val3 *= -1.0;
		}
		LineCurve geometry = new LineCurve(val2, val2 + val3);
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		return base.Update();
	}
}
