using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Cutters;

[CSBlock("ShankCutterMounting", "{2A7E8748-93E0-43e1-B84C-13896077DE92}")]
public sealed class CSShankCutterMounting : CSBlock
{
	public CSInputChannel InRail0 { get; }

	public CSInputChannel InRail1 { get; }

	public CSInputChannel InOffset { get; }

	public CSInputChannel InBlendLength { get; }

	public CSInputChannel InDoMountingCutoutTop { get; }

	public CSInputChannel InBaseAngleTop { get; }

	public CSInputChannel InMountingAngleTop { get; }

	public CSInputChannel InDoMountingCutoutBottom { get; }

	public CSInputChannel InBaseAngleBottom { get; }

	public CSInputChannel InMountingAngleBottom { get; }

	public CSOutputChannel OutRail0 { get; }

	public CSShankCutterMounting(string identifier)
		: base(identifier)
	{
		InRail0 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InRail1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InBlendLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InDoMountingCutoutTop = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InBaseAngleTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.1);
		InMountingAngleTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InDoMountingCutoutBottom = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InBaseAngleBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.1);
		InMountingAngleBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutRail0 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	private static bool MakeOffset(NurbsCurve rail0, NurbsCurve rail1, double vOffset, double blendLength, ref NurbsCurve shankCutterCurve, out string error)
	{
		Transform xform = Transform.Scale(Plane.WorldXY, 1.0, 0.0, 1.0);
		rail0.Transform(xform);
		rail1.Transform(xform);
		Point3d point3d = new Point3d(rail0.PointAtStart);
		Point3d point3d2 = new Point3d(rail1.PointAtStart);
		if (Math.Abs(point3d.X) > 0.001 || point3d.Z < 0.0)
		{
			LineCurve curveB = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections curveIntersections = Intersection.CurveCurve(rail0, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count == 0)
			{
				error = "CSShankCutter - Start Point of inside ring rail not at X=0";
				return false;
			}
			rail0.ChangeClosedCurveSeam(curveIntersections[0].ParameterA);
			point3d = new Point3d(rail0.PointAtStart);
			if (Math.Abs(point3d.X) > 0.001)
			{
				error = "CSShankCutter - Start Point of inside ring rail not at X=0";
				return false;
			}
		}
		if (Math.Abs(point3d2.X) > 0.001 || point3d2.Z < 0.0)
		{
			LineCurve curveB2 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections curveIntersections2 = Intersection.CurveCurve(rail1, curveB2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections2.Count == 0)
			{
				error = "CSShankCutter - Start Point of outside ring rail not at X=0";
				return false;
			}
			rail1.ChangeClosedCurveSeam(curveIntersections2[0].ParameterA);
			point3d2 = new Point3d(rail1.PointAtStart);
			if (Math.Abs(point3d2.X) > 0.001)
			{
				error = "CSShankCutter - Start Point of outside ring rail not at X=0";
				return false;
			}
		}
		if (point3d.Z > point3d2.Z)
		{
			error = "CSShankCutter - Inside Ring Rail is outside of Outside Ring Rail";
			return false;
		}
		Vector3d vector3d = new Vector3d(rail0.TangentAt(rail0.Domain.Min));
		Vector3d vector3d2 = new Vector3d(rail1.TangentAt(rail1.Domain.Min));
		if (vector3d.X < 0.0)
		{
			rail0.Reverse();
		}
		if (vector3d2.X < 0.0)
		{
			rail1.Reverse();
		}
		Curve curve = CSUtilGeometry.OffsetOld(rail0, new Vector3d(0.0, 1.0, 0.0), Point3d.Origin, 0.0 - vOffset, closed: true);
		Curve curve2 = CSUtilGeometry.OffsetOld(rail1, new Vector3d(0.0, 1.0, 0.0), Point3d.Origin, vOffset, closed: true);
		if (curve == null)
		{
			error = "CSShankCutter - Inside Ring Rail Offset Failed";
			return false;
		}
		if (curve2 == null)
		{
			error = "CSShankCutter - Outside Ring Rail Offset Failed";
			return false;
		}
		NurbsCurve nurbsCurve = curve.ToNurbsCurve();
		NurbsCurve nurbsCurve2 = curve2.ToNurbsCurve();
		point3d = nurbsCurve.PointAtStart;
		point3d2 = nurbsCurve2.PointAtStart;
		if (point3d.Z + 0.1 > point3d2.Z)
		{
			error = "CSShankCutter - Offset to big";
			return false;
		}
		CurveIntersections curveIntersections3 = Intersection.CurveCurve(curve, curve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections3.Count == 0)
		{
			error = "CSShankCutter - Intersect Failed";
			return false;
		}
		Point3d point3d3 = new Point3d(0.0, 0.0, -100.0);
		Point3d point3d4 = new Point3d(0.0, 0.0, -100.0);
		double t = 0.0;
		double t2 = 0.0;
		double t3 = 0.0;
		double t4 = 0.0;
		foreach (IntersectionEvent item in curveIntersections3)
		{
			Point3d pointA = item.PointA;
			if (pointA.X > 0.0)
			{
				if (pointA.Z > point3d4.Z)
				{
					point3d4 = pointA;
					t2 = item.ParameterA;
					t4 = item.ParameterB;
				}
			}
			else if (pointA.Z > point3d3.Z)
			{
				point3d3 = pointA;
				t = item.ParameterA;
				t3 = item.ParameterB;
			}
		}
		NurbsCurve nurbsCurve3 = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Min, t2)).ToNurbsCurve();
		NurbsCurve nurbsCurve4 = nurbsCurve.Trim(new Interval(t, nurbsCurve.Domain.Max)).ToNurbsCurve();
		NurbsCurve nurbsCurve5 = nurbsCurve2.Trim(new Interval(nurbsCurve2.Domain.Min, t4)).ToNurbsCurve();
		NurbsCurve nurbsCurve6 = nurbsCurve2.Trim(new Interval(t3, nurbsCurve2.Domain.Max)).ToNurbsCurve();
		List<Curve> list = new List<Curve>();
		if (blendLength > 0.01)
		{
			double length = nurbsCurve3.GetLength();
			nurbsCurve3.NormalizedLengthParameter((length - blendLength) / length, out var t5);
			nurbsCurve3 = nurbsCurve3.Trim(new Interval(nurbsCurve3.Domain.Min, t5)).ToNurbsCurve();
			length = nurbsCurve4.GetLength();
			nurbsCurve4.NormalizedLengthParameter(blendLength / length, out t5);
			nurbsCurve4 = nurbsCurve4.Trim(new Interval(t5, nurbsCurve4.Domain.Max)).ToNurbsCurve();
			length = nurbsCurve5.GetLength();
			nurbsCurve5.NormalizedLengthParameter((length - blendLength) / length, out t5);
			nurbsCurve5 = nurbsCurve5.Trim(new Interval(nurbsCurve5.Domain.Min, t5)).ToNurbsCurve();
			length = nurbsCurve6.GetLength();
			nurbsCurve6.NormalizedLengthParameter(blendLength / length, out t5);
			nurbsCurve6 = nurbsCurve6.Trim(new Interval(t5, nurbsCurve6.Domain.Max)).ToNurbsCurve();
			list.Add(nurbsCurve3);
			list.Add(CSUtilCurves.BlendCurve(nurbsCurve3.PointAtEnd, nurbsCurve5.PointAtEnd, nurbsCurve3.TangentAt(nurbsCurve3.Domain.Max), nurbsCurve5.TangentAt(nurbsCurve5.Domain.Max)));
			list.Add(nurbsCurve5);
			list.Add(nurbsCurve6);
			list.Add(CSUtilCurves.BlendCurve(nurbsCurve4.PointAtStart, nurbsCurve6.PointAtStart, -nurbsCurve4.TangentAt(nurbsCurve4.Domain.Min), -nurbsCurve6.TangentAt(nurbsCurve6.Domain.Min)));
			list.Add(nurbsCurve4);
		}
		else
		{
			list.Add(nurbsCurve3);
			list.Add(nurbsCurve5);
			list.Add(nurbsCurve6);
			list.Add(nurbsCurve4);
		}
		Curve[] array = Curve.JoinCurves(list, 0.01);
		if (array == null || array.Length == 0 || array.Length > 1)
		{
			error = "CSShankCutter - Curve split and re-merge failed";
			return false;
		}
		shankCutterCurve = array[0].ToNurbsCurve();
		if (shankCutterCurve == null)
		{
			error = "CSShankCutter - Curve split and re-merge failed";
			return false;
		}
		error = string.Empty;
		return true;
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InRail0.NurbsCurve;
		NurbsCurve nurbsCurve2 = InRail1.NurbsCurve;
		double vOffset = InOffset.Double;
		double blendLength = InBlendLength.Double;
		bool boolean = InDoMountingCutoutTop.Boolean;
		double num = InBaseAngleTop.Double;
		if (num < 0.01)
		{
			return UpdateError("Base Top Angle To Low");
		}
		double angleDegrees = InMountingAngleTop.Double;
		bool boolean2 = InDoMountingCutoutBottom.Boolean;
		double num2 = InBaseAngleBottom.Double;
		if (num2 < 0.01)
		{
			return UpdateError("Base Bottom Angle To Low");
		}
		double angleDegrees2 = InMountingAngleBottom.Double;
		NurbsCurve shankCutterCurve = null;
		if (!MakeOffset(nurbsCurve, nurbsCurve2, vOffset, blendLength, ref shankCutterCurve, out var error))
		{
			return UpdateError("Offset: " + error);
		}
		if (shankCutterCurve == null)
		{
			return UpdateError("Error with Cutter");
		}
		if (boolean)
		{
			LineCurve curveB = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections curveIntersections = Intersection.CurveCurve(shankCutterCurve, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count == 0)
			{
				return UpdateError("Error Intersect Failed");
			}
			int index = 0;
			for (int i = 1; i < curveIntersections.Count; i++)
			{
				if (curveIntersections[i].PointA.Z > curveIntersections[index].PointA.Z)
				{
					index = i;
				}
			}
			shankCutterCurve.ChangeClosedCurveSeam(curveIntersections[index].ParameterA);
			if (shankCutterCurve.TangentAt(shankCutterCurve.Domain.Min) * new Vector3d(1.0, 0.0, 0.0) < 0.0)
			{
				shankCutterCurve.Reverse();
			}
			Vector3d vector3d = new Vector3d(0.0, 0.0, 50.0);
			vector3d.Rotate(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num), Vector3d.YAxis);
			LineCurve curveB2 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0) + vector3d);
			vector3d.X = 0.0 - vector3d.X;
			LineCurve curveB3 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0) + vector3d);
			CurveIntersections curveIntersections2 = Intersection.CurveCurve(shankCutterCurve, curveB2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			CurveIntersections curveIntersections3 = Intersection.CurveCurve(shankCutterCurve, curveB3, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections2.Count == 0)
			{
				return UpdateError("Error Intersect Failed");
			}
			if (curveIntersections3.Count == 0)
			{
				return UpdateError("Error Intersect 2 Failed");
			}
			int index2 = 0;
			for (int j = 1; j < curveIntersections2.Count; j++)
			{
				if (curveIntersections2[j].PointA.Z > curveIntersections2[index2].PointA.Z)
				{
					index2 = j;
				}
			}
			int index3 = 0;
			for (int k = 1; k < curveIntersections3.Count; k++)
			{
				if (curveIntersections3[k].PointA.Z > curveIntersections3[index3].PointA.Z)
				{
					index3 = k;
				}
			}
			Vector3d vector3d2 = shankCutterCurve.TangentAt(curveIntersections3[index3].ParameterA);
			vector3d2.Unitize();
			double num3 = 0.0 - Math.Asin(vector3d2.Z);
			shankCutterCurve = shankCutterCurve.Trim(new Interval(curveIntersections3[index3].ParameterA, curveIntersections2[index2].ParameterA)).ToNurbsCurve();
			Vector3d vector3d3 = new Vector3d(0.0, 0.0, 10.0);
			vector3d3.Rotate(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees) + num3, Vector3d.YAxis);
			LineCurve lineCurve = new LineCurve(shankCutterCurve.PointAtStart, shankCutterCurve.PointAtStart + vector3d3);
			vector3d3.X = 0.0 - vector3d3.X;
			LineCurve lineCurve2 = new LineCurve(shankCutterCurve.PointAtEnd, shankCutterCurve.PointAtEnd + vector3d3);
			CurveIntersections curveIntersections4 = Intersection.CurveCurve(lineCurve, lineCurve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections4.Count < 1)
			{
				LineCurve lineCurve3 = new LineCurve(lineCurve.PointAtEnd, lineCurve2.PointAtEnd);
				Curve[] array = Curve.JoinCurves(new Curve[4] { shankCutterCurve, lineCurve, lineCurve3, lineCurve2 });
				if (array != null && array.Length != 0)
				{
					shankCutterCurve = array[0].ToNurbsCurve();
				}
			}
			else
			{
				Curve curve = lineCurve.Trim(new Interval(lineCurve.Domain.Min, curveIntersections4[0].ParameterA));
				Curve curve2 = lineCurve2.Trim(new Interval(lineCurve.Domain.Min, curveIntersections4[0].ParameterB));
				Curve[] array2 = Curve.JoinCurves(new Curve[3] { shankCutterCurve, curve, curve2 });
				if (array2 != null && array2.Length != 0)
				{
					shankCutterCurve = array2[0].ToNurbsCurve();
				}
			}
		}
		if (boolean2)
		{
			LineCurve curveB4 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections curveIntersections5 = Intersection.CurveCurve(shankCutterCurve, curveB4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections5.Count == 0)
			{
				return UpdateError("Error Intersect Failed");
			}
			int index4 = 0;
			for (int l = 1; l < curveIntersections5.Count; l++)
			{
				if (curveIntersections5[l].PointA.Z < curveIntersections5[index4].PointA.Z)
				{
					index4 = l;
				}
			}
			shankCutterCurve.ChangeClosedCurveSeam(curveIntersections5[index4].ParameterA);
			if (shankCutterCurve.TangentAt(shankCutterCurve.Domain.Min) * new Vector3d(1.0, 0.0, 0.0) < 0.0)
			{
				shankCutterCurve.Reverse();
			}
			Vector3d vector3d4 = new Vector3d(0.0, 0.0, 50.0);
			vector3d4.Rotate(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num2), Vector3d.YAxis);
			LineCurve curveB5 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0) + vector3d4);
			vector3d4.X = 0.0 - vector3d4.X;
			LineCurve curveB6 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0) + vector3d4);
			CurveIntersections curveIntersections6 = Intersection.CurveCurve(shankCutterCurve, curveB5, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			CurveIntersections curveIntersections7 = Intersection.CurveCurve(shankCutterCurve, curveB6, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections6.Count == 0)
			{
				return UpdateError("Error Intersect Failed");
			}
			if (curveIntersections7.Count == 0)
			{
				return UpdateError("Error Intersect Failed");
			}
			int index5 = 0;
			for (int m = 1; m < curveIntersections6.Count; m++)
			{
				if (curveIntersections6[m].PointA.Z < curveIntersections6[index5].PointA.Z)
				{
					index5 = m;
				}
			}
			int index6 = 0;
			for (int n = 1; n < curveIntersections7.Count; n++)
			{
				if (curveIntersections7[n].PointA.Z < curveIntersections7[index6].PointA.Z)
				{
					index6 = n;
				}
			}
			Vector3d vector3d5 = shankCutterCurve.TangentAt(curveIntersections7[index6].ParameterA);
			vector3d5.Unitize();
			double num4 = 0.0 - Math.Asin(vector3d5.Z);
			shankCutterCurve = shankCutterCurve.Trim(new Interval(curveIntersections7[index6].ParameterA, curveIntersections6[index5].ParameterA)).ToNurbsCurve();
			Vector3d vector3d6 = new Vector3d(0.0, 0.0, -10.0);
			vector3d6.Rotate(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2) + num4, Vector3d.YAxis);
			LineCurve lineCurve4 = new LineCurve(shankCutterCurve.PointAtStart, shankCutterCurve.PointAtStart + vector3d6);
			vector3d6.X = 0.0 - vector3d6.X;
			LineCurve lineCurve5 = new LineCurve(shankCutterCurve.PointAtEnd, shankCutterCurve.PointAtEnd + vector3d6);
			CurveIntersections curveIntersections8 = Intersection.CurveCurve(lineCurve4, lineCurve5, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections8.Count < 1)
			{
				LineCurve lineCurve6 = new LineCurve(lineCurve4.PointAtEnd, lineCurve5.PointAtEnd);
				Curve[] array3 = Curve.JoinCurves(new Curve[4] { shankCutterCurve, lineCurve4, lineCurve6, lineCurve5 });
				if (array3 != null && array3.Length != 0)
				{
					shankCutterCurve = array3[0].ToNurbsCurve();
				}
			}
			else
			{
				Curve curve3 = lineCurve4.Trim(new Interval(lineCurve4.Domain.Min, curveIntersections8[0].ParameterA));
				Curve curve4 = lineCurve5.Trim(new Interval(lineCurve4.Domain.Min, curveIntersections8[0].ParameterB));
				Curve[] array4 = Curve.JoinCurves(new Curve[3] { shankCutterCurve, curve3, curve4 });
				if (array4 != null && array4.Length != 0)
				{
					shankCutterCurve = array4[0].ToNurbsCurve();
				}
			}
		}
		OutRail0.SetData(new CSOnGeometryData(shankCutterCurve));
		return base.Update();
	}
}
