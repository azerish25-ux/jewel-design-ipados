using System;
using System.Collections.Generic;
using CS;
using CSBlocks.Shank.French;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.FrenchHalo;

public static class CSFrenchHaloMorph
{
	private sealed class FrenchHaloMorph : SpaceMorph
	{
		private readonly Point3d[] _crvUpperOut = new Point3d[101];

		private readonly Point3d[] _crvUpperIn = new Point3d[101];

		private readonly Point3d[] _crvLowerOut = new Point3d[101];

		private readonly Point3d[] _crvLowerIn = new Point3d[101];

		private readonly double _profileHeight;

		private readonly double _bottomSize;

		private readonly double _topSize;

		private readonly double _totalSize;

		private readonly double _basePer;

		private readonly double _midPer;

		private readonly double _topPer;

		private readonly bool _isSquished;

		private readonly double scaleToGem;

		public CSFrenchHaloHelper.FrenchObjectPart WhichPart;

		public FrenchHaloMorph(Curve crvSegUpperOut, Curve crvSegUpperIn, Curve crvSegLowerOut, Curve crvSegLowerIn, double profileHeight, double bottomSize, double midSize, double topSize, double spacing, double gem0X, double gem1X)
		{
			scaleToGem = Math.Max(gem0X, gem1X) * 2.0;
			double[] array = new double[101];
			if (Math.Abs(gem0X - gem1X) < 0.01)
			{
				for (int i = 0; i <= 100; i++)
				{
					array[i] = (double)i / 100.0;
				}
			}
			else
			{
				double y = (spacing - gem1X + gem0X) / 2.0 / spacing;
				Curve curveB = Curve.CreateInterpolatedCurve(new List<Point3d>
				{
					new Point3d(0.0, 0.0, 0.0),
					new Point3d(0.01, 0.01, 0.0),
					new Point3d(0.5, y, 0.0),
					new Point3d(0.99, 0.99, 0.0),
					new Point3d(1.0, 1.0, 0.0)
				}, 3);
				for (int j = 0; j <= 100; j++)
				{
					CurveIntersections curveIntersections = Intersection.CurveCurve(new LineCurve(new Line(new Point3d((double)j / 100.0, 0.0, 0.0), new Point3d((double)j / 100.0, 1.0, 0.0))), curveB, 0.001, 0.001);
					if (curveIntersections.Count == 1)
					{
						array[j] = curveIntersections[0].PointA.Y;
					}
				}
				array[0] = 0.0;
				array[100] = 1.0;
			}
			double[] array2 = crvSegUpperOut.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			double[] array3 = crvSegUpperIn.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			double[] array4 = crvSegLowerOut.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			double[] array5 = crvSegLowerIn.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			for (int k = 0; k <= 100; k++)
			{
				_crvUpperOut[k] = crvSegUpperOut.PointAt(array2[k]);
				_crvUpperIn[k] = crvSegUpperIn.PointAt(array3[k]);
				_crvLowerOut[k] = crvSegLowerOut.PointAt(array4[k]);
				_crvLowerIn[k] = crvSegLowerIn.PointAt(array5[k]);
			}
			_profileHeight = profileHeight;
			_bottomSize = bottomSize;
			_topSize = topSize;
			_totalSize = _topSize + midSize + _bottomSize;
			_basePer = _bottomSize / _totalSize;
			_midPer = midSize / _totalSize;
			_topPer = _topSize / _totalSize;
			_isSquished = profileHeight < _totalSize * scaleToGem;
		}

		public override Point3d MorphPoint(Point3d point)
		{
			double x = point.X;
			int num = (int)(x * 100.0);
			Point3d origin = Point3d.Origin;
			double zPer = point.Z;
			CSFrenchHaloHelper.ConvZ(ref zPer, WhichPart, _bottomSize, _topSize, _profileHeight, _basePer, _midPer, _topPer, scaleToGem, _totalSize);
			origin = PointInCurves(num, point.Y, zPer);
			if (num == 100)
			{
				return origin;
			}
			Point3d origin2 = Point3d.Origin;
			double num2 = x * 100.0 - (double)num;
			origin2 = PointInCurves(num + 1, point.Y, zPer);
			return origin * (1.0 - num2) + origin2 * num2;
		}

