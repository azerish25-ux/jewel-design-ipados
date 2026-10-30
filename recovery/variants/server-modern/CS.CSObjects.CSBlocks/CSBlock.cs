using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CS.CSDataUtilities;
using CS.CSObjects.ObsoleteGrouping;
using CS.CSValues.CSValueHandle.Interfaces;
using CS.CSValues.Interfaces;
using CS.MaterialGroup;
using CS.Parametric;
using CS.Parametric.Channels;
using CS.Utilities.Helpers;
using CSDatas;
using CSDatas.Interfaces;
using CSMaterials;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CS.CSObjects.CSBlocks;

public abstract class CSBlock(string identifier) : CSObject(identifier)
{
	private CancellationTokenSource _blockCancellationTokenSource;

	private int _lastProcessedMark = -1;

	private bool _autoCheckInputs;

	private static readonly object _parallelLockMaterialUpdate = new object();

	private bool _hasCreatedUndoTransaction;

	public List<CSInputChannel> InputChannels { get; } = new List<CSInputChannel>();

	public List<CSOutputChannel> OutputChannels { get; } = new List<CSOutputChannel>();

	public override int InputChannelCount => InputChannels.Count;

	public override int OutputChannelCount => OutputChannels.Count;

	public List<CSData> OutputChannelDatas => OutputChannels.Select((CSOutputChannel outputChannel) => outputChannel.Data).ToList();

	public bool HasIllegalLoop => LoopCheckStatus == BlockLoopCheckStatus.HasIllegalLoops;

	public bool IsOutputBlockProvider { get; set; }

	public BlockLoopCheckStatus LoopCheckStatus { get; set; }

	public BlockProcessingStatus ProcessingStatus { get; set; }

	private static HashSet<Guid> CheckedLoopAgainst { get; set; }

	public int AddInputChannel(string identifier, ChannelFlag flags = ChannelFlag.NotRequired)
	{
		CSInputChannel cSInputChannel = InputChannels.FirstOrDefault((CSInputChannel input) => input.Identifier == identifier);
		if (cSInputChannel != null)
		{
			cSInputChannel.SetParent(this);
		}
		else
		{
			new CSInputChannel(identifier, this, ChannelDataType.Any, ChannelArrayType.Any, flags);
		}
		return InputChannels.Count - 1;
	}

	public void AddInputChannelToParent(CSInputChannel csInputChannel)
	{
		InputChannels.Add(csInputChannel);
	}

	public virtual bool AddObjectFromCurrentSelection()
	{
		return false;
	}

	public virtual bool AddObjectFromDocument(string objectName)
	{
		return false;
	}

	public virtual bool AddObjectFromDocument(Guid value)
	{
		return false;
	}

	public virtual bool AddObjectFromFile(string fileName)
	{
		return false;
	}

	public virtual bool AddObjectFromLibrary(string libraryFileName)
	{
		return false;
	}

