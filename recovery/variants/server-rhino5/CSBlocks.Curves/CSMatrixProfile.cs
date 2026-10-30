using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Curves;

[CSBlock("MatrixProfile", "{9C7AFFD9-4922-4b7d-BD35-BF24F0FB5AED}")]
public sealed class CSMatrixProfile : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iPoints;

	public readonly CSOutputChannel oCurve;

	public CSMatrixProfile(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iPoints = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iCurve.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Profile: Missing Curve");
		}
		if (!value.IsValid)
		{
			return UpdateMissing("Profile: Nurbs Curve is not valid");
		}
		if (!value.IsClosed)
		{
			return UpdateMissing("Profile: Nurbs Curve is not closed");
		}
		NurbsCurve nurbsCurve = new NurbsCurve(value);
		Transform xform = Transform.Scale(Plane.WorldXY, 1.0, 1.0, 0.0);
		nurbsCurve.Transform(xform);
		LineCurve curveB = new LineCurve(new Point3d(0.0, -100.0, 0.0), new Point3d(0.0, 100.0, 0.0));
		CurveIntersections curveIntersections = Intersection.CurveCurve(nurbsCurve, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections.Count > 0)
		{
			int index = 0;
			for (int i = 1; i < curveIntersections.Count; i++)
			{
				if (curveIntersections[index].PointA.Y > curveIntersections[i].PointA.Y)
				{
					index = i;
				}
			}
			nurbsCurve.ChangeClosedCurveSeam(curveIntersections[index].ParameterA);
			if (nurbsCurve.TangentAt(nurbsCurve.Domain.Min) * new Vector3d(1.0, 0.0, 0.0) < 0.0)
			{
				nurbsCurve.Reverse();
			}
		}
		List<double> list = new List<double>();
		if (InputChannelCount > 1)
		{
			for (int j = 1; j < InputChannelCount; j++)
			{
				if (base.InputChannels[j] != null)
				{
					double value2 = 0.0;
					base.InputChannels[j].GetDouble(ref value2);
					if (value2 < 0.0)
					{
						value2 = 1.0 - value2;
					}
					double t = 0.0;
					if (nurbsCurve.NormalizedLengthParameter(value2, out t))
					{
						list.Add(t);
					}
				}
			}
		}
		else
		{
			for (int k = 1; k < 4; k++)
			{
				double t2 = 0.0;
				if (nurbsCurve.NormalizedLengthParameter((double)k / 4.0, out t2))
				{
					list.Add(t2);
				}
			}
		}
		oCurve.SetData(new CSProfileData(nurbsCurve, list, Transform.Unset, null));
		return base.Update();
	}
}
