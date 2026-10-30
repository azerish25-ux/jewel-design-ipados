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

namespace CSBlocks.Core;

[CSBlock("GroupOfGems", "{3194E95E-964B-4C5F-9251-AE802814BD5B}")]
public sealed class CSGroupOfGems : CSBlock, IGemSource
{
	private readonly List<CSGroupOfGemsIdentifier> _idents = new List<CSGroupOfGemsIdentifier>();

	public CSGroupOfGems(string identifier)
		: base(identifier)
	{
		base.IsOutputBlockProvider = true;
	}

	public override bool BlankOutBeforeUpdate()
	{
		return false;
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
		CSGroupOfGemsIdentifier item = new CSGroupOfGemsIdentifier(cSGemInstanceDefinition.Type, cSGemInstanceDefinition.Shape, cSGemInstanceDefinition.VersionMajor, cSGemInstanceDefinition.VersionMinor);
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
		List<CSGroupOfGemsIdentifier> collection = base.UserData.FindSerializedObject<List<CSGroupOfGemsIdentifier>>("CSGroupOfGems", "Idents");
		_idents.AddRange(collection);
		foreach (CSGroupOfGemsIdentifier item in _idents.ToList())
		{
			CSApplicationHelper.Application.Document.GemTable.GetGem(item.Type, item.Shape, item.VersionMajor, item.VersionMinor, item.GemTableSizeVersion);
		}
		return true;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		_idents.Clear();
		List<CSGroupOfGemsIdentifier> list = base.UserData.FindSerializedObject<List<CSGroupOfGemsIdentifier>>("CSGroupOfGems", "Idents");
		bool result;
		if (list != null && list.Any())
		{
			_idents.AddRange(list);
			result = true;
		}
		else
		{
			int i = 0;
			if (!archive.ReadInt(ref i))
			{
				return false;
			}
			for (int j = 0; j < i; j++)
			{
				string str = null;
				int num = 1 & (archive.ReadString(ref str) ? 1 : 0);
				string str2 = null;
				int num2 = num & (archive.ReadString(ref str2) ? 1 : 0);
				int i2 = 0;
				int num3 = num2 & (archive.ReadInt(ref i2) ? 1 : 0);
				int i3 = 0;
				if (((uint)num3 & (archive.ReadInt(ref i3) ? 1u : 0u)) != 0)
				{
					_idents.Add(new CSGroupOfGemsIdentifier(str, str2, i2, i3));
				}
			}
			result = _idents.Count == i;
		}
		foreach (CSGroupOfGemsIdentifier item in _idents.ToList())
		{
			CSApplicationHelper.Application.Document.GemTable.GetGem(item.Type, item.Shape, item.VersionMajor, item.VersionMinor, item.GemTableSizeVersion);
		}
		return result;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (!base.NotCreatedYet && setObject is CSGroupOfGems cSGroupOfGems)
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
		if (!base.NotCreatedYet && setObject is CSGroupOfGems cSGroupOfGems)
		{
			_idents.Clear();
			for (int i = 0; i < cSGroupOfGems._idents.Count; i++)
			{
				if (CSApplicationHelper.Application.Document.GemTable.LoadGemInTable(cSGroupOfGems._idents[i].Type, cSGroupOfGems._idents[i].Shape) || !(cSGroupOfGems._idents[i].Shape != "BaguetteTapered") || !(cSGroupOfGems._idents[i].Type != "Diamond"))
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
			archive.WriteString(ident.Type);
			archive.WriteString(ident.Shape);
			archive.WriteInt(ident.VersionMajor);
			archive.WriteInt(ident.VersionMinor);
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
			CSGroupOfGemsIdentifier item = new CSGroupOfGemsIdentifier(gem.Type, gem.Shape, gem.VersionMajor, gem.VersionMinor);
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
