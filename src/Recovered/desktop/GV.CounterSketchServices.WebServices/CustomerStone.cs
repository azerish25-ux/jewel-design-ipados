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
public class CustomerStone : INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private float? costField;

	private string setShapeField;

	private float setSizeField;

	private string setTypeField;

	private float? settingCostField;

	private int stoneTypeField;

	private float? valueField;

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

	public int StoneType
	{
		get
		{
			return stoneTypeField;
		}
		set
		{
			stoneTypeField = value;
			RaisePropertyChanged("StoneType");
		}
	}

	[XmlElement(IsNullable = true)]
	public float? Value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
			RaisePropertyChanged("Value");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
