using System;
using Newtonsoft.Json;

namespace CS.CSValues;

public sealed class ScriptIO : ICloneable
{
	public string Name;

	public ScriptVarType Type;

	[JsonIgnore]
	public object Value;

	public ScriptIO(string name, ScriptVarType varType)
	{
		Name = name;
		Type = varType;
	}

	public object Clone()
	{
		return new ScriptIO(Name, Type)
		{
			Value = Value
		};
	}
}
