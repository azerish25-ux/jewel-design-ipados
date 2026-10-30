using System;
using System.Collections.Generic;
using CS;
using CSBlocks.Shank.French;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings.FrenchHalo.Obsolete;

[Obsolete]
public static class CSFrenchHaloMorphCorner1
{
	private sealed class CornerMorph : SpaceMorph
	{
		private readonly Point3d[] _crvPointsAUpperOut = new Point3d[101];

		private readonly Point3d[] _crvPointsAUpperIn = new Point3d[101];

		private readonly Point3d[] _crvPointsALowerOut = new Point3d[101];

		private readonly Point3d[] _crvPointsALowerIn = new Point3d[101];

		private readonly Point3d[] _crvPointsBUpperOut = new Point3d[101];

		private readonly Point3d[] _crvPointsBUpperIn = new Point3d[101];

		private readonly Point3d[] _crvPointsBLowerOut = new Point3d[101];

		private readonly Point3d[] _crvPointsBLowerIn = new Point3d[101];

		private readonly double _profileHeight;

		private readonly double _bottomSize;

		private readonly double _topSize;

		private readonly double _basePer;

		private readonly double _midPer;

		private readonly double _topPer;

		private readonly bool _isSquished;

		private readonly double _scaleToGem;

		public NurbsSurface MainSrfTop;

		public NurbsSurface MainSrfBase;

		public double CxSize;

		public double CySize;

		public CSFrenchHaloHelper1.FrenchObjectPart WhichPart;

		public CornerMorph(Curve crvSegAUpperOut, Curve crvSegAUpperIn, Curve crvSegALowerOut, Curve crvSegALowerIn, Curve crvSegBUpperOut, Curve crvSegBUpperIn, Curve crvSegBLowerOut, Curve crvSegBLowerIn, double profileHeight, double bottomSize, double midSize, double topSize, double gem0X)
		{
			_scaleToGem = gem0X * 2.0;
			crvSegBUpperOut.Reverse();
			crvSegBUpperIn.Reverse();
			crvSegBLowerOut.Reverse();
			crvSegBLowerIn.Reverse();
			double[] array = new double[101];
			for (int i = 0; i <= 100; i++)
			{
				array[i] = (double)i / 100.0;
			}
			double[] array2 = crvSegAUpperOut.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			double[] array3 = crvSegAUpperIn.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			double[] array4 = crvSegALowerOut.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			double[] array5 = crvSegALowerIn.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			double[] array6 = crvSegBUpperOut.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			double[] array7 = crvSegBUpperIn.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			double[] array8 = crvSegBLowerOut.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			double[] array9 = crvSegBLowerIn.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			for (int j = 0; j <= 100; j++)
			{
				_crvPointsAUpperOut[j] = crvSegAUpperOut.PointAt(array2[j]);
				_crvPointsAUpperIn[j] = crvSegAUpperIn.PointAt(array3[j]);
				_crvPointsALowerOut[j] = crvSegALowerOut.PointAt(array4[j]);
				_crvPointsALowerIn[j] = crvSegALowerIn.PointAt(array5[j]);
				_crvPointsBUpperOut[j] = crvSegBUpperOut.PointAt(array6[j]);
				_crvPointsBUpperIn[j] = crvSegBUpperIn.PointAt(array7[j]);
				_crvPointsBLowerOut[j] = crvSegBLowerOut.PointAt(array8[j]);
				_crvPointsBLowerIn[j] = crvSegBLowerIn.PointAt(array9[j]);
			}
			_profileHeight = profileHeight;
			_bottomSize = bottomSize;
			_topSize = topSize;
			double num = _topSize + midSize + _bottomSize;
			_basePer = _bottomSize / num;
			_midPer = midSize / num;
			_topPer = _topSize / num;
			_isSquished = profileHeight < num * _scaleToGem;
		}

