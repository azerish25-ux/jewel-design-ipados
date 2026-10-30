using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DevExpress.Xpf.LayoutControl;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Model;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Utilities;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;
using Microsoft.Win32;

namespace GV.CounterSketch.Studio.ViewModel;

public class TileViewViewModelBase : ViewModelBase
{
	private static ColorPickerViewModel _colorPickerViewModel = new ColorPickerViewModel();

	private static bool _isInEditMode;

	private static List<TileItem> _originalTiles;

	private bool _isInitializing;

	private ObservableCollection<TileItem> _availableTiles;

	private TileItem _selectedTile;

	private ObservableCollection<TileItem> _tiles;

	private bool _isTemplateOne;

	private bool _isTemplateTwo;

	private bool _isTemplateThree;

	private bool _isLightTheme;

	private bool _isTechTheme;

	private Color _tileBackgroundColor;

	private Collection<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> _packageAttributes;

	private GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute _selectedAttribute;

	public virtual string PathToLayoutFile { get; } = Path.Combine(UIHelper.CSDataRootPath, "TileLayout.json");

	public ColorPickerViewModel ColorPickerViewModel
	{
		get
		{
			return _colorPickerViewModel;
		}
		set
		{
			_colorPickerViewModel = value;
			((ObservableObject)this).RaisePropertyChanged("ColorPickerViewModel");
		}
	}

	public Color TileBackgroundColor
	{
		get
		{
			return _tileBackgroundColor;
		}
		set
		{
			_tileBackgroundColor = value;
			((ObservableObject)this).RaisePropertyChanged("TileBackgroundColor");
		}
	}

	public bool IsInEditMode
	{
		get
		{
			return _isInEditMode;
		}
		set
		{
			_isInEditMode = value;
			((ObservableObject)this).RaisePropertyChanged("IsInEditMode");
			if (((ViewModelBase)this).IsInDesignMode || _isInitializing)
			{
				return;
			}
			if (!value)
			{
				LoadLayout();
			}
			UIHelper.AllowTileNavigation = !value;
			if (value)
			{
				Tiles?.ToList().ForEach(delegate(TileItem _)
				{
					_.CheckIsHitTestVisible();
				});
			}
		}
	}

	public bool IsTemplateOne
	{
		get
		{
			return _isTemplateOne;
		}
		set
		{
			if (value && !_isInitializing)
			{
				SaveLayout(stayInEditMode: true);
			}
			_isTemplateOne = value;
			((ObservableObject)this).RaisePropertyChanged("IsTemplateOne");
			if (!((ViewModelBase)this).IsInDesignMode && !_isInitializing && value)
			{
				IsTemplateTwo = false;
				IsTemplateThree = false;
				SaveThemeSettings();
				LoadLayout();
			}
		}
	}

	public bool IsTemplateTwo
	{
		get
		{
			return _isTemplateTwo;
		}
		set
		{
			if (value && !_isInitializing)
			{
				SaveLayout(stayInEditMode: true);
			}
			_isTemplateTwo = value;
			((ObservableObject)this).RaisePropertyChanged("IsTemplateTwo");
			if (!((ViewModelBase)this).IsInDesignMode && !_isInitializing && value)
			{
				IsTemplateOne = false;
				IsTemplateThree = false;
				SaveThemeSettings();
				LoadLayout();
			}
		}
	}

	public bool IsTemplateThree
	{
		get
		{
			return _isTemplateThree;
		}
		set
		{
			if (value && !_isInitializing)
			{
				SaveLayout(stayInEditMode: true);
			}
			_isTemplateThree = value;
			((ObservableObject)this).RaisePropertyChanged("IsTemplateThree");
			if (!((ViewModelBase)this).IsInDesignMode && !_isInitializing && value)
			{
				IsTemplateOne = false;
				IsTemplateTwo = false;
				SaveThemeSettings();
				LoadLayout();
			}
		}
	}

