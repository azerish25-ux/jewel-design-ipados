using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.UserControls.PagedListBox;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight.Command;

namespace GV.CounterSketch.Studio.ViewModel;

public class MaterialOptionSelectorViewModel : ItemChooserViewModelBase
{
	private ObservableCollection<MaterialOptionItem> _availableMaterialOptions;

	private string _controlTitle;

	private bool _isMetalMaterialSelection;

	private bool _isOrderingEnabled;

	private bool _isPopupOpen;

	private MaterialOptionItem _scrollToColorItem;

	private GemColorType _selectedGemColorType;

	private MaterialOptionItem _selectedMaterialOptionItem;

	private bool _isCenterGemMode;

	public bool IsCenterGemMode
	{
		get
		{
			return _isCenterGemMode;
		}
		set
		{
			_isCenterGemMode = value;
			RaisePropertyChanged("IsCenterGemMode");
		}
	}

	public ObservableCollection<MaterialOptionItem> AvailableMaterialOptions
	{
		get
		{
			return _availableMaterialOptions;
		}
		set
		{
			if (_availableMaterialOptions != value)
			{
				_availableMaterialOptions = value;
				RaisePropertyChanged("AvailableMaterialOptions");
			}
		}
	}

	public RelayCommand CommandApplyAnyGemAnyColor { get; set; }

	public RelayCommand CommandClearAllAnyGemAnyColor { get; set; }

	public RelayCommand CommandCloseAnyGemAnyColor { get; set; }

	public RelayCommand<GemColorType> CommandGemColorTypeClicked { get; set; }

	public string ControlTitle
	{
		get
		{
			return _controlTitle;
		}
		set
		{
			_controlTitle = value;
			RaisePropertyChanged(() => ControlTitle);
		}
	}

	public MaterialAssignmentMode CurrentAssignmentMode { get; set; }

	public MaterialOptionItem CurrentlyActiveMaterialOptionItem { get; set; }

	public IPagedListBoxItem FirstVisibleItem
	{
		set
		{
			if (value is MaterialOptionItem materialOptionItem)
			{
				_selectedGemColorType = materialOptionItem.ColorGroup;
				RaisePropertyChanged(() => SelectedGemColorType);
			}
		}
	}

	public bool IsMetalMaterialSelection
	{
		get
		{
			return _isMetalMaterialSelection;
		}
		set
		{
			_isMetalMaterialSelection = value;
			RaisePropertyChanged(() => IsMetalMaterialSelection);
		}
	}

	public bool IsOrderingEnabled
	{
		get
		{
			return _isOrderingEnabled;
		}
		set
		{
			if (_isOrderingEnabled != value)
			{
				_isOrderingEnabled = value;
				RaisePropertyChanged("IsOrderingEnabled");
			}
		}
	}

	public bool IsPopupOpen
	{
		get
		{
			return _isPopupOpen;
		}
		set
		{
			_isPopupOpen = value;
			RaisePropertyChanged(() => IsPopupOpen);
		}
	}

	public MaterialOptionItem ScrollToColorItem
	{
		get
		{
			return _scrollToColorItem;
		}
		set
		{
			_scrollToColorItem = value;
			RaisePropertyChanged(() => ScrollToColorItem);
		}
	}

