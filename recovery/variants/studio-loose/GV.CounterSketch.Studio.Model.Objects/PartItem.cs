using System;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.Model.Objects;

public class PartItem : ViewModelBase
{
	private bool _isCounterPart;

	private string _item3DMPath;

	private string _itemImagePath;

	private string _name;

	private Guid _uniqueId;

	private DateTime _createDate;

	public bool IsCounterPart
	{
		get
		{
			return _isCounterPart;
		}
		set
		{
			_isCounterPart = value;
			((ObservableObject)this).RaisePropertyChanged("IsCounterPart");
		}
	}

	public string Item3DMPath
	{
		get
		{
			return _item3DMPath;
		}
		set
		{
			_item3DMPath = value;
			((ObservableObject)this).RaisePropertyChanged("Item3DMPath");
		}
	}

	public string ItemImagePath
	{
		get
		{
			return _itemImagePath;
		}
		set
		{
			_itemImagePath = value;
			((ObservableObject)this).RaisePropertyChanged("ItemImagePath");
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			((ObservableObject)this).RaisePropertyChanged("Name");
		}
	}

	public DateTime CreateDate
	{
		get
		{
			return _createDate;
		}
		set
		{
			_createDate = value;
			((ObservableObject)this).RaisePropertyChanged("CreateDate");
		}
	}

	public Guid UniqueId
	{
		get
		{
			return _uniqueId;
		}
		set
		{
			_uniqueId = value;
			((ObservableObject)this).RaisePropertyChanged("UniqueId");
		}
	}
}
