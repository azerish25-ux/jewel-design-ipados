using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace GV.CounterSketchServices.WebServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CounterSketch.biz/")]
public class Stone : INotifyPropertyChanged
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
			RaisePropertyChanged("ExtensionData");
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
			RaisePropertyChanged("Cost");
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
			RaisePropertyChanged("ItemNumber");
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
			RaisePropertyChanged("Serial");
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
			RaisePropertyChanged("SetShape");
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
			RaisePropertyChanged("SetSize");
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
			RaisePropertyChanged("SetType");
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
			RaisePropertyChanged("SettingCost");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
