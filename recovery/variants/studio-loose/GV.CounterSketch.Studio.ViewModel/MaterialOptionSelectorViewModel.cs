using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.UserControls.PagedListBox;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
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
			((ObservableObject)this).RaisePropertyChanged("IsCenterGemMode");
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
				((ObservableObject)this).RaisePropertyChanged("AvailableMaterialOptions");
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
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => ControlTitle));
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
				((ObservableObject)this).RaisePropertyChanged<GemColorType>((Expression<Func<GemColorType>>)(() => SelectedGemColorType));
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
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsMetalMaterialSelection));
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
				((ObservableObject)this).RaisePropertyChanged("IsOrderingEnabled");
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
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsPopupOpen));
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
			((ObservableObject)this).RaisePropertyChanged<MaterialOptionItem>((Expression<Func<MaterialOptionItem>>)(() => ScrollToColorItem));
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
			((ObservableObject)this).RaisePropertyChanged<GemColorType>((Expression<Func<GemColorType>>)(() => SelectedGemColorType));
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
					((ObservableObject)this).RaisePropertyChanged<GemColorType>((Expression<Func<GemColorType>>)(() => SelectedGemColorType));
					((ObservableObject)this).RaisePropertyChanged<MaterialOptionItem>((Expression<Func<MaterialOptionItem>>)(() => SelectedMaterialOptionItem));
					RaiseMaterialOptionSelectionChanged();
					if (CurrentAssignmentMode == MaterialAssignmentMode.GemCenter && ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
					{
						ViewModelLocator.Current.StoneTrayVM.ClearAssignedStoneCommand.Execute((object)null);
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
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
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
		if (!((ViewModelBase)this).IsInDesignMode)
		{
			InitiailizeCommands();
			base.CommandUpdateMouseOverText = new RelayCommand<MaterialOptionItem>((Action<MaterialOptionItem>)UpdateMouseOverText, false);
			base.CommandResetMouseOverText = new RelayCommand((Action)ResetMouseOverText, false);
		}
	}

	public MaterialOptionSelectorViewModel()
		: this("Center Gem", MaterialAssignmentMode.GemCenter)
	{
		if (((ViewModelBase)this).IsInDesignMode)
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
			((ObservableObject)this).RaisePropertyChanged<GemColorType>((Expression<Func<GemColorType>>)(() => SelectedGemColorType));
			((ObservableObject)this).RaisePropertyChanged<MaterialOptionItem>((Expression<Func<MaterialOptionItem>>)(() => SelectedMaterialOptionItem));
		}
	}

	public void InitializeSelectedMaterialOptionItem(MaterialOptionItem selectedMaterialOptionItem)
	{
		_selectedMaterialOptionItem = selectedMaterialOptionItem;
		((ObservableObject)this).RaisePropertyChanged<MaterialOptionItem>((Expression<Func<MaterialOptionItem>>)(() => SelectedMaterialOptionItem));
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
				ViewModelLocator.Current.StoneTrayVM.ClearAssignedStoneCommand.Execute((object)null);
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
		((ObservableObject)this).RaisePropertyChanged<GemColorType>((Expression<Func<GemColorType>>)(() => SelectedGemColorType));
	}

	private void InitiailizeCommands()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		CommandClearAllAnyGemAnyColor = new RelayCommand((Action)ClearAllAnyGemAnyColor, false);
		CommandCloseAnyGemAnyColor = new RelayCommand((Action)CloseAnyGemAnyColor, false);
		CommandApplyAnyGemAnyColor = new RelayCommand((Action)ApplyAnyGemAnyColor, false);
		CommandGemColorTypeClicked = new RelayCommand<GemColorType>((Action<GemColorType>)GemColorTypeClicked, false);
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
