using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Mounting", Namespace = "http://services.stuller.com/v3/GemVision")]
public class Mounting : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private float? CostField;

	private CustomerHead[] CustomerHeadsField;

	private CustomerStone[] CustomerStonesField;

	private Head[] HeadsField;

	private string ModelField;

	private string QualityField;

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

	[DataMember(IsRequired = true)]
	public string Model
	{
		get
		{
			return ModelField;
		}
		set
		{
			if ((object)ModelField != value)
			{
				ModelField = value;
				RaisePropertyChanged("Model");
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
