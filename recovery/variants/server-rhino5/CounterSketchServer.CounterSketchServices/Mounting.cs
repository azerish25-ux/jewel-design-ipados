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
public class Mounting
{
	private ExtensionDataObject extensionDataField;

	private float? costField;

	private CustomerHead[] customerHeadsField;

	private CustomerStone[] customerStonesField;

	private Head[] headsField;

	private string modelField;

	private string qualityField;

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

	public CustomerHead[] CustomerHeads
	{
		get
		{
			return customerHeadsField;
		}
		set
		{
			customerHeadsField = value;
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

	public Head[] Heads
	{
		get
		{
			return headsField;
		}
		set
		{
			headsField = value;
		}
	}

	public string Model
	{
		get
		{
			return modelField;
		}
		set
		{
			modelField = value;
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
