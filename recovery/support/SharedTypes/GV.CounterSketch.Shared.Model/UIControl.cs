using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Shared.Pricing;

namespace GV.CounterSketch.Shared.Model;

[DataContract]
public class UIControl : INotifyPropertyChanged
{
	private IEnumerable<ComponentsEnum> _assignedDisplayComponents;

	private bool _categoryDeletable;

	private string _categoryName;

	private string _controlName;

	private string _controlSpecifier;

	private string _controlSpecifierValue;

	private UIControlStackLocation _currentUIControlStackLocation;

	private UIControlStatus _currentUIControlStatus;

	private UIControlType _currentUIControlType;

	private List<DataItem> _dataItems;

	private Guid _importGroupIdentifier;

	private string _importGroupName;

	private double _maximum1;

	private double _maximum2;

	private double _minimum1;

	private double _minimum2;

	private double _step1;

	private double _step2;

	private string _updateTarget = string.Empty;

	private double _value1;

	private double _value2;

	private object _valueGeneric;

	private string _displayText;

	private string _englishControlName;

	private byte[] _imageSourceBytes;

	private List<byte[]> _imageSourceBytesList;

	private Guid _blockInstanceGuid;

	private List<StudioGem> _gemList;

	private List<GemTableSizeInfo> _gemTableSizeInfoList;

	private bool _isCustomGemSize;

	private bool _isUIDisabled;

	private bool _listAsText;

	private int _listCount;

	private int _listIndex;

	private List<ListItemIndexPair> _listItems;

	private List<HeadStyleListItemIndexPair> _headStyleListItems;

	private string _valueListImageModifier;

	private bool _isLargeGemEnabled;

	private bool _isAssignedFromStone;

	private string _englishCategoryName;

	private Guid _importGroupPartGroupId;

	private double _gemCarat;

	private int _sortIndex;

	private double _gemDepth;

	private double _gemHeight;

	private double _gemPercentage;

	private double _gemSize;

	private int _gemTableIndex;

	private int _gemTableItemCount;

	private double _gemWidth;

	private int _whichGemTableTypeValue;

	private int _whichGemTableShapeValue;

	private int _whichLibraryValue;

	private string _whichLibraryMod;

	private bool _isEnabled1;

	private bool _isEnabled2;

	private bool _hasMissingObject;

	private bool _isOptional;

	private int _eRingSizeOptionIndex;

	private List<string> _eRingSizeOptions;

	private bool _isFilteredOut;

	private bool _categoryHasImage;

	private bool _genericBool;

	private int _genericInt;

	private double _genericDouble;

	private string _genericString;

	private List<string> _listGemTypes;

	private List<string> _listGemShapes;

	private int _listGemTypesIndex;

	private int _listGemShapesIndex;

	[DataMember]
	public IEnumerable<ComponentsEnum> AssignedDisplayComponents
	{
		get
		{
			return _assignedDisplayComponents;
		}
		set
		{
			_assignedDisplayComponents = value;
			RaisePropertyChanged("AssignedDisplayComponent");
		}
	}

	[DataMember]
	public Guid BlockInstanceGuid
	{
		get
		{
			return _blockInstanceGuid;
		}
		set
		{
			_blockInstanceGuid = value;
			RaisePropertyChanged("BlockInstanceGuid");
		}
	}

	[DataMember]
	public string CategoryName
	{
		get
		{
			return _categoryName;
		}
		set
		{
			_categoryName = value;
			RaisePropertyChanged("CategoryName");
		}
	}

	[DataMember]
	public bool CategoryHasImage
	{
		get
		{
			return _categoryHasImage;
		}
		set
		{
			_categoryHasImage = value;
			RaisePropertyChanged("CategoryHasImage");
		}
	}

	[DataMember]
	public string EnglishCategoryName
	{
		get
		{
			return _englishCategoryName;
		}
		set
		{
			_englishCategoryName = value;
			RaisePropertyChanged("EnglishCategoryName");
		}
	}

	[DataMember]
	public string ControlName
	{
		get
		{
			return _controlName;
		}
		set
		{
			_controlName = value;
			RaisePropertyChanged("ControlName");
		}
	}

