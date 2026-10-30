using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSUserInterfaceFlags;
using CS.CSValues;
using CS.CSValues.Interfaces;
using CS.Iteration;
using CS.JSonDoc;
using CS.MaterialGroup;
using CS.Parametric;
using CS.Parametric.Interfaces;
using CS.Utilities.Helpers;
using CSDatas;
using CSGemInstance;
using CSMaterials;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Shared.Utilities.Helpers;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;

namespace CS;

public sealed class CSDocument
{
	private const string _extDesignFileGz = ".gz";

	private const string _extDesignFileJson = ".json";

	private const int _majorVersion = 1;

	private const int _minorVersion = 1;

	private const double _quickTime = 100.0;

	private readonly List<Guid> _fileGuidList = new List<Guid>();

	private On3dmProperties _fileProperties = new On3dmProperties();

	private bool _isQuickUpdate;

	private DateTime _updateStartTime;

	public GCIterations DocumentIterations { get; } = new GCIterations();

	public CSParaGraph Graph { get; } = new CSParaGraph();

	public CSUndoTable UndoTable { get; } = new CSUndoTable();

	public CSUserData DocumentUserData { get; set; } = new CSUserData();

	public string SetupTemplateName
	{
		get
		{
			return DocumentUserData.FindValue("UI", "Template");
		}
		set
		{
			if (value != string.Empty)
			{
				DocumentUserData.AddKey("UI", "Template", value);
			}
		}
	}

	public bool DrawingBackfacesEnabled { get; set; }

	public bool DrawingCurvesEnabled { get; set; } = true;

	public bool DrawingObjectsEnabled { get; set; } = true;

	public bool DrawingPointsEnabed { get; set; } = true;

	public bool DrawingStartsEnabed { get; set; } = true;

	public string Filename { get; set; } = string.Empty;

	public CSGemInstanceDefinitionTable GemTable { get; set; } = new CSGemInstanceDefinitionTable();

	public DisplayComponents CurrentDisplayComponents { get; set; } = DisplayComponents.All;

	public bool IsReloading { get; set; }

	public bool IsWireframeQuickUpdateEnabled { get; set; }