		public override Point3d MorphPoint(Point3d point)
		{
			double zPer = point.Z;
			if (!_isSquished)
			{
				CSFrenchHaloHelper1.ConvZ(ref zPer, WhichPart, _bottomSize, _topSize, _profileHeight, _basePer, _midPer, _topPer, _scaleToGem);
			}
			if (point.X > 1.0 && point.Y < point.X)
			{
				double num = (point.X - 1.0) / (CxSize - 1.0);
				int num2 = (int)(num * 100.0);
				int num3 = num2 + 1;
				Point3d point3d = PointInCurves(_crvPointsBUpperOut[num2], _crvPointsBUpperIn[num2], _crvPointsBLowerOut[num2], _crvPointsBLowerIn[num2], point.Y, zPer);
				if (num2 == 100)
				{
					return point3d;
				}
				Point3d point3d2 = PointInCurves(_crvPointsBUpperOut[num3], _crvPointsBUpperIn[num3], _crvPointsBLowerOut[num3], _crvPointsBLowerIn[num3], point.Y, zPer);
				double num4 = num * 100.0 - (double)num2;
				return point3d * (1.0 - num4) + point3d2 * num4;
			}
			if (point.Y > 1.0)
			{
				double num5 = (point.Y - 1.0) / (CySize - 1.0);
				int num6 = (int)(num5 * 100.0);
				int num7 = num6 + 1;
				Point3d point3d3 = PointInCurves(_crvPointsAUpperOut[num6], _crvPointsAUpperIn[num6], _crvPointsALowerOut[num6], _crvPointsALowerIn[num6], point.X, zPer);
				if (num6 == 100)
				{
					return point3d3;
				}
				Point3d point3d4 = PointInCurves(_crvPointsAUpperOut[num7], _crvPointsAUpperIn[num7], _crvPointsALowerOut[num7], _crvPointsALowerIn[num7], point.X, zPer);
				double num8 = num5 * 100.0 - (double)num6;
				return point3d3 * (1.0 - num8) + point3d4 * num8;
			}
			Interval interval = MainSrfTop.Domain(0);
			Interval interval2 = MainSrfTop.Domain(1);
			double u = interval.Min + interval.Length * (1.0 - point.Y);
			double v = interval.Min + interval2.Length * (1.0 - point.X);
			Point3d point3d5 = MainSrfTop.PointAt(u, v);
			interval = MainSrfBase.Domain(0);
			interval2 = MainSrfBase.Domain(1);
			u = interval.Min + interval.Length * (1.0 - point.Y);
			v = interval.Min + interval2.Length * (1.0 - point.X);
			Point3d point3d6 = MainSrfBase.PointAt(u, v);
			Vector3d vector3d = point3d5 - point3d6;
			return point3d6 + vector3d * zPer;
		}

		private static Point3d PointInCurves(Point3d crvUpperOut, Point3d crvUpperIn, Point3d crvLowerOut, Point3d crvLowerIn, double x, double y)
		{
			Vector3d vector3d = crvUpperOut - crvLowerOut;
			Vector3d vector3d2 = crvUpperIn - crvLowerIn;
			Vector3d vector3d3 = crvLowerIn - crvLowerOut;
			return crvLowerOut + vector3d3 * x + vector3d * y * (1.0 - x) + vector3d2 * y * x;
		}
	}

