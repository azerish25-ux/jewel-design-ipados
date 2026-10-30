using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1;

[CSBlock("HeadProngs", "{00E763B9-11A6-4D75-B41D-524CF3089970}")]
public sealed class CSHeadProngs : CSBlock
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InProngLayout;

	public readonly CSInputChannel InProngRails;

	public readonly CSInputChannel InHeadTop;

	public readonly CSInputChannel InHeadBase;

	public readonly CSInputChannel InEnableVProngs;

	public readonly CSInputChannel InProngVBlendAmount;

	public readonly CSInputChannel InProngTopProfile;

	public readonly CSInputChannel InProngTopX;

	public readonly CSInputChannel InProngTopY;

	public readonly CSInputChannel InProngTopNudge;

	public readonly CSInputChannel InProngVTopAngle;

	public readonly CSInputChannel InEnableBase;

	public readonly CSInputChannel InProngBaseProfile;

	public readonly CSInputChannel InProngBaseX;

	public readonly CSInputChannel InProngBaseY;

	public readonly CSInputChannel InProngBaseNudge;

	public readonly CSInputChannel InProngVBaseAngle;

	public readonly CSInputChannel InProngDomeHeight;

	public readonly CSInputChannel InProngDomeSettingDistance;

	public readonly CSInputChannel InProngDomeSettingAngle;

	public readonly CSInputChannel InBaseStyle;

	public readonly CSInputChannel InBasePointSize;

	public readonly CSInputChannel InEnableModeDoItYourself;

	public readonly CSInputChannel InProngBaseDomeHeight;

	public readonly CSInputChannel InFastMulti;

	public readonly CSOutputChannel OutObjects;

	public readonly CSOutputChannel OutCurves;

	public CSHeadProngs(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InProngLayout = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InProngRails = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InHeadTop = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InHeadBase = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InEnableVProngs = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 125);
		InProngVBlendAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngTopProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProngTopX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 2.0);
		InProngTopY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 2.0);
		InProngTopNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngVTopAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEnableBase = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProngBaseProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProngBaseX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 2.0);
		InProngBaseY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 2.0);
		InProngBaseNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngVBaseAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngDomeHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InProngDomeSettingDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngDomeSettingAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBaseStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 122);
		InBasePointSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.1);
		InEnableModeDoItYourself = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProngBaseDomeHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFastMulti = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutObjects = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGems.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		bool boolean = InFastMulti.Boolean;
		Curve curve = InProngTopProfile.Curve;
		if (curve == null)
		{
			return UpdateMissing("Prong Top Profile");
		}
		double num = InProngTopX.Double;
		double num2 = InProngTopY.Double;
		double num3 = InProngTopNudge.Double;
		double num4 = InProngVTopAngle.Double;
		double num5 = 0.0 - num2 * num3;
		HeadStyle integer = (HeadStyle)InProngLayout.Integer;
		ValueListItem valueListItem = InEnableVProngs.ValueListItem;
		bool boolean2 = InEnableBase.Boolean;
		bool boolean3 = InEnableModeDoItYourself.Boolean;
		double prongVBlend = InProngVBlendAmount.Double;
		Curve curve2;
		double prongBaseX;
		double num6;
		double prongVBaseAngle;
		double prongBaseOffset;
		if (boolean2)
		{
			curve2 = InProngBaseProfile.Curve;
			if (curve2 == null)
			{
				return UpdateMissing("Prong Base Profile");
			}
			prongBaseX = InProngBaseX.Double;
			num6 = InProngBaseY.Double;
			double num7 = InProngBaseNudge.Double;
			prongVBaseAngle = InProngVBaseAngle.Double;
			prongBaseOffset = 0.0 - num6 * num7;
		}
		else
		{
			curve2 = curve.DuplicateCurve();
			prongBaseX = num;
			num6 = num2;
			prongVBaseAngle = num4;
			prongBaseOffset = num5;
		}
		double prongDomeHeight = InProngDomeHeight.Double;
		double prongDomeSettingDistance = InProngDomeSettingDistance.Double;
		double prongDomeSettingAngle = InProngDomeSettingAngle.Double;
		double prongBaseDomeHeight = InProngBaseDomeHeight.Double;
		ValueListItem valueListItem2 = InBaseStyle.ValueListItem;
		double basePointSize = InBasePointSize.Double;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		int num8 = gemList.Count;
		if (boolean)
		{
			num8 = 1;
		}
		for (int i = 0; i < num8; i++)
		{
			CSDataArray cSDataArray3 = new CSDataArray();
			CSDataArray cSDataArray4 = new CSDataArray();
			CSGemData cSGemData = gemList[i];
			if (cSGemData == null)
			{
				return UpdateError("gemDatas is invalid");
			}
			if (!cSGemData.GetGemXformRC(out var _, out var xUp))
			{
				continue;
			}
			Curve curve3 = InHeadTop.Curve;
			if (curve3 == null)
			{
				return UpdateError("Head Top Crv is invalid");
			}
			cSDataArray3.Append(new CSOnGeometryData(curve3, xUp));
			Curve curve4 = InHeadBase.Curve;
			if (curve4 == null)
			{
				return UpdateError("Head Base Crv is invalid");
			}
			cSDataArray3.Append(new CSOnGeometryData(curve4, xUp));
			List<Curve> curveList = InProngRails.CurveList;
			if (curveList == null)
			{
				return UpdateMissing("ProngRailArray input incorrect type");
			}
			List<CSHeadStyles.HeadProngInfo> list = CSHeadStyles.GetHeadProngInfo(integer).ToList();
			if (list.Count <= 0)
			{
				return UpdateError("ProngRailArray is invalid");
			}
			for (int j = 0; j < list.Count; j++)
			{
				CSHeadStyles.HeadProngInfo headProngInfo = list[j];
				Curve curve5 = curveList[j];
				if (curve5 == null)
				{
					CS.CSUtil.Print("Missing Prong Rail\n");
					continue;
				}
				curve3.ClosestPoint(curve5.PointAtStart, out var t);
				curve4.ClosestPoint(curve5.PointAtEnd, out var t2);
				if ((headProngInfo.Type == CSHeadStyles.EHeadProngType.Straight && valueListItem == ValueListItem.HeadBuilderProngOffsetLookup) || valueListItem == ValueListItem.HeadBuilderProngOffsetForce)
				{
					if (!MakeVProngs(curve5, curve3, t, num4, num2, num5, prongVBlend, curve4, t2, prongVBaseAngle, num6, prongBaseOffset, valueListItem2, prongBaseDomeHeight, prongDomeHeight, prongDomeSettingDistance, prongDomeSettingAngle, xUp, boolean3, out var outCurves, out var outProng, out var error))
					{
						return UpdateError(error);
					}
					foreach (CSOnGeometryData item in outCurves)
					{
						cSDataArray3.Append(item);
					}
					if (outProng != null)
					{
						cSDataArray4.Append(outProng);
					}
					continue;
				}
				if (!MakeProfileSweepProngs(j, curveList, curve5, cSGemData, curve3, curve, t, num, num2, num5, curve4, curve2, t2, prongBaseX, num6, prongBaseOffset, valueListItem2, basePointSize, prongDomeHeight, prongDomeSettingDistance, prongDomeSettingAngle, prongBaseDomeHeight, xUp, boolean3, headProngInfo, out var outCurves2, out var outProngs, out var error2))
				{
					return UpdateError(error2);
				}
				foreach (CSOnGeometryData item2 in outCurves2)
				{
					cSDataArray3.Append(item2);
				}
				foreach (CSOnGeometryData item3 in outProngs)
				{
					cSDataArray4.Append(item3);
				}
			}
			cSDataArray.Append(cSDataArray3);
			cSDataArray2.Append(cSDataArray4);
		}
		OutObjects.SetData(cSDataArray2);
		OutCurves.SetData(cSDataArray);
		return base.Update();
	}

	private static bool ArrangeGeometry(CSHeadStyles.HeadProngInfo headProngInfo, Transform xUp, Vector3d headTopTan, Vector3d headTopNrm, double prongTopX, GeometryBase[] arrGeometry, Brep cutter, ref List<CSOnGeometryData> outputGeometries)
	{
		switch (headProngInfo.Arrangment)
		{
		case CSHeadStyles.EHeadProngArrangment.Single:
		{
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry4 = CutGeometry(array[i], cutter, xUp, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry4));
			}
			break;
		}
		case CSHeadStyles.EHeadProngArrangment.Double:
		{
			double num3 = headProngInfo.Overlap * prongTopX;
			Transform transform4 = Transform.Translation(headTopTan * 1.0 * num3);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry5 = CutGeometry(array[i], cutter, xUp * transform4, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry5));
			}
			Transform transform5 = Transform.Translation(headTopTan * -1.0 * num3);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry6 = CutGeometry(array[i], cutter, xUp * transform5, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry6));
			}
			break;
		}
		case CSHeadStyles.EHeadProngArrangment.Triple:
		{
			double num = headProngInfo.Overlap * prongTopX;
			Transform transform = Transform.Translation(headTopTan * 1.0 * num);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry = CutGeometry(array[i], cutter, xUp * transform, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry));
			}
			Transform transform2 = Transform.Translation(headTopTan * -1.0 * num);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry2 = CutGeometry(array[i], cutter, xUp * transform2, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry2));
			}
			double num2 = headProngInfo.Overlap * prongTopX * 2.0 * Math.Sqrt(3.0) / 2.0;
			Transform transform3 = Transform.Translation(headTopNrm * num2);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry3 = CutGeometry(array[i], cutter, xUp * transform3, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry3));
			}
			break;
		}
		default:
			return false;
		}
		return true;
	}

	private static bool CreateProngDome(Curve prongProfile, double prongX, double prongY, double angle, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, double fixScale, ref Brep dome)
	{
		BoundingBox boundingBox = prongProfile.GetBoundingBox(accurate: true);
		Transform transform = Transform.Scale(Plane.WorldXY, prongX / boundingBox.Diagonal.X, prongY / boundingBox.Diagonal.Y * fixScale, 0.0);
		Transform transform2 = Transform.Translation(0.0, (0.0 - prongY) / 2.0, 0.0);
		Curve curve = prongProfile.DuplicateCurve();
		curve.Transform(transform2 * transform);
		LineCurve lineCurve = new LineCurve(new Point3d(0.0, -50.0, 0.0), new Point3d(0.0, 50.0, 0.0));
		CurveIntersections curveIntersections = Intersection.CurveCurve(curve, lineCurve, 0.01, 0.01);
		if (curveIntersections.Count <= 1)
		{
			return false;
		}
		double parameterA = curveIntersections[0].ParameterA;
		double t = curveIntersections[1].ParameterA;
		if (Math.Abs(parameterA - curve.Domain.Min) > 0.0001 && Math.Abs(parameterA - curve.Domain.Max) > 0.0001)
		{
			Point3d testPoint = curve.PointAt(t);
			curve.ChangeClosedCurveSeam(parameterA);
			curve.ClosestPoint(testPoint, out t);
		}
		Curve[] array = curve.Split(t);
		Curve curve2 = array[0];
		Curve curve3 = array[1];
		if (curve2 == null || curve3 == null)
		{
			return false;
		}
		Vector3d vector3d = lineCurve.TangentAt(lineCurve.Domain.Mid);
		vector3d.Unitize();
		Vector3d vector3d2 = curve2.TangentAt(curve2.Domain.Mid);
		vector3d2.Unitize();
		if (vector3d * vector3d2 < 0.0)
		{
			curve2.Reverse();
		}
		Vector3d vector3d3 = curve3.TangentAt(curve3.Domain.Mid);
		vector3d3.Unitize();
		if (vector3d * vector3d3 < 0.0)
		{
			curve3.Reverse();
		}
		Vector3d zAxis = Vector3d.ZAxis;
		bool num = prongDomeHeight > 0.0;
		if (!num)
		{
			zAxis.Z = 0.0 - zAxis.Z;
			prongDomeHeight = 0.0 - prongDomeHeight;
		}
		zAxis.Rotate(0.0 - angle, Vector3d.XAxis);
		zAxis.Unitize();
		Point3d center = boundingBox.Center;
		center.Transform(transform2 * transform);
		if (num)
		{
			center += Vector3d.ZAxis * prongDomeHeight;
			center += Vector3d.YAxis * prongDomeSettingDistance * fixScale;
		}
		else
		{
			center += zAxis * (prongDomeHeight / Math.Cos(angle));
			center += Vector3d.YAxis * prongDomeSettingDistance;
		}
		Vector3d vector3d4 = Vector3d.YAxis * -1.0;
		vector3d4.Rotate(CSBlockUtil.CSUtilMath.ToRadians(prongDomeSettingAngle), Vector3d.XAxis);
		vector3d4.Unitize();
		Vector3d vectorEnd = new Vector3d(vector3d4);
		vectorEnd.Reverse();
		Curve curve4 = CSUtilCurves.BlendCurve(curve2.PointAtStart, center, zAxis, vector3d4);
		Curve curve5 = CSUtilCurves.BlendCurve(curve2.PointAtEnd, center, zAxis, vectorEnd);
		Curve[] array2 = Curve.JoinCurves(new Curve[2] { curve4, curve5 });
		if (array2 == null)
		{
			return false;
		}
		if (array2.Length == 0)
		{
			return false;
		}
		LineCurve curveB = new LineCurve(new Point3d(-50.0, 0.0, 0.0), new Point3d(50.0, 0.0, 0.0));
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(curve2, curveB, 0.01, 0.01);
		if (curveIntersections2.Count <= 0)
		{
			return false;
		}
		double parameterA2 = curveIntersections2[0].ParameterA;
		Curve[] array3 = curve2.Split(parameterA2);
		Curve curve6 = array3[0];
		Curve curve7 = array3[1];
		if (curve6 == null || curve7 == null)
		{
			return false;
		}
		curve7.Reverse();
		CurveIntersections curveIntersections3 = Intersection.CurveCurve(curve3, curveB, 0.01, 0.01);
		if (!curveIntersections3.Any())
		{
			return false;
		}
		double parameterA3 = curveIntersections3[0].ParameterA;
		Curve[] array4 = curve3.Split(parameterA3);
		Curve curve8 = array4[0];
		Curve curve9 = array4[1];
		if (curve8 == null || curve9 == null)
		{
			return false;
		}
		curve9.Reverse();
		if (!CSUtilSweepRC.Sweep2(curve6, curve7, array2, new double[1] { curve6.Domain.Min }, new double[1] { curve7.Domain.Min }, out var breps, closed: false, maintainHeight: false))
		{
			return false;
		}
		if (!CSUtilSweepRC.Sweep2(curve8, curve9, array2, new double[1] { curve8.Domain.Min }, new double[1] { curve8.Domain.Min }, out var breps2, closed: false, maintainHeight: false))
		{
			return false;
		}
		Brep brep = CSUtilRhino.JoinAllBreps_BackwardCompatibility(new List<Brep> { breps, breps2 }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (brep == null)
		{
			return false;
		}
		Transform xform = Transform.Translation(0.0, prongY / 2.0, 0.0);
		brep.Transform(xform);
		dome = brep;
		return true;
	}

	private static bool CreateProngExtension(double prongExtensionHeight, ref Curve prongRail, ref Curve prongTopProfile)
	{
		Vector3d vector3d = prongRail.TangentAt(prongRail.Domain.Min);
		vector3d.Unitize();
		Point3d pointAtStart = prongRail.PointAtStart;
		Point3d point3d = pointAtStart - vector3d * prongExtensionHeight;
		Transform xform = Transform.Translation(point3d - pointAtStart);
		prongTopProfile.Transform(xform);
		LineCurve lineCurve = new LineCurve(pointAtStart, point3d);
		Curve curve = Curve.JoinCurves(new Curve[2] { prongRail, lineCurve })[0];
		if ((curve.PointAtStart - point3d).Length > 0.01)
		{
			curve.Reverse();
		}
		prongRail = curve;
		return true;
	}

	private static GeometryBase CutGeometry(GeometryBase g, Brep cut, Transform T, Transform xUp)
	{
		if (!(g is Brep brep) || cut == null)
		{
			GeometryBase geometryBase = g.Duplicate();
			geometryBase.Transform(T);
			return geometryBase;
		}
		Brep brep2 = brep.DuplicateBrep();
		brep2.Transform(T);
		Brep brep3 = cut.DuplicateBrep();
		brep3.Transform(xUp);
		Brep[] array = Brep.CreateBooleanDifference(brep2, brep3, 0.001);
		if (array == null)
		{
			return brep2.DuplicateBrep();
		}
		return array[0];
	}

	private static NurbsSurface LoftDir(Curve c0, Curve c1, Vector3d v0, Vector3d v1)
	{
		Brep brep = Brep.CreateFromLoft(new List<Curve> { c0, c1 }, Point3d.Unset, Point3d.Unset, LoftType.Tight, closed: false)[0];
		if (brep.Faces.Count != 1)
		{
			return null;
		}
		NurbsSurface nurbsSurface = brep.Faces[0].ToNurbsSurface();
		if (nurbsSurface.Points.CountU != 4)
		{
			return null;
		}
		int countV = nurbsSurface.Points.CountV;
		for (int i = 0; i < countV; i++)
		{
			Point3d location = nurbsSurface.Points.GetControlPoint(0, i).Location;
			Point3d location2 = nurbsSurface.Points.GetControlPoint(3, i).Location;
			double num = location.DistanceTo(location2);
			location += v0 * num / 3.0;
			location2 += v1 * num / 3.0;
			nurbsSurface.Points.SetControlPoint(1, i, location);
			nurbsSurface.Points.SetControlPoint(2, i, location2);
		}
		return nurbsSurface;
	}

	private static bool MakeProfileSweepProngs(int prongIndex, List<Curve> prongRailArray, Curve prongRail, CSGemData gem, Curve headTop, Curve inProngProfileTop, double paramTop, double prongTopX, double prongTopY, double prongTopOffset, Curve headBase, Curve inProngProfileBase, double paramBase, double prongBaseX, double prongBaseY, double prongBaseOffset, ValueListItem baseStyle, double basePointSize, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, double prongBaseDomeHeight, Transform xUp, bool enableDoItYourself, CSHeadStyles.HeadProngInfo headProngInfo, out List<CSOnGeometryData> outCurves, out List<CSOnGeometryData> outProngs, out string error)
	{
		outCurves = new List<CSOnGeometryData>();
		outProngs = new List<CSOnGeometryData>();
		Point3d pointAtStart = prongRail.PointAtStart;
		Point3d pointAtEnd = prongRail.PointAtEnd;
		Vector3d vector3d = CSBlockUtil.CSUtilMath.AverageTangentAt(headTop, paramTop);
		vector3d.Unitize();
		Vector3d headTopNrm = CSBlockUtil.CSUtilMath.AverageNormalAt(Vector3d.ZAxis, vector3d);
		headTopNrm.Unitize();
		BoundingBox boundingBox = inProngProfileTop.GetBoundingBox(accurate: true);
		double x = boundingBox.Diagonal.X;
		double y = boundingBox.Diagonal.Y;
		double num = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain.Min), vector3d);
		double num2 = 1.0 / Math.Cos(num);
		Transform transform = Transform.Scale(Plane.WorldXY, prongTopX / x, prongTopY / y * num2, 0.0);
		Transform transform2 = Transform.Translation(0.0, prongTopOffset, 0.0);
		Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtStart, vector3d, Vector3d.ZAxis);
		Curve prongTopProfile = inProngProfileTop.DuplicateCurve();
		prongTopProfile.Transform(transform3 * transform2 * transform);
		Vector3d vector3d2;
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			vector3d2 = vector3d;
		}
		else
		{
			vector3d2 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, paramBase);
			vector3d2.Unitize();
		}
		BoundingBox boundingBox2 = inProngProfileBase.GetBoundingBox(accurate: true);
		double x2 = boundingBox2.Diagonal.X;
		double y2 = boundingBox2.Diagonal.Y;
		double d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain.Max), vector3d2);
		double num3 = 1.0 / Math.Cos(d);
		Transform transform4 = Transform.Scale(Plane.WorldXY, prongBaseX / x2, prongBaseY / y2 * num3, 0.0);
		if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
		{
			prongBaseOffset = 0.0 - prongBaseY * num3 + basePointSize;
		}
		Transform transform5 = Transform.Translation(0.0, prongBaseOffset, 0.0);
		Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? vector3d : vector3d2, Vector3d.ZAxis);
		Curve prongBaseProfile = inProngProfileBase.DuplicateCurve();
		prongBaseProfile.Transform(transform6 * transform5 * transform4);
		Brep dome = null;
		Brep dome2 = null;
		if (CSApplicationHelper.Application.IsCamMode && !enableDoItYourself)
		{
			if (gem.GemZMax > prongRail.PointAtStart.Z && !CreateProngExtension(gem.GemZMax - prongRail.PointAtStart.Z, ref prongRail, ref prongTopProfile))
			{
				error = "Unable to create CAM prong extension.";
				return false;
			}
			if (!CreateProngDome(inProngProfileTop, prongTopX, prongTopY, num, prongDomeHeight, prongDomeSettingDistance, prongDomeSettingAngle, num2, ref dome))
			{
				error = "Unable to create top dome";
				return false;
			}
			pointAtStart = prongRail.PointAtStart;
			transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtStart, vector3d, Vector3d.ZAxis);
			dome.Transform(transform3 * transform2);
		}
		else if (prongDomeHeight <= 0.0)
		{
			if (!CreateProngExtension(0.0, ref prongRail, ref prongTopProfile))
			{
				error = "Unable to create top prong cap.";
				return false;
			}
		}
		else
		{
			if (!CreateProngDome(inProngProfileTop, prongTopX, prongTopY, num, prongDomeHeight, prongDomeSettingDistance, prongDomeSettingAngle, num2, ref dome))
			{
				error = "Unable to create top dome";
				return false;
			}
			dome.Transform(transform3 * transform2);
		}
		if (prongBaseDomeHeight > 0.0)
		{
			if (!ReduceProngRailLength(ref prongRail, ref prongBaseProfile, prongBaseDomeHeight))
			{
				error = "ReduceProngRailLength failed";
				return false;
			}
			headBase.ClosestPoint(prongRail.PointAtEnd, out paramBase);
			vector3d2 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, paramBase);
			vector3d2.Unitize();
			d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain.Max), vector3d2);
			if (!CreateProngDome(inProngProfileBase, prongBaseX, prongBaseY, d, 0.0 - prongBaseDomeHeight, 0.0, 0.0, num3, ref dome2))
			{
				error = "Unable to create base dome";
				return false;
			}
			pointAtEnd = prongRail.PointAtEnd;
			Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? vector3d : vector3d2, Vector3d.ZAxis);
			prongBaseProfile = inProngProfileBase.DuplicateCurve();
			prongBaseProfile.Transform(transform7 * transform5 * transform4);
			dome2.Transform(transform7 * transform5);
		}
		GeometryBase[] arrGeometry = new GeometryBase[3] { prongRail, prongTopProfile, prongBaseProfile };
		if (!ArrangeGeometry(headProngInfo, xUp, vector3d, headTopNrm, prongTopX, arrGeometry, null, ref outCurves))
		{
			error = "Invalid head style";
			return false;
		}
		if (enableDoItYourself)
		{
			if (!ArrangeGeometry(arrGeometry: (dome2 == null) ? new GeometryBase[1] { dome } : new GeometryBase[2] { dome, dome2 }, headProngInfo: headProngInfo, xUp: xUp, headTopTan: vector3d, headTopNrm: headTopNrm, prongTopX: prongTopX, cutter: null, outputGeometries: ref outProngs))
			{
				error = "Invalid head style";
				return false;
			}
		}
		else
		{
			Curve[] profiles = new Curve[2] { prongTopProfile, prongBaseProfile };
			double[] parameters = new double[2]
			{
				prongRail.Domain.Min,
				prongRail.Domain.Max
			};
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
			List<Brep> list = new List<Brep>();
			if (dome != null)
			{
				list.Add(dome);
			}
			if (dome2 != null)
			{
				list.Add(dome2);
			}
			list.AddRange(breps);
			Brep brep = CSUtilRhino.JoinAllBreps_BackwardCompatibility(list.ToArray(), 0.01);
			if (brep == null)
			{
				error = "Failed JoinBreps";
				return false;
			}
			Brep brep2;
			if (dome2 == null)
			{
				brep2 = CSUtilRhino.CapPlanarHoles(brep, 0.01);
				if (brep2 == null)
				{
					error = "Failed CapPlanarHoles";
					return false;
				}
			}
			else
			{
				brep2 = brep;
			}
			Brep brep3 = null;
			if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
			{
				List<Point3d> points;
				if (prongRailArray.Count > 2)
				{
					Curve curve = prongRailArray[(prongIndex + 1) % prongRailArray.Count];
					Curve curve2 = prongRailArray[(prongIndex - 1 + prongRailArray.Count) % prongRailArray.Count];
					Vector3d vector3d3 = new Vector3d(curve.PointAtStart);
					Vector3d vector3d4 = new Vector3d(prongRail.PointAtStart);
					Vector3d vector3d5 = new Vector3d(curve2.PointAtStart);
					vector3d3.Z = 0.0;
					vector3d4.Z = 0.0;
					vector3d5.Z = 0.0;
					vector3d3.Unitize();
					vector3d4.Unitize();
					vector3d5.Unitize();
					vector3d3 *= 20.0;
					vector3d4 *= 20.0;
					vector3d5 *= 20.0;
					Point3d item = new Point3d(vector3d3 + vector3d4);
					Point3d item2 = new Point3d(0.0, 0.0, 0.0);
					Point3d item3 = new Point3d(vector3d5 + vector3d4);
					Point3d item4 = new Point3d(vector3d4 * -2.0);
					item.Z = 20.0;
					item2.Z = 20.0;
					item3.Z = 20.0;
					item4.Z = 20.0;
					points = new List<Point3d> { item, item2, item3, item4, item };
				}
				else
				{
					Vector3d vector3d6 = new Vector3d(prongRail.PointAtStart);
					vector3d6.Z = 0.0;
					Vector3d vector3d7 = vector3d6;
					vector3d7.Unitize();
					vector3d7 *= 20.0;
					Vector3d vector3d8 = new Vector3d(vector3d7);
					vector3d8.Transform(Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Point3d.Origin));
					Point3d item5 = new Point3d(vector3d8);
					Point3d item6 = new Point3d(0.0, 0.0, 0.0);
					Point3d item7 = new Point3d(-vector3d8);
					Point3d item8 = new Point3d(vector3d7 * -2.0);
					item5.Z = 20.0;
					item6.Z = 20.0;
					item7.Z = 20.0;
					item8.Z = 20.0;
					points = new List<Point3d> { item5, item6, item7, item8, item5 };
				}
				PolylineCurve curveA = new PolylineCurve(points);
				PolylineCurve polylineCurve = new PolylineCurve(points);
				Transform xform = Transform.Translation(0.0, 0.0, -40.0);
				polylineCurve.Transform(xform);
				brep3 = CSUtilRhino.CapPlanarHoles(NurbsSurface.CreateRuledSurface(curveA, polylineCurve).ToBrep(), 0.001);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep2, doFullClean: true);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep3, doFullClean: true);
			}
			if (!ArrangeGeometry(headProngInfo, xUp, vector3d, headTopNrm, prongTopX, new GeometryBase[1] { brep2 }, brep3, ref outProngs))
			{
				error = "Invalid head style";
				return false;
			}
		}
		error = "";
		return true;
	}

	private static Curve MakeProngCurve(Point3d domeStart0, Point3d domeStart1, Vector3d prongDir, double prongDomeHeight, double prongDomeSettingDistance)
	{
		Point3d pointEnd = (domeStart0 + domeStart1) / 2.0;
		pointEnd += prongDir * prongDomeHeight;
		Vector3d vector3d = domeStart0 - domeStart1;
		vector3d.Unitize();
		pointEnd -= vector3d * prongDomeSettingDistance;
		Vector3d vectorEnd = new Vector3d(vector3d);
		vectorEnd.Reverse();
		Curve curve = CSUtilCurves.BlendCurve(domeStart0, pointEnd, prongDir, vector3d);
		Curve curve2 = CSUtilCurves.BlendCurve(domeStart1, pointEnd, prongDir, vectorEnd);
		return Curve.JoinCurves(new Curve[2] { curve, curve2 })[0].Rebuild(10, 3, preserveTangents: true);
	}

	private static bool MakeVProngDomes(IReadOnlyList<Curve> prongCurves, Vector3d prongDir, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, Curve trimPart, out Brep vDome)
	{
		Curve curve = prongCurves[0];
		Curve curve2 = prongCurves[1];
		Curve curve3 = prongCurves[2];
		Curve curve4 = prongCurves[3];
		vDome = null;
		double t = trimPart.Domain.Min;
		double max = trimPart.Domain.Max;
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		List<Curve> list3 = new List<Curve>();
		Curve curve5 = MakeProngCurve(curve2.PointAtStart, curve2.PointAtEnd, prongDir, prongDomeHeight, prongDomeSettingDistance);
		list3.Add(curve5);
		list.Add(curve.Domain.Min);
		list2.Add(curve3.Domain.Min);
		double t2;
		while (trimPart.GetNextDiscontinuity(Continuity.C1_continuous, t, max, out t2))
		{
			Point3d point3d = trimPart.PointAt(t2);
			t = t2;
			double t3 = t2 + 0.001;
			double t4 = t2 - 0.001;
			Vector3d vector3d = trimPart.TangentAt(t3);
			Vector3d vector3d2 = trimPart.TangentAt(t4);
			Vector3d a = vector3d + vector3d2;
			a.Unitize();
			Vector3d vector3d3 = Vector3d.CrossProduct(a, Vector3d.ZAxis);
			LineCurve curveB = new LineCurve(point3d + 50.0 * vector3d3, point3d - 50.0 * vector3d3);
			CurveIntersections curveIntersections = Intersection.CurveCurve(curve, curveB, 0.01, 0.01);
			CurveIntersections curveIntersections2 = Intersection.CurveCurve(curve3, curveB, 0.01, 0.01);
			if (curveIntersections.Count != 0 && curveIntersections2.Count != 0)
			{
				Curve item = MakeProngCurve(curveIntersections[0].PointA, curveIntersections2[0].PointA, prongDir, prongDomeHeight, prongDomeSettingDistance);
				list3.Add(item);
				list.Add(curveIntersections[0].ParameterA);
				list2.Add(curveIntersections2[0].ParameterA);
			}
		}
		Curve curve6 = MakeProngCurve(curve4.PointAtStart, curve4.PointAtEnd, prongDir, prongDomeHeight, prongDomeSettingDistance);
		list3.Add(curve6);
		list.Add(curve.Domain.Max);
		list2.Add(curve3.Domain.Max);
		Vector3d b = curve5.PointAtEnd - curve5.PointAtStart;
		Vector3d vector3d4 = Vector3d.CrossProduct(curve5.PointAtNormalizedLength(0.5) - curve5.PointAtStart, b);
		if (prongDir.Z < 0.0)
		{
			vector3d4 = -vector3d4;
		}
		Brep brep = LoftDir(curve2, curve5, prongDir, vector3d4).ToBrep();
		b = curve6.PointAtEnd - curve6.PointAtStart;
		vector3d4 = Vector3d.CrossProduct(curve6.PointAtNormalizedLength(0.5) - curve6.PointAtStart, b);
		if (prongDir.Z < 0.0)
		{
			vector3d4 = -vector3d4;
		}
		Brep brep2 = LoftDir(curve4, curve6, prongDir, -vector3d4).ToBrep();
		if (list3.Count > 2)
		{
			for (int i = 1; i < list3.Count - 2; i++)
			{
				if (Math.Abs(list[i] - list[i + 1]) < 0.001 || Math.Abs(list2[i] - list2[i + 1]) < 0.001)
				{
					list3.RemoveAt(i);
					list.RemoveAt(i);
					list2.RemoveAt(i);
					i--;
				}
			}
		}
		if (!CSUtilSweepRC.Sweep2(curve, curve3, list3.ToArray(), list.ToArray(), list2.ToArray(), out var breps, closed: false, maintainHeight: true))
		{
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		Brep[] inputBreps = new Brep[3] { brep, breps, brep2 };
		vDome = CSUtilRhino.JoinAllBreps_BackwardCompatibility(inputBreps, 0.01);
		if (vDome == null)
		{
			return false;
		}
		return true;
	}

	private static bool MakeVProngProfile(Curve headProfile, double param, double vProngAngle, double prongY, double prongNudge, double blendAmount, out Curve vProng, out Curve[] prongCurves, out Curve trimPart)
	{
		vProng = null;
		prongCurves = null;
		trimPart = null;
		if (headProfile == null)
		{
			return false;
		}
		Point3d point3d = headProfile.PointAt(param);
		LineCurve lineCurve = new LineCurve(new Point3d(0.0, 0.0, point3d.Z), new Point3d(point3d.X * 2.0, point3d.Y * 2.0, point3d.Z));
		LineCurve lineCurve2 = new LineCurve(new Point3d(0.0, 0.0, point3d.Z), new Point3d(point3d.X * 2.0, point3d.Y * 2.0, point3d.Z));
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(vProngAngle), Vector3d.ZAxis, Point3d.Origin);
		lineCurve.Transform(xform);
		xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - vProngAngle), Vector3d.ZAxis, Point3d.Origin);
		lineCurve2.Transform(xform);
		CurveIntersections curveIntersections = Intersection.CurveCurve(headProfile, lineCurve, 0.01, 0.01);
		if (curveIntersections.Count != 1)
		{
			return false;
		}
		double parameterA = curveIntersections[0].ParameterA;
		curveIntersections = Intersection.CurveCurve(headProfile, lineCurve2, 0.01, 0.01);
		if (curveIntersections.Count != 1)
		{
			return false;
		}
		double parameterA2 = curveIntersections[0].ParameterA;
		if (parameterA < parameterA2)
		{
			trimPart = headProfile.Trim(new Interval(parameterA, parameterA2));
		}
		else
		{
			Curve curve = headProfile.Trim(new Interval(parameterA, headProfile.Domain.Max));
			Curve curve2 = headProfile.Trim(new Interval(headProfile.Domain.Min, parameterA2));
			Curve[] array = Curve.JoinCurves(new Curve[2] { curve, curve2 }, 0.01);
			trimPart = array[0];
		}
		Curve crv = CSUtilGeometry.Offset(trimPart, Vector3d.ZAxis, new Point3d(0.0, 0.0, point3d.Z), 0.0 - prongNudge, CurveOffsetCornerStyle.Round);
		Curve crv2 = CSUtilGeometry.Offset(trimPart, Vector3d.ZAxis, new Point3d(0.0, 0.0, point3d.Z), 0.0 - (prongY + prongNudge), CurveOffsetCornerStyle.Round);
		if (crv == null || crv2 == null)
		{
			return false;
		}
		crv2.NormalizedLengthParameter(0.5, out var t);
		Point3d testPoint = crv2.PointAt(t);
		Point3d pointAtStart = crv.PointAtStart;
		Point3d pointAtStart2 = crv2.PointAtStart;
		Curve crv3 = new LineCurve(pointAtStart, pointAtStart2);
		Point3d pointAtEnd = crv.PointAtEnd;
		Point3d pointAtEnd2 = crv2.PointAtEnd;
		Curve crv4 = new LineCurve(pointAtEnd, pointAtEnd2);
		double num = blendAmount;
		double num2 = blendAmount;
		double length = crv3.GetLength();
		double length2 = crv.GetLength();
		double length3 = crv2.GetLength();
		if (blendAmount > length * 0.48)
		{
			blendAmount = length * 0.48;
		}
		if (num > length2 * 0.48)
		{
			num = length2 * 0.48;
		}
		if (num2 > length3 * 0.48)
		{
			num2 = length3 * 0.48;
		}
		if (blendAmount > 0.0)
		{
			CSUtilGeometry.TrimStartOfCrv(ref crv, num);
			CSUtilGeometry.TrimStartOfCrv(ref crv2, num2);
			CSUtilGeometry.TrimEndOfCrv(ref crv3, blendAmount);
			CSUtilGeometry.TrimStartOfCrv(ref crv3, blendAmount);
			Curve curve3 = CSUtilCurves.BlendCurve(crv, crv3, doBlendFromStart1: true);
			Curve curve4 = CSUtilCurves.BlendCurve(crv3, crv2);
			CSUtilGeometry.TrimEndOfCrv(ref crv, num);
			CSUtilGeometry.TrimEndOfCrv(ref crv2, num2);
			CSUtilGeometry.TrimEndOfCrv(ref crv4, blendAmount);
			CSUtilGeometry.TrimStartOfCrv(ref crv4, blendAmount);
			Curve curve5 = CSUtilCurves.BlendCurve(crv, crv4);
			Curve curve6 = CSUtilCurves.BlendCurve(crv4, crv2, doBlendFromStart1: false, doBlendFromStart2: false);
			Curve[] array2 = Curve.JoinCurves(new Curve[3] { curve3, crv3, curve4 }, 0.01);
			if (array2.Length != 1)
			{
				return false;
			}
			crv3 = array2[0];
			array2 = Curve.JoinCurves(new Curve[3] { curve5, crv4, curve6 }, 0.01);
			if (array2.Length != 1)
			{
				return false;
			}
			crv4 = array2[0];
		}
		prongCurves = new Curve[4] { crv, crv3, crv2, crv4 };
		Curve[] array3 = Curve.JoinCurves(prongCurves, 0.01);
		if (array3.Length != 1)
		{
			return false;
		}
		Curve curve7 = array3[0];
		curve7.ClosestPoint(testPoint, out var t2);
		curve7.ChangeClosedCurveSeam(t2);
		double min = curve7.Domain.Min;
		Vector3d a = curve7.TangentAt(min);
		Vector3d b = new Vector3d(testPoint.X, testPoint.Y, 0.0);
		if (Vector3d.CrossProduct(a, b).Z < 0.0)
		{
			curve7.Reverse();
		}
		vProng = curve7;
		return true;
	}

	private static bool MakeVProngs(Curve prongRail, Curve headTop, double paramTop, double prongVTopAngle, double prongTopY, double prongTopOffset, double prongVBlend, Curve headBase, double paramBase, double prongVBaseAngle, double prongBaseY, double prongBaseOffset, ValueListItem baseStyle, double prongBaseDomeHeight, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, Transform xUp, bool enableDoItYourself, out List<CSOnGeometryData> outCurves, out CSOnGeometryData outProng, out string error)
	{
		outCurves = new List<CSOnGeometryData>();
		outProng = null;
		Vector3d prongDir = -prongRail.TangentAt(prongRail.Domain.Min);
		prongDir.Unitize();
		Curve geometry = prongRail;
		if (!MakeVProngProfile(headTop, paramTop, prongVTopAngle, prongTopY, prongTopOffset, prongVBlend, out var vProng, out var prongCurves, out var trimPart))
		{
			error = "Error Making Straight Prong Profile Top";
			return false;
		}
		if (!MakeVProngDomes(prongCurves, prongDir, prongDomeHeight, prongDomeSettingDistance, prongDomeSettingAngle, trimPart, out var vDome))
		{
			error = "Error Making V Prong Domes";
			return false;
		}
		if (!MakeVProngProfile(headBase, paramBase, prongVBaseAngle, prongBaseY, prongBaseOffset, prongVBlend, out var vProng2, out var prongCurves2, out trimPart))
		{
			error = "Error Making Straight Prong Profile Base";
			return false;
		}
		Brep vDome2 = null;
		if (!CSApplicationHelper.Application.IsCamMode && baseStyle == ValueListItem.HeadBuilderProngHead && prongBaseDomeHeight > 0.0)
		{
			Point3d pointAtEnd = prongRail.PointAtEnd;
			if (!ReduceProngRailLength(ref prongRail, ref vProng2, prongBaseDomeHeight))
			{
				error = "ReduceProngRailLength failed";
				return false;
			}
			Point3d pointAtEnd2 = prongRail.PointAtEnd;
			Vector3d prongDir2 = prongRail.TangentAt(prongRail.Domain.Max);
			if (!MakeVProngDomes(prongCurves2, prongDir2, prongBaseDomeHeight, 0.0, 0.0, trimPart, out vDome2))
			{
				error = "Error Making V Prong Domes";
				return false;
			}
			Transform xform = Transform.Translation(pointAtEnd2 - pointAtEnd);
			vDome2.Transform(xform);
		}
		outCurves.Add(new CSOnGeometryData(prongRail, xUp));
		outCurves.Add(new CSOnGeometryData(geometry, xUp));
		outCurves.Add(new CSOnGeometryData(vProng, xUp));
		outCurves.Add(new CSOnGeometryData(vProng2, xUp));
		if (enableDoItYourself)
		{
			error = "";
			return true;
		}
		vProng.ClosestPoint(prongRail.PointAtStart, out var t);
		vProng.ChangeClosedCurveSeam(t);
		vProng2.ClosestPoint(prongRail.PointAtEnd, out t);
		vProng2.ChangeClosedCurveSeam(t);
		Curve[] profiles = new Curve[2] { vProng, vProng2 };
		double[] parameters = new double[2]
		{
			prongRail.Domain.Min,
			prongRail.Domain.Max
		};
		if (!CSUtilSweepRC.Sweep1(prongRail, profiles, parameters, out var breps, closed: false, globalShapeBlend: true))
		{
			error = "Failed Sweep1";
			return false;
		}
		if (breps == null || breps.Length == 0)
		{
			error = "Failed Sweep1: No results";
			return false;
		}
		Brep brep = breps[0];
		CSUtilGeometry.FixBrepSolidOrientation(ref brep);
		Brep brep2 = CSUtilRhino.JoinAllBreps_BackwardCompatibility((vDome2 != null) ? new Brep[3] { brep, vDome, vDome2 } : new Brep[2] { brep, vDome }, 0.01);
		Brep brep3;
		if (vDome2 != null)
		{
			brep3 = brep2;
		}
		else
		{
			brep3 = CSUtilRhino.CapPlanarHoles(brep2, 0.01);
			if (brep3 == null)
			{
				error = "Failed CapPlanarHoles";
				return false;
			}
		}
		CSUtilGeometry.FixBrepSolidOrientation(ref brep3);
		brep3.Transform(xUp);
		outProng = new CSOnGeometryData(brep3, xUp, applyTransform: false);
		error = "";
		return true;
	}

	private static bool ReduceProngRailLength(ref Curve prongRail, ref Curve prongBaseProfile, double trimLength)
	{
		Curve[] array = prongRail.Split(prongRail.Domain.Max - trimLength);
		prongRail = array[0];
		Curve curve = array[1];
		Vector3d translationVector = new Vector3d(curve.PointAtStart - curve.PointAtEnd);
		prongBaseProfile.Translate(translationVector);
		return true;
	}
}
