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
				_tan = value;
				_tanRc = CSConvert.On3dVectorCopyToVector3d(_tan);
			}
		}

		public Point3d PtRc
		{
			get
			{
				return _ptRc;
			}
			set
			{
				_ptRc = value;
				_pt = CSConvert.Point3dCopyToOn3dPoint(_ptRc);
			}
		}

		public Vector3d TanRc
		{
			get
			{
				return _tanRc;
			}
			set
			{
				_tanRc = value;
				_tan = CSConvert.Vector3dCopyToOn3dVector(_tanRc);
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
		double num = gemOnCurveInfo.Radius / sourceGem.GemXMax;
		OnXform onXform = new OnXform();
		if (gemOnCurveInfo.Height > 0.0)
		{
			double y_scale_factor = gemOnCurveInfo.Height / sourceGem.GemYSize;
			onXform.Scale(num, y_scale_factor, num);
		}
		else
		{
			onXform.Scale(num, num, num);
		}
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(roll), OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform onXform3 = new OnXform();
		onXform3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(tilt), OnUtil.On_xaxis, OnUtil.On_origin);
		CSGemData cSGemData = new CSGemData(sourceGem);
		cSGemData.ApplyTransform(onXform.Convert());
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, whichPlacementMethod, ref zOffset);
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0, 0.0, zOffset + gemOnCurveInfo.Placement);
		int fixedIndex = GetFixedIndex(crvInfos, gemOnCurveInfo.Index);
		IOn3dPoint pt = crvInfos[fixedIndex].Pt;
		IOn3dVector tan = crvInfos[fixedIndex].Tan;
		On3dVector on3dVector = (doFlip ? new On3dVector(0.0, 0.0, -1.0) : new On3dVector(0.0, 0.0, 1.0));
		On3dVector on3dVector2 = (doFlip ? new On3dVector(-1.0, 0.0, 0.0) : new On3dVector(1.0, 0.0, 0.0));
		OnXform onXform5;
		switch (whichOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			CSUtilGeometry.GetClosestNormalToBrep(srf, pt, ref destination);
			onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on3dVector, on3dVector2, pt, destination, tan);
			break;
		}
		case ValueListItem.GemOrientationMethodOrigin:
		{
			On3dVector destination = new On3dVector(pt);
			destination.Unitize();
			onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on3dVector2, on3dVector, pt, tan, destination);
			break;
		}
		case ValueListItem.GemOrientationMethodAxisY:
		{
			On3dVector destination = new On3dVector(pt.x, 0.0, pt.z);
			destination.Unitize();
			onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on3dVector2, on3dVector, pt, tan, destination);
			break;
		}
		case ValueListItem.GemOrientationMethodDown:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on3dVector, on3dVector2, pt, destination, tan);
			break;
		}
		default:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on3dVector, on3dVector2, pt, destination, tan);
			break;
		}
		}
		return onXform5 * onXform4 * onXform2 * onXform3 * onXform;
	}

	public static Transform CalculateGemXform(IList<CurveInfo> crvInfos, GemOnCurveInfo gemOnCurveInfo, Brep srf, CSGemData sourceGem, double roll, double tilt, ValueListItem whichOrientationMethod, ValueListItem whichPlacementMethod, bool doFlip)
	{
		double num = gemOnCurveInfo.Radius / sourceGem.GemXMax;
		Transform transform;
		if (gemOnCurveInfo.Height > 0.0)
		{
			double yScaleFactor = gemOnCurveInfo.Height / sourceGem.GemYSize;
			transform = Transform.Scale(Plane.WorldXY, num, yScaleFactor, num);
		}
		else
		{
			transform = Transform.Scale(Plane.WorldXY, num, num, num);
		}
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(roll), Vector3d.ZAxis, Point3d.Origin);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(tilt), Vector3d.XAxis, Point3d.Origin);
		CSGemData cSGemData = new CSGemData(sourceGem);
		cSGemData.ApplyTransform(transform);
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, whichPlacementMethod, ref zOffset);
		Transform transform4 = Transform.Translation(0.0, 0.0, zOffset + gemOnCurveInfo.Placement);
		int fixedIndex = GetFixedIndex(crvInfos, gemOnCurveInfo.Index);
		Point3d ptRc = crvInfos[fixedIndex].PtRc;
		Vector3d tanRc = crvInfos[fixedIndex].TanRc;
		Vector3d vector3d = (doFlip ? new Vector3d(0.0, 0.0, -1.0) : new Vector3d(0.0, 0.0, 1.0));
		Vector3d vector3d2 = (doFlip ? new Vector3d(-1.0, 0.0, 0.0) : new Vector3d(1.0, 0.0, 0.0));
		Transform transform5;
		switch (whichOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
		{
			Vector3d zAxis = Vector3d.ZAxis;
			CSUtilGeometry.GetClosestNormalToBrep(srf, ptRc, ref zAxis);
			transform5 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d, vector3d2, ptRc, zAxis, tanRc);
			break;
		}
		case ValueListItem.GemOrientationMethodOrigin:
		{
			Vector3d zAxis = new Vector3d(ptRc);
			zAxis.Unitize();
			transform5 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d2, vector3d, ptRc, tanRc, zAxis);
			break;
		}
		case ValueListItem.GemOrientationMethodAxisY:
		{
			Vector3d zAxis = new Vector3d(ptRc.X, 0.0, ptRc.Z);
			zAxis.Unitize();
			transform5 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d2, vector3d, ptRc, tanRc, zAxis);
			break;
		}
		case ValueListItem.GemOrientationMethodDown:
		{
			Vector3d zAxis = Vector3d.ZAxis;
			transform5 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d, vector3d2, ptRc, zAxis, tanRc);
			break;
		}
		default:
		{
			Vector3d zAxis = Vector3d.ZAxis;
			transform5 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d, vector3d2, ptRc, zAxis, tanRc);
			break;
		}
		}
		return transform5 * transform4 * transform2 * transform3 * transform;
	}

	public static bool CalculateTestGem(CSGemData sourceGem, double diameter, OnXform xform, ref double radius, ref double rotationalAdjustmentFactor)
	{
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
		rotationalAdjustmentFactor = (boundingBox.Max.X - boundingBox.Min.X) / cSGemData.GemXSize;
		return true;
	}

	public static bool CalculateTestGem(CSGemData sourceGem, double diameter, Transform xform, ref double radius, ref double rotationalAdjustmentFactor)
	{
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
		rotationalAdjustmentFactor = (boundingBox.Max.X - boundingBox.Min.X) / cSGemData.GemXSize;
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
		double num = crv.Domain.ParameterAt(pos);
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
		crvInfos = new List<CurveInfo>();
		if (crv == null)
		{
			return false;
		}
		double length = 0.0;
		if (!crv.GetLength(ref length))
		{
			return false;
		}
		int num = (int)(length * 50.0);
		int num2 = num + 1;
		double[] t = new double[num2];
		List<double> list = new List<double>();
		for (double num3 = 0.0; num3 < (double)num2; num3 += 1.0)
		{
			list.Add(num3 / (double)num);
		}
		if (!crv.GetNormalizedArcLengthPoints(list.ToArray(), ref t))
		{
			return false;
		}
		crvInfos.Clear();
		double[] array = t;
		foreach (double num4 in array)
		{
			On3dPoint point = new On3dPoint();
			On3dVector first_derivative = new On3dVector();
			if (crv.Ev1Der(num4, ref point, ref first_derivative))
			{
				first_derivative.Unitize();
				CurveInfo item = new CurveInfo
				{
					Parameter = num4,
					Pt = point,
					Tan = first_derivative
				};
				crvInfos.Add(item);
			}
		}
		if (crvInfos.Count > 2 && crv.IsClosed())
		{
			CurveInfo value = crvInfos[0];
			CurveInfo value2 = crvInfos[crvInfos.Count - 1];
			On3dVector on3dVector = (new On3dVector(value.Tan) + new On3dVector(value2.Tan)) / 2.0;
			on3dVector.Unitize();
			value.Tan = on3dVector;
			value2.Tan = on3dVector;
			crvInfos[0] = value;
			crvInfos[crvInfos.Count - 1] = value2;
		}
		return true;
	}

	public static bool PreProcessCrv(Curve crv, out IList<CurveInfo> crvInfos)
	{
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
			Point3d point3d = crv.PointAt(num4);
			Vector3d vector3d = crv.TangentAt(num4);
			if (!(point3d == Point3d.Unset) && !(vector3d == Vector3d.Unset))
			{
				vector3d.Unitize();
				CurveInfo item = new CurveInfo
				{
					Parameter = num4,
					PtRc = point3d,
					TanRc = vector3d
				};
				crvInfos.Add(item);
			}
		}
		if (crvInfos.Count > 2 && crv.IsClosed)
		{
			CurveInfo value = crvInfos[0];
			CurveInfo value2 = crvInfos[crvInfos.Count - 1];
			Vector3d tanRc = (new Vector3d(value.TanRc) + new Vector3d(value2.TanRc)) / 2.0;
			tanRc.Unitize();
			value.TanRc = tanRc;
			value2.TanRc = tanRc;
			crvInfos[0] = value;
			crvInfos[crvInfos.Count - 1] = value2;
		}
		return true;
	}
}
