using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using CS.CSDataUtilities;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CS.Parametric.Channels;

public sealed class CSInputChannel : CSChannel, IDisposable
{
	private readonly ChannelFlag _flags;

	private readonly object _defaultValue;

	private GraphicsPath _gpMouseLine;

	public override CSData Data => OutputChannelBlock?.GetOutputChannelData(OutputChannelIndex);

	public bool HasLink
	{
		get
		{
			if (OutputChannelBlock != null)
			{
				return OutputChannelIndex >= 0;
			}
			return false;
		}
	}

	public bool IsNotChecked => (_flags & ChannelFlag.NotLoopChecked) == ChannelFlag.NotLoopChecked;

	public CSBlock OutputChannelBlock { get; set; }

	public int OutputChannelIndex { get; set; } = -1;

	public CSObject OutputSVArrowBlock { get; set; }

	public int OutputSVArrowIndex { get; set; } = -1;

	public CSObject StaticConnectedBlock { get; set; }

	public int StaticConnectedIndex { get; set; } = -1;

	public GraphicsPath GraphicsPathMouseLine
	{
		get
		{
			return _gpMouseLine;
		}
		set
		{
			_gpMouseLine?.Dispose();
			_gpMouseLine = value;
		}
	}

	public override ChannelArrayType ArrayTypeFilter
	{
		get
		{
			return _arrayTypeFilter;
		}
		set
		{
			ChannelArrayType channelArrayType = value;
			if (channelArrayType.HasFlag(ChannelArrayType.Array))
			{
				channelArrayType |= ChannelArrayType.Array;
			}
			_arrayTypeFilter = channelArrayType;
		}
	}

	public override ChannelDataType DataTypeFilter
	{
		get
		{
			return _dataTypeFilter;
		}
		set
		{
			_dataTypeFilter = value;
		}
	}

	public bool InLoop { get; set; }

	private CSData GetFirstData
	{
		get
		{
			if (OutputChannelBlock == null)
			{
				return null;
			}
			CSOutputChannel outputChannel = OutputChannelBlock.GetOutputChannel(OutputChannelIndex);
			if (outputChannel == null)
			{
				return null;
			}
			CSData cSData = outputChannel.Data;
			while (cSData is ICSArray)
			{
				ICSArray iCSArray = cSData as ICSArray;
				cSData = ((iCSArray.GetArrayCount() == 0) ? null : iCSArray.GetArrayItem(0));
			}
			return cSData;
		}
	}

	private CSDataArray GetDataArray
	{
		get
		{
			CSData cSData = OutputChannelBlock?.GetOutputChannel(OutputChannelIndex).Data;
			if (cSData == null)
			{
				return null;
			}
			if (cSData is ICSArray iCSArray)
			{
				return iCSArray.GetFlattenArray();
			}
			CSDataArray cSDataArray = new CSDataArray();
			cSDataArray.Append(cSData);
			return cSDataArray;
		}
	}

	public List<CSGemData> GemList
	{
		get
		{
			CSDataArray getDataArray = GetDataArray;
			if (getDataArray == null)
			{
				return (_defaultValue as List<CSGemData>) ?? new List<CSGemData>();
			}
			List<CSGemData> list = new List<CSGemData>();
			int count = getDataArray.Array.Count;
			for (int i = 0; i < count; i++)
			{
				if (getDataArray.Array[i] is CSGemData item)
				{
					list.Add(item);
				}
			}
			return list;
		}
	}

	public List<int> IntegerList
	{
		get
		{
			CSDataArray getDataArray = GetDataArray;
			if (getDataArray == null)
			{
				return (_defaultValue as List<int>) ?? new List<int>();
			}
			List<int> list = new List<int>();
			int count = getDataArray.Array.Count;
			for (int i = 0; i < count; i++)
			{
				if (getDataArray.Array[i] is ICSInteger iCSInteger)
				{
					list.Add(iCSInteger.GetInteger());
				}
			}
			return list;
		}
	}

