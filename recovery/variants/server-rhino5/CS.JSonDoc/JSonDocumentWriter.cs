using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using CS.CSObjects;
using CS.Iteration;
using CS.MaterialGroup;
using CS.Parametric;
using Newtonsoft.Json.Linq;

namespace CS.JSonDoc;

public static class JSonDocumentWriter
{
	public static bool WriteCSFile(out JObject outObj, CSParaGraph graph, CSUserData documentUserData, DisplayComponents currentDisplayComponents, CSGemInstanceDefinitionTable gemTable, GCIterations documentIterations, List<Guid> fileGuidList, string previewfile, ObservableCollection<CSObject> selection = null)
	{
		CSApplicationHelper.WriteVersionInformation();
		outObj = new JObject();
		Bitmap bitmap = null;
		try
		{
			bitmap = ((!(previewfile != string.Empty)) ? CSUtil.GetCurrentViewportImageBitmap() : ((!File.Exists(previewfile)) ? CSUtil.GetCurrentViewportImageBitmap() : new Bitmap(previewfile)));
		}
		catch (Exception ex)
		{
			CSUtil.PrintLine("Error Capturing Viewport Image: " + ex);
		}
		if (bitmap == null || bitmap.Width < 1)
		{
			CSUtil.PrintLine("Bad Preview Image");
		}
		outObj.Add("PreviewBitmap", CSUtilIO.BitmapToString(bitmap));
		outObj.Add("FileInformation", CSFileInformation(fileGuidList));
		graph.SetIndexesOfGraphItems(activeOnly: true, selection);
		graph.SetParentUserData();
		graph.SetStaticData();
		graph.CategoryTable.SetFileIndexes();
		documentUserData.AddKey("Display", "CurrentComponents", currentDisplayComponents.ToString());
		documentUserData.AddKey("Display", "FreehandComponentsVisible", CSApplicationHelper.Application.IsFreehandPartsVisible.ToString());
		documentIterations.CleanUp();
		outObj.Add("DocumentIterations", documentIterations.WriteJson());
		outObj.Add("MaterialGroups", MaterialGroupHelper.WriteJson());
		outObj.Add("ImportedObjects", graph.WriteImportedObjectJson());
		outObj.Add("DocumentUserData", documentUserData.WriteJSon());
		graph.RemoveDeleted();
		outObj.Add("GemTable", gemTable.WriteJSon());
		outObj.Add("CategoryTable", graph.CategoryTable.WriteJSon());
		outObj.Add("Graph", graph.WriteGraphItemsJSon(CSApplicationHelper.Application.BlockList, activeOnly: true, selection));
		graph.CategoryTable.RemoveIndexes();
		graph.SetIndexesOfGraphItems();
		return true;
	}

	private static JObject CSFileInformation(List<Guid> fileGuidList)
	{
		return new JObject
		{
			{ "Name", "Render" },
			{ "Company", "Gemvision" },
			{
				"Year",
				CSApplicationHelper.Application.BuildDate.Year
			},
			{
				"Month",
				CSApplicationHelper.Application.BuildDate.Month
			},
			{
				"Day",
				CSApplicationHelper.Application.BuildDate.Day
			},
			{
				"Revision",
				CSApplicationHelper.Application.BuildRevision
			},
			{
				"GuidList",
				new JArray(fileGuidList)
			}
		};
	}
}
