using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using GV.CounterSketch.Shared.Annotations;

namespace GV.CounterSketch.Shared.Model.Objects;

[Serializable]
public class ImportGroup : INotifyPropertyChanged
{
	private BitmapImage _importGroupBitmapImage;

	private bool _isSelectedImportGroup;

	private bool _isSecondarySelectedImportGroup;

	private bool _isParentPart;

	private double _xScale = 1.0;

	private double _yScale = 1.0;

	private double _yHeight;

	private List<Guid> _blockIdentifiers;

	private bool _isViewportVisible = true;

	private string _name;

	[XmlIgnore]
	public bool IsSelected
	{
		get
		{
			if (_isSecondarySelectedImportGroup || _isSelectedImportGroup)
			{
				return true;
			}
			return false;
		}
	}

	public Guid CenterGemControlGuid { get; set; }

	public Guid PartGroupId { get; set; }

	public string PartGroupName { get; set; }

	public bool IsGroup { get; set; }

	[XmlIgnore]
	public double XScale
	{
		get
		{
			return _xScale;
		}
		set
		{
			_xScale = value;
			RaisePropertyChanged("XScale");
		}
	}

	[XmlIgnore]
	public double YScale
	{
		get
		{
			return _yScale;
		}
		set
		{
			_yScale = value;
			RaisePropertyChanged("YScale");
		}
	}

	[XmlIgnore]
	public double YHeight
	{
		get
		{
			return _yHeight;
		}
		set
		{
			_yHeight = value;
			RaisePropertyChanged("YHeight");
		}
	}

	public List<Guid> BlockIdentifiers
	{
		get
		{
			return _blockIdentifiers;
		}
		set
		{
			_blockIdentifiers = value;
		}
	}

	public Guid Identifier { get; set; }

	[XmlIgnore]
	public BitmapImage ImportGroupBitmapImage
	{
		get
		{
			return _importGroupBitmapImage;
		}
		set
		{
			_importGroupBitmapImage = value;
			RaisePropertyChanged("ImportGroupBitmapImage");
		}
	}

	public bool IsPrimaryMetal { get; set; }

	[XmlIgnore]
	public bool IsSelectedImportGroup
	{
		get
		{
			return _isSelectedImportGroup;
		}
		set
		{
			if (_isSelectedImportGroup != value)
			{
				_isSelectedImportGroup = value;
				RaisePropertyChanged("IsSelectedImportGroup");
				RaisePropertyChanged("IsSelected");
				HasSelectionChanged = true;
			}
		}
	}

	[XmlIgnore]
	public bool IsSecondarySelectedImportGroup
	{
		get
		{
			return _isSecondarySelectedImportGroup;
		}
		set
		{
			if (_isSecondarySelectedImportGroup != value)
			{
				_isSecondarySelectedImportGroup = value;
				if (value)
				{
					XScale = 0.9;
					YScale = 0.9;
					YHeight = 6.0;
				}
				else
				{
					XScale = 1.0;
					YScale = 1.0;
					YHeight = 0.0;
				}
				RaisePropertyChanged("IsSecondarySelectedImportGroup");
				RaisePropertyChanged("IsSelected");
				HasSelectionChanged = true;
			}
		}
	}

	[XmlIgnore]
	public bool HasSelectionChanged { get; set; }

	public bool IsViewportVisible
	{
		get
		{
			return _isViewportVisible;
		}
		set
		{
			_isViewportVisible = value;
			RaisePropertyChanged("IsViewportVisible");
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
			RaisePropertyChanged("Name");
		}
	}

	public bool IsParentPart
	{
		get
		{
			return _isParentPart;
		}
		set
		{
			_isParentPart = value;
			RaisePropertyChanged("IsParentPart");
		}
	}

	public Guid FirstGraphIdentifier { get; set; }

	public Guid LastGraphIdentifier { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public void RaiseSelectionStateChanged()
	{
		RaisePropertyChanged("IsSecondarySelectedImportGroup");
		RaisePropertyChanged("IsSelectedImportGroup");
		RaisePropertyChanged("IsSelected");
		RaisePropertyChanged("IsGroup");
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
