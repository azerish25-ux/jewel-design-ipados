using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CS.CSObjects.CSBlocks;
using CS.Parametric.Channels;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues;

public abstract class CSValueBase(string identifier) : CSBlock(identifier), IDisposable
{
	private bool _isUIDisabled;

	public Collection<CSUICategory> Categories { get; } = new Collection<CSUICategory>();

	public bool IsUIDisabled => _isUIDisabled;

	public CSInputChannel InIsUIDisabled { get; set; }

	public void CopyCategoryFromValueBase(IEnumerable<CSUICategory> categoriesToCopy, Guid importGroupIdentifier = default(Guid))
	{
		Categories.Clear();
		foreach (CSUICategory item2 in categoriesToCopy)
		{
			CSUICategory item = new CSUICategory(item2.CategoryName, item2.Deletable)
			{
				fileIndex = item2.fileIndex,
				CategoryName = item2.CategoryName,
				Index = item2.Index,
				ImportGroupIdentifier = importGroupIdentifier
			};
			Categories.Add(item);
		}
	}

	public override void CleanAfterTableChange(CSParaGraph graph)
	{
		base.CleanAfterTableChange(graph);
		for (int i = 0; i < Categories.Count; i++)
		{
			if (graph.CategoryTable.FindCategory(Categories[i]) < 0)
			{
				Categories.Remove(Categories[i]);
				i--;
			}
		}
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		bool result = base.ReadJson(archive, graph, major, minor);
		Categories.Clear();
		if (archive.TryGetValue("CategoryIndex", out var value) && value is JArray)
		{
			JArray jArray = value as JArray;
			if (!CSApplicationHelper.Application.InImport)
			{
				foreach (JToken item in jArray)
				{
					int i = (int)item;
					CSUICategory category = graph.CategoryTable.GetCategory(i);
					if (category != null)
					{
						Categories.Add(category);
					}
				}
			}
			else
			{
				foreach (JToken item2 in jArray)
				{
					int i2 = (int)item2;
					CSUICategory category2 = CSApplicationHelper.Application.ImportCategories.GetCategory(i2);
					if (category2 != null)
					{
						int i3 = CSApplicationHelper.Application.Document.Graph.CategoryTable.AddCategory(category2.CategoryName);
						CSUICategory category3 = CSApplicationHelper.Application.Document.Graph.CategoryTable.GetCategory(i3);
						if (category3 != null)
						{
							Categories.Add(category3);
						}
					}
				}
			}
			return result;
		}
		CSUtil.Print("Error Reading Invalid Lengths : UserInterfaceObject\n");
		return false;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		Categories.Clear();
		int[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		if (items == null)
		{
			CSUtil.Print("Error Reading Invalid Lengths : UserInterfaceObject\n");
			return false;
		}
		if (!CSApplicationHelper.Application.InImport)
		{
			int[] array = items;
			foreach (int i2 in array)
			{
				CSUICategory category = graph.CategoryTable.GetCategory(i2);
				if (category != null)
				{
					Categories.Add(category);
				}
			}
		}
		else
		{
			int[] array = items;
			foreach (int i3 in array)
			{
				CSUICategory category2 = CSApplicationHelper.Application.ImportCategories.GetCategory(i3);
				if (category2 != null)
				{
					int i4 = CSApplicationHelper.Application.Document.Graph.CategoryTable.AddCategory(category2.CategoryName);
					CSUICategory category3 = CSApplicationHelper.Application.Document.Graph.CategoryTable.GetCategory(i4);
					if (category3 != null)
					{
						Categories.Add(category3);
					}
				}
			}
		}
		if ((major == 1 && minor > 0) || (major == 2 && minor < 2))
		{
			int num = 0;
			archive.ReadInt(ref num);
		}
		return true;
	}

	public override bool Update()
	{
		if (base.InputChannels.Any((CSInputChannel inputObject) => inputObject.Identifier == "InIsUIDisabled"))
		{
			InIsUIDisabled?.GetBoolean(ref _isUIDisabled);
		}
		return base.Update();
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		JArray jArray = new JArray();
		foreach (CSUICategory category in Categories)
		{
			if (category.fileIndex >= 0)
			{
				jArray.Add(category.fileIndex);
			}
		}
		archive.Add("CategoryIndex", jArray);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		List<int> list = new List<int>();
		foreach (CSUICategory category in Categories)
		{
			if (category.fileIndex >= 0)
			{
				list.Add(category.fileIndex);
			}
		}
		CSUtilIO.WriteArray(list.ToArray(), ref archive);
		return true;
	}

	public virtual void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
	}
}
