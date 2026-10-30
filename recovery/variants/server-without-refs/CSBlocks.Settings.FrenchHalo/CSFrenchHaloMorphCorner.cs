using System.Collections.Generic;
using CS;
using CSBlocks.Shank.French;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings.FrenchHalo;

public static class CSFrenchHaloMorphCorner
{
	private sealed class CornerMorph : SpaceMorph
	{
		private readonly Point3d[] _crvPointsAUpperOut = (Point3d[])(object)new Point3d[101];

		private readonly Point3d[] _crvPointsAUpperIn = (Point3d[])(object)new Point3d[101];

		private readonly Point3d[] _crvPointsALowerOut = (Point3d[])(object)new Point3d[101];

		private readonly Point3d[] _crvPointsALowerIn = (Point3d[])(object)new Point3d[101];

		private readonly Point3d[] _crvPointsBUpperOut = (Point3d[])(object)new Point3d[101];

		private readonly Point3d[] _crvPointsBUpperIn = (Point3d[])(object)new Point3d[101];

		private readonly Point3d[] _crvPointsBLowerOut = (Point3d[])(object)new Point3d[101];

		private readonly Point3d[] _crvPointsBLowerIn = (Point3d[])(object)new Point3d[101];

		private readonly double _profileHeight;

		private readonly double _bottomSize;

		private readonly double _topSize;

		private readonly double _totalSize;

		private readonly double _basePer;

		private readonly double _midPer;

		private readonly double _topPer;

		private readonly bool _isSquished;

		private readonly double _scaleToGem;

		public NurbsSurface MainSrfTop;

		public NurbsSurface MainSrfBase;

		public double CxSize;

		public double CySize;

		public CSFrenchHaloHelper.FrenchObjectPart WhichPart;

		public CornerMorph(Curve crvSegAUpperOut, Curve crvSegAUpperIn, Curve crvSegALowerOut, Curve crvSegALowerIn, Curve crvSegBUpperOut, Curve crvSegBUpperIn, Curve crvSegBLowerOut, Curve crvSegBLowerIn, double profileHeight, double bottomSize, double midSize, double topSize, double gem0X)
		{
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
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
			_totalSize = _topSize + midSize + _bottomSize;
			_basePer = _bottomSize / _totalSize;
			_midPer = midSize / _totalSize;
			_topPer = _topSize / _totalSize;
			_isSquished = profileHeight < _totalSize * _scaleToGem;
		}

