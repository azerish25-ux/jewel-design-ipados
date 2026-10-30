using System;
using System.Collections.Generic;
using CS.Iteration;
using CS.MaterialGroup;
using CS.Parametric;
using Newtonsoft.Json.Linq;

namespace CS.JSonDoc;

public static class JSonDocumentReader
{
	public static bool ReadCSFile(string json, CSParaGraph graph, CSUserData documentUserData, CSGemInstanceDefinitionTable gemTable, GCIterations documentIterations, IEnumerable<CSBlockDescription> blockList, out List<Guid> fileGuidList)
	{
		JObject jObject = JObject.Parse(json);
		fileGuidList = ReadFileInformation(jObject["FileInformation"]);
		documentIterations.ReadJSon(jObject["DocumentIterations"]);
		documentIterations.CleanUp();
		MaterialGroupHelper.ReadJson(jObject["MaterialGroups"]);
		graph.ReadImportedObjectDataJson(graph.CategoryTable, jObject["ImportedObjects"]);
		documentUserData.ReadJSon(jObject["DocumentUserData"]);
		gemTable.ReadJson((JArray)jObject["GemTable"]);
		graph.CategoryTable.ReadJson((JArray)jObject["CategoryTable"]);
		graph.CategoryTable.SetIndexes();
		return graph.ReadGraphItemsJson((JArray)jObject["Graph"], blockList);
	}

	private static List<Guid> ReadFileInformation(JToken fileInfo)
	{
		List<Guid> list = new List<Guid>();
		foreach (JToken item2 in (JArray)fileInfo["GuidList"])
		{
			Guid item = new Guid(item2.ToString());
			list.Add(item);
		}
		if (list.Count == 0)
		{
			list.Add(CSApplicationHelper.Application.DesignerGuid);
		}
		else if (list[list.Count - 1].CompareTo(CSApplicationHelper.Application.DesignerGuid) != 0)
		{
			list.Add(CSApplicationHelper.Application.DesignerGuid);
		}
		return list;
	}

	public static bool ReadJsonObject<T>(this JObject archive, string key, out T value)
	{
		value = default(T);
		if (archive.TryGetValue(key, out var value2))
		{
			value = value2.ToObject<T>();
			return true;
		}
		return false;
	}

	public static bool ReadJsonValue<T>(this JObject archive, string key, out T value)
	{
		value = default(T);
		if (archive.TryGetValue(key, out var value2))
		{
			value = value2.Value<T>();
			return true;
		}
		return false;
	}
}
