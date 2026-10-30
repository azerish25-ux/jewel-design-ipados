using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CS.CSObjects.CSBlocks;
using CS.CSValues.CSValueHandle.Interfaces;
using CS.MaterialGroup;
using CS.Parametric.Channels;
using CSMaterials;

namespace CS.CSObjects;

public abstract class CSTreeObject(string identifier) : CSObject(identifier)
{
	private readonly List<CSInputChannel> _inputArrows = new List<CSInputChannel>();

	private readonly List<CSInputChannel> _outputArrows = new List<CSInputChannel>();

	private readonly List<CSObject> _childObjs = new List<CSObject>();

	private bool _mouseOverStart;

	public List<CSObject> Children => new List<CSObject>(_childObjs);

	public override int InputChannelCount => _inputArrows.Count;

	public override int OutputChannelCount => _outputArrows.Count;

	public abstract int SizeXSmall { get; }

	public abstract int SizeYSmall { get; }

	public int SizeXExpanded
	{
		get
		{
			if (CSObject.UseSecondValues && base.UserData.FindInt("TreeView", "SizeX1", out var value))
			{
				return value;
			}
			return base.UserData.FindInt("TreeView", "SizeX");
		}
	}

	public int SizeYExpanded
	{
		get
		{
			if (CSObject.UseSecondValues && base.UserData.FindInt("TreeView", "SizeY1", out var value))
			{
				return value;
			}
			return base.UserData.FindInt("TreeView", "SizeY");
		}
	}

	public int ChildObjectCount
	{
		get
		{
			int num = 0;
			foreach (CSObject childObj in _childObjs)
			{
				if (IsHiddenBlock(childObj))
				{
					num++;
				}
			}
			return _childObjs.Count - num;
		}
	}

	public List<CSObject> GetChildObjectList
	{
		get
		{
			List<CSObject> list = new List<CSObject>();
			list.AddRange(_childObjs.Where((CSObject child) => !IsHiddenBlock(child)));
			return list;
		}
	}

	public abstract ArrowDir InternalSocketInDirection { get; }

	public abstract ArrowDir InternalSocketOutDirection { get; }

	public abstract Rectangle SizeDragHit { get; }

	public int SVLayerIndex
	{
		get
		{
			string text = base.UserData.FindValue("UI", "Layer");
			if (text == null)
			{
				base.UserData.AddKey("UI", "Layer", "0");
				text = "0";
			}
			if (!int.TryParse(text, out var result))
			{
				base.UserData.AddKey("UI", "Layer", "0");
				return 0;
			}
			return result;
		}
		set
		{
			base.UserData.AddKey("UI", "Layer", value.ToString());
		}
	}

	public bool Expanded
	{
		get
		{
			if (CSObject.UseSecondValues && base.UserData.FindBool("TreeView", "Expanded1", out var value))
			{
				return value;
			}
			return base.UserData.FindBool("TreeView", "Expanded");
		}
		set
		{
			base.UserData.AddKey("TreeView", CSObject.UseSecondValues ? "Expanded1" : "Expanded", value.ToString());
		}
	}

	public override int XSize
	{
		get
		{
			if (Expanded)
			{
				return SizeXExpanded;
			}
			return SizeXSmall;
		}
	}

	public override int YSize
	{
		get
		{
			if (Expanded)
			{
				return SizeYExpanded;
			}
			return SizeYSmall;
		}
	}

