using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CS.CSObjects.ObsoleteGrouping;
using CS.JSonDoc;
using CS.MaterialGroup;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSMaterials;
using GV.CounterSketch.Shared.Model.Objects;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSObjects;

public abstract class CSObject
{
	public static bool AutoAddSelf = true;

	public static bool UseSecondValues;

	public string ExceptionMessage;

	public bool LoopTested;

	public string Message;

	protected uint BitFlags = 65520u;

	private bool _hasLookedUpImportGroup;

	private string _identifier;

	private double _identifierTextSize;

	private Guid _instanceGuid;

	private MaterialClassification _outputDisplayMaterialClassification;

	private CSTreeObject _parent;

	private CSPatchWireStart _parentPatchWire;

	private Bitmap _superviewImage;

	private uint _unsavedFlags = 4u;

	public CSPatchWireStart ParentPatchWire
	{
		get
		{
			if (_parentPatchWire != null)
			{
				return _parentPatchWire;
			}
			if (this is CSPatchWireStart)
			{
				_parentPatchWire = null;
				return _parentPatchWire;
			}
			for (int num = CSApplicationHelper.Application.Document.Graph.GraphList.IndexOf(this) - 1; num >= 0; num--)
			{
				if (CSApplicationHelper.Application.Document.Graph.GraphList[num] is CSPatchWireStart parentPatchWire)
				{
					_parentPatchWire = parentPatchWire;
					return _parentPatchWire;
				}
			}
			return null;
		}
	}

	public bool IsOutputDisplayMaterialClassificationAssigned
	{
		get
		{
			if (_outputDisplayMaterialClassification != MaterialClassification.Undefined)
			{
				return _outputDisplayMaterialClassification != MaterialClassification.UseOriginal;
			}
			return false;
		}
	}

	public double AvgLastUpdateTime
	{
		get
		{
			if (UpdateCounts != 0)
			{
				return TotalLastUpdateTime / (double)UpdateCounts;
			}
			return 0.0;
		}
	}

	public int XPosWorld
	{
		get
		{
			int num = XPosLocal;
			if (Parent != null)
			{
				num += Parent.XPosWorld;
			}
			return num;
		}
	}

	public int YPosWorld
	{
		get
		{
			int num = YPosLocal;
			if (Parent != null)
			{
				num += Parent.YPosWorld;
			}
			return num;
		}
	}

	public string IdentifierOriginal
	{
		get
		{
			if (UserData == null)
			{
				return _identifier;
			}
			string text = UserData.FindValue("Identifier", "Original");
			if (string.IsNullOrEmpty(text))
			{
				return _identifier;
			}
			return text;
		}
	}

	public virtual int InputChannelCount => 0;

	public virtual int OutputChannelCount => 0;

	public virtual int InputChannelSVDisplayCount => InputChannelCount;

	public virtual int OutputChannelSVDisplayCount => OutputChannelCount;

	public virtual ArrowDir NubInDirection => ArrowDir.Left;

	public virtual ArrowDir NubOutDirection => ArrowDir.Right;

	public virtual Rectangle MainBoxMouseHit => new Rectangle(10, 1, XSize - 20, YSize - 2);

	public int ObjectLastChanged { get; set; }

	public int UpdateMarker { get; set; }

	public Guid InstanceGuid
	{
		get
		{
			return _instanceGuid;
		}
		set
		{
			_instanceGuid = value;
		}
	}

	public CSUserData UserData { get; private set; } = new CSUserData();

	public int TableIndex { get; set; } = -1;

	public CSTreeObject Parent
	{
		get
		{
			return _parent;
		}
		set
		{
			PreChange("Parent");
			_parent = value;
		}
	}

	public MaterialClassification OutputDisplayMaterialClassification
	{
		get
		{
			return _outputDisplayMaterialClassification;
		}
		set
		{
			if (_outputDisplayMaterialClassification != value)
			{
				PreChange("OutputDisplayMaterialClassification");
				_outputDisplayMaterialClassification = value;
				Change();
			}
		}
	}

	public bool UIVisible
	{
		get
		{
			return (BitFlags & 1) == 1;
		}
		set
		{
			if ((!value || (BitFlags & 1) != 1) && (value || (BitFlags & 1) == 1))
			{
				PreChange("UIVisible");
				if (value)
				{
					BitFlags |= 1u;
				}
				else
				{
					BitFlags &= 4294967294u;
				}
				Change();
			}
		}
	}

