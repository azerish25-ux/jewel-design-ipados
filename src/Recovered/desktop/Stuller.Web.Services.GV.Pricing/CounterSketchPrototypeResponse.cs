using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CounterSketchPrototypeResponse", Namespace = "http://services.stuller.com/v3/GemVision")]
public class CounterSketchPrototypeResponse : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private int UnitsOrderedField;

	private int UnitsRemainingField;

	private DateTime? ProgramStartDateField;

	private DateTime? ProgramEndDateField;

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
	public int UnitsOrdered
	{
		get
		{
			return UnitsOrderedField;
		}
		set
		{
			if (!UnitsOrderedField.Equals(value))
			{
				UnitsOrderedField = value;
				RaisePropertyChanged("UnitsOrdered");
			}
		}
	}

	[DataMember]
	public int UnitsRemaining
	{
		get
		{
			return UnitsRemainingField;
		}
		set
		{
			if (!UnitsRemainingField.Equals(value))
			{
				UnitsRemainingField = value;
				RaisePropertyChanged("UnitsRemaining");
			}
		}
	}

	[DataMember(Order = 2)]
	public DateTime? ProgramStartDate
	{
		get
		{
			return ProgramStartDateField;
		}
		set
		{
			if (!ProgramStartDateField.Equals(value))
			{
				ProgramStartDateField = value;
				RaisePropertyChanged("ProgramStartDate");
			}
		}
	}

	[DataMember(Order = 3)]
	public DateTime? ProgramEndDate
	{
		get
		{
			return ProgramEndDateField;
		}
		set
		{
			if (!ProgramEndDateField.Equals(value))
			{
				ProgramEndDateField = value;
				RaisePropertyChanged("ProgramEndDate");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
