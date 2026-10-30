using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacerGemTo2RailXform", "{D8FCA1AB-512C-4955-9C46-9E9A895DCDBE}")]
public sealed class CSProfilePlacerGemTo2RailXform : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iOutsideRail;

	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iParameter;

	public readonly CSOutputChannel oXform;

	public readonly CSOutputChannel oParam;

	public readonly CSOutputChannel oParam2;

	public readonly CSOutputChannel oYSize;

	public CSProfilePlacerGemTo2RailXform(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iParameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Rail");
		}
		CSDataHelper.GetNurbsCurve(iOutsideRail.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Outside Rail");
		}
		CSData data = iGem.Data;
		if (data == null)
		{
			return UpdateMissing("Gem");
		}
		double value3 = 0.0;
		iParameter.GetDouble(ref value3);
		CSDataHelper.GetTransformNoArray(data, out var value4);
		if (!((Transform)(ref value4)).IsValid)
		{
			return UpdateError("Unable to get Xform from gem");
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(Point3d.Origin);
		((Point3d)(ref val)).Transform(value4);
		double num = default(double);
		if (!((Curve)value).ClosestPoint(val, ref num))
		{
			return UpdateError("GetClosestPoint to rail failed");
		}
		NurbsCurve obj = value;
		Interval domain = ((Curve)value).Domain;
		double length = ((Curve)obj).GetLength(0.01, new Interval(((Interval)(ref domain)).Min, num));
		if (double.IsNaN(length) || length < 0.0)
		{
			return UpdateError("GetLength to gemPoint failed");
		}
		length += value3;
		double length2 = ((Curve)value).GetLength();
		if (double.IsNaN(length2) || length2 < 0.0)
		{
			return UpdateError("GetLength of rail failed");
		}
		while (length >= length2)
		{
			length -= length2;
		}
		for (; length < 0.0; length += length2)
		{
		}
		double num2 = default(double);
		if (!((Curve)value).NormalizedLengthParameter(length / length2, ref num2))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		Point3d val2 = ((Curve)value).PointAt(num2);
		Vector3d val3 = CSBlockUtil.CSUtilMath.AverageTangentAt((Curve)(object)value, num2, CSApplicationHelper.Application.Tolerance);
		((Vector3d)(ref val3)).Unitize();
		Vector3d val4 = CSBlockUtil.CSUtilMath.AverageNormalAt(Vector3d.YAxis, val3);
		((Vector3d)(ref val4)).Unitize();
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(val2);
		((Vector3d)(ref val5)).Y = 0.0;
		if (Vector3d.Multiply(val5, val4) < 0.0)
		{
			((Vector3d)(ref val4)).Reverse();
		}
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, val2, val3, val4);
		LineCurve val6 = new LineCurve(val2, val2 + val4 * 100.0);
		CurveIntersections val7 = Intersection.CurveCurve((Curve)(object)value2, (Curve)(object)val6, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val7.Count <= 0)
		{
			return UpdateError("Unable to intersect outside rail");
		}
		double num3 = double.NaN;
		double num4 = double.NaN;
		for (int i = 0; i < val7.Count; i++)
		{
			Point3d pointA = val7[i].PointA;
			double parameterA = val7[i].ParameterA;
			double num5 = ((Point3d)(ref val2)).DistanceTo(pointA);
			if (!double.IsNaN(num3) || !double.IsNaN(num4) || !(num5 >= num4))
			{
				num3 = parameterA;
				num4 = num5;
			}
		}
		oXform.SetData(new CSXformData(transform));
		oParam.SetData(new CSNumberData(num2));
		oParam2.SetData(new CSNumberData(num3));
		oYSize.SetData(new CSNumberData(num4));
		return base.Update();
	}
}
