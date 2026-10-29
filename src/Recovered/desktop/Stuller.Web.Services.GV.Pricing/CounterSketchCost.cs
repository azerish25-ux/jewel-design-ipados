using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CounterSketchCost", Namespace = "http://services.stuller.com/v3/GemVision")]
public class CounterSketchCost : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private string AccountNumberField;

	private float? ChargesField;

	private string CountryCodeField;

	private string CultureField;

	private CustomerHead[] CustomerHeadsField;

	private CustomerStone[] CustomerStonesField;

	private Error[] ErrorsField;

	private Head[] HeadsField;

	private Mounting[] MountingsField;

	private float? RateField;

	private Stone[] StonesField;

	private float? TotalCostField;

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

	[DataMember(IsRequired = true)]
	public string AccountNumber
	{
		get
		{
			return AccountNumberField;
		}
		set
		{
			if ((object)AccountNumberField != value)
			{
				AccountNumberField = value;
				RaisePropertyChanged("AccountNumber");
			}
		}
	}

	[DataMember]
	public float? Charges
	{
		get
		{
			return ChargesField;
		}
		set
		{
			if (!ChargesField.Equals(value))
			{
				ChargesField = value;
				RaisePropertyChanged("Charges");
			}
		}
	}

	[DataMember(IsRequired = true)]
	public string CountryCode
	{
		get
		{
			return CountryCodeField;
		}
		set
		{
			if ((object)CountryCodeField != value)
			{
				CountryCodeField = value;
				RaisePropertyChanged("CountryCode");
			}
		}
	}

	[DataMember]
	public string Culture
	{
		get
		{
			return CultureField;
		}
		set
		{
			if ((object)CultureField != value)
			{
				CultureField = value;
				RaisePropertyChanged("Culture");
			}
		}
	}

	[DataMember]
	public CustomerHead[] CustomerHeads
	{
		get
		{
			return CustomerHeadsField;
		}
		set
		{
			if (CustomerHeadsField != value)
			{
				CustomerHeadsField = value;
				RaisePropertyChanged("CustomerHeads");
			}
		}
	}

	[DataMember]
	public CustomerStone[] CustomerStones
	{
		get
		{
			return CustomerStonesField;
		}
		set
		{
			if (CustomerStonesField != value)
			{
				CustomerStonesField = value;
				RaisePropertyChanged("CustomerStones");
			}
		}
	}

	[DataMember]
	public Error[] Errors
	{
		get
		{
			return ErrorsField;
		}
		set
		{
			if (ErrorsField != value)
			{
				ErrorsField = value;
				RaisePropertyChanged("Errors");
			}
		}
	}

	[DataMember]
	public Head[] Heads
	{
		get
		{
			return HeadsField;
		}
		set
		{
			if (HeadsField != value)
			{
				HeadsField = value;
				RaisePropertyChanged("Heads");
			}
		}
	}

	[DataMember]
	public Mounting[] Mountings
	{
		get
		{
			return MountingsField;
		}
		set
		{
			if (MountingsField != value)
			{
				MountingsField = value;
				RaisePropertyChanged("Mountings");
			}
		}
	}

	[DataMember]
	public float? Rate
	{
		get
		{
			return RateField;
		}
		set
		{
			if (!RateField.Equals(value))
			{
				RateField = value;
				RaisePropertyChanged("Rate");
			}
		}
	}

	[DataMember]
	public Stone[] Stones
	{
		get
		{
			return StonesField;
		}
		set
		{
			if (StonesField != value)
			{
				StonesField = value;
				RaisePropertyChanged("Stones");
			}
		}
	}

	[DataMember]
	public float? TotalCost
	{
		get
		{
			return TotalCostField;
		}
		set
		{
			if (!TotalCostField.Equals(value))
			{
				TotalCostField = value;
				RaisePropertyChanged("TotalCost");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
