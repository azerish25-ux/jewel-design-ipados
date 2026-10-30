using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CS.CSObjects.CSBlocks;
using CS.JSonDoc;
using CS.Parametric;
using CS.Utilities.Helpers;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSObjects.CSTreeObjects;

[CSBlock("PatchWireStart", "{CD89B4CE-7079-450b-AB45-BC3E4EC221C1}")]
public sealed class CSPatchWireStart : CSTreeObject
{
	public bool CAMAdjustmentInPatchwire;

	public bool ErrorInPatchwire;

	public bool ExceptionInPatchwire;

	public bool MissingInputInPatchwire;

	public bool ObsoletePatchwire;

	public bool OrphanedPatchwire;

	public double PatchWireUpdateTime;

	private bool _isBlockDeletable = true;

	private Guid _patchwireIdentifier;

	public override int SizeXSmall
	{
		get
		{
			int num = (int)GetIdentSize();
			int inputChannelCount = InputChannelCount;
			int outputChannelCount = OutputChannelCount;
			int num2 = 106 + num;
			if (inputChannelCount * 12 + 56 > num2)
			{
				num2 = inputChannelCount * 12 + 56;
			}
			if (outputChannelCount * 12 + 58 > num2)
			{
				num2 = outputChannelCount * 12 + 58;
			}
			return num2;
		}
	}

	public override int SizeYSmall => 107;

	public override ArrowDir NubInDirection => ArrowDir.Up;

	public override ArrowDir NubOutDirection => ArrowDir.Down;

	public override ArrowDir InternalSocketInDirection => ArrowDir.Down;

	public override ArrowDir InternalSocketOutDirection => ArrowDir.Up;

	public override Rectangle MainBoxMouseHit => new Rectangle(11, 10, XSize - 12, YSize - 20);

	public override Rectangle SizeDragHit
	{
		get
		{
			if (!base.Expanded)
			{
				return Rectangle.Empty;
			}
			return new Rectangle(XSize - 27, YSize - 35, 18, 18);
		}
	}

	public string ControlName => base.UserData.FindValue("UI", "ControlColorBrush");

	public string IconName => base.UserData.FindValue("UI", "IconImagePath");

	public DisplayComponents DisplayComponent { get; set; }

	public Guid PatchwireIdentifier
	{
		get
		{
			return _patchwireIdentifier;
		}
		set
		{
			_patchwireIdentifier = value;
		}
	}

	public int PatchwireVersionNumber { get; set; }

	public bool IsBlockDeletable
	{
		get
		{
			return _isBlockDeletable;
		}
		set
		{
			if (_isBlockDeletable != value)
			{
				_isBlockDeletable = value;
				UIChanged = true;
			}
		}
	}

	public event EventHandler ErrorChanged;

	public CSPatchWireStart(string identifier)
		: base(identifier)
	{
		BitFlags |= 8u;
	}

	public override bool Delete(bool checkOutputs = true)
	{
		if (!FindThisBlockinGraph(out var indexStartBlock, out var indexEndBlock))
		{
			return false;
		}
		if (checkOutputs && !IsBlockDeleteable(indexStartBlock, indexEndBlock))
		{
			return false;
		}
		for (int i = indexStartBlock; i <= indexEndBlock; i++)
		{
			CSApplicationHelper.Application.Document.Graph.GraphList[i].Deleted = true;
			CSUtil.Print("Deleting:" + CSApplicationHelper.Application.Document.Graph.GraphList[i].Identifier + "\n");
		}
		return true;
	}

	public bool DeleteDefaultStyleSheet()
	{
		string text = StyleSheetFolder();
		if (!string.IsNullOrEmpty(text))
		{
			if (!File.Exists(text))
			{
				return false;
			}
			File.Delete(text);
		}
		return false;
	}

	public Force GetForcePatchWire()
	{
		if (base.ForceVisible)
		{
			return Force.Visible;
		}
		if (base.ForceHide)
		{
			return Force.Hidden;
		}
		return Force.None;
	}

	public bool GetPatchWireVisible()
	{
		return base.DisplayVisible;
	}

	public override Point InternalSocketInLocation(int index)
	{
		return new Point(36 + index * 12 + 5, 10);
	}

	public override Point InternalSocketOutLocation(int index)
	{
		return new Point(XSize - 33 - (OutputChannelCount - index) * 12, YSize - 10);
	}

	public bool IsDeletable()
	{
		return IsBlockDeletable;
	}

