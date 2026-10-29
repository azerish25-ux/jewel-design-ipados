using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CustomerStone", Namespace = "http://services.stuller.com/v3/GemVision")]
public class CustomerStone : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private float? CostField;

	private string SetShapeField;

	private float SetSizeField;

	private string SetTypeField;

	private float? SettingCostField;

	private int StoneTypeField;

	private float? ValueField;

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

	[DataMember(IsRequired = true)]
	public string SetShape
	{
		get
		{
			return SetShapeField;
		}
		set
		{
			if ((object)SetShapeField != value)
			{
				SetShapeField = value;
				RaisePropertyChanged("SetShape");
			}
		}
	}

	[DataMember(IsRequired = true)]
	public float SetSize
	{
		get
		{
			return SetSizeField;
		}
		set
		{
			if (!SetSizeField.Equals(value))
			{
				SetSizeField = value;
				RaisePropertyChanged("SetSize");
			}
		}
	}

	[DataMember(IsRequired = true)]
	public string SetType
	{
		get
		{
			return SetTypeField;
		}
		set
		{
			if ((object)SetTypeField != value)
			{
				SetTypeField = value;
				RaisePropertyChanged("SetType");
			}
		}
	}

	[DataMember]
	public float? SettingCost
	{
		get
		{
			return SettingCostField;
		}
		set
		{
			if (!SettingCostField.Equals(value))
			{
				SettingCostField = value;
				RaisePropertyChanged("SettingCost");
			}
		}
	}

	[DataMember(IsRequired = true)]
	public int StoneType
	{
		get
		{
			return StoneTypeField;
		}
		set
		{
			if (!StoneTypeField.Equals(value))
			{
				StoneTypeField = value;
				RaisePropertyChanged("StoneType");
			}
		}
	}

	[DataMember(IsRequired = true)]
	public float? Value
	{
		get
		{
			return ValueField;
		}
		set
		{
			if (!ValueField.Equals(value))
			{
				ValueField = value;
				RaisePropertyChanged("Value");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