	public bool Import(string filePath, bool isFreehandImport = false, bool doShowFontSourceErrors = true)
	{
		CSApplicationHelper.Application.InImport = true;
		CSApplicationHelper.Application.ImportOffset = CSApplicationHelper.Application.Document.Graph.GraphList.Count;
		bool result = false;
		try
		{
			using (OnFileHandle onFileHandle = OnUtil.OpenFile(filePath, "rb"))
			{
				if (onFileHandle != null)
				{
					OnBinaryFile onBinaryFile = new OnBinaryFile(IOn.archive_mode.read3dm, onFileHandle);
					int version = 0;
					string app_name = string.Empty;
					onBinaryFile.Read3dmStartSection(ref version, ref app_name);
					_fileProperties.m_PreviewImage?.Dispose();
					onBinaryFile.Read3dmProperties(ref _fileProperties);
					_fileProperties.m_RevisionHistory.NewRevision();
					_fileProperties.m_RevisionHistory.m_sLastEditedBy = CSApplicationHelper.Application.DesignerGuid.ToString();
					On3dmSettings settings = new On3dmSettings();
					settings.Default();
					onBinaryFile.Read3dmSettings(ref settings);
					Guid id = Guid.NewGuid();
					if (!onBinaryFile.ReadUuid(ref id))
					{
						OnUtil.CloseFile(onFileHandle);
						CSApplicationHelper.Application.InImport = false;
						CSApplicationHelper.Application.ImportOffset = 0;
						return false;
					}
					if (id.CompareTo(new Guid("{B237EE6C-AC7E-4725-86EE-54BCCDA34FEA}")) != 0)
					{
						CSUtil.PrintLine("Invalid File Format");
						OnUtil.CloseFile(onFileHandle);
						CSApplicationHelper.Application.InImport = false;
						CSApplicationHelper.Application.ImportOffset = 0;
						return false;
					}
					OnBinaryArchive archive = onBinaryFile;
					int year = 0;
					int month = 0;
					int day = 0;
					int fileRevision = 0;
					if (ImportReadCSFileInformation(onBinaryFile, ref year, ref month, ref day, ref fileRevision))
					{
						CSUserData cSUserData = new CSUserData();
						cSUserData.Read(ref archive);
						CS.MaterialGroup.MaterialGroupHelper.LoadImportedMaterialGroups(cSUserData);
						CSGemInstanceDefinitionTable cSGemInstanceDefinitionTable = new CSGemInstanceDefinitionTable();
						cSGemInstanceDefinitionTable.Read(ref archive);
						GemTable.Merge(cSGemInstanceDefinitionTable);
						CSUICategoryTable cSUICategoryTable = new CSUICategoryTable();
						cSUICategoryTable.Read(ref archive);
						CSApplicationHelper.Application.ImportCategories = cSUICategoryTable;
						foreach (CSUICategory category in cSUICategoryTable.Categories)
						{
							Graph.CategoryTable.Categories.Add(category);
						}
						Graph.CategoryTable.SetIndexes();
						Graph.ImportReadGraphItems(archive, CSApplicationHelper.Application.BlockList, Path.GetFileNameWithoutExtension(filePath), isFreehandImport, cSUICategoryTable, cSUserData);
						Graph.CategoryTable.RemoveIndexes();
						uint sizeof_archive = 0u;
						archive.Read3dmEndMark(ref sizeof_archive);
						PostProcessFile(doShowFontSourceErrors);
						result = true;
						CSUtil.ForceGraphToRedraw();
					}
					else
					{
						CSUtil.PrintLine("Unable to load file");
					}
				}
				if (onFileHandle != null)
				{
					OnUtil.CloseFile(onFileHandle);
				}
			}
			CSApplicationHelper.Application.InImport = false;
			CSApplicationHelper.Application.ImportOffset = 0;
			return result;
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.StackTrace);
			CSApplicationHelper.Application.InImport = false;
			CSApplicationHelper.Application.ImportOffset = 0;
			return false;
		}
	}

	public bool ImportRepair()
	{
		int num = 0;
		bool result = false;
		List<Guid> list = new List<Guid>();
		foreach (CSObject item in CSApplicationHelper.Application.Document.Graph.AllObjects())
		{
			if (item != null)
			{
				if (!list.Contains(item.InstanceGuid))
				{
					list.Add(item.InstanceGuid);
					continue;
				}
				RhinoApp.WriteLine($"Repairing {item.Identifier}");
				item.InstanceGuid = Guid.NewGuid();
				num++;
			}
		}
		if (num > 0)
		{
			RhinoApp.WriteLine($"{num} Objects repaired");
			result = true;
		}
		return result;
	}

	public bool InterruptUpdate()
	{
		if (!_isQuickUpdate)
		{
			return false;
		}
		return (double)(DateTime.Now - _updateStartTime).Milliseconds > 100.0;
	}

	public bool Read(string filePath)
	{
		if (CSApplicationHelper.Application == null)
		{
			return false;
		}
		string text = (Path.GetExtension(filePath) ?? string.Empty).ToLower();
		if (text != ".json" && text != ".gz")
		{
			return Read(filePath, CSApplicationHelper.Application.BlockList);
		}
		string json;
		if (text == ".json")
		{
			json = File.ReadAllText(filePath);
		}
		else
		{
			using FileStream fileStream = File.OpenRead(filePath);
			byte[] array = new byte[4];
			fileStream.Seek(-4L, SeekOrigin.End);
			fileStream.Read(array, 0, 4);
			int num = BitConverter.ToInt32(array, 0);
			fileStream.Seek(0L, SeekOrigin.Begin);
			using GZipStream gZipStream = new GZipStream(fileStream, CompressionMode.Decompress);
			byte[] array2 = new byte[num];
			gZipStream.Read(array2, 0, num);
			json = Encoding.UTF8.GetString(array2);
		}
		JSonDocumentReader.ReadCSFile(json, Graph, DocumentUserData, GemTable, DocumentIterations, CSApplicationHelper.Application.BlockList, out var fileGuidList);
		_fileGuidList.Clear();
		_fileGuidList.AddRange(fileGuidList);
		PostRead();
		return true;
	}

	public bool ReadCSFileInformation(OnBinaryArchive bfile, ref int year, ref int month, ref int day, ref int fileRevision)
	{
		uint expected_tcode = TypeCodes.TC_USER | TypeCodes.TC_CRC;
		int minor_version = 0;
		int major_version = 0;
		if (bfile.BeginRead3dmChunk(expected_tcode, ref major_version, ref minor_version))
		{
			string str = string.Empty;
			string str2 = string.Empty;
			bfile.ReadString(ref str);
			bfile.ReadString(ref str2);
			bfile.ReadInt(ref year);
			bfile.ReadInt(ref month);
			bfile.ReadInt(ref day);
			bfile.ReadInt(ref fileRevision);
			int i = 0;
			bfile.ReadInt(ref i);
			_fileGuidList.Clear();
			for (int j = 0; j < i; j++)
			{
				Guid id = default(Guid);
				bfile.ReadUuid(ref id);
				_fileGuidList.Add(id);
			}
			if (_fileGuidList.Count == 0)
			{
				_fileGuidList.Add(CSApplicationHelper.Application.DesignerGuid);
			}
			else if (_fileGuidList[_fileGuidList.Count - 1].CompareTo(CSApplicationHelper.Application.DesignerGuid) != 0)
			{
				_fileGuidList.Add(CSApplicationHelper.Application.DesignerGuid);
			}
			bfile.EndRead3dmChunk();
			return true;
		}
		return false;
	}

	public void ReadFileValues(string filename, bool undo)
	{
		CSDocument cSDocument = new CSDocument();
		try
		{
			cSDocument.ReadSimple(filename);
			if (undo)
			{
				UndoTable.BeginUndoRecord("ReadFileValues");
			}
			for (int i = 0; i < CSApplicationHelper.Application.Document.Graph.GraphList.Count && i < cSDocument.Graph.GraphList.Count; i++)
			{
				Graph.GraphList[i].SetFileValue(cSDocument.Graph.GraphList[i]);
			}
			if (undo)
			{
				UndoTable.EndUndoRecord("ReadFileValues");
			}
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
		catch (Exception ex)
		{
			if (undo)
			{
				UndoTable.EndUndoRecord("ReadFileValues");
			}
			CSUtil.Print(ex.StackTrace);
		}
	}

	public void ReloadGems()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Reloading gems will replace the current gem size table with the most current set of sizes.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Please be aware that if any shape/size exclusions have been set, and the table version for that particular shape/size has been changed, that those exclusions will be removed.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Would you like to continue with reloading gems?");
		if (MessageBox.Show(stringBuilder.ToString(), "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
		{
			CSUtil.PrintLine("Running CSReloadGems command...");
			IsReloading = true;
			bool num = GemTable.ReloadGems();
			Graph.ReloadGems();
			CSUtil.ForceGraphToRedraw();
			CSUtil.RedrawUI();
			IsReloading = false;
			string arg = (num ? "Gem table reloaded" : "Gem table failed to reload properly");
			CSUtil.PrintLine($"CSReloadGems command completed.  {arg}, and graph objects updated.");
		}
		else
		{
			CSUtil.PrintLine("Cancelled CSReloadGems command.");
		}
	}

	public static bool RemoveUnusedGeometry()
	{
		bool result = false;
		foreach (CSObject item in CSApplicationHelper.Application.Document.Graph.AllObjects())
		{
			if (item != null && !item.Deleted && item is ICSChooser && (item as ICSChooser).DeleteUnused())
			{
				result = true;
			}
		}
		return result;
	}

	public void ReplaceGeneratedGem(CSGemInstanceDefinition gemInstanceDefinition)
	{
		GemTable.ReplaceGeneratedGem(gemInstanceDefinition);
		Graph.ReplaceGeneratedGem(gemInstanceDefinition);
	}

	public void Reset(bool doRedraw = true)
	{
		GemTable.Reset();
		foreach (CSObject graph in Graph.GraphList)
		{
			(graph as CSBlock)?.OutputChannelDatas.Where((CSData b) => b != null).ToList().ForEach(delegate(CSData b)
			{
				b.Dispose();
			});
			(graph as IDisposable)?.Dispose();
		}
		Graph.GraphList.Clear();
		CS.MaterialGroup.MaterialGroupHelper.ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.Highlighted);
		UndoTable.Reset();
		Graph.CategoryTable.Reset();
		_fileProperties.Default();
		_fileProperties.m_Application.m_application_name = string.Concat("CounterSketch (", CSApplicationHelper.Application.BuildDate, "):", CSApplicationHelper.Application.BuildRevision);
		_fileProperties.m_RevisionHistory.m_sLastEditedBy = CSApplicationHelper.Application.DesignerGuid.ToString();
		_fileProperties.m_RevisionHistory.m_sCreatedBy = CSApplicationHelper.Application.DesignerGuid.ToString();
		DocumentUserData.Clear();
		DocumentIterations.Clear();
		_fileGuidList.Clear();
		_fileGuidList.Add(CSApplicationHelper.Application.DesignerGuid);
		Filename = null;
		CS.MaterialGroup.MaterialGroupHelper.CreateDefaultMaterialGroups();
		Graph.ImportedCSGParts = new List<ImportGroup>();
		if (RhUtil.RhinoApp().ActiveView().ActiveViewport()
			.Name() == "Perspective")
		{
			RhUtil.RhinoApp().ActiveView().ActiveViewport()
				.SetTargetAndCameraLocation(OnUtil.On_origin, new On3dPoint(50.0, -50.0, 50.0));
		}
		CSUtil.Redraw();
		if (doRedraw)
		{
			CSUtil.Redraw();
		}
	}

	public void StartGenerate(bool quickUpdate)
	{
		_updateStartTime = DateTime.Now;
		_isQuickUpdate = quickUpdate;
		if (IsWireframeQuickUpdateEnabled)
		{
			CSApplicationHelper.Application.Conduit._isWireframeModeEnabled = _isQuickUpdate;
		}
	}

	public static bool TestDuplicateGuids()
	{
		List<Guid> list = new List<Guid>();
		foreach (CSObject item in CSApplicationHelper.Application.Document.Graph.AllObjects())
		{
			if (item != null)
			{
				if (list.Contains(item.InstanceGuid))
				{
					return true;
				}
				list.Add(item.InstanceGuid);
			}
		}
		return false;
	}

	public bool Write(string path, ObservableCollection<CSObject> selection = null)
	{
		string text = (Path.GetExtension(path) ?? string.Empty).ToLower();
		if (text != ".json" && text != ".gz")
		{
			return Write(path, string.Empty, selection);
		}
		JSonDocumentWriter.WriteCSFile(out var outObj, Graph, DocumentUserData, CurrentDisplayComponents, GemTable, DocumentIterations, _fileGuidList, null);
		if (text == ".json")
		{
			File.WriteAllText(path, outObj.ToString());
		}
		else
		{
			using FileStream stream = File.Create(path);
			using GZipStream stream2 = new GZipStream(stream, CompressionMode.Compress);
			using MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(outObj.ToString()));
			memoryStream.WriteTo(stream2);
		}
		Filename = path;
		return true;
	}

	public bool Write(string path, string previewfile, ObservableCollection<CSObject> selection = null)
	{
		CSApplicationHelper.WriteVersionInformation();
		OnFileHandle onFileHandle = OnUtil.OpenFile(path, "wb");
		if (onFileHandle == null)
		{
			CSUtil.PrintLine("File is null");
			return false;
		}
		OnBinaryFile onBinaryFile = new OnBinaryFile(IOn.archive_mode.write3dm, onFileHandle);
		onBinaryFile.Write3dmStartSection(5, "CSG_CounterSketch");
		_fileProperties.m_PreviewImage?.Dispose();
		try
		{
			if (previewfile != string.Empty)
			{
				if (File.Exists(previewfile))
				{
					Bitmap source = new Bitmap(previewfile);
					_fileProperties.m_PreviewImage = new OnWindowsBitmap(source);
				}
				else
				{
					_fileProperties.m_PreviewImage = CSUtil.GetCurrentViewportImage();
				}
			}
			else
			{
				_fileProperties.m_PreviewImage = CSUtil.GetCurrentViewportImage();
			}
		}
		catch (Exception ex)
		{
			CSUtil.PrintLine("Error Capturing Viewport Image: " + ex);
		}
		if (_fileProperties.m_PreviewImage == null || _fileProperties.m_PreviewImage.Width() < 1)
		{
			CSUtil.PrintLine("Bad Preview Image");
		}
		onBinaryFile.Write3dmProperties(_fileProperties);
		_fileProperties.m_RevisionHistory.NewRevision();
		On3dmSettings on3dmSettings = new On3dmSettings();
		on3dmSettings.Default();
		onBinaryFile.Write3dmSettings(on3dmSettings);
		Guid id = new Guid("{B237EE6C-AC7E-4725-86EE-54BCCDA34FEA}");
		onBinaryFile.WriteUuid(ref id);
		WriteCSFileInformation(onBinaryFile);
		Graph.SetIndexesOfGraphItems(activeOnly: true, selection);
		Graph.SetParentUserData();
		Graph.SetStaticData();
		Graph.CategoryTable.SetFileIndexes();
		OnBinaryArchive archive = onBinaryFile;
		DocumentUserData.AddKey("Display", "CurrentComponents", CurrentDisplayComponents.ToString());
		DocumentUserData.AddKey("Display", "FreehandComponentsVisible", CSApplicationHelper.Application.IsFreehandPartsVisible.ToString());
		DocumentIterations.CleanUp();
		DocumentIterations.WriteToUserData(DocumentUserData);
		CS.MaterialGroup.MaterialGroupHelper.SaveMaterialGroups(CSApplicationHelper.Application.Document.DocumentUserData);
		Graph.WriteImportedObjectData();
		DocumentUserData.Write(ref archive);
		Graph.RemoveDeleted();
		GemTable.Write(ref archive);
		Graph.CategoryTable.Write(ref archive);
		Graph.WriteGraphItems(archive, CSApplicationHelper.Application.BlockList, activeOnly: true, selection);
		archive.Write3dmEndMark();
		Graph.CategoryTable.RemoveIndexes();
		Graph.SetIndexesOfGraphItems();
		Filename = path;
		OnUtil.CloseFile(onFileHandle);
		return true;
	}

	public bool WriteCSFileInformation(OnBinaryArchive bfile)
	{
		bfile.BeginWrite3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, 1, 1);
		string text = " ";
		string text2 = " ";
		text = "Render";
		text2 = "Gemvision";
		if (text.Length < 1)
		{
			text = " ";
		}
		if (text.Length < 1)
		{
			text2 = " ";
		}
		bfile.WriteString(text);
		bfile.WriteString(text2);
		bfile.WriteInt(CSApplicationHelper.Application.BuildDate.Year);
		bfile.WriteInt(CSApplicationHelper.Application.BuildDate.Month);
		bfile.WriteInt(CSApplicationHelper.Application.BuildDate.Day);
		bfile.WriteInt(CSApplicationHelper.Application.BuildRevision);
		bfile.WriteInt(_fileGuidList.Count);
		for (int i = 0; i < _fileGuidList.Count; i++)
		{
			Guid id = _fileGuidList[i];
			bfile.WriteUuid(ref id);
		}
		bfile.EndWrite3dmChunk();
		return true;
	}

	private static void CheckFontSourceErrors()
	{
		List<GV.CounterSketch.Shared.Font> list = new List<GV.CounterSketch.Shared.Font>();
		List<GV.CounterSketch.Shared.Font> list2 = new List<GV.CounterSketch.Shared.Font>();
		List<int> list3 = new List<int>();
		foreach (IFontSource item in CSApplicationHelper.Application.Document.Graph.CSBlocks.OfType<IFontSource>())
		{
			if (item.MissingRestoreFont.HasValue && !list.Contains(item.MissingRestoreFont.Value))
			{
				list.Add(item.MissingRestoreFont.Value);
			}
			list2.AddRange(item.UnavailableFonts.Except(list2));
			list3.AddRange(item.UnrecognizedFontValues.Except(list3));
		}
		if (list2.Count > 0 || list3.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("One or more problems have been encountered while reading font information from file and attempting to reconcile it with this computer.");
			if (list3.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Unrecognized font values:");
				list3.Sort();
				foreach (int item2 in list3)
				{
					stringBuilder.AppendLine($"  - {item2}");
				}
			}
			if (list2.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Unavailable font names:");
				foreach (string item3 in from _ in list2
					select EnumHelper.GetEnumDescriptionFromValueDefault(_) into _
					orderby _
					select _)
				{
					stringBuilder.AppendLine($"  - {item3}");
				}
			}
			TopMostMessageBox.Show(stringBuilder.ToString(), "Missing Font", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		if (list.Count <= 0)
		{
			return;
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.AppendLine("Unable to the restore all of the previously selected fonts.  The design may have changed from how the last person to save the file intended.");
		stringBuilder2.AppendLine();
		stringBuilder2.AppendLine("Missing font names:");
		foreach (string item4 in from _ in list
			select EnumHelper.GetEnumDescriptionFromValueDefault(_) into _
			orderby _
			select _)
		{
			stringBuilder2.AppendLine($"  - {item4}");
		}
		TopMostMessageBox.Show(stringBuilder2.ToString(), "Missing Font", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
	}

	private static bool ImportReadCSFileInformation(OnBinaryArchive bfile, ref int year, ref int month, ref int day, ref int fileRevision)
	{
		uint expected_tcode = TypeCodes.TC_USER | TypeCodes.TC_CRC;
		int minor_version = 0;
		int major_version = 0;
		if (!bfile.BeginRead3dmChunk(expected_tcode, ref major_version, ref minor_version))
		{
			return false;
		}
		string str = string.Empty;
		string str2 = string.Empty;
		bfile.ReadString(ref str);
		bfile.ReadString(ref str2);
		bfile.ReadInt(ref year);
		bfile.ReadInt(ref month);
		bfile.ReadInt(ref day);
		bfile.ReadInt(ref fileRevision);
		int i = 0;
		bfile.ReadInt(ref i);
		List<Guid> list = new List<Guid>();
		list.Clear();
		for (int j = 0; j < i; j++)
		{
			Guid id = default(Guid);
			bfile.ReadUuid(ref id);
			list.Add(id);
		}
		if (list.Count == 0)
		{
			list.Add(CSApplicationHelper.Application.DesignerGuid);
		}
		else if (list[list.Count - 1].CompareTo(CSApplicationHelper.Application.DesignerGuid) != 0)
		{
			list.Add(CSApplicationHelper.Application.DesignerGuid);
		}
		bfile.EndRead3dmChunk();
		return true;
	}

	private bool PostProcessFile(bool doShowFontSourceErrors = true)
	{
		for (int i = 0; i < Graph.GraphList.Count; i++)
		{
			CSUserInterfaceFlagsBegin obj = Graph.GraphList[i] as CSUserInterfaceFlagsBegin;
			CSUserInterfaceFlagsEnd cSUserInterfaceFlagsEnd = Graph.GraphList[i] as CSUserInterfaceFlagsEnd;
			if (obj != null)
			{
				Graph.GraphList.RemoveAt(i);
				i--;
			}
			else if (cSUserInterfaceFlagsEnd != null)
			{
				Graph.GraphList.RemoveAt(i);
				i--;
			}
		}
		DocumentIterations.FindObjectsFromGuid();
		if (doShowFontSourceErrors)
		{
			CheckFontSourceErrors();
		}
		return true;
	}

	private void PostRead()
	{
		Graph.CategoryTable.RemoveIndexes();
		CurrentDisplayComponents = DisplayComponents.All;
		string text = DocumentUserData.FindValue("Display", "CurrentComponents");
		string value = DocumentUserData.FindValue("Display", "FreehandComponentsVisible");
		if (!string.IsNullOrWhiteSpace(value))
		{
			bool result = true;
			bool.TryParse(value, out result);
			CSApplicationHelper.Application.IsFreehandPartsVisible = result;
		}
		else
		{
			CSApplicationHelper.Application.IsFreehandPartsVisible = true;
		}
		if (text != null)
		{
			CurrentDisplayComponents = (DisplayComponents)Enum.Parse(typeof(DisplayComponents), text, ignoreCase: false);
		}
		PostProcessFile();
		if (CS.MaterialGroup.MaterialGroupHelper.IsGenerateMaterialGroupsFromGraphNeeded)
		{
			Graph.ActiveObjects().OfType<CSBlock>().ToList()
				.ForEach(delegate(CSBlock csBlock)
				{
					CS.MaterialGroup.MaterialGroupHelper.AddBlockToDefaultMaterialGroups(csBlock, redraw: false);
				});
			CS.MaterialGroup.MaterialGroupHelper.IsGenerateMaterialGroupsFromGraphNeeded = false;
		}
		if (CSMaterials.MaterialGroupHelper.IsVerificationMaterialGroupEnabled)
		{
			CS.MaterialGroup.MaterialGroupHelper.SetBackupMaterialGroups();
			CS.MaterialGroup.MaterialGroupHelper.SetVerificationMaterialsToDefaultMaterialGroups();
		}
		else
		{
			CS.MaterialGroup.MaterialGroupHelper.SetDesignerMaterialsToMaterialGroups();
		}
		CSRemoveLegacyGroups.Remove();
		Graph.GenerateAll();
		IEnumerable<CSUICategory> enumerable = Graph.CategoryTable.Categories.Where((CSUICategory category) => category.ImportGroupIdentifier != Guid.Empty && Graph.ImportedCSGParts.All((ImportGroup importGroup) => importGroup.Identifier != category.ImportGroupIdentifier));
		CSUICategory[] source = (enumerable as CSUICategory[]) ?? enumerable.ToArray();
		if (source.Any())
		{
			source.ToList().ForEach(delegate(CSUICategory category)
			{
				Graph.CategoryTable.Categories.Remove(category);
			});
		}
		Graph.CSBlocks.OfType<CSValueBase>().ToList().ForEach(delegate(CSValueBase csValueBaseObject)
		{
			CSUICategory category = csValueBaseObject.Categories.FirstOrDefault();
			CSUICategory cSUICategory = category;
			if (cSUICategory != null)
			{
				_ = cSUICategory.ImportGroupIdentifier;
				if (true && category.ImportGroupIdentifier != Guid.Empty && !Graph.ImportedCSGParts.Any((ImportGroup group) => group.Identifier == category.ImportGroupIdentifier))
				{
					ImportGroup realImportGroup = Graph.ImportedCSGParts.FirstOrDefault((ImportGroup importGroup) => importGroup.BlockIdentifiers.Contains(csValueBaseObject.InstanceGuid));
					if (realImportGroup != null)
					{
						CSUICategory cSUICategory2 = Graph.CategoryTable.Categories.FirstOrDefault((CSUICategory importCategory) => importCategory.ImportGroupIdentifier == realImportGroup.Identifier && importCategory.CategoryName == category.CategoryName);
						if (cSUICategory2 != null)
						{
							if (csValueBaseObject.Categories != null)
							{
								csValueBaseObject.Categories.Clear();
								csValueBaseObject.Categories.Add(cSUICategory2);
							}
						}
						else
						{
							CSUICategory cSUICategory3 = Graph.CategoryTable.Categories.FirstOrDefault((CSUICategory importCategory) => importCategory.ImportGroupIdentifier == realImportGroup.Identifier);
							if (cSUICategory3 != null)
							{
								csValueBaseObject.Categories.Clear();
								csValueBaseObject.Categories.Add(cSUICategory3);
							}
						}
					}
				}
			}
		});
		GraphHelper.RenumberPatchwireIdentifiers();
	}

	private bool Read(string filePath, IEnumerable<CSBlockDescription> blockList)
	{
		bool result = false;
		try
		{
			Reset(doRedraw: false);
			using (OnFileHandle onFileHandle = OnUtil.OpenFile(filePath, "rb"))
			{
				if (onFileHandle != null)
				{
					OnBinaryFile onBinaryFile = new OnBinaryFile(IOn.archive_mode.read3dm, onFileHandle);
					int version = 0;
					string app_name = string.Empty;
					if (!onBinaryFile.Read3dmStartSection(ref version, ref app_name))
					{
						OnUtil.CloseFile(onFileHandle);
						return false;
					}
					_fileProperties.m_PreviewImage?.Dispose();
					if (!onBinaryFile.Read3dmProperties(ref _fileProperties))
					{
						OnUtil.CloseFile(onFileHandle);
						return false;
					}
					_fileProperties.m_RevisionHistory.NewRevision();
					_fileProperties.m_RevisionHistory.m_sLastEditedBy = CSApplicationHelper.Application.DesignerGuid.ToString();
					On3dmSettings settings = new On3dmSettings();
					settings.Default();
					if (!onBinaryFile.Read3dmSettings(ref settings))
					{
						OnUtil.CloseFile(onFileHandle);
						return false;
					}
					Guid id = Guid.NewGuid();
					if (!onBinaryFile.ReadUuid(ref id))
					{
						OnUtil.CloseFile(onFileHandle);
						return false;
					}
					bool flag = false;
					bool flag2 = false;
					if (id.CompareTo(new Guid("{A31784F3-23DF-4409-B07F-0417DA142798}")) == 0)
					{
						flag = true;
					}
					if (id.CompareTo(new Guid("{DD8ECCF8-9380-46a8-A177-4BCDE4A6DFBD}")) == 0)
					{
						flag = true;
						flag2 = true;
					}
					if (id.CompareTo(new Guid("{847D0609-66E0-4833-8F20-76E61C6A441C}")) == 0)
					{
						flag = true;
						flag2 = true;
					}
					if (id.CompareTo(new Guid("{B237EE6C-AC7E-4725-86EE-54BCCDA34FEA}")) == 0)
					{
						flag = true;
					}
					if (!flag)
					{
						CSUtil.PrintLine("Invalid File Format");
						OnUtil.CloseFile(onFileHandle);
						return false;
					}
					OnBinaryArchive archive = onBinaryFile;
					int year = 0;
					int month = 0;
					int day = 0;
					int fileRevision = 0;
					if (ReadCSFileInformation(onBinaryFile, ref year, ref month, ref day, ref fileRevision))
					{
						DocumentUserData.Read(ref archive);
						GemTable.Read(ref archive);
						if (flag2)
						{
							new CSDisplayGroupTable().Read(ref archive);
						}
						Graph.CategoryTable.Read(ref archive);
						Graph.CategoryTable.SetIndexes();
						if (!Graph.ReadGraphItems(archive, blockList))
						{
							OnUtil.CloseFile(onFileHandle);
							return false;
						}
						uint sizeof_archive = 0u;
						archive.Read3dmEndMark(ref sizeof_archive);
						DocumentIterations.Clear();
						DocumentIterations.ReadFromUserData(DocumentUserData);
						DocumentIterations.CleanUp();
						CS.MaterialGroup.MaterialGroupHelper.LoadMaterialGroups(DocumentUserData);
						Graph.ReadImportedObjectData(Graph.CategoryTable, DocumentUserData);
						PostRead();
						result = true;
					}
					else
					{
						CSUtil.PrintLine("Unable to load file");
					}
				}
				if (onFileHandle != null)
				{
					OnUtil.CloseFile(onFileHandle);
				}
			}
			Filename = filePath;
			return result;
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.StackTrace);
			return false;
		}
	}

	private bool ReadSimple(string filePath)
	{
		bool result = false;
		try
		{
			Reset(doRedraw: false);
			using (OnFileHandle onFileHandle = OnUtil.OpenFile(filePath, "rb"))
			{
				if (onFileHandle != null)
				{
					OnBinaryFile onBinaryFile = new OnBinaryFile(IOn.archive_mode.read3dm, onFileHandle);
					int version = 0;
					string app_name = string.Empty;
					onBinaryFile.Read3dmStartSection(ref version, ref app_name);
					_fileProperties.m_PreviewImage?.Dispose();
					onBinaryFile.Read3dmProperties(ref _fileProperties);
					_fileProperties.m_RevisionHistory.NewRevision();
					_fileProperties.m_RevisionHistory.m_sLastEditedBy = CSApplicationHelper.Application.DesignerGuid.ToString();
					On3dmSettings settings = new On3dmSettings();
					settings.Default();
					onBinaryFile.Read3dmSettings(ref settings);
					Guid id = Guid.NewGuid();
					if (!onBinaryFile.ReadUuid(ref id))
					{
						OnUtil.CloseFile(onFileHandle);
						return false;
					}
					bool flag = false;
					bool flag2 = false;
					if (id.CompareTo(new Guid("{A31784F3-23DF-4409-B07F-0417DA142798}")) == 0)
					{
						flag = true;
					}
					if (id.CompareTo(new Guid("{DD8ECCF8-9380-46a8-A177-4BCDE4A6DFBD}")) == 0)
					{
						flag = true;
						flag2 = true;
					}
					if (id.CompareTo(new Guid("{847D0609-66E0-4833-8F20-76E61C6A441C}")) == 0)
					{
						flag = true;
						flag2 = true;
					}
					if (id.CompareTo(new Guid("{B237EE6C-AC7E-4725-86EE-54BCCDA34FEA}")) == 0)
					{
						flag = true;
					}
					if (!flag)
					{
						CSUtil.PrintLine("Invalid File Format");
						OnUtil.CloseFile(onFileHandle);
						return false;
					}
					OnBinaryArchive archive = onBinaryFile;
					int year = 0;
					int month = 0;
					int day = 0;
					int fileRevision = 0;
					if (ReadCSFileInformation(onBinaryFile, ref year, ref month, ref day, ref fileRevision))
					{
						DocumentUserData.Read(ref archive);
						GemTable.Read(ref archive);
						if (flag2)
						{
							new CSDisplayGroupTable().Read(ref archive);
						}
						Graph.CategoryTable.Read(ref archive);
						Graph.CategoryTable.SetIndexes();
						Graph.ReadGraphItems(archive, CSApplicationHelper.Application.BlockList);
						Graph.CategoryTable.RemoveIndexes();
						CurrentDisplayComponents = DisplayComponents.All;
						string text = DocumentUserData.FindValue("Display", "CurrentComponents");
						if (text != null)
						{
							CurrentDisplayComponents = (DisplayComponents)Enum.Parse(typeof(DisplayComponents), text, ignoreCase: false);
						}
						uint sizeof_archive = 0u;
						archive.Read3dmEndMark(ref sizeof_archive);
						PostProcessFile();
						result = true;
					}
					else
					{
						CSUtil.PrintLine("Unable to load file");
					}
				}
				if (onFileHandle != null)
				{
					OnUtil.CloseFile(onFileHandle);
				}
			}
			Filename = filePath;
			return result;
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.StackTrace);
			return false;
		}
	}
}
