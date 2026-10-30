using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CS.Parametric.Interfaces;
using CSDatas;
using CSGemInstance;
using CSGemInstance.GemTableData;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CSBlocks.Core.Obsolete;

[Obsolete]
[CSBlock("GroupOfGems_V1", "{ADB397E0-30F0-49f4-ADFC-94A2FC9109B6}")]
public sealed class CSGroupOfGems1 : CSBlock, IGemSource
{
	private readonly List<CSGroupOfGemsIdentifier> _idents = new List<CSGroupOfGemsIdentifier>();

	public CSGroupOfGems1(string identifier)
		: base(identifier)
	{
		base.IsOutputBlockProvider = true;
	}

	public void AddGem(GemTableType gemTableType, GemTableShape gemTableShape, bool isGenerated = false)
	{
		if (isGenerated)
		{
			CSGemInstanceDefinition cSGemInstanceDefinition = new CSGemInstanceDefinition(gemTableType, gemTableShape);
			if (CSApplicationHelper.Application.Document.GemTable.AddGemToTable(cSGemInstanceDefinition))
			{
				PreChange("AddGem");
				_idents.Add(new CSGroupOfGemsIdentifier(cSGemInstanceDefinition.Type, cSGemInstanceDefinition.Shape, cSGemInstanceDefinition.VersionMajor, cSGemInstanceDefinition.VersionMinor, GemTableHelper.GetCurrentGemTableVersion(gemTableType, gemTableShape)));
				Change();
			}
			return;
		}
		string descriptionFromValue = GemTableHelper.GetDescriptionFromValue(gemTableType);
		string descriptionFromValue2 = GemTableHelper.GetDescriptionFromValue(gemTableShape);
		if (CSApplicationHelper.Application.Document.GemTable.LoadGemInTable(descriptionFromValue, descriptionFromValue2))
		{
			PreChange("AddGem");
			_idents.Add(new CSGroupOfGemsIdentifier(descriptionFromValue, descriptionFromValue2, CSGemInstanceDefinitionHelper.CurrentVersionMajor, CSGemInstanceDefinitionHelper.CurrentVersionMinor, GemTableHelper.GetCurrentGemTableVersion(descriptionFromValue, descriptionFromValue2)));
			Change();
		}
	}

	public override bool AddObjectFromFile(string fileName)
	{
		CSGemInstanceDefinition cSGemInstanceDefinition = new CSGemInstanceDefinition();
		if (!cSGemInstanceDefinition.LoadFromPath(fileName, CS.CSUtil.PrintLine, CSApplicationHelper.Application.Document.IsReloading))
		{
			return false;
		}
		if (string.IsNullOrEmpty(cSGemInstanceDefinition.Shape))
		{
			return false;
		}
		if (string.IsNullOrEmpty(cSGemInstanceDefinition.Type))
		{
			return false;
		}
		CSGroupOfGemsIdentifier item = new CSGroupOfGemsIdentifier(cSGemInstanceDefinition.Type, cSGemInstanceDefinition.Shape, cSGemInstanceDefinition.VersionMajor, cSGemInstanceDefinition.VersionMinor, cSGemInstanceDefinition.GemTableVersion);
		if (!_idents.Contains(item))
		{
			_idents.Add(item);
		}
		Change();
		return CSApplicationHelper.Application.Document.GemTable.AddGemToTable(cSGemInstanceDefinition);
	}