	public RelayCommand CommandAddCustomTile { get; set; }

	public RelayCommand<Tile> CommandSetTileSizeMedium { get; set; }

	public RelayCommand<Tile> CommandSetTileSizeLarge { get; set; }

	public RelayCommand<Tile> CommandSetTileSizeSmall { get; set; }

	public RelayCommand<Tile> CommandSetTileSizeWide { get; set; }

	public RelayCommand CommandResetLayout { get; set; }

	public RelayCommand CommandLoadLayout { get; set; }

	public RelayCommand CommandSaveLayout { get; set; }

	public RelayCommand CommandCancelLayout { get; set; }

	public RelayCommand<TileItem> CommandSelectTile { get; set; }

	public RelayCommand<Tile> CommandSetTileSize { get; set; }

	public TileItem SelectedTile
	{
		get
		{
			return _selectedTile;
		}
		set
		{
			_selectedTile = value;
			((ObservableObject)this).RaisePropertyChanged("SelectedTile");
		}
	}

	public ObservableCollection<TileItem> Tiles
	{
		get
		{
			return _tiles ?? (_tiles = new ObservableCollection<TileItem>());
		}
		set
		{
			_tiles = value;
			((ObservableObject)this).RaisePropertyChanged("Tiles");
		}
	}

	public RelayCommand<TileItem> CommandEditTileImage { get; set; }

	public RelayCommand<TileItem> CommandEditTileURL { get; set; }

	public RelayCommand<TileItem> CommandRemoveTile { get; set; }

	public RelayCommand<TileItem> CommandAddTile { get; set; }

	public ObservableCollection<TileItem> AvailableTiles
	{
		get
		{
			return _availableTiles;
		}
		set
		{
			_availableTiles = value;
			((ObservableObject)this).RaisePropertyChanged("AvailableTiles");
		}
	}

	public bool IsLightTheme
	{
		get
		{
			return _isLightTheme;
		}
		set
		{
			_isLightTheme = value;
			((ObservableObject)this).RaisePropertyChanged("IsLightTheme");
		}
	}

	public bool IsTechTheme
	{
		get
		{
			return _isTechTheme;
		}
		set
		{
			_isTechTheme = value;
			((ObservableObject)this).RaisePropertyChanged("IsTechTheme");
		}
	}

	public RelayCommand<TileItem> CommandSetShowcaseTile { get; set; }

	public RelayCommand CommandAddShowcaseTile { get; set; }

	public RelayCommand CommandAddCustomURLTile { get; set; }

	public Collection<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> PackageAttributes
	{
		get
		{
			return _packageAttributes;
		}
		set
		{
			_packageAttributes = value;
			((ObservableObject)this).RaisePropertyChanged("PackageAttributes");
		}
	}

	public TileViewViewModelBase()
	{
		_isInitializing = true;
		if (((ViewModelBase)this).IsInDesignMode)
		{
			IsTemplateTwo = true;
		}
		else
		{
			InitializeCommands();
			ColorPickerViewModel.SelectedColorChanging += ColorPickerViewModel_SelectedColorChanging;
		}
		_isInitializing = false;
	}

	public void EditLayout()
	{
		_originalTiles = new List<TileItem>(Tiles.Select((TileItem _) => (TileItem)_.Clone()));
		IsInEditMode = true;
	}

	public virtual List<TileItem> GetDefaultTiles()
	{
		return new List<TileItem>();
	}

	public void Populate()
	{
		PackageAttributes = SearchHelper.GetPackageAttributes();
		LoadLayout();
		if (Tiles != null && Tiles.Any())
		{
			SelectedTile = Tiles[0];
		}
		if (((ViewModelBase)this).IsInDesignMode)
		{
			IsInEditMode = true;
			if (Tiles != null)
			{
				AvailableTiles = new ObservableCollection<TileItem>(Tiles.Take(3));
			}
		}
		LoadThemeSettings();
	}