	public virtual bool AddObjectsFromLibraryPath(string libraryPath)
	{
		if (!Directory.Exists(Path.Combine(CSApplicationHelper.Application.CSLibraryPath, libraryPath)))
		{
			return false;
		}
		DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(CSApplicationHelper.Application.CSLibraryPath, libraryPath));
		CSUtil.Print("Reading Profiles From " + directoryInfo.FullName + "\n");
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			if (fileInfo.Name.Substring(fileInfo.Name.Length - 4) == ".xml" || fileInfo.Name.Substring(fileInfo.Name.Length - 4) == ".3dm" || fileInfo.Name.Substring(fileInfo.Name.Length - 4) == ".bin")
			{
				AddObjectFromFile(fileInfo.FullName);
			}
		}
		return true;
	}

	public int AddOutputChannel(string identifier)
	{
		CSOutputChannel cSOutputChannel = OutputChannels.FirstOrDefault((CSOutputChannel output) => output.Identifier == identifier);
		if (cSOutputChannel != null)
		{
			cSOutputChannel.SetParent(this);
		}
		else
		{
			new CSOutputChannel(identifier, this, ChannelDataType.Any, ChannelArrayType.Any);
		}
		return OutputChannels.Count - 1;
	}

	public void AddOutputChannelToParent(CSOutputChannel csOutputChannel)
	{
		OutputChannels.Add(csOutputChannel);
	}

	public bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		for (int i = 0; i < OutputChannelCount; i++)
		{
			if (GetOutputChannelData(i) is ICSDocumentObject iCSDocumentObject)
			{
				flag &= iCSDocumentObject.AddToDocumentExport(ref rhinoRenderObjects, transform, GetOptionsExport());
			}
		}
		return flag;
	}

	public bool AddToDocumentRender(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		for (int i = 0; i < OutputChannelCount; i++)
		{
			if (GetOutputChannelData(i) is ICSRenderObject iCSRenderObject)
			{
				flag &= iCSRenderObject.AddToDocumentRender(ref rhinoRenderObjects, transform);
			}
		}
		return flag;
	}

	public virtual bool BlankOutBeforeUpdate()
	{
		return true;
	}

	public virtual bool CopySelected(int outputChannelIndex)
	{
		return false;
	}

	public virtual bool DeleteObject(int index)
	{
		return false;
	}

	public void Generate(bool printTiming = true)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		if (printTiming)
		{
			CSUtil.Print("Generate:\n");
		}
		MarkAllChanged();
		long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
		UpdateAllChanged();
		long elapsedMilliseconds2 = stopwatch.ElapsedMilliseconds;
		if (printTiming)
		{
			CSUtil.Print("Mark in " + elapsedMilliseconds + " ms\n");
			CSUtil.Print("Update in " + elapsedMilliseconds2 + " ms\n");
		}
	}

	public override bool GetArrowStartObject(int inputIndex, out CSObject connectionArrowObj, out int connectionArrowIndex, out bool isMyParent, out bool isStatic)
	{
		connectionArrowObj = null;
		connectionArrowIndex = -1;
		isMyParent = false;
		isStatic = false;
		if (inputIndex >= InputChannels.Count)
		{
			return false;
		}
		if (InputChannels[inputIndex] == null)
		{
			return false;
		}
		connectionArrowObj = InputChannels[inputIndex].OutputSVArrowBlock;
		connectionArrowIndex = InputChannels[inputIndex].OutputSVArrowIndex;
		if (connectionArrowObj == null)
		{
			connectionArrowObj = InputChannels[inputIndex].StaticConnectedBlock;
			connectionArrowIndex = InputChannels[inputIndex].StaticConnectedIndex;
			if (connectionArrowObj != null)
			{
				isStatic = true;
			}
		}
		isMyParent = connectionArrowObj == base.Parent;
		return connectionArrowObj != null;
	}

	public override CSInputChannel GetInputChannel(int inputChannelIndex)
	{
		if (inputChannelIndex < 0 || inputChannelIndex >= InputChannels.Count)
		{
			return null;
		}
		return InputChannels[inputChannelIndex];
	}

	public string GetInputChannelIdentifier(int inputChannelIndex)
	{
		if (inputChannelIndex < 0 || inputChannelIndex >= InputChannels.Count)
		{
			return null;
		}
		return InputChannels[inputChannelIndex].Identifier;
	}

	public CSBlock GetInputChannelParent(int inputChannelIndex)
	{
		if (inputChannelIndex < 0 || inputChannelIndex >= InputChannels.Count)
		{
			return null;
		}
		return InputChannels[inputChannelIndex].OutputChannelBlock;
	}

	public bool GetInputChannelParent(int inputChannelIndex, ref int graphIndex)
	{
		if (inputChannelIndex < 0 || inputChannelIndex >= InputChannels.Count)
		{
			return false;
		}
		if (GetInputChannelParent(inputChannelIndex) == null)
		{
			return false;
		}
		CSBlock outputChannelBlock = InputChannels[inputChannelIndex].OutputChannelBlock;
		if (outputChannelBlock == null)
		{
			return false;
		}
		graphIndex = CSApplicationHelper.Application.Document.Graph.GraphList.IndexOf(outputChannelBlock);
		if (graphIndex >= 0)
		{
			return graphIndex < CSApplicationHelper.Application.Document.Graph.GraphList.Count;
		}
		return false;
	}

	public bool GetMeshableObjects(ref ConcurrentBag<GeometryBase> meshableObjects)
	{
		bool flag = true;
		for (int i = 0; i < OutputChannelCount; i++)
		{
			if (GetOutputChannelData(i) is ICSMeshable iCSMeshable)
			{
				flag &= iCSMeshable.GetMeshableObjects(ref meshableObjects, GetOptionsExport());
			}
		}
		return flag;
	}

	public virtual CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (outputChannelIndex < 0 || outputChannelIndex >= OutputChannels.Count)
		{
			return null;
		}
		return OutputChannels[outputChannelIndex];
	}

	public virtual CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (outputChannelIndex < 0 || outputChannelIndex >= OutputChannels.Count)
		{
			return new CSData();
		}
		return OutputChannels[outputChannelIndex].Data;
	}

	public virtual string GetOutputChannelIdentifier(int outputChannelIndex)
	{
		if (outputChannelIndex < 0 || outputChannelIndex >= OutputChannels.Count)
		{
			return null;
		}
		return OutputChannels[outputChannelIndex].Identifier;
	}

	public bool InputError()
	{
		return UpdateInputOutputObject();
	}

	public bool IsInputChannelNotChecked(int inputChannelIndex)
	{
		return InputChannels[inputChannelIndex].IsNotChecked;
	}

	public bool MarkAllChanged()
	{
		return MarkAllChanged(base.UpdateMarker);
	}

	public virtual int ObjectCount()
	{
		return 0;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		if (!base.Read(ref archive, graph, major, minor))
		{
			return false;
		}
		base.UserData.RemoveAllDataOfType("SubObjectMaterialOverride");
		int[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		if (items == null)
		{
			CSUtil.Print($"ERROR: Reading object indices for {base.Identifier}\n");
			return false;
		}
		int[] items2 = null;
		CSUtilIO.ReadArray(ref items2, ref archive);
		if (items2 == null)
		{
			CSUtil.Print($"ERROR: Reading channel indices for {base.Identifier}\n");
			return false;
		}
		if (items.Length != items2.Length)
		{
			CSUtil.Print($"ERROR: Mismatched object and channel indices count for {base.Identifier}\n");
			return false;
		}
		for (int i = 0; i < items.Length; i++)
		{
			CSBlock outputChannelBlock = null;
			int outputChannelIndex = -1;
			int num = items[i];
			if (num >= 0)
			{
				if (CSApplicationHelper.Application.InImport)
				{
					num += CSApplicationHelper.Application.ImportOffset;
				}
				if (num >= 0 && num < graph.GraphList.Count)
				{
					outputChannelBlock = (CSBlock)graph.GraphList[num];
					outputChannelIndex = items2[i];
				}
			}
			if (i < InputChannelCount)
			{
				InputChannels[i].OutputChannelBlock = outputChannelBlock;
				InputChannels[i].OutputChannelIndex = outputChannelIndex;
			}
			else if (this is CSBlockExpandableInput cSBlockExpandableInput)
			{
				int index = cSBlockExpandableInput.CreateExpansionInputChannel();
				InputChannels[index].OutputChannelBlock = outputChannelBlock;
				InputChannels[index].OutputChannelIndex = outputChannelIndex;
			}
			else if (this is CSGroupStartObsolete || this is CSGroupEndObsolete)
			{
				CSInputChannel cSInputChannel = new CSInputChannel(outputChannelBlock, outputChannelIndex);
				cSInputChannel.SetParent(this);
				InputChannels.Add(cSInputChannel);
			}
			else
			{
				CSUtil.Print($"ERROR: Too many input channels for {base.Identifier}\n");
			}
		}
		string text = base.UserData.FindValue("SuperView", "StaticInputData");
		if (!string.IsNullOrEmpty(text))
		{
			List<int> list = text.Split(',').Select(int.Parse).ToList();
			int num2 = 0;
			for (int j = 0; j < list.Count; j += 3)
			{
				bool socketStatic = list[j] > 0;
				int num3 = list[j + 1];
				int staticConnectedIndex = list[j + 2];
				CSInputChannel inputChannel = GetInputChannel(num2);
				inputChannel.SocketStatic = socketStatic;
				inputChannel.StaticConnectedBlock = ((num3 >= 0) ? graph.GraphList[CSApplicationHelper.Application.ImportOffset + num3] : null);
				inputChannel.StaticConnectedIndex = staticConnectedIndex;
				num2++;
			}
		}
		string text2 = base.UserData.FindValue("SuperView", "StaticOutputData");
		if (!string.IsNullOrEmpty(text2))
		{
			List<int> list2 = text2.Split(',').Select(int.Parse).ToList();
			for (int k = 0; k < list2.Count; k++)
			{
				bool socketStatic2 = list2[k] > 0;
				GetOutputChannel(k).SocketStatic = socketStatic2;
			}
		}
		return true;
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		if (!base.ReadJson(jBlock, graph, major, minor))
		{
			return false;
		}
		base.UserData.RemoveAllDataOfType("SubObjectMaterialOverride");
		JArray jArray = (JArray)jBlock["ObjectIndex"];
		JArray jArray2 = (JArray)jBlock["ChannelIndex"];
		if (jArray.Count != jArray2.Count)
		{
			CSUtil.Print($"ERROR: Mismatched object and channel indices count for {base.Identifier}\n");
			return false;
		}
		for (int i = 0; i < jArray.Count; i++)
		{
			CSBlock outputChannelBlock = null;
			int outputChannelIndex = -1;
			int num = Convert.ToInt32(jArray[i]);
			if (num >= 0)
			{
				if (CSApplicationHelper.Application.InImport)
				{
					num += CSApplicationHelper.Application.ImportOffset;
				}
				if (num >= 0 && num < graph.GraphList.Count)
				{
					outputChannelBlock = (CSBlock)graph.GraphList[num];
					outputChannelIndex = Convert.ToInt32(jArray2[i]);
				}
			}
			if (i < InputChannelCount)
			{
				InputChannels[i].OutputChannelBlock = outputChannelBlock;
				InputChannels[i].OutputChannelIndex = outputChannelIndex;
			}
			else if (this is CSBlockExpandableInput cSBlockExpandableInput)
			{
				int index = cSBlockExpandableInput.CreateExpansionInputChannel();
				InputChannels[index].OutputChannelBlock = outputChannelBlock;
				InputChannels[index].OutputChannelIndex = outputChannelIndex;
			}
			else if (this is CSGroupStartObsolete || this is CSGroupEndObsolete)
			{
				CSInputChannel cSInputChannel = new CSInputChannel(outputChannelBlock, outputChannelIndex);
				cSInputChannel.SetParent(this);
				InputChannels.Add(cSInputChannel);
			}
			else
			{
				CSUtil.Print($"ERROR: Too many input channels for {base.Identifier}\n");
			}
		}
		string text = base.UserData.FindValue("SuperView", "StaticInputData");
		if (!string.IsNullOrEmpty(text))
		{
			List<int> list = text.Split(',').Select(int.Parse).ToList();
			int num2 = 0;
			for (int j = 0; j < list.Count; j += 3)
			{
				bool socketStatic = list[j] > 0;
				int num3 = list[j + 1];
				int staticConnectedIndex = list[j + 2];
				CSInputChannel inputChannel = GetInputChannel(num2);
				inputChannel.SocketStatic = socketStatic;
				inputChannel.StaticConnectedBlock = ((num3 >= 0) ? graph.GraphList[CSApplicationHelper.Application.ImportOffset + num3] : null);
				inputChannel.StaticConnectedIndex = staticConnectedIndex;
				num2++;
			}
		}
		string text2 = base.UserData.FindValue("SuperView", "StaticOutputData");
		if (!string.IsNullOrEmpty(text2))
		{
			List<int> list2 = text2.Split(',').Select(int.Parse).ToList();
			for (int k = 0; k < list2.Count; k++)
			{
				bool socketStatic2 = list2[k] > 0;
				GetOutputChannel(k).SocketStatic = socketStatic2;
			}
		}
		return true;
	}

	public void RemoveInputChannel(int inputChannelIndex)
	{
		if (inputChannelIndex >= 0 && inputChannelIndex < InputChannels.Count)
		{
			InputChannels.RemoveAt(inputChannelIndex);
		}
	}

	public void RequestCancellationOfParallelTasks()
	{
		if (_blockCancellationTokenSource != null && !_blockCancellationTokenSource.IsCancellationRequested)
		{
			_blockCancellationTokenSource.Cancel();
		}
	}

	public void ResetInputChannel(int inputChannelIndex)
	{
		if (inputChannelIndex >= 0 && inputChannelIndex < InputChannels.Count)
		{
			PreChange("ResetInputChannel");
			InputChannels[inputChannelIndex].BreakLink();
			Change();
		}
	}

	public virtual bool SetBitmapToChannel(int index, Bitmap image)
	{
		return false;
	}

	public bool SetInputChannel(int inputChannelIndex, CSBlock inputBlock, int outputChannelIndex)
	{
		if (inputChannelIndex < 0 || inputChannelIndex >= InputChannels.Count)
		{
			return false;
		}
		PreChange("SetInputChannel");
		InputChannels[inputChannelIndex] = new CSInputChannel(inputBlock, outputChannelIndex);
		InputChannels[inputChannelIndex].SetParent(this);
		Change();
		return true;
	}

	public void SetMessage(string message)
	{
		if (message != null)
		{
			if (Message == null)
			{
				Message = message;
			}
			else if (Message != message)
			{
				Message = Message + "\n" + message;
			}
		}
	}

	public void SetStaticUserData()
	{
		List<int> list = new List<int>();
		bool flag = false;
		for (int i = 0; i < InputChannelCount; i++)
		{
			CSInputChannel inputChannel = GetInputChannel(i);
			flag |= inputChannel.SocketStatic;
			list.Add(inputChannel.SocketStatic ? 1 : 0);
			list.Add(inputChannel.StaticConnectedBlock?.TableIndex ?? (-1));
			list.Add(inputChannel.StaticConnectedIndex);
		}
		if (!flag)
		{
			base.UserData.RemoveKey("SuperView", "StaticInputData");
		}
		else
		{
			base.UserData.AddKey("SuperView", "StaticInputData", string.Join(",", list));
		}
		List<int> list2 = new List<int>();
		flag = false;
		for (int j = 0; j < OutputChannelCount; j++)
		{
			CSOutputChannel outputChannel = GetOutputChannel(j);
			if (outputChannel != null)
			{
				flag |= outputChannel.SocketStatic;
				list2.Add(outputChannel.SocketStatic ? 1 : 0);
			}
		}
		if (!flag)
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
		if (!base.NotCreatedYet)
		{
			CSBlock ioObject = setObject as CSBlock;
			if (ioObject != null)
			{
				ioObject.InputChannels.ForEach(delegate(CSInputChannel undoRecordInputChannel)
				{
					CSInputChannel cSInputChannel = null;
					int num = ioObject.InputChannels.IndexOf(undoRecordInputChannel);
					if (InputChannels.Count - 1 < num)
					{
						cSInputChannel = new CSInputChannel(undoRecordInputChannel);
						cSInputChannel.SetParent(this);
						InputChannels.Add(cSInputChannel);
					}
					else
					{
						cSInputChannel = InputChannels.ElementAt(num);
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
				});
				if (InputChannels.Count > ioObject.InputChannelCount)
				{
					InputChannels.RemoveRange(ioObject.InputChannelCount, InputChannels.Count - ioObject.InputChannelCount);
				}
				ioObject.OutputChannels.ForEach(delegate(CSOutputChannel undoRecordOutputChannel)
				{
					CSOutputChannel cSOutputChannel = null;
					int num = ioObject.OutputChannels.IndexOf(undoRecordOutputChannel);
					if (OutputChannels.Count - 1 < num)
					{
						cSOutputChannel = new CSOutputChannel(undoRecordOutputChannel);
						cSOutputChannel.SetParent(this);
						OutputChannels.Add(cSOutputChannel);
					}
					else
					{
						cSOutputChannel = OutputChannels.ElementAt(num);
					}
					cSOutputChannel.SocketStatic = undoRecordOutputChannel.SocketStatic;
				});
				if (OutputChannels.Count > ioObject.OutputChannelCount)
				{
					OutputChannels.RemoveRange(ioObject.OutputChannelCount, OutputChannels.Count - ioObject.OutputChannelCount);
				}
			}
		}
		return true;
	}

	public override bool Update()
	{
		return UpdateInputOutputObject();
	}

	public bool UpdateAllChanged()
	{
		if (HasThisObjectChangedSinceLastUpdate())
		{
			UIChanged = true;
			base.Error = false;
			base.MissingInput = false;
			base.ExceptionThrown = false;
			Message = null;
			ExceptionMessage = null;
			int updateMarker = base.UpdateMarker;
			base.UpdateMarker = CSApplicationHelper.Application.Document.Graph.UpdateCount;
			foreach (CSInputChannel inputChannel in InputChannels)
			{
				if (inputChannel != null)
				{
					if (CSApplicationHelper.Application.Document.InterruptUpdate())
					{
						break;
					}
					inputChannel.OutputChannelBlock?.UpdateAllChanged();
				}
			}
			long ticks = DateTime.Now.Ticks;
			if (BlankOutBeforeUpdate())
			{
				CSData data = new CSData();
				for (int i = 0; i < OutputChannels.Count; i++)
				{
					OutputChannels.ElementAt(i).SetData(data);
				}
			}
			base.UpdateMarker = updateMarker;
			CSDataReport.PrintBlockInputs(this);
			bool flag = false;
			if (!CSApplicationHelper.Application.Document.InterruptUpdate())
			{
				try
				{
					base.ExceptionThrown = false;
					flag = PreUpdateCheck();
					if (flag)
					{
						flag = Update();
					}
					if (!flag)
					{
						base.MissingInput = true;
						base.Update();
					}
				}
				catch (Exception ex)
				{
					ExceptionMessage = ex.ToString();
					base.ExceptionThrown = true;
				}
				base.LastUpdateTime = (double)(DateTime.Now.Ticks - ticks) / 10000000.0;
				base.TotalLastUpdateTime += (double)(DateTime.Now.Ticks - ticks) / 10000000.0;
				base.UpdateCounts++;
			}
			CSDataReport.PrintBlockOutputs(this);
			return flag;
		}
		return false;
	}

	public bool UpdateCancel()
	{
		return false;
	}

	public bool UpdateError(string message = null)
	{
		bool result = UpdateInputOutputObject();
		base.Error = true;
		if (message != null)
		{
			SetMessage(message);
		}
		return result;
	}

	public virtual bool UpdateGroupObject(int index, NurbsCurve curve, List<double> points)
	{
		return false;
	}

	public virtual bool UpdateGroupObjectFromUI(int which, NurbsCurve curve, List<double> points, bool mouseDown)
	{
		bool result = false;
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			if (!_hasCreatedUndoTransaction)
			{
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("UpdateGroupObjectFromUI");
				PreChange("UpdateGroupObjectFromUI");
				_hasCreatedUndoTransaction = true;
			}
			if (mouseDown)
			{
				CSUtil.ToggleMouseDown(this);
				result = UpdateGroupObject(which, curve, points);
			}
			else
			{
				CSUtil.RemoveMouseDown();
				result = UpdateGroupObject(which, curve, points);
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("UpdateGroupObjectFromUI");
				_hasCreatedUndoTransaction = false;
			}
			CSApplicationHelper.Application.Document.Graph.Generate(mouseDown);
			CSUtil.Redraw();
			if (!mouseDown)
			{
				CSUtil.RedrawUI();
			}
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.Message + "\n");
			CSUtil.Print(ex.StackTrace + "\n");
		}
		return result;
	}

	public void UpdateMaterialAssignment()
	{
		lock (_parallelLockMaterialUpdate)
		{
			Guid instanceGuid = base.InstanceGuid;
			for (int i = 0; i < OutputChannelCount; i++)
			{
				CSData outputChannelData = GetOutputChannelData(i);
				if (outputChannelData == null)
				{
					continue;
				}
				if (outputChannelData is CSDataArray cSDataArray)
				{
					CSDataArray flattenArray = cSDataArray.GetFlattenArray();
					for (int j = 0; j < flattenArray.GetArrayCount(); j++)
					{
						CSData arrayItem = flattenArray.GetArrayItem(j);
						CSMaterials.MaterialGroup assignedMaterialGroup = CS.MaterialGroup.MaterialGroupHelper.GetAssignedMaterialGroup($"{instanceGuid}:{i}:{j}");
						if (assignedMaterialGroup != null)
						{
							AssignDisplayMaterialGroups(arrayItem, assignedMaterialGroup);
							continue;
						}
						CSMaterials.MaterialGroup assignedMaterialGroup2 = CS.MaterialGroup.MaterialGroupHelper.GetAssignedMaterialGroup($"{instanceGuid}:{i}");
						if (assignedMaterialGroup2 != null)
						{
							AssignDisplayMaterialGroups(arrayItem, assignedMaterialGroup2);
							continue;
						}
						CSMaterials.MaterialGroup assignedMaterialGroup3 = CS.MaterialGroup.MaterialGroupHelper.GetAssignedMaterialGroup($"{instanceGuid}");
						if (assignedMaterialGroup3 != null)
						{
							AssignDisplayMaterialGroups(arrayItem, assignedMaterialGroup3);
						}
						else if (!base.IsOutputDisplayMaterialClassificationAssigned)
						{
							CSMaterials.MaterialGroup currentMaterialGroup = arrayItem.CurrentMaterialGroup;
							if (currentMaterialGroup != null)
							{
								AssignDisplayMaterialGroups(arrayItem, currentMaterialGroup);
								CS.MaterialGroup.MaterialGroupHelper.AddItemToMaterialGroup(currentMaterialGroup, $"{instanceGuid}:{i}:{j}", redraw: false, isInherited: true);
							}
						}
					}
					continue;
				}
				CSMaterials.MaterialGroup assignedMaterialGroup4 = CS.MaterialGroup.MaterialGroupHelper.GetAssignedMaterialGroup($"{instanceGuid}:{i}:0");
				if (assignedMaterialGroup4 != null)
				{
					AssignDisplayMaterialGroups(outputChannelData, assignedMaterialGroup4);
					continue;
				}
				CSMaterials.MaterialGroup assignedMaterialGroup5 = CS.MaterialGroup.MaterialGroupHelper.GetAssignedMaterialGroup($"{instanceGuid}:{i}");
				if (assignedMaterialGroup5 != null)
				{
					AssignDisplayMaterialGroups(outputChannelData, assignedMaterialGroup5);
					continue;
				}
				CSMaterials.MaterialGroup assignedMaterialGroup6 = CS.MaterialGroup.MaterialGroupHelper.GetAssignedMaterialGroup($"{instanceGuid}");
				if (assignedMaterialGroup6 != null)
				{
					AssignDisplayMaterialGroups(outputChannelData, assignedMaterialGroup6);
				}
				else if (!base.IsOutputDisplayMaterialClassificationAssigned)
				{
					CSMaterials.MaterialGroup currentMaterialGroup2 = outputChannelData.CurrentMaterialGroup;
					if (currentMaterialGroup2 != null)
					{
						AssignDisplayMaterialGroups(outputChannelData, currentMaterialGroup2);
						CS.MaterialGroup.MaterialGroupHelper.AddItemToMaterialGroup(currentMaterialGroup2, $"{instanceGuid}:{i}:0", redraw: false, isInherited: true);
					}
				}
			}
		}
	}

	public bool UpdateMissing(string message = null)
	{
		bool result = UpdateInputOutputObject();
		base.MissingInput = true;
		if (message != null)
		{
			SetMessage(message);
		}
		return result;
	}

	public bool UpdateStatusForProcessing()
	{
		return UpdateStatusForProcessing(base.UpdateMarker);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		List<int> list = new List<int>(InputChannels.Count);
		List<int> list2 = new List<int>(InputChannels.Count);
		foreach (CSInputChannel inputChannel in InputChannels)
		{
			bool flag = false;
			if (inputChannel.OutputChannelBlock != null)
			{
				list.Add(inputChannel.OutputChannelBlock.TableIndex);
				list2.Add(inputChannel.OutputChannelIndex);
				flag = true;
			}
			if (!flag)
			{
				list.Add(-1);
				list2.Add(-5);
			}
		}
		int[] items = list.ToArray();
		int[] items2 = list2.ToArray();
		CSUtilIO.WriteArray(items, ref archive);
		CSUtilIO.WriteArray(items2, ref archive);
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		List<int> list = new List<int>(InputChannels.Count);
		List<int> list2 = new List<int>(InputChannels.Count);
		foreach (CSInputChannel inputChannel in InputChannels)
		{
			bool flag = false;
			if (inputChannel.OutputChannelBlock != null)
			{
				list.Add(inputChannel.OutputChannelBlock.TableIndex);
				list2.Add(inputChannel.OutputChannelIndex);
				flag = true;
			}
			if (!flag)
			{
				list.Add(-1);
				list2.Add(-5);
			}
		}
		JArray value = new JArray(list);
		JArray value2 = new JArray(list2);
		archive.Add("ObjectIndex", value);
		archive.Add("ChannelIndex", value2);
	}

	protected ParallelOptions GetBlockParallelOptions()
	{
		return new ParallelOptions
		{
			MaxDegreeOfParallelism = CSApplicationHelper.MaxNumberOfBlockUpdateAvailableCores,
			CancellationToken = _blockCancellationTokenSource.Token
		};
	}

	protected string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
	{
		if (propertyExpression == null)
		{
			throw new ArgumentNullException("propertyExpression");
		}
		PropertyInfo obj = ((propertyExpression.Body as MemberExpression) ?? throw new ArgumentException("Invalid argument", "propertyExpression")).Member as PropertyInfo;
		if (obj == null)
		{
			throw new ArgumentException("Argument is not a property", "propertyExpression");
		}
		return obj.Name;
	}

	protected void InitializeCancellationTokenSource()
	{
		RequestCancellationOfParallelTasks();
		_blockCancellationTokenSource = new CancellationTokenSource();
	}

	protected void RemoveInputChannel(string identifier)
	{
		for (int i = 0; i < InputChannels.Count; i++)
		{
			if (!(InputChannels[i].Identifier != identifier))
			{
				InputChannels[i].BreakLink();
				InputChannels.RemoveAt(i);
				break;
			}
		}
	}

	protected void SetChannelIdentifiers(bool autoCheckInputs = false)
	{
		FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		_autoCheckInputs = autoCheckInputs;
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			if (fieldInfo.GetValue(this) is CSInputChannel cSInputChannel)
			{
				if (string.IsNullOrWhiteSpace(cSInputChannel.Identifier))
				{
					string text = fieldInfo.Name;
					int num = text.IndexOf("<", StringComparison.Ordinal);
					if (num >= 0)
					{
						text = text.Substring(num + 1);
					}
					num = text.IndexOf(">", StringComparison.Ordinal);
					if (num >= 0)
					{
						text = text.Substring(0, num);
					}
					cSInputChannel.Identifier = text;
				}
			}
			else if (fieldInfo.GetValue(this) is CSOutputChannel cSOutputChannel && string.IsNullOrWhiteSpace(cSOutputChannel.Identifier))
			{
				string text2 = fieldInfo.Name;
				int num2 = text2.IndexOf("<", StringComparison.Ordinal);
				if (num2 >= 0)
				{
					text2 = text2.Substring(num2 + 1);
				}
				num2 = text2.IndexOf(">", StringComparison.Ordinal);
				if (num2 >= 0)
				{
					text2 = text2.Substring(0, num2);
				}
				cSOutputChannel.Identifier = text2;
			}
		}
	}

	private static void AssignDisplayMaterialGroups(CSData csData, CSMaterials.MaterialGroup materialGroup)
	{
		if (materialGroup != null)
		{
			(csData as ICSDisplayObject)?.AssignCurrentMaterialGroup(materialGroup);
		}
	}

	private static CSOptionsExport GetOptionsExport()
	{
		return new CSOptionsExport
		{
			WhenExportingHonorGemPrintingFlags = CSApplicationHelper.Application.WhenExportingHonorGemPrintingFlags,
			IsCenterGemAllowedForPrinting = CSApplicationHelper.Application.IsCenterGemAllowedForPrinting,
			IsSideGemAllowedForPrinting = CSApplicationHelper.Application.IsSideGemAllowedForPrinting,
			IsAccentGemAllowedForPrinting = CSApplicationHelper.Application.IsAccentGemAllowedForPrinting,
			IsExportingSTL = CSApplicationHelper.Application.IsExportingSTL,
			RingSize = CSApplicationHelper.Application.RingSizeTranslator
		};
	}

	private bool MarkAllChanged(int childLastUpdated)
	{
		bool flag = base.UpdateMarker > childLastUpdated;
		bool flag2 = HasThisObjectChangedSinceLastUpdate();
		if (_lastProcessedMark == CSApplicationHelper.Application.Document.Graph.UpdateCount)
		{
			return flag2 || flag;
		}
		_lastProcessedMark = CSApplicationHelper.Application.Document.Graph.UpdateCount;
		bool flag3 = false;
		foreach (CSInputChannel inputChannel in InputChannels)
		{
			CSInputChannel cSInputChannel = inputChannel;
			if (cSInputChannel != null && !inputChannel.IsNotChecked && cSInputChannel.OutputChannelBlock != null)
			{
				flag3 |= cSInputChannel.OutputChannelBlock.MarkAllChanged(base.UpdateMarker);
			}
		}
		if (flag3 && !flag2)
		{
			Change();
			flag2 = true;
		}
		return flag2 || flag;
	}

	private bool PreUpdateCheck()
	{
		if (!_autoCheckInputs)
		{
			return true;
		}
		foreach (CSInputChannel inputChannel in InputChannels)
		{
			if (!inputChannel.CheckInputValid())
			{
				UpdateMissing(inputChannel.Parent.Identifier + " : " + inputChannel.Identifier + " is missing inputs");
				return false;
			}
		}
		return true;
	}

	private bool UpdateInputOutputObject()
	{
		bool result = base.Update();
		if (base.IsOutputDisplayMaterialClassificationAssigned)
		{
			foreach (CSData item in OutputChannels.Select((CSOutputChannel channel) => channel.Data))
			{
				if (item is ICSDisplayObject && item.DefaultDisplayMaterialClassification == MaterialClassification.Undefined)
				{
					item.SetMaterialClassificationOnDisplay(base.OutputDisplayMaterialClassification);
				}
			}
		}
		if (base.InDisplay)
		{
			UpdateMaterialAssignment();
		}
		foreach (CSOutputChannel item2 in OutputChannels.Where((CSOutputChannel outputObject) => outputObject?.Data != null))
		{
			item2.Data.SetOutputObject();
		}
		return result;
	}

	public bool CheckIllegalLoops(CSBlock currentCSBlock = null)
	{
		ICSLoopUpdate loopedBlock = this as ICSLoopUpdate;
		bool hasLoops = HasIllegalLoop || currentCSBlock == this;
		if (currentCSBlock == null)
		{
			currentCSBlock = this;
			CheckedLoopAgainst = new HashSet<Guid>();
		}
		else
		{
			CheckedLoopAgainst.Add(currentCSBlock.InstanceGuid);
		}
		if (!hasLoops)
		{
			currentCSBlock.InputChannels.Where((CSInputChannel inputChannel) => inputChannel != null && (loopedBlock == null || inputChannel != loopedBlock.InDisplayUpdate) && !CheckedLoopAgainst.Contains(currentCSBlock.InstanceGuid)).ToList().ForEach(delegate(CSInputChannel inputChannel)
			{
				if (inputChannel.OutputChannelBlock != null)
				{
					hasLoops |= CheckIllegalLoops(inputChannel.OutputChannelBlock);
				}
			});
		}
		LoopCheckStatus = ((!hasLoops) ? BlockLoopCheckStatus.Passed : BlockLoopCheckStatus.HasIllegalLoops);
		return hasLoops;
	}

	public void GenerateConcurrently(bool printTiming = true)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		if (printTiming)
		{
			CSUtil.Print("Generate:\n");
		}
		UpdateStatusForProcessing();
		CheckIllegalLoops();
		long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
		ParallelGraphProcessingHelper.ResetWorkingQueue();
		ParallelGraphProcessingHelper.ProcessGraph(new List<CSBlock> { this });
		long elapsedMilliseconds2 = stopwatch.ElapsedMilliseconds;
		if (printTiming)
		{
			CSUtil.Print("Mark in " + elapsedMilliseconds + " ms\n");
			CSUtil.Print("Update in " + elapsedMilliseconds2 + " ms\n");
		}
	}

	public bool Process()
	{
		bool flag = true;
		if (HasThisObjectChangedSinceLastUpdate())
		{
			base.Error = false;
			base.MissingInput = false;
			base.ExceptionThrown = false;
			Message = null;
			ExceptionMessage = null;
			long ticks = DateTime.Now.Ticks;
			if (BlankOutBeforeUpdate())
			{
				OutputChannels.ForEach(delegate(CSOutputChannel outputChannel)
				{
					outputChannel.SetData(new CSData());
				});
			}
			if (!CSApplicationHelper.Application.Document.InterruptUpdate())
			{
				try
				{
					base.ExceptionThrown = false;
					flag = PreUpdateCheck();
					if (flag)
					{
						flag = Update();
					}
					if (!flag)
					{
						base.MissingInput = true;
						base.Update();
					}
				}
				catch (Exception ex)
				{
					ExceptionMessage = ex.ToString();
					base.ExceptionThrown = true;
				}
				base.LastUpdateTime = (double)(DateTime.Now.Ticks - ticks) / 10000000.0;
				base.TotalLastUpdateTime += (double)(DateTime.Now.Ticks - ticks) / 10000000.0;
				base.UpdateCounts++;
			}
		}
		return flag;
	}

	private bool UpdateStatusForProcessing(int childUpdateMarker)
	{
		bool flag = false;
		flag |= base.UpdateMarker > childUpdateMarker;
		bool flag2 = HasThisObjectChangedSinceLastUpdate();
		if (_lastProcessedMark == CSApplicationHelper.Application.Document.Graph.UpdateCount)
		{
			flag = flag || flag2;
		}
		else
		{
			_lastProcessedMark = CSApplicationHelper.Application.Document.Graph.UpdateCount;
			bool haveMyParentsChanged = false;
			InputChannels.Where((CSInputChannel inputChannel) => inputChannel.OutputChannelBlock != null).ToList().ForEach(delegate(CSInputChannel inputChannel)
			{
				if (inputChannel.OutputChannelBlock != null)
				{
					haveMyParentsChanged |= inputChannel.OutputChannelBlock.UpdateStatusForProcessing(base.UpdateMarker);
				}
			});
			if (haveMyParentsChanged && !flag2)
			{
				Change();
				flag2 = true;
			}
			flag = flag || flag2;
		}
		ProcessingStatus = ((!flag) ? BlockProcessingStatus.Processed : BlockProcessingStatus.NeedsProcessing);
		return flag;
	}
}
