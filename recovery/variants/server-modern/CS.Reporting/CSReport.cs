using System.Collections.Generic;
using System.Linq;
using CS.CSDataUtilities;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSValues;
using CS.MaterialGroup;
using CS.Parametric;
using CSCompatibility;
using CSDatas;
using CSMaterials;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;

namespace CS.Reporting;

public sealed class CSReport
{
	private DisplayComponents _displayComponent;

	private List<CSReportSurface> _surfaceList;

	public List<CSReportGem> GemList { get; private set; }

	public CSReportRingRail RingRail { get; private set; }

	public static void ClearGems()
	{
		CS.MaterialGroup.MaterialGroupHelper.ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.Highlighted);
		CSUtil.Redraw();
	}

	public CSReportGem Gem(int index)
	{
		if (!(index < GemList?.Count))
		{
			return null;
		}
		return GemList[index];
	}

	public int GemCount()
	{
		return GemList?.Count ?? 0;
	}

	public int MetalCountByComponent(ComponentsEnum comp, out bool has1, out bool has2)
	{
		has1 = false;
		has2 = false;
		foreach (CSReportSurface surface in _surfaceList)
		{
			if (surface.WhichComponent == comp)
			{
				if (surface.Part == MetalParts.Part1)
				{
					has1 = true;
				}
				if (surface.Part == MetalParts.Part2)
				{
					has2 = true;
				}
			}
		}
		int num = 0;
		if (has1)
		{
			num++;
		}
		if (has2)
		{
			num++;
		}
		return num;
	}

	public void PopulateReport()
	{
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		CSUtil.EnableDrawing(which: false);
		GemList = new List<CSReportGem>();
		_surfaceList = new List<CSReportSurface>();
		foreach (CSObject item in CSApplicationHelper.Application.Document.Graph.AllObjects())
		{
			if (item is CSValueRingSize)
			{
				if (RingRail == null)
				{
					RingRail = new CSReportRingRail();
				}
				CSValueRingSize cSValueRingSize = (CSValueRingSize)item;
				RingRail.RingSize = cSValueRingSize.Value * 2.0;
				RingRail.RingMax = cSValueRingSize.MaxValueRadiusMM * 2.0;
				RingRail.RingMin = cSValueRingSize.MinValueRadiusMM * 2.0;
			}
		}
		foreach (CSBlock item2 in CSApplicationHelper.Application.Document.Graph.DisplayObjects())
		{
			int outputChannelCount = item2.OutputChannelCount;
			for (int i = 0; i < outputChannelCount; i++)
			{
				CSData outputChannelData = item2.GetOutputChannelData(i);
				string outputIdentifier = CSReporting.GetOutputIdentifier(item2, i);
				if (outputChannelData == null)
				{
					continue;
				}
				MaterialClassification materialClassification = item2.OutputDisplayMaterialClassification;
				if (materialClassification == MaterialClassification.Undefined)
				{
					materialClassification = outputChannelData.DefaultDisplayMaterialClassification;
				}
				CSDataArray flattenArray = CSDataHelper.GetFlattenArray(outputChannelData);
				for (int j = 0; j < flattenArray.GetArrayCount(); j++)
				{
					CSData arrayItem = flattenArray.GetArrayItem(j);
					switch (CSReporting.GetDataType(arrayItem))
					{
					case IOReportingTypes.Gems:
					{
						if (!(arrayItem is CSGemData cSGemData))
						{
							break;
						}
						CSReportGem cSReportGem = new CSReportGem
						{
							GemData = (arrayItem as CSGemData),
							Identifier = $"{item2.InstanceGuid}:{i}:{j}",
							Name = outputIdentifier,
							Shape = cSGemData.GemShape,
							WhichSettingType = cSGemData.SettingType
						};
						if (item2.ParentPatchWire != null)
						{
							List<ComponentsEnum> list2 = CSUtil.DisplayComponentsToComponentsEnum(item2.ParentPatchWire.DisplayComponent);
							if (list2.Count > 0)
							{
								cSReportGem.WhichComponent = list2[0];
							}
						}
						cSReportGem.GemCenter = new On3dPoint(0.0, 0.0, 0.0);
						cSReportGem.GemCenter.Transform((IOnXform)(object)cSGemData.Xform.Convert());
						cSReportGem.GemX = cSGemData.GemXSize;
						cSReportGem.GemXMax = cSGemData.TableMaxX;
						cSReportGem.GemXMin = cSGemData.TableMinX;
						cSReportGem.GemY = cSGemData.GemYSize;
						cSReportGem.GemYMax = cSGemData.TableMaxY;
						cSReportGem.GemYMin = cSGemData.TableMinX;
						cSReportGem.GemZ = cSGemData.GemZSize;
						cSReportGem.FFactor = cSGemData.FFactor;
						cSReportGem.SetWeights();
						MaterialClassification materialClassification3 = materialClassification;
						if (materialClassification3 != MaterialClassification.Undefined)
						{
							materialClassification3 = cSGemData.DefaultDisplayMaterialClassification;
						}
						switch (materialClassification3)
						{
						case MaterialClassification.GemCenter:
							if (GemList != null && GemList.All((CSReportGem _) => _.GemPlacement != GemPlace.Center))
							{
								cSReportGem.GemPlacement = GemPlace.Center;
							}
							else
							{
								cSReportGem.GemPlacement = GemPlace.Side;
							}
							break;
						case MaterialClassification.GemSide:
							cSReportGem.GemPlacement = GemPlace.Side;
							break;
						case MaterialClassification.GemAccent:
							cSReportGem.GemPlacement = GemPlace.Melee;
							break;
						default:
							cSReportGem.GemPlacement = GemPlace.Unknown;
							break;
						}
						GemList.Add(cSReportGem);
						break;
					}
					case IOReportingTypes.Surfaces:
					{
						CSReportSurface cSReportSurface = new CSReportSurface
						{
							Name = outputIdentifier
						};
						MaterialClassification materialClassification2 = materialClassification;
						if (materialClassification2 != MaterialClassification.Undefined)
						{
							materialClassification2 = arrayItem.DefaultDisplayMaterialClassification;
						}
						switch (materialClassification2)
						{
						case MaterialClassification.MetalPrimary:
							cSReportSurface.Part = MetalParts.Part1;
							break;
						case MaterialClassification.MetalSecondary:
							cSReportSurface.Part = MetalParts.Part2;
							break;
						default:
							cSReportSurface.Part = MetalParts.Unknown;
							break;
						}
						if (item2.ParentPatchWire != null)
						{
							List<ComponentsEnum> list = CSUtil.DisplayComponentsToComponentsEnum(item2.ParentPatchWire.DisplayComponent);
							if (list.Count > 0)
							{
								cSReportSurface.WhichComponent = list[0];
							}
						}
						cSReportSurface.Volume = 0.0;
						_surfaceList.Add(cSReportSurface);
						if (item2.ParentPatchWire != null)
						{
							_displayComponent |= item2.ParentPatchWire.DisplayComponent;
						}
						break;
					}
					}
				}
			}
		}
		CSUtil.EnableDrawing(which: true);
	}

	public void ShowGemByGemIndex(int index)
	{
		if (GemList != null && index < GemList.Count)
		{
			CS.MaterialGroup.MaterialGroupHelper.AddItemToMaterialGroupByClassification(MaterialClassification.Highlighted, GemList[index].Identifier, redraw: false);
		}
	}

	public void ShowGemByGemNumber(string gemNumber)
	{
		if (GemList != null && GemList.Any() && !string.IsNullOrWhiteSpace(gemNumber))
		{
			CSReportGem cSReportGem = GemList.FirstOrDefault((CSReportGem gem) => string.Equals(gem.GemData.GemNumber, gemNumber));
			CS.MaterialGroup.MaterialGroupHelper.AddItemToMaterialGroupByClassification(MaterialClassification.Highlighted, cSReportGem.Identifier, redraw: false);
		}
	}
}
