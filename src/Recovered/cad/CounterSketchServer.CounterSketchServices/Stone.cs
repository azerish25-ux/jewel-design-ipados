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
public class Stone
{
	private ExtensionDataObject extensionDataField;

	private float? costField;

	private string itemNumberField;

	private int? serialField;

	private string setShapeField;

	private float setSizeField;

	private string setTypeField;

	private float? settingCostField;

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

	public string ItemNumber
	{
		get
		{
			return itemNumberField;
		}
		set
		{
			itemNumberField = value;
		}
	}

	[XmlElement(IsNullable = true)]
	public int? Serial
	{
		get
		{
			return serialField;
		}
		set
		{
			serialField = value;
		}
	}

	public string SetShape
	{
		get
		{
			return setShapeField;
		}
		set
		{
			setShapeField = value;
		}
	}

	public float SetSize
	{
		get
		{
			return setSizeField;
		}
		set
		{
			setSizeField = value;
		}
	}

	public string SetType
	{
		get
		{
			return setTypeField;
		}
		set
		{
			setTypeField = value;
		}
	}

	[XmlElement(IsNullable = true)]
	public float? SettingCost
	{
		get
		{
			return settingCostField;
		}
		set
		{
			settingCostField = value;
		}
	}
}