	public override bool DeleteObject(int index)
	{
		PreChange("DeleteObject");
		_idents.RemoveAt(index);
		Change();
		Update();
		return true;
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		base.ReadJson(archive, graph, major, minor);
		_idents.Clear();
		List<CSGroupOfGemsIdentifier> list = base.UserData.FindSerializedObject<List<CSGroupOfGemsIdentifier>>("CSGroupOfGems", "Idents");
		_idents.AddRange(list);
		foreach (CSGroupOfGemsIdentifier item in list)
		{
			if (item.GemTableSizeVersion > 0)
			{
				_idents.Add(item);
				continue;
			}
			CSGemInstanceDefinition gem = CSApplicationHelper.Application.Document.GemTable.GetGem(item.Type, item.Shape, item.VersionMajor, item.VersionMinor);
			if (gem != null)
			{
				_idents.Add(new CSGroupOfGemsIdentifier(gem.Type, gem.Shape, gem.VersionMajor, gem.VersionMinor, gem.GemTableVersion));
			}
			else
			{
				_idents.Add(new CSGroupOfGemsIdentifier(item.Type, item.Shape, CSGemInstanceDefinitionHelper.CurrentVersionMajor, CSGemInstanceDefinitionHelper.CurrentVersionMinor, 1));
			}
		}
		return true;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		_idents.Clear();
		List<CSGroupOfGemsIdentifier> list = base.UserData.FindSerializedObject<List<CSGroupOfGemsIdentifier>>("CSGroupOfGems", "Idents");
		if (list != null && list.Any())
		{
			foreach (CSGroupOfGemsIdentifier item in list)
			{
				if (item.GemTableSizeVersion > 0)
				{
					_idents.Add(item);
					continue;
				}
				CSGemInstanceDefinition gem = CSApplicationHelper.Application.Document.GemTable.GetGem(item.Type, item.Shape, item.VersionMajor, item.VersionMinor);
				if (gem != null)
				{
					_idents.Add(new CSGroupOfGemsIdentifier(gem.Type, gem.Shape, gem.VersionMajor, gem.VersionMinor, gem.GemTableVersion));
				}
				else
				{
					_idents.Add(new CSGroupOfGemsIdentifier(item.Type, item.Shape, CSGemInstanceDefinitionHelper.CurrentVersionMajor, CSGemInstanceDefinitionHelper.CurrentVersionMinor, 1));
				}
			}
		}
		else
		{
			string[] items = null;
			string[] items2 = null;
			CSUtilIO.ReadArray(ref items, ref archive);
			CSUtilIO.ReadArray(ref items2, ref archive);
			for (int i = 0; i < Math.Min(items.Length, items2.Length); i++)
			{
				CSGemInstanceDefinition gem2 = CSApplicationHelper.Application.Document.GemTable.GetGem(items2[i], items[i], 0, 0, 0, isUsingMajorMinor: false);
				_idents.Add((gem2 != null) ? new CSGroupOfGemsIdentifier(items2[i], items[i], gem2.VersionMajor, gem2.VersionMinor, gem2.GemTableVersion) : new CSGroupOfGemsIdentifier(items2[1], items[i], CSGemInstanceDefinitionHelper.CurrentVersionMajor, CSGemInstanceDefinitionHelper.CurrentVersionMinor, 1));
			}
		}
		foreach (CSGroupOfGemsIdentifier item2 in _idents.ToList())
		{
			CSApplicationHelper.Application.Document.GemTable.GetGem(item2.Type, item2.Shape, item2.VersionMajor, item2.VersionMinor, item2.GemTableSizeVersion);
		}
		return true;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (!base.NotCreatedYet && setObject is CSGroupOfGems1 cSGroupOfGems)
		{
			_idents.Clear();
			for (int i = 0; i < cSGroupOfGems._idents.Count; i++)
			{
				if (CSApplicationHelper.Application.Document.GemTable.LoadGemInTable(cSGroupOfGems._idents[i].Type, cSGroupOfGems._idents[i].Shape))
				{
					_idents.Add(cSGroupOfGems._idents[i]);
				}
			}
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && setObject is CSGroupOfGems1 cSGroupOfGems)
		{
			_idents.Clear();
			for (int i = 0; i < cSGroupOfGems._idents.Count; i++)
			{
				if (CSApplicationHelper.Application.Document.GemTable.LoadGemInTable(cSGroupOfGems._idents[i].Type, cSGroupOfGems._idents[i].Shape))
				{
					_idents.Add(cSGroupOfGems._idents[i]);
				}
			}
		}
		return true;
	}

	public override bool Update()
	{
		base.OutputChannels.Clear();
		foreach (CSGroupOfGemsIdentifier ident in _idents)
		{
			CSGemInstanceDefinition gem = CSApplicationHelper.Application.Document.GemTable.GetGem(ident.Type, ident.Shape, ident.VersionMajor, ident.VersionMinor, ident.GemTableSizeVersion);
			new CSOutputChannel(ident.ToString(), this, ChannelDataType.Any, ChannelArrayType.Any).SetData(new CSGemData(ident.Type, ident.Shape, gem));
		}
		return base.Update();
	}

	public override void WriteJson(ref JObject archive)
	{
		base.UserData.AddSerializedObject("CSGroupOfGems", "Idents", _idents);
		base.WriteJson(ref archive);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.UserData.AddSerializedObject("CSGroupOfGems", "Idents", _idents);
		base.Write(ref archive);
		archive.WriteInt(_idents.Count);
		foreach (CSGroupOfGemsIdentifier ident in _idents)
		{
			archive.WriteString(ident.Shape);
		}
		archive.WriteInt(_idents.Count);
		foreach (CSGroupOfGemsIdentifier ident2 in _idents)
		{
			archive.WriteString(ident2.Type);
		}
		return true;
	}

	public void ReloadGems()
	{
		List<CSGroupOfGemsIdentifier> list = new List<CSGroupOfGemsIdentifier>(_idents);
		_idents.Clear();
		foreach (CSGroupOfGemsIdentifier item2 in list)
		{
			CSGemInstanceDefinition gem = CSApplicationHelper.Application.Document.GemTable.GetGem(item2.Type, item2.Shape, CSGemInstanceDefinitionHelper.CurrentVersionMajor, CSGemInstanceDefinitionHelper.CurrentVersionMinor);
			if (gem == null)
			{
				if (!_idents.Contains(item2))
				{
					_idents.Add(item2);
				}
				continue;
			}
			CSGroupOfGemsIdentifier item = new CSGroupOfGemsIdentifier(gem.Type, gem.Shape, gem.VersionMajor, gem.VersionMinor, gem.GemTableVersion);
			if (!_idents.Contains(item))
			{
				_idents.Add(item);
			}
		}
	}

	public void ReplaceGem(CSGemInstanceDefinition gemInstanceDefinition)
	{
		if (_idents.RemoveAll((CSGroupOfGemsIdentifier cSGroupOfGemsIdentifier) => cSGroupOfGemsIdentifier.Type == gemInstanceDefinition.Type && cSGroupOfGemsIdentifier.Shape == gemInstanceDefinition.Shape) > 0)
		{
			CSGroupOfGemsIdentifier item = new CSGroupOfGemsIdentifier(gemInstanceDefinition.Type, gemInstanceDefinition.Shape, gemInstanceDefinition.VersionMajor, gemInstanceDefinition.VersionMinor, gemInstanceDefinition.GemTableVersion);
			_idents.Add(item);
		}
	}
}
