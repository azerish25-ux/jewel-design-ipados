using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace Geomagic.Utilities;

public class ArgParser
{
	private const int DEFAULT_NUM_PARAMETERS = -1;

	private const object DEFAULT_VALUE = null;

	private const string DEFAULT_GROUP = null;

	private Dictionary<string, ArgDefinition> _Map = new Dictionary<string, ArgDefinition>();

	private Dictionary<string, ArgGroup> _GroupMap = new Dictionary<string, ArgGroup>();

	protected Dictionary<string, ArgDefinition> Map => _Map;

	protected Dictionary<string, ArgGroup> GroupMap => _GroupMap;

	[DefaultValue(null)]
	public string Description { get; private set; }

	[DefaultValue(null)]
	public string Help { get; private set; }

	[DefaultValue(null)]
	public string Name { get; private set; }

	[DefaultValue(null)]
	public string Usage { get; private set; }

	[DefaultValue(null)]
	public string ErrorMessage { get; private set; }

	[DefaultValue(false)]
	public bool IgnoreCase { get; set; }

	[DefaultValue(false)]
	public bool Strict { get; set; }

	public ArgParser()
	{
		Name = Path.GetFileName(Assembly.GetEntryAssembly().Location);
	}

	public ArgParser(string Description)
		: this()
	{
		this.Description = Description;
	}

	public ArgParser(string Description, bool IgnoreCase)
		: this(Description)
	{
		this.IgnoreCase = IgnoreCase;
	}

	public void Add(string Indicator, string Name, string Help)
	{
		Add(Indicator, Name, typeof(bool), 0, false, Help);
	}

	public void Add(string Indicator, string Name, Type Type, string Help)
	{
		Add(Indicator, Name, Type, -1, null, Help);
	}

	public void Add(string Indicator, string Name, Type Type, int NumParameters, string Help)
	{
		Add(Indicator, Name, Type, NumParameters, null, Help);
	}

	public void Add(string Indicator, string Name, Type Type, int NumParameters, object DefaultValue, string Help)
	{
		Add(Indicator, Name, Type, NumParameters, DefaultValue, null, Help);
	}

	private void Add(string Indicator, string Name, Type Type, int NumParameters, object DefaultValue, string Group, string Help)
	{
		if (string.IsNullOrEmpty(Indicator))
		{
			throw new ArgumentNullException("Indicator");
		}
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		if (_Map.ContainsKey(Indicator))
		{
			throw new ArgumentException("An argument with the indicator " + Indicator + " already exists.");
		}
		if (_GroupMap.ContainsKey(Name))
		{
			throw new ArgumentException("A group with the name " + Name + " already exists.");
		}
		ArgDefinition argDefinition = new ArgDefinition();
		argDefinition.Indicator = Indicator;
		argDefinition.Name = Name;
		argDefinition.Type = Type;
		argDefinition.NumParameters = NumParameters;
		argDefinition.DefaultValue = DefaultValue;
		argDefinition.Group = Group;
		argDefinition.Help = Help;
		Add(argDefinition);
	}

	private void Add(ArgDefinition Definition)
	{
		string indicator = Definition.Indicator;
		_Map.Add(indicator, Definition);
	}

	public void AddGroup(string Name, Type Type, string Help)
	{
		AddGroup(Name, Type, 0, Help);
	}

	public void AddGroup(string Name, Type Type, int NumParameters, string Help)
	{
		AddGroup(Name, Type, NumParameters, null, Help);
	}

