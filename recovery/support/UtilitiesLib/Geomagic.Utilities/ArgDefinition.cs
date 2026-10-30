using System;
using System.ComponentModel;

namespace Geomagic.Utilities;

public class ArgDefinition
{
	[DefaultValue(null)]
	public string Indicator { get; set; }

	[DefaultValue(null)]
	public string Name { get; set; }

	[DefaultValue(null)]
	public Type Type { get; set; }

	public bool IsCollection
	{
		get
		{
			if (NumParameters >= 0)
			{
				return NumParameters > 1;
			}
			return true;
		}
	}

	[DefaultValue(0)]
	public int NumParameters { get; set; }

	[DefaultValue(null)]
	public object DefaultValue { get; set; }

	[DefaultValue(null)]
	public string Group { get; set; }

	public bool HasGroup => !string.IsNullOrEmpty(Group);

	[DefaultValue(null)]
	public string Help { get; set; }
}
