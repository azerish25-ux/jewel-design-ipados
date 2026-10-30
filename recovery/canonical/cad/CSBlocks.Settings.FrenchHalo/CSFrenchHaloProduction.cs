using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.FrenchHalo;

[CSBlock("FrenchHaloProduction", "{30B17848-D8C2-443A-B60B-43A38DDDD6ED}")]
public sealed class CSFrenchHaloProduction : CSBlock
{
	private const double _angleAdjustFactor = 1.0 / 300.0;

	public readonly CSInputChannel InCenterGem;

	public readonly CSInputChannel InSortedGems;

	public readonly CSInputChannel InShankTopProfile;

	public readonly CSInputChannel InChannelCutter;

	public readonly CSInputChannel InGraverCutter;

	public readonly CSInputChannel InTemplateGem;

	public readonly CSInputChannel InScallopCutterProfile;

	public readonly CSInputChannel InGemCutter;

	public readonly CSInputChannel InPilotHoleCutter;

	public readonly CSInputChannel InBooleanToggle;

	public readonly CSInputChannel InProductionShow;

	public readonly CSInputChannel InUnderLyingSurfacesToggle;

	public readonly CSInputChannel InUseDivotCutter;

	public readonly CSInputChannel InDivotRadius;

	public readonly CSInputChannel InDivotCutterTranslate;

	public readonly CSInputChannel InWallThickness;

	public readonly CSInputChannel InProfileHeight;

	public readonly CSInputChannel InProfileWidth;

	public readonly CSInputChannel InCenterGemSpacing;

	public readonly CSInputChannel InPlacement;

	public readonly CSInputChannel InClusterAngle;

	public readonly CSInputChannel InCenterSpacingFix;

	public readonly CSInputChannel InUsingSweep;

	public readonly CSOutputChannel OutFrenchHalo;

	public readonly CSOutputChannel OutChannelProfiles;

	public readonly CSOutputChannel OutShankProfiles;

	public readonly CSOutputChannel OutBaseSurface;

	public readonly CSOutputChannel OutChannelLoft;

	public readonly CSOutputChannel OutScallopCutters;

	public readonly CSOutputChannel OutGraverCutters;

	public readonly CSOutputChannel OutFrenchShank;

	public readonly CSOutputChannel OutGemCutters;

	public readonly CSOutputChannel OutPilotHoleCutters;