	public bool Deleted
	{
		get
		{
			return (BitFlags & 2) == 2;
		}
		set
		{
			if ((value && (BitFlags & 2) == 2) || (!value && (BitFlags & 2) != 2))
			{
				return;
			}
			if (this is CSBlock cSBlock && cSBlock.BlankOutBeforeUpdate())
			{
				CSData data = new CSData();
				foreach (CSOutputChannel outputChannel in cSBlock.OutputChannels)
				{
					if (outputChannel.Data != null)
					{
						outputChannel.Data.Dispose();
						outputChannel.SetData(data);
					}
				}
			}
			PreChange("Deleted");
			if (value)
			{
				BitFlags |= 2u;
			}
			else
			{
				BitFlags &= 4294967293u;
			}
			Change();
		}
	}

	public bool InDisplay
	{
		get
		{
			return (BitFlags & 4) == 4;
		}
		set
		{
			if ((!value || (BitFlags & 4) != 4) && (value || (BitFlags & 4) == 4))
			{
				PreChange("InDisplay");
				if (value)
				{
					BitFlags |= 4u;
				}
				else
				{
					BitFlags &= 4294967291u;
				}
				Change();
			}
		}
	}

	public bool DisplayVisible
	{
		get
		{
			return (BitFlags & 8) == 8;
		}
		set
		{
			if ((!value || (BitFlags & 8) != 8) && (value || (BitFlags & 8) == 8))
			{
				PreChange("DisplayVisible");
				if (value)
				{
					BitFlags |= 8u;
				}
				else
				{
					BitFlags &= 4294967287u;
				}
				Change();
			}
		}
	}

	public bool UIGenerate
	{
		get
		{
			return (BitFlags & 0x10) == 16;
		}
		set
		{
			if ((!value || (BitFlags & 0x10) != 16) && (value || (BitFlags & 0x10) == 16))
			{
				PreChange("UIGenerate");
				if (value)
				{
					BitFlags |= 16u;
				}
				else
				{
					BitFlags &= 4294967279u;
				}
				Change();
			}
		}
	}

	public bool NotCreatedYet
	{
		get
		{
			return (BitFlags & 0x20) == 32;
		}
		set
		{
			if ((!value || (BitFlags & 0x20) != 32) && (value || (BitFlags & 0x20) == 32))
			{
				PreChange("NotCreatedYet");
				if (value)
				{
					BitFlags |= 32u;
				}
				else
				{
					BitFlags &= 4294967263u;
				}
				Change();
			}
		}
	}

	public bool ForceVisible
	{
		get
		{
			return (BitFlags & 0x10000) == 65536;
		}
		set
		{
			if ((!value || (BitFlags & 0x10000) != 65536) && (value || (BitFlags & 0x10000) == 65536))
			{
				PreChange("ForceVisible");
				if (value)
				{
					BitFlags |= 65536u;
				}
				else
				{
					BitFlags &= 4294901759u;
				}
				Change();
			}
		}
	}

	public bool ForceHide
	{
		get
		{
			return (BitFlags & 0x20000) == 131072;
		}
		set
		{
			if ((!value || (BitFlags & 0x20000) != 131072) && (value || (BitFlags & 0x20000) == 131072))
			{
				PreChange("ForceHide");
				if (value)
				{
					BitFlags |= 131072u;
				}
				else
				{
					BitFlags &= 4294836223u;
				}
				Change();
			}
		}
	}

	public bool StudioVisible
	{
		get
		{
			return (BitFlags & 0x40000) == 262144;
		}
		set
		{
			if ((!value || (BitFlags & 0x40000) != 262144) && (value || (BitFlags & 0x40000) == 262144))
			{
				PreChange("StudioVisible");
				if (value)
				{
					BitFlags |= 262144u;
				}
				else
				{
					BitFlags &= 4294705151u;
				}
				Change();
			}
		}
	}

	public bool DummmyObject
	{
		get
		{
			return (BitFlags & 0x80000) == 524288;
		}
		set
		{
			if ((!value || (BitFlags & 0x80000) != 524288) && (value || (BitFlags & 0x80000) == 524288))
			{
				PreChange("DummmyObject");
				if (value)
				{
					BitFlags |= 524288u;
				}
				else
				{
					BitFlags &= 4294443007u;
				}
				Change();
			}
		}
	}

