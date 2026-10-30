using System;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS;

public sealed class CSUICategoryTable
{
	public Collection<CSUICategory> Categories { get; private set; } = new Collection<CSUICategory>();

	public CSUICategoryTable()
	{
		Reset();
	}

	public int AddCategory(string name, Guid importGroupIdentifier = default(Guid))
	{
		for (int i = 0; i < Categories.Count; i++)
		{
			if (Categories[i].CategoryName == name && Categories[i].ImportGroupIdentifier == importGroupIdentifier)
			{
				return i;
			}
		}
		Categories.Add(new CSUICategory(name, importGroupIdentifier));
		return Categories.Count - 1;
	}

	public int AddCategoryDeletable(string name)
	{
		for (int i = 0; i < Categories.Count; i++)
		{
			if (Categories[i].CategoryName == name)
			{
				return i;
			}
		}
		Categories.Add(new CSUICategory(name, delete: true));
		return Categories.Count - 1;
	}

	public int FindCategory(CSUICategory cat)
	{
		for (int i = 0; i < Categories.Count; i++)
		{
			if (Categories[i] == cat)
			{
				return i;
			}
		}
		return -1;
	}

	public int FindCategoryByName(string name)
	{
		for (int i = 0; i < Categories.Count; i++)
		{
			if (Categories[i].CategoryName.ToLower() == name.ToLower())
			{
				return i;
			}
		}
		return -1;
	}

	public CSUICategory GetCategory(int i)
	{
		if (i < 0)
		{
			return null;
		}
		if (i >= Categories.Count)
		{
			return null;
		}
		return Categories[i];
	}

	public int GetCategoryCount()
	{
		return Categories.Count;
	}

	public int GetCategoryIndex(CSUICategory category)
	{
		return Categories.IndexOf(category);
	}

	public int GetCategoryIndex(string categoryName)
	{
		CSUICategory cSUICategory = Categories.FirstOrDefault((CSUICategory category) => category.CategoryName == categoryName);
		if (cSUICategory != null)
		{
			return GetCategoryIndex(cSUICategory);
		}
		return -1;
	}

	public void MoveCategoryDown(CSUICategory category)
	{
		SetIndexes();
		int num = Categories.IndexOf(category) + 1;
		if (num <= Categories.Count - 1)
		{
			Categories.Remove(category);
			Categories.Insert(num, category);
			SetIndexes();
		}
	}

	public void MoveCategoryUp(CSUICategory category)
	{
		SetIndexes();
		int num = Categories.IndexOf(category) - 1;
		if (num < 0)
		{
			num = 0;
		}
		Categories.Remove(category);
		Categories.Insert(num, category);
		SetIndexes();
	}

	public bool Read(ref OnBinaryArchive archive)
	{
		uint num = TypeCodes.TC_USER | TypeCodes.TC_CRC;
		int num2 = 0;
		int num3 = 0;
		archive.BeginRead3dmChunk(num, ref num2, ref num3);
		string[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		Categories.Clear();
		if (items != null)
		{
			for (int i = 0; i < items.Length; i++)
			{
				if (items[i].Contains("[IMPORTGUID]"))
				{
					string name = items[i].Substring(0, items[i].IndexOf("[IMPORTGUID]"));
					Guid importGroupIdentifier = Guid.Parse(items[i].Substring(items[i].IndexOf("[IMPORTGUID]") + 12));
					Categories.Add(new CSUICategory(name, importGroupIdentifier));
				}
				else
				{
					Categories.Add(new CSUICategory(items[i]));
				}
			}
		}
		archive.EndRead3dmChunk();
		return true;
	}

	public void ReadJson(JArray jStringArray)
	{
		Categories.Clear();
		for (int i = 0; i < jStringArray.Count; i++)
		{
			string text = jStringArray[i].ToString();
			if (text.Contains("[IMPORTGUID]"))
			{
				string name = text.Substring(0, text.IndexOf("[IMPORTGUID]"));
				Guid importGroupIdentifier = Guid.Parse(text.Substring(text.IndexOf("[IMPORTGUID]") + 12));
				Categories.Add(new CSUICategory(name, importGroupIdentifier));
			}
			else
			{
				Categories.Add(new CSUICategory(text));
			}
		}
	}

	public void RemoveIndexes()
	{
		for (int i = 0; i < Categories.Count; i++)
		{
			Categories[i].Index = -1;
		}
	}

	public void Reset()
	{
		Categories.Clear();
	}

	public void SetFileIndexes()
	{
		for (int i = 0; i < Categories.Count; i++)
		{
			Categories[i].fileIndex = i;
		}
	}

	public void SetIndexes()
	{
		for (int i = 0; i < Categories.Count; i++)
		{
			Categories[i].Index = i;
		}
	}

	public void Sort()
	{
		for (int i = 0; i < Categories.Count; i++)
		{
			if (Categories[i].Index == -1)
			{
				Categories[i].Index = i;
			}
		}
		IOrderedEnumerable<CSUICategory> source = Categories.OrderBy((CSUICategory cat) => cat.Index);
		Categories = new Collection<CSUICategory>(source.ToList());
		SetIndexes();
	}

	public bool Write(ref OnBinaryArchive archive)
	{
		archive.BeginWrite3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, 1, 0);
		string[] array = new string[Categories.Count];
		for (int i = 0; i < Categories.Count; i++)
		{
			array[i] = $"{Categories[i].CategoryName}[IMPORTGUID]{Categories[i].ImportGroupIdentifier}";
		}
		CSUtilIO.WriteArray(array, ref archive);
		archive.EndWrite3dmChunk();
		return true;
	}

	public JArray WriteJSon()
	{
		JArray jArray = new JArray();
		foreach (CSUICategory category in Categories)
		{
			jArray.Add($"{category.CategoryName}[IMPORTGUID]{category.ImportGroupIdentifier}");
		}
		return jArray;
	}
}