	public CSFrenchHaloProduction(string identifier)
		: base(identifier)
	{
		InCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSortedGems = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InShankTopProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InChannelCutter = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InScallopCutterProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGraverCutter = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InTemplateGem = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InGemCutter = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InPilotHoleCutter = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InBooleanToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InProductionShow = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InUnderLyingSurfacesToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InUseDivotCutter = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDivotRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InDivotCutterTranslate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 2);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3);
		InWallThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InClusterAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterSpacingFix = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InUsingSweep = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutFrenchHalo = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutChannelProfiles = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutShankProfiles = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutBaseSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutChannelLoft = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutScallopCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutGraverCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutFrenchShank = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutGemCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutPilotHoleCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSGemData gem = InCenterGem.Gem;
		bool boolean = InBooleanToggle.Boolean;
		bool boolean2 = InProductionShow.Boolean;
		bool boolean3 = InUnderLyingSurfacesToggle.Boolean;
		if (!boolean3 && !boolean2)
		{
			return base.Update();
		}
		List<CSGemData> gemList = InSortedGems.GemList;
		if (gemList == null)
		{
			return UpdateMissing("Gems");
		}
		List<CSChannelGemData> list = new List<CSChannelGemData>();
		foreach (CSGemData item in gemList)
		{
			if (item != null)
			{
				list.Add(new CSChannelGemData(item, null));
			}
		}
		CSUtilGemRails.CreateRails(list);
		CSChannelGemData[] gemDatas = list.ToArray();
		Brep brep = InTemplateGem.Brep;
		Curve curve = InChannelCutter.Curve;
		Curve curve2 = InScallopCutterProfile.Curve;
		Brep brep2 = InGraverCutter.Brep;
		Brep brep3 = InGemCutter.Brep;
		Brep brep4 = InPilotHoleCutter.Brep;
		bool boolean4 = InUseDivotCutter.Boolean;
		double divotRadius = InDivotRadius.Double;
		double divotCutterTranslate = InDivotCutterTranslate.Double;
		double num = InWallThickness.Double;
		double profileHeight = InProfileHeight.Double;
		double scallopLength = InProfileWidth.Double;
		double centerGemSpacing = InCenterGemSpacing.Double + InCenterSpacingFix.Double;
		double num2 = InPlacement.Double;
		double num3 = RhinoMath.ToRadians(InClusterAngle.Double);
		bool boolean5 = InUsingSweep.Boolean;
		InShankTopProfile.GetValues(out List<Curve> value);
		Curve curve3 = null;
		Curve curve4 = null;
		List<Curve> list2 = new List<Curve>();
		foreach (Curve item2 in value)
		{
			string text = item2.GetUserString("CSName").ToLower();
			if (text == "top")
			{
				curve3 = item2.DuplicateCurve();
			}
			if (text == "side")
			{
				list2.Add(item2.DuplicateCurve());
			}
			if (text == "bottom")
			{
				curve4 = item2.DuplicateCurve();
			}
		}
		if (curve3 == null || list2.Count != 2 || curve4 == null)
		{
			return UpdateError("Error shank profile Curves");
		}
		gem.GetGemXformRC(out var xDown, out var xUp);
		CSFrenchHaloHelper.SetUpFrenchGems(gemDatas);
		gemDatas = CSFrenchHaloHelper.SorttoFirstCorner(gemDatas);
		CSFrenchHaloHelper.CalculateRail(gemDatas, 0, 1.0, 1.0, num, null, out var rail);
		CSFrenchHaloHelper.CalculateRail(gemDatas, -1, 1.0, 1.0, num, null, out var rail2);
		CSFrenchHaloHelper.CalculateRail(gemDatas, 1, 1.0, 1.0, num, null, out var rail3);
		if (!MakeHaloProfile(num, profileHeight, gemDatas[0], brep, curve3, curve4, list2, curve, num3, boolean5, out var adjustPlacement, out var _, out var _, out var _, out var _, out var outShankProfile))
		{
			return UpdateError("halo profile failed");
		}
		num2 -= adjustPlacement / 2.0;
		rail2.Transform(xDown);
		rail3.Transform(xDown);
		rail2.Transform(Transform.Translation(0.0, 0.0, num2 - rail2.PointAtStart.Z));
		rail3.Transform(Transform.Translation(0.0, 0.0, num2 - rail3.PointAtStart.Z));
		rail2.Transform(xUp);
		rail3.Transform(xUp);
		if (!MakeBaseSurface(outShankProfile, gem, rail2, rail, rail3, centerGemSpacing, num2, boolean5, out var baseBrep))
		{
			return UpdateError("make Base Profile failed");
		}
		if (!boolean5)
		{
			baseBrep.Transform(xUp);
		}
		List<Brep> list3 = new List<Brep>();
		List<Brep> scallopList = new List<Brep>();
		List<Brep> list4 = new List<Brep>();
		List<Brep> list5 = new List<Brep>();
		for (int i = 0; i < gemDatas.Length; i++)
		{
			CSChannelGemData cSChannelGemData = gemDatas[i];
			CSChannelGemData cSChannelGemData2 = ((i != gemDatas.Length - 1) ? gemDatas[i + 1] : gemDatas[0]);
			CSChannelGemData cSChannelGemData3 = ((i != 0) ? gemDatas[i - 1] : gemDatas[gemDatas.Length - 1]);
			string value2 = "";
			cSChannelGemData.GetUserString("IsBead", ref value2);
			if (value2 != "1")
			{
				double adjustCornerCutters = ((cSChannelGemData.Corner != ChannelCornerType.corner90) ? 0.0 : num3);
				MakeGemCuttersAndHoles(cSChannelGemData, brep, brep3, brep4, boolean4, divotRadius, divotCutterTranslate, adjustCornerCutters, out var outGemCutter, out var outPilotHoleCutter);
				list4.Add(outGemCutter);
				list5.Add(outPilotHoleCutter);
			}
			Brep endGraverBrep;
			if (cSChannelGemData.Corner != ChannelCornerType.corner90)
			{
				Point3d center = cSChannelGemData3.Center;
				Point3d center2 = cSChannelGemData2.Center;
				Point3d center3 = cSChannelGemData.Center;
				Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(cSChannelGemData.Center, cSChannelGemData.Up, cSChannelGemData.North, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
				center.Transform(xform);
				center2.Transform(xform);
				center3.Transform(xform);
				Vector3d b = center2 - center3;
				Vector3d a = center - center3;
				a.Z = 0.0;
				b.Z = 0.0;
				double vectorAngle = Vector3d.VectorAngle(a, b);
				MakeScallopCutter(gem, cSChannelGemData, brep, curve2, scallopLength, vectorAngle, out var scallopCutters);
				scallopList.Add(scallopCutters);
				if (cSChannelGemData2.Corner == ChannelCornerType.corner90)
				{
					MakeGraversCorner(cSChannelGemData, cSChannelGemData2, rail, brep, brep2, out endGraverBrep);
				}
				else
				{
					MakeGravers(cSChannelGemData, cSChannelGemData2, rail, brep, brep2, out endGraverBrep);
				}
			}
			else
			{
				Vector3d previousVector = cSChannelGemData.Center - cSChannelGemData3.Center;
				Vector3d nextVector = cSChannelGemData.Center - cSChannelGemData2.Center;
				MakeScallopCutterCorner(cSChannelGemData, brep, curve2, scallopLength, previousVector, nextVector, num3, out var scallopCutter);
				scallopCutter.ForEach(delegate(Brep _)
				{
					scallopList.Add(_);
				});
				MakeGraversCorner(cSChannelGemData, cSChannelGemData2, rail, brep, brep2, out endGraverBrep);
			}
			list3.Add(endGraverBrep);
		}
		List<Brep> list6 = new List<Brep> { baseBrep };
		CSDataArray gemCutterDataArray = new CSDataArray();
		list4.ForEach(delegate(Brep _)
		{
			gemCutterDataArray.Append(new CSOnGeometryData(_));
		});
		CSDataArray pilotHoleDataArray = new CSDataArray();
		list5.ForEach(delegate(Brep _)
		{
			pilotHoleDataArray.Append(new CSOnGeometryData(_));
		});
		CSDataArray scallopDataArray = new CSDataArray();
		scallopList.ForEach(delegate(Brep _)
		{
			scallopDataArray.Append(new CSOnGeometryData(_));
		});
		CSDataArray graverDataArray = new CSDataArray();
		list3.ForEach(delegate(Brep _)
		{
			graverDataArray.Append(new CSOnGeometryData(_));
		});
		if (boolean && boolean2)
		{
			Brep[] outBrep = list6.ToArray();
			Brep[] array = Brep.CreateBooleanDifference(list6, list3, 0.01);
			if (array == null)
			{
				list3.ForEach(delegate(Brep _)
				{
					outBrep = Brep.CreateBooleanDifference(outBrep[0], _, 0.01);
				});
				if (outBrep == null || outBrep.Length < 1)
				{
					return UpdateError("Graver Boolean Error");
				}
			}
			else
			{
				outBrep = array;
			}
			array = Brep.CreateBooleanDifference(outBrep, scallopList, 0.01);
			if (array == null)
			{
				scallopList.ForEach(delegate(Brep _)
				{
					outBrep = Brep.CreateBooleanDifference(outBrep[0], _, 0.01);
				});
				if (outBrep == null || outBrep.Length < 1)
				{
					return UpdateError("Scallop Boolean Error");
				}
			}
			else
			{
				outBrep = array;
			}
			array = Brep.CreateBooleanDifference(outBrep, list4, 0.01);
			if (outBrep.Length < 1)
			{
				UpdateError(" GemCutter Error");
			}
			if (array == null)
			{
				list4.ForEach(delegate(Brep _)
				{
					outBrep = Brep.CreateBooleanDifference(outBrep[0], _, 0.01);
				});
				if (outBrep == null || outBrep.Length < 1)
				{
					return UpdateError("Scallop Boolean Error");
				}
			}
			else
			{
				outBrep = array;
			}
			outBrep = Brep.CreateBooleanDifference(outBrep, list5, 0.01);
			if (outBrep.Length < 1)
			{
				UpdateError("Scallop Pilot/Divot Error");
			}
			Brep geometry = outBrep[0];
			OutFrenchHalo.SetData(new CSOnGeometryData(geometry));
		}
		else if (boolean2)
		{
			CSDataArray cSDataArray = new CSDataArray();
			cSDataArray.Append(new CSOnGeometryData(baseBrep));
			cSDataArray.Append(scallopDataArray);
			cSDataArray.Append(graverDataArray);
			cSDataArray.Append(gemCutterDataArray);
			cSDataArray.Append(pilotHoleDataArray);
			OutFrenchHalo.SetData(cSDataArray);
		}
		if (boolean3)
		{
			OutBaseSurface.SetData(new CSOnGeometryData(baseBrep));
			OutGemCutters.SetData(gemCutterDataArray);
			OutPilotHoleCutters.SetData(pilotHoleDataArray);
			OutScallopCutters.SetData(scallopDataArray);
			OutGraverCutters.SetData(graverDataArray);
		}
		return base.Update();
	}

	private bool MakeBaseSurface(Curve haloProfile, CSGemData centerGem, Curve innerRail, Curve rail, Curve outerRail, double centerGemSpacing, double placement, bool useSweep, out Brep baseBrep)
	{
		CSGemData cSGemData = centerGem.Duplicate() as CSGemData;
		baseBrep = null;
		Brep breps;
		if (!useSweep)
		{
			if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out Curve curve, false, out string error, 0.0))
			{
				return false;
			}
			NurbsCurve nurbsCurve = curve.ToNurbsCurve();
			NurbsCurve nurbsCurve2 = haloProfile.ToNurbsCurve();
			List<Point3d> list = new List<Point3d>();
			foreach (ControlPoint item in (IEnumerable<ControlPoint>)nurbsCurve2.Points)
			{
				list.Add(item.Location);
			}
			NurbsSurface nurbsSurface = NurbsSurface.Create(3, isRational: true, nurbsCurve2.Order, nurbsCurve.Order, nurbsCurve2.Points.Count, nurbsCurve.Points.Count);
			for (int i = 0; i < list.Count; i++)
			{
				Point3d point3d = list[i];
				double distance = point3d.X + centerGemSpacing;
				if (!GemOffsetCurve.GemOffset(cSGemData, distance, out curve, false, out error, 0.0))
				{
					return false;
				}
				curve.Translate(cSGemData.Up * (point3d.Y + placement));
				NurbsCurve nurbsCurve3 = curve.ToNurbsCurve();
				if (nurbsCurve.Points.Count != nurbsCurve3.Points.Count)
				{
					return false;
				}
				for (int j = 0; j < nurbsCurve.Points.Count; j++)
				{
					Point3d location = nurbsCurve3.Points[j].Location;
					double weight = nurbsCurve2.Points[i].Weight * nurbsCurve3.Points[j].Weight;
					nurbsSurface.Points.SetControlPoint(i, j, new ControlPoint(location.X, location.Y, location.Z, weight));
				}
			}
			for (int k = 0; k < nurbsCurve2.Knots.Count; k++)
			{
				nurbsSurface.KnotsU[k] = nurbsCurve2.Knots[k];
			}
			for (int l = 0; l < nurbsCurve.Knots.Count; l++)
			{
				nurbsSurface.KnotsV[l] = nurbsCurve.Knots[l];
			}
			if (nurbsSurface == null || !nurbsSurface.IsValid)
			{
				return false;
			}
			breps = nurbsSurface.ToBrep();
		}
		else
		{
			Point3d testPoint = centerGem.GetCurve().PointAt(centerGem.BezelParam);
			rail.ClosestPoint(testPoint, out var t);
			testPoint = rail.PointAt(t);
			Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, testPoint, rail.TangentAt(t), centerGem.Up);
			haloProfile.Transform(xform);
			Curve[] profile = new Curve[1] { haloProfile };
			innerRail.ClosestPoint(rail.PointAt(t), out var t2);
			double[] parameters = new double[1] { t2 };
			outerRail.ClosestPoint(innerRail.PointAt(t2), out t2);
			double[] parameters2 = new double[1] { t2 };
			CSUtilSweepRC.Sweep2(innerRail, outerRail, profile, parameters, parameters2, out breps, closed: true, maintainHeight: true);
		}
		CSUtilGeometry.FixBrepSolidOrientation(ref breps);
		baseBrep = breps.DuplicateBrep();
		return true;
	}

	private bool MakeGemCuttersAndHoles(CSChannelGemData currentGem, Brep templateGem, Brep gemCutter, Brep pilotHoleCutter, bool useDivotCutter, double divotRadius, double divotCutterTranslate, double adjustCornerCutters, out Brep outGemCutter, out Brep outPilotHoleCutter)
	{
		outPilotHoleCutter = null;
		currentGem = currentGem.Duplicate() as CSChannelGemData;
		Brep brep = gemCutter.DuplicateBrep();
		Brep brep2 = pilotHoleCutter.DuplicateBrep();
		BoundingBox boundingBox = templateGem.GetBoundingBox(accurate: true);
		double xScaleFactor = currentGem.GemXSize / boundingBox.Diagonal.X;
		double yScaleFactor = currentGem.GemYSize / boundingBox.Diagonal.Y;
		double zScaleFactor = currentGem.GemZSize / boundingBox.Diagonal.Z;
		Transform xform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
		brep.Transform(xform);
		brep2.Transform(xform);
		double num = 1.0 / 300.0 * RhinoMath.ToDegrees(adjustCornerCutters);
		if (useDivotCutter)
		{
			Curve curve = new Circle(Plane.WorldXY, Point3d.Origin, divotRadius).ToNurbsCurve();
			Point3d center = curve.GetBoundingBox(accurate: true).Center;
			Vector3d motion = -Vector3d.ZAxis;
			motion.Unitize();
			motion *= divotRadius;
			center.Transform(Transform.Translation(motion));
			NurbsCurve nurbsCurve = CSUtilCurves.BlendCurve(curve.PointAtStart, center, -Vector3d.ZAxis, Vector3d.XAxis);
			curve.ClosestPoint(curve.PointAtStart, out var t);
			Curve[] profiles = new Curve[1] { nurbsCurve };
			CSUtilSweepRC.Sweep1(curve, profiles, new double[1] { t }, out var breps, closed: true, globalShapeBlend: false);
			Brep item = breps[0];
			Point3d destination = currentGem.ZMaxPoint + currentGem.Down * currentGem.GemZSize;
			Extrusion extrusion = Extrusion.Create(curve, currentGem.GemZSize * 2.0, cap: false);
			item = Brep.JoinBreps(new List<Brep>
			{
				extrusion.ToBrep(),
				item
			}, CSApplicationHelper.Application.Tolerance)[0];
			item = item.CapPlanarHoles(0.01);
			if (item.SolidOrientation == BrepSolidOrientation.Inward)
			{
				item.Flip();
			}
			item.Transform(Transform.Translation(0.0, 0.0, num));
			Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination, currentGem.Up, currentGem.ForwardVector);
			item.Transform(xform2);
			Transform xform3 = Transform.Translation(currentGem.Up * divotCutterTranslate);
			item.Transform(xform3);
			outPilotHoleCutter = item;
		}
		brep.Transform(Transform.Translation(0.0, 0.0, num));
		Transform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, currentGem.Center, currentGem.Up, currentGem.ForwardVector);
		brep.Transform(xform4);
		outGemCutter = brep;
		if (!useDivotCutter)
		{
			brep2.Transform(Transform.Translation(0.0, 0.0, num));
			brep2.Transform(xform4);
			outPilotHoleCutter = brep2;
		}
		if (num != 0.0)
		{
			Vector3d north = currentGem.North;
			north.Z = 0.0;
			north.Unitize();
			north *= num;
			outGemCutter.Transform(Transform.Translation(north));
			outPilotHoleCutter.Transform(Transform.Translation(north));
		}
		return true;
	}

	private bool MakeGravers(CSChannelGemData currentGem, CSChannelGemData nextGem, Curve clusterCurve, Brep templateGem, Brep graverBrep, out Brep endGraverBrep)
	{
		Brep brep = graverBrep.DuplicateBrep();
		BoundingBox boundingBox = templateGem.GetBoundingBox(accurate: true);
		double xScaleFactor = (currentGem.GemXSize + nextGem.GemXSize) / 2.0 / boundingBox.Diagonal.X;
		double yScaleFactor = (currentGem.GemYSize + nextGem.GemYSize) / 2.0 / boundingBox.Diagonal.Y;
		double zScaleFactor = (currentGem.GemZSize + nextGem.GemZSize) / 2.0 / boundingBox.Diagonal.Z;
		clusterCurve.ClosestPoint(currentGem.Center, out var t);
		clusterCurve.ClosestPoint(nextGem.Center, out var t2);
		Curve curve = clusterCurve.Trim(t, t2);
		if (curve == null)
		{
			UpdateError("Making Graver Error");
		}
		double length = curve.GetLength();
		if (length < currentGem.GemXSize / 2.0 + nextGem.GemXSize / 2.0)
		{
			curve = curve.Trim(CurveEnd.Both, (length - 0.1) / 2.0);
		}
		else
		{
			curve = curve.Trim(CurveEnd.Start, currentGem.GemXSize / 2.0);
			curve = curve.Trim(CurveEnd.End, nextGem.GemXSize / 2.0);
		}
		if (length == curve.GetLength())
		{
			NurbsCurve nurbsCurve = curve.ToNurbsCurve();
			double s = currentGem.GemXSize / 2.0 / length;
			double num = nextGem.GemXSize / 2.0 / length;
			nurbsCurve.NormalizedLengthParameter(s, out var t3);
			nurbsCurve.NormalizedLengthParameter(1.0 - num, out var t4);
			curve = nurbsCurve.Trim(t3, t4).ToNurbsCurve();
		}
		clusterCurve.ClosestPoint(curve.PointAtNormalizedLength(0.5), out t);
		Point3d destination = clusterCurve.PointAt(t);
		Vector3d destination2 = (currentGem.Up + nextGem.Up) / 2.0;
		Vector3d destination3 = clusterCurve.TangentAt(t);
		Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination, destination2, destination3);
		Transform xform2 = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
		brep.Transform(xform2);
		brep.Transform(xform);
		endGraverBrep = brep.DuplicateBrep();
		return true;
	}

	private bool MakeGraversCorner(CSChannelGemData currentGem, CSChannelGemData nextGem, Curve clusterCurve, Brep templateGem, Brep graverBrep, out Brep endGraverBrep)
	{
		Brep brep = graverBrep.DuplicateBrep();
		BoundingBox boundingBox = templateGem.GetBoundingBox(accurate: true);
		double xScaleFactor = (currentGem.GemXSize + nextGem.GemXSize) / 2.0 / boundingBox.Diagonal.X;
		double yScaleFactor = (currentGem.GemYSize + nextGem.GemYSize) / 2.0 / boundingBox.Diagonal.Y;
		double zScaleFactor = (currentGem.GemZSize + nextGem.GemZSize) / 2.0 / boundingBox.Diagonal.Z;
		clusterCurve.ClosestPoint(currentGem.Center, out var t);
		clusterCurve.ClosestPoint(nextGem.Center, out var t2);
		Curve curve = clusterCurve.Trim(t, t2);
		curve = curve.Trim(CurveEnd.Start, currentGem.GemXSize / 2.0);
		curve = curve.Trim(CurveEnd.End, nextGem.GemXSize / 2.0);
		clusterCurve.ClosestPoint(curve.PointAtNormalizedLength(0.5), out t);
		Point3d destination = clusterCurve.PointAt(t);
		Vector3d destination2 = (currentGem.Up + nextGem.Up) / 2.0;
		Vector3d destination3 = clusterCurve.TangentAt(t);
		Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination, destination2, destination3);
		Transform xform2 = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
		brep.Transform(xform2);
		Point3d corner = new Point3d(5.0, 0.0, 10.0);
		Point3d corner2 = new Point3d(-5.0, 0.0, 10.0);
		Point3d corner3 = new Point3d(-5.0, 0.0, -10.0);
		Point3d corner4 = new Point3d(5.0, 0.0, -10.0);
		Brep[] array = brep.Split(Brep.CreateFromCornerPoints(corner, corner2, corner3, corner4, 0.01), 0.01);
		if (array.Length != 2)
		{
			UpdateError("Split Graver messed up");
		}
		brep = array[0].DuplicateBrep();
		brep.Transform(xform);
		endGraverBrep = brep.DuplicateBrep();
		return true;
	}

	private bool MakeHaloProfile(double wallThickness, double profileHeight, CSChannelGemData gem, Brep templateGem, Curve shankTopProfile, Curve shankBotProfile, List<Curve> shankSideProfiles, Curve channelProfile, double clusterAngle, bool usingSweep, out double adjustPlacement, out Curve outTopProfile, out Curve outBotProfile, out Curve outSide1Profile, out Curve outSide2Profile, out Curve outShankProfile)
	{
		Curve curve = shankTopProfile.DuplicateCurve();
		Curve curve2 = shankBotProfile.DuplicateCurve();
		adjustPlacement = 0.0;
		Point3d pointAtStart = curve.PointAtStart;
		Curve curve3;
		Curve curve4;
		if (shankSideProfiles[0].PointAtEnd.DistanceTo(pointAtStart) < shankSideProfiles[1].PointAtEnd.DistanceTo(pointAtStart))
		{
			curve3 = shankSideProfiles[0].DuplicateCurve();
			curve4 = shankSideProfiles[1].DuplicateCurve();
		}
		else
		{
			curve3 = shankSideProfiles[1].DuplicateCurve();
			curve4 = shankSideProfiles[0].DuplicateCurve();
		}
		Curve curve5 = channelProfile.DuplicateCurve();
		BoundingBox boundingBox = templateGem.GetBoundingBox(accurate: true);
		double num = shankTopProfile.GetBoundingBox(accurate: true).Diagonal.Y - boundingBox.Diagonal.Y;
		Transform xform = Transform.Scale(xScaleFactor: gem.GemXSize / boundingBox.Diagonal.X, yScaleFactor: (gem.GemYSize + wallThickness * 2.0) / (boundingBox.Diagonal.Y + num), zScaleFactor: gem.GemZSize / boundingBox.Diagonal.Z, plane: Plane.WorldXY);
		curve5.Transform(xform);
		curve.Transform(xform);
		curve2.Transform(xform);
		curve3.Transform(xform);
		curve4.Transform(xform);
		BoundingBox boundingBox2 = curve.GetBoundingBox(accurate: true);
		Point3d destination = (usingSweep ? Point3d.Origin : (Point3d.Origin + (Vector3d.XAxis * boundingBox2.Diagonal.Y / 2.0 - Vector3d.YAxis * boundingBox2.Diagonal.Z)));
		Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, destination, Vector3d.YAxis, Vector3d.XAxis);
		curve5.Transform(xform2);
		Curve mergedCrv = curve.DuplicateCurve();
		mergedCrv.Transform(xform2);
		CurveIntersections curveIntersections = Intersection.CurveCurve(mergedCrv, curve5, 0.01, 0.01);
		if (curveIntersections.Count > 1)
		{
			Curve item = mergedCrv.Trim(0.0, curveIntersections[0].ParameterA);
			Curve item2 = mergedCrv.Trim(curveIntersections[1].ParameterA, 1.0);
			Curve item3 = curve5.Trim(curveIntersections[0].ParameterB, curveIntersections[1].ParameterB);
			CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { item, item3, item2 }, out mergedCrv, 0.01);
		}
		Curve curve6 = curve2.DuplicateCurve();
		curve6.Transform(Transform.Translation(0.0, 0.0, 0.0 - curve6.GetBoundingBox(accurate: true).Min.Z));
		boundingBox2 = mergedCrv.GetBoundingBox(accurate: true);
		Point3d destination2 = new Point3d(destination.X, boundingBox2.Max.Y, destination.Z) - Vector3d.YAxis * profileHeight;
		Transform xform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, destination2, Vector3d.YAxis, Vector3d.XAxis);
		curve6.Transform(xform3);
		double num2 = curve6.PointAtStart.DistanceTo(mergedCrv.PointAtEnd);
		Curve curve7 = curve3.DuplicateCurve();
		BoundingBox boundingBox3 = curve7.GetBoundingBox(accurate: true);
		curve7.Transform(Transform.Scale(Plane.WorldXY, 1.0, 1.0, num2 / boundingBox3.Diagonal.Z));
		Transform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(curve7.PointAtEnd, Vector3d.ZAxis, Vector3d.YAxis, mergedCrv.PointAtStart, Vector3d.YAxis, Vector3d.XAxis);
		curve7.Transform(xform4);
		Curve curve8 = curve4.DuplicateCurve();
		BoundingBox boundingBox4 = curve8.GetBoundingBox(accurate: true);
		curve8.Transform(Transform.Scale(Plane.WorldXY, 1.0, 1.0, num2 / boundingBox4.Diagonal.Z));
		Transform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(curve8.PointAtStart, Vector3d.ZAxis, Vector3d.YAxis, mergedCrv.PointAtEnd, Vector3d.YAxis, Vector3d.XAxis);
		curve8.Transform(xform5);
		outTopProfile = mergedCrv;
		outBotProfile = curve6;
		outSide1Profile = curve7;
		outSide2Profile = curve8;
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { mergedCrv, curve7, curve6, curve8 }, out Curve mergedCrv2, 0.01);
		if (mergedCrv2 == null)
		{
			outShankProfile = null;
			return false;
		}
		if (!usingSweep)
		{
			mergedCrv2.Transform(Transform.Translation(0.0, 0.0 - boundingBox2.Max.Y, 0.0));
			adjustPlacement = boundingBox2.Max.Y;
		}
		Transform xform6 = Transform.Rotation(clusterAngle, Vector3d.ZAxis, Point3d.Origin);
		mergedCrv2.Transform(xform6);
		outShankProfile = mergedCrv2;
		return true;
	}

	private bool MakeScallopCutter(CSGemData centerGem, CSChannelGemData currentGem, Brep templateGem, Curve scallopProfile, double scallopLength, double vectorAngle, out Brep scallopCutters)
	{
		scallopCutters = null;
		Point3d center = currentGem.Center;
		BoundingBox boundingBox = templateGem.GetBoundingBox(accurate: true);
		double xScaleFactor = currentGem.GemXSize / boundingBox.Diagonal.X;
		double yScaleFactor = currentGem.GemYSize / boundingBox.Diagonal.Y;
		double zScaleFactor = currentGem.GemZSize / boundingBox.Diagonal.Z;
		Transform xform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
		Curve curve = scallopProfile.DuplicateCurve();
		curve.Transform(xform);
		Vector3d up = currentGem.Up;
		up.Unitize();
		vectorAngle = Math.PI - vectorAngle;
		double x = curve.GetBoundingBox(accurate: true).Diagonal.X;
		double value = x - Math.Tan(vectorAngle / 2.0) * scallopLength;
		double num = x + Math.Tan(vectorAngle / 2.0) * scallopLength;
		Curve curve2 = curve.DuplicateCurve();
		Curve curve3 = curve.DuplicateCurve();
		curve2.Transform(Transform.Scale(Plane.WorldXY, Math.Abs(value) / x, 1.0, 1.0));
		curve3.Transform(Transform.Scale(Plane.WorldXY, num / x, 1.0, 1.0));
		Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, center, up, currentGem.ForwardVector);
		curve2.Transform(xform2);
		curve3.Transform(xform2);
		Vector3d rightVector = currentGem.RightVector;
		rightVector.Unitize();
		Vector3d inputVector = currentGem.Center - centerGem.Center;
		inputVector.Unitize();
		if (CSValueHandleHelper.AngleBetweenVectors(rightVector, inputVector) > Math.PI / 2.0)
		{
			rightVector.Reverse();
		}
		if (vectorAngle < 0.0)
		{
			curve2.Transform(Transform.Translation(rightVector * scallopLength));
			curve3.Transform(Transform.Translation(rightVector * (0.0 - scallopLength)));
		}
		else
		{
			curve2.Transform(Transform.Translation(rightVector * (0.0 - scallopLength)));
			curve3.Transform(Transform.Translation(rightVector * scallopLength));
		}
		Brep brep = CSUtilSweepRC.Loft(new List<Curve> { curve2, curve3 }, LoftType.Normal, doCapped: true);
		if (brep == null)
		{
			return false;
		}
		if (brep.SolidOrientation == BrepSolidOrientation.Inward)
		{
			brep.Flip();
		}
		scallopCutters = brep;
		return true;
	}

	private bool MakeScallopCutterCorner(CSChannelGemData currentGem, Brep templateGem, Curve scallopProfile, double scallopLength, Vector3d previousVector, Vector3d nextVector, double clusterAngle, out List<Brep> scallopCutter)
	{
		scallopCutter = new List<Brep>();
		Point3d center = currentGem.Center;
		BoundingBox boundingBox = templateGem.GetBoundingBox(accurate: true);
		double xScaleFactor = currentGem.GemXSize / boundingBox.Diagonal.X;
		double yScaleFactor = currentGem.GemYSize / boundingBox.Diagonal.Y;
		double zScaleFactor = currentGem.GemZSize / boundingBox.Diagonal.Z;
		Transform xform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
		Curve curve = scallopProfile.DuplicateCurve();
		curve.Transform(xform);
		Vector3d up = currentGem.Up;
		up.Unitize();
		double x = curve.GetBoundingBox(accurate: true).Diagonal.X;
		double value = x;
		double num = x;
		Curve curve2 = curve.DuplicateCurve();
		Curve curve3 = curve.DuplicateCurve();
		curve2.Transform(Transform.Scale(Plane.WorldXY, Math.Abs(value) / x, 1.0, 1.0));
		curve3.Transform(Transform.Scale(Plane.WorldXY, num / x, 1.0, 1.0));
		double dz = 1.0 / 300.0 * RhinoMath.ToDegrees(clusterAngle);
		curve2.Transform(Transform.Translation(0.0, 0.0, dz));
		curve3.Transform(Transform.Translation(0.0, 0.0, dz));
		Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, center, up, nextVector);
		curve2.Transform(xform2);
		xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, center, up, previousVector);
		curve3.Transform(xform2);
		Curve curve4 = curve2.DuplicateCurve();
		Curve curve5 = curve3.DuplicateCurve();
		nextVector.Transform(Transform.Rotation(clusterAngle, currentGem.ForwardVector, center));
		previousVector.Transform(Transform.Rotation(clusterAngle, currentGem.ForwardVector, center));
		Vector3d vector3d = previousVector;
		vector3d.Unitize();
		curve5.Transform(Transform.Translation(vector3d * scallopLength));
		vector3d = nextVector;
		vector3d.Unitize();
		curve4.Transform(Transform.Translation(vector3d * scallopLength));
		Brep brep = CSUtilSweepRC.Loft(new List<Curve> { curve2, curve4 }, LoftType.Normal, doCapped: true);
		Brep brep2 = CSUtilSweepRC.Loft(new List<Curve> { curve3, curve5 }, LoftType.Normal, doCapped: true);
		if (brep == null)
		{
			return false;
		}
		if (brep2 == null)
		{
			return false;
		}
		if (brep.SolidOrientation == BrepSolidOrientation.Inward)
		{
			brep.Flip();
		}
		if (brep2.SolidOrientation == BrepSolidOrientation.Inward)
		{
			brep2.Flip();
		}
		scallopCutter.Add(brep);
		scallopCutter.Add(brep2);
		return true;
	}
}
