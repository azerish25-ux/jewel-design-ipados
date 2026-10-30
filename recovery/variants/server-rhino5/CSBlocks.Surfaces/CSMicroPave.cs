using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Surfaces;

[CSBlock("MicroPave", "{78E10B3E-8040-47D6-B262-0D102D7CC6F2}")]
public sealed class CSMicroPave : CSBlock
{
	private struct MicroPaveProngInfo(double param, double sizeGem, double sizeShank)
	{
		public double Param = param;

		public double SizeGem = sizeGem;
	}

	private Curve _interpCrv;

	private Curve _lineCrv;

	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InProfileBase;

	public readonly CSInputChannel InRingRailRadius;

	public readonly CSInputChannel InProngHeight;

	public readonly CSOutputChannel OutSurface;

	public readonly CSOutputChannel OutCurveStart;

	public readonly CSOutputChannel OutCurveEnd;

	public CSMicroPave(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfileBase = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InRingRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProngHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		OutCurveStart = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutCurveEnd = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		List<CSGemData> list = InGems.GetFlattenArray().Array.OfType<CSGemData>().ToList();
		if (list.Count <= 0)
		{
			return UpdateMissing("Gems");
		}
		CSDataHelper.GetNurbsCurve(InCurve.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataHelper.GetNurbsCurve(InProfileBase.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("ProfileBase");
		}
		double value3 = double.NaN;
		InRingRailRadius.GetDouble(ref value3);
		if (double.IsNaN(value3))
		{
			return UpdateMissing("RingRailRadius");
		}
		double value4 = 1.0;
		InProngHeight.GetDouble(ref value4);
		if ((BeenUpdatedAfterLastUpdate(InGems.OutputChannelBlock) || BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock) || _interpCrv == null || _lineCrv == null) && !CalculateCurves(value, list))
		{
			return UpdateError("CalculateCurves failed");
		}
		List<MicroPaveProngInfo> list2 = new List<MicroPaveProngInfo>();
		for (int i = 0; i < list.Count(); i++)
		{
			CSGemData cSGemData = list[i];
			double t = double.NaN;
			if (value.ClosestPoint(cSGemData.Center, out t))
			{
				MicroPaveProngInfo item = new MicroPaveProngInfo
				{
					Param = t,
					SizeGem = cSGemData.GemXSize / 2.0
				};
				list2.Add(item);
			}
		}
		if (!CreateMicroPaveSurface(value, value2, value3, list2, value4, out var srf, out var profileStart, out var profileEnd))
		{
			return UpdateError("CreateMicroPaveSurface failed");
		}
		OutSurface.SetData(new CSOnGeometryData(srf));
		OutCurveStart.SetData(new CSOnGeometryData(profileStart));
		OutCurveEnd.SetData(new CSOnGeometryData(profileEnd));
		return base.Update();
	}

	private bool CalculateCurves(Curve crv, IEnumerable<CSGemData> gemDatas)
	{
		_lineCrv = null;
		_interpCrv = null;
		double length = crv.GetLength();
		if (double.IsNaN(length) || length < 0.0)
		{
			return false;
		}
		Point3d point3d = new Point3d(Point3d.Origin);
		Point3d to = new Point3d(length, 0.0, 0.0);
		_lineCrv = new LineCurve(point3d, to);
		Point3dList point3dList = new Point3dList();
		foreach (CSGemData gemData in gemDatas)
		{
			if (!crv.ClosestPoint(gemData.Center, out var t))
			{
				continue;
			}
			Interval subdomain = new Interval(crv.Domain.Min, t);
			double length2 = crv.GetLength(CSApplicationHelper.Application.Tolerance, subdomain);
			if (!double.IsNaN(length2) && !(length2 < 0.0))
			{
				double s = length2 / length;
				if (_lineCrv.NormalizedLengthParameter(s, out var t2))
				{
					Point3d item = _lineCrv.PointAt(t2);
					item.Y = gemData.GemXSize / 2.0;
					item.Z = 0.0;
					point3dList.Add(item);
				}
			}
		}
		Curve curve = Curve.CreateInterpolatedCurve(point3dList, 3, CurveKnotStyle.Uniform);
		if (curve == null)
		{
			return false;
		}
		double x = curve.PointAtStart.X;
		if (x > 0.0)
		{
			curve = curve.Extend(CurveEnd.Start, x, CurveExtensionStyle.Line);
		}
		double num = length - curve.PointAtEnd.X;
		if (num > 0.0)
		{
			curve = curve.Extend(CurveEnd.End, num, CurveExtensionStyle.Line);
		}
		_interpCrv = curve;
		if (_lineCrv != null && _lineCrv.IsValid && _interpCrv != null)
		{
			return _interpCrv.IsValid;
		}
		return false;
	}