	public List<Curve> CurveList
	{
		get
		{
			CSDataArray getDataArray = GetDataArray;
			if (getDataArray == null)
			{
				return (_defaultValue as List<Curve>) ?? new List<Curve>();
			}
			List<Curve> list = new List<Curve>();
			int count = getDataArray.Array.Count;
			for (int i = 0; i < count; i++)
			{
				if (getDataArray.Array[i] is ICSCurve iCSCurve)
				{
					list.Add(iCSCurve.GetCurve());
				}
			}
			return list;
		}
	}

	public List<double> DoubleList
	{
		get
		{
			CSDataArray getDataArray = GetDataArray;
			if (getDataArray == null)
			{
				return (_defaultValue as List<double>) ?? new List<double>();
			}
			List<double> list = new List<double>();
			int count = getDataArray.Array.Count;
			for (int i = 0; i < count; i++)
			{
				if (getDataArray.Array[i] is ICSDouble iCSDouble)
				{
					list.Add(iCSDouble.GetDouble());
				}
			}
			return list;
		}
	}

	public List<bool> BooleanList
	{
		get
		{
			CSDataArray getDataArray = GetDataArray;
			if (getDataArray == null)
			{
				return (_defaultValue as List<bool>) ?? new List<bool>();
			}
			List<bool> list = new List<bool>();
			int count = getDataArray.Array.Count;
			for (int i = 0; i < count; i++)
			{
				if (getDataArray.Array[i] is ICSBoolean iCSBoolean)
				{
					list.Add(iCSBoolean.GetBoolean());
				}
			}
			return list;
		}
	}