	[DataMember]
	public string EnglishControlName
	{
		get
		{
			return _englishControlName;
		}
		set
		{
			_englishControlName = value;
			RaisePropertyChanged("EnglishControlName");
		}
	}

	[DataMember]
	public string ControlSpecifier
	{
		get
		{
			return _controlSpecifier;
		}
		set
		{
			_controlSpecifier = value;
			RaisePropertyChanged("ControlSpecifier");
		}
	}

	[DataMember]
	public string ControlSpecifierValue
	{
		get
		{
			return _controlSpecifierValue;
		}
		set
		{
			_controlSpecifierValue = value;
			RaisePropertyChanged("ControlSpecifierValue");
		}
	}

	[DataMember]
	public UIControlStackLocation CurrentUIControlStackLocation
	{
		get
		{
			return _currentUIControlStackLocation;
		}
		set
		{
			_currentUIControlStackLocation = value;
			RaisePropertyChanged("CurrentUIControlStackLocation");
		}
	}

	[DataMember]
	public UIControlStatus CurrentUIControlStatus
	{
		get
		{
			return _currentUIControlStatus;
		}
		set
		{
			_currentUIControlStatus = value;
			RaisePropertyChanged("CurrentUIControlStatus");
		}
	}

	[DataMember]
	public UIControlType CurrentUIControlType
	{
		get
		{
			return _currentUIControlType;
		}
		set
		{
			_currentUIControlType = value;
			RaisePropertyChanged("CurrentUIControlType");
		}
	}

	[DataMember]
	public List<DataItem> DataItems
	{
		get
		{
			return _dataItems;
		}
		set
		{
			_dataItems = value;
			RaisePropertyChanged("DataItems");
		}
	}

	[DataMember]
	public string DisplayText
	{
		get
		{
			return _displayText;
		}
		set
		{
			_displayText = value;
			RaisePropertyChanged("DisplayText");
		}
	}

	[DataMember]
	public double GemCarat
	{
		get
		{
			return _gemCarat;
		}
		set
		{
			_gemCarat = value;
			RaisePropertyChanged("GemCarat");
		}
	}

	[DataMember]
	public List<StudioGem> GemList
	{
		get
		{
			return _gemList;
		}
		set
		{
			_gemList = value;
			RaisePropertyChanged("GemList");
		}
	}

	[DataMember]
	public List<GemTableSizeInfo> GemTableSizeInfoList
	{
		get
		{
			return _gemTableSizeInfoList;
		}
		set
		{
			_gemTableSizeInfoList = value;
			RaisePropertyChanged("GemTableSizeInfoList");
		}
	}

	[DataMember]
	public Guid ImportGroupIdentifier
	{
		get
		{
			return _importGroupIdentifier;
		}
		set
		{
			_importGroupIdentifier = value;
			RaisePropertyChanged("ImportGroupIdentifier");
		}
	}

	[DataMember]
	public string ImportGroupName
	{
		get
		{
			return _importGroupName;
		}
		set
		{
			_importGroupName = value;
			RaisePropertyChanged("ImportGroupName");
		}
	}

	[DataMember]
	public bool IsCustomGemSize
	{
		get
		{
			return _isCustomGemSize;
		}
		set
		{
			_isCustomGemSize = value;
			RaisePropertyChanged("IsCustomGemSize");
		}
	}

	[DataMember]
	public bool IsUIDisabled
	{
		get
		{
			return _isUIDisabled;
		}
		set
		{
			_isUIDisabled = value;
			RaisePropertyChanged("IsUIDisabled");
		}
	}

	[DataMember]
	public double Maximum1
	{
		get
		{
			return _maximum1;
		}
		set
		{
			_maximum1 = value;
			RaisePropertyChanged("Maximum1");
		}
	}

	[DataMember]
	public double Maximum2
	{
		get
		{
			return _maximum2;
		}
		set
		{
			_maximum2 = value;
			RaisePropertyChanged("Maximum2");
		}
	}

