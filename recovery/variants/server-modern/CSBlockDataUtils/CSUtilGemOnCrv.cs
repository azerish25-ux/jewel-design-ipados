using System;
using System.Collections.Generic;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using Rhino;
using Rhino.Geometry;

namespace CSBlockDataUtils;

public static class CSUtilGemOnCrv
{
	public struct CurveInfo
	{
		private IOn3dPoint _pt;

		private IOn3dVector _tan;

		private Point3d _ptRc;

		private Vector3d _tanRc;

		public IOn3dPoint Pt
		{
			get
			{
				return _pt;
			}
			set
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				_pt = value;
				_ptRc = CSConvert.On3dPointCopyToPoint3d(_pt);
			}
		}

		public IOn3dVector Tan
		{
			get
			{
				return _tan;
			}
			set
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				_tan = value;
				_tanRc = CSConvert.On3dVectorCopyToVector3d(_tan);
			}
		}

		public Point3d PtRc
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _ptRc;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				_ptRc = value;
				_pt = (IOn3dPoint)(object)CSConvert.Point3dCopyToOn3dPoint(_ptRc);
			}
		}

		public Vector3d TanRc
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _tanRc;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				_tanRc = value;
				_tan = (IOn3dVector)(object)CSConvert.Vector3dCopyToOn3dVector(_tanRc);
			}
		}

		public double Parameter { get; set; }
	}

	public struct GemOnCurveInfo
	{
		public double Radius { get; set; }

		public double Height { get; set; }

		public double RadiusAdjusted { get; set; }

		public double Placement { get; set; }

		public int Index { get; set; }
	}

	public static OnXform CalculateGemXform(IList<CurveInfo> crvInfos, GemOnCurveInfo gemOnCurveInfo, IOnBrep srf, CSGemData sourceGem, double roll, double tilt, ValueListItem whichOrientationMethod, ValueListItem whichPlacementMethod, bool doFlip)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		double num = gemOnCurveInfo.Radius / sourceGem.GemXMax;
		OnXform val = new OnXform();
		if (gemOnCurveInfo.Height > 0.0)
		{
			double num2 = gemOnCurveInfo.Height / sourceGem.GemYSize;
			val.Scale(num, num2, num);
		}
		else
		{
			val.Scale(num, num, num);
		}
		OnXform val2 = new OnXform();
		val2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(roll), OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform val3 = new OnXform();
		val3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(tilt), OnUtil.On_xaxis, OnUtil.On_origin);
		CSGemData cSGemData = new CSGemData(sourceGem);
		cSGemData.ApplyTransform(val.Convert());
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, whichPlacementMethod, ref zOffset);
		OnXform val4 = new OnXform();
		val4.Translation(0.0, 0.0, zOffset + gemOnCurveInfo.Placement);
		int fixedIndex = GetFixedIndex(crvInfos, gemOnCurveInfo.Index);
		IOn3dPoint pt = crvInfos[fixedIndex].Pt;
		IOn3dVector tan = crvInfos[fixedIndex].Tan;
		On3dVector val5 = (doFlip ? new On3dVector(0.0, 0.0, -1.0) : new On3dVector(0.0, 0.0, 1.0));
		On3dVector val6 = (doFlip ? new On3dVector(-1.0, 0.0, 0.0) : new On3dVector(1.0, 0.0, 0.0));
		OnXform val7;
		switch (whichOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			CSUtilGeometry.GetClosestNormalToBrep(srf, pt, ref destination);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val5, (IOn3dVector)(object)val6, pt, (IOn3dVector)(object)destination, tan);
			break;
		}
		case ValueListItem.GemOrientationMethodOrigin:
		{
			On3dVector destination = new On3dVector(pt);
			destination.Unitize();
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val6, (IOn3dVector)(object)val5, pt, tan, (IOn3dVector)(object)destination);
			break;
		}
		case ValueListItem.GemOrientationMethodAxisY:
		{
			On3dVector destination = new On3dVector(pt.x, 0.0, pt.z);
			destination.Unitize();
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val6, (IOn3dVector)(object)val5, pt, tan, (IOn3dVector)(object)destination);
			break;
		}
		case ValueListItem.GemOrientationMethodDown:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val5, (IOn3dVector)(object)val6, pt, (IOn3dVector)(object)destination, tan);
			break;
		}
		default:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val5, (IOn3dVector)(object)val6, pt, (IOn3dVector)(object)destination, tan);
			break;
		}
		}
		return val7 * (IOnXform)(object)val4 * (IOnXform)(object)val2 * (IOnXform)(object)val3 * (IOnXform)(object)val;
	}

	public static Transform CalculateGemXform(IList<CurveInfo> crvInfos, GemOnCurveInfo gemOnCurveInfo, Brep srf, CSGemData sourceGem, double roll, double tilt, ValueListItem whichOrientationMethod, ValueListItem whichPlacementMethod, bool doFlip)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		double num = gemOnCurveInfo.Radius / sourceGem.GemXMax;
		Transform val;
		if (gemOnCurveInfo.Height > 0.0)
		{
			double num2 = gemOnCurveInfo.Height / sourceGem.GemYSize;
			val = Transform.Scale(Plane.WorldXY, num, num2, num);
		}
		else
		{
			val = Transform.Scale(Plane.WorldXY, num, num, num);
		}
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(roll), Vector3d.ZAxis, Point3d.Origin);
		Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(tilt), Vector3d.XAxis, Point3d.Origin);
		CSGemData cSGemData = new CSGemData(sourceGem);
		cSGemData.ApplyTransform(val);
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, whichPlacementMethod, ref zOffset);
		Transform val4 = Transform.Translation(0.0, 0.0, zOffset + gemOnCurveInfo.Placement);
		int fixedIndex = GetFixedIndex(crvInfos, gemOnCurveInfo.Index);
		Point3d ptRc = crvInfos[fixedIndex].PtRc;
		Vector3d tanRc = crvInfos[fixedIndex].TanRc;
		Vector3d val5 = (doFlip ? new Vector3d(0.0, 0.0, -1.0) : new Vector3d(0.0, 0.0, 1.0));
		Vector3d val6 = (doFlip ? new Vector3d(-1.0, 0.0, 0.0) : new Vector3d(1.0, 0.0, 0.0));
		Vector3d brepNormal = default(Vector3d);
		Transform val7;
		switch (whichOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
			brepNormal = Vector3d.ZAxis;
			CSUtilGeometry.GetClosestNormalToBrep(srf, ptRc, ref brepNormal);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val5, val6, ptRc, brepNormal, tanRc);
			break;
		case ValueListItem.GemOrientationMethodOrigin:
			((Vector3d)(ref brepNormal))._002Ector(ptRc);
			((Vector3d)(ref brepNormal)).Unitize();
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val6, val5, ptRc, tanRc, brepNormal);
			break;
		case ValueListItem.GemOrientationMethodAxisY:
			((Vector3d)(ref brepNormal))._002Ector(((Point3d)(ref ptRc)).X, 0.0, ((Point3d)(ref ptRc)).Z);
			((Vector3d)(ref brepNormal)).Unitize();
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val6, val5, ptRc, tanRc, brepNormal);
			break;
		case ValueListItem.GemOrientationMethodDown:
			brepNormal = Vector3d.ZAxis;
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val5, val6, ptRc, brepNormal, tanRc);
			break;
		default:
			brepNormal = Vector3d.ZAxis;
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val5, val6, ptRc, brepNormal, tanRc);
			break;
		}
		return val7 * val4 * val2 * val3 * val;
	}

	public static bool CalculateTestGem(CSGemData sourceGem, double diameter, OnXform xform, ref double radius, ref double rotationalAdjustmentFactor)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if (!CSUtilGem.GetGemScaleFromTableX(sourceGem, diameter, true, false, out OnXform xBestScale))
		{
			return false;
		}
		CSGemData cSGemData = new CSGemData(sourceGem);
		cSGemData.ApplyTransform(xform.Convert() * xBestScale.Convert());
		radius = cSGemData.GemXSize / 2.0;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox))
		{
			return false;
		}
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		rotationalAdjustmentFactor = (x - ((Point3d)(ref val)).X) / cSGemData.GemXSize;
		return true;
	}

	public static bool CalculateTestGem(CSGemData sourceGem, double diameter, Transform xform, ref double radius, ref double rotationalAdjustmentFactor)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		if (!CSUtilGem.GetGemScaleFromTableX(sourceGem, diameter, true, false, out Transform xBestScale))
		{
			return false;
		}
		CSGemData cSGemData = new CSGemData(sourceGem);
		cSGemData.ApplyTransform(xform * xBestScale);
		radius = cSGemData.GemXSize / 2.0;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox))
		{
			return false;
		}
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		rotationalAdjustmentFactor = (x - ((Point3d)(ref val)).X) / cSGemData.GemXSize;
		return true;
	}

	public static bool GetClosestIndexToPosition(IOnCurve crv, IList<CurveInfo> crvInfos, double pos, out int index)
	{
		index = -1;
		if (crv == null)
		{
			return false;
		}
		if (crvInfos == null)
		{
			return false;
		}
		if (crvInfos.Count <= 0)
		{
			return false;
		}
		double num = crv.Domain().ParameterAt(pos);
		double num2 = -1.23432101234321E+308;
		for (int i = 0; i < crvInfos.Count; i++)
		{
			double num3 = Math.Abs(num - crvInfos[i].Parameter);
			if (!RhinoMath.IsValidDouble(num2) || !(num2 < num3))
			{
				num2 = num3;
				index = i;
			}
		}
		return true;
	}

	public static bool GetClosestIndexToPosition(Curve crv, IList<CurveInfo> crvInfos, double pos, out int index)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		index = -1;
		if (crv == null)
		{
			return false;
		}
		if (crvInfos == null)
		{
			return false;
		}
		if (crvInfos.Count <= 0)
		{
			return false;
		}
		Interval domain = crv.Domain;
		double num = ((Interval)(ref domain)).ParameterAt(pos);
		double num2 = -1.23432101234321E+308;
		for (int i = 0; i < crvInfos.Count; i++)
		{
			double num3 = Math.Abs(num - crvInfos[i].Parameter);
			if (!RhinoMath.IsValidDouble(num2) || !(num2 < num3))
			{
				num2 = num3;
				index = i;
			}
		}
		return true;
	}

	public static bool GetDiameterFromTable(CSGemData sourceGem, double diameterRequested, out double diameterFromTable)
	{
		diameterFromTable = -1.23432101234321E+308;
		if (!CSUtilGem.GetGemSizeFromTableX(sourceGem, diameterRequested, useLargestPossible: true, useIterationOnly: false, out var bestSize))
		{
			return false;
		}
		diameterFromTable = bestSize.X;
		return true;
	}

	public static int GetFixedIndex(IList<CurveInfo> crvInfos, int index)
	{
		int i = index;
		if (crvInfos.Count == 0)
		{
			return 0;
		}
		for (; i < 0; i += crvInfos.Count)
		{
		}
		return i % crvInfos.Count;
	}

	public static bool GetWidthHeightFromTable(CSGemData sourceGem, double widthRequested, double heightRequested, out double widthFromTable, out double heightFromTable)
	{
		widthFromTable = -1.23432101234321E+308;
		heightFromTable = -1.23432101234321E+308;
		if (!CSUtilGem.GetGemSizeFromTable(sourceGem, widthRequested, heightRequested, useLargestPossible: true, useIterationOnly: false, out var bestSize))
		{
			return false;
		}
		widthFromTable = bestSize.X;
		heightFromTable = bestSize.Y;
		return true;
	}

	public static bool PreProcessCrv(IOnCurve crv, out IList<CurveInfo> crvInfos)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0147: Expected O, but got Unknown
		crvInfos = new List<CurveInfo>();
		if (crv == null)
		{
			return false;
		}
		double num = 0.0;
		if (!crv.GetLength(ref num))
		{
			return false;
		}
		int num2 = (int)(num * 50.0);
		int num3 = num2 + 1;
		double[] array = new double[num3];
		List<double> list = new List<double>();
		for (double num4 = 0.0; num4 < (double)num3; num4 += 1.0)
		{
			list.Add(num4 / (double)num2);
		}
		if (!crv.GetNormalizedArcLengthPoints(list.ToArray(), ref array))
		{
			return false;
		}
		crvInfos.Clear();
		double[] array2 = array;
		foreach (double num5 in array2)
		{
			On3dPoint pt = new On3dPoint();
			On3dVector val = new On3dVector();
			if (crv.Ev1Der(num5, ref pt, ref val))
			{
				val.Unitize();
				CurveInfo item = new CurveInfo
				{
					Parameter = num5,
					Pt = (IOn3dPoint)(object)pt,
					Tan = (IOn3dVector)(object)val
				};
				crvInfos.Add(item);
			}
		}
		if (crvInfos.Count > 2 && crv.IsClosed())
		{
			CurveInfo value = crvInfos[0];
			CurveInfo value2 = crvInfos[crvInfos.Count - 1];
			On3dVector val2 = (new On3dVector(value.Tan) + (IOn3dVector)new On3dVector(value2.Tan)) / 2.0;
			val2.Unitize();
			value.Tan = (IOn3dVector)(object)val2;
			value2.Tan = (IOn3dVector)(object)val2;
			crvInfos[0] = value;
			crvInfos[crvInfos.Count - 1] = value2;
		}
		return true;
	}

	public static bool PreProcessCrv(Curve crv, out IList<CurveInfo> crvInfos)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		crvInfos = new List<CurveInfo>();
		if (crv == null)
		{
			return false;
		}
		double length = crv.GetLength();
		if (double.IsNaN(length))
		{
			return false;
		}
		int num = (int)(length * 50.0);
		int num2 = num + 1;
		List<double> list = new List<double>();
		for (double num3 = 0.0; num3 < (double)num2; num3 += 1.0)
		{
			list.Add(num3 / (double)num);
		}
		double[] array = crv.NormalizedLengthParameters(list.ToArray(), CSUtil.Tolerance);
		if (array == null)
		{
			return false;
		}
		crvInfos.Clear();
		double[] array2 = array;
		foreach (double num4 in array2)
		{
			Point3d val = crv.PointAt(num4);
			Vector3d val2 = crv.TangentAt(num4);
			if (!(val == Point3d.Unset) && !(val2 == Vector3d.Unset))
			{
				((Vector3d)(ref val2)).Unitize();
				CurveInfo item = new CurveInfo
				{
					Parameter = num4,
					PtRc = val,
					TanRc = val2
				};
				crvInfos.Add(item);
			}
		}
		if (crvInfos.Count > 2 && crv.IsClosed)
		{
			CurveInfo value = crvInfos[0];
			CurveInfo value2 = crvInfos[crvInfos.Count - 1];
			Vector3d tanRc = (new Vector3d(value.TanRc) + new Vector3d(value2.TanRc)) / 2.0;
			((Vector3d)(ref tanRc)).Unitize();
			value.TanRc = tanRc;
			value2.TanRc = tanRc;
			crvInfos[0] = value;
			crvInfos[crvInfos.Count - 1] = value2;
		}
		return true;
	}
}
