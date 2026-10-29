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
public class Mounting : INotifyPropertyChanged
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

	public CustomerHead[] CustomerHeads
	{
		get
		{
			return customerHeadsField;
		}
		set
		{
			customerHeadsField = value;
			RaisePropertyChanged("CustomerHeads");
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

	public Head[] Heads
	{
		get
		{
			return headsField;
		}
		set
		{
			headsField = value;
			RaisePropertyChanged("Heads");
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
			RaisePropertyChanged("Model");
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
