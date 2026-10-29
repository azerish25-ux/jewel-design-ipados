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
public class CounterSketchCost
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

	public Error[] Errors
	{
		get
		{
			return errorsField;
		}
		set
		{
			errorsField = value;
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

	public Mounting[] Mountings
	{
		get
		{
			return mountingsField;
		}
		set
		{
			mountingsField = value;
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
		}
	}
}