	private bool CreateMicroPaveSurface(Curve crv, NurbsCurve profileBase, double ringRailRadius, IReadOnlyList<MicroPaveProngInfo> prongInfos, double prongHeight, out NurbsSurface srf, out NurbsCurve profileStart, out NurbsCurve profileEnd)
	{
		srf = null;
		profileStart = null;
		profileEnd = null;
		int num = prongInfos.Count + 1;
		int num2 = 10 * (num - 1) + 1;
		int count = profileBase.GrevillePoints().Count;
		int num3 = 15 + count * 2;
		int num4 = 0;
		double num5 = 0.0;
		Interval domain = profileBase.Domain;
		num5 = domain.Max;
		_ = domain.Min;
		double length = crv.GetLength();
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		list.Add(0.0);
		list2.Add(0.0);
		for (int i = 0; i < prongInfos.Count; i++)
		{
			double param = prongInfos[i].Param;
			double length2 = crv.GetLength(0.01, new Interval(crv.Domain.Min, param));
			length2 /= length;
			list.Add(length2);
			list2.Add(prongInfos[i].SizeGem);
		}
		list.Add(0.0);
		list2.Add(0.0);
		list[0] = list[1] - (list[2] - list[1]);
		list[list.Count - 1] = list[list.Count - 2] - (list[list.Count - 3] - list[list.Count - 2]);
		list2[0] = list2[1] - (list2[2] - list2[1]);
		list2[list2.Count - 1] = list2[list2.Count - 2] - (list2[list2.Count - 3] - list2[list2.Count - 2]);
		ControlPoint[,] array = new ControlPoint[num2, num3];
		int num12;
		for (int j = 0; j < num; j++)
		{
			double num6 = list[j];
			double num7 = list[j + 1] - num6;
			int num8 = 0;
			int num9 = 10;
			if (j == 0)
			{
				num8 = 5;
			}
			if (j == num - 1)
			{
				num9 = 6;
			}
			for (int k = num8; k < num9; k++)
			{
				double s = num6 + num7 * MicroPaveProng.Cv[k, 0].Location.X;
				if (!GetWidthAtNormalizedParam(s, out var width))
				{
					return false;
				}
				double t = 0.0;
				crv.NormalizedLengthParameter(s, out t);
				Point3d point3d = default(Point3d);
				Vector3d vector3d = default(Vector3d);
				point3d = crv.PointAt(t);
				vector3d = crv.TangentAt(t);
				Vector3d b = new Vector3d(0.0, 1.0, 0.0);
				Vector3d vector3d2 = Vector3d.CrossProduct(vector3d, b);
				vector3d2.Unitize();
				Vector3d vector3d3 = Vector3d.CrossProduct(vector3d2, vector3d);
				vector3d2.Y = 0.0;
				vector3d2.Unitize();
				Point3d point3d2 = new Point3d(point3d);
				point3d2.Y = 0.0;
				Point3d linePoint = point3d2;
				double lineParamOfClosestPoint = 0.0;
				double paramAlongLine = 0.0;
				if (!MicroPaveProng.PointOnLineAtDistanceFromPoint(linePoint, vector3d2, new Point3d(Point3d.Origin), ringRailRadius, ref lineParamOfClosestPoint, ref paramAlongLine))
				{
					return false;
				}
				double num10 = 0.0 - lineParamOfClosestPoint - paramAlongLine;
				int num11 = profileBase.GrevillePoints().Count - 1;
				num12 = 0;
				for (int l = 0; l < profileBase.GrevillePoints().Count; l++)
				{
					Point3d point3d3 = profileBase.GrevillePoint(num11 - l);
					point3d3.X /= 5.0;
					point3d3.Y /= 2.0;
					Point3d point3d4 = point3d + vector3d3 * point3d3.X * MicroPaveProng.Cv[k, 0].Location.Y * width + vector3d2 * (point3d3.Y - num10);
					array[num4, num12] = new ControlPoint(point3d4.X, point3d4.Y, point3d4.Z, 1.0);
					num12++;
				}
				for (int m = 0; m < 15; m++)
				{
					double num13 = MicroPaveProng.Cv[k, m].Location.Z * prongHeight;
					Point3d point3d5 = point3d + vector3d3 * MicroPaveProng.Cv[k, m].Location.Y * width + vector3d2 * num13;
					array[num4, num12] = new ControlPoint(point3d5.X, point3d5.Y, point3d5.Z, 1.0);
					num12++;
				}
				for (int n = 0; n < profileBase.GrevillePoints().Count; n++)
				{
					Point3d point3d6 = profileBase.GrevillePoint(n);
					point3d6.X = (0.0 - point3d6.X) / 5.0;
					point3d6.Y /= 2.0;
					Point3d point3d7 = point3d + vector3d3 * point3d6.X * MicroPaveProng.Cv[k, 0].Location.Y * width + vector3d2 * (point3d6.Y - num10);
					array[num4, num12] = new ControlPoint(point3d7.X, point3d7.Y, point3d7.Z, 1.0);
					num12++;
				}
				num4++;
			}
		}
		double[] array2 = new double[num2 + 2];
		array2[0] = 0.0;
		array2[1] = 0.0;
		for (int num14 = 0; num14 < num2 - 2; num14++)
		{
			array2[num14 + 2] = num14;
		}
		array2[num2] = num2 - 3;
		array2[num2 + 1] = num2 - 3;
		double[] array3 = new double[num3 + 2];
		int num15 = profileBase.Knots.Count - 1;
		num12 = 0;
		for (int num16 = 0; num16 < profileBase.GrevillePoints().Count + 1; num16++)
		{
			array3[num12] = num5 - profileBase.Knots[num15 - num16];
			num12++;
		}
		for (int num17 = 0; num17 < 15; num17++)
		{
			array3[num12] = num5 + (double)num17;
			num12++;
		}
		double num18 = num5 + 15.0 - 1.0;
		for (int num19 = 0; num19 < profileBase.GrevillePoints().Count + 1; num19++)
		{
			array3[num12] = num18 + profileBase.Knots[num19 + 1];
			num12++;
		}
		srf = NurbsSurface.Create(3, isRational: false, 4, 4, num2, num3);
		for (int num20 = 0; num20 < array2.Length; num20++)
		{
			srf.KnotsU[num20] = array2[num20];
		}
		for (int num21 = 0; num21 < array3.Length; num21++)
		{
			srf.KnotsV[num21] = array3[num21];
		}
		for (int num22 = 0; num22 < num2; num22++)
		{
			for (int num23 = 0; num23 < num3; num23++)
			{
				srf.Points.SetControlPoint(num22, num23, array[num22, num23]);
			}
		}
		profileStart = new NurbsCurve(3, rational: false, 4, num3);
		profileEnd = new NurbsCurve(3, rational: false, 4, num3);
		for (int num24 = 0; num24 < array3.Length; num24++)
		{
			profileStart.Knots[num24] = array3[num24];
			profileEnd.Knots[num24] = array3[num24];
		}
		for (int num25 = 0; num25 < num3; num25++)
		{
			profileStart.Points.SetPoint(num25, array[0, num25].Location.X, array[0, num25].Location.Y, array[0, num25].Location.Z, array[0, num25].Weight);
			profileEnd.Points.SetPoint(num25, array[num2 - 1, num25].Location.X, array[num2 - 1, num25].Location.Y, array[num2 - 1, num25].Location.Z, array[num2 - 1, num25].Weight);
		}
		return true;
	}

