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
public class CounterSketchCost : INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private string accountNumberField;

	private float? chargesField;

	private string countryCodeField;

	private string cultureField;

	private CustomerHead[] customerHeadsField;

	private CustomerStone[] customerStonesField;

	private Error[] errorsField;

	private Head[] headsField;

	private Mounting[] mountingsField;

	private float? rateField;

	private Stone[] stonesField;

	private float? totalCostField;

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

	public string AccountNumber
	{
		get
		{
			return accountNumberField;
		}
		set
		{
			accountNumberField = value;
			RaisePropertyChanged("AccountNumber");
		}
	}

	[XmlElement(IsNullable = true)]
	public float? Charges
	{
		get
		{
			return chargesField;
		}
		set
		{
			chargesField = value;
			RaisePropertyChanged("Charges");
		}
	}

	public string CountryCode
	{
		get
		{
			return countryCodeField;
		}
		set
		{
			countryCodeField = value;
			RaisePropertyChanged("CountryCode");
		}
	}

	public string Culture
	{
		get
		{
			return cultureField;
		}
		set
		{
			cultureField = value;
			RaisePropertyChanged("Culture");
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

	public Error[] Errors
	{
		get
		{
			return errorsField;
		}
		set
		{
			errorsField = value;
			RaisePropertyChanged("Errors");
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

	public Mounting[] Mountings
	{
		get
		{
			return mountingsField;
		}
		set
		{
			mountingsField = value;
			RaisePropertyChanged("Mountings");
		}
	}

	[XmlElement(IsNullable = true)]
	public float? Rate
	{
		get
		{
			return rateField;
		}
		set
		{
			rateField = value;
			RaisePropertyChanged("Rate");
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

	[XmlElement(IsNullable = true)]
	public float? TotalCost
	{
		get
		{
			return totalCostField;
		}
		set
		{
			totalCostField = value;
			RaisePropertyChanged("TotalCost");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
