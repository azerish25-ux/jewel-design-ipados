using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric.Channels;
using CS.Parametric.Interfaces;
using CSDatas;
using CSGemInstance;
using CSGemInstance.GemTableData;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS;

public sealed class CSGemInstanceDefinitionTable
{
	private readonly List<CSGemInstanceDefinition> _table = new List<CSGemInstanceDefinition>();

	public bool IsUsingLatestForMissingDefinitions { get; set; }

	public bool AddGemToTable(CSGemInstanceDefinition def)
	{
		if (ContainsGem(def.Type, def.Shape, def.VersionMajor, def.VersionMinor, def.GemTableVersion))
		{
			return true;
		}
		_table.Add(def);
		return true;
	}

	public void CreateGem(string filePath)
	{
		CSGemInstanceDefinition cSGemInstanceDefinition = new CSGemInstanceDefinition();
		if (!cSGemInstanceDefinition.CreateGem(filePath))
		{
			CSUtil.PrintLine($"Unable to create gem {filePath}");
		}
		else
		{
			_table.Add(cSGemInstanceDefinition);
		}
	}

	public CSGemInstanceDefinition GetGem(string type, string shape, int verisonMajor = 0, int versionMinor = 0, int gemTableVersion = 0, bool isUsingMajorMinor = true)
	{
		if (!(type == "Diamond") || !(shape == "BaguetteTapered"))
		{
			CSApplicationHelper.Application.Document.GemTable.LoadGemInTable(type, shape);
		}
		CSGemInstanceDefinition cSGemInstanceDefinition = (isUsingMajorMinor ? _table.FirstOrDefault((CSGemInstanceDefinition item) => item.IsGem(type, shape, verisonMajor, versionMinor, gemTableVersion)) : _table.FirstOrDefault((CSGemInstanceDefinition item) => item.IsGem(type, shape, gemTableVersion)));
		if (type == "Diamond" && shape == "BaguetteTapered")
		{
			return cSGemInstanceDefinition ?? CreateTaperedBaguetteCSGemInstanceDefinition();
		}
		if (cSGemInstanceDefinition != null)
		{
			return cSGemInstanceDefinition;
		}
		if (CSApplicationHelper.IsCounterSketchStudio || CSApplicationHelper.IsCounterSketchVerifier)
		{
			IsUsingLatestForMissingDefinitions = true;
		}
		if (IsUsingLatestForMissingDefinitions)
		{
			return GetGem(type, shape, verisonMajor, versionMinor, gemTableVersion, isUsingMajorMinor: false);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine($"This file is missing gem information for {type} {shape} {verisonMajor}.{versionMinor}.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Would you like to attempt to load the latest gem definitions for this session?");
		if (TopMostMessageBox.Show(stringBuilder.ToString(), "Missing Gem Definition", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
		{
			return null;
		}
		IsUsingLatestForMissingDefinitions = true;
		return GetGem(type, shape, verisonMajor, versionMinor, gemTableVersion, isUsingMajorMinor: false);
	}

	public string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (CSGemInstanceDefinition item in _table)
		{
			stringBuilder.AppendLine($"{item.Type} {item.Shape} {item.VersionMajor}.{item.VersionMinor} : {item.GemTableVersion}");
		}
		return stringBuilder.ToString();
	}

	public bool IsInStackObject(CSGemInstanceDefinition def)
	{
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.CSBlocks.OfType<IGemSource>().ToList().OfType<CSBlock>())
		{
			item.Update();
			if (item.OutputChannels.Select((CSOutputChannel csOutputChannel) => CSDataHelper.GetFlattenArray(csOutputChannel.Data)).Any((CSDataArray flatOutputDatas) => (from gem in flatOutputDatas.Array.OfType<CSGemData>()
				where gem.Def != null
				select gem).Any((CSGemData gem) => gem.Def == def)))
			{
				return true;
			}
		}
		return false;
	}

	public bool LoadGemInTable(string type, string shape)
	{
		if (ContainsGem(type, shape))
		{
			return true;
		}
		CSGemInstanceDefinition cSGemInstanceDefinition = new CSGemInstanceDefinition();
		if (!cSGemInstanceDefinition.LoadFromLibrary(type, shape, CSUtil.PrintLine, CSApplicationHelper.Application.Document.IsReloading))
		{
			CSUtil.PrintLine($"Unable to load gem {type} {shape} from library");
			return false;
		}
		_table.Add(cSGemInstanceDefinition);
		if (!ContainsGem(type, shape))
		{
			CSUtil.PrintLine($"Unable to load gem {type} {shape} from library for version {CSGemInstanceDefinitionHelper.CurrentVersionMajor}.{CSGemInstanceDefinitionHelper.CurrentVersionMinor}");
			return false;
		}
		return true;
	}

	public bool Merge(CSGemInstanceDefinitionTable defTable)
	{
		foreach (CSGemInstanceDefinition item in defTable._table)
		{
			AddGemToTable(item);
		}
		return true;
	}

	public void ReadJson(JArray jObjectArr)
	{
		_table.Clear();
		foreach (JToken item in jObjectArr)
		{
			CSGemInstanceDefinition cSGemInstanceDefinition = new CSGemInstanceDefinition();
			if (cSGemInstanceDefinition.ReadJson((JObject)item, CSUtil.PrintLine, CSApplicationHelper.Application.Document.IsReloading))
			{
				_table.Add(cSGemInstanceDefinition);
			}
		}
	}

	public bool Read(ref OnBinaryArchive archive)
	{
		int major_version = 0;
		int minor_version = 0;
		if (!archive.BeginRead3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, ref major_version, ref minor_version))
		{
			CSUtil.PrintLine("Unable to read Gem Table version");
			return false;
		}
		if (major_version != 1 || minor_version != 0)
		{
			CSUtil.PrintLine("Invalid Gem Table version");
			archive.EndRead3dmChunk();
			return false;
		}
		_table.Clear();
		int i = 0;
		if (!archive.ReadInt(ref i))
		{
			CSUtil.PrintLine("Error reading Gem Table count");
			return false;
		}
		for (int j = 0; j < i; j++)
		{
			CSGemInstanceDefinition cSGemInstanceDefinition = new CSGemInstanceDefinition();
			if (cSGemInstanceDefinition.Read(ref archive, CSUtil.PrintLine, CSApplicationHelper.Application.Document.IsReloading))
			{
				_table.Add(cSGemInstanceDefinition);
			}
		}
		if (i != _table.Count)
		{
			CSUtil.PrintLine("Error reading Gem Table items");
		}
		archive.EndRead3dmChunk();
		return true;
	}

