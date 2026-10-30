using System;
using CS.JSonDoc;
using CS.Parametric;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSObjects.CSUserInterfaceFlags;

[Obsolete]
[CSBlock("UserInterfaceFlagsBegin", "{E4FBD742-C4A2-4220-866D-12D10AB0CAF1}")]
public sealed class CSUserInterfaceFlagsBegin : CSObject
{
	[Flags]
	public enum UIFlags
	{
		None = 0,
		DualSlider = 1
	}

	private uint _uiFlags;

	public CSUserInterfaceFlagsBegin(string ident, UIFlags flag)
		: base(ident)
	{
		_uiFlags = (uint)flag;
	}

	public CSUserInterfaceFlagsBegin(string ident)
		: base(ident)
	{
	}

	public UIFlags GetCurrentFlags()
	{
		if (_uiFlags == 0)
		{
			return UIFlags.None;
		}
		if (_uiFlags == 1)
		{
			return UIFlags.DualSlider;
		}
		return UIFlags.None;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		bool result = base.Read(ref archive, graph, major, minor);
		archive.ReadInt(ref _uiFlags);
		return result;
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		return base.ReadJson(archive, graph, major, minor) & archive.ReadJsonValue<uint>("uiFlags", out _uiFlags);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		bool num = base.Write(ref archive);
		if (num)
		{
			archive.WriteInt(_uiFlags);
		}
		return num;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("uiFlags", _uiFlags);
	}
}