	private bool GetWidthAtNormalizedParam(double s, out double width)
	{
		width = double.NaN;
		if (_lineCrv == null || _interpCrv == null || s < 0.0 || s > 1.0)
		{
			return false;
		}
		double t = double.NaN;
		if (!_lineCrv.NormalizedLengthParameter(s, out t))
		{
			return false;
		}
		Point3d other = _lineCrv.PointAt(t);
		other.Y = 0.0;
		other.Z = 0.0;
		Point2d point2d = new Point2d(other.X, 0.0);
		Point2d to = new Point2d(other.X, 50.0);
		LineCurve curveB = new LineCurve(point2d, to);
		CurveIntersections curveIntersections = Intersection.CurveCurve(_interpCrv, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections.Count <= 0)
		{
			return false;
		}
		double num = double.NaN;
		double num2 = double.NaN;
		for (int i = 0; i < curveIntersections.Count; i++)
		{
			double parameterA = curveIntersections[i].ParameterA;
			double num3 = _interpCrv.PointAt(parameterA).DistanceTo(other);
			if (double.IsNaN(num) || double.IsNaN(num2) || !(num3 >= num2))
			{
				num = parameterA;
				num2 = num3;
			}
		}
		Point3d point3d = _interpCrv.PointAt(num);
		width = point3d.Y;
		return point3d.IsValid;
	}
}