	public bool ReloadGems()
	{
		RemovedUnusedGems();
		List<Tuple<string, string>> source = _table.Select((CSGemInstanceDefinition csGemInstanceDefinition) => new Tuple<string, string>(csGemInstanceDefinition.Type, csGemInstanceDefinition.Shape)).ToList();
		_table.Clear();
		return source.Distinct().Aggregate(seed: true, (bool current, Tuple<string, string> gemInfo) => current & LoadGemInTable(gemInfo.Item1, gemInfo.Item2));
	}

	public int RemovedUnusedGems()
	{
		int num = 0;
		for (int num2 = _table.Count - 1; num2 >= 0; num2--)
		{
			try
			{
				if (IsInStackObject(_table[num2]))
				{
					continue;
				}
			}
			catch (Exception ex)
			{
				CSUtil.Print(string.Format("IsInStackObject threw an exception\n\n{0}\n\n{1}\n\n", ex, ex.InnerException?.ToString() ?? "No inner exception"));
			}
			CSUtil.PrintLine($"Removing {_table[num2].Type} {_table[num2].Shape} {_table[num2].VersionMajor}.{_table[num2].VersionMinor}:{_table[num2].GemTableVersion} from Gem Table");
			_table.RemoveAt(num2);
			num++;
		}
		return num;
	}

