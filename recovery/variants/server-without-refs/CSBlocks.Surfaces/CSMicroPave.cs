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
using Rhino.Geometry.Collections;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlocks.Surfaces;

[CSBlock("MicroPave", "{78E10B3E-8040-47D6-B262-0D102D7CC6F2}")]
public sealed class CSMicroPave : CSBlock
{
	private struct MicroPaveProngInfo
	{
		public double Param;

		public double SizeGem;

		public MicroPaveProngInfo(double param, double sizeGem, double sizeShank)
		{
			Param = param;
			SizeGem = sizeGem;
		}
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
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
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
		if ((BeenUpdatedAfterLastUpdate(InGems.OutputChannelBlock) || BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock) || _interpCrv == null || _lineCrv == null) && !CalculateCurves((Curve)(object)value, list))
		{
			return UpdateError("CalculateCurves failed");
		}
		List<MicroPaveProngInfo> list2 = new List<MicroPaveProngInfo>();
		for (int i = 0; i < list.Count(); i++)
		{
			CSGemData cSGemData = list[i];
			double param = double.NaN;
			if (((Curve)value).ClosestPoint(cSGemData.Center, ref param))
			{
				MicroPaveProngInfo item = new MicroPaveProngInfo
				{
					Param = param,
					SizeGem = cSGemData.GemXSize / 2.0
				};
				list2.Add(item);
			}
		}
		if (!CreateMicroPaveSurface((Curve)(object)value, value2, value3, list2, value4, out var srf, out var profileStart, out var profileEnd))
		{
			return UpdateError("CreateMicroPaveSurface failed");
		}
		OutSurface.SetData(new CSOnGeometryData((GeometryBase)(object)srf));
		OutCurveStart.SetData(new CSOnGeometryData((GeometryBase)(object)profileStart));
		OutCurveEnd.SetData(new CSOnGeometryData((GeometryBase)(object)profileEnd));
		return base.Update();
	}

	private bool CalculateCurves(Curve crv, IEnumerable<CSGemData> gemDatas)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		_lineCrv = null;
		_interpCrv = null;
		double length = crv.GetLength();
		if (double.IsNaN(length) || length < 0.0)
		{
			return false;
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(Point3d.Origin);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(length, 0.0, 0.0);
		_lineCrv = (Curve)new LineCurve(val, val2);
		Point3dList val3 = new Point3dList();
		double num = default(double);
		Interval val4 = default(Interval);
		double num3 = default(double);
		foreach (CSGemData gemData in gemDatas)
		{
			if (!crv.ClosestPoint(gemData.Center, ref num))
			{
				continue;
			}
			Interval domain = crv.Domain;
			((Interval)(ref val4))._002Ector(((Interval)(ref domain)).Min, num);
			double length2 = crv.GetLength(CSApplicationHelper.Application.Tolerance, val4);
			if (!double.IsNaN(length2) && !(length2 < 0.0))
			{
				double num2 = length2 / length;
				if (_lineCrv.NormalizedLengthParameter(num2, ref num3))
				{
					Point3d val5 = _lineCrv.PointAt(num3);
					((Point3d)(ref val5)).Y = gemData.GemXSize / 2.0;
					((Point3d)(ref val5)).Z = 0.0;
					((RhinoList<Point3d>)(object)val3).Add(val5);
				}
			}
		}
		Curve val6 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val3, 3, (CurveKnotStyle)0);
		if (val6 == null)
		{
			return false;
		}
		Point3d val7 = val6.PointAtStart;
		double x = ((Point3d)(ref val7)).X;
		if (x > 0.0)
		{
			val6 = val6.Extend((CurveEnd)1, x, (CurveExtensionStyle)0);
		}
		val7 = val6.PointAtEnd;
		double num4 = length - ((Point3d)(ref val7)).X;
		if (num4 > 0.0)
		{
			val6 = val6.Extend((CurveEnd)2, num4, (CurveExtensionStyle)0);
		}
		_interpCrv = val6;
		if (_lineCrv != null && ((CommonObject)_lineCrv).IsValid && _interpCrv != null)
		{
			return ((CommonObject)_interpCrv).IsValid;
		}
		return false;
	}

	private bool CreateMicroPaveSurface(Curve crv, NurbsCurve profileBase, double ringRailRadius, IReadOnlyList<MicroPaveProngInfo> prongInfos, double prongHeight, out NurbsSurface srf, out NurbsCurve profileStart, out NurbsCurve profileEnd)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Expected O, but got Unknown
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Expected O, but got Unknown
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_084e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Unknown result type (might be due to invalid IL or missing references)
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		srf = null;
		profileStart = null;
		profileEnd = null;
		int num = prongInfos.Count + 1;
		int num2 = 10 * (num - 1) + 1;
		int count = ((RhinoList<Point3d>)(object)profileBase.GrevillePoints()).Count;
		int num3 = 15 + count * 2;
		int num4 = 0;
		double num5 = 0.0;
		Interval domain = ((Curve)profileBase).Domain;
		num5 = ((Interval)(ref domain)).Max;
		_ = ((Interval)(ref domain)).Min;
		double length = crv.GetLength();
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		list.Add(0.0);
		list2.Add(0.0);
		for (int i = 0; i < prongInfos.Count; i++)
		{
			double param = prongInfos[i].Param;
			Interval domain2 = crv.Domain;
			double length2 = crv.GetLength(0.01, new Interval(((Interval)(ref domain2)).Min, param));
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
		Point3d location;
		Vector3d val3 = default(Vector3d);
		int num14;
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
				location = ((ControlPoint)(ref MicroPaveProng.Cv[k, 0])).Location;
				double num10 = num6 + num7 * ((Point3d)(ref location)).X;
				if (!GetWidthAtNormalizedParam(num10, out var width))
				{
					return false;
				}
				double num11 = 0.0;
				crv.NormalizedLengthParameter(num10, ref num11);
				Point3d val = default(Point3d);
				Vector3d val2 = default(Vector3d);
				val = crv.PointAt(num11);
				val2 = crv.TangentAt(num11);
				((Vector3d)(ref val3))._002Ector(0.0, 1.0, 0.0);
				Vector3d val4 = Vector3d.CrossProduct(val2, val3);
				((Vector3d)(ref val4)).Unitize();
				Vector3d val5 = Vector3d.CrossProduct(val4, val2);
				((Vector3d)(ref val4)).Y = 0.0;
				((Vector3d)(ref val4)).Unitize();
				((Point3d)(ref location))._002Ector(val);
				((Point3d)(ref location)).Y = 0.0;
				Point3d linePoint = location;
				double lineParamOfClosestPoint = 0.0;
				double paramAlongLine = 0.0;
				if (!MicroPaveProng.PointOnLineAtDistanceFromPoint(linePoint, val4, new Point3d(Point3d.Origin), ringRailRadius, ref lineParamOfClosestPoint, ref paramAlongLine))
				{
					return false;
				}
				double num12 = 0.0 - lineParamOfClosestPoint - paramAlongLine;
				int num13 = ((RhinoList<Point3d>)(object)profileBase.GrevillePoints()).Count - 1;
				num14 = 0;
				for (int l = 0; l < ((RhinoList<Point3d>)(object)profileBase.GrevillePoints()).Count; l++)
				{
					Point3d val6 = profileBase.GrevillePoint(num13 - l);
					((Point3d)(ref val6)).X = ((Point3d)(ref val6)).X / 5.0;
					((Point3d)(ref val6)).Y = ((Point3d)(ref val6)).Y / 2.0;
					Point3d val7 = val;
					Vector3d val8 = val5 * ((Point3d)(ref val6)).X;
					location = ((ControlPoint)(ref MicroPaveProng.Cv[k, 0])).Location;
					Point3d val9 = val7 + val8 * ((Point3d)(ref location)).Y * width + val4 * (((Point3d)(ref val6)).Y - num12);
					array[num4, num14] = new ControlPoint(((Point3d)(ref val9)).X, ((Point3d)(ref val9)).Y, ((Point3d)(ref val9)).Z, 1.0);
					num14++;
				}
				for (int m = 0; m < 15; m++)
				{
					location = ((ControlPoint)(ref MicroPaveProng.Cv[k, m])).Location;
					double num15 = ((Point3d)(ref location)).Z * prongHeight;
					Point3d val10 = val;
					location = ((ControlPoint)(ref MicroPaveProng.Cv[k, m])).Location;
					Point3d val11 = val10 + val5 * ((Point3d)(ref location)).Y * width + val4 * num15;
					array[num4, num14] = new ControlPoint(((Point3d)(ref val11)).X, ((Point3d)(ref val11)).Y, ((Point3d)(ref val11)).Z, 1.0);
					num14++;
				}
				for (int n = 0; n < ((RhinoList<Point3d>)(object)profileBase.GrevillePoints()).Count; n++)
				{
					Point3d val12 = profileBase.GrevillePoint(n);
					((Point3d)(ref val12)).X = (0.0 - ((Point3d)(ref val12)).X) / 5.0;
					((Point3d)(ref val12)).Y = ((Point3d)(ref val12)).Y / 2.0;
					Point3d val13 = val;
					Vector3d val14 = val5 * ((Point3d)(ref val12)).X;
					location = ((ControlPoint)(ref MicroPaveProng.Cv[k, 0])).Location;
					Point3d val15 = val13 + val14 * ((Point3d)(ref location)).Y * width + val4 * (((Point3d)(ref val12)).Y - num12);
					array[num4, num14] = new ControlPoint(((Point3d)(ref val15)).X, ((Point3d)(ref val15)).Y, ((Point3d)(ref val15)).Z, 1.0);
					num14++;
				}
				num4++;
			}
		}
		double[] array2 = new double[num2 + 2];
		array2[0] = 0.0;
		array2[1] = 0.0;
		for (int num16 = 0; num16 < num2 - 2; num16++)
		{
			array2[num16 + 2] = num16;
		}
		array2[num2] = num2 - 3;
		array2[num2 + 1] = num2 - 3;
		double[] array3 = new double[num3 + 2];
		int num17 = profileBase.Knots.Count - 1;
		num14 = 0;
		for (int num18 = 0; num18 < ((RhinoList<Point3d>)(object)profileBase.GrevillePoints()).Count + 1; num18++)
		{
			array3[num14] = num5 - profileBase.Knots[num17 - num18];
			num14++;
		}
		for (int num19 = 0; num19 < 15; num19++)
		{
			array3[num14] = num5 + (double)num19;
			num14++;
		}
		double num20 = num5 + 15.0 - 1.0;
		for (int num21 = 0; num21 < ((RhinoList<Point3d>)(object)profileBase.GrevillePoints()).Count + 1; num21++)
		{
			array3[num14] = num20 + profileBase.Knots[num21 + 1];
			num14++;
		}
		srf = NurbsSurface.Create(3, false, 4, 4, num2, num3);
		for (int num22 = 0; num22 < array2.Length; num22++)
		{
			srf.KnotsU[num22] = array2[num22];
		}
		for (int num23 = 0; num23 < array3.Length; num23++)
		{
			srf.KnotsV[num23] = array3[num23];
		}
		for (int num24 = 0; num24 < num2; num24++)
		{
			for (int num25 = 0; num25 < num3; num25++)
			{
				srf.Points.SetControlPoint(num24, num25, array[num24, num25]);
			}
		}
		profileStart = new NurbsCurve(3, false, 4, num3);
		profileEnd = new NurbsCurve(3, false, 4, num3);
		for (int num26 = 0; num26 < array3.Length; num26++)
		{
			profileStart.Knots[num26] = array3[num26];
			profileEnd.Knots[num26] = array3[num26];
		}
		for (int num27 = 0; num27 < num3; num27++)
		{
			NurbsCurvePointList points = profileStart.Points;
			int num28 = num27;
			location = ((ControlPoint)(ref array[0, num27])).Location;
			double x = ((Point3d)(ref location)).X;
			location = ((ControlPoint)(ref array[0, num27])).Location;
			double y = ((Point3d)(ref location)).Y;
			location = ((ControlPoint)(ref array[0, num27])).Location;
			points.SetPoint(num28, x, y, ((Point3d)(ref location)).Z, ((ControlPoint)(ref array[0, num27])).Weight);
			NurbsCurvePointList points2 = profileEnd.Points;
			int num29 = num27;
			location = ((ControlPoint)(ref array[num2 - 1, num27])).Location;
			double x2 = ((Point3d)(ref location)).X;
			location = ((ControlPoint)(ref array[num2 - 1, num27])).Location;
			double y2 = ((Point3d)(ref location)).Y;
			location = ((ControlPoint)(ref array[num2 - 1, num27])).Location;
			points2.SetPoint(num29, x2, y2, ((Point3d)(ref location)).Z, ((ControlPoint)(ref array[num2 - 1, num27])).Weight);
		}
		return true;
	}

	private bool GetWidthAtNormalizedParam(double s, out double width)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		width = double.NaN;
		if (_lineCrv == null || _interpCrv == null || s < 0.0 || s > 1.0)
		{
			return false;
		}
		double num = double.NaN;
		if (!_lineCrv.NormalizedLengthParameter(s, ref num))
		{
			return false;
		}
		Point3d val = _lineCrv.PointAt(num);
		((Point3d)(ref val)).Y = 0.0;
		((Point3d)(ref val)).Z = 0.0;
		Point2d val2 = new Point2d(((Point3d)(ref val)).X, 0.0);
		Point2d val3 = default(Point2d);
		((Point2d)(ref val3))._002Ector(((Point3d)(ref val)).X, 50.0);
		LineCurve val4 = new LineCurve(val2, val3);
		CurveIntersections val5 = Intersection.CurveCurve(_interpCrv, (Curve)(object)val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val5.Count <= 0)
		{
			return false;
		}
		double num2 = double.NaN;
		double num3 = double.NaN;
		for (int i = 0; i < val5.Count; i++)
		{
			double parameterA = val5[i].ParameterA;
			Point3d val6 = _interpCrv.PointAt(parameterA);
			double num4 = ((Point3d)(ref val6)).DistanceTo(val);
			if (double.IsNaN(num2) || double.IsNaN(num3) || !(num4 >= num3))
			{
				num2 = parameterA;
				num3 = num4;
			}
		}
		Point3d val7 = _interpCrv.PointAt(num2);
		width = ((Point3d)(ref val7)).Y;
		return ((Point3d)(ref val7)).IsValid;
	}
}