	[DataMember]
	public double Minimum1
	{
		get
		{
			return _minimum1;
		}
		set
		{
			_minimum1 = value;
			RaisePropertyChanged("Minimum1");
		}
	}

	[DataMember]
	public double Minimum2
	{
		get
		{
			return _minimum2;
		}
		set
		{
			_minimum2 = value;
			RaisePropertyChanged("Minimum2");
		}
	}

	[DataMember]
	public int SortIndex
	{
		get
		{
			return _sortIndex;
		}
		set
		{
			_sortIndex = value;
			RaisePropertyChanged("SortIndex");
		}
	}

	[DataMember]
	public double Step1
	{
		get
		{
			return _step1;
		}
		set
		{
			_step1 = value;
			RaisePropertyChanged("step1");
		}
	}

	[DataMember]
	public double Step2
	{
		get
		{
			return _step2;
		}
		set
		{
			_step2 = value;
			RaisePropertyChanged("Step2");
		}
	}

	[DataMember]
	public string UpdateTarget
	{
		get
		{
			return _updateTarget;
		}
		set
		{
			_updateTarget = value;
			RaisePropertyChanged("UpdateTarget");
		}
	}

	[DataMember]
	public object ValueGeneric
	{
		get
		{
			return _valueGeneric;
		}
		set
		{
			_valueGeneric = value;
			RaisePropertyChanged("ValueGeneric");
		}
	}

	[DataMember]
	public double Value1
	{
		get
		{
			return _value1;
		}
		set
		{
			_value1 = value;
			RaisePropertyChanged("Value1");
		}
	}

	[DataMember]
	public double Value2
	{
		get
		{
			return _value2;
		}
		set
		{
			_value2 = value;
			RaisePropertyChanged("Value2");
		}
	}

	[DataMember]
	public byte[] ImageSourceBytes
	{
		get
		{
			return _imageSourceBytes;
		}
		set
		{
			_imageSourceBytes = value;
			RaisePropertyChanged("ImageSourceBytes");
		}
	}

	[DataMember]
	public List<byte[]> ImageSourceBytesListList
	{
		get
		{
			return _imageSourceBytesList;
		}
		set
		{
			_imageSourceBytesList = value;
			RaisePropertyChanged("ImageSourcesBytes");
		}
	}

	[DataMember]
	public double GemDepth
	{
		get
		{
			return _gemDepth;
		}
		set
		{
			_gemDepth = value;
			RaisePropertyChanged("GemDepth");
		}
	}

	[DataMember]
	public double GemHeight
	{
		get
		{
			return _gemHeight;
		}
		set
		{
			_gemHeight = value;
			RaisePropertyChanged("GemHeight");
		}
	}

	[DataMember]
	public double GemPercentage
	{
		get
		{
			return _gemPercentage;
		}
		set
		{
			_gemPercentage = value;
			RaisePropertyChanged("GemPercentage");
		}
	}

	[DataMember]
	public double GemSize
	{
		get
		{
			return _gemSize;
		}
		set
		{
			_gemSize = value;
			RaisePropertyChanged("GemSize");
		}
	}

	[DataMember]
	public int GemTableIndex
	{
		get
		{
			return _gemTableIndex;
		}
		set
		{
			_gemTableIndex = value;
			RaisePropertyChanged("GemTableIndex");
		}
	}

	[DataMember]
	public int GemTableItemCount
	{
		get
		{
			return _gemTableItemCount;
		}
		set
		{
			_gemTableItemCount = value;
			RaisePropertyChanged("GemTableItemCount");
		}
	}

	[DataMember]
	public double GemWidth
	{
		get
		{
			return _gemWidth;
		}
		set
		{
			_gemWidth = value;
			RaisePropertyChanged("GemWidth");
		}
	}

	[DataMember]
	public bool CategoryDeletable
	{
		get
		{
			return _categoryDeletable;
		}
		set
		{
			_categoryDeletable = value;
			RaisePropertyChanged("CategoryDeletable");
		}
	}

	[DataMember]
	public bool ListAsText
	{
		get
		{
			return _listAsText;
		}
		set
		{
			_listAsText = value;
			RaisePropertyChanged("ListAsText");
		}
	}