	public bool StudioVisibleAdvanced
	{
		get
		{
			return (BitFlags & 0x100000) == 1048576;
		}
		set
		{
			if ((!value || (BitFlags & 0x100000) != 1048576) && (value || (BitFlags & 0x100000) == 1048576))
			{
				PreChange("StudioVisibleAdvanced");
				if (value)
				{
					BitFlags |= 1048576u;
				}
				else
				{
					BitFlags &= 4293918719u;
				}
				Change();
			}
		}
	}

	public bool StudioVisibleDesignOption
	{
		get
		{
			return (BitFlags & 0x200000) == 2097152;
		}
		set
		{
			if ((!value || (BitFlags & 0x200000) != 2097152) && (value || (BitFlags & 0x200000) == 2097152))
			{
				PreChange("StudioVisibleDesignOption");
				if (value)
				{
					BitFlags |= 2097152u;
				}
				else
				{
					BitFlags &= 4292870143u;
				}
				Change();
			}
		}
	}

	public bool TempObject
	{
		get
		{
			return (_unsavedFlags & 1) == 1;
		}
		set
		{
			if (value)
			{
				_unsavedFlags |= 1u;
			}
			else
			{
				_unsavedFlags &= 4294967294u;
			}
		}
	}

	public bool MouseDownObject
	{
		get
		{
			return (_unsavedFlags & 2) == 2;
		}
		set
		{
			if (value)
			{
				_unsavedFlags |= 2u;
			}
			else
			{
				_unsavedFlags &= 4294967293u;
			}
		}
	}

	public virtual bool UIChanged { get; set; }

	public bool Error
	{
		get
		{
			return (_unsavedFlags & 8) == 8;
		}
		set
		{
			if (value)
			{
				_unsavedFlags |= 8u;
			}
			else
			{
				_unsavedFlags &= 4294967287u;
			}
		}
	}

	public bool MissingInput
	{
		get
		{
			return (_unsavedFlags & 0x10) == 16;
		}
		set
		{
			if (value)
			{
				_unsavedFlags |= 16u;
			}
			else
			{
				_unsavedFlags &= 4294967279u;
			}
		}
	}

	public bool ExceptionThrown
	{
		get
		{
			return (_unsavedFlags & 0x20) == 32;
		}
		set
		{
			if (value)
			{
				_unsavedFlags |= 32u;
			}
			else
			{
				_unsavedFlags &= 4294967263u;
			}
		}
	}

	public double LastUpdateTime { get; set; } = -1.0;

	public double TotalLastUpdateTime { get; set; }

	public int UpdateCounts { get; set; }

	public ImportGroup ImportGroup { get; set; }

	public int XPosLocal
	{
		get
		{
			if (UseSecondValues && UserData.FindInt("TreeView", "ScreenX1", out var value))
			{
				return value;
			}
			return UserData.FindInt("TreeView", "ScreenX");
		}
		set
		{
			UserData.AddKey("TreeView", UseSecondValues ? "ScreenX1" : "ScreenX", value.ToString(CultureInfo.InvariantCulture));
			if (UseSecondValues)
			{
				UserData.AddKeyNoReplace("TreeView", "ScreenX", value.ToString(CultureInfo.InvariantCulture));
			}
		}
	}

	public int YPosLocal
	{
		get
		{
			if (UseSecondValues && UserData.FindInt("TreeView", "ScreenY1", out var value))
			{
				return value;
			}
			return UserData.FindInt("TreeView", "ScreenY");
		}
		set
		{
			UserData.AddKey("TreeView", UseSecondValues ? "ScreenY1" : "ScreenY", value.ToString(CultureInfo.InvariantCulture));
			if (UseSecondValues)
			{
				UserData.AddKeyNoReplace("TreeView", "ScreenX", value.ToString(CultureInfo.InvariantCulture));
			}
		}
	}

	public virtual int XSize
	{
		get
		{
			return (int)GetIdentSize() + 38;
		}
		set
		{
			UserData.AddKey("TreeView", UseSecondValues ? "SizeX1" : "SizeX", value.ToString(CultureInfo.InvariantCulture));
			if (UseSecondValues)
			{
				UserData.AddKeyNoReplace("TreeView", "SizeX", value.ToString(CultureInfo.InvariantCulture));
			}
		}
	}

