using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Cutters;

[CSBlock("HaloCutter", "{AE7C8085-3F52-4af2-89C0-9CAB22291DCC}")]
public sealed class CSHaloCutter : CSBlock
{
	public CSInputChannel InHalo { get; }

	public CSInputChannel InRingRailRadius { get; }

	public CSInputChannel InRingRailOffset { get; }

	public CSInputChannel InHeight { get; }

	public CSInputChannel InLength { get; }

	public CSInputChannel InFlowSurfaceEnable { get; }

	public CSInputChannel InFlowSurfaceParamStart { get; }

	public CSInputChannel InFlowSurfaceParamEnd { get; }

	public CSOutputChannel OutCutter { get; }

	public CSOutputChannel OutCurve { get; }

	public CSOutputChannel OutSurface { get; }

	public CSHaloCutter(string identifier)
		: base(identifier)
	{
		InHalo = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InRingRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRingRailOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 50.0);
		InFlowSurfaceEnable = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InFlowSurfaceParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFlowSurfaceParamEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutCutter = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		OutCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSData data = InHalo.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Halo");
		}
		Brep brep = InHalo.Brep;
		if (brep == null)
		{
			return UpdateMissing("Missing Halo");
		}
		double num = InRingRailRadius.Double;
		double num2 = InRingRailOffset.Double;
		double height = InHeight.Double;
		double num3 = InLength.Double;
		double s = InFlowSurfaceParamStart.Double;
		double s2 = InFlowSurfaceParamEnd.Double;
		bool boolean = InFlowSurfaceEnable.Boolean;
		CSDataHelper.GetTransformNoArray(data, out var value);
		Vector3d endDirection = new Vector3d(Vector3d.ZAxis);
		endDirection.Transform(value);
		endDirection.Y = 0.0;
		Transform xform = Transform.Rotation(Vector3d.ZAxis, endDirection, Point3d.Origin);
		Curve cutterProfile = null;
		if (!MakeCutterProfile(num + num2, height, ref cutterProfile))
		{
			return UpdateError("Error Making Profile");
		}
		cutterProfile.Transform(xform);
		double num4 = num3 / 2.0;
		Curve curve = cutterProfile.DuplicateCurve();
		curve.Translate(new Vector3d(0.0, 0.0 - num4, 0.0));
		Curve curve2 = cutterProfile.DuplicateCurve();
		curve2.Translate(new Vector3d(0.0, num4, 0.0));
		Brep brep2 = CSUtilRhino.CapPlanarHoles(NurbsSurface.CreateRuledSurface(curve, curve2).ToBrep(), CSApplicationHelper.Application.Tolerance);
		if (brep2 == null)
		{
			UpdateError("Capping Cutter");
		}
		OutCutter.SetData(new CSOnGeometryData(brep2));
		if (boolean)
		{
			if (!Intersection.BrepBrep(brep, brep2, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var _))
			{
				return UpdateError("Error Intersect");
			}
			if (intersectionCurves.Length < 4)
			{
				return UpdateError("Too many intersects");
			}
			double num5 = 0.0;
			Curve[] array = intersectionCurves;
			foreach (Curve curve3 in array)
			{
				num5 += curve3.PointAtStart.Z;
			}
			num5 /= (double)intersectionCurves.Length;
			int num6 = -1;
			int num7 = -1;
			for (int j = 0; j < intersectionCurves.Length; j++)
			{
				if (intersectionCurves[j].PointAtStart.Z > num5)
				{
					if (num6 == -1 || intersectionCurves[j].PointAtStart.Y > intersectionCurves[num6].PointAtStart.Y)
					{
						num6 = j;
					}
				}
				else if (num7 == -1 || intersectionCurves[j].PointAtStart.Y > intersectionCurves[num7].PointAtStart.Y)
				{
					num7 = j;
				}
			}
			double t = 0.0;
			double t2 = 0.0;
			NurbsCurve nurbsCurve = intersectionCurves[num6].ToNurbsCurve();
			NurbsCurve nurbsCurve2 = intersectionCurves[num7].ToNurbsCurve();
			if (Vector3d.Multiply(nurbsCurve.TangentAt(nurbsCurve.Domain.Min), nurbsCurve2.TangentAt(nurbsCurve2.Domain.Min)) < 0.0)
			{
				nurbsCurve2.Reverse();
			}
			nurbsCurve.NormalizedLengthParameter(s, out t);
			nurbsCurve.NormalizedLengthParameter(s2, out t2);
			nurbsCurve = nurbsCurve.Trim(new Interval(t, t2)).ToNurbsCurve();
			nurbsCurve2.NormalizedLengthParameter(s, out t);
			nurbsCurve2.NormalizedLengthParameter(s2, out t2);
			nurbsCurve2 = nurbsCurve2.Trim(new Interval(t, t2)).ToNurbsCurve();
			CSDataArray cSDataArray = new CSDataArray();
			cSDataArray.Append(new CSOnGeometryData(nurbsCurve));
			cSDataArray.Append(new CSOnGeometryData(nurbsCurve2));
			OutCurve.SetData(cSDataArray);
			if (!CSUtilSweep.Sweep2NoProfile(nurbsCurve.Convert(), nurbsCurve2.Convert(), out var breps))
			{
				return UpdateError("Sweep");
			}
			if (breps.Length != 1)
			{
				return UpdateError("Sweep To Many Surfaes");
			}
			OutSurface.SetData(new CSOnGeometryData(breps[0].Convert()));
		}
		return base.Update();
	}

	private static bool MakeCutterProfile(double arcRadius, double height, ref Curve cutterProfile)
	{
		Point3d startPoint = new Point3d(arcRadius, 0.0, 0.0);
		Point3d pointOnInterior = new Point3d(0.0, 0.0, arcRadius);
		Point3d endPoint = new Point3d(0.0 - arcRadius, 0.0, 0.0);
		ArcCurve arcCurve = new ArcCurve(new Arc(startPoint, pointOnInterior, endPoint));
		PolylineCurve polylineCurve = new PolylineCurve(new List<Point3d>
		{
			new Point3d(0.0 - arcRadius, 0.0, 0.0),
			new Point3d(0.0 - arcRadius - 1.0, 0.0, 0.0),
			new Point3d(0.0 - arcRadius - 1.0, 0.0, arcRadius + height),
			new Point3d(arcRadius + 1.0, 0.0, arcRadius + height),
			new Point3d(arcRadius + 1.0, 0.0, 0.0),
			new Point3d(arcRadius, 0.0, 0.0)
		});
		Curve[] array = Curve.JoinCurves(new Curve[2] { arcCurve, polylineCurve }, CSApplicationHelper.Application.Tolerance);
		if (array.Length != 1)
		{
			return false;
		}
		cutterProfile = array[0];
		return true;
	}
}
