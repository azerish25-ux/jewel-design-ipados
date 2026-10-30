using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using CSBlockUtil;
using CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadProngProfileSweep
{
	public static bool ProngProfileSweep(int prongIndex, ProngParam[] prongParams, Curve headTop, double paramTop, double prongTopOffset, Curve headBase, double paramBase, double prongBaseOffset, ValueListItem baseStyle, double basePointSize, Transform xUp, bool enableDoItYourself, bool planarDome, Point3d focalPoint, out List<Brep> topDome, out List<Brep> middleProng, out List<Brep> lowerDome, out List<Curve> outCurves, out List<Brep> outProngs, out string error)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Invalid comparison between Unknown and I4
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Expected O, but got Unknown
		//IL_0d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Expected O, but got Unknown
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d12: Expected O, but got Unknown
		//IL_0d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1b: Expected O, but got Unknown
		//IL_0d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcc: Expected O, but got Unknown
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_096b: Expected O, but got Unknown
		//IL_0e25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e41: Unknown result type (might be due to invalid IL or missing references)
		ProngParam prongParam = prongParams[prongIndex];
		outCurves = new List<Curve>();
		outProngs = new List<Brep>();
		topDome = new List<Brep>();
		middleProng = new List<Brep>();
		lowerDome = new List<Brep>();
		Curve prongRail = prongParam.ProngRail;
		Point3d pointAtStart = prongRail.PointAtStart;
		Vector3d val = CSUtilMath.AverageTangentAt(headTop, paramTop);
		((Vector3d)(ref val)).Unitize();
		Vector3d headTopNrm = CSUtilMath.AverageNormalAt(Vector3d.ZAxis, val);
		((Vector3d)(ref headTopNrm)).Unitize();
		BoundingBox boundingBox = ((GeometryBase)prongParam.ProngTopProfile).GetBoundingBox(true);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		Vector3d inputVector = new Vector3d(0.0, 0.0, -1.0);
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		double num = CSUtilMath.AngleBetweenVectorsWithDirection(inputVector, obj.TangentAt(((Interval)(ref domain)).Min), val);
		double num2 = 1.0 / Math.Cos(num);
		Transform val2 = Transform.Scale(Plane.WorldXY, prongParam.ProngTopX / x, prongParam.ProngTopY / y * num2, 0.0);
		if (!planarDome)
		{
			val2 = Transform.Scale(Plane.WorldXY, prongParam.ProngTopX / x, prongParam.ProngTopY / y, 0.0);
		}
		Transform val3 = Transform.Translation(0.0, prongTopOffset, 0.0);
		Transform val4 = CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtStart, val, Vector3d.ZAxis);
		Curve prongTopProfile = prongParam.ProngTopProfile.DuplicateCurve();
		((GeometryBase)prongTopProfile).Transform(val4 * val3 * val2);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(0.0, 1.0, 0.0);
		((Vector3d)(ref val5)).Transform(val4);
		((Vector3d)(ref val5)).Unitize();
		Point3d rotationCenter = pointAtStart + val5 * prongTopOffset;
		Transform val6 = CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, rotationCenter);
		Transform reverseOrient = default(Transform);
		((Transform)(ref val4)).TryGetInverse(ref reverseOrient);
		if (!planarDome)
		{
			((GeometryBase)prongTopProfile).Transform(val6);
		}
		Vector3d val7;
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			val7 = val;
		}
		else
		{
			val7 = CSUtilMath.AverageTangentAt(headBase, paramBase);
			((Vector3d)(ref val7)).Unitize();
		}
		BoundingBox boundingBox2 = ((GeometryBase)prongParam.ProngBaseProfile).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double x2 = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double y2 = ((Vector3d)(ref diagonal)).Y;
		Transform val8 = Transform.Scale(Plane.WorldXY, prongParam.ProngBaseX / x2, prongParam.ProngBaseY / y2, 0.0);
		if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
		{
			prongBaseOffset = 0.0 - prongParam.ProngBaseY + basePointSize;
		}
		Transform val9 = Transform.Translation(0.0, prongBaseOffset, 0.0);
		Vector3d tangentAtEnd = prongRail.TangentAtEnd;
		Brep dome = null;
		Brep dome2 = null;
		if (prongParam.ProngDomeHeight <= 0.0)
		{
			if (!ProngExtension(0.0, ref prongRail, ref prongTopProfile))
			{
				error = "Unable to create top prong cap.";
				return false;
			}
			if (!planarDome)
			{
				((GeometryBase)prongTopProfile).Transform(val6);
			}
		}
		else
		{
			if (!ProngDome(prongParam, num, num2, planarDome, prongRail, reverseOrient, ref dome))
			{
				error = "Unable to create top dome";
				return false;
			}
			((GeometryBase)dome).Transform(val4 * val3);
		}
		Point3d val10 = prongRail.PointAtEnd;
		double num3 = ((Point3d)(ref val10)).Z + prongParam.ProngBaseDomeHeight;
		double num4 = CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? val : val7);
		double num5 = ((num4 < 0.0) ? (Math.Tan(0.0 - num4) * prongParam.ProngBaseY + 0.05) : 0.05);
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			num5 = ((num4 < 0.0) ? (Math.Tan(0.0 - num4) * prongParam.ProngBaseY + 0.05) : (Math.Tan(num4) * prongParam.ProngBaseY + 0.05));
		}
		prongRail = prongRail.Extend((CurveEnd)2, num5, (CurveExtensionStyle)0);
		Point3d pointAtEnd = prongRail.PointAtEnd;
		Transform val11 = CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? val : val7, -tangentAtEnd);
		Curve val12 = prongParam.ProngBaseProfile.DuplicateCurve();
		((GeometryBase)val12).Transform(val11 * val9 * val8);
		Curve[] profiles = (Curve[])(object)new Curve[2] { prongTopProfile, val12 };
		double[] array = new double[2];
		domain = prongRail.Domain;
		array[0] = ((Interval)(ref domain)).Min;
		domain = prongRail.Domain;
		array[1] = ((Interval)(ref domain)).Max;
		double[] parameters = array;
		if (!CSUtilSweepRC.Sweep1(prongRail, profiles, parameters, out var breps, closed: false, globalShapeBlend: false))
		{
			error = "Failed Sweep1";
			return false;
		}
		if (breps == null || breps.Length == 0)
		{
			error = "Failed Sweep1: No results";
			return false;
		}
		Brep val13 = ((Surface)NurbsSurface.CreateFromCorners(new Point3d(20.0, 20.0, num3), new Point3d(-20.0, 20.0, num3), new Point3d(-20.0, -20.0, num3), new Point3d(20.0, -20.0, num3))).ToBrep();
		Brep[] array2 = breps[0].Split(val13, 0.01);
		if (array2.Length != 2)
		{
			error = "Prong split failed";
			return false;
		}
		BoundingBox boundingBox3 = ((GeometryBase)array2[0]).GetBoundingBox(true);
		BoundingBox boundingBox4 = ((GeometryBase)array2[1]).GetBoundingBox(true);
		val10 = ((BoundingBox)(ref boundingBox3)).Center;
		double z = ((Point3d)(ref val10)).Z;
		val10 = ((BoundingBox)(ref boundingBox4)).Center;
		Brep val14 = ((z > ((Point3d)(ref val10)).Z) ? array2[0] : array2[1]);
		if (prongParam.ProngBaseDomeHeight > 0.0)
		{
			List<Curve> list = new List<Curve>();
			List<Curve> list2 = new List<Curve>();
			foreach (BrepEdge edge in val14.Edges)
			{
				if ((int)edge.Valence == 1)
				{
					list.Add(((Curve)edge).DuplicateCurve());
				}
				else
				{
					list2.Add(((Curve)edge).DuplicateCurve());
				}
			}
			Curve[] array3 = Curve.JoinCurves((IEnumerable<Curve>)list);
			if (array3.Length != 2)
			{
				error = "Prong edge curve extract failed";
				return false;
			}
			BoundingBox boundingBox5 = ((GeometryBase)array3[0]).GetBoundingBox(true);
			BoundingBox boundingBox6 = ((GeometryBase)array3[1]).GetBoundingBox(true);
			val10 = ((BoundingBox)(ref boundingBox5)).Center;
			double z2 = ((Point3d)(ref val10)).Z;
			val10 = ((BoundingBox)(ref boundingBox6)).Center;
			Curve obj2 = ((z2 < ((Point3d)(ref val10)).Z) ? array3[0] : array3[1]);
			Transform val15 = CSUtilMath.Orient3Point(list2[0].PointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? val : val7, Vector3d.ZAxis, Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis);
			((GeometryBase)obj2).Transform(val15);
			val12 = obj2;
			double angle = CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), list2[0].TangentAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? val : val7);
			if (!ProngBaseDome(val12, 0.0 - prongParam.ProngBaseDomeHeight, angle, ref dome2))
			{
				error = "Failed prong base";
				return false;
			}
			Transform val16 = CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, list2[0].PointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? val : val7, Vector3d.ZAxis);
			((GeometryBase)val12).Transform(val16);
			((GeometryBase)dome2).Transform(val16);
		}
		GeometryBase[] arrGeometry = (GeometryBase[])(object)new GeometryBase[3]
		{
			(GeometryBase)prongRail,
			(GeometryBase)prongTopProfile,
			(GeometryBase)val12
		};
		List<GeometryBase> outputGeometries = new List<GeometryBase>();
		if (!ArrangeGeometry(prongParam.ProngArrangement, xUp, val, headTopNrm, prongParam.ProngTopX, prongParam.ProngOverlap, arrGeometry, null, ref outputGeometries))
		{
			error = "Invalid head style";
			return false;
		}
		foreach (Curve item14 in outputGeometries)
		{
			Curve item = item14;
			outCurves.Add(item);
		}
		if (enableDoItYourself)
		{
			GeometryBase[] arrGeometry2;
			if (dome2 != null)
			{
				arrGeometry2 = (GeometryBase[])(object)new GeometryBase[2]
				{
					(GeometryBase)dome,
					(GeometryBase)dome2
				};
				List<GeometryBase> outputGeometries2 = new List<GeometryBase>();
				GeometryBase[] arrGeometry3 = (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)dome2 };
				if (!ArrangeGeometry(prongParam.ProngArrangement, xUp, val, headTopNrm, prongParam.ProngTopX, prongParam.ProngOverlap, arrGeometry3, null, ref outputGeometries2))
				{
					error = "Invalid head style";
					return false;
				}
				foreach (Brep item15 in outputGeometries2)
				{
					Brep item2 = item15;
					lowerDome.Add(item2);
				}
			}
			else
			{
				arrGeometry2 = (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)dome };
			}
			List<GeometryBase> outputGeometries3 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongParam.ProngArrangement, xUp, val, headTopNrm, prongParam.ProngTopX, prongParam.ProngOverlap, arrGeometry2, null, ref outputGeometries3))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item16 in outputGeometries3)
			{
				Brep item3 = item16;
				outProngs.Add(item3);
			}
			GeometryBase[] arrGeometry4 = (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)dome };
			List<GeometryBase> outputGeometries4 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongParam.ProngArrangement, xUp, val, headTopNrm, prongParam.ProngTopX, prongParam.ProngOverlap, arrGeometry4, null, ref outputGeometries4))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item17 in outputGeometries4)
			{
				Brep item4 = item17;
				topDome.Add(item4);
			}
		}
		else
		{
			List<Brep> list3 = new List<Brep>();
			if (dome != null)
			{
				list3.Add(dome);
			}
			if (dome2 != null)
			{
				list3.Add(dome2);
			}
			if (val14 != null)
			{
				list3.Add(val14);
			}
			Brep brep = CSUtilRhino.JoinAllBreps(list3.ToArray(), 0.01);
			if (brep == null)
			{
				error = "Failed JoinBreps";
				return false;
			}
			if (dome2 == null)
			{
				Brep val17 = CSUtilRhino.CapPlanarHoles(brep, 0.01);
				if (val17 == null)
				{
					error = "Failed CapPlanarHoles";
					return false;
				}
				brep = val17;
			}
			Brep brep2 = null;
			if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
			{
				List<Point3d> list4;
				if (prongParams.Length > 2)
				{
					Curve prongRail2 = prongParams[(prongIndex + 1) % prongParams.Length].ProngRail;
					if (prongRail2 == null)
					{
						error = "Failed to get prongRailCrvP";
						return false;
					}
					Curve val18 = prongRail2.DuplicateCurve();
					Curve prongRail3 = prongParams[(prongIndex - 1 + prongParams.Length) % prongParams.Length].ProngRail;
					if (prongRail3 == null)
					{
						error = "Failed to get prongRailCrvM";
						return false;
					}
					Curve val19 = prongRail3.DuplicateCurve();
					Vector3d val20 = default(Vector3d);
					((Vector3d)(ref val20))._002Ector(val18.PointAtStart);
					Vector3d val21 = default(Vector3d);
					((Vector3d)(ref val21))._002Ector(prongRail.PointAtStart);
					Vector3d val22 = default(Vector3d);
					((Vector3d)(ref val22))._002Ector(val19.PointAtStart);
					((Vector3d)(ref val20)).Z = 0.0;
					((Vector3d)(ref val21)).Z = 0.0;
					((Vector3d)(ref val22)).Z = 0.0;
					((Vector3d)(ref val20)).Unitize();
					((Vector3d)(ref val21)).Unitize();
					((Vector3d)(ref val22)).Unitize();
					val20 *= 20.0;
					val21 *= 20.0;
					val22 *= 20.0;
					Point3d item5 = default(Point3d);
					((Point3d)(ref item5))._002Ector(val20 + val21);
					Point3d item6 = default(Point3d);
					((Point3d)(ref item6))._002Ector(0.0, 0.0, 0.0);
					Point3d item7 = default(Point3d);
					((Point3d)(ref item7))._002Ector(val22 + val21);
					Point3d item8 = default(Point3d);
					((Point3d)(ref item8))._002Ector(val21 * -2.0);
					((Point3d)(ref item5)).Z = 20.0;
					((Point3d)(ref item6)).Z = 20.0;
					((Point3d)(ref item7)).Z = 20.0;
					((Point3d)(ref item8)).Z = 20.0;
					list4 = new List<Point3d> { item5, item6, item7, item8, item5 };
				}
				else
				{
					((Vector3d)(ref diagonal))._002Ector(prongRail.PointAtStart);
					((Vector3d)(ref diagonal)).Z = 0.0;
					Vector3d val23 = diagonal;
					((Vector3d)(ref val23)).Unitize();
					val23 *= 20.0;
					Vector3d val24 = default(Vector3d);
					((Vector3d)(ref val24))._002Ector(val23);
					((Vector3d)(ref val24)).Transform(Transform.Rotation(CSUtilMath.ToRadians(90.0), Point3d.Origin));
					Point3d item9 = default(Point3d);
					((Point3d)(ref item9))._002Ector(val24);
					Point3d item10 = default(Point3d);
					((Point3d)(ref item10))._002Ector(0.0, 0.0, 0.0);
					Point3d item11 = default(Point3d);
					((Point3d)(ref item11))._002Ector(-val24);
					Point3d item12 = default(Point3d);
					((Point3d)(ref item12))._002Ector(val23 * -2.0);
					((Point3d)(ref item9)).Z = 20.0;
					((Point3d)(ref item10)).Z = 20.0;
					((Point3d)(ref item11)).Z = 20.0;
					((Point3d)(ref item12)).Z = 20.0;
					list4 = new List<Point3d> { item9, item10, item11, item12, item9 };
				}
				PolylineCurve val25 = new PolylineCurve((IEnumerable<Point3d>)list4);
				PolylineCurve val26 = new PolylineCurve((IEnumerable<Point3d>)list4);
				Transform val27 = Transform.Translation(0.0, 0.0, -40.0);
				((GeometryBase)val26).Transform(val27);
				brep2 = CSUtilRhino.CapPlanarHoles(((Surface)NurbsSurface.CreateRuledSurface((Curve)(object)val25, (Curve)(object)val26)).ToBrep(), 0.001);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep, doFullClean: true);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep2, doFullClean: true);
			}
			List<GeometryBase> outputGeometries5 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongParam.ProngArrangement, xUp, val, headTopNrm, prongParam.ProngTopX, prongParam.ProngOverlap, (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)brep }, brep2, ref outputGeometries5))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item18 in outputGeometries5)
			{
				Brep item13 = item18;
				outProngs.Add(item13);
			}
			Brep val28 = ((dome != null) ? dome.DuplicateBrep() : null);
			Brep val29 = ((dome2 != null) ? dome2.DuplicateBrep() : null);
			Brep val30 = ((val14 != null) ? val14.DuplicateBrep() : null);
			if (val28 != null)
			{
				((GeometryBase)val28).Transform(xUp);
			}
			if (val29 != null)
			{
				((GeometryBase)val29).Transform(xUp);
			}
			if (val30 != null)
			{
				((GeometryBase)val30).Transform(xUp);
			}
			prongParam.TopDome.Add(val28);
			prongParam.LowerDome.Add(val29);
			prongParam.MiddleProng.Add(val30);
		}
		error = "";
		return true;
	}

	private static bool ArrangeGeometry(int prongArrangement, Transform xUp, Vector3d headTopTan, Vector3d headTopNrm, double prongTopX, double overlap, GeometryBase[] arrGeometry, Brep cutter, ref List<GeometryBase> outputGeometries)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		switch ((CSHeadStyles5.EHeadProngArrangment)prongArrangement)
		{
		case CSHeadStyles5.EHeadProngArrangment.Single:
		{
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item4 = CutGeometry(array[i], cutter, xUp, xUp);
				outputGeometries.Add(item4);
			}
			break;
		}
		case CSHeadStyles5.EHeadProngArrangment.Double:
		{
			double num3 = overlap * prongTopX;
			Transform val4 = Transform.Translation(headTopTan * 1.0 * num3);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item5 = CutGeometry(array[i], cutter, xUp * val4, xUp);
				outputGeometries.Add(item5);
			}
			Transform val5 = Transform.Translation(headTopTan * -1.0 * num3);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item6 = CutGeometry(array[i], cutter, xUp * val5, xUp);
				outputGeometries.Add(item6);
			}
			break;
		}
		case CSHeadStyles5.EHeadProngArrangment.Triple:
		{
			double num = overlap * prongTopX;
			Transform val = Transform.Translation(headTopTan * 1.0 * num);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item = CutGeometry(array[i], cutter, xUp * val, xUp);
				outputGeometries.Add(item);
			}
			Transform val2 = Transform.Translation(headTopTan * -1.0 * num);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item2 = CutGeometry(array[i], cutter, xUp * val2, xUp);
				outputGeometries.Add(item2);
			}
			double num2 = overlap * prongTopX * 2.0 * Math.Sqrt(3.0) / 2.0;
			Transform val3 = Transform.Translation(headTopNrm * num2);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item3 = CutGeometry(array[i], cutter, xUp * val3, xUp);
				outputGeometries.Add(item3);
			}
			break;
		}
		default:
			return false;
		}
		return true;
	}

	private static GeometryBase CutGeometry(GeometryBase g, Brep cut, Transform T, Transform xUp)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		Brep val = (Brep)(object)((g is Brep) ? g : null);
		if (val == null || cut == null)
		{
			GeometryBase obj = g.Duplicate();
			obj.Transform(T);
			return obj;
		}
		val = val.DuplicateBrep();
		((GeometryBase)val).Transform(T);
		Brep val2 = cut.DuplicateBrep();
		((GeometryBase)val2).Transform(xUp);
		Brep[] array = Brep.CreateBooleanDifference(val, val2, 0.001);
		if (array == null)
		{
			return (GeometryBase)(object)val.DuplicateBrep();
		}
		return (GeometryBase)(object)array[0];
	}

	private static bool ProngBaseDome(Curve prongProfile, double radius, double angle, ref Brep dome)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = ((GeometryBase)prongProfile).GetBoundingBox(true);
		Curve val = prongProfile.DuplicateCurve();
		((GeometryBase)val).Translate(new Vector3d(-((BoundingBox)(ref boundingBox)).Center));
		NurbsSurface val2 = CSUtilRailRevolve.RailRevolveRC(((Curve)CSUtilGeometry.CreateDomeRC(radius)).ToNurbsCurve(), val.ToNurbsCurve(), 0, zAdjust: true);
		if (val2 == null)
		{
			return false;
		}
		double num = Math.Tan(angle);
		for (int i = 0; i < val2.Points.CountU; i++)
		{
			for (int j = 0; j < val2.Points.CountV; j++)
			{
				ControlPoint controlPoint = val2.Points.GetControlPoint(i, j);
				Point3d location = ((ControlPoint)(ref controlPoint)).Location;
				((Point3d)(ref location)).Y = ((Point3d)(ref location)).Y + num * ((Point3d)(ref location)).Z;
				((ControlPoint)(ref controlPoint)).Location = location;
				val2.Points.SetControlPoint(i, j, controlPoint);
			}
		}
		((GeometryBase)val2).Translate(new Vector3d(((BoundingBox)(ref boundingBox)).Center));
		dome = ((Surface)val2).ToBrep();
		return true;
	}

	private static bool ProngDome(ProngParam prongParam, double angle, double fixScale, bool planarDome, Curve prongRail, Transform reverseOrient, ref Brep dome)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = ((GeometryBase)prongParam.ProngTopProfile).GetBoundingBox(true);
		bool flag = prongParam.ProngDomeHeight > 0.0;
		double blendAmount = prongParam.DomeBlendAmountInside;
		double blendAmount2 = prongParam.DomeBlendAmountOutside;
		double blendStart = prongParam.DomeBlendStartInside;
		double blendStart2 = prongParam.DomeBlendStartOutside;
		double blendEnd = prongParam.DomeBlendEndInside;
		double blendEnd2 = prongParam.DomeBlendEndOutside;
		if (!flag)
		{
			blendAmount = 1.0;
			blendAmount2 = 1.0;
			blendStart = 1.0;
			blendStart2 = 1.0;
			blendEnd = 1.0;
			blendEnd2 = 1.0;
		}
		Plane worldXY = Plane.WorldXY;
		double prongTopX = prongParam.ProngTopX;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num = prongTopX / ((Vector3d)(ref diagonal)).X;
		double prongTopY = prongParam.ProngTopY;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val = Transform.Scale(worldXY, num, prongTopY / ((Vector3d)(ref diagonal)).Y * fixScale, 0.0);
		if (!planarDome && flag)
		{
			Plane worldXY2 = Plane.WorldXY;
			double prongTopX2 = prongParam.ProngTopX;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double num2 = prongTopX2 / ((Vector3d)(ref diagonal)).X;
			double prongTopY2 = prongParam.ProngTopY;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			val = Transform.Scale(worldXY2, num2, prongTopY2 / ((Vector3d)(ref diagonal)).Y, 0.0);
		}
		Transform val2 = Transform.Translation(0.0, (0.0 - prongParam.ProngTopY) / 2.0, 0.0);
		Curve val3 = prongParam.ProngTopProfile.DuplicateCurve();
		((GeometryBase)val3).Transform(val2 * val);
		Curve val4 = prongRail.DuplicateCurve();
		if (flag && !planarDome)
		{
			((GeometryBase)val4).Transform(reverseOrient);
			Transform val5 = CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, val4.TangentAtStart, val3.PointAtStart);
			((GeometryBase)val3).Transform(val5);
		}
		LineCurve val6 = new LineCurve(new Point3d(0.0, -50.0, 0.0), new Point3d(0.0, 50.0, 0.0));
		NurbsSurface val7 = NurbsSurface.CreateFromCorners(new Point3d(0.0, -20.0, -10.0), new Point3d(0.0, 20.0, -10.0), new Point3d(0.0, 20.0, 10.0), new Point3d(0.0, -20.0, 10.0));
		CurveIntersections val8 = Intersection.CurveSurface(val3, (Surface)(object)val7, 0.01, 0.01);
		if (val8.Count <= 1)
		{
			return false;
		}
		double parameterA = val8[0].ParameterA;
		double parameterA2 = val8[1].ParameterA;
		Interval domain = val3.Domain;
		if (Math.Abs(parameterA - ((Interval)(ref domain)).Min) > 0.0001)
		{
			domain = val3.Domain;
			if (Math.Abs(parameterA - ((Interval)(ref domain)).Max) > 0.0001)
			{
				Point3d val9 = val3.PointAt(parameterA2);
				val3.ChangeClosedCurveSeam(parameterA);
				val3.ClosestPoint(val9, ref parameterA2);
			}
		}
		Curve[] array = val3.Split(parameterA2);
		Curve val10 = array[0];
		Curve val11 = array[1];
		if (val10 == null || val11 == null)
		{
			return false;
		}
		domain = ((Curve)val6).Domain;
		Vector3d val12 = ((Curve)val6).TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val12)).Unitize();
		domain = val10.Domain;
		Vector3d val13 = val10.TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val13)).Unitize();
		if (val12 * val13 < 0.0)
		{
			val10.Reverse();
		}
		domain = val11.Domain;
		Vector3d val14 = val11.TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val14)).Unitize();
		if (val12 * val14 < 0.0)
		{
			val11.Reverse();
		}
		double num3 = prongParam.ProngDomeHeight;
		Vector3d val15 = Vector3d.ZAxis;
		if (!flag)
		{
			((Vector3d)(ref val15)).Z = 0.0 - ((Vector3d)(ref val15)).Z;
			num3 = 0.0 - num3;
		}
		if (planarDome || !flag)
		{
			((Vector3d)(ref val15)).Rotate(0.0 - angle, Vector3d.XAxis);
		}
		((Vector3d)(ref val15)).Unitize();
		Point3d val16 = ((BoundingBox)(ref boundingBox)).Center;
		((Point3d)(ref val16)).Transform(val2 * val);
		Point3d rotationCenter = default(Point3d);
		((Point3d)(ref rotationCenter))._002Ector(val16);
		if (flag)
		{
			val16 = (planarDome ? (val16 + Vector3d.ZAxis * num3) : (val16 + val15 * (num3 / Math.Cos(angle))));
			((Point3d)(ref val16)).Transform(CSUtilMath.Rotation(CSUtilMath.ToRadians(prongParam.ProngDomeSettingDistance), Vector3d.XAxis, rotationCenter));
		}
		else
		{
			val16 += val15 * (num3 / Math.Cos(angle));
			((Point3d)(ref val16)).Transform(CSUtilMath.Rotation(CSUtilMath.ToRadians(prongParam.ProngDomeSettingDistance), Vector3d.XAxis, rotationCenter));
		}
		Vector3d val17 = Vector3d.YAxis * -1.0;
		((Vector3d)(ref val17)).Rotate(CSUtilMath.ToRadians(prongParam.ProngDomeSettingAngle) + CSUtilMath.ToRadians(prongParam.ProngDomeSettingDistance), Vector3d.XAxis);
		((Vector3d)(ref val17)).Unitize();
		Vector3d vectorEnd = default(Vector3d);
		((Vector3d)(ref vectorEnd))._002Ector(val17);
		((Vector3d)(ref vectorEnd)).Reverse();
		if (!planarDome && flag)
		{
			val15 = -val4.TangentAtStart;
		}
		Curve val18 = (Curve)(object)CSUtilCurves.BlendCurve(val10.PointAtStart, val16, val15, val17, blendAmount, blendStart, blendEnd);
		Curve val19 = (Curve)(object)CSUtilCurves.BlendCurve(val10.PointAtEnd, val16, val15, vectorEnd, blendAmount2, blendStart2, blendEnd2);
		Curve[] array2 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val18, val19 });
		if (array2 == null)
		{
			return false;
		}
		if (array2.Length == 0)
		{
			return false;
		}
		NurbsSurface val20 = NurbsSurface.CreateFromCorners(new Point3d(-20.0, 0.0, -10.0), new Point3d(20.0, 0.0, -10.0), new Point3d(20.0, 0.0, 10.0), new Point3d(-20.0, 0.0, 10.0));
		CurveIntersections val21 = Intersection.CurveSurface(val10, (Surface)(object)val20, 0.01, 0.01);
		if (val21.Count <= 0)
		{
			return false;
		}
		double parameterA3 = val21[0].ParameterA;
		Curve[] array3 = val10.Split(parameterA3);
		Curve val22 = array3[0];
		Curve val23 = array3[1];
		if (val22 == null || val23 == null)
		{
			return false;
		}
		val23.Reverse();
		CurveIntersections val24 = Intersection.CurveSurface(val11, (Surface)(object)val20, 0.01, 0.01);
		if (!((IEnumerable<IntersectionEvent>)val24).Any())
		{
			return false;
		}
		double parameterA4 = val24[0].ParameterA;
		Curve[] array4 = val11.Split(parameterA4);
		Curve val25 = array4[0];
		Curve val26 = array4[1];
		if (val25 == null || val26 == null)
		{
			return false;
		}
		val26.Reverse();
		double[] array5 = new double[1];
		domain = val22.Domain;
		array5[0] = ((Interval)(ref domain)).Min;
		double[] array6 = new double[1];
		domain = val23.Domain;
		array6[0] = ((Interval)(ref domain)).Min;
		if (!CSUtilSweepRC.Sweep2(val22, val23, array2, array5, array6, out var breps, closed: false, maintainHeight: false))
		{
			return false;
		}
		double[] array7 = new double[1];
		domain = val25.Domain;
		array7[0] = ((Interval)(ref domain)).Min;
		double[] array8 = new double[1];
		domain = val25.Domain;
		array8[0] = ((Interval)(ref domain)).Min;
		if (!CSUtilSweepRC.Sweep2(val25, val26, array2, array7, array8, out var breps2, closed: false, maintainHeight: false))
		{
			return false;
		}
		Brep val27 = CSUtilRhino.JoinAllBreps_BackwardCompatibility(new List<Brep> { breps, breps2 }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (val27 == null)
		{
			return false;
		}
		Transform val28 = Transform.Translation(0.0, prongParam.ProngTopY / 2.0, 0.0);
		((GeometryBase)val27).Transform(val28);
		dome = val27;
		return true;
	}

	private static bool ProngExtension(double prongExtensionHeight, ref Curve prongRail, ref Curve prongTopProfile)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		Vector3d val = obj.TangentAt(((Interval)(ref domain)).Min);
		((Vector3d)(ref val)).Unitize();
		Point3d pointAtStart = prongRail.PointAtStart;
		Point3d val2 = pointAtStart - val * prongExtensionHeight;
		Transform val3 = Transform.Translation(val2 - pointAtStart);
		((GeometryBase)prongTopProfile).Transform(val3);
		LineCurve val4 = new LineCurve(pointAtStart, val2);
		Curve val5 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			prongRail,
			(Curve)val4
		})[0];
		Vector3d val6 = val5.PointAtStart - val2;
		if (((Vector3d)(ref val6)).Length > 0.01)
		{
			val5.Reverse();
		}
		prongRail = val5;
		return true;
	}
}