	public bool MouseOverStart
	{
		get
		{
			return _mouseOverStart;
		}
		set
		{
			bool num = _mouseOverStart != value;
			_mouseOverStart = value;
			if (!num)
			{
				return;
			}
			if (_mouseOverStart)
			{
				if (CSUtil.GetRange(this) != null)
				{
					List<string> identifiers = (from csBlock in CSUtil.GetRange(this).OfType<CSBlock>()
						select csBlock.InstanceGuid.ToString()).ToList();
					CS.MaterialGroup.MaterialGroupHelper.AddRangeToMaterialGroupByClassification(MaterialClassification.Highlighted, identifiers);
				}
			}
			else
			{
				CS.MaterialGroup.MaterialGroupHelper.ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.Highlighted);
			}
		}
	}

	public static void BatchUpdateMouseOver(IEnumerable<CSTreeObject> highlightCsTreeObjects, bool isHighlighted = true)
	{
		CS.MaterialGroup.MaterialGroupHelper.ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.Highlighted);
		List<string> blockIdentifiers = new List<string>();
		if (isHighlighted)
		{
			highlightCsTreeObjects.ToList().ForEach(delegate(CSTreeObject _)
			{
				_._mouseOverStart = true;
				blockIdentifiers.AddRange((from csBlock in CSUtil.GetRange(_).OfType<CSBlock>()
					select csBlock.InstanceGuid.ToString()).ToList());
			});
			CS.MaterialGroup.MaterialGroupHelper.AddRangeToMaterialGroupByClassification(MaterialClassification.Highlighted, blockIdentifiers);
		}
		else
		{
			highlightCsTreeObjects.ToList().ForEach(delegate(CSTreeObject _)
			{
				_._mouseOverStart = false;
			});
		}
	}

	public CSObject Child(int i)
	{
		return _childObjs[i];
	}

	public void ChildAdd(CSObject obj)
	{
		_childObjs.Add(obj);
		obj.Parent = this;
	}

	public void ChildClear()
	{
		_childObjs.Clear();
	}

	public int ChildCount()
	{
		return _childObjs.Count;
	}

	public void ChildRemove(int i)
	{
		_childObjs.RemoveAt(i);
	}

	public abstract bool Delete(bool checkOutputs = true);

	public override bool GetArrowStartObject(int inputIndex, out CSObject connectionArrowObj, out int connectionArrowIndex, out bool isMyParent, out bool isStatic)
	{
		connectionArrowObj = null;
		connectionArrowIndex = -1;
		isMyParent = false;
		isStatic = false;
		if (inputIndex >= _inputArrows.Count)
		{
			return false;
		}
		if (_inputArrows[inputIndex] == null)
		{
			return false;
		}
		connectionArrowObj = _inputArrows[inputIndex].OutputSVArrowBlock;
		connectionArrowIndex = _inputArrows[inputIndex].OutputSVArrowIndex;
		if (connectionArrowObj == null)
		{
			connectionArrowObj = _inputArrows[inputIndex].StaticConnectedBlock;
			connectionArrowIndex = _inputArrows[inputIndex].StaticConnectedIndex;
			if (connectionArrowObj != null)
			{
				isStatic = true;
			}
		}
		isMyParent = connectionArrowObj == base.Parent;
		return connectionArrowObj != null;
	}

	public bool GetArrowStartOutputObject(int inputIndex, out CSObject connectionObj, out int connectionIndex, out bool isStatic)
	{
		isStatic = false;
		connectionObj = _outputArrows[inputIndex].OutputSVArrowBlock;
		connectionIndex = _outputArrows[inputIndex].OutputSVArrowIndex;
		if (connectionObj == null)
		{
			connectionObj = _outputArrows[inputIndex].StaticConnectedBlock;
			connectionIndex = _outputArrows[inputIndex].StaticConnectedIndex;
			if (connectionObj != null)
			{
				isStatic = true;
			}
		}
		return connectionObj != null;
	}

	public CSObject GetChildObject(int index)
	{
		int num = 0;
		for (int i = 0; i <= index; i++)
		{
			if (IsHiddenBlock(_childObjs[i]))
			{
				num++;
			}
		}
		return _childObjs[index + num];
	}

	public override CSInputChannel GetInputChannel(int index)
	{
		if (index >= _inputArrows.Count)
		{
			return null;
		}
		return _inputArrows[index];
	}

	public List<CSInputChannel> GetInputListDup()
	{
		List<CSInputChannel> list = new List<CSInputChannel>();
		list.AddRange(_inputArrows);
		return list;
	}

	public virtual CSInputChannel GetOutputChannel(int index)
	{
		if (index >= _outputArrows.Count)
		{
			return null;
		}
		return _outputArrows[index];
	}

	public void InputChannelAdd(CSInputChannel csic)
	{
		csic.Parent = this;
		_inputArrows.Add(csic);
	}

	public void InputChannelRemove(int index)
	{
		_inputArrows.RemoveAt(index);
	}

	public abstract Point InternalSocketInLocation(int index);

	public abstract Point InternalSocketOutLocation(int index);

	public void OutputChannelAdd(CSInputChannel csic)
	{
		csic.Parent = this;
		_outputArrows.Add(csic);
	}

	public void OutputChannelRemove(int index)
	{
		_outputArrows.RemoveAt(index);
	}

	public void PushToEnd(CSObject obj)
	{
		_childObjs.Remove(obj);
		_childObjs.Add(obj);
	}

	public void SetInputChannel(int index, CSInputChannel inChan)
	{
		if (index >= 0 && index < _inputArrows.Count)
		{
			_inputArrows[index] = inChan;
		}
	}

	public void SetOutputChannel(int index, CSInputChannel inChan)
	{
		if (index >= 0 && index < _outputArrows.Count)
		{
			_outputArrows[index] = inChan;
		}
	}

	public void SetStaticUserData()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < InputChannelCount; i++)
		{
			CSInputChannel inputChannel = GetInputChannel(i);
			if (!inputChannel.SocketStatic)
			{
				break;
			}
			list.Add(inputChannel.StaticConnectedBlock?.TableIndex ?? (-1));
			list.Add(inputChannel.StaticConnectedIndex);
			list.Add((int)inputChannel.DataTypeFilter);
			list.Add((int)inputChannel.ArrayTypeFilter);
			base.UserData.AddKey("SuperView", "InputIdent" + i, inputChannel.Identifier);
		}
		if (list.Count == 0)
		{
			base.UserData.RemoveKey("SuperView", "StaticInputData");
		}
		else
		{
			base.UserData.AddKey("SuperView", "StaticInputData", string.Join(",", list));
		}
		List<int> list2 = new List<int>();
		for (int j = 0; j < OutputChannelCount; j++)
		{
			CSInputChannel outputChannel = GetOutputChannel(j);
			if (!outputChannel.SocketStatic)
			{
				break;
			}
			list2.Add(outputChannel.StaticConnectedBlock?.TableIndex ?? (-1));
			list2.Add(outputChannel.StaticConnectedIndex);
			list2.Add((int)outputChannel.DataTypeFilter);
			list2.Add((int)outputChannel.ArrayTypeFilter);
			base.UserData.AddKey("SuperView", "OutputIdent" + j, outputChannel.Identifier);
		}
		if (list2.Count == 0)
		{
			base.UserData.RemoveKey("SuperView", "StaticOutputData");
		}
		else
		{
			base.UserData.AddKey("SuperView", "StaticOutputData", string.Join(",", list2));
		}
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		CSTreeObject ioObject = setObject as CSTreeObject;
		if (ioObject == null)
		{
			return true;
		}
		ioObject._inputArrows.ForEach(delegate(CSInputChannel undoRecordInputChannel)
		{
			int num = ioObject._inputArrows.IndexOf(undoRecordInputChannel);
			CSInputChannel cSInputChannel;
			if (_inputArrows.Count - 1 < num)
			{
				cSInputChannel = new CSInputChannel(undoRecordInputChannel);
				_inputArrows.Add(cSInputChannel);
			}
			else
			{
				cSInputChannel = _inputArrows.ElementAt(num);
			}
			if (undoRecordInputChannel.OutputChannelBlock != null)
			{
				if (undoRecordInputChannel.OutputChannelBlock.IsOutputBlockProvider && !(undoRecordInputChannel.Parent is ICSVHandle))
				{
					cSInputChannel.SetLink(undoRecordInputChannel.OutputChannelBlock, isMaintainingUndo: false);
				}
				else
				{
					cSInputChannel.SetLink(undoRecordInputChannel.OutputChannelBlock, undoRecordInputChannel.OutputChannelIndex, isMaintainingUndo: false);
				}
			}
			else
			{
				cSInputChannel.BreakLink(isMaintainingUndo: false);
			}
			cSInputChannel.StaticConnectedBlock = undoRecordInputChannel.StaticConnectedBlock;
			cSInputChannel.StaticConnectedIndex = undoRecordInputChannel.StaticConnectedIndex;
			cSInputChannel.SocketStatic = undoRecordInputChannel.SocketStatic;
			cSInputChannel.DataTypeFilter = undoRecordInputChannel.DataTypeFilter;
			cSInputChannel.ArrayTypeFilter = undoRecordInputChannel.ArrayTypeFilter;
		});
		if (_inputArrows.Count > ioObject.InputChannelCount)
		{
			_inputArrows.RemoveRange(ioObject.InputChannelCount, _inputArrows.Count - ioObject.InputChannelCount);
		}
		ioObject._outputArrows.ForEach(delegate(CSInputChannel undoRecordOutputChannel)
		{
			int num = ioObject._outputArrows.IndexOf(undoRecordOutputChannel);
			CSInputChannel cSInputChannel;
			if (_outputArrows.Count - 1 < num)
			{
				cSInputChannel = new CSInputChannel(undoRecordOutputChannel);
				cSInputChannel.SetParent(this);
				_outputArrows.Add(cSInputChannel);
			}
			else
			{
				cSInputChannel = _outputArrows.ElementAt(num);
			}
			cSInputChannel.StaticConnectedBlock = undoRecordOutputChannel.StaticConnectedBlock;
			cSInputChannel.StaticConnectedIndex = undoRecordOutputChannel.StaticConnectedIndex;
			cSInputChannel.SocketStatic = undoRecordOutputChannel.SocketStatic;
			cSInputChannel.DataTypeFilter = undoRecordOutputChannel.DataTypeFilter;
			cSInputChannel.ArrayTypeFilter = undoRecordOutputChannel.ArrayTypeFilter;
		});
		if (_outputArrows.Count > ioObject.OutputChannelCount)
		{
			_outputArrows.RemoveRange(ioObject.OutputChannelCount, _outputArrows.Count - ioObject.OutputChannelCount);
		}
		return true;
	}

	protected void GetStaticUserData(CSParaGraph graph)
	{
		string text = base.UserData.FindValue("SuperView", "StaticInputData");
		if (!string.IsNullOrEmpty(text))
		{
			List<int> list = text.Split(',').Select(int.Parse).ToList();
			int num = list.Count / 4;
			for (int i = 0; i < num; i++)
			{
				int num2 = list[i * 4];
				int staticConnectedIndex = list[i * 4 + 1];
				ChannelDataType dataType = (ChannelDataType)list[i * 4 + 2];
				ChannelArrayType arrayType = (ChannelArrayType)list[i * 4 + 3];
				CSInputChannel csic = new CSInputChannel(null, dataType, arrayType, ChannelFlag.NotRequired)
				{
					Identifier = base.UserData.FindValue("SuperView", "InputIdent" + i),
					SocketStatic = true,
					StaticConnectedBlock = ((num2 >= 0) ? graph.GraphList[CSApplicationHelper.Application.ImportOffset + num2] : null),
					StaticConnectedIndex = staticConnectedIndex
				};
				InputChannelAdd(csic);
			}
		}
		string text2 = base.UserData.FindValue("SuperView", "StaticOutputData");
		if (!string.IsNullOrEmpty(text2))
		{
			List<int> list2 = text2.Split(',').Select(int.Parse).ToList();
			int num3 = list2.Count / 4;
			int count = graph.GraphList.Count;
			for (int j = 0; j < num3; j++)
			{
				int num4 = list2[j * 4];
				int staticConnectedIndex2 = list2[j * 4 + 1];
				ChannelDataType dataType2 = (ChannelDataType)list2[j * 4 + 2];
				ChannelArrayType arrayType2 = (ChannelArrayType)list2[j * 4 + 3];
				CSInputChannel csic2 = new CSInputChannel(null, dataType2, arrayType2, ChannelFlag.NotRequired)
				{
					Identifier = base.UserData.FindValue("SuperView", "OutputIdent" + j),
					SocketStatic = true,
					StaticConnectedBlock = ((num4 >= 0 && CSApplicationHelper.Application.ImportOffset + num4 < count) ? graph.GraphList[CSApplicationHelper.Application.ImportOffset + num4] : null),
					StaticConnectedIndex = staticConnectedIndex2
				};
				OutputChannelAdd(csic2);
			}
		}
	}
}
