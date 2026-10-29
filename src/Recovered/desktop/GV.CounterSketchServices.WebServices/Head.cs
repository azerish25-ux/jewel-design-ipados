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
public class Head : INotifyPropertyChanged
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
			RaisePropertyChanged("ExtensionData");
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
			RaisePropertyChanged("AssemblyCost");
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

	public CustomerStone[] CustomerStones
	{
		get
		{
			return customerStonesField;
		}
		set
		{
			customerStonesField = value;
			RaisePropertyChanged("CustomerStones");
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
			RaisePropertyChanged("Quality");
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
			RaisePropertyChanged("Series");
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
			RaisePropertyChanged("Size");
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
			RaisePropertyChanged("Stones");
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
			RaisePropertyChanged("Weight");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
