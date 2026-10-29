using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Head", Namespace = "http://services.stuller.com/v3/GemVision")]
public class Head : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private float? AssemblyCostField;

	private float? CostField;

	private CustomerStone[] CustomerStonesField;

	private string QualityField;

	private string SeriesField;

	private float SizeField;

	private Stone[] StonesField;

	private float WeightField;

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

	[DataMember]
	public float? AssemblyCost
	{
		get
		{
			return AssemblyCostField;
		}
		set
		{
			if (!AssemblyCostField.Equals(value))
			{
				AssemblyCostField = value;
				RaisePropertyChanged("AssemblyCost");
			}
		}
	}

	[DataMember]
	public float? Cost
	{
		get
		{
			return CostField;
		}
		set
		{
			if (!CostField.Equals(value))
			{
				CostField = value;
				RaisePropertyChanged("Cost");
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

	[DataMember(IsRequired = true)]
	public string Quality
	{
		get
		{
			return QualityField;
		}
		set
		{
			if ((object)QualityField != value)
			{
				QualityField = value;
				RaisePropertyChanged("Quality");
			}
		}
	}

	[DataMember(IsRequired = true)]
	public string Series
	{
		get
		{
			return SeriesField;
		}
		set
		{
			if ((object)SeriesField != value)
			{
				SeriesField = value;
				RaisePropertyChanged("Series");
			}
		}
	}

	[DataMember(IsRequired = true)]
	public float Size
	{
		get
		{
			return SizeField;
		}
		set
		{
			if (!SizeField.Equals(value))
			{
				SizeField = value;
				RaisePropertyChanged("Size");
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

	[DataMember(IsRequired = true)]
	public float Weight
	{
		get
		{
			return WeightField;
		}
		set
		{
			if (!WeightField.Equals(value))
			{
				WeightField = value;
				RaisePropertyChanged("Weight");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
