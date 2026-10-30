using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Localization;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.ViewModel;

public class StoneTrayViewModel : ViewModelBase
{
	private bool _isStoneAssigned;

	private decimal _providedMaxCaratFilter;

	private decimal _providedMinCaratFilter;

	private string _providedShapeFilter;

	private Diamond _selectedStoneTrayStone;

	private Diamond _stoneChosen;

	private ObservableCollection<Diamond> _stoneResults;

	private bool isBusy;

	private int _stonesInTrayCount;

	public RelayCommand<Diamond> AssignStoneSearchCommand => new RelayCommand<Diamond>((Action<Diamond>)AssignStoneSearch, false);

	public RelayCommand<Diamond> AssignStoneCommand { get; set; }

	public RelayCommand ClearAssignedStoneCommand { get; private set; }

	public RelayCommand<Diamond> DeleteStoneCommand { get; set; }

	public bool IsStoneAssigned
	{
		get
		{
			return _isStoneAssigned;
		}
		set
		{
			_isStoneAssigned = value;
			((ObservableObject)this).RaisePropertyChanged("IsStoneAssigned");
		}
	}

	public RelayCommand<Gem> OpenStoneTrayCommand { get; set; }

	public Gem ProvidedGemToFit { get; set; }

	public Diamond SelectedStoneTrayStone
	{
		get
		{
			return _selectedStoneTrayStone;
		}
		set
		{
			_selectedStoneTrayStone = value;
			((ObservableObject)this).RaisePropertyChanged("SelectedStoneTrayStone");
		}
	}