	[DataMember]
	public int ListCount
	{
		get
		{
			return _listCount;
		}
		set
		{
			_listCount = value;
			RaisePropertyChanged("ListCount");
		}
	}

	[DataMember]
	public int ListIndex
	{
		get
		{
			return _listIndex;
		}
		set
		{
			_listIndex = value;
			RaisePropertyChanged("ListIndex");
		}
	}

	[DataMember]
	public List<ListItemIndexPair> ListItems
	{
		get
		{
			return _listItems;
		}
		set
		{
			_listItems = value;
			RaisePropertyChanged("ListItems");
		}
	}

	[DataMember]
	public List<HeadStyleListItemIndexPair> HeadStyleListItems
	{
		get
		{
			return _headStyleListItems;
		}
		set
		{
			_headStyleListItems = value;
			RaisePropertyChanged("HeadStyleListItems");
		}
	}

	[DataMember]
	public string ValueListImageModifier
	{
		get
		{
			return _valueListImageModifier;
		}
		set
		{
			_valueListImageModifier = value;
			RaisePropertyChanged("ValueListImageModifier");
		}
	}

	[DataMember]
	public bool IsLargeGemEnabled
	{
		get
		{
			return _isLargeGemEnabled;
		}
		set
		{
			_isLargeGemEnabled = value;
			RaisePropertyChanged("IsLargeGemEnabled");
		}
	}

	[DataMember]
	public bool IsAssignedFromStone
	{
		get
		{
			return _isAssignedFromStone;
		}
		set
		{
			_isAssignedFromStone = value;
			RaisePropertyChanged("IsAssignedFromStone");
		}
	}

	[DataMember]
	public Guid ImportGroupPartGroupId
	{
		get
		{
			return _importGroupPartGroupId;
		}
		set
		{
			_importGroupPartGroupId = value;
			RaisePropertyChanged("ImportGroupPartGroupId");
		}
	}

	[DataMember]
	public int WhichGemTableTypeValue
	{
		get
		{
			return _whichGemTableTypeValue;
		}
		set
		{
			_whichGemTableTypeValue = value;
			RaisePropertyChanged("WhichGemTableTypeValue");
		}
	}

	[DataMember]
	public int WhichGemTableShapeValue
	{
		get
		{
			return _whichGemTableShapeValue;
		}
		set
		{
			_whichGemTableShapeValue = value;
			RaisePropertyChanged("WhichGemTableShapeValue");
		}
	}

	[DataMember]
	public List<string> ListGemTypes
	{
		get
		{
			return _listGemTypes;
		}
		set
		{
			_listGemTypes = value;
			RaisePropertyChanged("ListGemTypes");
		}
	}

	[DataMember]
	public int ListGemTypesIndex
	{
		get
		{
			return _listGemTypesIndex;
		}
		set
		{
			_listGemTypesIndex = value;
			RaisePropertyChanged("ListGemTypesIndex");
		}
	}

	[DataMember]
	public List<string> ListGemShapes
	{
		get
		{
			return _listGemShapes;
		}
		set
		{
			_listGemShapes = value;
			RaisePropertyChanged("ListGemShapes");
		}
	}

	[DataMember]
	public int ListGemShapesIndex
	{
		get
		{
			return _listGemShapesIndex;
		}
		set
		{
			_listGemShapesIndex = value;
			RaisePropertyChanged("ListGemShapesIndex");
		}
	}

	[DataMember]
	public int WhichLibraryValue
	{
		get
		{
			return _whichLibraryValue;
		}
		set
		{
			_whichLibraryValue = value;
			RaisePropertyChanged("WhichLibraryValue");
		}
	}

	[DataMember]
	public string WhichLibraryMod
	{
		get
		{
			return _whichLibraryMod;
		}
		set
		{
			_whichLibraryMod = value;
			RaisePropertyChanged("WhichLibraryMod");
		}
	}

	[DataMember]
	public bool IsEnabled1
	{
		get
		{
			return _isEnabled1;
		}
		set
		{
			_isEnabled1 = value;
			RaisePropertyChanged("IsEnabled1");
		}
	}