	public GemColorType SelectedGemColorType
	{
		get
		{
			return _selectedGemColorType;
		}
		set
		{
			_selectedGemColorType = value;
			RaisePropertyChanged(() => SelectedGemColorType);
			ScrollToColorItem = AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem materialOption) => materialOption.ColorGroup == _selectedGemColorType);
		}
	}

	public MaterialOptionItem SelectedMaterialOptionItem
	{
		get
		{
			return _selectedMaterialOptionItem;
		}
		set
		{
			if (value == null)
			{
				GlobalErrorHandler.WriteErrorToEventLog($"Null Passed to SelectedMaterialOptionItem()\n{new StackTrace()}");
			}
			else
			{
				if (_selectedMaterialOptionItem == value)
				{
					return;
				}
				MaterialOptionItem materialOptionItem = AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem availableItem) => availableItem.SearchAttributeId == value.SearchAttributeId);
				if (materialOptionItem != null)
				{
					_selectedMaterialOptionItem = materialOptionItem;
					_selectedGemColorType = _selectedMaterialOptionItem.ColorGroup;
					RaisePropertyChanged(() => SelectedGemColorType);
					RaisePropertyChanged(() => SelectedMaterialOptionItem);
					RaiseMaterialOptionSelectionChanged();
					if (CurrentAssignmentMode == MaterialAssignmentMode.GemCenter && ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
					{
						ViewModelLocator.Current.StoneTrayVM.ClearAssignedStoneCommand.Execute(null);
					}
					base.MouseOverDescription = materialOptionItem.Description;
					base.MouseOverText = materialOptionItem.PrimaryText;
				}
			}
		}
	}

	public event EventHandler MaterialOptionSelectionChanged;

	public MaterialOptionSelectorViewModel(string title, MaterialAssignmentMode materialAssignmentMode, int selectedMaterialId = -1, bool isOrderingEnabled = false)
	{
		CurrentAssignmentMode = materialAssignmentMode;
		switch (materialAssignmentMode)
		{
		case MaterialAssignmentMode.MetalPrimary:
		case MaterialAssignmentMode.MetalSecondary:
			ControlTitle = "METALS";
			IsMetalMaterialSelection = true;
			AvailableMaterialOptions = new ObservableCollection<MaterialOptionItem>(MetalHelper.GetMetalMaterialOptionItems());
			IsOrderingEnabled = false;
			break;
		case MaterialAssignmentMode.GemCenter:
		case MaterialAssignmentMode.GemSide:
		case MaterialAssignmentMode.GemAccent:
		case MaterialAssignmentMode.GemAnyColor:
			ControlTitle = "STONES";
			IsMetalMaterialSelection = false;
			IsOrderingEnabled = isOrderingEnabled;
			AvailableMaterialOptions = new ObservableCollection<MaterialOptionItem>(GemHelper.GetGemMaterialOptionItems(isListFiltered: true));
			break;
		}
		IsCenterGemMode = materialAssignmentMode == MaterialAssignmentMode.GemCenter;
		if (selectedMaterialId >= 0)
		{
			SelectedMaterialOptionItem = AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem materialOptionItem) => materialOptionItem.SearchAttributeId == selectedMaterialId);
		}
		base.Title = title;
		base.Columns = (IsMetalMaterialSelection ? 5 : 8);
		if (!base.IsInDesignMode)
		{
			InitiailizeCommands();
			base.CommandUpdateMouseOverText = new RelayCommand<MaterialOptionItem>(UpdateMouseOverText);
			base.CommandResetMouseOverText = new RelayCommand(ResetMouseOverText);
		}
	}

	public MaterialOptionSelectorViewModel()
		: this("Center Gem", MaterialAssignmentMode.GemCenter)
	{
		if (base.IsInDesignMode)
		{
			IsCenterGemMode = true;
		}
	}

	public void SetCurrentMaterialOptionItemWithoutNotification(MaterialOptionItem value)
	{
		MaterialOptionItem materialOptionItem = AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem availableItem) => availableItem.SearchAttributeId == value.SearchAttributeId);
		if (materialOptionItem != null)
		{
			_selectedMaterialOptionItem = materialOptionItem;
			_selectedGemColorType = _selectedMaterialOptionItem.ColorGroup;
			RaisePropertyChanged(() => SelectedGemColorType);
			RaisePropertyChanged(() => SelectedMaterialOptionItem);
		}
	}

	public void InitializeSelectedMaterialOptionItem(MaterialOptionItem selectedMaterialOptionItem)
	{
		_selectedMaterialOptionItem = selectedMaterialOptionItem;
		RaisePropertyChanged(() => SelectedMaterialOptionItem);
		base.MouseOverText = selectedMaterialOptionItem.PrimaryText;
		base.MouseOverDescription = selectedMaterialOptionItem.Description;
	}

	public void RaiseMaterialOptionSelectionChanged()
	{
		if (CurrentAssignmentMode != MaterialAssignmentMode.GemAnyColor && this.MaterialOptionSelectionChanged != null)
		{
			Application.Current.Dispatcher.Invoke(delegate
			{
				this.MaterialOptionSelectionChanged(this, new EventArgs());
			});
		}
	}

	private void ApplyAnyGemAnyColor()
	{
		if (SelectedMaterialOptionItem != null)
		{
			EventHelper.RecordEvent(EventHelper.EventTypes.AnyGemAnyColor);
			if (UIHelper.CoreDataService.DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems((GemMaterial)SelectedMaterialOptionItem.SearchAttributeId) && ViewModelLocator.Current.StoneTrayVM != null)
			{
				ViewModelLocator.Current.StoneTrayVM.ClearAssignedStoneCommand.Execute(null);
			}
		}
	}

	private void ClearAllAnyGemAnyColor()
	{
		UIHelper.CoreDataService.DocumentClearAllAnyGemAnyColor();
		ViewModelLocator.Current.GemReportVM.Populate();
	}

	private void CloseAnyGemAnyColor()
	{
		ViewModelLocator.Current.MainVM.CloseAnyGemAnyColorWindow();
	}

	private void GemColorTypeClicked(GemColorType gemColorType)
	{
		_selectedGemColorType = gemColorType;
		RaisePropertyChanged(() => SelectedGemColorType);
	}

	private void InitiailizeCommands()
	{
		CommandClearAllAnyGemAnyColor = new RelayCommand(ClearAllAnyGemAnyColor);
		CommandCloseAnyGemAnyColor = new RelayCommand(CloseAnyGemAnyColor);
		CommandApplyAnyGemAnyColor = new RelayCommand(ApplyAnyGemAnyColor);
		CommandGemColorTypeClicked = new RelayCommand<GemColorType>(GemColorTypeClicked);
	}

	private void ResetMouseOverText()
	{
		base.MouseOverText = SelectedMaterialOptionItem.PrimaryText;
		base.MouseOverDescription = SelectedMaterialOptionItem.Description;
	}

	private void UpdateMouseOverText(MaterialOptionItem materialOptionItem)
	{
		base.MouseOverText = materialOptionItem.PrimaryText;
		base.MouseOverDescription = materialOptionItem.Description;
	}
}