	public Diamond StoneChosen
	{
		get
		{
			return _stoneChosen;
		}
		set
		{
			_stoneChosen = value;
			((ObservableObject)this).RaisePropertyChanged("StoneChosen");
			IsStoneAssigned = value != null;
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneChosenImagePath));
			OnStoneChosenEvent();
		}
	}

	public string StoneChosenImagePath
	{
		get
		{
			if (StoneChosen != null && _stoneChosen.Images.Any())
			{
				Image val = _stoneChosen.Images.FirstOrDefault();
				if (val != null)
				{
					return val.FullUrl;
				}
			}
			return null;
		}
	}

	public ObservableCollection<Diamond> StoneResults
	{
		get
		{
			if (_stoneResults == null)
			{
				return null;
			}
			ObservableCollection<Diamond> observableCollection = new ObservableCollection<Diamond>();
			foreach (Diamond stoneResult in _stoneResults)
			{
				ViewModelLocator.Current.GemReportVM.IsDiamondAssignedToNonCenterStone(stoneResult.SerialNumber);
				if (StoneChosen != null)
				{
					_ = StoneChosen.SerialNumber == stoneResult.SerialNumber;
				}
				else
					_ = 0;
				if (false)
				{
					stoneResult.Active = true;
				}
				else
				{
					stoneResult.Active = false;
				}
				observableCollection.Add(stoneResult);
			}
			return observableCollection;
		}
		set
		{
			_stoneResults = value;
			((ObservableObject)this).RaisePropertyChanged<ObservableCollection<Diamond>>((Expression<Func<ObservableCollection<Diamond>>>)(() => StoneResults));
			((ObservableObject)this).RaisePropertyChanged<int>((Expression<Func<int>>)(() => StonesInTrayCount));
		}
	}

	public int StonesInTrayCount
	{
		get
		{
			if (StoneResults == null)
			{
				return 0;
			}
			return StoneResults.Count;
		}
	}

	public bool IsBusy
	{
		get
		{
			return isBusy;
		}
		set
		{
			isBusy = value;
			((ObservableObject)this).RaisePropertyChanged("IsBusy");
		}
	}

	public bool AssignmentFailed { get; set; }

	public event EventHandler StoneChosenEvent;

	public StoneTrayViewModel()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		if (((ViewModelBase)this).IsInDesignMode)
		{
			Currency price = new Currency(1000m, "USD");
			List<Diamond> list = new List<Diamond>();
			list.Add(new Diamond
			{
				Shape = "Round",
				CaratWeight = 1.00m,
				Make = "Very Good",
				Color = "F",
				Clarity = "SI",
				Price = price
			});
			list.Add(new Diamond
			{
				Shape = "Pear",
				CaratWeight = 1.00m,
				Make = "Very Good",
				Color = "F",
				Clarity = "SI",
				Price = price
			});
			list.Add(new Diamond
			{
				Shape = "Princess",
				CaratWeight = 1.00m,
				Make = "Very Good",
				Color = "F",
				Clarity = "SI",
				Price = price
			});
			list.ForEach(delegate(Diamond d)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Expected O, but got Unknown
				d.Images.Add(new Image
				{
					ThumbnailUrl = "http://stuller.scene7.com/is/image/Stuller/f43c252f-594e-45af-827d-a28300f9d620?bgc=000000&$geminfo$"
				});
			});
		}
		else
		{
			InitializeCommands();
		}
	}

	public void AddStonesToTray(List<Diamond> serializedStones)
	{
		serializedStones.ForEach(delegate(Diamond serializedStone)
		{
			if (StoneResults == null)
			{
				StoneResults = new ObservableCollection<Diamond> { serializedStone };
			}
			else
			{
				if (_stoneResults.All((Diamond x) => x.SerialNumber != serializedStone.SerialNumber))
				{
					_stoneResults.Add(serializedStone);
				}
				_stoneResults.ToList().ForEach(delegate(Diamond x)
				{
					if (x.SerialNumber == serializedStone.SerialNumber)
					{
						x.Shape = serializedStone.Shape;
						x.ShowcasePrice = serializedStone.ShowcasePrice;
						x.ShowcasePricePerCarat = serializedStone.ShowcasePricePerCarat;
					}
				});
			}
		});
		((ObservableObject)this).RaisePropertyChanged<ObservableCollection<Diamond>>((Expression<Func<ObservableCollection<Diamond>>>)(() => StoneResults));
		((ObservableObject)this).RaisePropertyChanged<int>((Expression<Func<int>>)(() => StonesInTrayCount));
	}

	public void ClearAssignedStone()
	{
		ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned = false;
		if (ProvidedGemToFit != null)
		{
			ProvidedGemToFit.AssignedDiamond = null;
		}
		UIHelper.CoreDataService.DocumentResetCenterGemAnyGemAnyColorMaterialAssignment();
		ViewModelLocator.Current.StudioMaterialSelectionVM.ResetCenterGemToDiamondMetarial();
		ViewModelLocator.Current.StudioMaterialSelectionVM.MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialGroupOption) => materialGroupOption.Title == Strings.CenterStones).MaterialOptionSelectorVM.SetCurrentMaterialOptionItemWithoutNotification(GemHelper.GetGemMaterialOptionItems().ToList().FirstOrDefault((MaterialOptionItem _) => _.GemMaterial == GemMaterial.Diamond));
		Messenger.Default.Send<string>("CloseStoneTray");
		StoneChosen = null;
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.StoneTrayVMStoneChosen();
	}

	public void DeleteStone(Diamond diamond)
	{
		PunchoutHelper.RemoveStoneFromTray(diamond);
		if (_stoneResults != null && _stoneResults.Contains(diamond))
		{
			_stoneResults.Remove(diamond);
			((ObservableObject)this).RaisePropertyChanged<ObservableCollection<Diamond>>((Expression<Func<ObservableCollection<Diamond>>>)(() => StoneResults));
			((ObservableObject)this).RaisePropertyChanged<int>((Expression<Func<int>>)(() => StonesInTrayCount));
		}
		if (StoneChosen != null && diamond.SerialNumber == StoneChosen.SerialNumber)
		{
			ClearAssignedStone();
		}
	}

	public void OpenStoneTray(Gem suppliedGem, double? minCarat, double? maxCarat, string shape)
	{
		ProvidedGemToFit = suppliedGem;
		IsBusy = true;
		Task task = PunchoutHelper.RetrieveStoneTray();
		Task.WhenAll(task).ContinueWith(delegate
		{
			((ObservableObject)this).RaisePropertyChanged("StoneResults");
			IsBusy = false;
		});
		((ObservableObject)this).RaisePropertyChanged<ObservableCollection<Diamond>>((Expression<Func<ObservableCollection<Diamond>>>)(() => StoneResults));
	}

	public void Reset()
	{
		if (_stoneResults != null)
		{
			_stoneResults.Clear();
		}
		ClearAssignedStone();
	}

	public void SetDefaultSelectedStone()
	{
		if (_stoneResults != null)
		{
			SelectedStoneTrayStone = StoneResults.FirstOrDefault();
		}
	}

	protected virtual void OnStoneChosenEvent()
	{
		this.StoneChosenEvent?.Invoke(this, EventArgs.Empty);
	}

	private void AssignStone(Diamond diamond)
	{
		Messenger.Default.Send<string>("CloseStoneTray");
		StoneChosen = diamond;
		if (AssignmentFailed)
		{
			AssignmentFailed = false;
			MessageBox.Show(Strings.Assigned_Stone_Not_Supported, "", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.Cancel, MessageBoxOptions.ServiceNotification);
			ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned = false;
			_stoneChosen = null;
			ViewModelLocator.Current.StudioMaterialSelectionVM.MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialGroupOption) => materialGroupOption.Title == Strings.CenterStones).MaterialOptionSelectorVM.SetCurrentMaterialOptionItemWithoutNotification(GemHelper.GetGemMaterialOptionItems().ToList().FirstOrDefault((MaterialOptionItem _) => _.GemMaterial == GemMaterial.Diamond));
			return;
		}
		MaterialOptionItem materialFromStullerDiamond = GetMaterialFromStullerDiamond(diamond);
		if (materialFromStullerDiamond != null)
		{
			ViewModelLocator.Current.StudioMaterialSelectionVM.MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialGroupOption) => materialGroupOption.Title == Strings.CenterStones).MaterialOptionSelectorVM.SetCurrentMaterialOptionItemWithoutNotification(materialFromStullerDiamond);
			ViewModelLocator.Current.StudioMaterialSelectionVM.GemCenterMaterial = materialFromStullerDiamond;
		}
		else
		{
			GlobalErrorHandler.WriteErrorToEventLog("+++> " + diamond.StoneType + "  ::  Missing Stuller Gem Material Mapping.");
		}
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.StoneTrayVMStoneChosen();
		if (diamond.StoneType == "Diamond")
		{
			Application.Current.Dispatcher.Invoke(delegate
			{
				UIHelper.CoreDataService.DocumentResetCenterGemAnyGemAnyColorMaterialAssignment();
				ViewModelLocator.Current.StudioMaterialSelectionVM.ResetCenterGemToDiamondMetarial();
			});
		}
		else
		{
			Application.Current.Dispatcher.InvokeAsync(delegate
			{
				UIHelper.CoreDataService.DocumentResetCenterGemAnyGemAnyColorMaterialAssignment();
				ViewModelLocator.Current.StudioMaterialSelectionVM.ResetCenterGemToSpecifiedMetarial();
			});
		}
		if (ProvidedGemToFit != null)
		{
			ProvidedGemToFit.AssignedDiamond = diamond;
		}
		ViewModelLocator.Current.OrderSummaryVM.Populate();
	}

	private void AssignStoneSearch(Diamond diamond)
	{
		AssignStone(diamond);
		if (!AssignmentFailed)
		{
			TileSearchCategory searchCategory = ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.TileSearchCategories.FirstOrDefault((TileSearchCategory _) => _.Id == 0);
			ViewModelLocator.Current.MainVM.SetSearchModeActive();
			ViewModelLocator.Current.TileSearchMainVM.SetSelectedSearchTile(searchCategory);
			PunchoutHelper.CloseBrowser();
		}
	}

	private MaterialOptionItem GetMaterialFromStullerDiamond(Diamond gemstone)
	{
		List<MaterialOptionItem> source = GemHelper.GetGemMaterialOptionItems().ToList();
		MaterialOptionItem result = source.FirstOrDefault((MaterialOptionItem _) => _.GemMaterial == GemMaterial.Diamond);
		if (gemstone.IsDiamond)
		{
			return result;
		}
		List<Tuple<string, string, string, GemMaterial>> list = new List<Tuple<string, string, string, GemMaterial>>();
		list.Add(new Tuple<string, string, string, GemMaterial>("Aquamarine", "BL", "Blue", GemMaterial.Aquamarine));
		list.Add(new Tuple<string, string, string, GemMaterial>("Amethyst", "PU", "Purple", GemMaterial.Amethyst));
		list.Add(new Tuple<string, string, string, GemMaterial>("Alexandrite", "CO", "Color Change", GemMaterial.Alexandrite));
		list.Add(new Tuple<string, string, string, GemMaterial>("Aquamarine", "BL", "Blue", GemMaterial.Aquamarine));
		list.Add(new Tuple<string, string, string, GemMaterial>("Emerald", "GR", "Green", GemMaterial.Emerald));
		list.Add(new Tuple<string, string, string, GemMaterial>("Garnet", "PU", "Purple", GemMaterial.SapphirePurple));
		list.Add(new Tuple<string, string, string, GemMaterial>("Garnet", "GR", "DEMANTOID", GemMaterial.GarnetTsavorite));
		list.Add(new Tuple<string, string, string, GemMaterial>("Garnet", "GR", "Green", GemMaterial.GarnetBrazilian));
		list.Add(new Tuple<string, string, string, GemMaterial>("Garnet", "OR", "Orange", GemMaterial.Citrine));
		list.Add(new Tuple<string, string, string, GemMaterial>("Garnet", "BL", "Color Change", GemMaterial.GarnetColorChange));
		list.Add(new Tuple<string, string, string, GemMaterial>("Garnet", "GR", "Red", GemMaterial.GarnetTsavorite));
		list.Add(new Tuple<string, string, string, GemMaterial>("Moonstone", "CL", "CLEAR", GemMaterial.Diamond));
		list.Add(new Tuple<string, string, string, GemMaterial>("Morganite", "PI", "Pink", GemMaterial.Morganite));
		list.Add(new Tuple<string, string, string, GemMaterial>("Opal", "WH", "White", GemMaterial.OpalWhite));
		list.Add(new Tuple<string, string, string, GemMaterial>("Peridot", "GR", "Green", GemMaterial.Peridot));
		list.Add(new Tuple<string, string, string, GemMaterial>("Topaz", "OR", "Orange", GemMaterial.TopazPreciousGolden));
		list.Add(new Tuple<string, string, string, GemMaterial>("Ruby", "RE", "Red", GemMaterial.Ruby));
		list.Add(new Tuple<string, string, string, GemMaterial>("Sapphire", "BL", "Blue", GemMaterial.SapphireBlue));
		list.Add(new Tuple<string, string, string, GemMaterial>("Sapphire", "WH", "White", GemMaterial.SapphireWhite));
		list.Add(new Tuple<string, string, string, GemMaterial>("Sapphire", "PI", "Pink", GemMaterial.SapphirePink));
		list.Add(new Tuple<string, string, string, GemMaterial>("Sapphire", "YE", "Yellow", GemMaterial.SapphireYellow));
		list.Add(new Tuple<string, string, string, GemMaterial>("Sapphire", "PU", "Purple", GemMaterial.SapphirePurple));
		list.Add(new Tuple<string, string, string, GemMaterial>("Sapphire", "GR", "Green", GemMaterial.DiamondDarkGreen));
		list.Add(new Tuple<string, string, string, GemMaterial>("Sapphire", "OR", "Orange", GemMaterial.SapphireOrange));
		list.Add(new Tuple<string, string, string, GemMaterial>("Spinel", "PI", "Pink", GemMaterial.SpinelPink));
		list.Add(new Tuple<string, string, string, GemMaterial>("Spinel", "BL", "Blue", GemMaterial.SpinelAqua));
		list.Add(new Tuple<string, string, string, GemMaterial>("Spinel", "PU", "Purple", GemMaterial.CZLavender));
		list.Add(new Tuple<string, string, string, GemMaterial>("Spinel", "PU", "Purple", GemMaterial.CZLavender));
		list.Add(new Tuple<string, string, string, GemMaterial>("Spinel", "RE", "Red", GemMaterial.Ruby));
		list.Add(new Tuple<string, string, string, GemMaterial>("Tanzanite", "BL", "Blue", GemMaterial.Tanzanite));
		list.Add(new Tuple<string, string, string, GemMaterial>("Tourmaline", "PI", "Pink", GemMaterial.TourmalinePink));
		list.Add(new Tuple<string, string, string, GemMaterial>("Tourmaline", "GR", "Green", GemMaterial.TourmalineGreen));
		list.Add(new Tuple<string, string, string, GemMaterial>("Tourmaline", "RE", "Red", GemMaterial.TourmalinePink));
		list.Add(new Tuple<string, string, string, GemMaterial>("Tourmaline", "BL", "Blue (Indicolite)", GemMaterial.TourmalineBlueParaiba));
		list.Add(new Tuple<string, string, string, GemMaterial>("Tourmaline", "YE", "Yellow", GemMaterial.TourmalineGolden));
		list.Add(new Tuple<string, string, string, GemMaterial>("Zircon", "BL", "Blue", GemMaterial.ZirconBlue));
		list.Add(new Tuple<string, string, string, GemMaterial>("Zircon", "RE", "Red", GemMaterial.GarnetBrazilian));
		Tuple<string, string, string, GemMaterial> stullerGemstoneColor = list.FirstOrDefault((Tuple<string, string, string, GemMaterial> _) => _.Item1 == gemstone.StoneType && _.Item2 == gemstone.Color);
		if (stullerGemstoneColor == null)
		{
			switch (gemstone.Color)
			{
			case "BL":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.SapphireBlue);
				break;
			case "PU":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.SapphirePurple);
				break;
			case "CO":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.GarnetColorChange);
				break;
			case "GR":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.DiamondDarkGreen);
				break;
			case "OR":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.SapphireOrange);
				break;
			case "CL":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.Diamond);
				break;
			case "PI":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.DiamondPink);
				break;
			case "WH":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.Diamond);
				break;
			case "RE":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.Ruby);
				break;
			case "YE":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.DiamondYellow);
				break;
			case "BR":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.TourmalineEarth);
				break;
			case "DE":
				stullerGemstoneColor = new Tuple<string, string, string, GemMaterial>(gemstone.StoneType, gemstone.Color, "", GemMaterial.GarnetTsavorite);
				break;
			default:
				stullerGemstoneColor = list.FirstOrDefault((Tuple<string, string, string, GemMaterial> _) => _.Item1 == gemstone.StoneType);
				break;
			}
		}
		MaterialOptionItem materialOptionItem = source.FirstOrDefault((MaterialOptionItem _) => _.GemMaterial == stullerGemstoneColor.Item4);
		if (materialOptionItem != null)
		{
			return materialOptionItem;
		}
		return result;
	}

	private void InitializeCommands()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		AssignStoneCommand = new RelayCommand<Diamond>((Action<Diamond>)AssignStone, false);
		DeleteStoneCommand = new RelayCommand<Diamond>((Action<Diamond>)DeleteStone, false);
		ClearAssignedStoneCommand = new RelayCommand((Action)ClearAssignedStone, false);
	}
}