	public void ReplaceGeneratedGem(CSGemInstanceDefinition gemInstanceDefinition)
	{
		if (_table.RemoveAll((CSGemInstanceDefinition item) => item != null && item.IsGem(gemInstanceDefinition.Type, gemInstanceDefinition.Shape) && item.IsGeneratedGem) > 0)
		{
			_table.Add(gemInstanceDefinition);
		}
	}

	public void Reset()
	{
		IsUsingLatestForMissingDefinitions = false;
		_table.Clear();
	}

	public bool Write(ref OnBinaryArchive archive)
	{
		RemovedUnusedGems();
		archive.BeginWrite3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, 1, 0);
		int i = _table.Count((CSGemInstanceDefinition t) => t.IsValid);
		archive.WriteInt(i);
		foreach (CSGemInstanceDefinition item in _table)
		{
			if (item.IsValid)
			{
				item.Write(ref archive, CSUtil.PrintLine);
				continue;
			}
			CSUtil.PrintLine($"Skipped invalid entry {item.Type} {item.Shape} {item.VersionMajor}.{item.VersionMinor}:{item.GemTableVersion}");
		}
		archive.EndWrite3dmChunk();
		return true;
	}

	public JArray WriteJSon()
	{
		RemovedUnusedGems();
		JArray jArray = new JArray();
		foreach (CSGemInstanceDefinition item in _table)
		{
			if (item.IsValid)
			{
				jArray.Add(item.WriteJson(CSUtil.PrintLine));
				continue;
			}
			CSUtil.PrintLine($"Skipped invalid entry {item.Type} {item.Shape} {item.VersionMajor}.{item.VersionMinor}:{item.GemTableVersion}");
		}
		return jArray;
	}

	public void WriteTableAsIndividualFiles(string directoryPath)
	{
		foreach (CSGemInstanceDefinition item in _table)
		{
			string text = Path.Combine(directoryPath, "Gems", item.Type);
			Directory.CreateDirectory(text);
			OnFileHandle onFileHandle = OnUtil.OpenFile(Path.Combine(text, item.Shape + ".gem"), "wb");
			if (onFileHandle != null)
			{
				OnBinaryArchive archive = new OnBinaryFile(IOn.archive_mode.write3dm, onFileHandle);
				item.Write(ref archive, CSUtil.PrintLine);
			}
			if (onFileHandle != null)
			{
				OnUtil.CloseFile(onFileHandle);
			}
		}
	}

	private bool ContainsGem(string type, string shape)
	{
		return ContainsGem(type, shape, CSGemInstanceDefinitionHelper.CurrentVersionMajor, CSGemInstanceDefinitionHelper.CurrentVersionMinor);
	}

	private bool ContainsGem(string type, string shape, int versionMajor, int versionMinor)
	{
		return _table.Where((CSGemInstanceDefinition item) => item != null).Any((CSGemInstanceDefinition item) => item.IsGem(type, shape, versionMajor, versionMinor));
	}

	private bool ContainsGem(string type, string shape, int versionMajor, int versionMinor, int tableVersion)
	{
		return _table.Where((CSGemInstanceDefinition item) => item != null).Any((CSGemInstanceDefinition item) => item.IsGem(type, shape, versionMajor, versionMinor, tableVersion));
	}

	private static CSGemInstanceDefinition CreateTaperedBaguetteCSGemInstanceDefinition()
	{
		CSGemInstanceDefinition cSGemInstanceDefinition = new CSGemInstanceDefinition(GemTableType.Diamond, GemTableShape.BaguetteTapered);
		CSApplicationHelper.Application.Document.GemTable.AddGemToTable(cSGemInstanceDefinition);
		CSGemData cSGemData = new CSGemData(cSGemInstanceDefinition.GenerateCSGemData(0));
		CSApplicationHelper.Application.Document.ReplaceGeneratedGem(cSGemData.Def);
		return cSGemData.Def;
	}
}
