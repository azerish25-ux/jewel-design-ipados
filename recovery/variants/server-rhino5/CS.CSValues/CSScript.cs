using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Microsoft.CSharp;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues;

[CSBlock("Script", "{ADC50346-3B56-4103-A630-1A88A50286FF}")]
public sealed class CSScript : CSValueBase
{
	private List<ScriptIO> _inputs = new List<ScriptIO>();

	private List<ScriptIO> _outputs = new List<ScriptIO>();

	private string _script;

	private object _scriptObject;

	public List<KeyValuePair<int, string>> Errors { get; } = new List<KeyValuePair<int, string>>();

	public List<CompilerError> CompilerErrors { get; } = new List<CompilerError>();

	public string Script => _script;

	public CSScript(string identifier)
		: base(identifier)
	{
		_script = string.Empty;
		_scriptObject = null;
	}

	public void AddInput(string name, ScriptVarType varType, CSOutputChannel csOutputChannel)
	{
		ScriptIO scriptIO = new ScriptIO(name, varType);
		AddInput(scriptIO, csOutputChannel);
	}

	public void AddInput(string name, ScriptVarType varType)
	{
		ScriptIO scriptIO = new ScriptIO(name, varType);
		AddInput(scriptIO);
	}

	public CSOutputChannel AddOutput(string name, ScriptVarType varType)
	{
		ScriptIO scriptIO = new ScriptIO(name, varType);
		return AddOutput(scriptIO);
	}

	public IEnumerable<CSInputChannel> GetInputChannels()
	{
		return new List<CSInputChannel>(base.InputChannels);
	}

	public List<ScriptIO> GetInputs()
	{
		return _inputs;
	}

	public static IEnumerable<CSInputChannel> GetLinks(CSOutputChannel outputChannel)
	{
		if (!GetChannelParentInfo(outputChannel, out var outputChannelParent, out var outputChannelIndex))
		{
			return new List<CSInputChannel>();
		}
		return GetLinks(outputChannelParent, outputChannelIndex);
	}

	public IEnumerable<CSOutputChannel> GetOutputChannels()
	{
		return new List<CSOutputChannel>(base.OutputChannels);
	}

	public List<ScriptIO> GetOutputs()
	{
		return _outputs;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		_script = string.Empty;
		archive.ReadString(ref _script);
		_scriptObject = null;
		base.InputChannels.Clear();
		int num = 0;
		archive.ReadInt(ref num);
		_inputs.Clear();
		for (int i = 0; i < num; i++)
		{
			string name = null;
			archive.ReadString(ref name);
			int varType = 0;
			archive.ReadInt(ref varType);
			ScriptIO scriptIO = new ScriptIO(name, (ScriptVarType)varType);
			bool flag = false;
			double num2 = 0.0;
			int num3 = 0;
			string empty = string.Empty;
			switch (scriptIO.Type)
			{
			case ScriptVarType.vDouble:
				archive.ReadDouble(ref num2);
				break;
			case ScriptVarType.vInt:
				archive.ReadInt(ref num3);
				break;
			case ScriptVarType.vBool:
				archive.ReadBool(ref flag);
				break;
			case ScriptVarType.vString:
				archive.ReadString(ref empty);
				break;
			case ScriptVarType.vListDouble:
				archive.ReadString(ref empty);
				break;
			case ScriptVarType.vListInt:
				archive.ReadString(ref empty);
				break;
			case ScriptVarType.vListBool:
				archive.ReadString(ref empty);
				break;
			case ScriptVarType.vListString:
				archive.ReadString(ref empty);
				break;
			}
			_inputs.Add(scriptIO);
			new CSInputChannel(scriptIO.Name + ScriptVariableTypeString(scriptIO.Type), this, GetChannelDataType(scriptIO.Type), ChannelArrayType.Any, ChannelFlag.NotRequired);
		}
		base.OutputChannels.Clear();
		int num4 = 0;
		archive.ReadInt(ref num4);
		_outputs.Clear();
		for (int j = 0; j < num4; j++)
		{
			string name2 = null;
			archive.ReadString(ref name2);
			int varType2 = 0;
			archive.ReadInt(ref varType2);
			ScriptIO scriptIO2 = new ScriptIO(name2, (ScriptVarType)varType2);
			_outputs.Add(scriptIO2);
			new CSOutputChannel(scriptIO2.Name + ScriptVariableTypeString(scriptIO2.Type), this, GetChannelDataType(scriptIO2.Type), ChannelArrayType.Any);
		}
		base.Read(ref archive, graph, major, minor);
		return true;
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		bool flag = true;
		flag &= archive.ReadJsonValue<string>("Script", out _script);
		_scriptObject = null;
		flag &= archive.ReadJsonObject<List<ScriptIO>>("Inputs", out _inputs);
		flag &= archive.ReadJsonObject<List<ScriptIO>>("Outputs", out _outputs);
		base.InputChannels.Clear();
		foreach (ScriptIO input in _inputs)
		{
			new CSInputChannel(input.Name + ScriptVariableTypeString(input.Type), this, GetChannelDataType(input.Type), ChannelArrayType.Any, ChannelFlag.NotRequired);
		}
		base.OutputChannels.Clear();
		foreach (ScriptIO output in _outputs)
		{
			new CSOutputChannel(output.Name + ScriptVariableTypeString(output.Type), this, GetChannelDataType(output.Type), ChannelArrayType.Any);
		}
		return flag & base.ReadJson(archive, graph, major, minor);
	}