	public void LoadSettings()
	{
		PatchWireOptions patchWireOptions = new PatchWireOptions();
		int _nValue = 0;
		if (patchWireOptions.GetSetting(PatchwireIdentifier, base.Identifier, Guid.Empty, null, "ExpandWidth", ref _nValue, 0))
		{
			XSize = _nValue;
		}
		if (patchWireOptions.GetSetting(PatchwireIdentifier, base.Identifier, Guid.Empty, null, "ExpandHeight", ref _nValue, 0))
		{
			YSize = _nValue;
		}
		int num = 0;
		foreach (CSObject graph in CSApplicationHelper.Application.Document.Graph.GraphList)
		{
			if (!graph.Deleted && graph is CSPatchWireStart)
			{
				num++;
			}
		}
		base.XPosLocal = 10;
		base.YPosLocal = num * 100;
	}

	public override Point NubInLocation(int index)
	{
		return new Point(base.XPosLocal + 41 + index * 12, base.YPosLocal + 4);
	}

	public override Rectangle NubInMouseHit(int index)
	{
		return new Rectangle(41 + index * 12 - 5, 0, 10, 9);
	}

	public override Point NubOutLocation(int index)
	{
		return new Point(base.XPosLocal + XSize - 33 - (OutputChannelCount - index) * 12, base.YPosLocal + YSize - 4);
	}

	public override Rectangle NubOutMouseHit(int index)
	{
		return new Rectangle(XSize - 33 - (OutputChannelCount - index) * 12 - 5, YSize - 10, 10, 9);
	}

	public void OnErrorChanged(EventArgs e)
	{
		if (this.ErrorChanged != null)
		{
			this.ErrorChanged(this, e);
		}
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		bool flag = base.Read(ref archive, graph, major, minor);
		if (flag && major == 1 && minor == 1)
		{
			int i = 0;
			archive.ReadInt(ref i);
		}
		if (flag)
		{
			DisplayComponent = DisplayComponents.None;
			string text = base.UserData.FindValue("Display", "Component");
			if (text != null)
			{
				DisplayComponent = (DisplayComponents)Enum.Parse(typeof(DisplayComponents), text, ignoreCase: false);
			}
		}
		if (major >= 3 || (major == 2 && minor >= 1))
		{
			string str = string.Empty;
			int i2 = 0;
			flag &= archive.ReadString(ref str);
			flag &= archive.ReadInt(ref i2);
			if (flag)
			{
				Guid.TryParse(str, out _patchwireIdentifier);
				PatchwireVersionNumber = i2;
			}
		}
		GetStaticUserData(graph);
		return flag;
	}

	public bool ReadDefaultStyleSheet()
	{
		string text = PathDefaultStyleSheet();
		if (string.IsNullOrWhiteSpace(text))
		{
			return true;
		}
		return ReadStyleSheet(text, doUndo: false, isDefault: true);
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		int value;
		string value2;
		int value3;
		bool result = base.ReadJson(jBlock, graph, major, minor) & jBlock.ReadJsonValue<int>("PatchwireDisplayComponent", out value) & jBlock.ReadJsonValue<string>("PatchwireIdentifier", out value2) & jBlock.ReadJsonValue<int>("PatchwireVersionNumber", out value3);
		DisplayComponent = (DisplayComponents)value;
		_patchwireIdentifier = new Guid(value2);
		PatchwireVersionNumber = value3;
		GetStaticUserData(graph);
		return result;
	}

