using System.Linq;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class BreadCrumbViewModel : ViewModelBase
{
	private string _designName;

	private bool _isOrderingVisible;

	private bool _isEditingVisible;

	private bool _isSearchVisible;

	private bool _isOrdering;

	private bool _isEditing;

	private bool _isSearch;

	private bool _isHomeVisible;

	private string _labelSearch;

	private string _labelEditing;

	private bool _isDesignLoaded;

	private bool _isHome;

	public bool IsDesignLoaded
	{
		get
		{
			return _isDesignLoaded;
		}
		set
		{
			_isDesignLoaded = value;
			RaisePropertyChanged("IsDesignLoaded");
			if (!value)
			{
				LabelEditing = "Freehand";
			}
		}
	}

	public RelayCommand CommandNavigateToHome { get; set; }

	public RelayCommand CommandNavigateToSearch { get; set; }

	public RelayCommand CommandNavigateToCustomize { get; set; }

	public RelayCommand CommandNavigateToOrder { get; set; }

	public string LabelSearch
	{
		get
		{
			return _labelSearch;
		}
		set
		{
			_labelSearch = value;
			RaisePropertyChanged("LabelSearch");
		}
	}

	public string LabelEditing
	{
		get
		{
			return _labelEditing;
		}
		set
		{
			_labelEditing = value;
			RaisePropertyChanged("LabelEditing");
		}
	}

	public bool IsHomeVisible
	{
		get
		{
			return _isHomeVisible;
		}
		set
		{
			_isHomeVisible = value;
			RaisePropertyChanged("IsHomeVisible");
		}
	}

	public bool IsSearch
	{
		get
		{
			return _isSearch;
		}
		set
		{
			if (value != _isSearch)
			{
				_isSearch = value;
				RaisePropertyChanged("IsSearch");
			}
		}
	}

	public bool IsEditing
	{
		get
		{
			return _isEditing;
		}
		set
		{
			if (value != _isEditing)
			{
				_isEditing = value;
				RaisePropertyChanged("IsEditing");
			}
		}
	}

	public bool IsOrdering
	{
		get
		{
			return _isOrdering;
		}
		set
		{
			if (value != _isOrdering)
			{
				_isOrdering = value;
				RaisePropertyChanged("IsOrdering");
			}
		}
	}

	public bool IsSearchVisible
	{
		get
		{
			return _isSearchVisible;
		}
		set
		{
			_isSearchVisible = value;
			RaisePropertyChanged("IsSearchVisible");
		}
	}

	public bool IsEditingVisible
	{
		get
		{
			return _isEditingVisible;
		}
		set
		{
			_isEditingVisible = value;
			RaisePropertyChanged("IsEditingVisible");
		}
	}

	public bool IsOrderingVisible
	{
		get
		{
			return _isOrderingVisible;
		}
		set
		{
			_isOrderingVisible = value;
			RaisePropertyChanged("IsOrderingVisible");
		}
	}

	public string DesignName
	{
		get
		{
			return _designName;
		}
		set
		{
			_designName = value;
			RaisePropertyChanged("DesignName");
		}
	}

	public bool IsHome
	{
		get
		{
			return _isHome;
		}
		set
		{
			if (value != _isHome)
			{
				_isHome = value;
				RaisePropertyChanged("IsHome");
			}
		}
	}

	public BreadCrumbViewModel()
	{
		IsDesignLoaded = false;
		LabelEditing = "Freehand";
		if (base.IsInDesignMode)
		{
			Reset();
			LabelEditing = "SCS-020234";
			LabelSearch = "New Designs";
			NavigateToSection(Sections.Customize);
		}
		else
		{
			InitializeCommands();
		}
	}

	public void NavigateToSection(Sections sectionNumber)
	{
		GlobalErrorHandler.WriteMethodInfo("NavigateToSection");
		LabelSearch = null;
		LabelEditing = null;
		LabelSearch = ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.SelectedSearchTileCategory?.Title;
		if (ViewModelLocator.Current.TileSearchMainVM.IsKeywordPopulated)
		{
			LabelSearch = Strings.KeywordSearch;
		}
		if (IsDesignLoaded)
		{
			string labelEditing = (ViewModelLocator.Current.BreadCrumbVM.LabelEditing = UIHelper.LoadedDesign.SPINumber);
			LabelEditing = labelEditing;
		}
		else if (ViewModelLocator.Current.CustomizePartsListVM.ImportGroupsCollection.Any())
		{
			LabelEditing = Strings.Freehand;
		}
		switch (sectionNumber)
		{
		case Sections.Tiles:
			if (!IsDesignLoaded)
			{
				LabelSearch = null;
			}
			IsHome = true;
			IsSearch = false;
			IsEditing = false;
			IsOrdering = false;
			IsHomeVisible = !string.IsNullOrWhiteSpace(LabelEditing);
			IsSearchVisible = !string.IsNullOrWhiteSpace(LabelSearch);
			IsEditingVisible = !string.IsNullOrWhiteSpace(LabelEditing);
			RefreshOrderVisibility();
			break;
		case Sections.Search:
			IsHome = false;
			IsSearch = true;
			IsEditing = false;
			IsOrdering = false;
			IsHomeVisible = true;
			IsSearchVisible = true;
			IsEditingVisible = !string.IsNullOrWhiteSpace(LabelEditing);
			RefreshOrderVisibility();
			break;
		case Sections.Customize:
			if (!IsDesignLoaded)
			{
				LabelEditing = Strings.Freehand;
			}
			IsHome = false;
			IsSearch = false;
			IsEditing = true;
			IsOrdering = false;
			IsHomeVisible = true;
			if (LabelEditing == Strings.Freehand && LabelSearch != Strings.KeywordSearch)
			{
				IsSearchVisible = false;
			}
			else
			{
				IsSearchVisible = !string.IsNullOrWhiteSpace(LabelSearch);
			}
			ViewModelLocator.Current.MainVM.IsViewportControlsVisible = true;
			IsEditingVisible = true;
			RefreshOrderVisibility();
			break;
		case Sections.Order:
			IsHome = false;
			IsSearch = false;
			IsEditing = false;
			IsOrdering = true;
			IsHomeVisible = true;
			if (LabelEditing == Strings.Freehand && LabelSearch != Strings.KeywordSearch)
			{
				IsSearchVisible = false;
			}
			else
			{
				IsSearchVisible = !string.IsNullOrWhiteSpace(LabelSearch);
			}
			ViewModelLocator.Current.MainVM.IsViewportControlsVisible = false;
			IsEditingVisible = true;
			IsOrderingVisible = true;
			break;
		}
		if (UIHelper.KeyProfile.IsDesignLibraryDisabled)
		{
			IsHomeVisible = false;
			IsSearchVisible = false;
		}
		GlobalErrorHandler.WriteInfoToEventLog("End Navigate Breadcrumb", 2);
	}

	public void RefreshOrderVisibility()
	{
		IsOrderingVisible = IsDesignLoaded || ViewModelLocator.Current.CustomizePartsListVM.ImportGroupsCollection.Any();
		if (LabelEditing == Strings.Freehand && LabelSearch != Strings.KeywordSearch)
		{
			IsSearchVisible = false;
		}
		else
		{
			IsSearchVisible = !string.IsNullOrWhiteSpace(LabelSearch);
		}
	}

	public void Reset()
	{
		UIHelper.AllowTileNavigation = true;
		IsDesignLoaded = false;
		LabelEditing = null;
		LabelSearch = null;
		NavigateToSection(Sections.Tiles);
	}

	private void InitializeCommands()
	{
		CommandNavigateToCustomize = new RelayCommand(NavigateToCustomize);
		CommandNavigateToHome = new RelayCommand(NavigateToHome);
		CommandNavigateToOrder = new RelayCommand(NavigateToOrder);
		CommandNavigateToSearch = new RelayCommand(NavigateToSearch);
	}

	private void NavigateToCustomize()
	{
		if (!IsEditing && UIHelper.AllowTileNavigation)
		{
			GlobalErrorHandler.WriteMethodInfo("NavigateToCustomize");
			NavigateToSection(Sections.Customize);
			ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Customize, notifyBreadCrumb: false);
			ViewModelLocator.Current.CustomizeVM.IsOrderSectionVisible = false;
		}
	}

	private void NavigateToHome()
	{
		if (!IsHome && UIHelper.AllowTileNavigation)
		{
			GlobalErrorHandler.WriteMethodInfo("NavigateToHome");
			NavigateToSection(Sections.Tiles);
			ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Tiles, notifyBreadCrumb: false);
			ViewModelLocator.Current.CustomizeVM.IsOrderSectionVisible = false;
		}
	}

	private void NavigateToOrder()
	{
		if (!IsOrdering && UIHelper.AllowTileNavigation)
		{
			GlobalErrorHandler.WriteMethodInfo("NavigateToOrder");
			NavigateToSection(Sections.Order);
			ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Customize, notifyBreadCrumb: false);
			ViewModelLocator.Current.CustomizeVM.IsOrderSectionVisible = true;
		}
	}

	private void NavigateToSearch()
	{
		if (!IsSearch && UIHelper.AllowTileNavigation)
		{
			GlobalErrorHandler.WriteMethodInfo("NavigateToSearch");
			NavigateToSection(Sections.Search);
			ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Search, notifyBreadCrumb: false);
			ViewModelLocator.Current.CustomizeVM.IsOrderSectionVisible = false;
		}
	}
}
