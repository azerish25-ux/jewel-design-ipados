using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace CounterSketchServer.CounterSketchServices;

[Serializable]
[GeneratedCode("System.Xml", "4.6.1586.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CounterSketch.biz/")]
public class Head
{
	private ExtensionDataObject extensionDataField;

	private float? assemblyCostField;

	private float? costField;

	private CustomerStone[] customerStonesField;

	private string qualityField;

	private string seriesField;

	private float sizeField;

	private Stone[] stonesField;

	private float weightField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[XmlElement(IsNullable = true)]
	public float? AssemblyCost
	{
		get
		{
			return assemblyCostField;
		}
		set
		{
			assemblyCostField = value;
		}
	}

	[XmlElement(IsNullable = true)]
	public float? Cost
	{
		get
		{
			return costField;
		}
		set
		{
			costField = value;
		}
	}

	public CustomerStone[] CustomerStones
	{
		get
		{
			return customerStonesField;
		}
		set
		{
			customerStonesField = value;
		}
	}

	public string Quality
	{
		get
		{
			return qualityField;
		}
		set
		{
			qualityField = value;
		}
	}

	public string Series
	{
		get
		{
			return seriesField;
		}
		set
		{
			seriesField = value;
		}
	}

	public float Size
	{
		get
		{
			return sizeField;
		}
		set
		{
			sizeField = value;
		}
	}

	public Stone[] Stones
	{
		get
		{
			return stonesField;
		}
		set
		{
			stonesField = value;
		}
	}

	public float Weight
	{
		get
		{
			return weightField;
		}
		set
		{
			weightField = value;
		}
	}
}