	[DataMember]
	public bool IsEnabled2
	{
		get
		{
			return _isEnabled2;
		}
		set
		{
			_isEnabled2 = value;
			RaisePropertyChanged("IsEnabled2");
		}
	}

	[DataMember]
	public bool HasMissingObject
	{
		get
		{
			return _hasMissingObject;
		}
		set
		{
			_hasMissingObject = value;
			RaisePropertyChanged("HasMissingObject");
		}
	}

	[DataMember]
	public bool IsOptional
	{
		get
		{
			return _isOptional;
		}
		set
		{
			_isOptional = value;
			RaisePropertyChanged("IsOptional");
		}
	}

	[DataMember]
	public int ERingSizeOptionIndex
	{
		get
		{
			return _eRingSizeOptionIndex;
		}
		set
		{
			_eRingSizeOptionIndex = value;
			RaisePropertyChanged("ERingSizeOptionIndex");
		}
	}

	[DataMember]
	public List<string> ERingSizeOptions
	{
		get
		{
			return _eRingSizeOptions;
		}
		set
		{
			_eRingSizeOptions = value;
			RaisePropertyChanged("ERingSizeOptions");
		}
	}

	[DataMember]
	public bool IsFilteredOut
	{
		get
		{
			return _isFilteredOut;
		}
		set
		{
			_isFilteredOut = value;
			RaisePropertyChanged("IsFilteredOut");
		}
	}

	[DataMember]
	public bool GenericBool
	{
		get
		{
			return _genericBool;
		}
		set
		{
			_genericBool = value;
			RaisePropertyChanged("GenericBool");
		}
	}

	[DataMember]
	public int GenericInt
	{
		get
		{
			return _genericInt;
		}
		set
		{
			_genericInt = value;
			RaisePropertyChanged("GenericInt");
		}
	}

	[DataMember]
	public double GenericDouble
	{
		get
		{
			return _genericDouble;
		}
		set
		{
			_genericDouble = value;
			RaisePropertyChanged("GenericDouble");
		}
	}

	[DataMember]
	public string GenericString
	{
		get
		{
			return _genericString;
		}
		set
		{
			_genericString = value;
			RaisePropertyChanged("GenericString");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public UIControl(string categoryName, string controlName, double minimum1, double maximum1, double value1, double step1, double minimum2, double maximum2, double value2, double step2, UIControlType uiControlType, UIControlStatus uiControlStatus, string controlSpecifier, string controlSpecifierValue, UIControlStackLocation uiControlStackLocation)
	{
		_categoryName = categoryName;
		_controlName = controlName;
		_englishControlName = controlName;
		_controlSpecifier = controlSpecifier;
		_controlSpecifierValue = controlSpecifierValue;
		_minimum1 = minimum1;
		_maximum1 = maximum1;
		_value1 = value1;
		_step1 = step1;
		_minimum2 = minimum2;
		_maximum2 = maximum2;
		_value2 = value2;
		_step2 = step2;
		_currentUIControlType = uiControlType;
		_currentUIControlStatus = uiControlStatus;
		_currentUIControlStackLocation = uiControlStackLocation;
	}

	public UIControl(string categoryName, string controlName, double minimum1, double maximum1, double value1, double step1, UIControlType uiControlType, UIControlStatus uiControlStatus, string controlSpecifier, string controlSpecifierValue, UIControlStackLocation uiControlStackLocation)
	{
		_categoryName = categoryName;
		_controlName = controlName;
		_englishControlName = controlName;
		_controlSpecifier = controlSpecifier;
		_controlSpecifierValue = controlSpecifierValue;
		_minimum1 = minimum1;
		_maximum1 = maximum1;
		_value1 = value1;
		_step1 = step1;
		_currentUIControlType = uiControlType;
		_currentUIControlStatus = uiControlStatus;
		_currentUIControlStackLocation = uiControlStackLocation;
	}

	public UIControl()
	{
	}

	private void RaisePropertyChanged(string info)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
	}
}
