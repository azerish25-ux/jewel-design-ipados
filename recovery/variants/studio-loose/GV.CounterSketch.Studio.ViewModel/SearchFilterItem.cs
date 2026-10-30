using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GV.CounterSketch.Common.Annotations;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.UserControls.PagedListBox;

namespace GV.CounterSketch.Studio.ViewModel;

public class SearchFilterItem : INotifyPropertyChanged, ICloneable, IPagedListBoxItem
{
	private bool _isSelected;

	public long CSOrder { get; set; }

	public List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> GroupedAttributes { get; set; }

	public bool IsDesignedByFilter { get; set; }

	public int SearchAttributeId { get; set; }

	public string Abbreviation { get; set; }

	public string Description { get; set; }

	public string ImagePath { get; set; }

	public bool IsFavorite { get; set; }

	public bool IsReadOnly { get; set; }

	public string PrimaryText { get; set; }

	public string SecondaryText { get; set; }

	public bool IsSelected
	{
		get
		{
			return _isSelected;
		}
		set
		{
			_isSelected = value;
			OnPropertyChanged("IsSelected");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public object Clone()
	{
		return MemberwiseClone();
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