		private Point3d PointInCurves(int aVal, double x, double y)
		{
			Vector3d vector3d = _crvUpperIn[aVal] - _crvLowerIn[aVal];
			Vector3d vector3d2 = _crvUpperOut[aVal] - _crvLowerOut[aVal];
			Vector3d vector3d3 = _crvLowerOut[aVal] - _crvLowerIn[aVal];
			return _crvLowerIn[aVal] + vector3d3 * x + vector3d * y * (1.0 - x) + vector3d2 * y * x;
		}
	}

	public static void DoMorph(int i, CSChannelGemData[] gemDatas, CSChannelGemData[] gemDataUp, CSChannelGemData[] gemDataLower, CSFrenchGeometry cobjStraight, Curve[] rails, double leftCornerScale, double rightCornerScale, double wallWidth, double profileHeight, double bottomSize, double midSize, double topSize, ref List<GeometryBase> nextm)
	{
		int num = (i + 1) % gemDatas.Length;
		Point3d p = CSFrenchHaloHelper.LeftPointAngled(gemDataUp[i], leftCornerScale, wallWidth);
		Point3d p2 = CSFrenchHaloHelper.RightPointAngled(gemDataUp[i], rightCornerScale, wallWidth);
		Point3d p3 = CSFrenchHaloHelper.LeftPointAngled(gemDataLower[i], leftCornerScale, wallWidth);
		Point3d p4 = CSFrenchHaloHelper.RightPointAngled(gemDataLower[i], rightCornerScale, wallWidth);
		Point3d p5 = CSFrenchHaloHelper.LeftPointAngled(gemDataUp[num], leftCornerScale, wallWidth);
		Point3d p6 = CSFrenchHaloHelper.RightPointAngled(gemDataUp[num], rightCornerScale, wallWidth);
		Point3d p7 = CSFrenchHaloHelper.LeftPointAngled(gemDataLower[num], leftCornerScale, wallWidth);
		Point3d p8 = CSFrenchHaloHelper.RightPointAngled(gemDataLower[num], rightCornerScale, wallWidth);
		Curve crvSegUpperOut = CSFrenchHaloHelper.TrimCrvToPoints(rails[0], p, p5);
		Curve crvSegUpperIn = CSFrenchHaloHelper.TrimCrvToPoints(rails[1], p2, p6);
		Curve crvSegLowerOut = CSFrenchHaloHelper.TrimCrvToPoints(rails[2], p3, p7);
		Curve crvSegLowerIn = CSFrenchHaloHelper.TrimCrvToPoints(rails[3], p4, p8);
		double length = (gemDataUp[i].Center - gemDataUp[num].Center).Length;
		FrenchHaloMorph frenchHaloMorph = new FrenchHaloMorph(crvSegUpperOut, crvSegUpperIn, crvSegLowerOut, crvSegLowerIn, profileHeight, bottomSize, midSize, topSize, length, gemDataUp[i].GemXMax, gemDataUp[num].GemXMax);
		frenchHaloMorph.WhichPart = CSFrenchHaloHelper.FrenchObjectPart.Bot;
		GeometryBase geometryBase = cobjStraight.GBase.Duplicate();
		frenchHaloMorph.Morph(geometryBase);
		nextm.Add(geometryBase);
		frenchHaloMorph.WhichPart = CSFrenchHaloHelper.FrenchObjectPart.Side;
		foreach (GeometryBase item in cobjStraight.GSide)
		{
			GeometryBase geometryBase2 = item.Duplicate();
			frenchHaloMorph.Morph(geometryBase2);
			nextm.Add(geometryBase2);
		}
		frenchHaloMorph.WhichPart = CSFrenchHaloHelper.FrenchObjectPart.Top;
		GeometryBase geometryBase3 = cobjStraight.GTop.Duplicate();
		frenchHaloMorph.Morph(geometryBase3);
		nextm.Add(geometryBase3);
	}
}
