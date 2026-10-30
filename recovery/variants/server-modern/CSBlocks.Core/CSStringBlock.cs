using System.Collections.ObjectModel;
using CS;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CSBlocks.Core;

[CSBlock("StringBlock", "{3EF18121-9B39-4036-BD0A-F9E2F8C4BBEE}")]
public sealed class CSStringBlock : CSBlock
{
	private readonly Collection<string> _strings = new Collection<string>();

	public CSOutputChannel oString { get; }

	public CSStringBlock(string identifier)
		: base(identifier)
	{
		_strings.Add("Text");
		oString = new CSOutputChannel(GetPropertyName(() => oString), this, ChannelDataType.String, ChannelArrayType.Individual);
	}

	public string GetString()
	{
		if (_strings.Count > 0)
		{
			return _strings[0];
		}
		return null;
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		bool result = base.ReadJson(jBlock, graph, major, minor);
		if (!jBlock.ContainsKey("string"))
		{
			return false;
		}
		Collection<string> collection = jBlock["string"].ToObject<Collection<string>>();
		_strings.Clear();
		foreach (string item in collection)
		{
			_strings.Add(item);
		}
		return result;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		_strings.Clear();
		string[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		if (items == null)
		{
			return true;
		}
		string[] array = items;
		foreach (string item in array)
		{
			_strings.Add(item);
		}
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		_strings.Clear();
		if (setObject is CSStringBlock cSStringBlock)
		{
			for (int i = 0; i < cSStringBlock._strings.Count; i++)
			{
				_strings.Add(cSStringBlock._strings[i]);
			}
		}
		return base.SetFileValue(setObject);
	}

	public void SetString(string value)
	{
		if (_strings.Count < 1)
		{
			_strings.Add(value);
			Change();
		}
		else if (!(_strings[0] == value))
		{
			_strings[0] = value;
			Change();
		}
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		_strings.Clear();
		if (setObject is CSStringBlock cSStringBlock)
		{
			for (int i = 0; i < cSStringBlock._strings.Count; i++)
			{
				_strings.Add(cSStringBlock._strings[i]);
			}
		}
		return base.SetStyle(setObject, showRewirePrompt);
	}

	public override bool SetUndo(CSObject setObject)
	{
		_strings.Clear();
		if (setObject is CSStringBlock cSStringBlock)
		{
			for (int i = 0; i < cSStringBlock._strings.Count; i++)
			{
				_strings.Add(cSStringBlock._strings[i]);
			}
		}
		return base.SetUndo(setObject);
	}

	public void SetValueFromUI(string value)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetValueFromUI");
		SetString(value);
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetValueFromUI");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
	}

	public override bool Update()
	{
		base.OutputChannels.Clear();
		for (int i = 0; i < _strings.Count; i++)
		{
			new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.String, ChannelArrayType.Individual).SetData(new CSStringData(_strings[i]));
		}
		return base.Update();
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		string[] array = new string[_strings.Count];
		for (int i = 0; i < _strings.Count; i++)
		{
			array[i] = _strings[i];
		}
		CSUtilIO.WriteArray(array, ref archive);
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("string", new JArray(_strings));
	}
}
