using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSBlocks.UDT;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1;

[CSBlock("HeadFinish", "{FB9E9D46-0CE0-4A5E-ACE8-DBE9F331F5EB}")]
public sealed class CSHeadFinish : CSBlock
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InHeadProngObjects;

	public readonly CSInputChannel InHeadProngCurves;

	public readonly CSInputChannel InHeadRail1Objects;

	public readonly CSInputChannel InHeadRail1Curves;

	public readonly CSInputChannel InHeadRail2Objects;

	public readonly CSInputChannel InHeadRail2Curves;

	public readonly CSInputChannel InHeadRail3Objects;

	public readonly CSInputChannel InHeadRail3Curves;

	public readonly CSInputChannel InFastMulti;

	public readonly CSInputChannel InDIY;

	public readonly CSInputChannel InPullToBaseObject;

	public readonly CSInputChannel InBaseObject;

	public readonly CSOutputChannel OutHeads;

	public readonly CSOutputChannel OutProngDomes;

	public readonly CSOutputChannel OutConstructionCurves;

	public CSHeadFinish(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InHeadProngObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InHeadProngCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InHeadRail1Objects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InHeadRail1Curves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InHeadRail2Objects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InHeadRail2Curves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InHeadRail3Objects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InHeadRail3Curves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InFastMulti = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDIY = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InPullToBaseObject = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InBaseObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutHeads = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutProngDomes = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Array);
		OutConstructionCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		bool boolean = InFastMulti.Boolean;
		bool boolean2 = InDIY.Boolean;
		List<CSGemData> gemList = InGems.GemList;
		CSDataArray flattenArray = InBaseObject.GetFlattenArray();
		bool boolean3 = InPullToBaseObject.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray prongs = InHeadProngObjects.Data as CSDataArray;
		CSDataArray baseRails = InHeadRail1Objects.Data as CSDataArray;
		CSDataArray cSDataArray2 = InHeadRail2Objects.Data as CSDataArray;
		CSDataArray cSDataArray3 = InHeadRail3Objects.Data as CSDataArray;
		CSDataArray prongCurves = InHeadProngCurves.Data as CSDataArray;
		CSDataArray baseRailCurves = InHeadRail1Curves.Data as CSDataArray;
		CSDataArray data = InHeadRail2Curves.Data as CSDataArray;
		CSDataArray data2 = InHeadRail3Curves.Data as CSDataArray;
		if (boolean3)
		{
			prongs = prongs.Duplicate() as CSDataArray;
			baseRails = baseRails.Duplicate() as CSDataArray;
			prongCurves = prongCurves.Duplicate() as CSDataArray;
			baseRailCurves = baseRailCurves.Duplicate() as CSDataArray;
			PullToBase(flattenArray, gemList, ref prongs, ref baseRails, ref prongCurves, ref baseRailCurves);
		}
		CSGemData cSGemData = gemList[0];
		if (cSGemData == null)
		{
			return false;
		}
		cSGemData.GetGemXformRC(out var xDown, out var _);
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData2 = gemList[i];
			if (cSGemData2 == null)
			{
				return UpdateMissing("Invalid Gem");
			}
			CSDataArray cSDataArray4;
			if (boolean)
			{
				cSDataArray4 = new CSDataArray();
				if (prongs != null)
				{
					cSDataArray4.Append(prongs.GetFlattenArray().Duplicate());
				}
				if (baseRails != null)
				{
					cSDataArray4.Append(baseRails.GetFlattenArray().Duplicate());
				}
				if (cSDataArray2 != null)
				{
					cSDataArray4.Append(cSDataArray2.GetFlattenArray().Duplicate());
				}
				if (cSDataArray3 != null)
				{
					cSDataArray4.Append(cSDataArray3.GetFlattenArray().Duplicate());
				}
				cSGemData2.GetGemXformRC(out var _, out var xUp2);
				cSDataArray4.ApplyTransform(xUp2 * xDown);
			}
			else
			{
				cSDataArray4 = new CSDataArray();
				if (prongs != null)
				{
					cSDataArray4.Append(prongs.GetArrayItem(i));
				}
				if (baseRails != null && baseRails.GetArrayItem(i) is CSDataArray cSDataArray5 && cSDataArray5.GetArrayCount() == 1 && cSDataArray5.GetArrayItem(0) is CSOnGeometryData data3)
				{
					cSDataArray4.Append(data3);
				}
				if (cSDataArray2 != null && cSDataArray2.GetArrayItem(i) is CSDataArray cSDataArray6 && cSDataArray6.GetArrayCount() == 1 && cSDataArray6.GetArrayItem(0) is CSOnGeometryData data4)
				{
					cSDataArray4.Append(data4);
				}
				if (cSDataArray3 != null && cSDataArray3.GetArrayItem(i) is CSDataArray cSDataArray7 && cSDataArray7.GetArrayCount() == 1 && cSDataArray7.GetArrayItem(0) is CSOnGeometryData data5)
				{
					cSDataArray4.Append(data5);
				}
			}
			cSDataArray.Append(cSDataArray4);
		}
		if (!boolean2)
		{
			OutHeads.SetData(cSDataArray.GetFlattenArray());
			OutProngDomes.SetData(null);
			OutConstructionCurves.SetData(null);
			return base.Update();
		}
		OutHeads.SetData(null);
		OutProngDomes.SetData(cSDataArray);
		CSDataArray cSDataArray8 = new CSDataArray();
		cSDataArray8.Append(prongCurves);
		cSDataArray8.Append(baseRailCurves);
		cSDataArray8.Append(data);
		cSDataArray8.Append(data2);
		CSDataArray cSDataArray9 = new CSDataArray();
		if (boolean)
		{
			for (int j = 0; j < gemList.Count; j++)
			{
				CSGemData cSGemData3 = gemList[j];
				if (cSGemData3 == null)
				{
					return UpdateMissing("Invalid Gem");
				}
				CSData data6 = cSDataArray8.Duplicate();
				cSGemData3.GetGemXformRC(out var _, out var xUp3);
				CSDataHelper.Transform(data6, xUp3 * xDown);
				cSDataArray9.Append(data6);
			}
		}
		else
		{
			cSDataArray9 = cSDataArray8;
		}
		OutConstructionCurves.SetData(cSDataArray9);
		return base.Update();
	}

	private void PullToBase(CSDataArray baseObject, List<CSGemData> gems, ref CSDataArray prongs, ref CSDataArray baseRails, ref CSDataArray prongCurves, ref CSDataArray baseRailCurves)
	{
		if (baseObject.GetArrayCount() < 1)
		{
			return;
		}
		Brep brep = null;
		double num = double.NaN;
		CSData cSData = baseObject.GetArrayItem(0);
		if (cSData is CSRingRailData cSRingRailData)
		{
			num = cSRingRailData.Radius;
			cSData = null;
		}
		if (cSData != null)
		{
			if (cSData is ICSCurve iCSCurve)
			{
				Curve curve = iCSCurve.GetCurve();
				if (curve != null)
				{
					Curve curve2 = curve.DuplicateCurve();
					Curve curve3 = curve.DuplicateCurve();
					curve2.Translate(new Vector3d(0.0, 1.0, 0.0) * -50.0);
					curve3.Translate(new Vector3d(0.0, 1.0, 0.0) * 50.0);
					brep = NurbsSurface.CreateRuledSurface(curve2, curve3).ToBrep();
				}
			}
			if (brep == null && cSData is ICSBrep iCSBrep)
			{
				brep = iCSBrep.GetBrep();
			}
			if (brep == null && cSData is ICSSurface iCSSurface)
			{
				Surface surface = iCSSurface.GetSurface();
				if (surface != null)
				{
					brep = surface.ToBrep();
				}
			}
		}
		if (double.IsNaN(num) && brep == null)
		{
			return;
		}
		for (int i = 0; i < gems.Count; i++)
		{
			CSBlockUtil.CSUtilMath.GetUnscaledXform(gems[i].Xform, out var xUp);
			if (!xUp.TryGetInverse(out var inverseTransform))
			{
				break;
			}
			double angle = double.NaN;
			if (!double.IsNaN(num))
			{
				Point3d origin = Point3d.Origin;
				origin.Transform(xUp);
				Point3d point3d = new Point3d(Vector3d.ZAxis);
				point3d.Transform(xUp);
				angle = CSBlockUtil.CSUtilMath.AngleOfPointD(point3d.X - origin.X, point3d.Z - origin.Z);
				inverseTransform = PullToRailMorph.TopCenterXformFromAngle(angle).Convert();
			}
			BoundingBox boundingBox = BoundingBox.Unset;
			CSDataArray cSDataArray = (CSDataArray)prongs.GetArrayItem(i);
			CSDataArray cSDataArray2 = (CSDataArray)baseRails.GetArrayItem(i);
			cSDataArray.GetTightBoundingBox(ref boundingBox, doGrow: true, inverseTransform);
			cSDataArray2.GetTightBoundingBox(ref boundingBox, doGrow: true, inverseTransform);
			CSPullToBaseBrep cSPullToBaseBrep = null;
			SpaceMorph spaceMorph;
			if (!double.IsNaN(num))
			{
				spaceMorph = new CSPullToRailCenterMorph(angle, num, 1.0, boundingBox);
			}
			else
			{
				cSPullToBaseBrep = new CSPullToBaseBrep(boundingBox, brep, xUp, inverseTransform);
				spaceMorph = cSPullToBaseBrep;
			}
			spaceMorph.Tolerance = 0.05;
			spaceMorph.PreserveStructure = true;
			for (int j = 0; j < cSDataArray.GetArrayCount(); j++)
			{
				CSData arrayItem = cSDataArray.GetArrayItem(j);
				if (arrayItem == null)
				{
					continue;
				}
				CSOnGeometryData cSOnGeometryData = arrayItem as CSOnGeometryData;
				Brep brep2 = cSOnGeometryData.GetBrep();
				List<Brep> list = new List<Brep>();
				for (int k = 0; k < brep2.Faces.Count; k++)
				{
					Brep brep3 = brep2.Faces[k].ToBrep();
					spaceMorph.Morph(brep3);
					list.Add(brep3);
				}
				Brep[] array = Brep.JoinBreps(list, 0.05);
				Brep brep4 = null;
				if (array.Length != 0)
				{
					brep4 = array[0];
					if (!brep4.IsSolid)
					{
						Brep[] array2 = Brep.JoinBreps(list, 0.001);
						if (array2.Length != 0)
						{
							Brep brep5 = array2[0];
							if (brep5.IsSolid)
							{
								brep4 = brep5;
							}
						}
					}
				}
				cSOnGeometryData = new CSOnGeometryData(brep4);
				cSDataArray.Array[j] = cSOnGeometryData;
			}
			for (int l = 0; l < cSDataArray2.GetArrayCount(); l++)
			{
				CSData arrayItem2 = cSDataArray2.GetArrayItem(l);
				if (arrayItem2 != null)
				{
					if (cSPullToBaseBrep != null)
					{
						cSPullToBaseBrep.Count = 0;
					}
					arrayItem2.Morph(spaceMorph);
				}
			}
			for (int m = 0; m < prongCurves.GetArrayCount(); m++)
			{
				CSData arrayItem3 = prongCurves.GetArrayItem(m);
				if (arrayItem3 != null)
				{
					if (cSPullToBaseBrep != null)
					{
						cSPullToBaseBrep.Count = 0;
					}
					arrayItem3.Morph(spaceMorph);
				}
			}
			for (int n = 0; n < baseRailCurves.GetArrayCount(); n++)
			{
				CSData arrayItem4 = baseRailCurves.GetArrayItem(n);
				if (arrayItem4 != null)
				{
					if (cSPullToBaseBrep != null)
					{
						cSPullToBaseBrep.Count = 0;
					}
					arrayItem4.Morph(spaceMorph);
				}
			}
		}
	}
}