	public bool Boolean
	{
		get
		{
			bool flag = false;
			if (_defaultValue is bool)
			{
				flag = (bool)_defaultValue;
			}
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return flag;
			}
			return (getFirstData as ICSBoolean)?.GetBoolean() ?? flag;
		}
	}

	public int Integer
	{
		get
		{
			int num = 0;
			if (_defaultValue is int)
			{
				num = (int)_defaultValue;
			}
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return num;
			}
			return (getFirstData as ICSInteger)?.GetInteger() ?? num;
		}
	}

	public double Double
	{
		get
		{
			double num = 0.0;
			if (_defaultValue is double)
			{
				num = (double)_defaultValue;
			}
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return num;
			}
			return (getFirstData as ICSDouble)?.GetDouble() ?? num;
		}
	}

	public string String
	{
		get
		{
			string text = string.Empty;
			if (_defaultValue is string text2)
			{
				text = text2;
			}
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return text;
			}
			return (getFirstData as ICSString)?.GetString() ?? text;
		}
	}

	public double Percentage => Double / 100.0;

	public Point3d Point3d
	{
		get
		{
			Point3d result = Point3d.Unset;
			if (_defaultValue is Point3d)
			{
				result = (Point3d)_defaultValue;
			}
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return result;
			}
			if (!(getFirstData is ICSPoint iCSPoint))
			{
				return result;
			}
			if (!iCSPoint.GetPoint(out var value))
			{
				return result;
			}
			return value;
		}
	}

	public Transform Transform
	{
		get
		{
			Transform result = Transform.Unset;
			if (_defaultValue is Transform)
			{
				result = (Transform)_defaultValue;
			}
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return result;
			}
			if (!(getFirstData is ICSTransform iCSTransform))
			{
				return result;
			}
			if (!iCSTransform.GetTransform(out var value))
			{
				return result;
			}
			return value;
		}
	}

	public ValueListItem ValueListItem
	{
		get
		{
			int integer = Integer;
			if (!Enum.IsDefined(typeof(ValueListItem), integer))
			{
				return (ValueListItem)_defaultValue;
			}
			return (ValueListItem)integer;
		}
	}

	public Font Font
	{
		get
		{
			int integer = Integer;
			if (!Enum.IsDefined(typeof(Font), integer))
			{
				return (Font)_defaultValue;
			}
			return (Font)integer;
		}
	}

	public HeadStyle HeadStyle
	{
		get
		{
			int integer = Integer;
			if (!Enum.IsDefined(typeof(HeadStyle), integer))
			{
				return (HeadStyle)_defaultValue;
			}
			return (HeadStyle)integer;
		}
	}

	public Curve Curve
	{
		get
		{
			Curve result = _defaultValue as Curve;
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return result;
			}
			if (getFirstData is ICSCurve iCSCurve)
			{
				return iCSCurve.GetCurve();
			}
			return result;
		}
	}

	public NurbsCurve NurbsCurve
	{
		get
		{
			NurbsCurve result = _defaultValue as NurbsCurve;
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return result;
			}
			if (getFirstData is ICSCurve iCSCurve)
			{
				return iCSCurve.GetNurbsCurve();
			}
			return result;
		}
	}

	public Brep Brep
	{
		get
		{
			Brep result = _defaultValue as Brep;
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return result;
			}
			if (getFirstData is ICSBrep iCSBrep)
			{
				return iCSBrep.GetBrep();
			}
			return result;
		}
	}

	public Mesh Mesh
	{
		get
		{
			Mesh result = _defaultValue as Mesh;
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return result;
			}
			if (getFirstData is ICSMesh iCSMesh)
			{
				return iCSMesh.GetMesh();
			}
			return result;
		}
	}

	public CSGemData Gem
	{
		get
		{
			CSGemData cSGemData = _defaultValue as CSGemData;
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return cSGemData;
			}
			return (getFirstData as CSGemData) ?? cSGemData;
		}
	}

	public CSInputChannel(string identifier, CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags = ChannelFlag.None)
		: base(identifier, parent, dataType, arrayType)
	{
		_flags = flags;
		(base.Parent as CSBlock)?.AddInputChannelToParent(this);
		_defaultValue = null;
	}

	private CSInputChannel(string identifier, CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, object defaultValue)
		: base(identifier, parent, dataType, arrayType)
	{
		_flags = flags;
		(base.Parent as CSBlock)?.AddInputChannelToParent(this);
		_defaultValue = defaultValue;
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags = ChannelFlag.None)
		: base(string.Empty, parent, dataType, arrayType)
	{
		_flags = flags;
		(base.Parent as CSBlock)?.AddInputChannelToParent(this);
		_defaultValue = null;
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, List<bool> defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, List<int> defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, List<double> defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, bool defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, int defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, ValueListItem defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, Font defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, HeadStyle defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, double defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, string defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, Point3d defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ChannelFlag flags, Transform defaultValue)
		: this(string.Empty, parent, dataType, arrayType, flags, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, List<bool> defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, List<int> defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, List<double> defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, bool defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, int defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, ValueListItem defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, Font defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, HeadStyle defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, double defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, string defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, Point3d defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType, Transform defaultValue)
		: this(string.Empty, parent, dataType, arrayType, ChannelFlag.NotRequired, defaultValue)
	{
	}

	public CSInputChannel(CSBlock outputChannelBlock, int outputChannelIndex)
	{
		OutputChannelBlock = outputChannelBlock;
		OutputChannelIndex = outputChannelIndex;
	}

	public CSInputChannel(CSInputChannel copy)
		: base(copy)
	{
		_flags = copy._flags;
		OutputChannelBlock = copy.OutputChannelBlock;
		OutputChannelIndex = copy.OutputChannelIndex;
		OutputSVArrowBlock = copy.OutputSVArrowBlock;
		OutputSVArrowIndex = copy.OutputSVArrowIndex;
		StaticConnectedBlock = copy.StaticConnectedBlock;
		StaticConnectedIndex = copy.StaticConnectedIndex;
		SocketStatic = copy.SocketStatic;
		InLoop = copy.InLoop;
	}

	public CSInputChannel()
	{
	}

	public void Dispose()
	{
		if (_gpMouseLine != null)
		{
			_gpMouseLine.Dispose();
			_gpMouseLine = null;
		}
	}

	public void BreakLink(bool isMaintainingUndo = true)
	{
		if (isMaintainingUndo)
		{
			base.Parent.PreChange("BreakLink");
		}
		OutputChannelBlock = null;
		OutputChannelIndex = -1;
		OutputSVArrowBlock = null;
		OutputSVArrowIndex = -1;
		InLoop = false;
		(base.Parent as CSBlockExpandableInput)?.CleanupInputChannels();
		if (isMaintainingUndo)
		{
			base.Parent.Change();
		}
	}

	public bool CheckIfStaticConnectionIsConnectedTo(CSTreeObject parentObj, int parentIndex)
	{
		if (StaticConnectedBlock == null || StaticConnectedIndex == -1)
		{
			return false;
		}
		if (StaticConnectedBlock == parentObj && StaticConnectedIndex == parentIndex)
		{
			return true;
		}
		if (!(StaticConnectedBlock is CSTreeObject cSTreeObject))
		{
			return false;
		}
		return cSTreeObject.GetInputChannel(StaticConnectedIndex).CheckIfStaticConnectionIsConnectedTo(parentObj, parentIndex);
	}

	[Obsolete]
	public void SetLink(CSBlock csBlock, bool isMaintainingUndo = true)
	{
		if (isMaintainingUndo)
		{
			base.Parent.PreChange("SetLink");
		}
		OutputChannelBlock = csBlock;
		OutputChannelIndex = -1;
		if (isMaintainingUndo)
		{
			base.Parent.Change();
		}
	}

	public void SetLink(CSOutputChannel csOutputChannel, bool isMaintainingUndo = true)
	{
		if (csOutputChannel == null)
		{
			BreakLink(isMaintainingUndo);
			return;
		}
		if (isMaintainingUndo)
		{
			base.Parent.PreChange("SetLink");
		}
		OutputChannelBlock = (CSBlock)csOutputChannel.Parent;
		OutputChannelIndex = OutputChannelBlock.OutputChannels.IndexOf(csOutputChannel);
		(base.Parent as CSBlockExpandableInput)?.CreateExpansionInputChannel();
		if (isMaintainingUndo)
		{
			base.Parent.Change();
		}
	}

	public void SetLink(CSBlock parentBlock, int index, bool isMaintainingUndo = true)
	{
		if (parentBlock == null)
		{
			BreakLink(isMaintainingUndo);
			return;
		}
		if (isMaintainingUndo)
		{
			base.Parent.PreChange("SetLink");
		}
		OutputChannelBlock = parentBlock;
		OutputChannelIndex = index;
		(base.Parent as CSBlockExpandableInput)?.CreateExpansionInputChannel();
		if (isMaintainingUndo)
		{
			base.Parent.Change();
		}
	}

	public CSData GetArrayItem(int index)
	{
		if (OutputChannelBlock == null)
		{
			return new CSData();
		}
		return CSDataHelper.GetArrayItem(Data, index);
	}

	public bool GetBoolean(ref bool value)
	{
		if (OutputChannelBlock == null)
		{
			return false;
		}
		return OutputChannelBlock.GetOutputChannelData(OutputChannelIndex)?.GetBoolean(ref value) ?? false;
	}

	public OnBrep GetBrep()
	{
		return (OutputChannelBlock?.GetOutputChannelData(OutputChannelIndex))?.GetBrep().Convert();
	}

	public bool GetDouble(ref double value)
	{
		return (OutputChannelBlock?.GetOutputChannelData(OutputChannelIndex))?.GetDouble(ref value) ?? false;
	}

	public bool GetDoublePercentage(ref double value)
	{
		double value2 = 0.0;
		if (!GetDouble(ref value2))
		{
			return false;
		}
		value = value2 / 100.0;
		return true;
	}

	public CSDataArray GetFlattenArray()
	{
		if (OutputChannelBlock == null)
		{
			return new CSDataArray();
		}
		return CSDataHelper.GetFlattenArray(Data);
	}

	public bool GetInteger(ref int value)
	{
		return (OutputChannelBlock?.GetOutputChannelData(OutputChannelIndex))?.GetInteger(ref value) ?? false;
	}

	public OnNurbsCurve GetNurbsCurve()
	{
		return (OutputChannelBlock?.GetOutputChannelData(OutputChannelIndex))?.GetNurbsCurve().Convert();
	}

	public OnNurbsSurface GetNurbsSurface()
	{
		return (OutputChannelBlock?.GetOutputChannelData(OutputChannelIndex))?.GetNurbsSurface().Convert();
	}

	public bool GetPoint(ref On3dPoint onPoint)
	{
		if (OutputChannelBlock == null)
		{
			return false;
		}
		if (!(OutputChannelBlock.GetOutputChannelData(OutputChannelIndex) is ICSPoint iCSPoint))
		{
			return false;
		}
		if (!iCSPoint.GetPoint(out var value))
		{
			return false;
		}
		onPoint = value.Convert();
		return true;
	}

	public bool GetString(ref string output)
	{
		return (OutputChannelBlock?.GetOutputChannelData(OutputChannelIndex))?.GetString(ref output) ?? false;
	}

	public bool GetValueListItem(ref ValueListItem whichItem)
	{
		int value = 0;
		if (!GetInteger(ref value))
		{
			return false;
		}
		if (!Enum.IsDefined(typeof(ValueListItem), value))
		{
			return false;
		}
		whichItem = (ValueListItem)value;
		return true;
	}

	public bool GetValueFont(ref Font whichFont)
	{
		int value = 0;
		if (!GetInteger(ref value))
		{
			return false;
		}
		if (!Enum.IsDefined(typeof(Font), value))
		{
			return false;
		}
		whichFont = (Font)value;
		return true;
	}

	public bool GetValueHeadStyle(ref HeadStyle whichHeadStyle)
	{
		int value = 0;
		if (!GetInteger(ref value))
		{
			return false;
		}
		if (!Enum.IsDefined(typeof(HeadStyle), value))
		{
			return false;
		}
		whichHeadStyle = (HeadStyle)value;
		return true;
	}

	public bool GetVector(ref On3dVector onVector)
	{
		if (OutputChannelBlock == null)
		{
			return false;
		}
		if (!(OutputChannelBlock.GetOutputChannelData(OutputChannelIndex) is ICSVector iCSVector))
		{
			return false;
		}
		if (!iCSVector.GetVector(out var value))
		{
			return false;
		}
		onVector = value.Convert();
		return true;
	}

	private bool CheckTypes(ChannelDataType cdt, out CSData localData)
	{
		localData = null;
		if ((DataTypeFilter | cdt) != cdt && DataTypeFilter != ChannelDataType.Any)
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(string.Concat(base.Parent.Identifier, " : ", Identifier, " : Requested ", cdt, " , defined as ", DataTypeFilter));
			return false;
		}
		if (ArrayTypeFilter != ChannelArrayType.Individual)
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Requested Individual Item but defined as " + ArrayTypeFilter);
			return false;
		}
		localData = GetFirstData;
		if (localData == null && (_flags & ChannelFlag.NotRequired) != ChannelFlag.NotRequired)
		{
			base.Parent.MissingInput = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Missing Required Input");
			return false;
		}
		return true;
	}

	private bool CheckArrayTypes(ChannelDataType cdt, out CSDataArray localData)
	{
		localData = null;
		if ((DataTypeFilter & cdt) != cdt && DataTypeFilter != ChannelDataType.Any)
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(string.Concat(base.Parent.Identifier, " : ", Identifier, " : Requested ", cdt, " , defined as ", DataTypeFilter));
			return false;
		}
		if (ArrayTypeFilter != ChannelArrayType.Array)
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Requested Array Item but defined as " + ArrayTypeFilter);
			return false;
		}
		localData = GetDataArray;
		if (localData == null && (_flags & ChannelFlag.NotRequired) != ChannelFlag.NotRequired)
		{
			base.Parent.MissingInput = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Missing Required Input");
			return false;
		}
		return true;
	}

	public bool GetValue(out double value)
	{
		value = 0.0;
		if (!CheckTypes(ChannelDataType.Double, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (double)_defaultValue;
			return true;
		}
		if (!(localData is ICSDouble iCSDouble))
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved");
			return false;
		}
		value = iCSDouble.GetDouble();
		return true;
	}

	public bool GetValues(out List<double> value)
	{
		value = null;
		if (!CheckArrayTypes(ChannelDataType.Double, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (List<double>)_defaultValue;
			return true;
		}
		value = new List<double>();
		int count = localData.Array.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(localData.Array[i] is ICSDouble iCSDouble))
			{
				base.Parent.Error = true;
				((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved in array position " + i + " out of " + count);
				return false;
			}
			value.Add(iCSDouble.GetDouble());
		}
		return true;
	}

	public bool GetValue(out int value)
	{
		value = 0;
		if (!CheckTypes(ChannelDataType.Integer, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (int)_defaultValue;
			return true;
		}
		if (!(localData is ICSInteger iCSInteger))
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved");
			return false;
		}
		value = iCSInteger.GetInteger();
		return true;
	}

	public bool GetValues(out List<int> value)
	{
		value = null;
		if (!CheckArrayTypes(ChannelDataType.Integer, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (List<int>)_defaultValue;
			return true;
		}
		value = new List<int>();
		int count = localData.Array.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(localData.Array[i] is ICSInteger iCSInteger))
			{
				base.Parent.Error = true;
				((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved in array position " + i + " out of " + count);
				return false;
			}
			value.Add(iCSInteger.GetInteger());
		}
		return true;
	}

	public bool GetValue(out bool value)
	{
		value = false;
		if (!CheckTypes(ChannelDataType.Boolean, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (bool)_defaultValue;
			return true;
		}
		if (!(localData is ICSBoolean iCSBoolean))
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved");
			return false;
		}
		value = iCSBoolean.GetBoolean();
		return true;
	}

	public bool GetValues(out List<bool> value)
	{
		value = null;
		if (!CheckArrayTypes(ChannelDataType.Boolean, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (List<bool>)_defaultValue;
			return true;
		}
		value = new List<bool>();
		int count = localData.Array.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(localData.Array[i] is ICSBoolean iCSBoolean))
			{
				base.Parent.Error = true;
				((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved in array position " + i + " out of " + count);
				return false;
			}
			value.Add(iCSBoolean.GetBoolean());
		}
		return true;
	}

	public bool GetValue(out Curve value)
	{
		value = null;
		if (!CheckTypes(ChannelDataType.Curve, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (Curve)_defaultValue;
			return true;
		}
		if (!(localData is ICSCurve iCSCurve))
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved");
			return false;
		}
		value = iCSCurve.GetCurve();
		return true;
	}

	public bool GetValues(out List<Curve> value)
	{
		value = null;
		if (!CheckArrayTypes(ChannelDataType.Curve, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (List<Curve>)_defaultValue;
			return true;
		}
		value = new List<Curve>();
		int count = localData.Array.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(localData.Array[i] is ICSCurve iCSCurve))
			{
				base.Parent.Error = true;
				((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved in array position " + i + " out of " + count);
				return false;
			}
			value.Add(iCSCurve.GetCurve());
		}
		return true;
	}

	public bool GetValue(out Mesh value)
	{
		value = null;
		if (!CheckTypes(ChannelDataType.Mesh, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (Mesh)_defaultValue;
			return true;
		}
		if (!(localData is ICSMesh iCSMesh))
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved");
			return false;
		}
		value = iCSMesh.GetMesh();
		return true;
	}

	public bool GetValues(out List<Mesh> value)
	{
		value = null;
		if (!CheckArrayTypes(ChannelDataType.Mesh, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (List<Mesh>)_defaultValue;
			return true;
		}
		value = new List<Mesh>();
		int count = localData.Array.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(localData.Array[i] is ICSMesh iCSMesh))
			{
				base.Parent.Error = true;
				((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved in array position " + i + " out of " + count);
				return false;
			}
			Mesh mesh = iCSMesh.GetMesh();
			if (mesh != null)
			{
				value.Add(mesh);
			}
		}
		return true;
	}

	public bool GetValues(out List<Brep> value)
	{
		value = null;
		if (!CheckArrayTypes(ChannelDataType.Mesh, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (List<Brep>)_defaultValue;
			return true;
		}
		value = new List<Brep>();
		int count = localData.Array.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(localData.Array[i] is ICSBrep iCSBrep))
			{
				base.Parent.Error = true;
				((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved in array position " + i + " out of " + count);
				return false;
			}
			Brep brep = iCSBrep.GetBrep();
			if (brep != null)
			{
				value.Add(brep);
			}
		}
		return true;
	}

	public bool GetValue(out Point3d value)
	{
		value = Point3d.Unset;
		if (!CheckTypes(ChannelDataType.Point, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (Point3d)_defaultValue;
			return true;
		}
		if (!(localData is ICSPoint iCSPoint))
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved");
			return false;
		}
		return iCSPoint.GetPoint(out value);
	}

	public bool GetValue(out Transform value)
	{
		value = Transform.Identity;
		if (!CheckTypes(ChannelDataType.Transform, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (Transform)_defaultValue;
			return true;
		}
		if (!(localData is ICSTransform iCSTransform))
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved");
			return false;
		}
		return iCSTransform.GetTransform(out value);
	}

	public bool GetValue(out CSGemData value)
	{
		value = null;
		if (!CheckTypes(ChannelDataType.Gem, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (CSGemData)_defaultValue;
			return true;
		}
		if (!(localData is CSGemData cSGemData))
		{
			base.Parent.Error = true;
			((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved");
			return false;
		}
		value = cSGemData;
		return true;
	}

	public bool GetValues(out List<CSGemData> value)
	{
		value = null;
		if (!CheckArrayTypes(ChannelDataType.Gem, out var localData))
		{
			return false;
		}
		if (localData == null)
		{
			value = (List<CSGemData>)_defaultValue;
			return true;
		}
		value = new List<CSGemData>();
		int count = localData.Array.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(localData.Array[i] is CSGemData item))
			{
				base.Parent.Error = true;
				((CSBlock)base.Parent).SetMessage(base.Parent.Identifier + " : " + Identifier + " : Invalid DataType Recieved in array position " + i + " out of " + count);
				return false;
			}
			value.Add(item);
		}
		return true;
	}

	public bool CheckInputValid()
	{
		_ = base.Parent.Identifier;
		_ = base.Parent.ParentPatchWire.Identifier;
		_ = Identifier;
		if (ArrayTypeFilter != ChannelArrayType.Individual)
		{
			return true;
		}
		CSData getFirstData = GetFirstData;
		if (getFirstData == null)
		{
			if ((_flags & ChannelFlag.NotRequired) == ChannelFlag.NotRequired)
			{
				return true;
			}
			return false;
		}
		if (DataTypeFilter == ChannelDataType.Any)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Boolean) != 0 && getFirstData is ICSBoolean)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Integer) != 0 && getFirstData is ICSInteger)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Double) != 0 && getFirstData is ICSDouble)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.String) != 0 && getFirstData is ICSString)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Transform) != 0 && getFirstData is ICSTransform)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Point) != 0 && getFirstData is ICSPoint)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Vector) != 0 && getFirstData is ICSVector)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Curve) != 0 && getFirstData is ICSCurve)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Surface) != 0 && getFirstData is ICSSurface)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Brep) != 0 && getFirstData is ICSBrep)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Mesh) != 0 && getFirstData is ICSMesh)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Profile) != 0 && getFirstData is CSProfileData)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.Gem) != 0 && getFirstData is CSGemData)
		{
			return true;
		}
		if ((DataTypeFilter & ChannelDataType.BrepEdge) != 0 && getFirstData is ICSBrepEdge)
		{
			return true;
		}
		if ((_flags & ChannelFlag.NotRequired) == ChannelFlag.NotRequired)
		{
			return true;
		}
		return false;
	}
}
