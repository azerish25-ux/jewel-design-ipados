using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Studio.CounterSketchThemeServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CustomerTheme", Namespace = "http://schemas.datacontract.org/2004/07/CounterSketchServices.BLL")]
public class CustomerTheme : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string BaseBackgroundField;

	[OptionalField]
	private string HighlightAndAccentField;

	[OptionalField]
	private int IdField;

	[OptionalField]
	private bool IsGlobalThemeField;

	[OptionalField]
	private string NameField;

	[OptionalField]
	private string TextBoxAndFrameBackgroundField;

	[OptionalField]
	private string ToolHeaderBackgroundField;

	[OptionalField]
	private string WindowFrameBackgroundField;

	[Browsable(false)]
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
	public string BaseBackground
	{
		get
		{
			return BaseBackgroundField;
		}
		set
		{
			if ((object)BaseBackgroundField != value)
			{
				BaseBackgroundField = value;
				RaisePropertyChanged("BaseBackground");
			}
		}
	}

	[DataMember]
	public string HighlightAndAccent
	{
		get
		{
			return HighlightAndAccentField;
		}
		set
		{
			if ((object)HighlightAndAccentField != value)
			{
				HighlightAndAccentField = value;
				RaisePropertyChanged("HighlightAndAccent");
			}
		}
	}

	[DataMember]
	public int Id
	{
		get
		{
			return IdField;
		}
		set
		{
			if (!IdField.Equals(value))
			{
				IdField = value;
				RaisePropertyChanged("Id");
			}
		}
	}

	[DataMember]
	public bool IsGlobalTheme
	{
		get
		{
			return IsGlobalThemeField;
		}
		set
		{
			if (!IsGlobalThemeField.Equals(value))
			{
				IsGlobalThemeField = value;
				RaisePropertyChanged("IsGlobalTheme");
			}
		}
	}

	[DataMember]
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

	[DataMember]
	public string TextBoxAndFrameBackground
	{
		get
		{
			return TextBoxAndFrameBackgroundField;
		}
		set
		{
			if ((object)TextBoxAndFrameBackgroundField != value)
			{
				TextBoxAndFrameBackgroundField = value;
				RaisePropertyChanged("TextBoxAndFrameBackground");
			}
		}
	}

	[DataMember]
	public string ToolHeaderBackground
	{
		get
		{
			return ToolHeaderBackgroundField;
		}
		set
		{
			if ((object)ToolHeaderBackgroundField != value)
			{
				ToolHeaderBackgroundField = value;
				RaisePropertyChanged("ToolHeaderBackground");
			}
		}
	}

	[DataMember]
	public string WindowFrameBackground
	{
		get
		{
			return WindowFrameBackgroundField;
		}
		set
		{
			if ((object)WindowFrameBackgroundField != value)
			{
				WindowFrameBackgroundField = value;
				RaisePropertyChanged("WindowFrameBackground");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
