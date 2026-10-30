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
using Rhino.Runtime;

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
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
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
		Curve val = null;
		Curve val2 = null;
		List<Curve> list2 = new List<Curve>();
		foreach (Curve item2 in value)
		{
			string text = ((GeometryBase)item2).GetUserString("CSName").ToLower();
			if (text == "top")
			{
				val = item2.DuplicateCurve();
			}
			if (text == "side")
			{
				list2.Add(item2.DuplicateCurve());
			}
			if (text == "bottom")
			{
				val2 = item2.DuplicateCurve();
			}
		}
		if (val == null || list2.Count != 2 || val2 == null)
		{
			return UpdateError("Error shank profile Curves");
		}
		gem.GetGemXformRC(out var xDown, out var xUp);
		CSFrenchHaloHelper.SetUpFrenchGems(gemDatas);
		gemDatas = CSFrenchHaloHelper.SorttoFirstCorner(gemDatas);
		CSFrenchHaloHelper.CalculateRail(gemDatas, 0, 1.0, 1.0, num, null, out var rail);
		CSFrenchHaloHelper.CalculateRail(gemDatas, -1, 1.0, 1.0, num, null, out var rail2);
		CSFrenchHaloHelper.CalculateRail(gemDatas, 1, 1.0, 1.0, num, null, out var rail3);
		if (!MakeHaloProfile(num, profileHeight, gemDatas[0], brep, val, val2, list2, curve, num3, boolean5, out var adjustPlacement, out var _, out var _, out var _, out var _, out var outShankProfile))
		{
			return UpdateError("halo profile failed");
		}
		num2 -= adjustPlacement / 2.0;
		((GeometryBase)rail2).Transform(xDown);
		((GeometryBase)rail3).Transform(xDown);
		Curve obj = rail2;
		double num4 = num2;
		Point3d pointAtStart = rail2.PointAtStart;
		((GeometryBase)obj).Transform(Transform.Translation(0.0, 0.0, num4 - ((Point3d)(ref pointAtStart)).Z));
		Curve obj2 = rail3;
		double num5 = num2;
		pointAtStart = rail3.PointAtStart;
		((GeometryBase)obj2).Transform(Transform.Translation(0.0, 0.0, num5 - ((Point3d)(ref pointAtStart)).Z));
		((GeometryBase)rail2).Transform(xUp);
		((GeometryBase)rail3).Transform(xUp);
		if (!MakeBaseSurface(outShankProfile, gem, rail2, rail, rail3, centerGemSpacing, num2, boolean5, out var baseBrep))
		{
			return UpdateError("make Base Profile failed");
		}
		if (!boolean5)
		{
			((GeometryBase)baseBrep).Transform(xUp);
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
				Transform val3 = CSBlockUtil.CSUtilMath.Orient3Point(cSChannelGemData.Center, cSChannelGemData.Up, cSChannelGemData.North, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
				((Point3d)(ref center)).Transform(val3);
				((Point3d)(ref center2)).Transform(val3);
				((Point3d)(ref center3)).Transform(val3);
				Vector3d val4 = center2 - center3;
				Vector3d val5 = center - center3;
				((Vector3d)(ref val5)).Z = 0.0;
				((Vector3d)(ref val4)).Z = 0.0;
				double vectorAngle = Vector3d.VectorAngle(val5, val4);
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
			gemCutterDataArray.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		CSDataArray pilotHoleDataArray = new CSDataArray();
		list5.ForEach(delegate(Brep _)
		{
			pilotHoleDataArray.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		CSDataArray scallopDataArray = new CSDataArray();
		scallopList.ForEach(delegate(Brep _)
		{
			scallopDataArray.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		CSDataArray graverDataArray = new CSDataArray();
		list3.ForEach(delegate(Brep _)
		{
			graverDataArray.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		if (boolean && boolean2)
		{
			Brep[] outBrep = list6.ToArray();
			Brep[] array = Brep.CreateBooleanDifference((IEnumerable<Brep>)list6, (IEnumerable<Brep>)list3, 0.01);
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
			array = Brep.CreateBooleanDifference((IEnumerable<Brep>)outBrep, (IEnumerable<Brep>)scallopList, 0.01);
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
			array = Brep.CreateBooleanDifference((IEnumerable<Brep>)outBrep, (IEnumerable<Brep>)list4, 0.01);
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
			outBrep = Brep.CreateBooleanDifference((IEnumerable<Brep>)outBrep, (IEnumerable<Brep>)list5, 0.01);
			if (outBrep.Length < 1)
			{
				UpdateError("Scallop Pilot/Divot Error");
			}
			Brep geometry = outBrep[0];
			OutFrenchHalo.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		}
		else if (boolean2)
		{
			CSDataArray cSDataArray = new CSDataArray();
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)baseBrep));
			cSDataArray.Append(scallopDataArray);
			cSDataArray.Append(graverDataArray);
			cSDataArray.Append(gemCutterDataArray);
			cSDataArray.Append(pilotHoleDataArray);
			OutFrenchHalo.SetData(cSDataArray);
		}
		if (boolean3)
		{
			OutBaseSurface.SetData(new CSOnGeometryData((GeometryBase)(object)baseBrep));
			OutGemCutters.SetData(gemCutterDataArray);
			OutPilotHoleCutters.SetData(pilotHoleDataArray);
			OutScallopCutters.SetData(scallopDataArray);
			OutGraverCutters.SetData(graverDataArray);
		}
		return base.Update();
	}

	private bool MakeBaseSurface(Curve haloProfile, CSGemData centerGem, Curve innerRail, Curve rail, Curve outerRail, double centerGemSpacing, double placement, bool useSweep, out Brep baseBrep)
	{
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		CSGemData cSGemData = centerGem.Duplicate() as CSGemData;
		baseBrep = null;
		Brep breps;
		if (!useSweep)
		{
			if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out Curve curve, false, out string error, 0.0))
			{
				return false;
			}
			NurbsCurve val = curve.ToNurbsCurve();
			NurbsCurve val2 = haloProfile.ToNurbsCurve();
			List<Point3d> list = new List<Point3d>();
			foreach (ControlPoint item in (IEnumerable<ControlPoint>)val2.Points)
			{
				ControlPoint current = item;
				list.Add(((ControlPoint)(ref current)).Location);
			}
			NurbsSurface val3 = NurbsSurface.Create(3, true, val2.Order, val.Order, val2.Points.Count, val.Points.Count);
			for (int i = 0; i < list.Count; i++)
			{
				Point3d val4 = list[i];
				double distance = ((Point3d)(ref val4)).X + centerGemSpacing;
				if (!GemOffsetCurve.GemOffset(cSGemData, distance, out curve, false, out error, 0.0))
				{
					return false;
				}
				((GeometryBase)curve).Translate(cSGemData.Up * (((Point3d)(ref val4)).Y + placement));
				NurbsCurve val5 = curve.ToNurbsCurve();
				if (val.Points.Count != val5.Points.Count)
				{
					return false;
				}
				for (int j = 0; j < val.Points.Count; j++)
				{
					ControlPoint val6 = val5.Points[j];
					Point3d location = ((ControlPoint)(ref val6)).Location;
					val6 = val2.Points[i];
					double weight = ((ControlPoint)(ref val6)).Weight;
					val6 = val5.Points[j];
					double num = weight * ((ControlPoint)(ref val6)).Weight;
					val3.Points.SetControlPoint(i, j, new ControlPoint(((Point3d)(ref location)).X, ((Point3d)(ref location)).Y, ((Point3d)(ref location)).Z, num));
				}
			}
			for (int k = 0; k < val2.Knots.Count; k++)
			{
				val3.KnotsU[k] = val2.Knots[k];
			}
			for (int l = 0; l < val.Knots.Count; l++)
			{
				val3.KnotsV[l] = val.Knots[l];
			}
			if (val3 == null || !((CommonObject)val3).IsValid)
			{
				return false;
			}
			breps = ((Surface)val3).ToBrep();
		}
		else
		{
			Point3d val7 = centerGem.GetCurve().PointAt(centerGem.BezelParam);
			double num2 = default(double);
			rail.ClosestPoint(val7, ref num2);
			val7 = rail.PointAt(num2);
			Transform val8 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, val7, rail.TangentAt(num2), centerGem.Up);
			((GeometryBase)haloProfile).Transform(val8);
			Curve[] profile = (Curve[])(object)new Curve[1] { haloProfile };
			double num3 = default(double);
			innerRail.ClosestPoint(rail.PointAt(num2), ref num3);
			double[] parameters = new double[1] { num3 };
			outerRail.ClosestPoint(innerRail.PointAt(num3), ref num3);
			double[] parameters2 = new double[1] { num3 };
			CSUtilSweepRC.Sweep2(innerRail, outerRail, profile, parameters, parameters2, out breps, closed: true, maintainHeight: true);
		}
		CSUtilGeometry.FixBrepSolidOrientation(ref breps);
		baseBrep = breps.DuplicateBrep();
		return true;
	}

	private bool MakeGemCuttersAndHoles(CSChannelGemData currentGem, Brep templateGem, Brep gemCutter, Brep pilotHoleCutter, bool useDivotCutter, double divotRadius, double divotCutterTranslate, double adjustCornerCutters, out Brep outGemCutter, out Brep outPilotHoleCutter)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Invalid comparison between Unknown and I4
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		outPilotHoleCutter = null;
		currentGem = currentGem.Duplicate() as CSChannelGemData;
		Brep val = gemCutter.DuplicateBrep();
		Brep val2 = pilotHoleCutter.DuplicateBrep();
		BoundingBox boundingBox = ((GeometryBase)templateGem).GetBoundingBox(true);
		double gemXSize = currentGem.GemXSize;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num = gemXSize / ((Vector3d)(ref diagonal)).X;
		double gemYSize = currentGem.GemYSize;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num2 = gemYSize / ((Vector3d)(ref diagonal)).Y;
		double gemZSize = currentGem.GemZSize;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num3 = gemZSize / ((Vector3d)(ref diagonal)).Z;
		Transform val3 = Transform.Scale(Plane.WorldXY, num, num2, num3);
		((GeometryBase)val).Transform(val3);
		((GeometryBase)val2).Transform(val3);
		double num4 = 1.0 / 300.0 * RhinoMath.ToDegrees(adjustCornerCutters);
		if (useDivotCutter)
		{
			Circle val4 = new Circle(Plane.WorldXY, Point3d.Origin, divotRadius);
			Curve val5 = (Curve)(object)((Circle)(ref val4)).ToNurbsCurve();
			BoundingBox boundingBox2 = ((GeometryBase)val5).GetBoundingBox(true);
			Point3d center = ((BoundingBox)(ref boundingBox2)).Center;
			Vector3d val6 = -Vector3d.ZAxis;
			((Vector3d)(ref val6)).Unitize();
			val6 *= divotRadius;
			((Point3d)(ref center)).Transform(Transform.Translation(val6));
			NurbsCurve val7 = CSUtilCurves.BlendCurve(val5.PointAtStart, center, -Vector3d.ZAxis, Vector3d.XAxis);
			double num5 = default(double);
			val5.ClosestPoint(val5.PointAtStart, ref num5);
			Curve[] profiles = (Curve[])(object)new Curve[1] { (Curve)val7 };
			CSUtilSweepRC.Sweep1(val5, profiles, new double[1] { num5 }, out var breps, closed: true, globalShapeBlend: false);
			Brep item = breps[0];
			Point3d destination = currentGem.ZMaxPoint + currentGem.Down * currentGem.GemZSize;
			Extrusion val8 = Extrusion.Create(val5, currentGem.GemZSize * 2.0, false);
			item = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep>
			{
				((Surface)val8).ToBrep(),
				item
			}, CSApplicationHelper.Application.Tolerance)[0];
			item = item.CapPlanarHoles(0.01);
			if ((int)item.SolidOrientation == -1)
			{
				item.Flip();
			}
			((GeometryBase)item).Transform(Transform.Translation(0.0, 0.0, num4));
			Transform val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination, currentGem.Up, currentGem.ForwardVector);
			((GeometryBase)item).Transform(val9);
			Transform val10 = Transform.Translation(currentGem.Up * divotCutterTranslate);
			((GeometryBase)item).Transform(val10);
			outPilotHoleCutter = item;
		}
		((GeometryBase)val).Transform(Transform.Translation(0.0, 0.0, num4));
		Transform val11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, currentGem.Center, currentGem.Up, currentGem.ForwardVector);
		((GeometryBase)val).Transform(val11);
		outGemCutter = val;
		if (!useDivotCutter)
		{
			((GeometryBase)val2).Transform(Transform.Translation(0.0, 0.0, num4));
			((GeometryBase)val2).Transform(val11);
			outPilotHoleCutter = val2;
		}
		if (num4 != 0.0)
		{
			Vector3d val12 = currentGem.North;
			((Vector3d)(ref val12)).Z = 0.0;
			((Vector3d)(ref val12)).Unitize();
			val12 *= num4;
			((GeometryBase)outGemCutter).Transform(Transform.Translation(val12));
			((GeometryBase)outPilotHoleCutter).Transform(Transform.Translation(val12));
		}
		return true;
	}

	private bool MakeGravers(CSChannelGemData currentGem, CSChannelGemData nextGem, Curve clusterCurve, Brep templateGem, Brep graverBrep, out Brep endGraverBrep)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		Brep val = graverBrep.DuplicateBrep();
		BoundingBox boundingBox = ((GeometryBase)templateGem).GetBoundingBox(true);
		double num = (currentGem.GemXSize + nextGem.GemXSize) / 2.0;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num2 = num / ((Vector3d)(ref diagonal)).X;
		double num3 = (currentGem.GemYSize + nextGem.GemYSize) / 2.0;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num4 = num3 / ((Vector3d)(ref diagonal)).Y;
		double num5 = (currentGem.GemZSize + nextGem.GemZSize) / 2.0;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num6 = num5 / ((Vector3d)(ref diagonal)).Z;
		double num7 = default(double);
		clusterCurve.ClosestPoint(currentGem.Center, ref num7);
		double num8 = default(double);
		clusterCurve.ClosestPoint(nextGem.Center, ref num8);
		Curve val2 = clusterCurve.Trim(num7, num8);
		if (val2 == null)
		{
			UpdateError("Making Graver Error");
		}
		double length = val2.GetLength();
		if (length < currentGem.GemXSize / 2.0 + nextGem.GemXSize / 2.0)
		{
			val2 = val2.Trim((CurveEnd)3, (length - 0.1) / 2.0);
		}
		else
		{
			val2 = val2.Trim((CurveEnd)1, currentGem.GemXSize / 2.0);
			val2 = val2.Trim((CurveEnd)2, nextGem.GemXSize / 2.0);
		}
		if (length == val2.GetLength())
		{
			NurbsCurve obj = val2.ToNurbsCurve();
			double num9 = currentGem.GemXSize / 2.0 / length;
			double num10 = nextGem.GemXSize / 2.0 / length;
			double num11 = default(double);
			((Curve)obj).NormalizedLengthParameter(num9, ref num11);
			double num12 = default(double);
			((Curve)obj).NormalizedLengthParameter(1.0 - num10, ref num12);
			val2 = (Curve)(object)((Curve)obj).Trim(num11, num12).ToNurbsCurve();
		}
		clusterCurve.ClosestPoint(val2.PointAtNormalizedLength(0.5), ref num7);
		Point3d destination = clusterCurve.PointAt(num7);
		Vector3d destination2 = (currentGem.Up + nextGem.Up) / 2.0;
		Vector3d destination3 = clusterCurve.TangentAt(num7);
		Transform val3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination, destination2, destination3);
		Transform val4 = Transform.Scale(Plane.WorldXY, num2, num4, num6);
		((GeometryBase)val).Transform(val4);
		((GeometryBase)val).Transform(val3);
		endGraverBrep = val.DuplicateBrep();
		return true;
	}

	private bool MakeGraversCorner(CSChannelGemData currentGem, CSChannelGemData nextGem, Curve clusterCurve, Brep templateGem, Brep graverBrep, out Brep endGraverBrep)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		Brep val = graverBrep.DuplicateBrep();
		BoundingBox boundingBox = ((GeometryBase)templateGem).GetBoundingBox(true);
		double num = (currentGem.GemXSize + nextGem.GemXSize) / 2.0;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num2 = num / ((Vector3d)(ref diagonal)).X;
		double num3 = (currentGem.GemYSize + nextGem.GemYSize) / 2.0;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num4 = num3 / ((Vector3d)(ref diagonal)).Y;
		double num5 = (currentGem.GemZSize + nextGem.GemZSize) / 2.0;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num6 = num5 / ((Vector3d)(ref diagonal)).Z;
		double num7 = default(double);
		clusterCurve.ClosestPoint(currentGem.Center, ref num7);
		double num8 = default(double);
		clusterCurve.ClosestPoint(nextGem.Center, ref num8);
		Curve val2 = clusterCurve.Trim(num7, num8);
		val2 = val2.Trim((CurveEnd)1, currentGem.GemXSize / 2.0);
		val2 = val2.Trim((CurveEnd)2, nextGem.GemXSize / 2.0);
		clusterCurve.ClosestPoint(val2.PointAtNormalizedLength(0.5), ref num7);
		Point3d destination = clusterCurve.PointAt(num7);
		Vector3d destination2 = (currentGem.Up + nextGem.Up) / 2.0;
		Vector3d destination3 = clusterCurve.TangentAt(num7);
		Transform val3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination, destination2, destination3);
		Transform val4 = Transform.Scale(Plane.WorldXY, num2, num4, num6);
		((GeometryBase)val).Transform(val4);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(5.0, 0.0, 10.0);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(-5.0, 0.0, 10.0);
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(-5.0, 0.0, -10.0);
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector(5.0, 0.0, -10.0);
		Brep[] array = val.Split(Brep.CreateFromCornerPoints(val5, val6, val7, val8, 0.01), 0.01);
		if (array.Length != 2)
		{
			UpdateError("Split Graver messed up");
		}
		val = array[0].DuplicateBrep();
		((GeometryBase)val).Transform(val3);
		endGraverBrep = val.DuplicateBrep();
		return true;
	}

	private bool MakeHaloProfile(double wallThickness, double profileHeight, CSChannelGemData gem, Brep templateGem, Curve shankTopProfile, Curve shankBotProfile, List<Curve> shankSideProfiles, Curve channelProfile, double clusterAngle, bool usingSweep, out double adjustPlacement, out Curve outTopProfile, out Curve outBotProfile, out Curve outSide1Profile, out Curve outSide2Profile, out Curve outShankProfile)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		Curve obj = shankTopProfile.DuplicateCurve();
		Curve val = shankBotProfile.DuplicateCurve();
		adjustPlacement = 0.0;
		Point3d pointAtStart = obj.PointAtStart;
		Point3d val2 = shankSideProfiles[0].PointAtEnd;
		double num = ((Point3d)(ref val2)).DistanceTo(pointAtStart);
		val2 = shankSideProfiles[1].PointAtEnd;
		Curve val3;
		Curve val4;
		if (num < ((Point3d)(ref val2)).DistanceTo(pointAtStart))
		{
			val3 = shankSideProfiles[0].DuplicateCurve();
			val4 = shankSideProfiles[1].DuplicateCurve();
		}
		else
		{
			val3 = shankSideProfiles[1].DuplicateCurve();
			val4 = shankSideProfiles[0].DuplicateCurve();
		}
		Curve val5 = channelProfile.DuplicateCurve();
		BoundingBox boundingBox = ((GeometryBase)templateGem).GetBoundingBox(true);
		BoundingBox boundingBox2 = ((GeometryBase)shankTopProfile).GetBoundingBox(true);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num2 = y - ((Vector3d)(ref diagonal)).Y;
		double gemXSize = gem.GemXSize;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num3 = gemXSize / ((Vector3d)(ref diagonal)).X;
		double num4 = gem.GemYSize + wallThickness * 2.0;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num5 = num4 / (((Vector3d)(ref diagonal)).Y + num2);
		double gemZSize = gem.GemZSize;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num6 = gemZSize / ((Vector3d)(ref diagonal)).Z;
		Transform val6 = Transform.Scale(Plane.WorldXY, num3, num5, num6);
		((GeometryBase)val5).Transform(val6);
		((GeometryBase)obj).Transform(val6);
		((GeometryBase)val).Transform(val6);
		((GeometryBase)val3).Transform(val6);
		((GeometryBase)val4).Transform(val6);
		BoundingBox boundingBox3 = ((GeometryBase)obj).GetBoundingBox(true);
		Point3d val8;
		if (!usingSweep)
		{
			Point3d origin = Point3d.Origin;
			Vector3d xAxis = Vector3d.XAxis;
			diagonal = ((BoundingBox)(ref boundingBox3)).Diagonal;
			Vector3d val7 = xAxis * ((Vector3d)(ref diagonal)).Y / 2.0;
			Vector3d yAxis = Vector3d.YAxis;
			diagonal = ((BoundingBox)(ref boundingBox3)).Diagonal;
			val8 = origin + (val7 - yAxis * ((Vector3d)(ref diagonal)).Z);
		}
		else
		{
			val8 = Point3d.Origin;
		}
		Point3d destination = val8;
		Transform val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, destination, Vector3d.YAxis, Vector3d.XAxis);
		((GeometryBase)val5).Transform(val9);
		Curve mergedCrv = obj.DuplicateCurve();
		((GeometryBase)mergedCrv).Transform(val9);
		CurveIntersections val10 = Intersection.CurveCurve(mergedCrv, val5, 0.01, 0.01);
		if (val10.Count > 1)
		{
			Curve item = mergedCrv.Trim(0.0, val10[0].ParameterA);
			Curve item2 = mergedCrv.Trim(val10[1].ParameterA, 1.0);
			Curve item3 = val5.Trim(val10[0].ParameterB, val10[1].ParameterB);
			CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { item, item3, item2 }, out mergedCrv, 0.01);
		}
		Curve val11 = val.DuplicateCurve();
		BoundingBox boundingBox4 = ((GeometryBase)val11).GetBoundingBox(true);
		val2 = ((BoundingBox)(ref boundingBox4)).Min;
		((GeometryBase)val11).Transform(Transform.Translation(0.0, 0.0, 0.0 - ((Point3d)(ref val2)).Z));
		boundingBox3 = ((GeometryBase)mergedCrv).GetBoundingBox(true);
		double x = ((Point3d)(ref destination)).X;
		val2 = ((BoundingBox)(ref boundingBox3)).Max;
		Point3d destination2 = new Point3d(x, ((Point3d)(ref val2)).Y, ((Point3d)(ref destination)).Z) - Vector3d.YAxis * profileHeight;
		Transform val12 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, destination2, Vector3d.YAxis, Vector3d.XAxis);
		((GeometryBase)val11).Transform(val12);
		val2 = val11.PointAtStart;
		double num7 = ((Point3d)(ref val2)).DistanceTo(mergedCrv.PointAtEnd);
		Curve val13 = val3.DuplicateCurve();
		BoundingBox boundingBox5 = ((GeometryBase)val13).GetBoundingBox(true);
		Plane worldXY = Plane.WorldXY;
		diagonal = ((BoundingBox)(ref boundingBox5)).Diagonal;
		((GeometryBase)val13).Transform(Transform.Scale(worldXY, 1.0, 1.0, num7 / ((Vector3d)(ref diagonal)).Z));
		Transform val14 = CSBlockUtil.CSUtilMath.Orient3Point(val13.PointAtEnd, Vector3d.ZAxis, Vector3d.YAxis, mergedCrv.PointAtStart, Vector3d.YAxis, Vector3d.XAxis);
		((GeometryBase)val13).Transform(val14);
		Curve val15 = val4.DuplicateCurve();
		BoundingBox boundingBox6 = ((GeometryBase)val15).GetBoundingBox(true);
		Plane worldXY2 = Plane.WorldXY;
		diagonal = ((BoundingBox)(ref boundingBox6)).Diagonal;
		((GeometryBase)val15).Transform(Transform.Scale(worldXY2, 1.0, 1.0, num7 / ((Vector3d)(ref diagonal)).Z));
		Transform val16 = CSBlockUtil.CSUtilMath.Orient3Point(val15.PointAtStart, Vector3d.ZAxis, Vector3d.YAxis, mergedCrv.PointAtEnd, Vector3d.YAxis, Vector3d.XAxis);
		((GeometryBase)val15).Transform(val16);
		outTopProfile = mergedCrv;
		outBotProfile = val11;
		outSide1Profile = val13;
		outSide2Profile = val15;
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { mergedCrv, val13, val11, val15 }, out Curve mergedCrv2, 0.01);
		if (mergedCrv2 == null)
		{
			outShankProfile = null;
			return false;
		}
		if (!usingSweep)
		{
			Curve obj2 = mergedCrv2;
			val2 = ((BoundingBox)(ref boundingBox3)).Max;
			((GeometryBase)obj2).Transform(Transform.Translation(0.0, 0.0 - ((Point3d)(ref val2)).Y, 0.0));
			val2 = ((BoundingBox)(ref boundingBox3)).Max;
			adjustPlacement = ((Point3d)(ref val2)).Y;
		}
		Transform val17 = Transform.Rotation(clusterAngle, Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)mergedCrv2).Transform(val17);
		outShankProfile = mergedCrv2;
		return true;
	}

	private bool MakeScallopCutter(CSGemData centerGem, CSChannelGemData currentGem, Brep templateGem, Curve scallopProfile, double scallopLength, double vectorAngle, out Brep scallopCutters)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Invalid comparison between Unknown and I4
		scallopCutters = null;
		Point3d center = currentGem.Center;
		BoundingBox boundingBox = ((GeometryBase)templateGem).GetBoundingBox(true);
		double gemXSize = currentGem.GemXSize;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num = gemXSize / ((Vector3d)(ref diagonal)).X;
		double gemYSize = currentGem.GemYSize;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num2 = gemYSize / ((Vector3d)(ref diagonal)).Y;
		double gemZSize = currentGem.GemZSize;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num3 = gemZSize / ((Vector3d)(ref diagonal)).Z;
		Transform val = Transform.Scale(Plane.WorldXY, num, num2, num3);
		Curve obj = scallopProfile.DuplicateCurve();
		((GeometryBase)obj).Transform(val);
		Vector3d up = currentGem.Up;
		((Vector3d)(ref up)).Unitize();
		vectorAngle = Math.PI - vectorAngle;
		BoundingBox boundingBox2 = ((GeometryBase)obj).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		double value = x - Math.Tan(vectorAngle / 2.0) * scallopLength;
		double num4 = x + Math.Tan(vectorAngle / 2.0) * scallopLength;
		Curve val2 = obj.DuplicateCurve();
		Curve val3 = obj.DuplicateCurve();
		((GeometryBase)val2).Transform(Transform.Scale(Plane.WorldXY, Math.Abs(value) / x, 1.0, 1.0));
		((GeometryBase)val3).Transform(Transform.Scale(Plane.WorldXY, num4 / x, 1.0, 1.0));
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, center, up, currentGem.ForwardVector);
		((GeometryBase)val2).Transform(val4);
		((GeometryBase)val3).Transform(val4);
		Vector3d rightVector = currentGem.RightVector;
		((Vector3d)(ref rightVector)).Unitize();
		Vector3d inputVector = currentGem.Center - centerGem.Center;
		((Vector3d)(ref inputVector)).Unitize();
		if (CSValueHandleHelper.AngleBetweenVectors(rightVector, inputVector) > Math.PI / 2.0)
		{
			((Vector3d)(ref rightVector)).Reverse();
		}
		if (vectorAngle < 0.0)
		{
			((GeometryBase)val2).Transform(Transform.Translation(rightVector * scallopLength));
			((GeometryBase)val3).Transform(Transform.Translation(rightVector * (0.0 - scallopLength)));
		}
		else
		{
			((GeometryBase)val2).Transform(Transform.Translation(rightVector * (0.0 - scallopLength)));
			((GeometryBase)val3).Transform(Transform.Translation(rightVector * scallopLength));
		}
		Brep val5 = CSUtilSweepRC.Loft(new List<Curve> { val2, val3 }, (LoftType)0, doCapped: true);
		if (val5 == null)
		{
			return false;
		}
		if ((int)val5.SolidOrientation == -1)
		{
			val5.Flip();
		}
		scallopCutters = val5;
		return true;
	}

	private bool MakeScallopCutterCorner(CSChannelGemData currentGem, Brep templateGem, Curve scallopProfile, double scallopLength, Vector3d previousVector, Vector3d nextVector, double clusterAngle, out List<Brep> scallopCutter)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Invalid comparison between Unknown and I4
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Invalid comparison between Unknown and I4
		scallopCutter = new List<Brep>();
		Point3d center = currentGem.Center;
		BoundingBox boundingBox = ((GeometryBase)templateGem).GetBoundingBox(true);
		double gemXSize = currentGem.GemXSize;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num = gemXSize / ((Vector3d)(ref diagonal)).X;
		double gemYSize = currentGem.GemYSize;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num2 = gemYSize / ((Vector3d)(ref diagonal)).Y;
		double gemZSize = currentGem.GemZSize;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num3 = gemZSize / ((Vector3d)(ref diagonal)).Z;
		Transform val = Transform.Scale(Plane.WorldXY, num, num2, num3);
		Curve obj = scallopProfile.DuplicateCurve();
		((GeometryBase)obj).Transform(val);
		Vector3d up = currentGem.Up;
		((Vector3d)(ref up)).Unitize();
		BoundingBox boundingBox2 = ((GeometryBase)obj).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		double value = x;
		double num4 = x;
		Curve val2 = obj.DuplicateCurve();
		Curve val3 = obj.DuplicateCurve();
		((GeometryBase)val2).Transform(Transform.Scale(Plane.WorldXY, Math.Abs(value) / x, 1.0, 1.0));
		((GeometryBase)val3).Transform(Transform.Scale(Plane.WorldXY, num4 / x, 1.0, 1.0));
		double num5 = 1.0 / 300.0 * RhinoMath.ToDegrees(clusterAngle);
		((GeometryBase)val2).Transform(Transform.Translation(0.0, 0.0, num5));
		((GeometryBase)val3).Transform(Transform.Translation(0.0, 0.0, num5));
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, center, up, nextVector);
		((GeometryBase)val2).Transform(val4);
		val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, center, up, previousVector);
		((GeometryBase)val3).Transform(val4);
		Curve val5 = val2.DuplicateCurve();
		Curve val6 = val3.DuplicateCurve();
		((Vector3d)(ref nextVector)).Transform(Transform.Rotation(clusterAngle, currentGem.ForwardVector, center));
		((Vector3d)(ref previousVector)).Transform(Transform.Rotation(clusterAngle, currentGem.ForwardVector, center));
		Vector3d val7 = previousVector;
		((Vector3d)(ref val7)).Unitize();
		((GeometryBase)val6).Transform(Transform.Translation(val7 * scallopLength));
		val7 = nextVector;
		((Vector3d)(ref val7)).Unitize();
		((GeometryBase)val5).Transform(Transform.Translation(val7 * scallopLength));
		Brep val8 = CSUtilSweepRC.Loft(new List<Curve> { val2, val5 }, (LoftType)0, doCapped: true);
		Brep val9 = CSUtilSweepRC.Loft(new List<Curve> { val3, val6 }, (LoftType)0, doCapped: true);
		if (val8 == null)
		{
			return false;
		}
		if (val9 == null)
		{
			return false;
		}
		if ((int)val8.SolidOrientation == -1)
		{
			val8.Flip();
		}
		if ((int)val9.SolidOrientation == -1)
		{
			val9.Flip();
		}
		scallopCutter.Add(val8);
		scallopCutter.Add(val9);
		return true;
	}
}