	public bool ReadStyleSheet()
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Filter = "CounterSketch Style Sheet(*.csss)|*.csss|All files (*.*)|*.*",
			InitialDirectory = Path.Combine(CSApplicationHelper.Application.CSStyleSheetPath, StyleSheetFolder())
		};
		Directory.CreateDirectory(openFileDialog.InitialDirectory);
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return true;
		}
		return ReadStyleSheet(openFileDialog.FileName, doUndo: true);
	}

	public void SaveDefaultStyleSheet()
	{
		string text = PathDefaultStyleSheet();
		if (!string.IsNullOrEmpty(text))
		{
			SaveStyleSheet(text);
		}
	}

	public void SaveStyleSheet()
	{
		SaveFileDialog saveFileDialog = new SaveFileDialog
		{
			Filter = "CounterSketch Style Sheet (*.csss)|*.csss|All files (*.*)|*.*",
			InitialDirectory = Path.Combine(CSApplicationHelper.Application.CSStyleSheetPath, StyleSheetFolder())
		};
		Directory.CreateDirectory(saveFileDialog.InitialDirectory);
		if (saveFileDialog.ShowDialog() == DialogResult.OK)
		{
			SaveStyleSheet(saveFileDialog.FileName);
		}
	}

	public void SetDummy(bool dummy)
	{
		CSObject[] range = CSUtil.GetRange(this);
		if (range != null)
		{
			CSObject[] array = range;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DummmyObject = dummy;
			}
		}
	}

	public Force SetForceVisibility(Force force)
	{
		CSObject[] range = CSUtil.GetRange(this);
		if (range == null)
		{
			return Force.None;
		}
		if (range.Length < 1)
		{
			return Force.None;
		}
		if (range[0].ForceVisible && force == Force.Visible)
		{
			return Force.Visible;
		}
		if (range[0].ForceHide && force == Force.Hidden)
		{
			return Force.Hidden;
		}
		if (!range[0].ForceHide && !range[0].ForceVisible && force == Force.None)
		{
			return Force.None;
		}
		bool flag = false;
		bool flag2 = force == Force.Hidden;
		if (force == Force.Visible)
		{
			flag = true;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetForceVisibility");
		if (range != null)
		{
			for (int i = 0; i < range.Length; i++)
			{
				range[i].ForceVisible = flag;
				range[i].ForceHide = flag2;
			}
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetForceVisibility");
		CSUtil.Redraw();
		if (flag2)
		{
			return Force.Hidden;
		}
		if (flag)
		{
			return Force.Visible;
		}
		return Force.None;
	}

	public void SetFromUI(string title)
	{
		if (!(base.Identifier == title))
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
			PreChange("SetFromUI");
			if (string.IsNullOrWhiteSpace(base.UserData.FindValue("Identifier", "Original")))
			{
				base.UserData.AddKey("Identifier", "Original", base.Identifier);
			}
			SetIdentifier(title);
			GraphHelper.RenumberPatchwireIdentifiers();
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
	}

	public void SetStyleSheetFolder(string styleSheetFolder)
	{
		if (styleSheetFolder != null)
		{
			base.UserData.AddKey("StyleSheet", "Folder", styleSheetFolder);
		}
	}

	public string StyleSheetFolder()
	{
		string text = base.UserData.FindValue("StyleSheet", "Folder");
		if (text == null)
		{
			return string.Empty;
		}
		return text.TrimStart('\\');
	}

	public void TogglePatchWireVisible(bool isDisplayVisible, bool doRegen = true)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("TogglePatchWireVisible");
		base.DisplayVisible = isDisplayVisible;
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("TogglePatchWireVisible");
		if (doRegen)
		{
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
		}
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.UserData.AddKey("Display", "Component", DisplayComponent.ToString());
		return base.Write(ref archive) & archive.WriteString(PatchwireIdentifier.ToString()) & archive.WriteInt(PatchwireVersionNumber);
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("PatchwireDisplayComponent", (int)DisplayComponent);
		archive.Add("PatchwireIdentifier", PatchwireIdentifier);
		archive.Add("PatchwireVersionNumber", PatchwireVersionNumber);
	}

	private bool FindThisBlockinGraph(out int indexStartBlock, out int indexEndBlock)
	{
		indexEndBlock = -1;
		indexStartBlock = CSApplicationHelper.Application.Document.Graph.GraphList.FindIndex((CSObject check) => Equals(check));
		if (indexStartBlock < 0 || indexStartBlock >= CSApplicationHelper.Application.Document.Graph.GraphList.Count)
		{
			return false;
		}
		indexEndBlock = CSApplicationHelper.Application.Document.Graph.GraphList.Count - 1;
		for (int num = indexStartBlock + 1; num < CSApplicationHelper.Application.Document.Graph.GraphList.Count; num++)
		{
			CSObject cSObject = CSApplicationHelper.Application.Document.Graph.GraphList[num];
			if (cSObject is CSPatchWireStart || cSObject is CSSuperViewGroup)
			{
				indexEndBlock = num - 1;
				break;
			}
		}
		return true;
	}

	private bool IsBlockDeleteable(int indexStartBlock, int indexEndBlock)
	{
		bool result = true;
		for (int i = 0; i < CSApplicationHelper.Application.Document.Graph.GraphList.Count; i++)
		{
			if ((i >= indexStartBlock && i <= indexEndBlock) || CSApplicationHelper.Application.Document.Graph.GraphList[i].Deleted || !(CSApplicationHelper.Application.Document.Graph.GraphList[i] is CSBlock cSBlock))
			{
				continue;
			}
			for (int j = 0; j < cSBlock.InputChannelCount; j++)
			{
				CSBlock inputChannelParent = cSBlock.GetInputChannelParent(j);
				if (inputChannelParent != null)
				{
					int num = CSApplicationHelper.Application.Document.Graph.GraphList.IndexOf(inputChannelParent);
					if (num >= indexStartBlock && num <= indexEndBlock)
					{
						result = false;
					}
				}
			}
		}
		return result;
	}

	private string PathDefaultStyleSheet()
	{
		string text = StyleSheetFolder();
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		return Path.Combine(Path.Combine(CSApplicationHelper.Application.CSStyleSheetPath, text), "default.csss");
	}

	private bool ReadStyleSheet(string filename, bool doUndo, bool isDefault = false)
	{
		CSStyleSheet cSStyleSheet = new CSStyleSheet();
		try
		{
			OnFileHandle onFileHandle = OnUtil.OpenFile(filename, "rb");
			if (onFileHandle != null)
			{
				OnBinaryFile onBinaryFile = new OnBinaryFile(IOn.archive_mode.read3dm, onFileHandle);
				int version = 0;
				string app_name = string.Empty;
				onBinaryFile.Read3dmStartSection(ref version, ref app_name);
				On3dmProperties properties = new On3dmProperties();
				properties.Default();
				onBinaryFile.Read3dmProperties(ref properties);
				On3dmSettings settings = new On3dmSettings();
				settings.Default();
				onBinaryFile.Read3dmSettings(ref settings);
				Guid id = Guid.NewGuid();
				if (!onBinaryFile.ReadUuid(ref id))
				{
					OnUtil.CloseFile(onFileHandle);
					return false;
				}
				if (id.CompareTo(new Guid("{43D1A372-ECE4-488b-8DC9-41A9309A2478}")) != 0)
				{
					CSUtil.PrintLine("Invalid File Format");
					OnUtil.CloseFile(onFileHandle);
					return false;
				}
				OnBinaryArchive onBinaryArchive = onBinaryFile;
				cSStyleSheet.ReadGraphItems(onBinaryArchive, CSApplicationHelper.Application.BlockList);
				uint sizeof_archive = 0u;
				onBinaryArchive.Read3dmEndMark(ref sizeof_archive);
				OnUtil.CloseFile(onFileHandle);
				CSUtil.Redraw();
			}
			CSPatchWireStart patchwireStart = cSStyleSheet.GetPatchwireStart();
			if (patchwireStart != null)
			{
				if (!(patchwireStart.PatchwireIdentifier == PatchwireIdentifier) || patchwireStart.PatchwireVersionNumber != PatchwireVersionNumber)
				{
					return false;
				}
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				if (doUndo)
				{
					CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("ReadStyleSheet");
				}
				bool showRewirePrompt = !isDefault;
				cSStyleSheet.SetStyleSheetToMain(this, showRewirePrompt);
				if (doUndo)
				{
					CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("ReadStyleSheet");
				}
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.Redraw();
			}
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.StackTrace);
		}
		return true;
	}

	private void SaveStyleSheet(string filename)
	{
		CSStyleSheet cSStyleSheet = new CSStyleSheet();
		cSStyleSheet.CreateStyleSheetFromMain(this);
		OnFileHandle onFileHandle = OnUtil.OpenFile(filename, "wb");
		if (onFileHandle != null)
		{
			OnBinaryFile onBinaryFile = new OnBinaryFile(IOn.archive_mode.write3dm, onFileHandle);
			onBinaryFile.Write3dmStartSection(4, "This is not a 3dm");
			On3dmProperties on3dmProperties = new On3dmProperties();
			on3dmProperties.Default();
			onBinaryFile.Write3dmProperties(on3dmProperties);
			On3dmSettings on3dmSettings = new On3dmSettings();
			on3dmSettings.Default();
			onBinaryFile.Write3dmSettings(on3dmSettings);
			Guid id = new Guid("{43D1A372-ECE4-488b-8DC9-41A9309A2478}");
			onBinaryFile.WriteUuid(ref id);
			cSStyleSheet.SetIndexesOfGraphItems();
			OnBinaryArchive archive = onBinaryFile;
			cSStyleSheet.WriteGraphItems(archive, CSApplicationHelper.Application.BlockList);
		}
	}
}