	public virtual int YSize
	{
		get
		{
			return Math.Max(InputChannelCount, OutputChannelCount) * 8 + 14;
		}
		set
		{
			UserData.AddKey("TreeView", UseSecondValues ? "SizeY1" : "SizeY", value.ToString(CultureInfo.InvariantCulture));
			if (UseSecondValues)
			{
				UserData.AddKeyNoReplace("TreeView", "SizeY", value.ToString(CultureInfo.InvariantCulture));
			}
		}
	}

	public string Identifier
	{
		get
		{
			return _identifier;
		}
		set
		{
			if (!(_identifier == value))
			{
				PreChange("Identifier");
				if (!string.IsNullOrEmpty(_identifier))
				{
					UserData.AddKeyNoReplace("Identifier", "Original", _identifier);
				}
				SetIdentifier(value);
			}
		}
	}

	public Bitmap SuperViewImage
	{
		get
		{
			return _superviewImage;
		}
		set
		{
			if (_superviewImage != null)
			{
				_superviewImage.Dispose();
			}
			_superviewImage = value;
		}
	}

	public bool IsSelected { get; set; }

	public bool IsFiltered { get; set; }

	public bool IsScoped { get; set; }

	protected int TempUpdateMarker { get; set; }

	protected CSObject(string identifier)
	{
		NotCreatedYet = false;
		_identifier = identifier;
		_identifierTextSize = double.NaN;
		_instanceGuid = Guid.NewGuid();
		ObjectLastChanged = -1;
		UpdateMarker = -2;
		IsFiltered = true;
		if (AutoAddSelf)
		{
			CSApplicationHelper.Application.Document.Graph.Add(this);
		}
	}

	public bool BeenUpdatedAfterLastUpdate(CSObject obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj.UpdateMarker > UpdateMarker;
	}

	public void Change()
	{
		UIChanged = true;
		ObjectLastChanged = CSApplicationHelper.Application.Document.Graph.UpdateCount;
	}

	public virtual void CleanAfterTableChange(CSParaGraph graph)
	{
	}

	public void ClearLoopSwitch()
	{
		UpdateMarker = TempUpdateMarker;
	}

	public CSObject CreateTempObject()
	{
		int count = CSApplicationHelper.Application.Document.Graph.GraphList.Count;
		Type[] types = new Type[1] { typeof(string) };
		Type type = GetType();
		ConstructorInfo constructor = type.GetConstructor(types);
		if (constructor == null)
		{
			CSUtil.PrintLine($"Missing constructor {GetType().FullName}(string identifier)");
			return null;
		}
		object[] parameters = new object[1] { _identifier };
		CSApplicationHelper.Application.Document.UndoTable.Pause(isPausing: true);
		CSObject cSObject = (CSObject)constructor.Invoke(parameters);
		CSApplicationHelper.Application.Document.UndoTable.Pause(isPausing: false);
		CSApplicationHelper.Application.Document.Graph.GraphList.RemoveAt(count);
		cSObject.ObjectLastChanged = 0;
		cSObject.UpdateMarker = 0;
		if (type == typeof(CSPatchWireStart) && this is CSPatchWireStart cSPatchWireStart)
		{
			((CSPatchWireStart)cSObject).PatchwireIdentifier = cSPatchWireStart.PatchwireIdentifier;
			((CSPatchWireStart)cSObject).PatchwireVersionNumber = cSPatchWireStart.PatchwireVersionNumber;
		}
		return cSObject;
	}

	public void FinishedUI()
	{
		UIChanged = false;
	}

	public virtual bool GetArrowStartObject(int inputIndex, out CSObject connectionArrowObj, out int connectionArrowIndex, out bool isMyParent, out bool isStatic)
	{
		connectionArrowObj = null;
		connectionArrowIndex = -1;
		isMyParent = false;
		isStatic = false;
		return false;
	}

	public double GetIdentSize()
	{
		if (!double.IsNaN(_identifierTextSize))
		{
			return _identifierTextSize;
		}
		_identifierTextSize = ((SizeF)TextRenderer.MeasureText(_identifier, new Font("Microsoft Sans Serif", 8f, FontStyle.Regular))).Width;
		return _identifierTextSize;
	}

	public virtual CSInputChannel GetInputChannel(int i)
	{
		return null;
	}

	public bool GetUIChanged()
	{
		return UIChanged;
	}

	public bool HasThisObjectChangedSinceLastUpdate()
	{
		return UpdateMarker < ObjectLastChanged;
	}

