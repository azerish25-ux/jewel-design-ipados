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
		private readonly Point3d[] _crvUpperOut = (Point3d[])(object)new Point3d[101];

		private readonly Point3d[] _crvUpperIn = (Point3d[])(object)new Point3d[101];

		private readonly Point3d[] _crvLowerOut = (Point3d[])(object)new Point3d[101];

		private readonly Point3d[] _crvLowerIn = (Point3d[])(object)new Point3d[101];

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
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Expected O, but got Unknown
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
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
				double num = (spacing - gem1X + gem0X) / 2.0 / spacing;
				Curve val = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)new List<Point3d>
				{
					new Point3d(0.0, 0.0, 0.0),
					new Point3d(0.01, 0.01, 0.0),
					new Point3d(0.5, num, 0.0),
					new Point3d(0.99, 0.99, 0.0),
					new Point3d(1.0, 1.0, 0.0)
				}, 3);
				for (int j = 0; j <= 100; j++)
				{
					CurveIntersections val2 = Intersection.CurveCurve((Curve)new LineCurve(new Line(new Point3d((double)j / 100.0, 0.0, 0.0), new Point3d((double)j / 100.0, 1.0, 0.0))), val, 0.001, 0.001);
					if (val2.Count == 1)
					{
						int num2 = j;
						Point3d pointA = val2[0].PointA;
						array[num2] = ((Point3d)(ref pointA)).Y;
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
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			double x = ((Point3d)(ref point)).X;
			int num = (int)(x * 100.0);
			Point3d origin = Point3d.Origin;
			double zPer = ((Point3d)(ref point)).Z;
			CSFrenchHaloHelper.ConvZ(ref zPer, WhichPart, _bottomSize, _topSize, _profileHeight, _basePer, _midPer, _topPer, scaleToGem, _totalSize);
			origin = PointInCurves(num, ((Point3d)(ref point)).Y, zPer);
			if (num == 100)
			{
				return origin;
			}
			Point3d origin2 = Point3d.Origin;
			double num2 = x * 100.0 - (double)num;
			origin2 = PointInCurves(num + 1, ((Point3d)(ref point)).Y, zPer);
			return origin * (1.0 - num2) + origin2 * num2;
		}

		private Point3d PointInCurves(int aVal, double x, double y)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			Vector3d val = _crvUpperIn[aVal] - _crvLowerIn[aVal];
			Vector3d val2 = _crvUpperOut[aVal] - _crvLowerOut[aVal];
			Vector3d val3 = _crvLowerOut[aVal] - _crvLowerIn[aVal];
			return _crvLowerIn[aVal] + val3 * x + val * y * (1.0 - x) + val2 * y * x;
		}
	}

	public static void DoMorph(int i, CSChannelGemData[] gemDatas, CSChannelGemData[] gemDataUp, CSChannelGemData[] gemDataLower, CSFrenchGeometry cobjStraight, Curve[] rails, double leftCornerScale, double rightCornerScale, double wallWidth, double profileHeight, double bottomSize, double midSize, double topSize, ref List<GeometryBase> nextm)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
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
		Vector3d val = gemDataUp[i].Center - gemDataUp[num].Center;
		double length = ((Vector3d)(ref val)).Length;
		FrenchHaloMorph frenchHaloMorph = new FrenchHaloMorph(crvSegUpperOut, crvSegUpperIn, crvSegLowerOut, crvSegLowerIn, profileHeight, bottomSize, midSize, topSize, length, gemDataUp[i].GemXMax, gemDataUp[num].GemXMax);
		frenchHaloMorph.WhichPart = CSFrenchHaloHelper.FrenchObjectPart.Bot;
		GeometryBase val2 = cobjStraight.GBase.Duplicate();
		((SpaceMorph)frenchHaloMorph).Morph(val2);
		nextm.Add(val2);
		frenchHaloMorph.WhichPart = CSFrenchHaloHelper.FrenchObjectPart.Side;
		foreach (GeometryBase item in cobjStraight.GSide)
		{
			GeometryBase val3 = item.Duplicate();
			((SpaceMorph)frenchHaloMorph).Morph(val3);
			nextm.Add(val3);
		}
		frenchHaloMorph.WhichPart = CSFrenchHaloHelper.FrenchObjectPart.Top;
		GeometryBase val4 = cobjStraight.GTop.Duplicate();
		((SpaceMorph)frenchHaloMorph).Morph(val4);
		nextm.Add(val4);
	}
}