	public static void DoCornerMorph(int i, CSChannelGemData[] gemDatas, CSChannelGemData[] gemDataUp, CSChannelGemData[] gemDataLower, CSFrenchGeometry cobj90, double cornerXSize, double cornerYSize, Curve[] rails, double leftCornerScale, double rightCornerScale, double wallWidth, double cornerSplitPercent, double profileHeight, double bottomSize, double midSize, double topSize, ref List<GeometryBase> nextm)
	{
		int num = (i + 1) % gemDatas.Length;
		int num2 = (i - 1 + gemDatas.Length) % gemDatas.Length;
		Point3d point3d = CSFrenchHaloHelper.LeftPointAngled(gemDataUp[i], leftCornerScale, wallWidth);
		Point3d point3d2 = CSFrenchHaloHelper.RightPointAngled(gemDataUp[i], rightCornerScale, wallWidth);
		Point3d point3d3 = CSFrenchHaloHelper.LeftPointAngled(gemDataLower[i], leftCornerScale, wallWidth);
		Point3d point3d4 = CSFrenchHaloHelper.RightPointAngled(gemDataLower[i], rightCornerScale, wallWidth);
		Point3d p = CSFrenchHaloHelper.LeftPointAngled(gemDataUp[num], leftCornerScale, wallWidth);
		Point3d p2 = CSFrenchHaloHelper.RightPointAngled(gemDataUp[num], rightCornerScale, wallWidth);
		Point3d p3 = CSFrenchHaloHelper.LeftPointAngled(gemDataLower[num], leftCornerScale, wallWidth);
		Point3d p4 = CSFrenchHaloHelper.RightPointAngled(gemDataLower[num], rightCornerScale, wallWidth);
		Point3d p5 = CSFrenchHaloHelper.LeftPointAngled(gemDataUp[num2], leftCornerScale, wallWidth);
		Point3d p6 = CSFrenchHaloHelper.RightPointAngled(gemDataUp[num2], rightCornerScale, wallWidth);
		Point3d p7 = CSFrenchHaloHelper.LeftPointAngled(gemDataLower[num2], leftCornerScale, wallWidth);
		Point3d p8 = CSFrenchHaloHelper.RightPointAngled(gemDataLower[num2], rightCornerScale, wallWidth);
		Curve crv = CSFrenchHaloHelper.TrimCrvToPoints(rails[0], point3d, p);
		Curve crvSegAUpperIn = CSFrenchHaloHelper.TrimCrvToPoints(rails[1], point3d2, p2);
		Curve crv2 = CSFrenchHaloHelper.TrimCrvToPoints(rails[2], point3d3, p3);
		Curve crvSegALowerIn = CSFrenchHaloHelper.TrimCrvToPoints(rails[3], point3d4, p4);
		Curve crv3 = CSFrenchHaloHelper.TrimCrvToPoints(rails[0], p5, point3d);
		Curve crvSegBUpperIn = CSFrenchHaloHelper.TrimCrvToPoints(rails[1], p6, point3d2);
		Curve crv4 = CSFrenchHaloHelper.TrimCrvToPoints(rails[2], p7, point3d3);
		Curve crvSegBLowerIn = CSFrenchHaloHelper.TrimCrvToPoints(rails[3], p8, point3d4);
		NurbsSurface srf = GetSrf(CSFrenchHaloHelper.RightPointAngled(gemDataUp[i], rightCornerScale, wallWidth), crv3, crv, cornerSplitPercent);
		NurbsSurface srf2 = GetSrf(CSFrenchHaloHelper.RightPointAngled(gemDataLower[i], rightCornerScale, wallWidth), crv4, crv2, cornerSplitPercent);
		CSFrenchHaloHelper.TrimCrv(ref crv3, ref crv, cornerSplitPercent);
		CSFrenchHaloHelper.TrimCrv(ref crv4, ref crv2, cornerSplitPercent);
		CornerMorph cornerMorph = new CornerMorph(crv, crvSegAUpperIn, crv2, crvSegALowerIn, crv3, crvSegBUpperIn, crv4, crvSegBLowerIn, profileHeight, bottomSize, midSize, topSize, gemDataUp[i].GemYMax)
		{
			MainSrfBase = srf2,
			MainSrfTop = srf,
			CxSize = cornerXSize,
			CySize = cornerYSize
		};
		if (cobj90.GBase != null)
		{
			cornerMorph.WhichPart = CSFrenchHaloHelper1.FrenchObjectPart.Bot;
			GeometryBase geometryBase = cobj90.GBase.Duplicate();
			cornerMorph.Morph(geometryBase);
			nextm.Add(geometryBase);
		}
		foreach (GeometryBase item in cobj90.GSide)
		{
			cornerMorph.WhichPart = CSFrenchHaloHelper1.FrenchObjectPart.Side;
			GeometryBase geometryBase2 = item.Duplicate();
			cornerMorph.Morph(geometryBase2);
			nextm.Add(geometryBase2);
		}
		if (cobj90.GTop != null)
		{
			cornerMorph.WhichPart = CSFrenchHaloHelper1.FrenchObjectPart.Top;
			GeometryBase geometryBase3 = cobj90.GTop.Duplicate();
			cornerMorph.Morph(geometryBase3);
			nextm.Add(geometryBase3);
		}
	}

	private static NurbsSurface GetSrf(Point3d baseP, Curve crv0, Curve crv1, double cornerSplitPercent)
	{
		crv0.NormalizedLengthParameter(cornerSplitPercent, out var t);
		Point3d to = crv0.PointAt(t);
		crv1.NormalizedLengthParameter(1.0 - cornerSplitPercent, out var t2);
		Point3d to2 = crv1.PointAt(t2);
		LineCurve lineCurve = new LineCurve(new Line(baseP, to));
		Curve curve = new LineCurve(new Line(baseP, to2));
		Curve curve2 = crv0.Trim(t, crv0.Domain.Max);
		Curve curve3 = crv1.Trim(crv1.Domain.Min, t2);
		curve3.Reverse();
		NurbsCurve rail = lineCurve.Rebuild(5, 3, preserveTangents: false);
		curve = curve.Rebuild(5, 3, preserveTangents: false);
		curve2 = curve2.Rebuild(5, 3, preserveTangents: false);
		curve3 = curve3.Rebuild(5, 3, preserveTangents: false);
		List<Curve> shapes = new List<Curve> { curve, curve2 };
		return Brep.CreateFromSweep(rail, curve3, shapes, closed: false, CSApplicationHelper.Application.Tolerance)[0].Faces[0].ToNurbsSurface();
	}
}
