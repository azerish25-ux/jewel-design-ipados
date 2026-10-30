using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlocks.Curves;

[CSBlock("MatchingBand", "{ECF32893-E33A-4967-8044-45D5043C27B1}")]
public sealed class CSMatchingBand : CSBlock
{
	public readonly CSInputChannel InRingRail;

	public readonly CSInputChannel InObjects;

	public readonly CSInputChannel InTopSampling;

	public readonly CSInputChannel InBotSampling;

	public readonly CSInputChannel InSampleDistance;

	public readonly CSInputChannel InPosition;

	public readonly CSInputChannel InOffset;

	public readonly CSInputChannel InOutsideRingRail;

	public readonly CSInputChannel InTaperingToggle;

	public readonly CSInputChannel InSimpleToggle;

	public readonly CSInputChannel InSimpleHeight;

	public readonly CSInputChannel InStraightBandToggle;

	public readonly CSInputChannel InMirror;

	public readonly CSInputChannel InMirrorSide;

	public readonly CSInputChannel InMinRadius;

	public readonly CSInputChannel InPullBackPercent;

	public readonly CSOutputChannel OutMatchingRail;

	public readonly CSOutputChannel OutMatchingOutsideRail;

	public CSMatchingBand(string identifier)
		: base(identifier)
	{
		InRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InTopSampling = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InBotSampling = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InSampleDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InPosition = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InOutsideRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InTaperingToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InSimpleToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InSimpleHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InStraightBandToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirrorSide = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InMinRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 5);
		InPullBackPercent = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		OutMatchingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		OutMatchingOutsideRail = new CSOutputChannel(this, ChannelDataType.OutsideRingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!CSDataHelper.GetCurve(InRingRail, out var value))
		{
			return UpdateError("Curve");
		}
		bool flag = false;
		if (CSDataHelper.GetCurve(InOutsideRingRail, out var value2))
		{
			flag = true;
		}
		InMirror.GetValue(out bool value3);
		InMirrorSide.GetValue(out int value4);
		InMinRadius.GetValue(out double value5);
		InPullBackPercent.GetValue(out double value6);
		CSData data = InObjects.Data;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetBoundingBox(ref boundingBox))
		{
			return UpdateError("Error getting BoundingBox");
		}
		double num = boundingBox.Min.Y - 1.0;
		double num2 = boundingBox.Max.Y + 1.0;
		double val = Math.Sqrt(Math.Pow(boundingBox.Max.X, 2.0) + Math.Pow(boundingBox.Max.Z, 2.0));
		double val2 = Math.Sqrt(Math.Pow(boundingBox.Min.X, 2.0) + Math.Pow(boundingBox.Max.Z, 2.0));
		double num3 = Math.Max(val, val2);
		InSampleDistance.GetValue(out double value7);
		value7 = num3 + value7;
		Surface surface = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(value7, num2, 0.0), new Point3d(value7, num, 0.0), new Point3d(0.0, num, 0.0));
		int value8 = 0;
		InTopSampling.GetInteger(ref value8);
		switch (value8)
		{
		case 1:
			value8 = 60;
			break;
		case 2:
			value8 = 45;
			break;
		case 3:
			value8 = 30;
			break;
		case 4:
			value8 = 18;
			break;
		case 5:
			value8 = 10;
			break;
		case 6:
			value8 = 5;
			break;
		case 7:
			value8 = 4;
			break;
		case 8:
			value8 = 2;
			break;
		case 9:
			value8 = 1;
			break;
		default:
			return UpdateError("Invalid Angle");
		}
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value8), Vector3d.YAxis, Point3d.Origin);
		int value9 = 0;
		InBotSampling.GetInteger(ref value9);
		switch (value9)
		{
		case 1:
			value9 = 60;
			break;
		case 2:
			value9 = 45;
			break;
		case 3:
			value9 = 30;
			break;
		case 4:
			value9 = 18;
			break;
		case 5:
			value9 = 10;
			break;
		case 6:
			value9 = 5;
			break;
		case 7:
			value9 = 4;
			break;
		case 8:
			value9 = 2;
			break;
		case 9:
			value9 = 1;
			break;
		default:
			return UpdateError("Invalid Angle");
		}
		Transform xform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value9), Vector3d.YAxis, Point3d.Origin);
		InPosition.GetValue(out bool value10);
		InSimpleToggle.GetValue(out bool value11);
		double value12 = 0.0;
		InOffset.GetDouble(ref value12);
		double num4 = 0.0;
		CSDataArray flattenArray = InObjects.GetFlattenArray();
		bool value13 = false;
		InTaperingToggle.GetValue(out value13);
		if (value13)
		{
			num4 = ((!value10) ? double.MaxValue : double.MinValue);
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				if (!flattenArray.GetArrayItem(i).GetBoundingBox(ref boundingBox))
				{
					continue;
				}
				if (value10)
				{
					if (boundingBox.Min.Y > num4 && boundingBox.Min.Y < 0.0)
					{
						num4 = boundingBox.Min.Y;
					}
				}
				else if (boundingBox.Max.Y < num4 && boundingBox.Max.Y > 0.0)
				{
					num4 = boundingBox.Max.Y;
				}
			}
		}
		Surface surface2 = null;
		Surface surface3;
		if (value10)
		{
			surface3 = Surface.CreateExtrusion(value, Vector3d.YAxis * (0.0 - num3));
			surface3.Translate(0.0, num2, 0.0);
			if (flag)
			{
				surface2 = Surface.CreateExtrusion(value2, Vector3d.YAxis * (0.0 - num3));
				surface2.Translate(0.0, num2, 0.0);
			}
		}
		else
		{
			surface3 = Surface.CreateExtrusion(value, Vector3d.YAxis * num3);
			surface3.Translate(0.0, 0.0 - num2, 0.0);
			if (flag)
			{
				surface2 = Surface.CreateExtrusion(value2, Vector3d.YAxis * num3);
				surface2.Translate(0.0, 0.0 - num2, 0.0);
			}
		}
		Point3dList point3dList = new Point3dList();
		Point3dList point3dList2 = new Point3dList();
		Point3dList point3dList3 = new Point3dList();
		double num5 = 0.0;
		InStraightBandToggle.GetValue(out bool value14);
		bool flag2 = true;
		while (flag2)
		{
			while (num5 <= 180.0)
			{
				CurveIntersections curveIntersections = Intersection.CurveSurface(value, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				Point3d point3d = Point3d.Unset;
				double num6 = double.MaxValue;
				for (int j = 0; j < curveIntersections.Count; j++)
				{
					Point3d pointA = curveIntersections[j].PointA;
					double num7 = Math.Sqrt(Math.Pow(pointA.X, 2.0) + Math.Pow(pointA.Y, 2.0));
					if (num7 < num6)
					{
						num6 = num7;
						point3d = pointA;
					}
				}
				CSDataArray flattenArray2 = InObjects.GetFlattenArray();
				if (flattenArray2.GetArrayCount() <= 0)
				{
					return UpdateMissing("Mising Object");
				}
				BoundingBox unset = BoundingBox.Unset;
				for (int k = 0; k < flattenArray2.GetArrayCount(); k++)
				{
					CSData arrayItem = flattenArray2.GetArrayItem(k);
					if (arrayItem == null)
					{
						continue;
					}
					CSDataHelper.GetBrep(arrayItem, out var value15);
					if (value15 != null && Intersection.BrepSurface(value15, surface, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var intersectionPoints) && (intersectionCurves.Length >= 1 || intersectionPoints.Length >= 1))
					{
						Curve[] array = intersectionCurves;
						for (int l = 0; l < array.Length; l++)
						{
							BoundingBox boundingBox2 = array[l].GetBoundingBox(accurate: true);
							unset.Union(boundingBox2);
						}
						Point3d[] array2 = intersectionPoints;
						foreach (Point3d point in array2)
						{
							unset.Union(point);
						}
					}
				}
				double y = unset.Min.Y;
				double y2 = unset.Max.Y;
				Point3d item;
				if (value10)
				{
					item = new Point3d(point3d.X, y - value12, point3d.Z);
					if (value13 && y > num4)
					{
						item.Y = num4 - value12;
					}
					if (value14)
					{
						item.Y = item.Y - y + num + 1.0;
					}
				}
				else
				{
					item = new Point3d(point3d.X, y2 + value12, point3d.Z);
					if (value13 && y2 < num4)
					{
						item.Y = num4 + value12;
					}
					if (value14)
					{
						item.Y = item.Y - y2 + num2 - 1.0;
					}
				}
				point3dList.Add(item);
				if (flag)
				{
					CurveIntersections curveIntersections2 = Intersection.CurveSurface(value2, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					point3d = Point3d.Unset;
					num6 = double.MaxValue;
					for (int m = 0; m < curveIntersections2.Count; m++)
					{
						Point3d pointA2 = curveIntersections2[m].PointA;
						double num8 = Math.Sqrt(Math.Pow(pointA2.X, 2.0) + Math.Pow(pointA2.Y, 2.0));
						if (num8 < num6)
						{
							num6 = num8;
							point3d = pointA2;
						}
					}
					flattenArray2 = InObjects.GetFlattenArray();
					if (flattenArray2.GetArrayCount() <= 0)
					{
						return UpdateMissing("Mising Object");
					}
					unset = BoundingBox.Unset;
					for (int n = 0; n < flattenArray2.GetArrayCount(); n++)
					{
						CSData arrayItem2 = flattenArray2.GetArrayItem(n);
						if (arrayItem2 == null)
						{
							continue;
						}
						CSDataHelper.GetBrep(arrayItem2, out var value16);
						if (value16 != null && Intersection.BrepSurface(value16, surface, CSApplicationHelper.Application.Tolerance, out var intersectionCurves2, out var intersectionPoints2) && (intersectionCurves2.Length >= 1 || intersectionPoints2.Length >= 1))
						{
							Curve[] array = intersectionCurves2;
							for (int l = 0; l < array.Length; l++)
							{
								BoundingBox boundingBox3 = array[l].GetBoundingBox(accurate: true);
								unset.Union(boundingBox3);
							}
							Point3d[] array2 = intersectionPoints2;
							foreach (Point3d point2 in array2)
							{
								unset.Union(point2);
							}
						}
					}
					y = unset.Min.Y;
					y2 = unset.Max.Y;
					if (value10)
					{
						item = new Point3d(point3d.X, y - value12, point3d.Z);
						if (value13 && y > num4)
						{
							item.Y = num4 - value12;
						}
						if (value14)
						{
							item.Y = item.Y - y + num + 1.0;
						}
					}
					else
					{
						item = new Point3d(point3d.X, y2 + value12, point3d.Z);
						if (value13 && y2 < num4)
						{
							item.Y = num4 + value12;
						}
						if (value14)
						{
							item.Y = item.Y - y2 + num2 - 1.0;
						}
					}
					point3dList2.Add(item);
				}
				if (num5 == 180.0)
				{
					break;
				}
				num5 += (double)value9;
				surface.Transform(xform2);
			}
			flag2 = false;
			Curve curve = Curve.CreateInterpolatedCurve(point3dList, 3);
			for (int num9 = 1; num9 < point3dList.Count; num9++)
			{
				curve.ClosestPoint(point3dList[num9], out var t);
				curve.ClosestPoint(point3dList[num9 - 1], out var t2);
				Vector3d a = curve.TangentAt(t);
				a.Unitize();
				Vector3d b = curve.TangentAt(t2);
				b.Unitize();
				CSBlockUtil.CSUtilMath.ToDegrees(Vector3d.VectorAngle(a, b));
				_ = 80.0 / num2;
			}
		}
		while (num5 <= 360.0)
		{
			CurveIntersections curveIntersections3 = Intersection.CurveSurface(value, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			Point3d point3d2 = Point3d.Unset;
			double num10 = double.MaxValue;
			for (int num11 = 0; num11 < curveIntersections3.Count; num11++)
			{
				Point3d pointA3 = curveIntersections3[num11].PointA;
				double num12 = Math.Sqrt(Math.Pow(pointA3.X, 2.0) + Math.Pow(pointA3.Y, 2.0));
				if (num12 < num10)
				{
					num10 = num12;
					point3d2 = pointA3;
				}
			}
			CSDataArray flattenArray3 = InObjects.GetFlattenArray();
			if (flattenArray3.GetArrayCount() <= 0)
			{
				return UpdateMissing("Mising Object");
			}
			double num13 = double.MaxValue;
			double num14 = double.MinValue;
			for (int num15 = 0; num15 < flattenArray3.GetArrayCount(); num15++)
			{
				CSData arrayItem3 = flattenArray3.GetArrayItem(num15);
				if (arrayItem3 == null)
				{
					continue;
				}
				CSDataHelper.GetBrep(arrayItem3, out var value17);
				if (value17 != null && Intersection.BrepSurface(value17, surface, CSApplicationHelper.Application.Tolerance, out var intersectionCurves3, out var intersectionPoints3) && (intersectionCurves3.Length >= 1 || intersectionPoints3.Length >= 1))
				{
					BoundingBox unset2 = BoundingBox.Unset;
					Curve[] array = intersectionCurves3;
					for (int l = 0; l < array.Length; l++)
					{
						BoundingBox boundingBox4 = array[l].GetBoundingBox(accurate: true);
						unset2.Union(boundingBox4);
					}
					Point3d[] array2 = intersectionPoints3;
					foreach (Point3d point3 in array2)
					{
						unset2.Union(point3);
					}
					if (unset2.Min.Y < num13)
					{
						num13 = unset2.Min.Y;
					}
					if (unset2.Max.Y > num14)
					{
						num14 = unset2.Max.Y;
					}
				}
			}
			if (num14 < num13)
			{
				num13 = point3dList.Last.Y + value12;
				num14 = point3dList.Last.Y - value12;
			}
			Point3d item2;
			if (value10)
			{
				item2 = new Point3d(point3d2.X, num13 - value12, point3d2.Z);
				if (value13 && num13 > num4)
				{
					item2.Y = num4 - value12;
				}
				if (value14)
				{
					item2.Y = item2.Y - num13 + num + 1.0;
				}
			}
			else
			{
				item2 = new Point3d(point3d2.X, num14 + value12, point3d2.Z);
				if (value13 && num14 < num4)
				{
					item2.Y = num4 + value12;
				}
				if (value14)
				{
					item2.Y = item2.Y - num14 + num2 - 1.0;
				}
			}
			point3dList.Add(item2);
			if (flag)
			{
				CurveIntersections curveIntersections4 = Intersection.CurveSurface(value2, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				point3d2 = Point3d.Unset;
				num10 = double.MaxValue;
				for (int num16 = 0; num16 < curveIntersections4.Count; num16++)
				{
					Point3d pointA4 = curveIntersections4[num16].PointA;
					double num17 = Math.Sqrt(Math.Pow(pointA4.X, 2.0) + Math.Pow(pointA4.Y, 2.0));
					if (num17 < num10)
					{
						num10 = num17;
						point3d2 = pointA4;
					}
				}
				flattenArray3 = InObjects.GetFlattenArray();
				if (flattenArray3.GetArrayCount() <= 0)
				{
					return UpdateMissing("Mising Object");
				}
				num13 = double.MaxValue;
				num14 = double.MinValue;
				for (int num18 = 0; num18 < flattenArray3.GetArrayCount(); num18++)
				{
					CSData arrayItem4 = flattenArray3.GetArrayItem(num18);
					if (arrayItem4 == null)
					{
						continue;
					}
					CSDataHelper.GetBrep(arrayItem4, out var value18);
					if (value18 != null && Intersection.BrepSurface(value18, surface, CSApplicationHelper.Application.Tolerance, out var intersectionCurves4, out var intersectionPoints4) && (intersectionCurves4.Length >= 1 || intersectionPoints4.Length >= 1))
					{
						BoundingBox unset3 = BoundingBox.Unset;
						Curve[] array = intersectionCurves4;
						for (int l = 0; l < array.Length; l++)
						{
							BoundingBox boundingBox5 = array[l].GetBoundingBox(accurate: true);
							unset3.Union(boundingBox5);
						}
						Point3d[] array2 = intersectionPoints4;
						foreach (Point3d point4 in array2)
						{
							unset3.Union(point4);
						}
						if (unset3.Min.Y < num13)
						{
							num13 = unset3.Min.Y;
						}
						if (unset3.Max.Y > num14)
						{
							num14 = unset3.Max.Y;
						}
					}
				}
				if (num14 < num13)
				{
					num13 = point3dList.Last.Y + value12;
					num14 = point3dList.Last.Y - value12;
				}
				if (value10)
				{
					item2 = new Point3d(point3d2.X, num13 - value12, point3d2.Z);
					if (value13 && num13 > num4)
					{
						item2.Y = num4 - value12;
					}
					if (value14)
					{
						item2.Y = item2.Y - num13 + num + 1.0;
					}
				}
				else
				{
					item2 = new Point3d(point3d2.X, num14 + value12, point3d2.Z);
					if (value13 && num14 < num4)
					{
						item2.Y = num4 + value12;
					}
					if (value14)
					{
						item2.Y = item2.Y - num14 + num2 - 1.0;
					}
				}
				if (value11)
				{
					point3dList3.Add(item2);
				}
				else
				{
					point3dList2.Add(item2);
				}
			}
			num5 += (double)value8;
			surface.Transform(xform);
		}
		Curve curve2 = Curve.CreateControlPointCurve(point3dList, 3);
		if (curve2 == null)
		{
			return UpdateError("Error getting control point curve");
		}
		Curve curve3 = curve2.DuplicateCurve();
		curve3.Transform(Transform.Scale(Plane.WorldXY, 1.5, 1.0, 1.5));
		Curve curve4 = curve2.DuplicateCurve();
		curve4.Transform(Transform.Scale(Plane.WorldXY, 0.5, 1.0, 0.5));
		Brep[] array3 = Brep.CreateFromLoft(new Curve[2] { curve4, curve3 }, Point3d.Unset, Point3d.Unset, LoftType.Normal, closed: false);
		if (array3.Length < 1)
		{
			return UpdateError("Error getting loft surface");
		}
		Intersection.BrepSurface(array3[0], surface3, CSApplicationHelper.Application.Tolerance, out var intersectionCurves5, out var intersectionPoints5);
		if (intersectionCurves5.Length < 1)
		{
			return UpdateError("Error getting loft intersect");
		}
		Curve curve5 = intersectionCurves5[0];
		double num19;
		double t4;
		double num21;
		double num20;
		if (flag)
		{
			Curve curve6 = Curve.CreateControlPointCurve(point3dList2, 3);
			Curve curve7 = curve6.DuplicateCurve();
			curve7.Transform(Transform.Scale(Plane.WorldXY, 1.5, 1.0, 1.5));
			Curve curve8 = curve6.DuplicateCurve();
			curve8.Transform(Transform.Scale(Plane.WorldXY, 0.5, 1.0, 0.5));
			Brep[] array4 = Brep.CreateFromLoft(new Curve[2] { curve8, curve7 }, Point3d.Unset, Point3d.Unset, LoftType.Normal, closed: false);
			if (array4.Length < 1)
			{
				return UpdateError("Error getting loft surface");
			}
			Intersection.BrepSurface(array4[0], surface2, CSApplicationHelper.Application.Tolerance, out intersectionCurves5, out intersectionPoints5);
			Curve curve9 = intersectionCurves5[0];
			if (value11)
			{
				Curve curve10 = Curve.CreateControlPointCurve(point3dList3, 3);
				curve10.MakeDeformable();
				Point3d point3d3 = curve10.PointAtNormalizedLength(0.5);
				InSimpleHeight.GetValue(out double value19);
				Point3d pointAtStart = curve10.PointAtStart;
				Point3d point3d4 = new Point3d(0.0, pointAtStart.Y, 0.0);
				Vector3d vector3d = pointAtStart - point3d4;
				curve10.Transform(Transform.Scale(Plane.WorldXY, 1.0, 1.0, (value19 + vector3d.Length) / point3d3.Z));
				Curve[] array5 = Curve.JoinCurves(new List<Curve> { curve9, curve10 }, 0.01);
				if (array5.Length < 1)
				{
					return UpdateError();
				}
				curve9 = array5[0];
			}
			double newScale = curve9.PointAtStart.DistanceTo(curve9.PointAtNormalizedLength(0.5)) / 2.0;
			surface = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(0.0, num2, value7), new Point3d(0.0, num, value7), new Point3d(0.0, num, 0.0));
			CurveIntersections curveIntersections5 = Intersection.CurveSurface(curve9, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			curve9.ClosestPoint(curveIntersections5[0].PointA, out var t3);
			curve9.ChangeClosedCurveSeam(t3);
			if (!Curve.DoDirectionsMatch(curve9, value2))
			{
				curve9.Reverse();
			}
			num19 = 0.0;
			num20 = 0.0;
			num21 = value5;
			while (num19 <= 90.0)
			{
				Point3d testPoint = curve9.PointAtNormalizedLength(num20);
				curve9.ClosestPoint(testPoint, out t4);
				if (1.0 / curve9.CurvatureAt(t4).Length < num21)
				{
					curve9.NormalizedLengthParameter(t4, out var t5);
					double num22 = t5;
					double num23 = t5;
					if (t5 == 0.0)
					{
						num22 = 1.0;
					}
					if (t5 == 1.0)
					{
						num23 = 0.0;
					}
					num20 = num22 * curve9.GetLength();
					double t6 = num20 - num21 / 2.0 * value6;
					curve9.ClosestPoint(curve9.PointAtLength(t6), out t6);
					num20 = num23 * curve9.GetLength();
					double t7 = num20 + num21 / 2.0 * value6;
					curve9.ClosestPoint(curve9.PointAtLength(t7), out t7);
					Curve curve11 = curve9.Trim(new Interval(t7, t6));
					NurbsCurve item3 = CSUtilCurves.BlendCurve(curve11.PointAtStart, curve11.PointAtEnd, -curve11.TangentAtStart, curve11.TangentAtEnd);
					curve9 = Curve.JoinCurves(new List<Curve> { curve11, item3 })[0].DuplicateCurve();
				}
				num19 += (double)value8 / 3.0;
				num20 += (double)value8 / 3.0 / 360.0;
			}
			while (num19 < 270.0)
			{
				Point3d testPoint2 = curve9.PointAtNormalizedLength(num20);
				curve9.ClosestPoint(testPoint2, out t4);
				if (1.0 / curve9.CurvatureAt(t4).Length < num21)
				{
					curve9.NormalizedLengthParameter(t4, out var t8);
					double num24 = t8;
					double num25 = t8;
					if (t8 == 0.0)
					{
						num24 = 1.0;
					}
					if (t8 == 1.0)
					{
						num25 = 0.0;
					}
					num20 = num24 * curve9.GetLength();
					double t9 = num20 - num21 / 2.0 * value6;
					curve9.ClosestPoint(curve9.PointAtLength(t9), out t9);
					num20 = num25 * curve9.GetLength();
					double t10 = num20 + num21 / 2.0 * value6;
					curve9.ClosestPoint(curve9.PointAtLength(t10), out t10);
					Curve curve12 = curve9.Trim(new Interval(t10, t9));
					NurbsCurve item4 = CSUtilCurves.BlendCurve(curve12.PointAtStart, curve12.PointAtEnd, -curve12.TangentAtStart, curve12.TangentAtEnd);
					curve9 = Curve.JoinCurves(new List<Curve> { curve12, item4 })[0].DuplicateCurve();
				}
				num19 += (double)value9 / 3.0;
				num20 += (double)value9 / 3.0 / 360.0;
			}
			while (num19 <= 360.0)
			{
				Point3d testPoint3 = curve9.PointAtNormalizedLength(num20);
				curve9.ClosestPoint(testPoint3, out t4);
				if (1.0 / curve9.CurvatureAt(t4).Length < num21)
				{
					curve9.NormalizedLengthParameter(t4, out var t11);
					double num26 = t11;
					double num27 = t11;
					if (t11 == 0.0)
					{
						num26 = 1.0;
					}
					if (t11 == 1.0)
					{
						num27 = 0.0;
					}
					num20 = num26 * curve9.GetLength();
					double t12 = num20 - num21 / 2.0 * value6;
					curve9.ClosestPoint(curve9.PointAtLength(t12), out t12);
					num20 = num27 * curve9.GetLength();
					double t13 = num20 + num21 / 2.0 * value6;
					curve9.ClosestPoint(curve9.PointAtLength(t13), out t13);
					Curve curve13 = curve9.Trim(new Interval(t13, t12));
					NurbsCurve item5 = CSUtilCurves.BlendCurve(curve13.PointAtStart, curve13.PointAtEnd, -curve13.TangentAtStart, curve13.TangentAtEnd);
					curve9 = Curve.JoinCurves(new List<Curve> { curve13, item5 })[0].DuplicateCurve();
				}
				num19 += (double)value8 / 3.0;
				num20 += (double)value8 / 3.0 / 360.0;
			}
			surface = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(0.0, num2, value7), new Point3d(0.0, num, value7), new Point3d(0.0, num, 0.0));
			curveIntersections5 = Intersection.CurveSurface(curve9, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			curve9.ClosestPoint(curveIntersections5[0].PointA, out t3);
			curve9.ChangeClosedCurveSeam(t3);
			if (!Curve.DoDirectionsMatch(curve9, value2))
			{
				curve9.Reverse();
			}
			if (value3)
			{
				double t14;
				double t15;
				if (value4 == 0)
				{
					curve9.NormalizedLengthParameter(0.01, out t14);
					NurbsSurface surface4 = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(1.0, num2, 0.0 - value7), new Point3d(1.0, num, 0.0 - value7), new Point3d(0.0, num, 0.0));
					CurveIntersections curveIntersections6 = Intersection.CurveSurface(curve9, surface4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					curve9.ClosestPoint(curveIntersections6[0].PointA, out t15);
					curve9 = curve9.Trim(t14, t15);
					Curve curve14 = curve9.DuplicateCurve();
					curve14.Transform(Transform.Mirror(Plane.WorldYZ));
					NurbsCurve nurbsCurve = CSUtilCurves.BlendCurve(curve9.PointAtStart, curve14.PointAtStart, -curve9.TangentAtStart, -curve14.TangentAtStart);
					NurbsCurve nurbsCurve2 = CSUtilCurves.BlendCurve(curve9.PointAtEnd, curve14.PointAtEnd, curve9.TangentAtEnd, curve14.TangentAtEnd);
					CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[4] { curve9, nurbsCurve, curve14, nurbsCurve2 }, out Curve mergedCrv, 0.01);
					curve9 = mergedCrv;
				}
				if (value4 == 1)
				{
					NurbsSurface surface5 = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(-1.0, num2, 0.0 - value7), new Point3d(-1.0, num, 0.0 - value7), new Point3d(0.0, num, 0.0));
					CurveIntersections curveIntersections7 = Intersection.CurveSurface(curve9, surface5, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					curve9.ClosestPoint(curveIntersections7[0].PointA, out t14);
					curve9.NormalizedLengthParameter(0.99, out t15);
					curve9 = curve9.Trim(t14, t15);
					Curve curve15 = curve9.DuplicateCurve();
					curve15.Transform(Transform.Mirror(Plane.WorldYZ));
					NurbsCurve nurbsCurve3 = CSUtilCurves.BlendCurve(curve9.PointAtStart, curve15.PointAtStart, -curve9.TangentAtStart, -curve15.TangentAtStart);
					NurbsCurve nurbsCurve4 = CSUtilCurves.BlendCurve(curve9.PointAtEnd, curve15.PointAtEnd, curve9.TangentAtEnd, curve15.TangentAtEnd);
					CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[4] { curve9, nurbsCurve3, curve15, nurbsCurve4 }, out Curve mergedCrv2, 0.01);
					curve9 = mergedCrv2;
				}
				curveIntersections5 = Intersection.CurveSurface(curve9, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				curve9.ClosestPoint(curveIntersections5[0].PointA, out t3);
				curve9.ChangeClosedCurveSeam(t3);
				if (!Curve.DoDirectionsMatch(curve9, value2))
				{
					curve9.Reverse();
				}
			}
			OnNurbsCurve nCrv = ((OnCurve)Interop.ToOnCurve(curve9)).NurbsCurve();
			OutMatchingOutsideRail.SetData(new CSOutsideRingRailData(nCrv.Convert(), newScale));
		}
		surface = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(0.0, num2, value7), new Point3d(0.0, num, value7), new Point3d(0.0, num, 0.0));
		CurveIntersections curveIntersections8 = Intersection.CurveSurface(curve5, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curve5.ClosestPoint(curveIntersections8[0].PointA, out var t16);
		curve5.ChangeClosedCurveSeam(t16);
		if (!Curve.DoDirectionsMatch(curve5, value))
		{
			curve5.Reverse();
		}
		double radius = curve5.PointAtNormalizedLength(0.25).DistanceTo(curve5.PointAtNormalizedLength(0.75)) / 2.0;
		num19 = 0.0;
		t4 = 0.0;
		num20 = 0.0;
		num21 = value5;
		while (num19 <= 90.0)
		{
			Point3d testPoint4 = curve5.PointAtNormalizedLength(num20);
			curve5.ClosestPoint(testPoint4, out t4);
			Point3d point3d5 = curve5.PointAt(t4);
			Vector3d vector3d2 = curve5.CurvatureAt(t4);
			double num28 = 1.0 / vector3d2.Length;
			vector3d2.Unitize();
			Point3d to = point3d5 + vector3d2 * num28;
			new Line(point3d5, to);
			if (num28 < num21)
			{
				curve5.NormalizedLengthParameter(t4, out var t17);
				double num29 = t17;
				double num30 = t17;
				if (t17 == 0.0)
				{
					num29 = 1.0;
				}
				if (t17 == 1.0)
				{
					num30 = 0.0;
				}
				num20 = num29 * curve5.GetLength();
				double t18 = num20 - num21 / 2.0 * value6;
				curve5.ClosestPoint(curve5.PointAtLength(t18), out t18);
				num20 = num30 * curve5.GetLength();
				double t19 = num20 + num21 / 2.0 * value6;
				curve5.ClosestPoint(curve5.PointAtLength(t19), out t19);
				Curve curve16 = curve5.Trim(new Interval(t19, t18));
				NurbsCurve item6 = CSUtilCurves.BlendCurve(curve16.PointAtStart, curve16.PointAtEnd, -curve16.TangentAtStart, curve16.TangentAtEnd);
				curve5 = Curve.JoinCurves(new List<Curve> { curve16, item6 })[0].DuplicateCurve();
			}
			num19 += (double)value8 / 3.0;
			num20 += (double)value8 / 3.0 / 360.0;
		}
		while (num19 < 270.0)
		{
			Point3d testPoint5 = curve5.PointAtNormalizedLength(num20);
			curve5.ClosestPoint(testPoint5, out t4);
			if (1.0 / curve5.CurvatureAt(t4).Length < num21)
			{
				curve5.NormalizedLengthParameter(t4, out var t20);
				double num31 = t20;
				double num32 = t20;
				if (t20 == 0.0)
				{
					num31 = 1.0;
				}
				if (t20 == 1.0)
				{
					num32 = 0.0;
				}
				num20 = num31 * curve5.GetLength();
				double t21 = num20 - num21 / 2.0 * value6;
				curve5.ClosestPoint(curve5.PointAtLength(t21), out t21);
				num20 = num32 * curve5.GetLength();
				double t22 = num20 + num21 / 2.0 * value6;
				curve5.ClosestPoint(curve5.PointAtLength(t22), out t22);
				Curve curve17 = curve5.Trim(new Interval(t22, t21));
				NurbsCurve item7 = CSUtilCurves.BlendCurve(curve17.PointAtStart, curve17.PointAtEnd, -curve17.TangentAtStart, curve17.TangentAtEnd);
				curve5 = Curve.JoinCurves(new List<Curve> { curve17, item7 })[0].DuplicateCurve();
			}
			num19 += (double)value9 / 3.0;
			num20 += (double)value9 / 3.0 / 360.0;
		}
		while (num19 <= 360.0)
		{
			Point3d testPoint6 = curve5.PointAtNormalizedLength(num20);
			curve5.ClosestPoint(testPoint6, out t4);
			curve5.PointAt(t4);
			if (1.0 / curve5.CurvatureAt(t4).Length < num21)
			{
				curve5.NormalizedLengthParameter(t4, out var t23);
				double num33 = t23;
				double num34 = t23;
				if (t23 == 0.0)
				{
					num33 = 1.0;
				}
				if (t23 == 1.0)
				{
					num34 = 0.0;
				}
				num20 = num33 * curve5.GetLength();
				double t24 = num20 - num21 / 2.0 * value6;
				curve5.ClosestPoint(curve5.PointAtLength(t24), out t24);
				num20 = num34 * curve5.GetLength();
				double t25 = num20 + num21 / 2.0 * value6;
				curve5.ClosestPoint(curve5.PointAtLength(t25), out t25);
				Curve curve18 = curve5.Trim(new Interval(t25, t24));
				NurbsCurve item8 = CSUtilCurves.BlendCurve(curve18.PointAtStart, curve18.PointAtEnd, -curve18.TangentAtStart, curve18.TangentAtEnd);
				curve5 = Curve.JoinCurves(new List<Curve> { curve18, item8 })[0].DuplicateCurve();
			}
			num19 += (double)value8 / 3.0;
			num20 += (double)value8 / 3.0 / 360.0;
		}
		curveIntersections8 = Intersection.CurveSurface(curve5, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curve5.ClosestPoint(curveIntersections8[0].PointA, out t16);
		curve5.ChangeClosedCurveSeam(t16);
		if (!Curve.DoDirectionsMatch(curve5, value))
		{
			curve5.Reverse();
		}
		if (value3)
		{
			double t26;
			double t27;
			if (value4 == 0)
			{
				curve5.NormalizedLengthParameter(0.01, out t26);
				NurbsSurface surface6 = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(1.0, num2, 0.0 - value7), new Point3d(1.0, num, 0.0 - value7), new Point3d(0.0, num, 0.0));
				CurveIntersections curveIntersections9 = Intersection.CurveSurface(curve5, surface6, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				curve5.ClosestPoint(curveIntersections9[0].PointA, out t27);
				curve5 = curve5.Trim(t26, t27);
				Curve curve19 = curve5.DuplicateCurve();
				curve19.Transform(Transform.Mirror(Plane.WorldYZ));
				NurbsCurve nurbsCurve5 = CSUtilCurves.BlendCurve(curve5.PointAtStart, curve19.PointAtStart, -curve5.TangentAtStart, -curve19.TangentAtStart);
				NurbsCurve nurbsCurve6 = CSUtilCurves.BlendCurve(curve5.PointAtEnd, curve19.PointAtEnd, curve5.TangentAtEnd, curve19.TangentAtEnd);
				CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[4] { curve5, nurbsCurve5, curve19, nurbsCurve6 }, out Curve mergedCrv3, 0.01);
				curve5 = mergedCrv3;
			}
			if (value4 == 1)
			{
				NurbsSurface surface7 = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(-1.0, num2, 0.0 - value7), new Point3d(-1.0, num, 0.0 - value7), new Point3d(0.0, num, 0.0));
				CurveIntersections curveIntersections10 = Intersection.CurveSurface(curve5, surface7, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				curve5.ClosestPoint(curveIntersections10[0].PointA, out t26);
				curve5.NormalizedLengthParameter(0.99, out t27);
				curve5 = curve5.Trim(t26, t27);
				Curve curve20 = curve5.DuplicateCurve();
				curve20.Transform(Transform.Mirror(Plane.WorldYZ));
				NurbsCurve nurbsCurve7 = CSUtilCurves.BlendCurve(curve5.PointAtStart, curve20.PointAtStart, -curve5.TangentAtStart, -curve20.TangentAtStart);
				NurbsCurve nurbsCurve8 = CSUtilCurves.BlendCurve(curve5.PointAtEnd, curve20.PointAtEnd, curve5.TangentAtEnd, curve20.TangentAtEnd);
				CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[4] { curve5, nurbsCurve7, curve20, nurbsCurve8 }, out Curve mergedCrv4, 0.01);
				curve5 = mergedCrv4;
			}
			curveIntersections8 = Intersection.CurveSurface(curve5, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			curve5.ClosestPoint(curveIntersections8[0].PointA, out t16);
			curve5.ChangeClosedCurveSeam(t16);
			if (!Curve.DoDirectionsMatch(curve5, value))
			{
				curve5.Reverse();
			}
		}
		OnNurbsCurve nCrv2 = ((OnCurve)Interop.ToOnCurve(curve5)).NurbsCurve();
		OutMatchingRail.SetData(new CSRingRailData(nCrv2.Convert(), radius));
		return base.Update();
	}
}
