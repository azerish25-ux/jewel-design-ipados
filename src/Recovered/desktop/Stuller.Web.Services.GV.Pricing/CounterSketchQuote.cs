using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CounterSketchQuote", Namespace = "http://services.stuller.com/v3/GemVision")]
public class CounterSketchQuote : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private string BillToAccountNumberField;

	private string ShipToAccountNumberField;

	private string CompanyNameField;

	private string NameField;

	private string EmailField;

	private string PhoneField;

	private string RingSizeField;

	private string BaseMetalField;

	private string TopMetal1Field;

	private string TopMetal2Field;

	private string TopMetal3Field;

	private FinishType FinishTypeField;

	private AssemblyType AssemblyTypeField;

	private SettingType SettingTypeField;

	private StonesType StonesTypeField;

	private TrademarkType TrademarkTypeField;

	private string StoneCommentsField;

	private string CustomerStoneCommentsField;

	private string FinishCommentsField;

	private string CustomerReferenceField;

	private string FileNameField;

	private int FileSizeField;

	private bool IsTestField;

	private string SCSField;

	private RequestSource SourceField;

	private string CommentsField;

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
	public string BillToAccountNumber
	{
		get
		{
			return BillToAccountNumberField;
		}
		set
		{
			if ((object)BillToAccountNumberField != value)
			{
				BillToAccountNumberField = value;
				RaisePropertyChanged("BillToAccountNumber");
			}
		}
	}

	[DataMember]
	public string ShipToAccountNumber
	{
		get
		{
			return ShipToAccountNumberField;
		}
		set
		{
			if ((object)ShipToAccountNumberField != value)
			{
				ShipToAccountNumberField = value;
				RaisePropertyChanged("ShipToAccountNumber");
			}
		}
	}

	[DataMember(Order = 2)]
	public string CompanyName
	{
		get
		{
			return CompanyNameField;
		}
		set
		{
			if ((object)CompanyNameField != value)
			{
				CompanyNameField = value;
				RaisePropertyChanged("CompanyName");
			}
		}
	}

	[DataMember(Order = 3)]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			if ((object)NameField != value)
			{
				NameField = value;
				RaisePropertyChanged("Name");
			}
		}
	}

	[DataMember(Order = 4)]
	public string Email
	{
		get
		{
			return EmailField;
		}
		set
		{
			if ((object)EmailField != value)
			{
				EmailField = value;
				RaisePropertyChanged("Email");
			}
		}
	}

	[DataMember(Order = 5)]
	public string Phone
	{
		get
		{
			return PhoneField;
		}
		set
		{
			if ((object)PhoneField != value)
			{
				PhoneField = value;
				RaisePropertyChanged("Phone");
			}
		}
	}

	[DataMember(Order = 6)]
	public string RingSize
	{
		get
		{
			return RingSizeField;
		}
		set
		{
			if ((object)RingSizeField != value)
			{
				RingSizeField = value;
				RaisePropertyChanged("RingSize");
			}
		}
	}

	[DataMember(Order = 7)]
	public string BaseMetal
	{
		get
		{
			return BaseMetalField;
		}
		set
		{
			if ((object)BaseMetalField != value)
			{
				BaseMetalField = value;
				RaisePropertyChanged("BaseMetal");
			}
		}
	}

	[DataMember(Order = 8)]
	public string TopMetal1
	{
		get
		{
			return TopMetal1Field;
		}
		set
		{
			if ((object)TopMetal1Field != value)
			{
				TopMetal1Field = value;
				RaisePropertyChanged("TopMetal1");
			}
		}
	}

	[DataMember(Order = 9)]
	public string TopMetal2
	{
		get
		{
			return TopMetal2Field;
		}
		set
		{
			if ((object)TopMetal2Field != value)
			{
				TopMetal2Field = value;
				RaisePropertyChanged("TopMetal2");
			}
		}
	}

	[DataMember(Order = 10)]
	public string TopMetal3
	{
		get
		{
			return TopMetal3Field;
		}
		set
		{
			if ((object)TopMetal3Field != value)
			{
				TopMetal3Field = value;
				RaisePropertyChanged("TopMetal3");
			}
		}
	}

	[DataMember(Order = 11)]
	public FinishType FinishType
	{
		get
		{
			return FinishTypeField;
		}
		set
		{
			if (!FinishTypeField.Equals(value))
			{
				FinishTypeField = value;
				RaisePropertyChanged("FinishType");
			}
		}
	}

	[DataMember(Order = 12)]
	public AssemblyType AssemblyType
	{
		get
		{
			return AssemblyTypeField;
		}
		set
		{
			if (!AssemblyTypeField.Equals(value))
			{
				AssemblyTypeField = value;
				RaisePropertyChanged("AssemblyType");
			}
		}
	}

	[DataMember(Order = 13)]
	public SettingType SettingType
	{
		get
		{
			return SettingTypeField;
		}
		set
		{
			if (!SettingTypeField.Equals(value))
			{
				SettingTypeField = value;
				RaisePropertyChanged("SettingType");
			}
		}
	}

	[DataMember(Order = 14)]
	public StonesType StonesType
	{
		get
		{
			return StonesTypeField;
		}
		set
		{
			if (!StonesTypeField.Equals(value))
			{
				StonesTypeField = value;
				RaisePropertyChanged("StonesType");
			}
		}
	}

	[DataMember(Order = 15)]
	public TrademarkType TrademarkType
	{
		get
		{
			return TrademarkTypeField;
		}
		set
		{
			if (!TrademarkTypeField.Equals(value))
			{
				TrademarkTypeField = value;
				RaisePropertyChanged("TrademarkType");
			}
		}
	}

	[DataMember(Order = 16)]
	public string StoneComments
	{
		get
		{
			return StoneCommentsField;
		}
		set
		{
			if ((object)StoneCommentsField != value)
			{
				StoneCommentsField = value;
				RaisePropertyChanged("StoneComments");
			}
		}
	}

	[DataMember(Order = 17)]
	public string CustomerStoneComments
	{
		get
		{
			return CustomerStoneCommentsField;
		}
		set
		{
			if ((object)CustomerStoneCommentsField != value)
			{
				CustomerStoneCommentsField = value;
				RaisePropertyChanged("CustomerStoneComments");
			}
		}
	}

	[DataMember(Order = 18)]
	public string FinishComments
	{
		get
		{
			return FinishCommentsField;
		}
		set
		{
			if ((object)FinishCommentsField != value)
			{
				FinishCommentsField = value;
				RaisePropertyChanged("FinishComments");
			}
		}
	}

	[DataMember(Order = 19)]
	public string CustomerReference
	{
		get
		{
			return CustomerReferenceField;
		}
		set
		{
			if ((object)CustomerReferenceField != value)
			{
				CustomerReferenceField = value;
				RaisePropertyChanged("CustomerReference");
			}
		}
	}

	[DataMember(Order = 20)]
	public string FileName
	{
		get
		{
			return FileNameField;
		}
		set
		{
			if ((object)FileNameField != value)
			{
				FileNameField = value;
				RaisePropertyChanged("FileName");
			}
		}
	}

	[DataMember(Order = 21)]
	public int FileSize
	{
		get
		{
			return FileSizeField;
		}
		set
		{
			if (!FileSizeField.Equals(value))
			{
				FileSizeField = value;
				RaisePropertyChanged("FileSize");
			}
		}
	}

	[DataMember(Order = 22)]
	public bool IsTest
	{
		get
		{
			return IsTestField;
		}
		set
		{
			if (!IsTestField.Equals(value))
			{
				IsTestField = value;
				RaisePropertyChanged("IsTest");
			}
		}
	}

	[DataMember(Order = 23)]
	public string SCS
	{
		get
		{
			return SCSField;
		}
		set
		{
			if ((object)SCSField != value)
			{
				SCSField = value;
				RaisePropertyChanged("SCS");
			}
		}
	}

	[DataMember(Order = 24)]
	public RequestSource Source
	{
		get
		{
			return SourceField;
		}
		set
		{
			if (!SourceField.Equals(value))
			{
				SourceField = value;
				RaisePropertyChanged("Source");
			}
		}
	}

	[DataMember(Order = 25)]
	public string Comments
	{
		get
		{
			return CommentsField;
		}
		set
		{
			if ((object)CommentsField != value)
			{
				CommentsField = value;
				RaisePropertyChanged("Comments");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