	public void RemoveInput(ScriptIO sIO)
	{
		ScriptIO scriptIO = _inputs.FirstOrDefault((ScriptIO input) => input.Name == sIO.Name && input.Type == sIO.Type);
		if (scriptIO != null)
		{
			RemoveInputChannel(GetChannelIdentifier(sIO));
			_inputs.Remove(scriptIO);
			_scriptObject = null;
		}
	}

	public void RemoveOutput(ScriptIO sIO)
	{
		ScriptIO scriptIO = _outputs.FirstOrDefault((ScriptIO output) => output.Name == sIO.Name && output.Type == sIO.Type);
		if (scriptIO != null)
		{
			RemoveOutputChannel(GetChannelIdentifier(sIO));
			_outputs.Remove(scriptIO);
			_scriptObject = null;
		}
	}

	public void SetInputChannels(IEnumerable<CSInputChannel> inputChannels)
	{
		base.InputChannels.Clear();
		foreach (CSInputChannel inputChannel in inputChannels)
		{
			base.InputChannels.Add(inputChannel);
		}
	}

	public void SetInputs(IEnumerable<ScriptIO> scriptIos)
	{
		_inputs.Clear();
		_inputs.AddRange(scriptIos);
	}

	public void SetOutputChannels(IEnumerable<CSOutputChannel> outputChannels)
	{
		base.OutputChannels.Clear();
		foreach (CSOutputChannel outputChannel in outputChannels)
		{
			base.OutputChannels.Add(outputChannel);
		}
	}

	public void SetOutputs(IEnumerable<ScriptIO> scriptIos)
	{
		_outputs.Clear();
		_outputs.AddRange(scriptIos);
	}