	protected void InitializeCommands()
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		CommandSelectTile = new RelayCommand<TileItem>((Action<TileItem>)SelectTile, false);
		CommandSetTileSizeSmall = new RelayCommand<Tile>((Action<Tile>)SetTileSizeSmall, false);
		CommandSetTileSizeMedium = new RelayCommand<Tile>((Action<Tile>)SetTileSizeMedium, false);
		CommandSetTileSizeWide = new RelayCommand<Tile>((Action<Tile>)SetTileSizeWide, false);
		CommandSetTileSizeLarge = new RelayCommand<Tile>((Action<Tile>)SetTileSizeLarge, false);
		CommandCancelLayout = new RelayCommand((Action)CancelLayout, false);
		CommandSaveLayout = new RelayCommand((Action)SaveLayout, false);
		CommandResetLayout = new RelayCommand((Action)ResetLayout, false);
		CommandAddTile = new RelayCommand<TileItem>((Action<TileItem>)AddTile, false);
		CommandRemoveTile = new RelayCommand<TileItem>((Action<TileItem>)RemoveTile, false);
		CommandEditTileImage = new RelayCommand<TileItem>((Action<TileItem>)EditTileImage, false);
		CommandAddCustomTile = new RelayCommand((Action)AddCustomTile, false);
		CommandSetShowcaseTile = new RelayCommand<TileItem>((Action<TileItem>)SetShowcaseTile, false);
		CommandEditTileURL = new RelayCommand<TileItem>((Action<TileItem>)EditTileURL, false);
	}

	private void AddCustomTile()
	{
		int id = Tiles.Count + AvailableTiles.Count + 999 + 1;
		TileItem item = new TileItem
		{
			Id = id,
			TileCommandType = TileCommandType.CustomTile,
			Size = (TileSize)3,
			ParentViewModel = this
		};
		Tiles.Add(item);
	}

	private void AddTile(TileItem tile)
	{
		tile.CustomHeight = 0.0;
		tile.CustomWidth = 0.0;
		tile.Size = (TileSize)2;
		tile.FixCustomSize();
		AvailableTiles.Remove(tile);
		Tiles.Add(tile);
	}

	private static bool AllowCommandExecution(object arg)
	{
		return UIHelper.AllowTileNavigation;
	}

	private void CancelLayout()
	{
		Tiles = new ObservableCollection<TileItem>(_originalTiles);
		SaveLayout();
	}

	private void ColorPickerViewModel_SelectedColorChanging(object sender, EventArgs e)
	{
		TileBackgroundColor = ColorPickerViewModel.SelectedColor;
		SaveThemeSettings();
	}

	private void EditTileImage(TileItem obj)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Filter = "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff"
		};
		if (openFileDialog.ShowDialog() == true)
		{
			string fileName = openFileDialog.FileName;
			obj.ImagePath = fileName;
		}
	}

	private void EditTileURL(TileItem tile)
	{
		string customURL = tile.CustomURL;
		string question = "Specify the URL for this Tile:";
		WindowMessage windowMessage = new WindowMessage(WindowMessage.WindowType.QuestionAnswer)
		{
			Title = "Custom Link Tile",
			Question = question,
			Topmost = true,
			Owner = Application.Current.MainWindow,
			Answer = tile.CustomURL
		};
		windowMessage.SetOKText(Strings.Save);
		windowMessage.ShowDialog();
		if (windowMessage.IsAccepted == true)
		{
			tile.CustomURL = windowMessage.Answer;
		}
		else
		{
			tile.CustomURL = customURL;
		}
	}

	private void LoadLayout()
	{
		LoadThemeSettings();
		string pathToLayoutFile = PathToLayoutFile;
		if (!File.Exists(pathToLayoutFile))
		{
			GlobalErrorHandler.WriteErrorToEventLog("Missing Tile Layout file " + pathToLayoutFile + ", Restoring from Default Tile Layout.");
			string fileName = Path.GetFileName(pathToLayoutFile);
			if (fileName != null)
			{
				fileName = fileName.Replace("2.json", "1.json");
				fileName = fileName.Replace("3.json", "1.json");
				File.Copy(Path.Combine(UIHelper.CSRootPath, "Layouts\\", fileName), pathToLayoutFile, overwrite: true);
			}
		}
		TileLayoutItemStore tileLayoutItemStore = new SerializeHelper<TileLayoutItemStore>().ReadJSONSettings(pathToLayoutFile);
		if (tileLayoutItemStore?.Tiles != null)
		{
			Collection<TileItem> tiles = new Collection<TileItem>(tileLayoutItemStore.Tiles);
			Collection<TileItem> collection = new Collection<TileItem>(tileLayoutItemStore.AvailableTiles);
			IEnumerable<TileItem> first = GetDefaultTiles().Except(tiles, new TileItemComparer());
			IEnumerable<TileItem> second = GetDefaultTiles().Except(collection, new TileItemComparer());
			first.Intersect(second, new TileItemComparer()).ToList().ForEach(delegate(TileItem _)
			{
				tiles.Add(_);
			});
			IEnumerable<int> tilesIdsNoLongerUsed = from _ in tiles.Where((TileItem _) => _.Id < 999).Except(GetDefaultTiles(), new TileItemComparer())
				select _.Id;
			IEnumerable<int> availableTilesIdsNoLongerUsed = from _ in collection.Where((TileItem _) => _.Id < 999).Except(GetDefaultTiles(), new TileItemComparer())
				select _.Id;
			tiles.ToList().ForEach(delegate(TileItem _)
			{
				_.FixCustomSize();
			});
			collection.ToList().ForEach(delegate(TileItem _)
			{
				_.FixCustomSize();
			});
			TileItem tileItem = tiles.FirstOrDefault((TileItem _) => _.Id == 19);
			if (tileItem != null)
			{
				if (UIHelper.CounterSketchOptions.HasEverAndEver || UIHelper.CounterSketchOptions.PrivateLabel)
				{
					tileItem.Title = string.Empty;
				}
				else
				{
					tileItem.Title = Strings.CuratedCollection;
				}
			}
			tiles.ForEach(delegate(TileItem t)
			{
				t.Title = TranslationHelper.GetTranslatedString(t.Title);
			});
			Tiles = new ObservableCollection<TileItem>(tiles.Where((TileItem _) => !tilesIdsNoLongerUsed.Contains(_.Id)));
			AvailableTiles = new ObservableCollection<TileItem>(collection.Where((TileItem _) => !availableTilesIdsNoLongerUsed.Contains(_.Id)));
			WireupTileCommands();
		}
		else
		{
			GlobalErrorHandler.WriteInfoToEventLog("Saved Layout File Invalid.");
			ResetLayout();
		}
	}

	private void LoadThemeSettings()
	{
		_isInitializing = true;
		IsLightTheme = UIHelper.CounterSketchOptions.TileIsLightTheme;
		IsTechTheme = false;
		if (this is TileViewDesignsViewModel)
		{
			if (UIHelper.CounterSketchOptions.IsDesignTemplateOne && UIHelper.CounterSketchOptions.IsDesignTemplateTwo && UIHelper.CounterSketchOptions.IsDesignTemplateThree)
			{
				UIHelper.CounterSketchOptions.IsDesignTemplateTwo = false;
				UIHelper.CounterSketchOptions.IsDesignTemplateThree = false;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			if (!UIHelper.CounterSketchOptions.IsDesignTemplateOne && !UIHelper.CounterSketchOptions.IsDesignTemplateTwo && !UIHelper.CounterSketchOptions.IsDesignTemplateThree)
			{
				UIHelper.CounterSketchOptions.IsDesignTemplateOne = true;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			IsTemplateOne = UIHelper.CounterSketchOptions.IsDesignTemplateOne;
			IsTemplateTwo = UIHelper.CounterSketchOptions.IsDesignTemplateTwo;
			IsTemplateThree = UIHelper.CounterSketchOptions.IsDesignTemplateThree;
		}
		else
		{
			if (UIHelper.CounterSketchOptions.IsPartsTemplateOne && UIHelper.CounterSketchOptions.IsPartsTemplateTwo && UIHelper.CounterSketchOptions.IsPartsTemplateThree)
			{
				UIHelper.CounterSketchOptions.IsPartsTemplateTwo = false;
				UIHelper.CounterSketchOptions.IsPartsTemplateThree = false;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			if (!UIHelper.CounterSketchOptions.IsPartsTemplateOne && !UIHelper.CounterSketchOptions.IsPartsTemplateTwo && !UIHelper.CounterSketchOptions.IsPartsTemplateThree)
			{
				UIHelper.CounterSketchOptions.IsPartsTemplateOne = true;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			IsTemplateOne = UIHelper.CounterSketchOptions.IsPartsTemplateOne;
			IsTemplateTwo = UIHelper.CounterSketchOptions.IsPartsTemplateTwo;
			IsTemplateThree = UIHelper.CounterSketchOptions.IsPartsTemplateThree;
		}
		TileBackgroundColor = UIHelper.CounterSketchOptions.TileBackgroundColor;
		ColorPickerViewModel.IsInitializing = true;
		ColorPickerViewModel.BrightnessAdjustment = UIHelper.CounterSketchOptions.TileBackgroundBrightness;
		ColorPickerViewModel.SaturationAdjustment = UIHelper.CounterSketchOptions.TileBackgroundSaturation;
		ColorPickerViewModel.IsInitializing = false;
		_isInitializing = false;
	}

	private void RemoveTile(TileItem tile)
	{
		Tiles.Remove(tile);
		if (tile.TileCommandType != TileCommandType.CustomTile)
		{
			tile.Size = (TileSize)2;
			AvailableTiles.Add(tile);
		}
	}

	private void ResetColors()
	{
		IsLightTheme = false;
		IsTechTheme = false;
		ColorPickerViewModel.SelectedColor = Colors.DimGray;
		TileBackgroundColor = Colors.DimGray;
		ColorPickerViewModel.BrightnessAdjustment = 0.0;
		ColorPickerViewModel.SaturationAdjustment = 0.5;
	}

	private void ResetLayout()
	{
		GlobalErrorHandler.WriteInfoToEventLog("Resetting Tile Layout and Colors.");
		ResetColors();
		string pathToLayoutFile = PathToLayoutFile;
		File.SetAttributes(pathToLayoutFile, FileAttributes.Normal);
		File.Delete(pathToLayoutFile);
		LoadLayout();
		SaveLayout(stayInEditMode: true);
	}

	private void SaveLayout()
	{
		SaveLayout(stayInEditMode: false);
	}

	private void SaveLayout(bool stayInEditMode = false)
	{
		SaveThemeSettings();
		new SerializeHelper<TileLayoutItemStore>().WriteJSONSettings(classToSerialze: new TileLayoutItemStore
		{
			Tiles = Tiles.ToList(),
			AvailableTiles = AvailableTiles.ToList()
		}, path: PathToLayoutFile);
		if (!stayInEditMode)
		{
			IsInEditMode = false;
		}
	}

	private void SaveThemeSettings()
	{
		UIHelper.CounterSketchOptions.TileIsLightTheme = IsLightTheme;
		UIHelper.CounterSketchOptions.TileIsTechTheme = false;
		if (this is TileViewDesignsViewModel)
		{
			UIHelper.CounterSketchOptions.IsDesignTemplateOne = IsTemplateOne;
			UIHelper.CounterSketchOptions.IsDesignTemplateTwo = IsTemplateTwo;
			UIHelper.CounterSketchOptions.IsDesignTemplateThree = IsTemplateThree;
		}
		else
		{
			UIHelper.CounterSketchOptions.IsPartsTemplateOne = IsTemplateOne;
			UIHelper.CounterSketchOptions.IsPartsTemplateTwo = IsTemplateTwo;
			UIHelper.CounterSketchOptions.IsPartsTemplateThree = IsTemplateThree;
		}
		UIHelper.CounterSketchOptions.TileBackgroundColor = TileBackgroundColor;
		UIHelper.CounterSketchOptions.TileBackgroundBrightness = ColorPickerViewModel.BrightnessAdjustment;
		UIHelper.CounterSketchOptions.TileBackgroundSaturation = ColorPickerViewModel.SaturationAdjustment;
		UIHelper.CounterSketchOptions.WriteSettings();
	}

	private void SelectTile(TileItem tile)
	{
		SelectedTile = tile;
	}

	private void SetShowcaseTile(TileItem tile)
	{
		string customURL = tile.CustomURL;
		tile.CustomURL = UIHelper.CounterSketchOptions.JewelerShowcaseURL;
		string question = "Verify the URL for Showcase:";
		WindowMessage windowMessage = new WindowMessage(WindowMessage.WindowType.QuestionAnswer)
		{
			Title = "Showcase Tile",
			Question = question,
			Topmost = true,
			Owner = Application.Current.MainWindow,
			Answer = tile.CustomURL
		};
		windowMessage.SetOKText(Strings.Save);
		windowMessage.ShowDialog();
		if (windowMessage.IsAccepted == true)
		{
			tile.CustomURL = windowMessage.Answer;
		}
		else
		{
			tile.CustomURL = customURL;
		}
	}

	private void SetTileSizeLarge(Tile selectedTile)
	{
		if (selectedTile != null)
		{
			selectedTile.Size = (TileSize)3;
		}
	}

	private void SetTileSizeMedium(Tile selectedTile)
	{
		if (selectedTile != null)
		{
			selectedTile.Size = (TileSize)1;
		}
	}

	private void SetTileSizeSmall(Tile selectedTile)
	{
		if (selectedTile != null)
		{
			selectedTile.Size = (TileSize)0;
		}
	}

	private void SetTileSizeWide(Tile selectedTile)
	{
		if (selectedTile != null)
		{
			selectedTile.Size = (TileSize)2;
		}
	}

	private static void WireupTile(TileViewViewModelBase parentViewModel, List<TileItem> defaultTiles, TileItem tile)
	{
		tile.ParentViewModel = parentViewModel;
		if (tile.TileCommandType == TileCommandType.CustomTile)
		{
			tile.SelectedAttribute = parentViewModel.PackageAttributes.FirstOrDefault((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute _) => _.AttributeId == tile.SelectedAttributeId);
			if (tile.SelectedAttributeId > 0)
			{
				tile.TileCommandObject = new RelayCommand<object>((Action<object>)delegate(object category)
				{
					try
					{
						GlobalErrorHandler.WriteInfoToEventLog($"Executing Search By Package Attribute: {category}", 2);
						ViewModelLocator.Current.TileSearchMainVM.SetSelectedSearchTile((TileSearchCategory)category);
					}
					catch (Exception arg)
					{
						GlobalErrorHandler.WriteErrorToEventLog($"Error Executing Custom Tile Command: {arg}");
					}
				}, (Func<object, bool>)AllowCommandExecution, false);
				tile.TileCommandObjectInput = new TileSearchCategory
				{
					Id = 15,
					Title = tile.SelectedAttribute?.AttributeName,
					TileImagePath = tile.ImagePath,
					PackageAttributeId = tile.SelectedAttributeId
				};
				return;
			}
			if (tile.CustomURL != null)
			{
				tile.TileCommandObject = new RelayCommand<object>((Action<object>)delegate(object url)
				{
					try
					{
						GlobalErrorHandler.WriteInfoToEventLog("Executing Custom Tile Command: " + url, 2);
						Process.Start((string)url);
					}
					catch (Exception arg)
					{
						GlobalErrorHandler.WriteErrorToEventLog($"Error Executing Custom Tile Command: {arg}");
					}
				}, (Func<object, bool>)AllowCommandExecution, false);
				tile.TileCommandObjectInput = tile.CustomURL;
				return;
			}
		}
		TileItem tileItem = defaultTiles.FirstOrDefault((TileItem d) => d.Id == tile.Id);
		if (tileItem == null)
		{
			return;
		}
		tile.ParentViewModel = tileItem.ParentViewModel;
		tile.ImagePath = tileItem.ImagePath;
		switch (tileItem.TileCommandType)
		{
		case TileCommandType.LaunchStyleQuiz:
			tile.TileCommandObject = new RelayCommand<object>((Action<object>)delegate
			{
				ViewModelLocator.Current.MainVM.OpenStyleQuizWindow();
			}, (Func<object, bool>)AllowCommandExecution, false);
			break;
		case TileCommandType.SearchByTileSearchCategory:
			tile.TileCommandObject = new RelayCommand<object>((Action<object>)delegate(object category)
			{
				GlobalErrorHandler.WriteInfoToEventLog("Executing Search By Tile Category: " + ((TileSearchCategory)category).Title, 2);
				ViewModelLocator.Current.TileSearchMainVM.SetSelectedSearchTile((TileSearchCategory)category);
			}, (Func<object, bool>)AllowCommandExecution, false);
			tile.TileCommandObjectInput = tileItem.TileCommandObjectInput;
			break;
		case TileCommandType.SearchByFreehandCategory:
			tile.TileCommandObject = new RelayCommand<object>((Action<object>)delegate(object category)
			{
				ViewModelLocator.Current.TileSearchMainVM.SetSelectedPartTile((TileSearchCategory)category);
			}, (Func<object, bool>)AllowCommandExecution, false);
			tile.TileCommandObjectInput = tileItem.TileCommandObjectInput;
			break;
		case TileCommandType.LaunchFreehand:
			tile.TileCommandObject = new RelayCommand<object>((Action<object>)delegate
			{
				ViewModelLocator.Current.CustomizePartsListVM.AddPart();
			}, (Func<object, bool>)AllowCommandExecution, false);
			break;
		case TileCommandType.SearchByUserParts:
			tile.TileCommandObject = new RelayCommand<object>((Action<object>)delegate
			{
				ViewModelLocator.Current.TileSearchMainVM.SetSelectedUserPartTile();
			}, (Func<object, bool>)AllowCommandExecution, false);
			break;
		case TileCommandType.LaunchStoneShopper:
			tile.TileCommandObject = new RelayCommand<object>((Action<object>)delegate
			{
				ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.ShowStoneShopper();
			}, (Func<object, bool>)AllowCommandExecution, false);
			break;
		case TileCommandType.LaunchGemShopper:
			tile.TileCommandObject = new RelayCommand<object>((Action<object>)delegate
			{
				ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.ShowGemShopper();
			}, (Func<object, bool>)AllowCommandExecution, false);
			break;
		case TileCommandType.InvokePart:
		case TileCommandType.CustomTile:
			break;
		}
	}

	private void WireupTileCommands()
	{
		if (!((ViewModelBase)this).IsInDesignMode)
		{
			List<TileItem> defaultTiles = GetDefaultTiles();
			AvailableTiles.ForEach(delegate(TileItem _)
			{
				WireupTile(this, defaultTiles, _);
			});
			Tiles.ForEach(delegate(TileItem _)
			{
				WireupTile(this, defaultTiles, _);
			});
		}
	}
}
