using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class SearchFilterViewModel : ItemChooserViewModelBase
{
	private bool _isFilterDisabled;

	private SearchFilterItem _selectedSearchFilterItem;

	private bool _isOpen;

	private ObservableCollection<SearchFilterItem> _searchFilterItems;

	private List<int> _previousSelectedItems;

	private List<string> _selectedSearchFilterItemImages;

	private string _selectedItemLabel;

	public new string TitleTranslated => TranslationHelper.GetTranslatedString(base.Title);

	public TileSearchResultViewModelBase ParentViewModel { get; set; }

	public bool IsOpen
	{
		get
		{
			return _isOpen;
		}
		set
		{
			_isOpen = value;
			RaisePropertyChanged("IsOpen");
			if (value)
			{
				PreviousSelectedItems = (from _ in SearchFilterItems
					where _.IsSelected
					select _.SearchAttributeId).ToList();
			}
		}
	}

	public ObservableCollection<SearchFilterItem> SearchFilterItems
	{
		get
		{
			return _searchFilterItems;
		}
		set
		{
			_searchFilterItems = value;
			RaisePropertyChanged("SearchFilterItems");
		}
	}

	public List<int> PreviousSelectedItems
	{
		get
		{
			return _previousSelectedItems;
		}
		set
		{
			_previousSelectedItems = value;
			RaisePropertyChanged("PreviousSelectedItems");
		}
	}

	public string SelectedItemLabel
	{
		get
		{
			return _selectedItemLabel;
		}
		set
		{
			_selectedItemLabel = value;
			RaisePropertyChanged("SelectedItemLabel");
		}
	}

	public List<string> SelectedSearchFilterItemImages
	{
		get
		{
			return _selectedSearchFilterItemImages;
		}
		set
		{
			_selectedSearchFilterItemImages = value;
			RaisePropertyChanged("SelectedSearchFilterItemImages");
		}
	}

	public SearchFilterItem SelectedSearchFilterItem
	{
		get
		{
			return _selectedSearchFilterItem;
		}
		set
		{
			if (_selectedSearchFilterItem != value)
			{
				_selectedSearchFilterItem = value;
				RaisePropertyChanged("SelectedSearchFilterItem");
			}
			SelectedSearchFilterItemImages = (from _ in SearchFilterItems
				where _.IsSelected
				select _.ImagePath).ToList();
			if (SelectedSearchFilterItemImages.Count > 1)
			{
				SelectedItemLabel = "Mixed";
			}
			else if (value == null)
			{
				SelectedItemLabel = "Show All";
			}
			else
			{
				SelectedItemLabel = value.PrimaryText;
			}
		}
	}

	public bool IsFilterDisabled
	{
		get
		{
			return _isFilterDisabled;
		}
		set
		{
			_isFilterDisabled = value;
			RaisePropertyChanged("IsFilterDisabled");
		}
	}

	public SearchFilterViewModel()
	{
		if (base.IsInDesignMode)
		{
			base.Title = "Sample Filter";
			SearchFilterItem item = new SearchFilterItem
			{
				ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemColors/105_Diamond.jpg",
				PrimaryText = "Round"
			};
			SearchFilterItem item2 = new SearchFilterItem
			{
				ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemColors/430_TopazPoppy.jpg",
				SearchAttributeId = -1,
				PrimaryText = "Show All"
			};
			SearchFilterItems = new ObservableCollection<SearchFilterItem>
			{
				item2, item, item, item, item, item, item, item, item, item,
				item, item
			};
		}
		else
		{
			base.CommandOK = new RelayCommand(OKClicked);
			base.CommandCancel = new RelayCommand(CancelClicked);
			base.CommandClear = new RelayCommand(ClearClicked);
		}
	}

	public void UpdateDisabledStatus()
	{
		if (base.Title == "Center Shape")
		{
			IsFilterDisabled = ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned;
		}
		else
		{
			IsFilterDisabled = false;
		}
	}

	private void CancelClicked()
	{
		SearchFilterItems.ToList().ForEach(delegate(SearchFilterItem _)
		{
			_.IsSelected = false;
		});
		PreviousSelectedItems.ForEach(delegate(int _)
		{
			SearchFilterItem searchFilterItem4 = SearchFilterItems.LastOrDefault((SearchFilterItem i) => i.SearchAttributeId == _);
			if (searchFilterItem4 != null)
			{
				searchFilterItem4.IsSelected = true;
			}
		});
		SearchFilterItem searchFilterItem = (SelectedSearchFilterItem = SearchFilterItems.FirstOrDefault((SearchFilterItem _) => _.IsSelected));
		SearchFilterItem searchFilterItem3 = searchFilterItem;
		SelectedSearchFilterItem = searchFilterItem3 ?? SearchFilterItems.Last();
		Cancel();
	}

	private void ClearClicked()
	{
		SearchFilterItems.ToList().ForEach(delegate(SearchFilterItem _)
		{
			_.IsSelected = false;
		});
		SelectedSearchFilterItem = SearchFilterItems.Last();
		SelectedItemLabel = "Show All";
		Cancel();
		ParentViewModel.UpdateSearchResults(this);
	}

	private void OKClicked()
	{
		SearchFilterItem searchFilterItem = (SelectedSearchFilterItem = SearchFilterItems.FirstOrDefault((SearchFilterItem _) => _.IsSelected));
		SearchFilterItem searchFilterItem3 = searchFilterItem;
		SelectedSearchFilterItem = searchFilterItem3 ?? SearchFilterItems.Last();
		Cancel();
		ParentViewModel.UpdateSearchResults(this);
	}
}