	public void AddGroup(string Name, Type Type, int NumParameters, object DefaultValue, string Help)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		if (_GroupMap.ContainsKey(Name))
		{
			throw new ArgumentException("A group with the name " + Name + " already exists.");
		}
		if (_GroupMap.ContainsKey(Name))
		{
			throw new ArgumentException("A group with the name " + Name + " already exists.");
		}
		ArgGroup argGroup = new ArgGroup();
		argGroup.Name = Name;
		argGroup.Type = Type;
		argGroup.NumParameters = NumParameters;
		argGroup.DefaultValue = DefaultValue;
		argGroup.Help = Help;
		Add(argGroup);
	}

	public void Add(string GroupName, string Indicator, string Name, object DefaultValue, string Help)
	{
		if (!_GroupMap.ContainsKey(GroupName))
		{
			throw new ArgumentException("Cannot find group: " + GroupName);
		}
		ArgGroup argGroup = _GroupMap[GroupName];
		Add(Indicator, Name, argGroup.Type, argGroup.NumParameters, DefaultValue, argGroup.Name, Help);
	}

	private void Add(ArgGroup Group)
	{
		string name = Group.Name;
		if (!_GroupMap.ContainsKey(name))
		{
			_GroupMap.Add(name, Group);
		}
	}

	private Dictionary<string, object> GetDefaultValues()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (ArgGroup value in GroupMap.Values)
		{
			dictionary.Add(value.Name, value.DefaultValue);
		}
		foreach (ArgDefinition value2 in Map.Values)
		{
			if (!value2.HasGroup)
			{
				dictionary.Add(value2.Name, value2.DefaultValue);
			}
		}
		return dictionary;
	}

	private int GetStartIndex(string[] Args)
	{
		int result = 0;
		if (Args == null || Args.Length == 0)
		{
			return result;
		}
		string location = Assembly.GetEntryAssembly().Location;
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(location);
		string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(Args[0]);
		return string.Equals(fileNameWithoutExtension2, fileNameWithoutExtension, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
	}

	private string GetHelpString()
	{
		string text = "arguments:" + Environment.NewLine;
		foreach (string key in Map.Keys)
		{
			string text2 = text;
			text = text2 + " " + key + " " + Map[key].Help + Environment.NewLine;
		}
		return text;
	}

	private string GetUsageString(string[] Args)
	{
		string text = "usage: " + Name;
		if (Args == null)
		{
			return text;
		}
		foreach (string key in Args)
		{
			if (Map.ContainsKey(key))
			{
				ArgDefinition argDefinition = Map[key];
				string empty = string.Empty;
				empty = argDefinition.NumParameters switch
				{
					0 => string.Empty, 
					1 => " " + argDefinition.Type.Name, 
					_ => " " + argDefinition.Type.Name + " ...", 
				};
				string text2 = text;
				text = text2 + " [" + argDefinition.Indicator + empty + "]";
			}
		}
		return text;
	}

	private string GetErrorMessage()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(Usage);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine(Description);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine(Help);
		stringBuilder.AppendLine();
		return stringBuilder.ToString();
	}

	private bool TryFindDefinition(string Indicator, out ArgDefinition Definition)
	{
		Definition = null;
		foreach (string key in Map.Keys)
		{
			if (string.Compare(key, Indicator, IgnoreCase) == 0)
			{
				Definition = Map[key];
				return true;
			}
		}
		return false;
	}

	public bool Parse(out Dictionary<string, object> Result)
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		return Parse(commandLineArgs, out Result);
	}

	public bool Parse(string[] Args, out Dictionary<string, object> Result)
	{
		Result = (Strict ? new Dictionary<string, object>() : GetDefaultValues());
		Help = GetHelpString();
		Usage = GetUsageString(Args);
		if (Args == null || Args.Length == 0)
		{
			return true;
		}
		int startIndex = GetStartIndex(Args);
		for (int i = startIndex; i < Args.Length; i++)
		{
			string indicator = Args[i];
			object obj = null;
			ArgDefinition Definition = null;
			if (TryFindDefinition(indicator, out Definition))
			{
				int num = 0;
				for (int j = i + 1; j < Args.Length; j++)
				{
					ArgDefinition Definition2 = null;
					if (TryFindDefinition(Args[j], out Definition2))
					{
						break;
					}
					num++;
				}
				if (Definition.NumParameters < 0 || Definition.NumParameters == num)
				{
					if (!Definition.IsCollection)
					{
						obj = ((Definition.NumParameters == 1) ? ConvertFromString(Args[++i], Definition.Type) : ((!(Definition.Type == typeof(bool))) ? Definition.DefaultValue : ((object)true)));
					}
					else
					{
						List<object> list = new List<object>();
						for (int k = i + 1; k < i + 1 + num; k++)
						{
							list.Add(ConvertFromString(Args[k], Definition.Type));
						}
						i += num;
						obj = list;
					}
					string key = (Definition.HasGroup ? Definition.Group : Definition.Name);
					if (Result.ContainsKey(key))
					{
						Result[key] = obj;
					}
					else
					{
						Result.Add(key, obj);
					}
					continue;
				}
				ErrorMessage = GetErrorMessage();
				return false;
			}
			ErrorMessage = GetErrorMessage();
			return false;
		}
		return true;
	}

	private object ConvertFromString(string Value, Type ValueType)
	{
		object obj = null;
		MethodInfo method = ValueType.GetMethod("Parse", new Type[1] { typeof(string) });
		if (method != null)
		{
			return method.Invoke(null, new object[1] { Value });
		}
		if (ValueType.IsEnum)
		{
			return Enum.Parse(ValueType, Value);
		}
		return Convert.ChangeType(Value, ValueType);
	}
}