	public void SetScript(string script)
	{
		_script = script;
		_scriptObject = null;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (!(setObject is CSScript cSScript))
		{
			return true;
		}
		_script = cSScript._script;
		_scriptObject = null;
		bool flag = true;
		if (showRewirePrompt)
		{
			flag = TopMostMessageBox.Show($"The custom settings you are importing will modify the custom script named {base.Identifier} in this patchwire.  Do you want to retain the wiring of the custom script?", "Custom Script Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
		}
		List<ScriptIO> list = new List<ScriptIO>(_inputs);
		List<ScriptIO> list2 = new List<ScriptIO>(_outputs);
		List<Tuple<CSBlock, int>> list3 = base.InputChannels.Select((CSInputChannel csInputChannel) => new Tuple<CSBlock, int>(csInputChannel.OutputChannelBlock, csInputChannel.OutputChannelIndex)).ToList();
		List<IEnumerable<CSInputChannel>> list4 = base.OutputChannels.Select(GetLinks).ToList();
		foreach (CSInputChannel inputChannel in base.InputChannels)
		{
			inputChannel.BreakLink();
		}
		base.InputChannels.Clear();
		foreach (CSOutputChannel outputChannel in base.OutputChannels)
		{
			BreakLinks(outputChannel);
		}
		base.OutputChannels.Clear();
		SetInputs(cSScript._inputs);
		SetOutputs(cSScript._outputs);
		if (flag)
		{
			if (list.Count > _inputs.Count)
			{
				for (int num = _inputs.Count; num < list.Count; num++)
				{
					_inputs.Add(new ScriptIO($"UnknownInput{num}_{list[num].Name})", list[num].Type));
				}
			}
			if (list2.Count > _outputs.Count)
			{
				for (int num2 = _outputs.Count; num2 < list2.Count; num2++)
				{
					_outputs.Add(new ScriptIO($"UnknownOutput{num2}_{list2[num2].Name}", list2[num2].Type));
				}
			}
		}
		int count = _inputs.Count;
		for (int num3 = 0; num3 < count; num3++)
		{
			ScriptIO scriptIO = _inputs[num3];
			new CSInputChannel(scriptIO.Name + ScriptVariableTypeString(scriptIO.Type), this, GetChannelDataType(scriptIO.Type), ChannelArrayType.Any, ChannelFlag.NotRequired);
		}
		int count2 = _outputs.Count;
		for (int num4 = 0; num4 < count2; num4++)
		{
			ScriptIO scriptIO2 = _outputs[num4];
			new CSOutputChannel(scriptIO2.Name + ScriptVariableTypeString(scriptIO2.Type), this, GetChannelDataType(scriptIO2.Type), ChannelArrayType.Any);
		}
		if (flag)
		{
			for (int num5 = 0; num5 < list3.Count; num5++)
			{
				Tuple<CSBlock, int> tuple = list3[num5];
				base.InputChannels[num5].SetLink(tuple.Item1, tuple.Item2);
			}
			for (int num6 = 0; num6 < list4.Count; num6++)
			{
				foreach (CSInputChannel item in list4[num6])
				{
					item.SetLink(this, num6);
				}
			}
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		if (!(setObject is CSScript cSScript))
		{
			return base.SetUndo(setObject);
		}
		SetInputs((from _ in cSScript.GetInputs()
			select _.Clone()).Cast<ScriptIO>());
		SetOutputs((from _ in cSScript.GetOutputs()
			select _.Clone()).Cast<ScriptIO>());
		_script = cSScript.Script;
		return base.SetUndo(setObject);
	}

	public override bool Update()
	{
		if (!BuildAndCompileScript(out var inCount, out var outCount))
		{
			return UpdateError("Script contains errors!");
		}
		object[] array = new object[inCount + outCount];
		for (int i = 0; i < inCount - 1; i++)
		{
			switch (_inputs[i].Type)
			{
			case ScriptVarType.vDouble:
			{
				double value5 = 0.0;
				base.InputChannels[i].GetDouble(ref value5);
				value5 = Math.Round(value5, 3);
				array[i] = value5;
				_inputs[i].Value = value5;
				break;
			}
			case ScriptVarType.vInt:
			{
				double value3 = 0.0;
				base.InputChannels[i].GetDouble(ref value3);
				array[i] = (int)Math.Round(value3, 0);
				_inputs[i].Value = (int)Math.Round(value3, 0);
				break;
			}
			case ScriptVarType.vBool:
			{
				double value = 0.0;
				base.InputChannels[i].GetDouble(ref value);
				array[i] = value > 0.5;
				_inputs[i].Value = value > 0.5;
				break;
			}
			case ScriptVarType.vString:
			{
				string output = string.Empty;
				base.InputChannels[i].GetString(ref output);
				array[i] = output;
				_inputs[i].Value = output;
				break;
			}
			case ScriptVarType.vObject:
				array[i] = base.InputChannels[i].Data;
				_inputs[i].Value = array[i];
				break;
			case ScriptVarType.vListDouble:
			{
				CSDataArray flattenArray3 = base.InputChannels[i].GetFlattenArray();
				List<double> list3 = new List<double>();
				for (int l = 0; l < flattenArray3.GetArrayCount(); l++)
				{
					double value2 = 0.0;
					flattenArray3.Array[l].GetDouble(ref value2);
					list3.Add(value2);
				}
				array[i] = list3;
				_inputs[i].Value = string.Join(",", list3);
				break;
			}
			case ScriptVarType.vListInt:
			{
				CSDataArray flattenArray5 = base.InputChannels[i].GetFlattenArray();
				List<int> list5 = new List<int>();
				for (int n = 0; n < flattenArray5.GetArrayCount(); n++)
				{
					int value6 = 0;
					flattenArray5.Array[n].GetInteger(ref value6);
					list5.Add(value6);
				}
				array[i] = list5;
				_inputs[i].Value = string.Join(",", list5);
				break;
			}
			case ScriptVarType.vListBool:
			{
				CSDataArray flattenArray4 = base.InputChannels[i].GetFlattenArray();
				List<bool> list4 = new List<bool>();
				for (int m = 0; m < flattenArray4.GetArrayCount(); m++)
				{
					bool value4 = false;
					flattenArray4.Array[m].GetBoolean(ref value4);
					list4.Add(value4);
				}
				array[i] = list4;
				_inputs[i].Value = string.Join(",", list4);
				break;
			}
			case ScriptVarType.vListString:
			{
				CSDataArray flattenArray2 = base.InputChannels[i].GetFlattenArray();
				List<string> list2 = new List<string>();
				for (int k = 0; k < flattenArray2.GetArrayCount(); k++)
				{
					string output2 = string.Empty;
					flattenArray2.Array[k].GetString(ref output2);
					list2.Add(output2);
				}
				array[i] = list2;
				_inputs[i].Value = string.Join(",", list2);
				break;
			}
			case ScriptVarType.vListObject:
			{
				CSDataArray flattenArray = base.InputChannels[i].GetFlattenArray();
				List<object> list = new List<object>();
				for (int j = 0; j < flattenArray.GetArrayCount(); j++)
				{
					list.Add(flattenArray.Array[j]);
				}
				array[i] = list;
				_inputs[i].Value = flattenArray.GetArrayCount() + " Objects";
				break;
			}
			}
			array[inCount - 1] = CSApplicationHelper.Application.IsCamMode;
		}
		for (int num = 0; num < outCount; num++)
		{
			switch (_outputs[num].Type)
			{
			case ScriptVarType.vDouble:
				array[num + inCount] = 0.0;
				_outputs[num].Value = 0.0;
				break;
			case ScriptVarType.vInt:
				array[num + inCount] = 0;
				_outputs[num].Value = 0;
				break;
			case ScriptVarType.vBool:
				array[num + inCount] = false;
				_outputs[num].Value = false;
				break;
			case ScriptVarType.vString:
			{
				string value7 = (string)(array[num + inCount] = string.Empty);
				_outputs[num].Value = value7;
				break;
			}
			case ScriptVarType.vObject:
				array[num + inCount] = null;
				_outputs[num].Value = null;
				break;
			case ScriptVarType.vListDouble:
				array[num + inCount] = new List<double>();
				_outputs[num].Value = string.Empty;
				break;
			case ScriptVarType.vListInt:
				array[num + inCount] = new List<int>();
				_outputs[num].Value = string.Empty;
				break;
			case ScriptVarType.vListBool:
				array[num + inCount] = new List<bool>();
				_outputs[num].Value = string.Empty;
				break;
			case ScriptVarType.vListString:
				array[num + inCount] = new List<string>();
				_outputs[num].Value = string.Empty;
				break;
			case ScriptVarType.vListObject:
				array[num + inCount] = new List<object>();
				_outputs[num].Value = string.Empty;
				break;
			}
		}
		try
		{
			_scriptObject.GetType().GetMethod("Main").Invoke(_scriptObject, array);
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			if (ex.InnerException != null)
			{
				message = ex.InnerException.Message;
			}
			Errors.Add(new KeyValuePair<int, string>(0, message));
			CompilerErrors.Add(new CompilerError
			{
				Column = 0,
				ErrorNumber = "Runtime Error",
				ErrorText = message
			});
			return UpdateError(message);
		}
		for (int num2 = 0; num2 < outCount; num2++)
		{
			switch (_outputs[num2].Type)
			{
			case ScriptVarType.vDouble:
				base.OutputChannels[num2].SetData(new CSNumberData((double)array[num2 + inCount]));
				_outputs[num2].Value = (double)array[num2 + inCount];
				break;
			case ScriptVarType.vInt:
				base.OutputChannels[num2].SetData(new CSNumberData((int)array[num2 + inCount]));
				_outputs[num2].Value = (int)array[num2 + inCount];
				break;
			case ScriptVarType.vBool:
			{
				double num3 = (((bool)array[num2 + inCount]) ? 1.0 : 0.0);
				base.OutputChannels[num2].SetData(new CSNumberData(num3));
				_outputs[num2].Value = num3;
				break;
			}
			case ScriptVarType.vString:
				base.OutputChannels[num2].SetData(new CSStringData((string)array[num2 + inCount]));
				_outputs[num2].Value = array[num2 + inCount];
				break;
			case ScriptVarType.vObject:
				base.OutputChannels[num2].SetData((CSData)array[num2 + inCount]);
				_outputs[num2].Value = array[num2 + inCount];
				break;
			case ScriptVarType.vListDouble:
			{
				CSDataArray cSDataArray5 = new CSDataArray();
				List<double> list10 = (List<double>)array[num2 + inCount];
				foreach (double item in list10)
				{
					cSDataArray5.Array.Add(new CSNumberData(item));
				}
				base.OutputChannels[num2].SetData(cSDataArray5);
				_outputs[num2].Value = string.Join(",", list10);
				break;
			}
			case ScriptVarType.vListInt:
			{
				CSDataArray cSDataArray4 = new CSDataArray();
				List<int> list9 = (List<int>)array[num2 + inCount];
				foreach (int item2 in list9)
				{
					cSDataArray4.Array.Add(new CSNumberData(item2));
				}
				base.OutputChannels[num2].SetData(cSDataArray4);
				_outputs[num2].Value = string.Join(",", list9);
				break;
			}
			case ScriptVarType.vListBool:
			{
				CSDataArray cSDataArray3 = new CSDataArray();
				List<bool> list8 = (List<bool>)array[num2 + inCount];
				foreach (bool item3 in list8)
				{
					cSDataArray3.Array.Add(new CSNumberData(item3 ? 1 : 0));
				}
				base.OutputChannels[num2].SetData(cSDataArray3);
				_outputs[num2].Value = string.Join(",", list8);
				break;
			}
			case ScriptVarType.vListString:
			{
				CSDataArray cSDataArray2 = new CSDataArray();
				List<string> list7 = (List<string>)array[num2 + inCount];
				foreach (string item4 in list7)
				{
					cSDataArray2.Array.Add(new CSStringData(item4));
				}
				base.OutputChannels[num2].SetData(cSDataArray2);
				_outputs[num2].Value = string.Join(",", list7);
				break;
			}
			case ScriptVarType.vListObject:
			{
				CSDataArray cSDataArray = new CSDataArray();
				List<object> list6 = (List<object>)array[num2 + inCount];
				foreach (object item5 in list6)
				{
					cSDataArray.Array.Add((CSData)item5);
				}
				base.OutputChannels[num2].SetData(cSDataArray);
				_outputs[num2].Value = list6.Count + "Objects";
				break;
			}
			}
		}
		return base.Update();
	}

	public bool UpdateInput(ScriptIO prevScriptIO, ScriptIO newScriptIO)
	{
		int num = _inputs.IndexOf(prevScriptIO);
		if (num < 0)
		{
			return false;
		}
		int num2 = base.InputChannels.FindIndex((CSInputChannel _) => _.Identifier == GetChannelIdentifier(prevScriptIO));
		if (num2 < 0)
		{
			return false;
		}
		int outputChannelIndex = base.InputChannels[num2].OutputChannelIndex;
		CSBlock outputChannelBlock = base.InputChannels[num2].OutputChannelBlock;
		_scriptObject = null;
		_inputs[num] = newScriptIO;
		base.InputChannels[num2].BreakLink();
		base.InputChannels[num2] = new CSInputChannel(GetChannelIdentifier(newScriptIO), null, GetChannelDataType(newScriptIO.Type), GetChannelArrayType(newScriptIO.Type), ChannelFlag.NotRequired);
		base.InputChannels[num2].SetParent(this);
		base.InputChannels[num2].SetLink(outputChannelBlock, outputChannelIndex);
		return true;
	}

	public bool UpdateOutput(ScriptIO prevScriptIO, ScriptIO newScriptIO)
	{
		int num = _outputs.IndexOf(prevScriptIO);
		if (num < 0)
		{
			return false;
		}
		int num2 = base.OutputChannels.FindIndex((CSOutputChannel _) => _.Identifier == GetChannelIdentifier(prevScriptIO));
		if (num2 < 0)
		{
			return false;
		}
		_scriptObject = null;
		_outputs[num] = newScriptIO;
		base.OutputChannels[num2] = new CSOutputChannel(GetChannelIdentifier(newScriptIO), null, GetChannelDataType(newScriptIO.Type), GetChannelArrayType(newScriptIO.Type));
		base.OutputChannels[num2].SetParent(this);
		return true;
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		archive.WriteString(_script ?? string.Empty);
		archive.WriteInt(_inputs.Count);
		foreach (ScriptIO input in _inputs)
		{
			archive.WriteString(input.Name ?? string.Empty);
			archive.WriteInt((int)input.Type);
			switch (input.Type)
			{
			case ScriptVarType.vDouble:
				archive.WriteDouble(0.0);
				break;
			case ScriptVarType.vInt:
				archive.WriteInt(0);
				break;
			case ScriptVarType.vBool:
				archive.WriteBool(false);
				break;
			case ScriptVarType.vString:
				archive.WriteString(string.Empty);
				break;
			case ScriptVarType.vListDouble:
				archive.WriteString(string.Empty);
				break;
			case ScriptVarType.vListInt:
				archive.WriteString(string.Empty);
				break;
			case ScriptVarType.vListBool:
				archive.WriteString(string.Empty);
				break;
			case ScriptVarType.vListString:
				archive.WriteString(string.Empty);
				break;
			}
		}
		archive.WriteInt(_outputs.Count);
		foreach (ScriptIO output in _outputs)
		{
			archive.WriteString(output.Name ?? string.Empty);
			archive.WriteInt((int)output.Type);
		}
		base.Write(ref archive);
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		archive.Add("Script", _script ?? string.Empty);
		archive.Add("Inputs", JArray.FromObject(_inputs));
		archive.Add("Outputs", JArray.FromObject(_outputs));
		base.WriteJson(ref archive);
	}

	private CSInputChannel AddInput(ScriptIO scriptIO, CSOutputChannel csOutputChannel)
	{
		CSInputChannel cSInputChannel = AddInput(scriptIO);
		cSInputChannel.SetLink(csOutputChannel);
		return cSInputChannel;
	}

	private CSInputChannel AddInput(ScriptIO scriptIO)
	{
		_scriptObject = null;
		_inputs.Add(scriptIO);
		return new CSInputChannel(GetChannelIdentifier(scriptIO), this, GetChannelDataType(scriptIO.Type), GetChannelArrayType(scriptIO.Type), ChannelFlag.NotRequired);
	}

	private CSOutputChannel AddOutput(ScriptIO scriptIO)
	{
		CSOutputChannel result = new CSOutputChannel(GetChannelIdentifier(scriptIO), this, GetChannelDataType(scriptIO.Type), GetChannelArrayType(scriptIO.Type));
		_outputs.Add(scriptIO);
		_scriptObject = null;
		return result;
	}

	private static void BreakLinks(CSOutputChannel outputChannel)
	{
		foreach (CSInputChannel link in GetLinks(outputChannel))
		{
			link.BreakLink();
		}
	}

	private bool BuildAndCompileScript(out int inCount, out int outCount)
	{
		base.Error = false;
		Message = null;
		Errors.Clear();
		CompilerErrors.Clear();
		inCount = _inputs.Count;
		outCount = _outputs.Count;
		if (_scriptObject != null)
		{
			inCount++;
			return true;
		}
		CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });
		CompilerParameters compilerParameters = new CompilerParameters
		{
			GenerateExecutable = false,
			GenerateInMemory = true,
			IncludeDebugInformation = true
		};
		compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
		List<string> list = new List<string>();
		for (int i = 0; i < inCount; i++)
		{
			switch (_inputs[i].Type)
			{
			case ScriptVarType.vDouble:
				list.Add("double " + _inputs[i].Name);
				break;
			case ScriptVarType.vInt:
				list.Add("int " + _inputs[i].Name);
				break;
			case ScriptVarType.vBool:
				list.Add("bool " + _inputs[i].Name);
				break;
			case ScriptVarType.vString:
				list.Add("string " + _inputs[i].Name);
				break;
			case ScriptVarType.vObject:
				list.Add("object " + _inputs[i].Name);
				break;
			case ScriptVarType.vListDouble:
				list.Add("List<double> " + _inputs[i].Name);
				break;
			case ScriptVarType.vListInt:
				list.Add("List<int> " + _inputs[i].Name);
				break;
			case ScriptVarType.vListBool:
				list.Add("List<bool> " + _inputs[i].Name);
				break;
			case ScriptVarType.vListString:
				list.Add("List<string> " + _inputs[i].Name);
				break;
			case ScriptVarType.vListObject:
				list.Add("List<Object> " + _inputs[i].Name);
				break;
			}
		}
		inCount++;
		list.Add("bool camMode");
		for (int j = 0; j < outCount; j++)
		{
			switch (_outputs[j].Type)
			{
			case ScriptVarType.vDouble:
				list.Add("ref double " + _outputs[j].Name);
				break;
			case ScriptVarType.vInt:
				list.Add("ref int " + _outputs[j].Name);
				break;
			case ScriptVarType.vBool:
				list.Add("ref bool " + _outputs[j].Name);
				break;
			case ScriptVarType.vString:
				list.Add("ref string " + _outputs[j].Name);
				break;
			case ScriptVarType.vObject:
				list.Add("ref object " + _outputs[j].Name);
				break;
			case ScriptVarType.vListDouble:
				list.Add("ref List<double> " + _outputs[j].Name);
				break;
			case ScriptVarType.vListInt:
				list.Add("ref List<int> " + _outputs[j].Name);
				break;
			case ScriptVarType.vListBool:
				list.Add("ref List<bool> " + _outputs[j].Name);
				break;
			case ScriptVarType.vListString:
				list.Add("ref List<string> " + _outputs[j].Name);
				break;
			case ScriptVarType.vListObject:
				list.Add("ref List<object> " + _outputs[j].Name);
				break;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("using System;");
		stringBuilder.AppendLine("using System.Collections.Generic;");
		stringBuilder.AppendLine("using System.Diagnostics;");
		stringBuilder.AppendLine("using System.Linq;");
		stringBuilder.AppendLine("using System.IO;");
		stringBuilder.AppendLine("using System.Text;");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("class ScriptClass");
		stringBuilder.AppendLine("{");
		stringBuilder.AppendLine(string.Format("public void Main({0})", string.Join(", ", list)));
		int num = 10;
		_script = (string.IsNullOrWhiteSpace(_script) ? string.Empty : _script);
		bool num2 = _script.TrimStart().StartsWith("{");
		if (!num2)
		{
			stringBuilder.AppendLine("{");
			num++;
		}
		stringBuilder.AppendLine(_script);
		if (!num2)
		{
			stringBuilder.AppendLine("}");
		}
		stringBuilder.AppendLine("}");
		string text = stringBuilder.ToString();
		CompilerResults compilerResults = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, text);
		if (compilerResults.Errors.Count > 0)
		{
			foreach (CompilerError error in compilerResults.Errors)
			{
				error.Column--;
				error.Line -= num;
				Errors.Add(new KeyValuePair<int, string>(error.Line, error.ErrorText));
				CompilerErrors.Add(error);
			}
			return false;
		}
		_scriptObject = compilerResults.CompiledAssembly.CreateInstance("ScriptClass");
		return true;
	}

	private static ChannelArrayType GetChannelArrayType(ScriptVarType whichScriptVarType)
	{
		switch (whichScriptVarType)
		{
		case ScriptVarType.vDouble:
		case ScriptVarType.vInt:
		case ScriptVarType.vBool:
		case ScriptVarType.vString:
			return ChannelArrayType.Individual;
		case ScriptVarType.vListDouble:
		case ScriptVarType.vListInt:
		case ScriptVarType.vListBool:
		case ScriptVarType.vListString:
			return ChannelArrayType.Array;
		case ScriptVarType.vObject:
		case ScriptVarType.vListObject:
			return ChannelArrayType.Any;
		default:
			return ChannelArrayType.Any;
		}
	}

	private static ChannelDataType GetChannelDataType(ScriptVarType whichScriptVarType)
	{
		switch (whichScriptVarType)
		{
		case ScriptVarType.vDouble:
		case ScriptVarType.vListDouble:
			return ChannelDataType.Double;
		case ScriptVarType.vInt:
		case ScriptVarType.vListInt:
			return ChannelDataType.Integer;
		case ScriptVarType.vBool:
		case ScriptVarType.vListBool:
			return ChannelDataType.Boolean;
		case ScriptVarType.vString:
		case ScriptVarType.vListString:
			return ChannelDataType.String;
		case ScriptVarType.vObject:
		case ScriptVarType.vListObject:
			return ChannelDataType.Any;
		default:
			return ChannelDataType.Any;
		}
	}

	private static string GetChannelIdentifier(ScriptIO scriptIO)
	{
		return scriptIO.Name + ScriptVariableTypeString(scriptIO.Type);
	}

	private static bool GetChannelParentInfo(CSOutputChannel outputChannel, out CSBlock outputChannelParent, out int outputChannelIndex)
	{
		outputChannelIndex = -1;
		outputChannelParent = outputChannel.Parent as CSBlock;
		if (outputChannelParent == null)
		{
			return false;
		}
		outputChannelIndex = outputChannelParent.OutputChannels.IndexOf(outputChannel);
		if (outputChannelIndex < 0)
		{
			return false;
		}
		return true;
	}

	private static IEnumerable<CSInputChannel> GetLinks(CSBlock outputChannelParent, int outputChannelIndex)
	{
		List<CSInputChannel> list = new List<CSInputChannel>();
		foreach (CSBlock cSBlock in CSApplicationHelper.Application.Document.Graph.CSBlocks)
		{
			list.AddRange(cSBlock.InputChannels.Where((CSInputChannel csInputChannel) => csInputChannel.HasLink && csInputChannel.OutputChannelBlock == outputChannelParent && csInputChannel.OutputChannelIndex == outputChannelIndex));
		}
		return list;
	}

	private void RemoveOutputChannel(string findIdentifier)
	{
		for (int i = 0; i < base.OutputChannels.Count; i++)
		{
			if (!(base.OutputChannels[i].Identifier != findIdentifier))
			{
				BreakLinks(base.OutputChannels[i]);
				base.OutputChannels.RemoveAt(i);
				break;
			}
		}
	}

	private static string ScriptVariableTypeString(ScriptVarType whichScriptVariableType)
	{
		return whichScriptVariableType switch
		{
			ScriptVarType.vDouble => "_double", 
			ScriptVarType.vInt => "_int", 
			ScriptVarType.vBool => "_bool", 
			ScriptVarType.vString => "_string", 
			ScriptVarType.vObject => "_object", 
			ScriptVarType.vListDouble => "_listDouble", 
			ScriptVarType.vListInt => "_listInt", 
			ScriptVarType.vListBool => "_listBool", 
			ScriptVarType.vListString => "_listString", 
			ScriptVarType.vListObject => "_listObject", 
			_ => "_error", 
		};
	}
}