		public override Point3d MorphPoint(Point3d point)
		{
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			double zPer = ((Point3d)(ref point)).Z;
			CSFrenchHaloHelper.ConvZ(ref zPer, WhichPart, _bottomSize, _topSize, _profileHeight, _basePer, _midPer, _topPer, _scaleToGem, _totalSize);
			if (((Point3d)(ref point)).X > 1.0 && ((Point3d)(ref point)).Y < ((Point3d)(ref point)).X)
			{
				double num = (((Point3d)(ref point)).X - 1.0) / (CxSize - 1.0);
				int num2 = (int)(num * 100.0);
				int num3 = num2 + 1;
				Point3d val = PointInCurves(_crvPointsBUpperOut[num2], _crvPointsBUpperIn[num2], _crvPointsBLowerOut[num2], _crvPointsBLowerIn[num2], ((Point3d)(ref point)).Y, zPer);
				if (num2 == 100)
				{
					return val;
				}
				Point3d val2 = PointInCurves(_crvPointsBUpperOut[num3], _crvPointsBUpperIn[num3], _crvPointsBLowerOut[num3], _crvPointsBLowerIn[num3], ((Point3d)(ref point)).Y, zPer);
				double num4 = num * 100.0 - (double)num2;
				return val * (1.0 - num4) + val2 * num4;
			}
			if (((Point3d)(ref point)).Y > 1.0)
			{
				double num5 = (((Point3d)(ref point)).Y - 1.0) / (CySize - 1.0);
				int num6 = (int)(num5 * 100.0);
				int num7 = num6 + 1;
				Point3d val3 = PointInCurves(_crvPointsAUpperOut[num6], _crvPointsAUpperIn[num6], _crvPointsALowerOut[num6], _crvPointsALowerIn[num6], ((Point3d)(ref point)).X, zPer);
				if (num6 == 100)
				{
					return val3;
				}
				Point3d val4 = PointInCurves(_crvPointsAUpperOut[num7], _crvPointsAUpperIn[num7], _crvPointsALowerOut[num7], _crvPointsALowerIn[num7], ((Point3d)(ref point)).X, zPer);
				double num8 = num5 * 100.0 - (double)num6;
				return val3 * (1.0 - num8) + val4 * num8;
			}
			Interval val5 = ((Surface)MainSrfTop).Domain(0);
			Interval val6 = ((Surface)MainSrfTop).Domain(1);
			double num9 = ((Interval)(ref val5)).Min + ((Interval)(ref val5)).Length * (1.0 - ((Point3d)(ref point)).Y);
			double num10 = ((Interval)(ref val5)).Min + ((Interval)(ref val6)).Length * (1.0 - ((Point3d)(ref point)).X);
			Point3d val7 = ((Surface)MainSrfTop).PointAt(num9, num10);
			val5 = ((Surface)MainSrfBase).Domain(0);
			val6 = ((Surface)MainSrfBase).Domain(1);
			num9 = ((Interval)(ref val5)).Min + ((Interval)(ref val5)).Length * (1.0 - ((Point3d)(ref point)).Y);
			num10 = ((Interval)(ref val5)).Min + ((Interval)(ref val6)).Length * (1.0 - ((Point3d)(ref point)).X);
			Point3d val8 = ((Surface)MainSrfBase).PointAt(num9, num10);
			Vector3d val9 = val7 - val8;
			return val8 + val9 * zPer;
		}

