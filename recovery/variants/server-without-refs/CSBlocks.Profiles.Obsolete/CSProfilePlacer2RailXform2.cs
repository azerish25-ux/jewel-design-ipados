using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Profiles.Obsolete;

[Obsolete]
[CSBlock("ProfilePlacer2RailXform_V2", "{A2CE5B93-2D3B-40b3-8A9E-8CC410356EDF}")]
public sealed class CSProfilePlacer2RailXform2 : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iOutsideRail;

	public readonly CSInputChannel iOutsideRailScale;

	public readonly CSInputChannel iParameter;

	public readonly CSInputChannel iOffset;

	public readonly CSInputChannel iFromObject;

	public readonly CSOutputChannel oXform;

	public readonly CSOutputChannel oParam;

	public readonly CSOutputChannel oParam2;

	public readonly CSOutputChannel oYSize;

	public CSProfilePlacer2RailXform2(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRailScale = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iParameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFromObject = new CSInputChannel(this, ChannelDataType.Point | ChannelDataType.Curve, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		NurbsCurve val = new NurbsCurve(value);
		CSDataHelper.GetNurbsCurve(iOutsideRail.Data, out var value2);
		double value3 = 0.0;
		iParameter.GetDouble(ref value3);
		double value4 = 0.0;
		iOffset.GetDouble(ref value4);
		value3 += value4;
		NurbsCurve value5;
		if (!(iFromObject.Data is ICSArray))
		{
			CSDataHelper.GetNurbsCurve(iFromObject.Data, out value5);
		}
		else
		{
			value5 = null;
		}
		if (value5 == null)
		{
			if (((Curve)value).IsClosed)
			{
				if (value3 < 0.0)
				{
					value3 += 1.0;
				}
				if (value3 > 1.0)
				{
					value3 -= 1.0;
				}
				if (value3 == 1.0)
				{
					value3 = 0.0;
				}
			}
			else
			{
				if (value3 < 0.0)
				{
					value3 = 0.0;
				}
				if (value3 > 1.0)
				{
					value3 = 1.0;
				}
			}
		}
		double lengthParameter = value3;
		bool flip = false;
		CSUtilGeometry.AdjustRailFromCurve(value5, ref value, ref lengthParameter, ref flip);
		Interval domain;
		double num = default(double);
		if (lengthParameter >= 1.0)
		{
			domain = ((Curve)value).Domain;
			num = ((Interval)(ref domain)).Max;
		}
		else if (value3 <= 0.0)
		{
			domain = ((Curve)value).Domain;
			num = ((Interval)(ref domain)).Min;
		}
		else if (!((Curve)value).NormalizedLengthParameter(lengthParameter, ref num))
		{
			return UpdateError("Get Arc Length Failed");
		}
		double value6 = 1.0;
		iOutsideRailScale.GetDouble(ref value6);
		double num2 = lengthParameter;
		double number = num;
		if (value5 != null)
		{
			double num3 = default(double);
			if (((Curve)val).ClosestPoint(((Curve)value).PointAt(num), ref num3))
			{
				num2 = num3;
			}
			double length = ((Curve)val).GetLength();
			domain = ((Curve)val).Domain;
			number = ((Curve)((Curve)val).Trim(new Interval(((Interval)(ref domain)).Min, num2)).ToNurbsCurve()).GetLength() / length;
		}
		oParam.SetData(new CSNumberData(number));
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp((Curve)(object)value, value3, ref x, value6 / 2.0);
		oXform.SetData(new CSXformData(x));
		if (value2 != null)
		{
			LineCurve val2 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 30.0, 0.0));
			((GeometryBase)val2).Transform(x);
			CurveIntersections val3 = Intersection.CurveCurve((Curve)(object)val2, (Curve)(object)value2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val3.Count > 0)
			{
				Point3d pointAtStart = ((Curve)val2).PointAtStart;
				Point3d pointA = val3[0].PointA;
				double num4 = ((Point3d)(ref pointAtStart)).DistanceTo(pointA);
				if (num4 < 0.01)
				{
					num4 = 0.01;
				}
				oYSize.SetData(new CSNumberData(num4));
				double num5 = val3[0].ParameterB;
				double num6 = value3;
				domain = ((Curve)value).Domain;
				if (Math.Abs(num6 - ((Interval)(ref domain)).Min) < 0.0001)
				{
					double num7 = num5;
					domain = ((Curve)value2).Domain;
					if (Math.Abs(num7 - ((Interval)(ref domain)).Max) < 0.0001)
					{
						domain = ((Curve)value2).Domain;
						num5 = ((Interval)(ref domain)).Min;
					}
				}
				double num8 = value3;
				domain = ((Curve)value).Domain;
				if (Math.Abs(num8 - ((Interval)(ref domain)).Max) < 0.0001)
				{
					double num9 = num5;
					domain = ((Curve)value2).Domain;
					if (Math.Abs(num9 - ((Interval)(ref domain)).Min) < 0.0001)
					{
						domain = ((Curve)value2).Domain;
						num5 = ((Interval)(ref domain)).Max;
					}
				}
				oParam2.SetData(new CSNumberData(num5));
			}
		}
		return base.Update();
	}
}
