using System.Collections.Generic;
using System.Linq;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CSDatas;
using CSMaterials;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CS.Utilities.Helpers;

public static class GraphHelper
{
	public static List<CSData> GetDisplayObjectsByComponentAndMaterialClassification(ComponentsEnum whichComponent, params MaterialClassification[] whichMaterialClassifications)
	{
		List<CSData> list = new List<CSData>();
		Transform identity = Transform.Identity;
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.DisplayObjects())
		{
			CSPatchWireStart cSPatchWireStart = item?.ParentPatchWire;
			if (cSPatchWireStart == null || !CSUtil.DisplayComponentsToComponentsEnum(cSPatchWireStart.DisplayComponent).Contains(whichComponent))
			{
				continue;
			}
			for (int i = 0; i < item.OutputChannelCount; i++)
			{
				if (item.GetOutputChannelData(i) == null)
				{
					continue;
				}
				foreach (MaterialClassification searchMaterialClassification in whichMaterialClassifications)
				{
					CSData outputChannelData = item.GetOutputChannelData(i);
					if (!(outputChannelData is CSDataArray cSDataArray))
					{
						outputChannelData.GetAllMaterial(list, identity, item.OutputDisplayMaterialClassification, searchMaterialClassification);
						continue;
					}
					foreach (CSData item2 in (from data in cSDataArray.GetFlattenedList()
						where data != null
						select data).ToList())
					{
						item2.GetAllMaterial(list, identity, item.OutputDisplayMaterialClassification, searchMaterialClassification);
					}
				}
			}
		}
		return list;
	}

	public static List<CSData> GetDisplayObjectsByMaterialClassification(params MaterialClassification[] whichMaterialClassifications)
	{
		List<CSData> list = new List<CSData>();
		Transform identity = Transform.Identity;
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.DisplayObjects())
		{
			if (item == null)
			{
				continue;
			}
			for (int i = 0; i < item.OutputChannelCount; i++)
			{
				if (item.GetOutputChannelData(i) == null)
				{
					continue;
				}
				foreach (MaterialClassification searchMaterialClassification in whichMaterialClassifications)
				{
					CSData outputChannelData = item.GetOutputChannelData(i);
					if (!(outputChannelData is CSDataArray cSDataArray))
					{
						outputChannelData.GetAllMaterial(list, identity, item.OutputDisplayMaterialClassification, searchMaterialClassification);
						continue;
					}
					foreach (CSData item2 in (from data in cSDataArray.GetFlattenedList()
						where data != null
						select data).ToList())
					{
						item2.GetAllMaterial(list, identity, item.OutputDisplayMaterialClassification, searchMaterialClassification);
					}
				}
			}
		}
		return list;
	}

	public static void RenumberPatchwireIdentifiers()
	{
		Dictionary<string, int> sectionNameList = new Dictionary<string, int>();
		(from obj in CSApplicationHelper.Application.Document.Graph.AllObjects()
			where obj is CSPatchWireStart
			select obj).ToList().ForEach(delegate(CSObject patchWire)
		{
			string text = patchWire.UserData.FindValue("Identifier", "Original");
			if (string.IsNullOrEmpty(text))
			{
				patchWire.UserData.AddKey("Identifier", "Original", patchWire.Identifier);
				text = patchWire.Identifier;
			}
			if (sectionNameList.ContainsKey(text))
			{
				int num = sectionNameList[text];
				num++;
				sectionNameList[text] = num;
				patchWire.UserData.AddKey("Identifier", "Count", num.ToString());
				if (patchWire.Identifier == text)
				{
					patchWire.Identifier = text + " " + num;
				}
			}
			else
			{
				sectionNameList.Add(text, 1);
				patchWire.UserData.AddKey("Identifier", "Count", "1");
			}
		});
	}
}
