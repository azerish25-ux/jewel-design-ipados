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
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		NurbsCurve nurbsCurve = new NurbsCurve(value);
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
			if (value.IsClosed)
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
		double t;
		if (lengthParameter >= 1.0)
		{
			t = value.Domain.Max;
		}
		else if (value3 <= 0.0)
		{
			t = value.Domain.Min;
		}
		else if (!value.NormalizedLengthParameter(lengthParameter, out t))
		{
			return UpdateError("Get Arc Length Failed");
		}
		double value6 = 1.0;
		iOutsideRailScale.GetDouble(ref value6);
		double t2 = lengthParameter;
		double number = t;
		if (value5 != null)
		{
			if (nurbsCurve.ClosestPoint(value.PointAt(t), out var t3))
			{
				t2 = t3;
			}
			double length = nurbsCurve.GetLength();
			number = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Min, t2)).ToNurbsCurve().GetLength() / length;
		}
		oParam.SetData(new CSNumberData(number));
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp(value, value3, ref x, value6 / 2.0);
		oXform.SetData(new CSXformData(x));
		if (value2 != null)
		{
			LineCurve lineCurve = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 30.0, 0.0));
			lineCurve.Transform(x);
			CurveIntersections curveIntersections = Intersection.CurveCurve(lineCurve, value2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count > 0)
			{
				Point3d pointAtStart = lineCurve.PointAtStart;
				Point3d pointA = curveIntersections[0].PointA;
				double num = pointAtStart.DistanceTo(pointA);
				if (num < 0.01)
				{
					num = 0.01;
				}
				oYSize.SetData(new CSNumberData(num));
				double num2 = curveIntersections[0].ParameterB;
				if (Math.Abs(value3 - value.Domain.Min) < 0.0001 && Math.Abs(num2 - value2.Domain.Max) < 0.0001)
				{
					num2 = value2.Domain.Min;
				}
				if (Math.Abs(value3 - value.Domain.Max) < 0.0001 && Math.Abs(num2 - value2.Domain.Min) < 0.0001)
				{
					num2 = value2.Domain.Max;
				}
				oParam2.SetData(new CSNumberData(num2));
			}
		}
		return base.Update();
	}
}