	public bool JustUpdated()
	{
		return UpdateMarker == CSApplicationHelper.Application.Document.Graph.UpdateCount;
	}

	public virtual Point NubInLocation(int index)
	{
		return new Point(XPosLocal + 4, YPosLocal + (YSize - InputChannelCount * 8) / 2 + index * 8 + 5);
	}

	public virtual Rectangle NubInMouseHit(int index)
	{
		return new Rectangle(0, (YSize - InputChannelCount * 8) / 2 + index * 8 + 1, 10, 8);
	}

	public virtual Point NubOutLocation(int index)
	{
		return new Point(XPosLocal + XSize - 4, YPosLocal + (YSize - OutputChannelCount * 8) / 2 + index * 8 + 5);
	}

	public virtual Rectangle NubOutMouseHit(int index)
	{
		return new Rectangle(XSize - 10, (YSize - OutputChannelCount * 8) / 2 + index * 8 + 1, 10, 8);
	}

	public bool PostRead(ref OnBinaryArchive archive)
	{
		return archive.EndRead3dmChunk();
	}

	public bool PostWrite(ref OnBinaryArchive archive)
	{
		return archive.EndWrite3dmChunk();
	}

	public void PreChange([CallerMemberName] string callingMember = "")
	{
		CSApplicationHelper.Application.Document.UndoTable.RecordObjectStateInUndoList(this, callingMember);
	}