		private static Point3d PointInCurves(Point3d crvUpperOut, Point3d crvUpperIn, Point3d crvLowerOut, Point3d crvLowerIn, double x, double y)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			Vector3d val = crvUpperOut - crvLowerOut;
			Vector3d val2 = crvUpperIn - crvLowerIn;
			Vector3d val3 = crvLowerIn - crvLowerOut;
			return crvLowerOut + val3 * x + val * y * (1.0 - x) + val2 * y * x;
		}
	}

	public static void DoCornerMorph(int i, CSChannelGemData[] gemDatas, CSChannelGemData[] gemDataUp, CSChannelGemData[] gemDataLower, CSFrenchGeometry cobj90, double cornerXSize, double cornerYSize, Curve[] rails, double leftCornerScale, double rightCornerScale, double wallWidth, double cornerSplitPercent, double profileHeight, double bottomSize, double midSize, double topSize, ref List<GeometryBase> nextm)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		int num = (i + 1) % gemDatas.Length;
		int num2 = (i - 1 + gemDatas.Length) % gemDatas.Length;
		Point3d val = CSFrenchHaloHelper.LeftPointAngled(gemDataUp[i], leftCornerScale, wallWidth);
		Point3d val2 = CSFrenchHaloHelper.RightPointAngled(gemDataUp[i], rightCornerScale, wallWidth);
		Point3d val3 = CSFrenchHaloHelper.LeftPointAngled(gemDataLower[i], leftCornerScale, wallWidth);
		Point3d val4 = CSFrenchHaloHelper.RightPointAngled(gemDataLower[i], rightCornerScale, wallWidth);
		Point3d p = CSFrenchHaloHelper.LeftPointAngled(gemDataUp[num], leftCornerScale, wallWidth);
		Point3d p2 = CSFrenchHaloHelper.RightPointAngled(gemDataUp[num], rightCornerScale, wallWidth);
		Point3d p3 = CSFrenchHaloHelper.LeftPointAngled(gemDataLower[num], leftCornerScale, wallWidth);
		Point3d p4 = CSFrenchHaloHelper.RightPointAngled(gemDataLower[num], rightCornerScale, wallWidth);
		Point3d p5 = CSFrenchHaloHelper.LeftPointAngled(gemDataUp[num2], leftCornerScale, wallWidth);
		Point3d p6 = CSFrenchHaloHelper.RightPointAngled(gemDataUp[num2], rightCornerScale, wallWidth);
		Point3d p7 = CSFrenchHaloHelper.LeftPointAngled(gemDataLower[num2], leftCornerScale, wallWidth);
		Point3d p8 = CSFrenchHaloHelper.RightPointAngled(gemDataLower[num2], rightCornerScale, wallWidth);
		Curve crv = CSFrenchHaloHelper.TrimCrvToPoints(rails[0], val, p);
		Curve crvSegAUpperIn = CSFrenchHaloHelper.TrimCrvToPoints(rails[1], val2, p2);
		Curve crv2 = CSFrenchHaloHelper.TrimCrvToPoints(rails[2], val3, p3);
		Curve crvSegALowerIn = CSFrenchHaloHelper.TrimCrvToPoints(rails[3], val4, p4);
		Curve crv3 = CSFrenchHaloHelper.TrimCrvToPoints(rails[0], p5, val);
		Curve crvSegBUpperIn = CSFrenchHaloHelper.TrimCrvToPoints(rails[1], p6, val2);
		Curve crv4 = CSFrenchHaloHelper.TrimCrvToPoints(rails[2], p7, val3);
		Curve crvSegBLowerIn = CSFrenchHaloHelper.TrimCrvToPoints(rails[3], p8, val4);
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
			cornerMorph.WhichPart = CSFrenchHaloHelper.FrenchObjectPart.Bot;
			GeometryBase val5 = cobj90.GBase.Duplicate();
			((SpaceMorph)cornerMorph).Morph(val5);
			nextm.Add(val5);
		}
		foreach (GeometryBase item in cobj90.GSide)
		{
			cornerMorph.WhichPart = CSFrenchHaloHelper.FrenchObjectPart.Side;
			GeometryBase val6 = item.Duplicate();
			((SpaceMorph)cornerMorph).Morph(val6);
			nextm.Add(val6);
		}
		if (cobj90.GTop != null)
		{
			cornerMorph.WhichPart = CSFrenchHaloHelper.FrenchObjectPart.Top;
			GeometryBase val7 = cobj90.GTop.Duplicate();
			((SpaceMorph)cornerMorph).Morph(val7);
			nextm.Add(val7);
		}
	}

	private static NurbsSurface GetSrf(Point3d baseP, Curve crv0, Curve crv1, double cornerSplitPercent)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		double num = default(double);
		crv0.NormalizedLengthParameter(cornerSplitPercent, ref num);
		Point3d val = crv0.PointAt(num);
		double num2 = default(double);
		crv1.NormalizedLengthParameter(1.0 - cornerSplitPercent, ref num2);
		Point3d val2 = crv1.PointAt(num2);
		LineCurve val3 = new LineCurve(new Line(baseP, val));
		Curve val4 = (Curve)new LineCurve(new Line(baseP, val2));
		double num3 = num;
		Interval domain = crv0.Domain;
		Curve val5 = crv0.Trim(num3, ((Interval)(ref domain)).Max);
		domain = crv1.Domain;
		Curve val6 = crv1.Trim(((Interval)(ref domain)).Min, num2);
		val6.Reverse();
		NurbsCurve obj = ((Curve)val3).Rebuild(5, 3, false);
		val4 = (Curve)(object)val4.Rebuild(5, 3, false);
		val5 = (Curve)(object)val5.Rebuild(5, 3, false);
		val6 = (Curve)(object)val6.Rebuild(5, 3, false);
		List<Curve> list = new List<Curve> { val4, val5 };
		return ((Surface)Brep.CreateFromSweep((Curve)(object)obj, val6, (IEnumerable<Curve>)list, false, CSApplicationHelper.Application.Tolerance)[0].Faces[0]).ToNurbsSurface();
	}
}