	public bool PreRead(ref OnBinaryArchive archive, ref int major, ref int minor)
	{
		return archive.BeginRead3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, ref major, ref minor);
	}

	public bool PreWrite(ref OnBinaryArchive archive)
	{
		MajorMinorVersion(out var majorVersion, out var minorVersion);
		return archive.BeginWrite3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, majorVersion, minorVersion);
	}

	public bool PreWriteJson(ref JObject jObj)
	{
		MajorMinorVersion(out var majorVersion, out var minorVersion);
		jObj.Add("majorVersion", majorVersion);
		jObj.Add("minorVersion", minorVersion);
		return true;
	}

	public virtual bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		bool flag = true;
		flag &= archive.ReadString(ref _identifier);
		flag &= archive.ReadInt(ref BitFlags);
		if (major <= 2 && minor <= 3)
		{
			int num = 0;
			flag &= archive.ReadInt(ref num);
			if (Enum.IsDefined(typeof(LayerNumber), num))
			{
				LayerNumber whichLayer = (LayerNumber)num;
				_outputDisplayMaterialClassification = CS.MaterialGroup.MaterialGroupHelper.GetMaterialClassifictionByLayer(whichLayer);
			}
			else
			{
				_outputDisplayMaterialClassification = MaterialClassification.UseOriginal;
			}
		}
		else
		{
			int num2 = 0;
			flag &= archive.ReadInt(ref num2);
			_outputDisplayMaterialClassification = ((!Enum.IsDefined(typeof(MaterialClassification), num2)) ? MaterialClassification.UseOriginal : ((MaterialClassification)num2));
		}
		flag &= UserData.Read(ref archive);
		if (!string.IsNullOrEmpty(_identifier) && string.IsNullOrEmpty(UserData.FindValue("Identifier", "Original")))
		{
			UserData.AddKey("Identifier", "Original", _identifier);
		}
		if (UserData.FindInt("SuperView", "Parent", out var value))
		{
			if (CSApplicationHelper.Application.ImportOffset > 0)
			{
				value += CSApplicationHelper.Application.ImportOffset;
				UserData.AddKey("SuperView", "Parent", value.ToString(CultureInfo.InvariantCulture));
			}
			Parent = ((value < graph.GraphList.Count) ? (graph.GraphList[value] as CSTreeObject) : null);
		}
		else
		{
			Parent = null;
		}
		string empty = string.Empty;
		if (major >= 2)
		{
			flag &= archive.ReadString(ref empty);
		}
		if (!Guid.TryParse(empty, out _instanceGuid))
		{
			_instanceGuid = Guid.NewGuid();
		}
		return flag;
	}

	public virtual bool ReadJson(JObject jObject, CSParaGraph graph, int major, int minor)
	{
		int value;
		bool result = jObject.ReadJsonValue<string>("Identifier", out _identifier) & jObject.ReadJsonValue<uint>("BitFlags", out BitFlags) & jObject.ReadJsonValue<int>("OutputDisplayMaterialClass", out value);
		_outputDisplayMaterialClassification = ((!Enum.IsDefined(typeof(MaterialClassification), value)) ? MaterialClassification.UseOriginal : ((MaterialClassification)value));
		UserData.ReadJSon(jObject["UserData"]);
		if (!string.IsNullOrEmpty(_identifier) && string.IsNullOrEmpty(UserData.FindValue("Identifier", "Original")))
		{
			UserData.AddKey("Identifier", "Original", _identifier);
		}
		if (UserData.FindInt("SuperView", "Parent", out var value2))
		{
			if (CSApplicationHelper.Application.ImportOffset > 0)
			{
				value2 += CSApplicationHelper.Application.ImportOffset;
				UserData.AddKey("SuperView", "Parent", value2.ToString(CultureInfo.InvariantCulture));
			}
			Parent = ((value2 < graph.GraphList.Count) ? (graph.GraphList[value2] as CSTreeObject) : null);
		}
		else
		{
			Parent = null;
		}
		jObject.ReadJsonValue<string>("InstanceGuid", out var value3);
		if (!Guid.TryParse(value3, out _instanceGuid))
		{
			_instanceGuid = Guid.NewGuid();
		}
		return result;
	}

	public virtual bool SetFileValue(CSObject setObject)
	{
		Change();
		return true;
	}

	public void SetLoopSwitch()
	{
		TempUpdateMarker = UpdateMarker;
		UpdateMarker = CSApplicationHelper.Application.Document.Graph.UpdateCount;
	}

	public virtual bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		Change();
		StudioVisible = setObject.StudioVisible;
		StudioVisibleAdvanced = setObject.StudioVisibleAdvanced;
		StudioVisibleDesignOption = setObject.StudioVisibleDesignOption;
		return true;
	}

	public virtual bool SetUndo(CSObject setObject)
	{
		Change();
		if (setObject == null)
		{
			return false;
		}
		UIChanged = true;
		BitFlags = setObject.BitFlags;
		if (NotCreatedYet)
		{
			BitFlags = setObject.BitFlags | 2;
		}
		else
		{
			_parent = setObject._parent;
			_identifier = setObject._identifier;
			_identifierTextSize = setObject._identifierTextSize;
			BitFlags = setObject.BitFlags;
			XPosLocal = setObject.XPosLocal;
			YPosLocal = setObject.YPosLocal;
			UserData = new CSUserData(setObject.UserData);
			if (_outputDisplayMaterialClassification != MaterialClassification.UseOriginal)
			{
				_outputDisplayMaterialClassification = setObject._outputDisplayMaterialClassification;
			}
		}
		return true;
	}

	public virtual bool Update()
	{
		if (!_hasLookedUpImportGroup)
		{
			ImportGroup = CSUtil.IsInImportGroup(this);
			_hasLookedUpImportGroup = true;
		}
		UpdateMarker = CSApplicationHelper.Application.Document.Graph.UpdateCount;
		return true;
	}

	public virtual bool VerticalTabs()
	{
		return false;
	}

	public virtual bool Write(ref OnBinaryArchive archive)
	{
		return (byte)(1u & (archive.WriteString(_identifier) ? 1u : 0u) & (archive.WriteInt(BitFlags) ? 1u : 0u) & (archive.WriteInt((int)_outputDisplayMaterialClassification) ? 1u : 0u) & (UserData.Write(ref archive) ? 1u : 0u) & (archive.WriteString(InstanceGuid.ToString()) ? 1u : 0u)) != 0;
	}

	public virtual void WriteJson(ref JObject archive)
	{
		archive.Add("Identifier", _identifier);
		archive.Add("BitFlags", BitFlags);
		archive.Add("OutputDisplayMaterialClass", (int)_outputDisplayMaterialClassification);
		archive.Add("UserData", UserData.WriteJSon());
		archive.Add("InstanceGuid", InstanceGuid);
	}

	protected bool IsHiddenBlock(CSObject obj)
	{
		if (!(obj is CSBlockGroupEnd) && !(obj is CSGroupStartObsolete))
		{
			return obj is CSGroupEndObsolete;
		}
		return true;
	}

	protected virtual void MajorMinorVersion(out int majorVersion, out int minorVersion)
	{
		majorVersion = 2;
		minorVersion = 4;
	}

	protected void SetIdentifier(string identifier)
	{
		_identifier = identifier;
		_identifierTextSize = double.NaN;
	}
}
